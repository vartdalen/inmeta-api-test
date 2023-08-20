using HashidsNet;
using Inmeta.Test.Data.Abstractions.Repositories;
using Inmeta.Test.Data.Collections;
using Inmeta.Test.Data.Models.Entities;
using Inmeta.Test.Data.Models.Requests;
using Inmeta.Test.Services.Abstractions;
using Inmeta.Test.Services.Models.Dtos.Addresses;
using Inmeta.Test.Services.Models.Dtos.Indexes;
using Mapster;

namespace Inmeta.Test.Services
{
	public class AddressService : IAddressService
    {
        private readonly IAddressRepository _repository;
        private readonly IHashids _aHashids;

        public AddressService(
            IAddressRepository repository,
            IHashidsService hashidsService
        )
        {
            _repository = repository;
            _aHashids = hashidsService.Get(typeof(Address));
        }

        public async Task<ReadAddressDto> Create(CreateAddressDto addressDto)
        {
            var request = new AddressRequest { Address = addressDto.Adapt<Address>() };
            var address = await _repository.Create(request);
            return address.Adapt<ReadAddressDto>();
        }

        public async Task<ReadAddressDto> Read(AddressHashId hashId)
        {
            if (!_aHashids.TryDecodeSingle(hashId.Value, out var id)) throw new KeyNotFoundException();
            var address = await _repository.Read(id) ?? throw new KeyNotFoundException();
            return address.Adapt<ReadAddressDto>();
        }

        public async Task<PagedList<ReadAddressDto>> Read(
            int pageNumber,
            int pageSize,
            string? q
        )
        {
            var addresss = await _repository.Read(pageNumber, pageSize, q);
            return addresss.Adapt<PagedList<ReadAddressDto>>();
        }

        public async Task Update(AddressHashId hashId, UpdateAddressDto addressDto)
        {
            if (!_aHashids.TryDecodeSingle(hashId.Value, out var id)) throw new KeyNotFoundException();
            var address = await _repository.Read(id) ?? throw new KeyNotFoundException();
            var request = new AddressRequest { Address = addressDto.Adapt(address) };
            await _repository.Update(request);
        }

        public async Task Delete(AddressHashId hashId)
        {
            if (!_aHashids.TryDecodeSingle(hashId.Value, out var id)) throw new KeyNotFoundException();
            var address = await _repository.Read(id) ?? throw new KeyNotFoundException();
            await _repository.Delete(address.Id);
        }
    }
}
