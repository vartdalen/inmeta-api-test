using Inmeta.Test.Data.Collections;
using Inmeta.Test.Data.Models.Entities;
using Inmeta.Test.Data.Models.Indexes;
using Inmeta.Test.Data.Models.Requests;

namespace Inmeta.Test.Data.Abstractions.Repositories
{
    public interface IOrderLogRepository :
        ICrudRepository<int, OrderLog, OrderLogRequest>,
        IPagedQueryRepository<OrderLog>,
        IPagedResourceRepository<OrderLog>
    {
        Task<PagedList<OrderLog>> Read(
            int pageNumber,
            int pageSize,
            OrderId orderId
        );
    }
}
