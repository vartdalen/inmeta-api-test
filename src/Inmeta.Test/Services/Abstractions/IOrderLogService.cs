using Inmeta.Test.Services.Models.Dtos.OrderLogs;

namespace Inmeta.Test.Services.Abstractions
{
    public interface IOrderLogService :
        IPagedService<ReadOrderLogDto>
    {
    }
}
