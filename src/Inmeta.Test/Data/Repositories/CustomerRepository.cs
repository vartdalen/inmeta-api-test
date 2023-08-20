using Inmeta.Test.Data.Abstractions.Repositories;
using Inmeta.Test.Data.Collections;
using Inmeta.Test.Data.Extensions;
using Inmeta.Test.Data.Models.Entities;
using Inmeta.Test.Data.Models.Indexes;
using Inmeta.Test.Data.Models.Requests;
using Microsoft.EntityFrameworkCore;

namespace Inmeta.Test.Data.Repositories
{
	public class CustomerRepository : ICustomerRepository
    {
        private readonly OrdersDbContext _context;

        public CustomerRepository(OrdersDbContext context)
        {
            _context = context;
        }

        public async Task<Customer> Create(CustomerRequest request)
        {
            var result = await _context.Customers.AddAsync(request.Customer);
            await _context.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<Customer?> Read(int id)
        {
            return await _context.Customers.FindAsync(id);
        }

        public async Task<PagedList<Customer>> Read(
            int pageNumber,
            int pageSize,
            string? q
        )
        {
            var orders = _context.Customers.Conditional(
                !q.IsNullOrEmpty(),
                x => x.Where(c =>
                    c.Email.Contains(q!) ||
                    c.Phone == q! ||
                    c.Name.Contains(q!)
                )
            );
            return await PagedList<Customer>.Create(orders, pageNumber, pageSize);
        }

        public async Task<Customer?> Read(CustomerEmail customerEmail)
        {
            return await _context.Customers.SingleOrDefaultAsync(c => c.Email == customerEmail.Value);
        }

        public async Task Update(CustomerRequest request)
        {
            _context.Customers.Update(request.Customer);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var customer = await Read(id);
            if (customer is not null)
            {
                _context.Customers.Remove(customer);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> IsResourceOwner(CustomerId customerId, OrderId orderId)
        {
            return await _context.Orders.AnyAsync(o => o.CustomerId == customerId.Value && o.Id == orderId.Value);
        }
    }
}