using Inmeta.Test.Data.Abstractions;
using Inmeta.Test.Data.Models.Enums;
using Inmeta.Test.Services.Abstractions.Nullable;
using Inmeta.Test.Services.Models.Dtos.Addresses;
using System.ComponentModel.DataAnnotations;

namespace Inmeta.Test.Services.Models.Dtos.Orders
{
	public readonly record struct UpdateOrderDto : INullableOrder, ILoggable
    {
		[EnumDataType(typeof(OrderStatus))]
		public OrderStatus? OrderStatus { get; init; }
		[EnumDataType(typeof(Service))]
		public Service? Service { get; init; }
		public string Message { get; init; }
		public ReadAddressDto? FromAddress { get; init; }
		public ReadAddressDto? ToAddress { get; init; }
	}
}
