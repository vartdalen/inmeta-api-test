using Inmeta.Test.Data.Collections;

namespace Inmeta.Test.Data.Abstractions.Repositories
{
    public interface IPagedQueryRepository<T>
    {
        Task<PagedList<T>> Read(
            int pageNumber,
            int pageSize,
            string? q
        );
    }
}
