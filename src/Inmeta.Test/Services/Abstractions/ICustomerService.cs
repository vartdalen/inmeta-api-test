using Inmeta.Test.Data.Models.Indexes;
using Inmeta.Test.Services.Models.Dtos.Customers;
using Inmeta.Test.Services.Models.Dtos.Indexes;

namespace Inmeta.Test.Services.Abstractions
{
	public interface ICustomerService :
        ICrudService<CustomerHashId, CreateCustomerDto, ReadCustomerDto, UpdateCustomerDto>,
        IPagedService<ReadCustomerDto>
    {
        Task<ReadCustomerDto> Read(CustomerEmail customerEmail);
        Task<bool> IsResourceOwner(CustomerHashId customerHashId, OrderHashId orderHashId);
    }
}
