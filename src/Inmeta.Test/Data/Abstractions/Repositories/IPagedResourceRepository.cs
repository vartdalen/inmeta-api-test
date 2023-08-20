using Inmeta.Test.Data.Collections;
using Inmeta.Test.Data.Models.Indexes;

namespace Inmeta.Test.Data.Abstractions.Repositories
{
    public interface IPagedResourceRepository<T>
    {
        Task<PagedList<T>> Read(
            int pageNumber,
            int pageSize,
            CustomerId customerId
        );
    }
}
