using HashidsNet;
using Mapster;
using Inmeta.Test.Data.Abstractions.Repositories;
using Inmeta.Test.Data.Collections;
using Inmeta.Test.Data.Models.Entities;
using Inmeta.Test.Data.Models.Indexes;
using Inmeta.Test.Data.Models.Requests;
using Inmeta.Test.Services.Abstractions;
using Inmeta.Test.Services.Models.Dtos.Indexes;
using Inmeta.Test.Services.Models.Dtos.Orders;

namespace Inmeta.Test.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _or;
        private readonly IOrderLogRepository _olr;
        private readonly IHashids _oHashids, _cHashids;

        public OrderService(
            IOrderRepository or,
            IOrderLogRepository olr,
            IHashidsService hashidsService
        )
        {
            _or = or;
            _olr = olr;
            _oHashids = hashidsService.Get(typeof(Order));
            _cHashids = hashidsService.Get(typeof(Customer));
        }

        public async Task<ReadOrderDto> Create(CreateOrderDto orderDto)
        {
            var request = new OrderRequest { Order = orderDto.Adapt<Order>() };
            var order = await _or.Create(request);
            return order.Adapt<ReadOrderDto>();
        }

        public async Task<ReadOrderDto> Read(OrderHashId hashId)
        {
            if (!_oHashids.TryDecodeSingle(hashId.Value, out var id)) throw new KeyNotFoundException();
            var order = await _or.Read(id) ?? throw new KeyNotFoundException();
            return order.Adapt<ReadOrderDto>();
        }

        public async Task<PagedList<ReadOrderDto>> Read(
            int pageNumber,
            int pageSize,
            string? q
        )
        {
            if (_cHashids.TryDecodeSingle(q, out var customerId)) {
                var customerOrders = await _or.Read(pageNumber, pageSize, new CustomerId(customerId));
                return customerOrders.Adapt<PagedList<ReadOrderDto>>();
            }
            var orders = await _or.Read(pageNumber, pageSize, q);
            return orders.Adapt<PagedList<ReadOrderDto>>();
        }

        public async Task Update(OrderHashId hashId, UpdateOrderDto orderDto)
        {
            if (!_oHashids.TryDecodeSingle(hashId.Value, out var id)) throw new KeyNotFoundException();
            var order = await _or.Read(id) ?? throw new KeyNotFoundException();
            if (order.OrderStatus == orderDto.OrderStatus) throw new InvalidOperationException();
			if (order.Service == orderDto.Service) throw new InvalidOperationException();
			var oRequest = new OrderRequest { Order = orderDto.Adapt(order) };
            await _or.Update(oRequest);
            
            var olRequest = new OrderLogRequest { OrderLog = orderDto.Adapt<OrderLog>() };
            olRequest.OrderLog.Order = order;
            await _olr.Create(olRequest);
        }

        public async Task Delete(OrderHashId hashId)
        {
            if (!_oHashids.TryDecodeSingle(hashId.Value, out var id)) throw new KeyNotFoundException();
            var order = await _or.Read(id) ?? throw new KeyNotFoundException();
            await _or.Delete(order.Id);
        }
    }
}
