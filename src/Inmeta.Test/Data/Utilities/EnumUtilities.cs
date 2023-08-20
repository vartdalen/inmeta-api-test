namespace Inmeta.Test.Data.Utilities
{
    public static class EnumUtilities
    {
        /// <summary>
        /// Ensures that the provided value is comprised of only valid flags for the provided <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of the enum to check the flags on.</typeparam>
        /// <param name="value">The value to check for only valid flags.</param>
        /// <returns>True - the value contains only valid flags; False - otherwise.</returns>
        public static bool IsFlagsValid<T>(long value)
        {
            Type enumType = typeof(T);
            if (enumType.IsEnum && enumType.IsDefined(typeof(FlagsAttribute), false))
            {
                long compositeValues = 0;
                // Build up all of the valid bits into one superset.
                foreach (object flag in Enum.GetValues(enumType))
                    compositeValues |= Convert.ToInt64(flag);
                // Ensure none of the invalid bits are on.
                return (~compositeValues & value) == 0;
            }
            else { return false; }
        }
    }
}
