using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Inmeta.Test.Api.Abstractions;
using Inmeta.Test.Api.Extensions;
using Inmeta.Test.Data.Models.Indexes;
using Inmeta.Test.Services.Abstractions;
using Inmeta.Test.Services.Models.Dtos.Customers;
using Inmeta.Test.Services.Models.Dtos.Indexes;
using System.ComponentModel.DataAnnotations;

namespace Inmeta.Test.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomersController :
        ControllerBase,
        ICrudController<CreateCustomerDto, UpdateCustomerDto>,
        IPagedController
    {
        private readonly ICustomerService _customerService;

        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [Authorize(AuthenticationSchemes = "Bearer", Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateCustomerDto dto)
        {
            try
            {
                var customer = await _customerService.Create(dto);
                return CreatedAtAction(nameof(Get), new { hashId = customer.HashId }, customer);
            }
            catch (Exception ex)
            {
                if (ex.IsMySqlDataTooLong()) return UnprocessableEntity();
                if (ex.IsMySqlDuplicateKeyEntry()) return Conflict();
                throw;
            }
        }

        [Authorize(AuthenticationSchemes = "Cookie,Bearer", Roles = "admin")]
        [HttpGet]
        public async Task<IActionResult> Get(
            [FromQuery][Range(1, int.MaxValue)] int? pageNumber,
            [FromQuery][Range(1, int.MaxValue)] int? pageSize,
            [FromQuery] string? q
        )
        {
            var customers = await _customerService.Read(pageNumber ?? 1, pageSize ?? 20, q);
            return Ok(customers);
        }

        [Authorize(AuthenticationSchemes = "Cookie,Bearer", Policy = "AdminOrCustomerHashIdRouteValueMatch")]
        [HttpGet("{hashId}")]
        public async Task<IActionResult> Get(string hashId)
        {
            try
            {
                var customer = await _customerService.Read(new CustomerHashId(hashId));
                return Ok(customer);
            }
            catch (Exception ex)
            {
                if (ex.IsSystemNotFound()) return NotFound().WithInvalidIdentifier(hashId);
                throw;
            }
        }

        [Authorize(AuthenticationSchemes = "Cookie,Bearer", Roles = "admin")]
        [HttpGet("{email:email}")]
        public async Task<IActionResult> GetByEmail(string email)
        {
            try
            {
                var customer = await _customerService.Read(new CustomerEmail(email));
                return Ok(customer);
            }
            catch (Exception ex)
            {
                if (ex.IsSystemNotFound()) return NotFound().WithInvalidIdentifier(email);
                throw;
            }
        }

        [Authorize(AuthenticationSchemes = "Bearer", Policy = "AdminOrCustomerHashIdRouteValueMatch")]
        [HttpPatch("{hashId}")]
        public async Task<IActionResult> Patch(
            string hashId,
            [FromBody] UpdateCustomerDto dto
        )
        {
            if (!string.IsNullOrEmpty(dto.Email) && !User.IsInRole("admin")) return Forbid();
            try
            {
                await _customerService.Update(new CustomerHashId(hashId), dto);
                return NoContent();
            }
            catch (Exception ex)
            {
                if (ex.IsMySqlDataTooLong()) return UnprocessableEntity();
                if (ex.IsMySqlDuplicateKeyEntry()) return Conflict();
                if (ex.IsSystemNotFound()) return NotFound().WithInvalidIdentifier(hashId);
                throw;
            }
        }

        [Authorize(AuthenticationSchemes = "Bearer", Policy = "AdminOrCustomerHashIdRouteValueMatch")]
        [HttpDelete("{hashId}")]
        public async Task<IActionResult> Delete(string hashId)
        {
            try
            {
                await _customerService.Delete(new CustomerHashId(hashId));
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