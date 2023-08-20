using Inmeta.Test.Data.Abstractions;
using Inmeta.Test.Data.Models.Enums;
using Inmeta.Test.Services.Models.Abstractions;

namespace Inmeta.Test.Services.Models.Dtos.OrderLogs
{
	public readonly record struct ReadOrderLogDto : ILoggable, IHashedIdentifiable, IAuditable
    {
        public string HashId { get; init; }
        public string OrderHashId { get; init; }
        public DateTimeOffset CreatedAt { get; init; }
        public DateTimeOffset ModifiedAt { get; init; }
		public string Message { get; init; }
    }
}
