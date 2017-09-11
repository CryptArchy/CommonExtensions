using System;
using System.Collections.Generic;

namespace CommonExtensions.NumericExtensions
{
    public static partial class NumericExt
    {
        /// <summary>
        /// Returns whether the value is between the 2 specified boundaries, bouth bounds inclusive.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value to compare</param>
        /// <param name="minimumInclusiveValue">The min value (inclusive).</param>
        /// <param name="maximumInclusiveValue">The max value (inclusive).</param>
        /// <returns>True if the value is equal to or between the boundaries; False otherwise.</returns>
        /// <remarks>See also: 
        /// <seealso cref="Within">Within</seealso>
        /// <seealso cref="BetweenMin">BetweenMin</seealso>
        /// <seealso cref="BetweenMax">BetweenMax</seealso></remarks>
        public static bool Between<T>(this T value, T minimumInclusiveValue, T maximumInclusiveValue) where T : IComparable<T>
        {
            return value.CompareTo(minimumInclusiveValue) >= 0 && value.CompareTo(maximumInclusiveValue) <= 0;
        }

        /// <summary>
        /// Returns whether the value is within the 2 specified boundaries, both bounds exclusive.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value to compare</param>
        /// <param name="minimumExclusiveValue">The min value (exclusive).</param>
        /// <param name="maximumExclusiveValue">The max value (exclusive).</param>
        /// <returns>True if the value is between the boundaries; False otherwise.</returns>
        /// <remarks>See also: 
        /// <seealso cref="Between">Between</seealso>
        /// <seealso cref="BetweenMin">BetweenMin</seealso>
        /// <seealso cref="BetweenMax">BetweenMax</seealso></remarks>
        public static bool Within<T>(this T value, T minimumExclusiveValue, T maximumExclusiveValue) where T : IComparable<T>
        {
            return value.CompareTo(minimumExclusiveValue) > 0 && value.CompareTo(maximumExclusiveValue) < 0;
        }

        /// <summary>
        /// Returns whether the value is between the 2 specified boundaries, lower bound inclusive.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value to compare</param>
        /// <param name="minimumInclusiveValue">The min value (inclusive).</param>
        /// <param name="maximumExclusiveValue">The max value (exclusive).</param>
        /// <returns>True if the value is between the boundaries; False otherwise.</returns>
        /// <remarks>See also: 
        /// <seealso cref="Between">Between</seealso>
        /// <seealso cref="Within">Within</seealso>
        /// <seealso cref="BetweenMax">BetweenMax</seealso></remarks>
        public static bool BetweenMin<T>(this T value, T minimumInclusiveValue, T maximumExclusiveValue) where T : IComparable<T>
        {
            return value.CompareTo(minimumInclusiveValue) >= 0 && value.CompareTo(maximumExclusiveValue) < 0;
        }

        /// <summary>
        /// Returns whether the value is between the 2 specified boundaries, upper bound inclusive.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value to compare</param>
        /// <param name="minimumExclusiveValue">The min value (exclusive).</param>
        /// <param name="maximumInclusiveValue">The max value (inclusive).</param>
        /// <returns>True if the value is between the boundaries; False otherwise.</returns>
        /// <remarks>See also: 
        /// <seealso cref="Between">Between</seealso>
        /// <seealso cref="BetweenMax">Within</seealso>
        /// <seealso cref="BetweenMin">BetweenMin</seealso></remarks>
        public static bool BetweenMax<T>(this T value, T minimumExclusiveValue, T maximumInclusiveValue) where T : IComparable<T>
        {
            return value.CompareTo(minimumExclusiveValue) > 0 && value.CompareTo(maximumInclusiveValue) <= 0;
        }

        public static T LesserOf<T>(this T x, T y)
        {
            return Comparer<T>.Default.Compare(x, y) < 0 ? x : y;
        }

        public static T GreaterOf<T>(this T x, T y)
        {
            return Comparer<T>.Default.Compare(x, y) > 0 ? x : y;
        }

        public static double RoundTo(this double value, int decimalPlaces)
        {
            return Math.Round(value, decimalPlaces, MidpointRounding.AwayFromZero);
        }

        public static decimal RoundTo(this decimal value, int decimalPlaces)
        {
            return Math.Round(value, decimalPlaces, MidpointRounding.AwayFromZero);
        }
    }
}
