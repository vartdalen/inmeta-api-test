using Inmeta.Test.Data.Abstractions;
using Inmeta.Test.Data.Abstractions.Entities;
using Inmeta.Test.Data.Models.Enums;

namespace Inmeta.Test.Services.Models.Dtos.Addresses
{
	public readonly record struct ReadAddressDto : IAddress, IAuditable
    {
		public string HashId { get; init; }
		public DateTimeOffset CreatedAt { get; init; }
        public DateTimeOffset ModifiedAt { get; init; }
		public string StreetAddress { get; init; }
		public ZipCode ZipCode { get; init; }
		public string? City { get; init; }
		public Country? Country { get; init; }
	}
}
