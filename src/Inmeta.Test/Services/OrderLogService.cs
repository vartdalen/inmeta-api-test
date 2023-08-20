using HashidsNet;
using Mapster;
using Inmeta.Test.Data.Abstractions.Repositories;
using Inmeta.Test.Data.Collections;
using Inmeta.Test.Data.Models.Entities;
using Inmeta.Test.Data.Models.Indexes;
using Inmeta.Test.Services.Abstractions;
using Inmeta.Test.Services.Models.Dtos.OrderLogs;

namespace Inmeta.Test.Services
{
    public class OrderLogService : IOrderLogService
    {
        private readonly IOrderLogRepository _repository;
        private readonly IHashids _cHashids, _oHashids;

        public OrderLogService(
            IOrderLogRepository repository,
            IHashidsService hashidsService
        )
        {
            _repository = repository;
            _cHashids = hashidsService.Get(typeof(Customer));
            _oHashids = hashidsService.Get(typeof(Order));
        }

        public async Task<PagedList<ReadOrderLogDto>> Read(
            int pageNumber,
            int pageSize,
            string? q
        )
        {
            if (_cHashids.TryDecodeSingle(q, out var customerId))
            {
                var orderLogsForCustomer = await _repository.Read(pageNumber, pageSize, new CustomerId(customerId));
                return orderLogsForCustomer.Adapt<PagedList<ReadOrderLogDto>>();
            }
            if (_oHashids.TryDecodeSingle(q, out var orderId)) {
                var orderLogsForOrder = await _repository.Read(pageNumber, pageSize, new OrderId(orderId));
                return orderLogsForOrder.Adapt<PagedList<ReadOrderLogDto>>();
            }
            var orderLogs = await _repository.Read(pageNumber, pageSize, q);
            return orderLogs.Adapt<PagedList<ReadOrderLogDto>>();
        }
    }
}
