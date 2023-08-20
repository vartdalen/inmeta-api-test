using Inmeta.Test.Data.Abstractions;
using Inmeta.Test.Data.Abstractions.Entities;
using Inmeta.Test.Services.Models.Abstractions;

namespace Inmeta.Test.Services.Models.Dtos.Customers
{
    public readonly record struct ReadCustomerDto : ICustomer, IHashedIdentifiable, IAuditable
    {
        public string HashId { get; init; }
        public DateTimeOffset CreatedAt { get; init; }
        public DateTimeOffset ModifiedAt { get; init; }
        public string Email { get; init; }
		public string Phone { get; init; }
		public string Name { get; init; }
	}
}
