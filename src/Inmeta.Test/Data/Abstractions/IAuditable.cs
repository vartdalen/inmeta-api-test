namespace Inmeta.Test.Data.Abstractions
{
    public interface IAuditable
    {
        DateTimeOffset CreatedAt { get; }
        DateTimeOffset ModifiedAt { get; }
    }
}
