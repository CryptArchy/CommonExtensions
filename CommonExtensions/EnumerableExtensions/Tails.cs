using System.Collections.Generic;
using System.Linq;

namespace CommonExtensions.EnumerableExtensions
{
    public static partial class EnumerableExt
    {
        /// <summary>
        /// Generates a sequence of all sub-sequence tails of the initial enumerable.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source.</typeparam>
        /// <param name="source">The enumerable to generate tails for.</param>
        /// <returns>An enumerable of enumerables, each of which is a sub-sequence of the original.</returns>
        /// <remarks>The sub-sequences are ordered longest-first, starting with the original sequence.</remarks>
        /// <example>
        ///     Given the following sequence:
        ///     <code>var example = new List&lt;string&gt;() {"A","B","C","D","E"}; </code>
        ///     calling Tails like:
        ///     <code>var result = example.Tails()</code>
        ///     will give back a result like:
        ///     <code>{{"A","B","C","D","E"},{"B","C","D","E"},{"C","D","E"},{"D","E"},{"E"}}</code>
        /// </example>
        public static IEnumerable<IEnumerable<TSource>> Tails<TSource>(this IEnumerable<TSource> source)
        {
            source.ThrowIfNull("source");
            return source.Select((e, i) => source.Skip(i));
        }
    }
}
