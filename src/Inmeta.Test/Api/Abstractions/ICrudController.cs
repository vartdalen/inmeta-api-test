using Microsoft.AspNetCore.Mvc;

namespace Inmeta.Test.Api.Abstractions
{
    internal interface ICrudController<
        TCreateDto,
        TUpdateDto
    >
    {
        Task<IActionResult> Post(TCreateDto productDto);
        Task<IActionResult> Get(string id);
        Task<IActionResult> Patch(string id, TUpdateDto dto);
        Task<IActionResult> Delete(string id);
    }
}
