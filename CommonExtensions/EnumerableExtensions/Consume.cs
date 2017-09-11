using System.Collections.Generic;

namespace CommonExtensions.EnumerableExtensions
{
    public static partial class EnumerableExt
    {
        /// <summary>
        /// Completely consumes the given sequence. This method uses immediate execution, and doesn't store any data during execution.
        /// </summary>
        /// <typeparam name="TSource">Element type of the sequence</typeparam>
        /// <param name="source">Source to consume</param>
        public static void Consume<TSource>(this IEnumerable<TSource> source)
        {
            source.ThrowIfNull("source");
            foreach (var element in source) { }
        }
    }
}
