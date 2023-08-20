using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Inmeta.Test.Services.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace Inmeta.Test.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderLogsController : ControllerBase
    {
        private readonly IOrderLogService _orderLogsService;

        public OrderLogsController(IOrderLogService orderLogsService)
        {
            _orderLogsService = orderLogsService;
        }

        [Authorize(AuthenticationSchemes = "Cookie,Bearer", Policy = "AdminOrCustomerHashIdQueryMatch")]
        [HttpGet]
        public async Task<IActionResult> Get(
            [FromQuery][Range(1, int.MaxValue)] int? pageNumber,
            [FromQuery][Range(1, int.MaxValue)] int? pageSize,
            [FromQuery] string? q
        )
        {
            var orderLogs = await _orderLogsService.Read(pageNumber ?? 1, pageSize ?? 20, q);
            return Ok(orderLogs);
        }
    }
}
