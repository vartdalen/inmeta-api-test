using Inmeta.Test.Data.Abstractions.Entities;
using Inmeta.Test.Data.Models.Enums;
using Inmeta.Test.Services.Models.Abstractions;
using Inmeta.Test.Services.Models.Dtos.Addresses;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Inmeta.Test.Services.Models.Dtos.Orders
{
	public readonly record struct CreateOrderDto : IOrder, ICustomerHashedIdentifiable
    {
        [JsonRequired]
        public string CustomerHashId { get; init; }
        [JsonRequired]
        [EnumDataType(typeof(OrderStatus))]
        public OrderStatus OrderStatus { get; init; }
		[JsonRequired]
		[EnumDataType(typeof(Service))]
		public Service Service { get; init; }
		public CreateAddressDto FromAddress { get; init; }
		public CreateAddressDto ToAddress { get; init; }
	}
}
