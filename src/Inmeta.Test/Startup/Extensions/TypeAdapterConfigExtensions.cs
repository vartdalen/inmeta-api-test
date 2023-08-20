using Inmeta.Test.Data.Models.Entities;
using Inmeta.Test.Services.Abstractions;
using Inmeta.Test.Services.Models.Dtos.Addresses;
using Inmeta.Test.Services.Models.Dtos.Customers;
using Inmeta.Test.Services.Models.Dtos.OrderLogs;
using Inmeta.Test.Services.Models.Dtos.Orders;
using Mapster;

namespace Inmeta.Test.Startup.Extensions
{
	internal static class TypeAdapterConfigExtensions
    {
        internal static void ConfigureMapping(
            this TypeAdapterConfig config,
            IHashidsService hashidsService
        )
        {
			var aHashids	= hashidsService.Get(typeof(Address));
			var cHashids    = hashidsService.Get(typeof(Customer));
            var oHashids    = hashidsService.Get(typeof(Order));
			var olHashids   = hashidsService.Get(typeof(OrderLog));

			config.ForType<Address, ReadAddressDto>()
				.Map(dest => dest.HashId, src => aHashids.Encode(src.Id))
				.Compile();

			config.ForType<Customer, ReadCustomerDto>()
                .Map(dest => dest.HashId, src => cHashids.Encode(src.Id))
                .Compile();

            config.ForType<UpdateCustomerDto, Customer>()
                .IgnoreIf((src, dest) => string.IsNullOrEmpty(src.Email), dest => dest.Email)
				.IgnoreIf((src, dest) => string.IsNullOrEmpty(src.Phone), dest => dest.Phone)
				.IgnoreIf((src, dest) => string.IsNullOrEmpty(src.Name), dest => dest.Name)
				.Compile();

            config.ForType<CreateOrderDto, Order>()
                .Map(dest => dest.CustomerId, src => cHashids.DecodeSingle(src.CustomerHashId))
                .Compile();

			config.ForType<UpdateOrderDto, Order>()
				.IgnoreIf((src, dest) => !src.OrderStatus.HasValue, dest => dest.OrderStatus)
				.IgnoreIf((src, dest) => !src.Service.HasValue, dest => dest.Service)
				.IgnoreIf((src, dest) => !src.FromAddress.HasValue, dest => dest.FromAddress)
				.IgnoreIf((src, dest) => !src.ToAddress.HasValue, dest => dest.ToAddress)
				.Compile();

			config.ForType<Order, ReadOrderDto>()
                .Map(dest => dest.HashId, src => oHashids.Encode(src.Id))
                .Map(dest => dest.CustomerHashId, src => cHashids.Encode(src.CustomerId))
                .Compile();

			config.ForType<OrderLog, ReadOrderLogDto>()
				.Map(dest => dest.HashId, src => olHashids.Encode(src.Id))
				.Map(dest => dest.OrderHashId, src => oHashids.Encode(src.OrderId))
				.Compile();
		}
    }
}