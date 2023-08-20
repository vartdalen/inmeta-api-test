using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Inmeta.Test.Api.Abstractions;
using Inmeta.Test.Api.Extensions;
using Inmeta.Test.Services.Abstractions;
using Inmeta.Test.Services.Models.Dtos.Indexes;
using Inmeta.Test.Services.Models.Dtos.Orders;
using System.ComponentModel.DataAnnotations;

namespace Inmeta.Test.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrdersController :
        ControllerBase,
        ICrudController<CreateOrderDto, UpdateOrderDto>,
        IPagedController
    {
        private readonly IOrderService _orderService;
        private readonly IAuthorizationService _authorizationService;

        public OrdersController(
            IOrderService orderService,
            IAuthorizationService authorizationService
        )
        {
            _orderService = orderService;
            _authorizationService = authorizationService;
        }

        [Authorize(AuthenticationSchemes = "Cookie,Bearer")]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateOrderDto dto)
        {
            var authorizeResult = await _authorizationService.AuthorizeAsync(User, dto, "AdminOrCustomerHashIdDtoPropertyMatch");
            if (!authorizeResult.Succeeded) return Forbid();
            try
            {
                var order = await _orderService.Create(dto);
                return CreatedAtAction(nameof(Get), new { hashId = order.HashId }, order);
            }
            catch (Exception ex)
            {
                if (ex.IsMySqlDataTooLong()) return UnprocessableEntity();
                if (ex.IsMySqlNoReferencedRow()) return NotFound();
                if (ex.IsSystemNotFound()) return NotFound().WithInvalidIdentifier(dto.CustomerHashId);
                throw;
            }
        }

        [Authorize(AuthenticationSchemes = "Cookie,Bearer", Policy = "AdminOrCustomerHashIdQueryMatch")]
        [HttpGet]
        public async Task<IActionResult> Get(
            [FromQuery][Range(1, int.MaxValue)] int? pageNumber,
            [FromQuery][Range(1, int.MaxValue)] int? pageSize,
            [FromQuery] string? q
        )
        {
            var orders = await _orderService.Read(pageNumber ?? 1, pageSize ?? 20, q);
            return Ok(orders);
        }

        [Authorize(AuthenticationSchemes = "Cookie,Bearer", Policy = "AdminOrOrderHashIdRouteValueMatch")]
        [HttpGet("{hashId}")]
        public async Task<IActionResult> Get(string hashId)
        {
            try
            {
                var order = await _orderService.Read(new OrderHashId(hashId));
                return Ok(order);
            }
            catch (Exception ex)
            {
                if (ex.IsSystemNotFound()) return NotFound().WithInvalidIdentifier(hashId);
                throw;
            }
        }

        [Authorize(AuthenticationSchemes = "Cookie,Bearer", Roles = "admin")]
        [HttpPatch("{hashId}")]
        public async Task<IActionResult> Patch(
            string hashId,
            [FromBody] UpdateOrderDto dto
        )
        {
            try
            {
                await _orderService.Update(new OrderHashId(hashId), dto);
                return NoContent();
            }
            catch (Exception ex)
            {
                if (ex is InvalidOperationException) return UnprocessableEntity().WithDetail($"{nameof(dto.OrderStatus)} is already set to '{dto.OrderStatus}'");
                if (ex.IsMySqlDataTooLong()) return UnprocessableEntity();
                if (ex.IsMySqlDuplicateKeyEntry()) return Conflict();
                if (ex.IsSystemNotFound()) return NotFound().WithInvalidIdentifier(hashId);
                throw;
            }
        }

        [Authorize(AuthenticationSchemes = "Cookie,Bearer", Roles = "admin")]
        [HttpDelete("{hashId}")]
        public async Task<IActionResult> Delete(string hashId)
        {
            try
            {
                await _orderService.Delete(new OrderHashId(hashId));
                return NoContent();
            }
            catch (Exception ex)
            {
                if (ex.IsSystemNotFound()) return NotFound().WithInvalidIdentifier(hashId);
                throw;
            }
        }
    }
}
