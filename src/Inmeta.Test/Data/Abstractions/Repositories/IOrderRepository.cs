using Inmeta.Test.Data.Models.Entities;
using Inmeta.Test.Data.Models.Requests;

namespace Inmeta.Test.Data.Abstractions.Repositories
{
    public interface IOrderRepository :
        ICrudRepository<int, Order, OrderRequest>,
        IPagedQueryRepository<Order>,
        IPagedResourceRepository<Order>
    {
    }
}
