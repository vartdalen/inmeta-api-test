using Inmeta.Test.Data.Utilities;
using System.Text;

namespace Inmeta.Test.Data.Extensions
{
    internal static class StringExtensions {
        internal static bool TryParse<T>(this string input, out T result) where T : struct, Enum
        {
            // Attempt to parse the input string as the enum type
            if (Enum.TryParse(input, true, out result))
            {
                return true;
            }
            else
            {
                var type = typeof(T).GetEnumUnderlyingType();
                if (type == typeof(byte) &&
                    byte.TryParse(input, out var byteValue) &&
                    (Enum.IsDefined(typeof(T), byteValue) || EnumUtilities.IsFlagsValid<T>(byteValue)))
                {
                    result = (T)Enum.ToObject(typeof(T), byteValue);
                    return true;
                }
                if (type == typeof(short) &&
                    short.TryParse(input, out var shortValue) &&
                    (Enum.IsDefined(typeof(T), shortValue) || EnumUtilities.IsFlagsValid<T>(shortValue)))
                {
                    result = (T)Enum.ToObject(typeof(T), shortValue);
                    return true;
                }
                if (type == typeof(int) &&
                    int.TryParse(input, out var intValue) &&
                    (Enum.IsDefined(typeof(T), intValue) || EnumUtilities.IsFlagsValid<T>(intValue)))
                {
                    result = (T)Enum.ToObject(typeof(T), intValue);
                    return true;
                }
            }

            result = default;
            return false;
        }

        internal static string ToKebabCase(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            var result = new StringBuilder();

            bool prevWasUpper = false;
            bool prevWasSymbol = false;

            foreach (char c in input)
            {
                if (char.IsUpper(c))
                {
                    if (!prevWasUpper && !prevWasSymbol && result.Length > 0)
                        result.Append('-');

                    result.Append(char.ToLower(c));
                    prevWasUpper = true;
                    prevWasSymbol = false;
                }
                else if (char.IsLetterOrDigit(c))
                {
                    result.Append(c);
                    prevWasUpper = false;
                    prevWasSymbol = false;
                }
                else
                {
                    if (!prevWasSymbol && result.Length > 0)
                        result.Append('-');

                    prevWasUpper = false;
                    prevWasSymbol = true;
                }
            }

            return result.ToString();
        }
    }
}
