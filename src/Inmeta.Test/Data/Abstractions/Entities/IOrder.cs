using Inmeta.Test.Data.Models.Entities;
using Inmeta.Test.Data.Models.Enums;

namespace Inmeta.Test.Data.Abstractions.Entities
{
    public interface IOrder
    {
        OrderStatus OrderStatus { get; }
        Service Service { get; }
	}
}
