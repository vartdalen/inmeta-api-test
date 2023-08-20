using Inmeta.Test.Services.Models.Dtos.Addresses;
using Inmeta.Test.Services.Models.Dtos.Indexes;

namespace Inmeta.Test.Services.Abstractions
{
	public interface IAddressService :
        ICrudService<AddressHashId, CreateAddressDto, ReadAddressDto, UpdateAddressDto>,
        IPagedService<ReadAddressDto>
    {
    }
}
