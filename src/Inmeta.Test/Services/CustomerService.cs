using HashidsNet;
using Mapster;
using Inmeta.Test.Data.Abstractions.Repositories;
using Inmeta.Test.Data.Collections;
using Inmeta.Test.Data.Models.Entities;
using Inmeta.Test.Data.Models.Indexes;
using Inmeta.Test.Data.Models.Requests;
using Inmeta.Test.Services.Abstractions;
using Inmeta.Test.Services.Models.Dtos.Customers;
using Inmeta.Test.Services.Models.Dtos.Indexes;

namespace Inmeta.Test.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _repository;
        private readonly IHashids _cHashids, _oHashids;

        public CustomerService(
            ICustomerRepository repository,
            IHashidsService hashidsService
        )
        {
            _repository = repository;
            _cHashids = hashidsService.Get(typeof(Customer));
            _oHashids = hashidsService.Get(typeof(Order));
        }

        public async Task<ReadCustomerDto> Create(CreateCustomerDto customerDto)
        {
            var request = new CustomerRequest { Customer = customerDto.Adapt<Customer>() };
            var customer = await _repository.Create(request);
            return customer.Adapt<ReadCustomerDto>();
        }

        public async Task<ReadCustomerDto> Read(CustomerHashId hashId)
        {
            if (!_cHashids.TryDecodeSingle(hashId.Value, out var id)) throw new KeyNotFoundException();
            var customer = await _repository.Read(id) ?? throw new KeyNotFoundException();
            return customer.Adapt<ReadCustomerDto>();
        }

        public async Task<PagedList<ReadCustomerDto>> Read(
            int pageNumber,
            int pageSize,
            string? q
        )
        {
            var customers = await _repository.Read(pageNumber, pageSize, q);
            return customers.Adapt<PagedList<ReadCustomerDto>>();
        }

        public async Task<ReadCustomerDto> Read(CustomerEmail customerEmail)
        {
            var customer = await _repository.Read(customerEmail) ?? throw new KeyNotFoundException();
            return customer.Adapt<ReadCustomerDto>();
        }

        public async Task Update(CustomerHashId hashId, UpdateCustomerDto customerDto)
        {
            if (!_cHashids.TryDecodeSingle(hashId.Value, out var id)) throw new KeyNotFoundException();
            var customer = await _repository.Read(id) ?? throw new KeyNotFoundException();
            var request = new CustomerRequest { Customer = customerDto.Adapt(customer) };
            await _repository.Update(request);
        }

        public async Task Delete(CustomerHashId hashId)
        {
            if (!_cHashids.TryDecodeSingle(hashId.Value, out var id)) throw new KeyNotFoundException();
            var customer = await _repository.Read(id) ?? throw new KeyNotFoundException();
            await _repository.Delete(customer.Id);
        }

        public async Task<bool> IsResourceOwner(CustomerHashId customerHashId, OrderHashId orderHashId)
        {
            if (!_cHashids.TryDecodeSingle(customerHashId.Value, out var customerId)) throw new KeyNotFoundException();
            if (!_oHashids.TryDecodeSingle(orderHashId.Value, out var orderId)) throw new KeyNotFoundException();
            return await _repository.IsResourceOwner(new CustomerId(customerId), new OrderId(orderId));
        }
    }
}
