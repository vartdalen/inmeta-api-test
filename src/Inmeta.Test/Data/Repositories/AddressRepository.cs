using Inmeta.Test.Data.Abstractions.Repositories;
using Inmeta.Test.Data.Collections;
using Inmeta.Test.Data.Extensions;
using Inmeta.Test.Data.Models.Entities;
using Inmeta.Test.Data.Models.Enums;
using Inmeta.Test.Data.Models.Requests;
using Microsoft.EntityFrameworkCore;

namespace Inmeta.Test.Data.Repositories
{
	public class AddressRepository : IAddressRepository
	{
        private readonly OrdersDbContext _context;

        public AddressRepository(OrdersDbContext context)
        {
            _context = context;
        }

        public async Task<Address> Create(AddressRequest request)
		{
			var existing = await Read(request);
			if (existing != null) { return existing; }
			var result = await _context.Addresses.AddAsync(request.Address);
			await _context.SaveChangesAsync();
			return result.Entity;
		}

		public async Task<Address?> Read(int id)
        {
            return await _context.Addresses.FindAsync(id);
        }

        public async Task<PagedList<Address>> Read(
            int pageNumber,
            int pageSize,
            string? q
        )
        {
			var isZipCodeSet = q!.TryParse<ZipCode>(out var zipCode);
			var isCountrySet = q!.TryParse<Country>(out var country);
			var orders = _context.Addresses.Conditional(
                !q.IsNullOrEmpty(),
                x => x.Where(a =>
                    a.StreetAddress.Contains(q!) ||
					(!string.IsNullOrEmpty(a.City) && a.City.Contains(q!)) ||
					isZipCodeSet && a.ZipCode == zipCode ||
					isCountrySet && a.Country == country
				)
            );
            return await PagedList<Address>.Create(orders, pageNumber, pageSize);
        }

		public async Task<Address?> Read(AddressRequest request)
		{
			return await _context.Addresses.FirstOrDefaultAsync(a =>
				a.StreetAddress == request.Address.StreetAddress &&
				a.City == request.Address.City &&
				a.ZipCode == request.Address.ZipCode &&
				a.Country == request.Address.Country
			);
		}

		public async Task Update(AddressRequest request)
        {
            _context.Addresses.Update(request.Address);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var customer = await Read(id);
            if (customer is not null)
            {
                _context.Addresses.Remove(customer);
                await _context.SaveChangesAsync();
            }
        }
    }
}