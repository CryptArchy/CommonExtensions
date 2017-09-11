using System;
using System.Collections.Generic;

namespace CommonExtensions.EnumerableExtensions
{
    public static partial class EnumerableExt
    {
        /// <summary>
        /// Immediately executes the given action on each element in the source sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the sequence.</typeparam>
        /// <param name="source">The sequence of elements.</param>
        /// <param name="action">The action to execute on each element.</param>
        public static void ForEach<TSource>(this IEnumerable<TSource> source, Action<TSource> action)
        {
            source.ThrowIfNull("source");
            action.ThrowIfNull("action");

            foreach (var element in source)
                action(element);
        }
    }
}
