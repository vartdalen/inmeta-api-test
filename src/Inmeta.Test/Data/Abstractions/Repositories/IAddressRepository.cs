using Inmeta.Test.Data.Models.Entities;
using Inmeta.Test.Data.Models.Requests;

namespace Inmeta.Test.Data.Abstractions.Repositories
{
	public interface IAddressRepository :
        ICrudRepository<int, Address, AddressRequest>,
        IPagedQueryRepository<Address>
    {
        Task<Address?> Read(AddressRequest request);
    }
}
