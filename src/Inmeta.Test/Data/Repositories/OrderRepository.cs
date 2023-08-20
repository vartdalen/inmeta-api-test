using Inmeta.Test.Data.Abstractions.Repositories;
using Inmeta.Test.Data.Collections;
using Inmeta.Test.Data.Extensions;
using Inmeta.Test.Data.Models.Entities;
using Inmeta.Test.Data.Models.Enums;
using Inmeta.Test.Data.Models.Indexes;
using Inmeta.Test.Data.Models.Requests;
using Microsoft.EntityFrameworkCore;

namespace Inmeta.Test.Data.Repositories
{
	public class OrderRepository : IOrderRepository
    {
        private readonly OrdersDbContext _context;
        private readonly IAddressRepository _addressRepository;

        public OrderRepository(
            OrdersDbContext context,
			IAddressRepository addressRepository
		)
        {
            _context = context;
            _addressRepository = addressRepository;
		}

        public async Task<Order> Create(OrderRequest request)
		{
			await EnsureUniqueAddresses(request);
			var result = await _context.Orders.AddAsync(request.Order);
			await _context.SaveChangesAsync();
			return result.Entity;
		}

		public async Task<Order?> Read(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order is not null)
            {
                await _context.Entry(order)
                    .Reference(o => o.FromAddress)
                    .LoadAsync();
            }
			if (order is not null)
			{
				await _context.Entry(order)
					.Reference(o => o.ToAddress)
					.LoadAsync();
			}
            return order;
		}

        public async Task<PagedList<Order>> Read(
            int pageNumber,
            int pageSize,
            string? q
        )
        {
            var isOrderStatusSet = q!.TryParse<OrderStatus>(out var orderStatus);
			var isServiceSet = q!.TryParse<Service>(out var service);
			if (!string.IsNullOrEmpty(q) && !isOrderStatusSet)
            {
                return PagedList<Order>.Empty();
            }
            var orders = _context.Orders.Conditional(
                !string.IsNullOrEmpty(q),
                x => x.Where(o =>
                    isOrderStatusSet && o.OrderStatus == orderStatus ||
					isServiceSet && o.Service == service
				)
            )
			.Include(o => o.FromAddress)
			.Include(o => o.ToAddress);
            return await PagedList<Order>.Create(orders, pageNumber, pageSize);
        }

        public async Task<PagedList<Order>> Read(
            int pageNumber,
            int pageSize,
            CustomerId customerId
        )
        {
            var orders = _context.Orders
                .Where(o => o.CustomerId == customerId.Value)
				.Include(o => o.FromAddress)
				.Include(o => o.ToAddress);
            return await PagedList<Order>.Create(orders, pageNumber, pageSize);
        }

        public async Task Update(OrderRequest request)
        {
            await EnsureUniqueAddresses(request);
            _context.Orders.Update(request.Order);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order is not null)
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }
        }

		private async Task EnsureUniqueAddresses(OrderRequest request)
		{
			request.Order.FromAddress = await _addressRepository.Read(new AddressRequest(request.Order.FromAddress)) ?? request.Order.FromAddress;
			request.Order.ToAddress = await _addressRepository.Read(new AddressRequest(request.Order.ToAddress)) ?? request.Order.ToAddress;
		}
	}
}