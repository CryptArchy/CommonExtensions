using System.Collections.Generic;
using System.Linq;

namespace CommonExtensions.EnumerableExtensions
{
    public static partial class EnumerableExt
    {
        /// <summary>
        /// Puts the given element at the head of the enumerable.
        /// </summary>
        /// <typeparam name="TSource">The type of the enumerable.</typeparam>
        /// <param name="source">The base enumerable to prepend on.</param>
        /// <param name="head">The value to be prepended.</param>
        /// <returns>An enumerable with the value as the first element.</returns>
        public static IEnumerable<TSource> Prepend<TSource>(this IEnumerable<TSource> source, TSource head)
        {
            source.ThrowIfNull("source");
            return Enumerable.Concat(Enumerable.Repeat(head, 1), source);
        }

        /// <summary>
        /// Puts the given element at the end of the enumerable.
        /// </summary>
        /// <typeparam name="TSource">The type of the enumerable.</typeparam>
        /// <param name="source">The base enumerable to append on.</param>
        /// <param name="tail">The value to be appended.</param>
        /// <returns>An enumerable with the value as the last element.</returns>
        public static IEnumerable<TSource> Append<TSource>(this IEnumerable<TSource> source, TSource tail)
        {
            source.ThrowIfNull("source");
            return Enumerable.Concat(source, Enumerable.Repeat(tail, 1));
        }
    }
}
