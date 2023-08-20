using Inmeta.Test.Data.Models.Enums;
using Inmeta.Test.Services.Abstractions.Nullable;
using System.ComponentModel.DataAnnotations;

namespace Inmeta.Test.Services.Models.Dtos.Addresses
{
	public readonly record struct UpdateAddressDto : INullableAddress
    {
		public string? StreetAddress { get; init; }
		[EnumDataType(typeof(ZipCode))]
		public ZipCode? ZipCode { get; init; }
		public string? City { get; init; }
		[EnumDataType(typeof(Country))]
		public Country? Country { get; init; }
}
}
