using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace CommonExtensions.EnumerableExtensions
{
    public static partial class EnumerableExt
    {
        /// <summary>
        /// Partitions the source into equally-sized groups ("chunks it").  
        /// If the source is not evenly divisible by the size, the last chunk will have fewer elements.
        /// </summary>
        /// <typeparam name="TSource">Type of elements in <paramref name="source"/> sequence.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="size">Size of each sub-group.</param>
        /// <returns>A sequence of equally sized chunks containing elements of the source collection.</returns>
        /// <remarks> This operator uses deferred execution and streams its results (chunks and chunk content).</remarks>
        public static IEnumerable<IEnumerable<TSource>> Chunk<TSource>(this IEnumerable<TSource> source, int size)
        {
            return Chunk(source, size, Identity);
        }


        /// <summary>
        /// Partitions the source into equally-sized groups ("chunks it"), and applies a projection function to each chunk.
        /// If the source is not evenly divisible by the size, the last chunk will have fewer elements.
        /// </summary>
        /// <typeparam name="TSource">Type of elements in <paramref name="source"/> sequence.</typeparam>
        /// <typeparam name="TResult">Type of result returned by <paramref name="resultSelector"/>.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="size">Size of each sub-group.</param>
        /// <param name="resultSelector">The projection to apply to each chunk.</param>
        /// <returns>A sequence of equally sized chunks containing elements of the source collection.</returns>
        /// <remarks> This operator uses deferred execution and streams its results (chunks and chunk content).</remarks>
        public static IEnumerable<TResult> Chunk<TSource, TResult>(this IEnumerable<TSource> source, int size, Func<IEnumerable<TSource>, TResult> resultSelector)
        {
            source.ThrowIfNull("source");
            size.ThrowIfNonPositive("size");
            resultSelector.ThrowIfNull("resultSelector");
            return ChunkIterator(source, size, resultSelector);
        }


        private static IEnumerable<TResult> ChunkIterator<TSource, TResult>(this IEnumerable<TSource> source, int size, Func<IEnumerable<TSource>, TResult> resultSelector)
        {
            Debug.Assert(source != null);
            Debug.Assert(size > 0);
            Debug.Assert(resultSelector != null);

            TSource[] bucket = null;
            var count = 0;

            foreach (var item in source)
            {
                if (bucket == null)
                    bucket = new TSource[size];

                bucket[count++] = item;

                // The bucket is fully buffered before it's yielded
                if (count != size)
                    continue;

                // Select is necessary so bucket contents are streamed too
                yield return resultSelector(bucket.Select(x => x));

                bucket = null;
                count = 0;
            }

            // Return the last bucket with all remaining elements
            if (bucket != null && count > 0)
                yield return resultSelector(bucket.Take(count));
        }

        /// <summary>
        /// Partitions the source into equally-sized groups ("chunks it").  
        /// If the source is not evenly divisible by the size, the last chunk will have fewer elements.
        /// </summary>
        /// <typeparam name="TSource">Type of elements in <paramref name="source"/> sequence.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="size">Size of each sub-group.</param>
        /// <returns>A sequence of equally sized chunks containing elements of the source collection.</returns>
        /// <remarks> This operator uses fully lazy execution and streams each chunk and it's contents.</remarks>
        public static IEnumerable<IEnumerable<TSource>> ChunkStream<TSource>(this IEnumerable<TSource> source, int size)
        {
            source.ThrowIfNull("source");
            size.ThrowIfNonPositive("size");

            using (var enumerator = source.GetEnumerator())
                while (enumerator.MoveNext())
                    yield return ChunkStreamIterator(enumerator, size);
        }

        private static IEnumerable<TSource> ChunkStreamIterator<TSource>(IEnumerator<TSource> source, int size)
        {
            yield return source.Current;
            for (int i = 0; i < (size - 1) && source.MoveNext(); i++)
                yield return source.Current;
        }

        public static IEnumerable<String> Chunk(this string source, int size)
        {
            var results = new List<String>();
            if (size > source.Length || size <= 0)
                size = source.Length;
            for (int i = 0; i <= source.Length - size; i += size)
                results.Add(source.Substring(i, Math.Min(size, source.Length - i)));
            return results;
        }
    }
}
