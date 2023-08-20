using Inmeta.Test.Services.Models.Dtos.Indexes;
using Inmeta.Test.Services.Models.Dtos.Orders;

namespace Inmeta.Test.Services.Abstractions
{
    public interface IOrderService :
        ICrudService<OrderHashId, CreateOrderDto, ReadOrderDto, UpdateOrderDto>,
        IPagedService<ReadOrderDto>
    {
    }
}
