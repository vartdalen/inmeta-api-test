using Inmeta.Test.Data.Abstractions;
using Inmeta.Test.Data.Abstractions.Entities;
using Inmeta.Test.Data.Models.Enums;
using Inmeta.Test.Services.Models.Abstractions;
using Inmeta.Test.Services.Models.Dtos.Addresses;

namespace Inmeta.Test.Services.Models.Dtos.Orders
{
	public readonly record struct ReadOrderDto : IOrder, IHashedIdentifiable, IAuditable
    {
        public string HashId { get; init; }
        public string CustomerHashId { get; init; }
        public DateTimeOffset CreatedAt { get; init; }
        public DateTimeOffset ModifiedAt { get; init; }
        public OrderStatus OrderStatus { get; init; }
		public Service Service { get; init; }
		public decimal TotalCost { get; init; }
        public ReadAddressDto FromAddress { get; init; }
		public ReadAddressDto ToAddress { get; init; }
	}
}
