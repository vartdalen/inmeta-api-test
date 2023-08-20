using Inmeta.Test.Data.Models.Entities;
using Inmeta.Test.Data.Models.Enums;

namespace Inmeta.Test.Services.Abstractions.Nullable
{
    public interface INullableOrder
    {
        OrderStatus? OrderStatus { get; }
        Service? Service { get; }
	}
}
