namespace Inmeta.Test.Data.Extensions
{
    internal static class IEnumerableExtensions
    {
        internal static bool IsNullOrEmpty<T>(this IEnumerable<T>? source)
        {
            return source is null || !source.Any();
        }
    }
}
