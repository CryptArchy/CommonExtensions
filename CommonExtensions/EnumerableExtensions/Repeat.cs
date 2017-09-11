using System.Collections.Generic;

namespace CommonExtensions.EnumerableExtensions
{
    public static partial class EnumerableExt
    {
        /// <summary>
        /// Repeats the specific sequences <paramref name="count"/> times.
        /// </summary>
        /// <param name="source">The sequence to repeat</param>
        /// <param name="count">Number of times to repeat the sequence</param>
        /// <returns>A sequence produced from the repetition of the original source sequence</returns>
        public static IEnumerable<TSource> Repeat<TSource>(this IEnumerable<TSource> source, int count)
        {
            source.ThrowIfNull("source");
            count.ThrowIfNonPositive("count");

            return RepeatImpl(source, count);
        }


        private static IEnumerable<TSource> RepeatImpl<TSource>(this IEnumerable<TSource> source, int count)
        {
            var buffer = new List<TSource>();

            foreach (var item in source)
            {
                buffer.Add(item);
                yield return item;
            }

            count--;

            while (count-- > 0)
            {
                foreach (var item in buffer)
                    yield return item;
            }
        }
    }
}
