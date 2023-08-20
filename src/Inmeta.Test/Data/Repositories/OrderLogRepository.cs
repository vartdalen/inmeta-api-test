using Microsoft.EntityFrameworkCore;
using Inmeta.Test.Data.Abstractions.Repositories;
using Inmeta.Test.Data.Collections;
using Inmeta.Test.Data.Extensions;
using Inmeta.Test.Data.Models.Entities;
using Inmeta.Test.Data.Models.Enums;
using Inmeta.Test.Data.Models.Indexes;
using Inmeta.Test.Data.Models.Requests;

namespace Inmeta.Test.Data.Repositories
{
    public class OrderLogRepository : IOrderLogRepository
    {
        private readonly OrdersDbContext _context;

        public OrderLogRepository(
			OrdersDbContext context
        )
        {
            _context = context;
        }

        public async Task<OrderLog> Create(OrderLogRequest request)
        {
            var result = await _context.OrderLogs.AddAsync(request.OrderLog);
            await _context.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<OrderLog?> Read(int id)
        {
            var orderLog = await _context.OrderLogs.FindAsync(id);
            return orderLog;
        }

        public async Task<PagedList<OrderLog>> Read(
            int pageNumber,
            int pageSize,
            string? q
        )
        {
            var isOrderStatusSet = q!.TryParse<OrderStatus>(out var orderStatus);
			var isServiceSet = q!.TryParse<Service>(out var service);
			var orderLogs = _context.OrderLogs.Conditional(
                !string.IsNullOrEmpty(q),
                x => x.Where(ol => ol.Message.Contains(q!))
            );

            return await PagedList<OrderLog>.Create(orderLogs, pageNumber, pageSize);
        }

        public async Task<PagedList<OrderLog>> Read(
            int pageNumber,
            int pageSize,
            CustomerId customerId
        )
        {
            var orderLogs = _context.OrderLogs
                .Include(ol => ol.Order)
                .Where(ol => ol.Order.CustomerId == customerId.Value);
            return await PagedList<OrderLog>.Create(orderLogs, pageNumber, pageSize);
        }

        public async Task<PagedList<OrderLog>> Read(
            int pageNumber,
            int pageSize,
            OrderId orderId
        )
        {
            var orderLogs = _context.OrderLogs.Where(ol => ol.OrderId == orderId.Value);
            return await PagedList<OrderLog>.Create(orderLogs, pageNumber, pageSize);
        }

        public async Task Update(OrderLogRequest request)
        {
            _context.OrderLogs.Update(request.OrderLog);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var orderLog = await _context.OrderLogs.FindAsync(id);
            if (orderLog is not null)
            {
                _context.OrderLogs.Remove(orderLog);
                await _context.SaveChangesAsync();
            }
        }
    }
}
