using Microsoft.AspNetCore.Mvc;

namespace Inmeta.Test.Api.Abstractions
{
    public interface IPagedController
    {
        Task<IActionResult> Get(
            int? pageNumber,
            int? pageSize,
            string? q
        );
    }
}