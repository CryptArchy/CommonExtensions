using System;

namespace CommonExtensions
{
    public static partial class ThrowExt
    {
        /// <summary>
        /// Throws an ArgumentNullException if the value is null.
        /// </summary>
        /// <typeparam name="T">Any type</typeparam>
        /// <param name="argument">The value to test</param>
        /// <param name="name">The name of the value, for the exception</param>
        public static void ThrowIfNull<T>(this T argument, string name) where T : class
        {
            if (argument == null)
            {
                throw new ArgumentNullException(name);
            }
        }
    }
}
