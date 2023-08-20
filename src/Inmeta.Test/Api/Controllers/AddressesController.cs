using Inmeta.Test.Api.Abstractions;
using Inmeta.Test.Api.Extensions;
using Inmeta.Test.Services.Abstractions;
using Inmeta.Test.Services.Models.Dtos.Addresses;
using Inmeta.Test.Services.Models.Dtos.Indexes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Inmeta.Test.Api.Controllers
{
	[ApiController]
    [Route("[controller]")]
    public class AddressesController :
        ControllerBase,
        ICrudController<CreateAddressDto, UpdateAddressDto>,
        IPagedController
    {
        private readonly IAddressService _addressService;

        public AddressesController(IAddressService addressService)
        {
            _addressService = addressService;
        }

        [Authorize(AuthenticationSchemes = "Bearer", Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateAddressDto dto)
        {
            try
            {
                var address = await _addressService.Create(dto);
                return CreatedAtAction(nameof(Get), new { hashId = address.HashId }, address);
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
            var addresses = await _addressService.Read(pageNumber ?? 1, pageSize ?? 20, q);
            return Ok(addresses);
        }

        [Authorize(AuthenticationSchemes = "Cookie,Bearer", Roles = "admin")]
        [HttpGet("{hashId}")]
        public async Task<IActionResult> Get(string hashId)
        {
            try
            {
                var address = await _addressService.Read(new AddressHashId(hashId));
                return Ok(address);
            }
            catch (Exception ex)
            {
                if (ex.IsSystemNotFound()) return NotFound().WithInvalidIdentifier(hashId);
                throw;
            }
        }

        [Authorize(AuthenticationSchemes = "Bearer", Roles = "admin")]
        [HttpPatch("{hashId}")]
        public async Task<IActionResult> Patch(
            string hashId,
            [FromBody] UpdateAddressDto dto
        )
        {
            try
            {
                await _addressService.Update(new AddressHashId(hashId), dto);
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

        [Authorize(AuthenticationSchemes = "Bearer", Roles = "admin")]
        [HttpDelete("{hashId}")]
        public async Task<IActionResult> Delete(string hashId)
        {
            try
            {
                await _addressService.Delete(new AddressHashId(hashId));
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