using Inmeta.Test.Data.Models.Entities;
using Inmeta.Test.Data.Models.Indexes;
using Inmeta.Test.Data.Models.Requests;

namespace Inmeta.Test.Data.Abstractions.Repositories
{
    public interface ICustomerRepository :
        ICrudRepository<int, Customer, CustomerRequest>,
        IPagedQueryRepository<Customer>
    {
        Task<Customer?> Read(CustomerEmail customerEmail);
        Task<bool> IsResourceOwner(CustomerId customerId, OrderId orderId);
    }
}
