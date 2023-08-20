using Inmeta.Test.Data.Collections;

namespace Inmeta.Test.Services.Abstractions
{
	public interface IPagedService<TReadDto>
    {
        Task<PagedList<TReadDto>> Read(
            int pageNumber,
            int pageSize,
            string? query = null
        );
    }
}