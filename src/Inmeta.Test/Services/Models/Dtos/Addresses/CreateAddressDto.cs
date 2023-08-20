using Inmeta.Test.Data.Abstractions.Entities;
using Inmeta.Test.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Inmeta.Test.Services.Models.Dtos.Addresses
{
	public readonly record struct CreateAddressDto : IAddress
    {
		[JsonRequired]
		public string StreetAddress { get; init; }
		[JsonRequired]
		[EnumDataType(typeof(ZipCode))]
		public ZipCode ZipCode { get; init; }
		public string? City { get; init; }
		[EnumDataType(typeof(Country))]
		public Country? Country { get; init; }
}
}
