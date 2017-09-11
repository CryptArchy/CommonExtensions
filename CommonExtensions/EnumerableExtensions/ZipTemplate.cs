 

using System;
using System.Collections.Generic;

namespace CommonExtensions.EnumerableExtensions
{
    public static partial class EnumerableExt
    {

		/// <summary>Returns a projection of tuples, where each tuple contains the N-th element from each of the argument sequences.</summary>
        /// <remarks>
        /// If the two input sequences are of different lengths, the result sequence is terminated as soon as the shortest input sequence is exhausted.
        /// This operator uses deferred execution and streams its results.
        /// </remarks>
        /// <example>
        /// <code>
        /// int[] numbers = { 1, 2, 3 };
        /// string[] letters = { "A", "B", "C", "D" };
        /// var zipped = numbers.ZipShortest(letters, (n, l) => n + l);
        /// </code>
        /// The <c>zipped</c> variable, when iterated over, will yield "1A", "2B", "3C", in turn.
        /// </example>
		/// <typeparam name="TFirst">Type of elements in the first sequence.</typeparam>
		/// <typeparam name="TSecond">Type of elements in the second sequence.</typeparam>
		/// <typeparam name="TResult">Type of elements in result sequence</typeparam>
		/// <param name="first">First sequence</param>
		/// <param name="second">Second sequence</param>
		/// <param name="resultSelector">Function to apply to each group of elements</param>
		public static IEnumerable<TResult> ZipShortest<TFirst, TSecond, TResult>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second,  Func<TFirst, TSecond, TResult> resultSelector)
        {
			first.ThrowIfNull("first");
			second.ThrowIfNull("second");
            resultSelector.ThrowIfNull("resultSelector");

            return ZipIter(first, second, resultSelector, ImbalancedZipStrategy.Truncate);
        }

		/// <summary>Returns a projection of tuples, where each tuple contains the N-th element from each of the argument sequences.</summary>
        /// <remarks>
        /// If the two input sequences are of different lengths, then <see cref="InvalidOperationException"/> is thrown.
        /// This operator uses deferred execution and streams its results.
        /// </remarks>
        /// <example>
        /// <code>
        /// int[] numbers = { 1, 2, 3 };
        /// string[] letters = { "A", "B", "C", "D" };
        /// var zipped = numbers.ZipEven(letters, (n, l) => n + l);
        /// </code>
        /// The <c>zipped</c> variable, when iterated over, will yield "1A", "2B", "3C", in turn.
        /// </example>
		/// <typeparam name="TFirst">Type of elements in the first sequence.</typeparam>
		/// <typeparam name="TSecond">Type of elements in the second sequence.</typeparam>
		/// <typeparam name="TResult">Type of elements in result sequence</typeparam>
		/// <param name="first">First sequence</param>
		/// <param name="second">Second sequence</param>
		/// <param name="resultSelector">Function to apply to each group of elements</param>
		public static IEnumerable<TResult> ZipEven<TFirst, TSecond, TResult>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second,  Func<TFirst, TSecond, TResult> resultSelector)
        {
			first.ThrowIfNull("first");
			second.ThrowIfNull("second");
            resultSelector.ThrowIfNull("resultSelector");

            return ZipIter(first, second, resultSelector, ImbalancedZipStrategy.Fail);
        }

		/// <summary>Returns a projection of tuples, where each tuple contains the N-th element from each of the argument sequences.</summary>
        /// <remarks>
        /// If the two input sequences are of different lengths, then the result sequence will always be as long as the longest of the input sequences.
        /// The default values of the shorter sequence element types are used for padding.
        /// This operator uses deferred execution and streams its results.
        /// </remarks>
        /// <example>
        /// <code>
        /// int[] numbers = { 1, 2, 3 };
        /// string[] letters = { "A", "B", "C", "D" };
        /// var zipped = numbers.ZipLongest(letters, (n, l) => n + l);
        /// </code>
        /// The <c>zipped</c> variable, when iterated over, will yield "1A", "2B", "3C", in turn.
        /// </example>
		/// <typeparam name="TFirst">Type of elements in the first sequence.</typeparam>
		/// <typeparam name="TSecond">Type of elements in the second sequence.</typeparam>
		/// <typeparam name="TResult">Type of elements in result sequence</typeparam>
		/// <param name="first">First sequence</param>
		/// <param name="second">Second sequence</param>
		/// <param name="resultSelector">Function to apply to each group of elements</param>
		public static IEnumerable<TResult> ZipLongest<TFirst, TSecond, TResult>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second,  Func<TFirst, TSecond, TResult> resultSelector)
        {
			first.ThrowIfNull("first");
			second.ThrowIfNull("second");
            resultSelector.ThrowIfNull("resultSelector");

            return ZipIter(first, second, resultSelector, ImbalancedZipStrategy.Pad);
        }

		private static IEnumerable<TResult> ZipIter<TFirst, TSecond, TResult>(IEnumerable<TFirst> first, IEnumerable<TSecond> second,  Func<TFirst, TSecond, TResult> resultSelector, ImbalancedZipStrategy imbalanceStrategy)
        {
			using (var e1 = first.GetEnumerator())
			using (var e2 = second.GetEnumerator())
            {
                bool end;
				bool fail;

			    TFirst v1 = default(TFirst);
			    TSecond v2 = default(TSecond);

                while (true)
                {
                    end = true;
					fail = false;


                    if (e1.MoveNext())
                    {
                        v1 = e1.Current;
                        end = false;
                    }
                    else
                    {
                        switch (imbalanceStrategy)
                        {
                            case ImbalancedZipStrategy.Fail:
								fail = true;
								break;
                            case ImbalancedZipStrategy.Truncate:
                                yield break;
                            case ImbalancedZipStrategy.Pad:
								v1 = default(TFirst);
                                break;
                        }
                    }

                    if (e2.MoveNext())
                    {
                        v2 = e2.Current;
                        end = false;
                    }
                    else
                    {
                        switch (imbalanceStrategy)
                        {
                            case ImbalancedZipStrategy.Fail:
								fail = true;
								break;
                            case ImbalancedZipStrategy.Truncate:
                                yield break;
                            case ImbalancedZipStrategy.Pad:
								v2 = default(TSecond);
                                break;
                        }
                    }

					if (end)
						yield break;
					else if (fail)
						throw new InvalidOperationException("Sequences were not all same length.");
					else
						yield return resultSelector(v1, v2);
                }
            }            
        }

		/// <summary>Returns a projection of tuples, where each tuple contains the N-th element from each of the argument sequences.</summary>
        /// <remarks>
        /// If the two input sequences are of different lengths, the result sequence is terminated as soon as the shortest input sequence is exhausted.
		/// A global index over all sequences is passed into the result selector function.
        /// This operator uses deferred execution and streams its results.
        /// </remarks>
        /// <example>
        /// <code>
        /// int[] numbers = { 1, 2, 3 };
        /// string[] letters = { "A", "B", "C", "D" };
        /// var zipped = numbers.ZipShortest(letters, (n, l) => n + l);
        /// </code>
        /// The <c>zipped</c> variable, when iterated over, will yield "1A", "2B", "3C", in turn.
        /// </example>
		/// <typeparam name="TFirst">Type of elements in the first sequence.</typeparam>
		/// <typeparam name="TSecond">Type of elements in the second sequence.</typeparam>
		/// <typeparam name="TResult">Type of elements in result sequence</typeparam>
		/// <param name="first">First sequence</param>
		/// <param name="second">Second sequence</param>
		/// <param name="resultSelector">Function to apply to each group of elements</param>
		public static IEnumerable<TResult> ZipShortest<TFirst, TSecond, TResult>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second,  Func<TFirst, TSecond, int, TResult> resultSelector)
        {
			first.ThrowIfNull("first");
			second.ThrowIfNull("second");
            resultSelector.ThrowIfNull("resultSelector");

            return ZipIter(first, second, resultSelector, ImbalancedZipStrategy.Truncate);
        }

		/// <summary>Returns a projection of tuples, where each tuple contains the N-th element from each of the argument sequences.</summary>
        /// <remarks>
        /// If the two input sequences are of different lengths, then <see cref="InvalidOperationException"/> is thrown.
		/// A global index over all sequences is passed into the result selector function.
        /// This operator uses deferred execution and streams its results.
        /// </remarks>
        /// <example>
        /// <code>
        /// int[] numbers = { 1, 2, 3 };
        /// string[] letters = { "A", "B", "C", "D" };
        /// var zipped = numbers.ZipEven(letters, (n, l) => n + l);
        /// </code>
        /// The <c>zipped</c> variable, when iterated over, will yield "1A", "2B", "3C", in turn.
        /// </example>
		/// <typeparam name="TFirst">Type of elements in the first sequence.</typeparam>
		/// <typeparam name="TSecond">Type of elements in the second sequence.</typeparam>
		/// <typeparam name="TResult">Type of elements in result sequence</typeparam>
		/// <param name="first">First sequence</param>
		/// <param name="second">Second sequence</param>
		/// <param name="resultSelector">Function to apply to each group of elements</param>
		public static IEnumerable<TResult> ZipEven<TFirst, TSecond, TResult>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second,  Func<TFirst, TSecond, int, TResult> resultSelector)
        {
			first.ThrowIfNull("first");
			second.ThrowIfNull("second");
            resultSelector.ThrowIfNull("resultSelector");

            return ZipIter(first, second, resultSelector, ImbalancedZipStrategy.Fail);
        }

		/// <summary>Returns a projection of tuples, where each tuple contains the N-th element from each of the argument sequences.</summary>
        /// <remarks>
        /// If the two input sequences are of different lengths, then the result sequence will always be as long as the longest of the input sequences.
        /// The default values of the shorter sequence element types are used for padding.
		/// A global index over all sequences is passed into the result selector function.
        /// This operator uses deferred execution and streams its results.
        /// </remarks>
        /// <example>
        /// <code>
        /// int[] numbers = { 1, 2, 3 };
        /// string[] letters = { "A", "B", "C", "D" };
        /// var zipped = numbers.ZipLongest(letters, (n, l) => n + l);
        /// </code>
        /// The <c>zipped</c> variable, when iterated over, will yield "1A", "2B", "3C", in turn.
        /// </example>
		/// <typeparam name="TFirst">Type of elements in the first sequence.</typeparam>
		/// <typeparam name="TSecond">Type of elements in the second sequence.</typeparam>
		/// <typeparam name="TResult">Type of elements in result sequence</typeparam>
		/// <param name="first">First sequence</param>
		/// <param name="second">Second sequence</param>
		/// <param name="resultSelector">Function to apply to each group of elements</param>
		public static IEnumerable<TResult> ZipLongest<TFirst, TSecond, TResult>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second,  Func<TFirst, TSecond, int, TResult> resultSelector)
        {
			first.ThrowIfNull("first");
			second.ThrowIfNull("second");
            resultSelector.ThrowIfNull("resultSelector");

            return ZipIter(first, second, resultSelector, ImbalancedZipStrategy.Pad);
        }

		private static IEnumerable<TResult> ZipIter<TFirst, TSecond, TResult>(IEnumerable<TFirst> first, IEnumerable<TSecond> second,  Func<TFirst, TSecond, int, TResult> resultSelector, ImbalancedZipStrategy imbalanceStrategy)
        {
			var count = 0;
			using (var e1 = first.GetEnumerator())
			using (var e2 = second.GetEnumerator())
            {
                bool end;
				bool fail;

			    TFirst v1 = default(TFirst);
			    TSecond v2 = default(TSecond);

                while (true)
                {
                    end = true;
					fail = false;


                    if (e1.MoveNext())
                    {
                        v1 = e1.Current;
                        end = false;
                    }
                    else
                    {
                        switch (imbalanceStrategy)
                        {
                            case ImbalancedZipStrategy.Fail:
                                fail = true;
								break;
                            case ImbalancedZipStrategy.Truncate:
                                yield break;
                            case ImbalancedZipStrategy.Pad:
								v1 = default(TFirst);
                                break;
                        }
                    }

                    if (e2.MoveNext())
                    {
                        v2 = e2.Current;
                        end = false;
                    }
                    else
                    {
                        switch (imbalanceStrategy)
                        {
                            case ImbalancedZipStrategy.Fail:
                                fail = true;
								break;
                            case ImbalancedZipStrategy.Truncate:
                                yield break;
                            case ImbalancedZipStrategy.Pad:
								v2 = default(TSecond);
                                break;
                        }
                    }

					if (end)
						yield break;
					else if (fail)
						throw new InvalidOperationException("Sequences were not all same length.");
					else
						yield return resultSelector(v1, v2, count++);
                }
            }            
        }

		/// <summary>Returns a projection of tuples, where each tuple contains the N-th element from each of the argument sequences.</summary>
        /// <remarks>
        /// If the three input sequences are of different lengths, the result sequence is terminated as soon as the shortest input sequence is exhausted.
        /// This operator uses deferred execution and streams its results.
        /// </remarks>
        /// <example>
        /// <code>
        /// int[] numbers = { 1, 2, 3 };
        /// string[] letters = { "A", "B", "C", "D" };
        /// var zipped = numbers.ZipShortest(letters, (n, l) => n + l);
        /// </code>
        /// The <c>zipped</c> variable, when iterated over, will yield "1A", "2B", "3C", in turn.
        /// </example>
		/// <typeparam name="TFirst">Type of elements in the first sequence.</typeparam>
		/// <typeparam name="TSecond">Type of elements in the second sequence.</typeparam>
		/// <typeparam name="TThird">Type of elements in the third sequence.</typeparam>
		/// <typeparam name="TResult">Type of elements in result sequence</typeparam>
		/// <param name="first">First sequence</param>
		/// <param name="second">Second sequence</param>
		/// <param name="third">Third sequence</param>
		/// <param name="resultSelector">Function to apply to each group of elements</param>
		public static IEnumerable<TResult> ZipShortest<TFirst, TSecond, TThird, TResult>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second, IEnumerable<TThird> third,  Func<TFirst, TSecond, TThird, TResult> resultSelector)
        {
			first.ThrowIfNull("first");
			second.ThrowIfNull("second");
			third.ThrowIfNull("third");
            resultSelector.ThrowIfNull("resultSelector");

            return ZipIter(first, second, third, resultSelector, ImbalancedZipStrategy.Truncate);
        }

		/// <summary>Returns a projection of tuples, where each tuple contains the N-th element from each of the argument sequences.</summary>
        /// <remarks>
        /// If the three input sequences are of different lengths, then <see cref="InvalidOperationException"/> is thrown.
        /// This operator uses deferred execution and streams its results.
        /// </remarks>
        /// <example>
        /// <code>
        /// int[] numbers = { 1, 2, 3 };
        /// string[] letters = { "A", "B", "C", "D" };
        /// var zipped = numbers.ZipEven(letters, (n, l) => n + l);
        /// </code>
        /// The <c>zipped</c> variable, when iterated over, will yield "1A", "2B", "3C", in turn.
        /// </example>
		/// <typeparam name="TFirst">Type of elements in the first sequence.</typeparam>
		/// <typeparam name="TSecond">Type of elements in the second sequence.</typeparam>
		/// <typeparam name="TThird">Type of elements in the third sequence.</typeparam>
		/// <typeparam name="TResult">Type of elements in result sequence</typeparam>
		/// <param name="first">First sequence</param>
		/// <param name="second">Second sequence</param>
		/// <param name="third">Third sequence</param>
		/// <param name="resultSelector">Function to apply to each group of elements</param>
		public static IEnumerable<TResult> ZipEven<TFirst, TSecond, TThird, TResult>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second, IEnumerable<TThird> third,  Func<TFirst, TSecond, TThird, TResult> resultSelector)
        {
			first.ThrowIfNull("first");
			second.ThrowIfNull("second");
			third.ThrowIfNull("third");
            resultSelector.ThrowIfNull("resultSelector");

            return ZipIter(first, second, third, resultSelector, ImbalancedZipStrategy.Fail);
        }

		/// <summary>Returns a projection of tuples, where each tuple contains the N-th element from each of the argument sequences.</summary>
        /// <remarks>
        /// If the three input sequences are of different lengths, then the result sequence will always be as long as the longest of the input sequences.
        /// The default values of the shorter sequence element types are used for padding.
        /// This operator uses deferred execution and streams its results.
        /// </remarks>
        /// <example>
        /// <code>
        /// int[] numbers = { 1, 2, 3 };
        /// string[] letters = { "A", "B", "C", "D" };
        /// var zipped = numbers.ZipLongest(letters, (n, l) => n + l);
        /// </code>
        /// The <c>zipped</c> variable, when iterated over, will yield "1A", "2B", "3C", in turn.
        /// </example>
		/// <typeparam name="TFirst">Type of elements in the first sequence.</typeparam>
		/// <typeparam name="TSecond">Type of elements in the second sequence.</typeparam>
		/// <typeparam name="TThird">Type of elements in the third sequence.</typeparam>
		/// <typeparam name="TResult">Type of elements in result sequence</typeparam>
		/// <param name="first">First sequence</param>
		/// <param name="second">Second sequence</param>
		/// <param name="third">Third sequence</param>
		/// <param name="resultSelector">Function to apply to each group of elements</param>
		public static IEnumerable<TResult> ZipLongest<TFirst, TSecond, TThird, TResult>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second, IEnumerable<TThird> third,  Func<TFirst, TSecond, TThird, TResult> resultSelector)
        {
			first.ThrowIfNull("first");
			second.ThrowIfNull("second");
			third.ThrowIfNull("third");
            resultSelector.ThrowIfNull("resultSelector");

            return ZipIter(first, second, third, resultSelector, ImbalancedZipStrategy.Pad);
        }

		private static IEnumerable<TResult> ZipIter<TFirst, TSecond, TThird, TResult>(IEnumerable<TFirst> first, IEnumerable<TSecond> second, IEnumerable<TThird> third,  Func<TFirst, TSecond, TThird, TResult> resultSelector, ImbalancedZipStrategy imbalanceStrategy)
        {
			using (var e1 = first.GetEnumerator())
			using (var e2 = second.GetEnumerator())
			using (var e3 = third.GetEnumerator())
            {
                bool end;
				bool fail;

			    TFirst v1 = default(TFirst);
			    TSecond v2 = default(TSecond);
			    TThird v3 = default(TThird);

                while (true)
                {
                    end = true;
					fail = false;


                    if (e1.MoveNext())
                    {
                        v1 = e1.Current;
                        end = false;
                    }
                    else
                    {
                        switch (imbalanceStrategy)
                        {
                            case ImbalancedZipStrategy.Fail:
								fail = true;
								break;
                            case ImbalancedZipStrategy.Truncate:
                                yield break;
                            case ImbalancedZipStrategy.Pad:
								v1 = default(TFirst);
                                break;
                        }
                    }

                    if (e2.MoveNext())
                    {
                        v2 = e2.Current;
                        end = false;
                    }
                    else
                    {
                        switch (imbalanceStrategy)
                        {
                            case ImbalancedZipStrategy.Fail:
								fail = true;
								break;
                            case ImbalancedZipStrategy.Truncate:
                                yield break;
                            case ImbalancedZipStrategy.Pad:
								v2 = default(TSecond);
                                break;
                        }
                    }

                    if (e3.MoveNext())
                    {
                        v3 = e3.Current;
                        end = false;
                    }
                    else
                    {
                        switch (imbalanceStrategy)
                        {
                            case ImbalancedZipStrategy.Fail:
								fail = true;
								break;
                            case ImbalancedZipStrategy.Truncate:
                                yield break;
                            case ImbalancedZipStrategy.Pad:
								v3 = default(TThird);
                                break;
                        }
                    }

					if (end)
						yield break;
					else if (fail)
						throw new InvalidOperationException("Sequences were not all same length.");
					else
						yield return resultSelector(v1, v2, v3);
                }
            }            
        }

		/// <summary>Returns a projection of tuples, where each tuple contains the N-th element from each of the argument sequences.</summary>
        /// <remarks>
        /// If the three input sequences are of different lengths, the result sequence is terminated as soon as the shortest input sequence is exhausted.
		/// A global index over all sequences is passed into the result selector function.
        /// This operator uses deferred execution and streams its results.
        /// </remarks>
        /// <example>
        /// <code>
        /// int[] numbers = { 1, 2, 3 };
        /// string[] letters = { "A", "B", "C", "D" };
        /// var zipped = numbers.ZipShortest(letters, (n, l) => n + l);
        /// </code>
        /// The <c>zipped</c> variable, when iterated over, will yield "1A", "2B", "3C", in turn.
        /// </example>
		/// <typeparam name="TFirst">Type of elements in the first sequence.</typeparam>
		/// <typeparam name="TSecond">Type of elements in the second sequence.</typeparam>
		/// <typeparam name="TThird">Type of elements in the third sequence.</typeparam>
		/// <typeparam name="TResult">Type of elements in result sequence</typeparam>
		/// <param name="first">First sequence</param>
		/// <param name="second">Second sequence</param>
		/// <param name="third">Third sequence</param>
		/// <param name="resultSelector">Function to apply to each group of elements</param>
		public static IEnumerable<TResult> ZipShortest<TFirst, TSecond, TThird, TResult>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second, IEnumerable<TThird> third,  Func<TFirst, TSecond, TThird, int, TResult> resultSelector)
        {
			first.ThrowIfNull("first");
			second.ThrowIfNull("second");
			third.ThrowIfNull("third");
            resultSelector.ThrowIfNull("resultSelector");

            return ZipIter(first, second, third, resultSelector, ImbalancedZipStrategy.Truncate);
        }

		/// <summary>Returns a projection of tuples, where each tuple contains the N-th element from each of the argument sequences.</summary>
        /// <remarks>
        /// If the three input sequences are of different lengths, then <see cref="InvalidOperationException"/> is thrown.
		/// A global index over all sequences is passed into the result selector function.
        /// This operator uses deferred execution and streams its results.
        /// </remarks>
        /// <example>
        /// <code>
        /// int[] numbers = { 1, 2, 3 };
        /// string[] letters = { "A", "B", "C", "D" };
        /// var zipped = numbers.ZipEven(letters, (n, l) => n + l);
        /// </code>
        /// The <c>zipped</c> variable, when iterated over, will yield "1A", "2B", "3C", in turn.
        /// </example>
		/// <typeparam name="TFirst">Type of elements in the first sequence.</typeparam>
		/// <typeparam name="TSecond">Type of elements in the second sequence.</typeparam>
		/// <typeparam name="TThird">Type of elements in the third sequence.</typeparam>
		/// <typeparam name="TResult">Type of elements in result sequence</typeparam>
		/// <param name="first">First sequence</param>
		/// <param name="second">Second sequence</param>
		/// <param name="third">Third sequence</param>
		/// <param name="resultSelector">Function to apply to each group of elements</param>
		public static IEnumerable<TResult> ZipEven<TFirst, TSecond, TThird, TResult>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second, IEnumerable<TThird> third,  Func<TFirst, TSecond, TThird, int, TResult> resultSelector)
        {
			first.ThrowIfNull("first");
			second.ThrowIfNull("second");
			third.ThrowIfNull("third");
            resultSelector.ThrowIfNull("resultSelector");

            return ZipIter(first, second, third, resultSelector, ImbalancedZipStrategy.Fail);
        }

		/// <summary>Returns a projection of tuples, where each tuple contains the N-th element from each of the argument sequences.</summary>
        /// <remarks>
        /// If the three input sequences are of different lengths, then the result sequence will always be as long as the longest of the input sequences.
        /// The default values of the shorter sequence element types are used for padding.
		/// A global index over all sequences is passed into the result selector function.
        /// This operator uses deferred execution and streams its results.
        /// </remarks>
        /// <example>
        /// <code>
        /// int[] numbers = { 1, 2, 3 };
        /// string[] letters = { "A", "B", "C", "D" };
        /// var zipped = numbers.ZipLongest(letters, (n, l) => n + l);
        /// </code>
        /// The <c>zipped</c> variable, when iterated over, will yield "1A", "2B", "3C", in turn.
        /// </example>
		/// <typeparam name="TFirst">Type of elements in the first sequence.</typeparam>
		/// <typeparam name="TSecond">Type of elements in the second sequence.</typeparam>
		/// <typeparam name="TThird">Type of elements in the third sequence.</typeparam>
		/// <typeparam name="TResult">Type of elements in result sequence</typeparam>
		/// <param name="first">First sequence</param>
		/// <param name="second">Second sequence</param>
		/// <param name="third">Third sequence</param>
		/// <param name="resultSelector">Function to apply to each group of elements</param>
		public static IEnumerable<TResult> ZipLongest<TFirst, TSecond, TThird, TResult>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second, IEnumerable<TThird> third,  Func<TFirst, TSecond, TThird, int, TResult> resultSelector)
        {
			first.ThrowIfNull("first");
			second.ThrowIfNull("second");
			third.ThrowIfNull("third");
            resultSelector.ThrowIfNull("resultSelector");

            return ZipIter(first, second, third, resultSelector, ImbalancedZipStrategy.Pad);
        }

		private static IEnumerable<TResult> ZipIter<TFirst, TSecond, TThird, TResult>(IEnumerable<TFirst> first, IEnumerable<TSecond> second, IEnumerable<TThird> third,  Func<TFirst, TSecond, TThird, int, TResult> resultSelector, ImbalancedZipStrategy imbalanceStrategy)
        {
			var count = 0;
			using (var e1 = first.GetEnumerator())
			using (var e2 = second.GetEnumerator())
			using (var e3 = third.GetEnumerator())
            {
                bool end;
				bool fail;

			    TFirst v1 = default(TFirst);
			    TSecond v2 = default(TSecond);
			    TThird v3 = default(TThird);

                while (true)
                {
                    end = true;
					fail = false;


                    if (e1.MoveNext())
                    {
                        v1 = e1.Current;
                        end = false;
                    }
                    else
                    {
                        switch (imbalanceStrategy)
                        {
                            case ImbalancedZipStrategy.Fail:
                                fail = true;
								break;
                            case ImbalancedZipStrategy.Truncate:
                                yield break;
                            case ImbalancedZipStrategy.Pad:
								v1 = default(TFirst);
                                break;
                        }
                    }

                    if (e2.MoveNext())
                    {
                        v2 = e2.Current;
                        end = false;
                    }
                    else
                    {
                        switch (imbalanceStrategy)
                        {
                            case ImbalancedZipStrategy.Fail:
                                fail = true;
								break;
                            case ImbalancedZipStrategy.Truncate:
                                yield break;
                            case ImbalancedZipStrategy.Pad:
								v2 = default(TSecond);
                                break;
                        }
                    }

                    if (e3.MoveNext())
                    {
                        v3 = e3.Current;
                        end = false;
                    }
                    else
                    {
                        switch (imbalanceStrategy)
                        {
                            case ImbalancedZipStrategy.Fail:
                                fail = true;
								break;
                            case ImbalancedZipStrategy.Truncate:
                                yield break;
                            case ImbalancedZipStrategy.Pad:
								v3 = default(TThird);
                                break;
                        }
                    }

					if (end)
						yield break;
					else if (fail)
						throw new InvalidOperationException("Sequences were not all same length.");
					else
						yield return resultSelector(v1, v2, v3, count++);
                }
            }            
        }

		/// <summary>Returns a projection of tuples, where each tuple contains the N-th element from each of the argument sequences.</summary>
        /// <remarks>
        /// If the four input sequences are of different lengths, the result sequence is terminated as soon as the shortest input sequence is exhausted.
        /// This operator uses deferred execution and streams its results.
        /// </remarks>
        /// <example>
        /// <code>
        /// int[] numbers = { 1, 2, 3 };
        /// string[] letters = { "A", "B", "C", "D" };
        /// var zipped = numbers.ZipShortest(letters, (n, l) => n + l);
        /// </code>
        /// The <c>zipped</c> variable, when iterated over, will yield "1A", "2B", "3C", in turn.
        /// </example>
		/// <typeparam name="TFirst">Type of elements in the first sequence.</typeparam>
		/// <typeparam name="TSecond">Type of elements in the second sequence.</typeparam>
		/// <typeparam name="TThird">Type of elements in the third sequence.</typeparam>
		/// <typeparam name="TFourth">Type of elements in the fourth sequence.</typeparam>
		/// <typeparam name="TResult">Type of elements in result sequence</typeparam>
		/// <param name="first">First sequence</param>
		/// <param name="second">Second sequence</param>
		/// <param name="third">Third sequence</param>
		/// <param name="fourth">Fourth sequence</param>
		/// <param name="resultSelector">Function to apply to each group of elements</param>
		public static IEnumerable<TResult> ZipShortest<TFirst, TSecond, TThird, TFourth, TResult>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second, IEnumerable<TThird> third, IEnumerable<TFourth> fourth,  Func<TFirst, TSecond, TThird, TFourth, TResult> resultSelector)
        {
			first.ThrowIfNull("first");
			second.ThrowIfNull("second");
			third.ThrowIfNull("third");
			fourth.ThrowIfNull("fourth");
            resultSelector.ThrowIfNull("resultSelector");

            return ZipIter(first, second, third, fourth, resultSelector, ImbalancedZipStrategy.Truncate);
        }

		/// <summary>Returns a projection of tuples, where each tuple contains the N-th element from each of the argument sequences.</summary>
        /// <remarks>
        /// If the four input sequences are of different lengths, then <see cref="InvalidOperationException"/> is thrown.
        /// This operator uses deferred execution and streams its results.
        /// </remarks>
        /// <example>
        /// <code>
        /// int[] numbers = { 1, 2, 3 };
        /// string[] letters = { "A", "B", "C", "D" };
        /// var zipped = numbers.ZipEven(letters, (n, l) => n + l);
        /// </code>
        /// The <c>zipped</c> variable, when iterated over, will yield "1A", "2B", "3C", in turn.
        /// </example>
		/// <typeparam name="TFirst">Type of elements in the first sequence.</typeparam>
		/// <typeparam name="TSecond">Type of elements in the second sequence.</typeparam>
		/// <typeparam name="TThird">Type of elements in the third sequence.</typeparam>
		/// <typeparam name="TFourth">Type of elements in the fourth sequence.</typeparam>
		/// <typeparam name="TResult">Type of elements in result sequence</typeparam>
		/// <param name="first">First sequence</param>
		/// <param name="second">Second sequence</param>
		/// <param name="third">Third sequence</param>
		/// <param name="fourth">Fourth sequence</param>
		/// <param name="resultSelector">Function to apply to each group of elements</param>
		public static IEnumerable<TResult> ZipEven<TFirst, TSecond, TThird, TFourth, TResult>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second, IEnumerable<TThird> third, IEnumerable<TFourth> fourth,  Func<TFirst, TSecond, TThird, TFourth, TResult> resultSelector)
        {
			first.ThrowIfNull("first");
			second.ThrowIfNull("second");
			third.ThrowIfNull("third");
			fourth.ThrowIfNull("fourth");
            resultSelector.ThrowIfNull("resultSelector");

            return ZipIter(first, second, third, fourth, resultSelector, ImbalancedZipStrategy.Fail);
        }

		/// <summary>Returns a projection of tuples, where each tuple contains the N-th element from each of the argument sequences.</summary>
        /// <remarks>
        /// If the four input sequences are of different lengths, then the result sequence will always be as long as the longest of the input sequences.
        /// The default values of the shorter sequence element types are used for padding.
        /// This operator uses deferred execution and streams its results.
        /// </remarks>
        /// <example>
        /// <code>
        /// int[] numbers = { 1, 2, 3 };
        /// string[] letters = { "A", "B", "C", "D" };
        /// var zipped = numbers.ZipLongest(letters, (n, l) => n + l);
        /// </code>
        /// The <c>zipped</c> variable, when iterated over, will yield "1A", "2B", "3C", in turn.
        /// </example>
		/// <typeparam name="TFirst">Type of elements in the first sequence.</typeparam>
		/// <typeparam name="TSecond">Type of elements in the second sequence.</typeparam>
		/// <typeparam name="TThird">Type of elements in the third sequence.</typeparam>
		/// <typeparam name="TFourth">Type of elements in the fourth sequence.</typeparam>
		/// <typeparam name="TResult">Type of elements in result sequence</typeparam>
		/// <param name="first">First sequence</param>
		/// <param name="second">Second sequence</param>
		/// <param name="third">Third sequence</param>
		/// <param name="fourth">Fourth sequence</param>
		/// <param name="resultSelector">Function to apply to each group of elements</param>
		public static IEnumerable<TResult> ZipLongest<TFirst, TSecond, TThird, TFourth, TResult>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second, IEnumerable<TThird> third, IEnumerable<TFourth> fourth,  Func<TFirst, TSecond, TThird, TFourth, TResult> resultSelector)
        {
			first.ThrowIfNull("first");
			second.ThrowIfNull("second");
			third.ThrowIfNull("third");
			fourth.ThrowIfNull("fourth");
            resultSelector.ThrowIfNull("resultSelector");

            return ZipIter(first, second, third, fourth, resultSelector, ImbalancedZipStrategy.Pad);
        }

		private static IEnumerable<TResult> ZipIter<TFirst, TSecond, TThird, TFourth, TResult>(IEnumerable<TFirst> first, IEnumerable<TSecond> second, IEnumerable<TThird> third, IEnumerable<TFourth> fourth,  Func<TFirst, TSecond, TThird, TFourth, TResult> resultSelector, ImbalancedZipStrategy imbalanceStrategy)
        {
			using (var e1 = first.GetEnumerator())
			using (var e2 = second.GetEnumerator())
			using (var e3 = third.GetEnumerator())
			using (var e4 = fourth.GetEnumerator())
            {
                bool end;
				bool fail;

			    TFirst v1 = default(TFirst);
			    TSecond v2 = default(TSecond);
			    TThird v3 = default(TThird);
			    TFourth v4 = default(TFourth);

                while (true)
                {
                    end = true;
					fail = false;


                    if (e1.MoveNext())
                    {
                        v1 = e1.Current;
                        end = false;
                    }
                    else
                    {
                        switch (imbalanceStrategy)
                        {
                            case ImbalancedZipStrategy.Fail:
								fail = true;
								break;
                            case ImbalancedZipStrategy.Truncate:
                                yield break;
                            case ImbalancedZipStrategy.Pad:
								v1 = default(TFirst);
                                break;
                        }
                    }

                    if (e2.MoveNext())
                    {
                        v2 = e2.Current;
                        end = false;
                    }
                    else
                    {
                        switch (imbalanceStrategy)
                        {
                            case ImbalancedZipStrategy.Fail:
								fail = true;
								break;
                            case ImbalancedZipStrategy.Truncate:
                                yield break;
                            case ImbalancedZipStrategy.Pad:
								v2 = default(TSecond);
                                break;
                        }
                    }

                    if (e3.MoveNext())
                    {
                        v3 = e3.Current;
                        end = false;
                    }
                    else
                    {
                        switch (imbalanceStrategy)
                        {
                            case ImbalancedZipStrategy.Fail:
								fail = true;
								break;
                            case ImbalancedZipStrategy.Truncate:
                                yield break;
                            case ImbalancedZipStrategy.Pad:
								v3 = default(TThird);
                                break;
                        }
                    }

                    if (e4.MoveNext())
                    {
                        v4 = e4.Current;
                        end = false;
                    }
                    else
                    {
                        switch (imbalanceStrategy)
                        {
                            case ImbalancedZipStrategy.Fail:
								fail = true;
								break;
                            case ImbalancedZipStrategy.Truncate:
                                yield break;
                            case ImbalancedZipStrategy.Pad:
								v4 = default(TFourth);
                                break;
                        }
                    }

					if (end)
						yield break;
					else if (fail)
						throw new InvalidOperationException("Sequences were not all same length.");
					else
						yield return resultSelector(v1, v2, v3, v4);
                }
            }            
        }

		/// <summary>Returns a projection of tuples, where each tuple contains the N-th element from each of the argument sequences.</summary>
        /// <remarks>
        /// If the four input sequences are of different lengths, the result sequence is terminated as soon as the shortest input sequence is exhausted.
		/// A global index over all sequences is passed into the result selector function.
        /// This operator uses deferred execution and streams its results.
        /// </remarks>
        /// <example>
        /// <code>
        /// int[] numbers = { 1, 2, 3 };
        /// string[] letters = { "A", "B", "C", "D" };
        /// var zipped = numbers.ZipShortest(letters, (n, l) => n + l);
        /// </code>
        /// The <c>zipped</c> variable, when iterated over, will yield "1A", "2B", "3C", in turn.
        /// </example>
		/// <typeparam name="TFirst">Type of elements in the first sequence.</typeparam>
		/// <typeparam name="TSecond">Type of elements in the second sequence.</typeparam>
		/// <typeparam name="TThird">Type of elements in the third sequence.</typeparam>
		/// <typeparam name="TFourth">Type of elements in the fourth sequence.</typeparam>
		/// <typeparam name="TResult">Type of elements in result sequence</typeparam>
		/// <param name="first">First sequence</param>
		/// <param name="second">Second sequence</param>
		/// <param name="third">Third sequence</param>
		/// <param name="fourth">Fourth sequence</param>
		/// <param name="resultSelector">Function to apply to each group of elements</param>
		public static IEnumerable<TResult> ZipShortest<TFirst, TSecond, TThird, TFourth, TResult>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second, IEnumerable<TThird> third, IEnumerable<TFourth> fourth,  Func<TFirst, TSecond, TThird, TFourth, int, TResult> resultSelector)
        {
			first.ThrowIfNull("first");
			second.ThrowIfNull("second");
			third.ThrowIfNull("third");
			fourth.ThrowIfNull("fourth");
            resultSelector.ThrowIfNull("resultSelector");

            return ZipIter(first, second, third, fourth, resultSelector, ImbalancedZipStrategy.Truncate);
        }

		/// <summary>Returns a projection of tuples, where each tuple contains the N-th element from each of the argument sequences.</summary>
        /// <remarks>
        /// If the four input sequences are of different lengths, then <see cref="InvalidOperationException"/> is thrown.
		/// A global index over all sequences is passed into the result selector function.
        /// This operator uses deferred execution and streams its results.
        /// </remarks>
        /// <example>
        /// <code>
        /// int[] numbers = { 1, 2, 3 };
        /// string[] letters = { "A", "B", "C", "D" };
        /// var zipped = numbers.ZipEven(letters, (n, l) => n + l);
        /// </code>
        /// The <c>zipped</c> variable, when iterated over, will yield "1A", "2B", "3C", in turn.
        /// </example>
		/// <typeparam name="TFirst">Type of elements in the first sequence.</typeparam>
		/// <typeparam name="TSecond">Type of elements in the second sequence.</typeparam>
		/// <typeparam name="TThird">Type of elements in the third sequence.</typeparam>
		/// <typeparam name="TFourth">Type of elements in the fourth sequence.</typeparam>
		/// <typeparam name="TResult">Type of elements in result sequence</typeparam>
		/// <param name="first">First sequence</param>
		/// <param name="second">Second sequence</param>
		/// <param name="third">Third sequence</param>
		/// <param name="fourth">Fourth sequence</param>
		/// <param name="resultSelector">Function to apply to each group of elements</param>
		public static IEnumerable<TResult> ZipEven<TFirst, TSecond, TThird, TFourth, TResult>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second, IEnumerable<TThird> third, IEnumerable<TFourth> fourth,  Func<TFirst, TSecond, TThird, TFourth, int, TResult> resultSelector)
        {
			first.ThrowIfNull("first");
			second.ThrowIfNull("second");
			third.ThrowIfNull("third");
			fourth.ThrowIfNull("fourth");
            resultSelector.ThrowIfNull("resultSelector");

            return ZipIter(first, second, third, fourth, resultSelector, ImbalancedZipStrategy.Fail);
        }

		/// <summary>Returns a projection of tuples, where each tuple contains the N-th element from each of the argument sequences.</summary>
        /// <remarks>
        /// If the four input sequences are of different lengths, then the result sequence will always be as long as the longest of the input sequences.
        /// The default values of the shorter sequence element types are used for padding.
		/// A global index over all sequences is passed into the result selector function.
        /// This operator uses deferred execution and streams its results.
        /// </remarks>
        /// <example>
        /// <code>
        /// int[] numbers = { 1, 2, 3 };
        /// string[] letters = { "A", "B", "C", "D" };
        /// var zipped = numbers.ZipLongest(letters, (n, l) => n + l);
        /// </code>
        /// The <c>zipped</c> variable, when iterated over, will yield "1A", "2B", "3C", in turn.
        /// </example>
		/// <typeparam name="TFirst">Type of elements in the first sequence.</typeparam>
		/// <typeparam name="TSecond">Type of elements in the second sequence.</typeparam>
		/// <typeparam name="TThird">Type of elements in the third sequence.</typeparam>
		/// <typeparam name="TFourth">Type of elements in the fourth sequence.</typeparam>
		/// <typeparam name="TResult">Type of elements in result sequence</typeparam>
		/// <param name="first">First sequence</param>
		/// <param name="second">Second sequence</param>
		/// <param name="third">Third sequence</param>
		/// <param name="fourth">Fourth sequence</param>
		/// <param name="resultSelector">Function to apply to each group of elements</param>
		public static IEnumerable<TResult> ZipLongest<TFirst, TSecond, TThird, TFourth, TResult>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second, IEnumerable<TThird> third, IEnumerable<TFourth> fourth,  Func<TFirst, TSecond, TThird, TFourth, int, TResult> resultSelector)
        {
			first.ThrowIfNull("first");
			second.ThrowIfNull("second");
			third.ThrowIfNull("third");
			fourth.ThrowIfNull("fourth");
            resultSelector.ThrowIfNull("resultSelector");

            return ZipIter(first, second, third, fourth, resultSelector, ImbalancedZipStrategy.Pad);
        }

		private static IEnumerable<TResult> ZipIter<TFirst, TSecond, TThird, TFourth, TResult>(IEnumerable<TFirst> first, IEnumerable<TSecond> second, IEnumerable<TThird> third, IEnumerable<TFourth> fourth,  Func<TFirst, TSecond, TThird, TFourth, int, TResult> resultSelector, ImbalancedZipStrategy imbalanceStrategy)
        {
			var count = 0;
			using (var e1 = first.GetEnumerator())
			using (var e2 = second.GetEnumerator())
			using (var e3 = third.GetEnumerator())
			using (var e4 = fourth.GetEnumerator())
            {
                bool end;
				bool fail;

			    TFirst v1 = default(TFirst);
			    TSecond v2 = default(TSecond);
			    TThird v3 = default(TThird);
			    TFourth v4 = default(TFourth);

                while (true)
                {
                    end = true;
					fail = false;


                    if (e1.MoveNext())
                    {
                        v1 = e1.Current;
                        end = false;
                    }
                    else
                    {
                        switch (imbalanceStrategy)
                        {
                            case ImbalancedZipStrategy.Fail:
                                fail = true;
								break;
                            case ImbalancedZipStrategy.Truncate:
                                yield break;
                            case ImbalancedZipStrategy.Pad:
								v1 = default(TFirst);
                                break;
                        }
                    }

                    if (e2.MoveNext())
                    {
                        v2 = e2.Current;
                        end = false;
                    }
                    else
                    {
                        switch (imbalanceStrategy)
                        {
                            case ImbalancedZipStrategy.Fail:
                                fail = true;
								break;
                            case ImbalancedZipStrategy.Truncate:
                                yield break;
                            case ImbalancedZipStrategy.Pad:
								v2 = default(TSecond);
                                break;
                        }
                    }

                    if (e3.MoveNext())
                    {
                        v3 = e3.Current;
                        end = false;
                    }
                    else
                    {
                        switch (imbalanceStrategy)
                        {
                            case ImbalancedZipStrategy.Fail:
                                fail = true;
								break;
                            case ImbalancedZipStrategy.Truncate:
                                yield break;
                            case ImbalancedZipStrategy.Pad:
								v3 = default(TThird);
                                break;
                        }
                    }

                    if (e4.MoveNext())
                    {
                        v4 = e4.Current;
                        end = false;
                    }
                    else
                    {
                        switch (imbalanceStrategy)
                        {
                            case ImbalancedZipStrategy.Fail:
                                fail = true;
								break;
                            case ImbalancedZipStrategy.Truncate:
                                yield break;
                            case ImbalancedZipStrategy.Pad:
								v4 = default(TFourth);
                                break;
                        }
                    }

					if (end)
						yield break;
					else if (fail)
						throw new InvalidOperationException("Sequences were not all same length.");
					else
						yield return resultSelector(v1, v2, v3, v4, count++);
                }
            }            
        }

		/// <summary>Returns a projection of tuples, where each tuple contains the N-th element from each of the argument sequences.</summary>
        /// <remarks>
        /// If the five input sequences are of different lengths, the result sequence is terminated as soon as the shortest input sequence is exhausted.
        /// This operator uses deferred execution and streams its results.
        /// </remarks>
        /// <example>
        /// <code>
        /// int[] numbers = { 1, 2, 3 };
        /// string[] letters = { "A", "B", "C", "D" };
        /// var zipped = numbers.ZipShortest(letters, (n, l) => n + l);
        /// </code>
        /// The <c>zipped</c> variable, when iterated over, will yield "1A", "2B", "3C", in turn.
        /// </example>
		/// <typeparam name="TFirst">Type of elements in the first sequence.</typeparam>
		/// <typeparam name="TSecond">Type of elements in the second sequence.</typeparam>
		/// <typeparam name="TThird">Type of elements in the third sequence.</typeparam>
		/// <typeparam name="TFourth">Type of elements in the fourth sequence.</typeparam>
		/// <typeparam name="TFifth">Type of elements in the fifth sequence.</typeparam>
		/// <typeparam name="TResult">Type of elements in result sequence</typeparam>
		/// <param name="first">First sequence</param>
		/// <param name="second">Second sequence</param>
		/// <param name="third">Third sequence</param>
		/// <param name="fourth">Fourth sequence</param>
		/// <param name="fifth">Fifth sequence</param>
		/// <param name="resultSelector">Function to apply to each group of elements</param>
		public static IEnumerable<TResult> ZipShortest<TFirst, TSecond, TThird, TFourth, TFifth, TResult>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second, IEnumerable<TThird> third, IEnumerable<TFourth> fourth, IEnumerable<TFifth> fifth,  Func<TFirst, TSecond, TThird, TFourth, TFifth, TResult> resultSelector)
        {
			first.ThrowIfNull("first");
			second.ThrowIfNull("second");
			third.ThrowIfNull("third");
			fourth.ThrowIfNull("fourth");
			fifth.ThrowIfNull("fifth");
            resultSelector.ThrowIfNull("resultSelector");

            return ZipIter(first, second, third, fourth, fifth, resultSelector, ImbalancedZipStrategy.Truncate);
        }

		/// <summary>Returns a projection of tuples, where each tuple contains the N-th element from each of the argument sequences.</summary>
        /// <remarks>
        /// If the five input sequences are of different lengths, then <see cref="InvalidOperationException"/> is thrown.
        /// This operator uses deferred execution and streams its results.
        /// </remarks>
        /// <example>
        /// <code>
        /// int[] numbers = { 1, 2, 3 };
        /// string[] letters = { "A", "B", "C", "D" };
        /// var zipped = numbers.ZipEven(letters, (n, l) => n + l);
        /// </code>
        /// The <c>zipped</c> variable, when iterated over, will yield "1A", "2B", "3C", in turn.
        /// </example>
		/// <typeparam name="TFirst">Type of elements in the first sequence.</typeparam>
		/// <typeparam name="TSecond">Type of elements in the second sequence.</typeparam>
		/// <typeparam name="TThird">Type of elements in the third sequence.</typeparam>
		/// <typeparam name="TFourth">Type of elements in the fourth sequence.</typeparam>
		/// <typeparam name="TFifth">Type of elements in the fifth sequence.</typeparam>
		/// <typeparam name="TResult">Type of elements in result sequence</typeparam>
		/// <param name="first">First sequence</param>
		/// <param name="second">Second sequence</param>
		/// <param name="third">Third sequence</param>
		/// <param name="fourth">Fourth sequence</param>
		/// <param name="fifth">Fifth sequence</param>
		/// <param name="resultSelector">Function to apply to each group of elements</param>
		public static IEnumerable<TResult> ZipEven<TFirst, TSecond, TThird, TFourth, TFifth, TResult>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second, IEnumerable<TThird> third, IEnumerable<TFourth> fourth, IEnumerable<TFifth> fifth,  Func<TFirst, TSecond, TThird, TFourth, TFifth, TResult> resultSelector)
        {
			first.ThrowIfNull("first");
			second.ThrowIfNull("second");
			third.ThrowIfNull("third");
			fourth.ThrowIfNull("fourth");
			fifth.ThrowIfNull("fifth");
            resultSelector.ThrowIfNull("resultSelector");

            return ZipIter(first, second, third, fourth, fifth, resultSelector, ImbalancedZipStrategy.Fail);
        }

		/// <summary>Returns a projection of tuples, where each tuple contains the N-th element from each of the argument sequences.</summary>
        /// <remarks>
        /// If the five input sequences are of different lengths, then the result sequence will always be as long as the longest of the input sequences.
        /// The default values of the shorter sequence element types are used for padding.
        /// This operator uses deferred execution and streams its results.
        /// </remarks>
        /// <example>
        /// <code>
        /// int[] numbers = { 1, 2, 3 };
        /// string[] letters = { "A", "B", "C", "D" };
        /// var zipped = numbers.ZipLongest(letters, (n, l) => n + l);
        /// </code>
        /// The <c>zipped</c> variable, when iterated over, will yield "1A", "2B", "3C", in turn.
        /// </example>
		/// <typeparam name="TFirst">Type of elements in the first sequence.</typeparam>
		/// <typeparam name="TSecond">Type of elements in the second sequence.</typeparam>
		/// <typeparam name="TThird">Type of elements in the third sequence.</typeparam>
		/// <typeparam name="TFourth">Type of elements in the fourth sequence.</typeparam>
		/// <typeparam name="TFifth">Type of elements in the fifth sequence.</typeparam>
		/// <typeparam name="TResult">Type of elements in result sequence</typeparam>
		/// <param name="first">First sequence</param>
		/// <param name="second">Second sequence</param>
		/// <param name="third">Third sequence</param>
		/// <param name="fourth">Fourth sequence</param>
		/// <param name="fifth">Fifth sequence</param>
		/// <param name="resultSelector">Function to apply to each group of elements</param>
		public static IEnumerable<TResult> ZipLongest<TFirst, TSecond, TThird, TFourth, TFifth, TResult>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second, IEnumerable<TThird> third, IEnumerable<TFourth> fourth, IEnumerable<TFifth> fifth,  Func<TFirst, TSecond, TThird, TFourth, TFifth, TResult> resultSelector)
        {
			first.ThrowIfNull("first");
			second.ThrowIfNull("second");
			third.ThrowIfNull("third");
			fourth.ThrowIfNull("fourth");
			fifth.ThrowIfNull("fifth");
            resultSelector.ThrowIfNull("resultSelector");

            return ZipIter(first, second, third, fourth, fifth, resultSelector, ImbalancedZipStrategy.Pad);
        }

		private static IEnumerable<TResult> ZipIter<TFirst, TSecond, TThird, TFourth, TFifth, TResult>(IEnumerable<TFirst> first, IEnumerable<TSecond> second, IEnumerable<TThird> third, IEnumerable<TFourth> fourth, IEnumerable<TFifth> fifth,  Func<TFirst, TSecond, TThird, TFourth, TFifth, TResult> resultSelector, ImbalancedZipStrategy imbalanceStrategy)
        {
			using (var e1 = first.GetEnumerator())
			using (var e2 = second.GetEnumerator())
			using (var e3 = third.GetEnumerator())
			using (var e4 = fourth.GetEnumerator())
			using (var e5 = fifth.GetEnumerator())
            {
                bool end;
				bool fail;

			    TFirst v1 = default(TFirst);
			    TSecond v2 = default(TSecond);
			    TThird v3 = default(TThird);
			    TFourth v4 = default(TFourth);
			    TFifth v5 = default(TFifth);

                while (true)
                {
                    end = true;
					fail = false;


                    if (e1.MoveNext())
                    {
                        v1 = e1.Current;
                        end = false;
                    }
                    else
                    {
                        switch (imbalanceStrategy)
                        {
                            case ImbalancedZipStrategy.Fail:
								fail = true;
								break;
                            case ImbalancedZipStrategy.Truncate:
                                yield break;
                            case ImbalancedZipStrategy.Pad:
								v1 = default(TFirst);
                                break;
                        }
                    }

                    if (e2.MoveNext())
                    {
                        v2 = e2.Current;
                        end = false;
                    }
                    else
                    {
                        switch (imbalanceStrategy)
                        {
                            case ImbalancedZipStrategy.Fail:
								fail = true;
								break;
                            case ImbalancedZipStrategy.Truncate:
                                yield break;
                            case ImbalancedZipStrategy.Pad:
								v2 = default(TSecond);
                                break;
                        }
                    }

                    if (e3.MoveNext())
                    {
                        v3 = e3.Current;
                        end = false;
                    }
                    else
                    {
                        switch (imbalanceStrategy)
                        {
                            case ImbalancedZipStrategy.Fail:
								fail = true;
								break;
                            case ImbalancedZipStrategy.Truncate:
                                yield break;
                            case ImbalancedZipStrategy.Pad:
								v3 = default(TThird);
                                break;
                        }
                    }

                    if (e4.MoveNext())
                    {
                        v4 = e4.Current;
                        end = false;
                    }
                    else
                    {
                        switch (imbalanceStrategy)
                        {
                            case ImbalancedZipStrategy.Fail:
								fail = true;
								break;
                            case ImbalancedZipStrategy.Truncate:
                                yield break;
                            case ImbalancedZipStrategy.Pad:
								v4 = default(TFourth);
                                break;
                        }
                    }

                    if (e5.MoveNext())
                    {
                        v5 = e5.Current;
                        end = false;
                    }
                    else
                    {
                        switch (imbalanceStrategy)
                        {
                            case ImbalancedZipStrategy.Fail:
								fail = true;
								break;
                            case ImbalancedZipStrategy.Truncate:
                                yield break;
                            case ImbalancedZipStrategy.Pad:
								v5 = default(TFifth);
                                break;
                        }
                    }

					if (end)
						yield break;
					else if (fail)
						throw new InvalidOperationException("Sequences were not all same length.");
					else
						yield return resultSelector(v1, v2, v3, v4, v5);
                }
            }            
        }

		/// <summary>Returns a projection of tuples, where each tuple contains the N-th element from each of the argument sequences.</summary>
        /// <remarks>
        /// If the five input sequences are of different lengths, the result sequence is terminated as soon as the shortest input sequence is exhausted.
		/// A global index over all sequences is passed into the result selector function.
        /// This operator uses deferred execution and streams its results.
        /// </remarks>
        /// <example>
        /// <code>
        /// int[] numbers = { 1, 2, 3 };
        /// string[] letters = { "A", "B", "C", "D" };
        /// var zipped = numbers.ZipShortest(letters, (n, l) => n + l);
        /// </code>
        /// The <c>zipped</c> variable, when iterated over, will yield "1A", "2B", "3C", in turn.
        /// </example>
		/// <typeparam name="TFirst">Type of elements in the first sequence.</typeparam>
		/// <typeparam name="TSecond">Type of elements in the second sequence.</typeparam>
		/// <typeparam name="TThird">Type of elements in the third sequence.</typeparam>
		/// <typeparam name="TFourth">Type of elements in the fourth sequence.</typeparam>
		/// <typeparam name="TFifth">Type of elements in the fifth sequence.</typeparam>
		/// <typeparam name="TResult">Type of elements in result sequence</typeparam>
		/// <param name="first">First sequence</param>
		/// <param name="second">Second sequence</param>
		/// <param name="third">Third sequence</param>
		/// <param name="fourth">Fourth sequence</param>
		/// <param name="fifth">Fifth sequence</param>
		/// <param name="resultSelector">Function to apply to each group of elements</param>
		public static IEnumerable<TResult> ZipShortest<TFirst, TSecond, TThird, TFourth, TFifth, TResult>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second, IEnumerable<TThird> third, IEnumerable<TFourth> fourth, IEnumerable<TFifth> fifth,  Func<TFirst, TSecond, TThird, TFourth, TFifth, int, TResult> resultSelector)
        {
			first.ThrowIfNull("first");
			second.ThrowIfNull("second");
			third.ThrowIfNull("third");
			fourth.ThrowIfNull("fourth");
			fifth.ThrowIfNull("fifth");
            resultSelector.ThrowIfNull("resultSelector");

            return ZipIter(first, second, third, fourth, fifth, resultSelector, ImbalancedZipStrategy.Truncate);
        }

		/// <summary>Returns a projection of tuples, where each tuple contains the N-th element from each of the argument sequences.</summary>
        /// <remarks>
        /// If the five input sequences are of different lengths, then <see cref="InvalidOperationException"/> is thrown.
		/// A global index over all sequences is passed into the result selector function.
        /// This operator uses deferred execution and streams its results.
        /// </remarks>
        /// <example>
        /// <code>
        /// int[] numbers = { 1, 2, 3 };
        /// string[] letters = { "A", "B", "C", "D" };
        /// var zipped = numbers.ZipEven(letters, (n, l) => n + l);
        /// </code>
        /// The <c>zipped</c> variable, when iterated over, will yield "1A", "2B", "3C", in turn.
        /// </example>
		/// <typeparam name="TFirst">Type of elements in the first sequence.</typeparam>
		/// <typeparam name="TSecond">Type of elements in the second sequence.</typeparam>
		/// <typeparam name="TThird">Type of elements in the third sequence.</typeparam>
		/// <typeparam name="TFourth">Type of elements in the fourth sequence.</typeparam>
		/// <typeparam name="TFifth">Type of elements in the fifth sequence.</typeparam>
		/// <typeparam name="TResult">Type of elements in result sequence</typeparam>
		/// <param name="first">First sequence</param>
		/// <param name="second">Second sequence</param>
		/// <param name="third">Third sequence</param>
		/// <param name="fourth">Fourth sequence</param>
		/// <param name="fifth">Fifth sequence</param>
		/// <param name="resultSelector">Function to apply to each group of elements</param>
		public static IEnumerable<TResult> ZipEven<TFirst, TSecond, TThird, TFourth, TFifth, TResult>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second, IEnumerable<TThird> third, IEnumerable<TFourth> fourth, IEnumerable<TFifth> fifth,  Func<TFirst, TSecond, TThird, TFourth, TFifth, int, TResult> resultSelector)
        {
			first.ThrowIfNull("first");
			second.ThrowIfNull("second");
			third.ThrowIfNull("third");
			fourth.ThrowIfNull("fourth");
			fifth.ThrowIfNull("fifth");
            resultSelector.ThrowIfNull("resultSelector");

            return ZipIter(first, second, third, fourth, fifth, resultSelector, ImbalancedZipStrategy.Fail);
        }

		/// <summary>Returns a projection of tuples, where each tuple contains the N-th element from each of the argument sequences.</summary>
        /// <remarks>
        /// If the five input sequences are of different lengths, then the result sequence will always be as long as the longest of the input sequences.
        /// The default values of the shorter sequence element types are used for padding.
		/// A global index over all sequences is passed into the result selector function.
        /// This operator uses deferred execution and streams its results.
        /// </remarks>
        /// <example>
        /// <code>
        /// int[] numbers = { 1, 2, 3 };
        /// string[] letters = { "A", "B", "C", "D" };
        /// var zipped = numbers.ZipLongest(letters, (n, l) => n + l);
        /// </code>
        /// The <c>zipped</c> variable, when iterated over, will yield "1A", "2B", "3C", in turn.
        /// </example>
		/// <typeparam name="TFirst">Type of elements in the first sequence.</typeparam>
		/// <typeparam name="TSecond">Type of elements in the second sequence.</typeparam>
		/// <typeparam name="TThird">Type of elements in the third sequence.</typeparam>
		/// <typeparam name="TFourth">Type of elements in the fourth sequence.</typeparam>
		/// <typeparam name="TFifth">Type of elements in the fifth sequence.</typeparam>
		/// <typeparam name="TResult">Type of elements in result sequence</typeparam>
		/// <param name="first">First sequence</param>
		/// <param name="second">Second sequence</param>
		/// <param name="third">Third sequence</param>
		/// <param name="fourth">Fourth sequence</param>
		/// <param name="fifth">Fifth sequence</param>
		/// <param name="resultSelector">Function to apply to each group of elements</param>
		public static IEnumerable<TResult> ZipLongest<TFirst, TSecond, TThird, TFourth, TFifth, TResult>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second, IEnumerable<TThird> third, IEnumerable<TFourth> fourth, IEnumerable<TFifth> fifth,  Func<TFirst, TSecond, TThird, TFourth, TFifth, int, TResult> resultSelector)
        {
			first.ThrowIfNull("first");
			second.ThrowIfNull("second");
			third.ThrowIfNull("third");
			fourth.ThrowIfNull("fourth");
			fifth.ThrowIfNull("fifth");
            resultSelector.ThrowIfNull("resultSelector");

            return ZipIter(first, second, third, fourth, fifth, resultSelector, ImbalancedZipStrategy.Pad);
        }

		private static IEnumerable<TResult> ZipIter<TFirst, TSecond, TThird, TFourth, TFifth, TResult>(IEnumerable<TFirst> first, IEnumerable<TSecond> second, IEnumerable<TThird> third, IEnumerable<TFourth> fourth, IEnumerable<TFifth> fifth,  Func<TFirst, TSecond, TThird, TFourth, TFifth, int, TResult> resultSelector, ImbalancedZipStrategy imbalanceStrategy)
        {
			var count = 0;
			using (var e1 = first.GetEnumerator())
			using (var e2 = second.GetEnumerator())
			using (var e3 = third.GetEnumerator())
			using (var e4 = fourth.GetEnumerator())
			using (var e5 = fifth.GetEnumerator())
            {
                bool end;
				bool fail;

			    TFirst v1 = default(TFirst);
			    TSecond v2 = default(TSecond);
			    TThird v3 = default(TThird);
			    TFourth v4 = default(TFourth);
			    TFifth v5 = default(TFifth);

                while (true)
                {
                    end = true;
					fail = false;


                    if (e1.MoveNext())
                    {
                        v1 = e1.Current;
                        end = false;
                    }
                    else
                    {
                        switch (imbalanceStrategy)
                        {
                            case ImbalancedZipStrategy.Fail:
                                fail = true;
								break;
                            case ImbalancedZipStrategy.Truncate:
                                yield break;
                            case ImbalancedZipStrategy.Pad:
								v1 = default(TFirst);
                                break;
                        }
                    }

                    if (e2.MoveNext())
                    {
                        v2 = e2.Current;
                        end = false;
                    }
                    else
                    {
                        switch (imbalanceStrategy)
                        {
                            case ImbalancedZipStrategy.Fail:
                                fail = true;
								break;
                            case ImbalancedZipStrategy.Truncate:
                                yield break;
                            case ImbalancedZipStrategy.Pad:
								v2 = default(TSecond);
                                break;
                        }
                    }

                    if (e3.MoveNext())
                    {
                        v3 = e3.Current;
                        end = false;
                    }
                    else
                    {
                        switch (imbalanceStrategy)
                        {
                            case ImbalancedZipStrategy.Fail:
                                fail = true;
								break;
                            case ImbalancedZipStrategy.Truncate:
                                yield break;
                            case ImbalancedZipStrategy.Pad:
								v3 = default(TThird);
                                break;
                        }
                    }

                    if (e4.MoveNext())
                    {
                        v4 = e4.Current;
                        end = false;
                    }
                    else
                    {
                        switch (imbalanceStrategy)
                        {
                            case ImbalancedZipStrategy.Fail:
                                fail = true;
								break;
                            case ImbalancedZipStrategy.Truncate:
                                yield break;
                            case ImbalancedZipStrategy.Pad:
								v4 = default(TFourth);
                                break;
                        }
                    }

                    if (e5.MoveNext())
                    {
                        v5 = e5.Current;
                        end = false;
                    }
                    else
                    {
                        switch (imbalanceStrategy)
                        {
                            case ImbalancedZipStrategy.Fail:
                                fail = true;
								break;
                            case ImbalancedZipStrategy.Truncate:
                                yield break;
                            case ImbalancedZipStrategy.Pad:
								v5 = default(TFifth);
                                break;
                        }
                    }

					if (end)
						yield break;
					else if (fail)
						throw new InvalidOperationException("Sequences were not all same length.");
					else
						yield return resultSelector(v1, v2, v3, v4, v5, count++);
                }
            }            
        }

		/// <summary>Returns a projection of tuples, where each tuple contains the N-th element from each of the argument sequences.</summary>
        /// <remarks>
        /// If the six input sequences are of different lengths, the result sequence is terminated as soon as the shortest input sequence is exhausted.
        /// This operator uses deferred execution and streams its results.
        /// </remarks>
        /// <example>
        /// <code>
        /// int[] numbers = { 1, 2, 3 };
        /// string[] letters = { "A", "B", "C", "D" };
        /// var zipped = numbers.ZipShortest(letters, (n, l) => n + l);
        /// </code>
        /// The <c>zipped</c> variable, when iterated over, will yield "1A", "2B", "3C", in turn.
        /// </example>
		/// <typeparam name="TFirst">Type of elements in the first sequence.</typeparam>
		/// <typeparam name="TSecond">Type of elements in the second sequence.</typeparam>
		/// <typeparam name="TThird">Type of elements in the third sequence.</typeparam>
		/// <typeparam name="TFourth">Type of elements in the fourth sequence.</typeparam>
		/// <typeparam name="TFifth">Type of elements in the fifth sequence.</typeparam>
		/// <typeparam name="TSixth">Type of elements in the sixth sequence.</typeparam>
		/// <typeparam name="TResult">Type of elements in result sequence</typeparam>
		/// <param name="first">First sequence</param>
		/// <param name="second">Second sequence</param>
		/// <param name="third">Third sequence</param>
		/// <param name="fourth">Fourth sequence</param>
		/// <param name="fifth">Fifth sequence</param>
		/// <param name="sixth">Sixth sequence</param>
		/// <param name="resultSelector">Function to apply to each group of elements</param>
		public static IEnumerable<TResult> ZipShortest<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TResult>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second, IEnumerable<TThird> third, IEnumerable<TFourth> fourth, IEnumerable<TFifth> fifth, IEnumerable<TSixth> sixth,  Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TResult> resultSelector)
        {
			first.ThrowIfNull("first");
			second.ThrowIfNull("second");
			third.ThrowIfNull("third");
			fourth.ThrowIfNull("fourth");
			fifth.ThrowIfNull("fifth");
			sixth.ThrowIfNull("sixth");
            resultSelector.ThrowIfNull("resultSelector");

            return ZipIter(first, second, third, fourth, fifth, sixth, resultSelector, ImbalancedZipStrategy.Truncate);
        }

		/// <summary>Returns a projection of tuples, where each tuple contains the N-th element from each of the argument sequences.</summary>
        /// <remarks>
        /// If the six input sequences are of different lengths, then <see cref="InvalidOperationException"/> is thrown.
        /// This operator uses deferred execution and streams its results.
        /// </remarks>
        /// <example>
        /// <code>
        /// int[] numbers = { 1, 2, 3 };
        /// string[] letters = { "A", "B", "C", "D" };
        /// var zipped = numbers.ZipEven(letters, (n, l) => n + l);
        /// </code>
        /// The <c>zipped</c> variable, when iterated over, will yield "1A", "2B", "3C", in turn.
        /// </example>
		/// <typeparam name="TFirst">Type of elements in the first sequence.</typeparam>
		/// <typeparam name="TSecond">Type of elements in the second sequence.</typeparam>
		/// <typeparam name="TThird">Type of elements in the third sequence.</typeparam>
		/// <typeparam name="TFourth">Type of elements in the fourth sequence.</typeparam>
		/// <typeparam name="TFifth">Type of elements in the fifth sequence.</typeparam>
		/// <typeparam name="TSixth">Type of elements in the sixth sequence.</typeparam>
		/// <typeparam name="TResult">Type of elements in result sequence</typeparam>
		/// <param name="first">First sequence</param>
		/// <param name="second">Second sequence</param>
		/// <param name="third">Third sequence</param>
		/// <param name="fourth">Fourth sequence</param>
		/// <param name="fifth">Fifth sequence</param>
		/// <param name="sixth">Sixth sequence</param>
		/// <param name="resultSelector">Function to apply to each group of elements</param>
		public static IEnumerable<TResult> ZipEven<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TResult>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second, IEnumerable<TThird> third, IEnumerable<TFourth> fourth, IEnumerable<TFifth> fifth, IEnumerable<TSixth> sixth,  Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TResult> resultSelector)
        {
			first.ThrowIfNull("first");
			second.ThrowIfNull("second");
			third.ThrowIfNull("third");
			fourth.ThrowIfNull("fourth");
			fifth.ThrowIfNull("fifth");
			sixth.ThrowIfNull("sixth");
            resultSelector.ThrowIfNull("resultSelector");

            return ZipIter(first, second, third, fourth, fifth, sixth, resultSelector, ImbalancedZipStrategy.Fail);
        }

		/// <summary>Returns a projection of tuples, where each tuple contains the N-th element from each of the argument sequences.</summary>
        /// <remarks>
        /// If the six input sequences are of different lengths, then the result sequence will always be as long as the longest of the input sequences.
        /// The default values of the shorter sequence element types are used for padding.
        /// This operator uses deferred execution and streams its results.
        /// </remarks>
        /// <example>
        /// <code>
        /// int[] numbers = { 1, 2, 3 };
        /// string[] letters = { "A", "B", "C", "D" };
        /// var zipped = numbers.ZipLongest(letters, (n, l) => n + l);
        /// </code>
        /// The <c>zipped</c> variable, when iterated over, will yield "1A", "2B", "3C", in turn.
        /// </example>
		/// <typeparam name="TFirst">Type of elements in the first sequence.</typeparam>
		/// <typeparam name="TSecond">Type of elements in the second sequence.</typeparam>
		/// <typeparam name="TThird">Type of elements in the third sequence.</typeparam>
		/// <typeparam name="TFourth">Type of elements in the fourth sequence.</typeparam>
		/// <typeparam name="TFifth">Type of elements in the fifth sequence.</typeparam>
		/// <typeparam name="TSixth">Type of elements in the sixth sequence.</typeparam>
		/// <typeparam name="TResult">Type of elements in result sequence</typeparam>
		/// <param name="first">First sequence</param>
		/// <param name="second">Second sequence</param>
		/// <param name="third">Third sequence</param>
		/// <param name="fourth">Fourth sequence</param>
		/// <param name="fifth">Fifth sequence</param>
		/// <param name="sixth">Sixth sequence</param>
		/// <param name="resultSelector">Function to apply to each group of elements</param>
		public static IEnumerable<TResult> ZipLongest<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TResult>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second, IEnumerable<TThird> third, IEnumerable<TFourth> fourth, IEnumerable<TFifth> fifth, IEnumerable<TSixth> sixth,  Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TResult> resultSelector)
        {
			first.ThrowIfNull("first");
			second.ThrowIfNull("second");
			third.ThrowIfNull("third");
			fourth.ThrowIfNull("fourth");
			fifth.ThrowIfNull("fifth");
			sixth.ThrowIfNull("sixth");
            resultSelector.ThrowIfNull("resultSelector");

            return ZipIter(first, second, third, fourth, fifth, sixth, resultSelector, ImbalancedZipStrategy.Pad);
        }

		private static IEnumerable<TResult> ZipIter<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TResult>(IEnumerable<TFirst> first, IEnumerable<TSecond> second, IEnumerable<TThird> third, IEnumerable<TFourth> fourth, IEnumerable<TFifth> fifth, IEnumerable<TSixth> sixth,  Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TResult> resultSelector, ImbalancedZipStrategy imbalanceStrategy)
        {
			using (var e1 = first.GetEnumerator())
			using (var e2 = second.GetEnumerator())
			using (var e3 = third.GetEnumerator())
			using (var e4 = fourth.GetEnumerator())
			using (var e5 = fifth.GetEnumerator())
			using (var e6 = sixth.GetEnumerator())
            {
                bool end;
				bool fail;

			    TFirst v1 = default(TFirst);
			    TSecond v2 = default(TSecond);
			    TThird v3 = default(TThird);
			    TFourth v4 = default(TFourth);
			    TFifth v5 = default(TFifth);
			    TSixth v6 = default(TSixth);

                while (true)
                {
                    end = true;
					fail = false;


                    if (e1.MoveNext())
                    {
                        v1 = e1.Current;
                        end = false;
                    }
                    else
                    {
                        switch (imbalanceStrategy)
                        {
                            case ImbalancedZipStrategy.Fail:
								fail = true;
								break;
                            case ImbalancedZipStrategy.Truncate:
                                yield break;
                            case ImbalancedZipStrategy.Pad:
								v1 = default(TFirst);
                                break;
                        }
                    }

                    if (e2.MoveNext())
                    {
                        v2 = e2.Current;
                        end = false;
                    }
                    else
                    {
                        switch (imbalanceStrategy)
                        {
                            case ImbalancedZipStrategy.Fail:
								fail = true;
								break;
                            case ImbalancedZipStrategy.Truncate:
                                yield break;
                            case ImbalancedZipStrategy.Pad:
								v2 = default(TSecond);
                                break;
                        }
                    }

                    if (e3.MoveNext())
                    {
                        v3 = e3.Current;
                        end = false;
                    }
                    else
                    {
                        switch (imbalanceStrategy)
                        {
                            case ImbalancedZipStrategy.Fail:
								fail = true;
								break;
                            case ImbalancedZipStrategy.Truncate:
                                yield break;
                            case ImbalancedZipStrategy.Pad:
								v3 = default(TThird);
                                break;
                        }
                    }

                    if (e4.MoveNext())
                    {
                        v4 = e4.Current;
                        end = false;
                    }
                    else
                    {
                        switch (imbalanceStrategy)
                        {
                            case ImbalancedZipStrategy.Fail:
								fail = true;
								break;
                            case ImbalancedZipStrategy.Truncate:
                                yield break;
                            case ImbalancedZipStrategy.Pad:
								v4 = default(TFourth);
                                break;
                        }
                    }

                    if (e5.MoveNext())
                    {
                        v5 = e5.Current;
                        end = false;
                    }
                    else
                    {
                        switch (imbalanceStrategy)
                        {
                            case ImbalancedZipStrategy.Fail:
								fail = true;
								break;
                            case ImbalancedZipStrategy.Truncate:
                                yield break;
                            case ImbalancedZipStrategy.Pad:
								v5 = default(TFifth);
                                break;
                        }
                    }

                    if (e6.MoveNext())
                    {
                        v6 = e6.Current;
                        end = false;
                    }
                    else
                    {
                        switch (imbalanceStrategy)
                        {
                            case ImbalancedZipStrategy.Fail:
								fail = true;
								break;
                            case ImbalancedZipStrategy.Truncate:
                                yield break;
                            case ImbalancedZipStrategy.Pad:
								v6 = default(TSixth);
                                break;
                        }
                    }

					if (end)
						yield break;
					else if (fail)
						throw new InvalidOperationException("Sequences were not all same length.");
					else
						yield return resultSelector(v1, v2, v3, v4, v5, v6);
                }
            }            
        }

		/// <summary>Returns a projection of tuples, where each tuple contains the N-th element from each of the argument sequences.</summary>
        /// <remarks>
        /// If the six input sequences are of different lengths, the result sequence is terminated as soon as the shortest input sequence is exhausted.
		/// A global index over all sequences is passed into the result selector function.
        /// This operator uses deferred execution and streams its results.
        /// </remarks>
        /// <example>
        /// <code>
        /// int[] numbers = { 1, 2, 3 };
        /// string[] letters = { "A", "B", "C", "D" };
        /// var zipped = numbers.ZipShortest(letters, (n, l) => n + l);
        /// </code>
        /// The <c>zipped</c> variable, when iterated over, will yield "1A", "2B", "3C", in turn.
        /// </example>
		/// <typeparam name="TFirst">Type of elements in the first sequence.</typeparam>
		/// <typeparam name="TSecond">Type of elements in the second sequence.</typeparam>
		/// <typeparam name="TThird">Type of elements in the third sequence.</typeparam>
		/// <typeparam name="TFourth">Type of elements in the fourth sequence.</typeparam>
		/// <typeparam name="TFifth">Type of elements in the fifth sequence.</typeparam>
		/// <typeparam name="TSixth">Type of elements in the sixth sequence.</typeparam>
		/// <typeparam name="TResult">Type of elements in result sequence</typeparam>
		/// <param name="first">First sequence</param>
		/// <param name="second">Second sequence</param>
		/// <param name="third">Third sequence</param>
		/// <param name="fourth">Fourth sequence</param>
		/// <param name="fifth">Fifth sequence</param>
		/// <param name="sixth">Sixth sequence</param>
		/// <param name="resultSelector">Function to apply to each group of elements</param>
		public static IEnumerable<TResult> ZipShortest<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TResult>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second, IEnumerable<TThird> third, IEnumerable<TFourth> fourth, IEnumerable<TFifth> fifth, IEnumerable<TSixth> sixth,  Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, int, TResult> resultSelector)
        {
			first.ThrowIfNull("first");
			second.ThrowIfNull("second");
			third.ThrowIfNull("third");
			fourth.ThrowIfNull("fourth");
			fifth.ThrowIfNull("fifth");
			sixth.ThrowIfNull("sixth");
            resultSelector.ThrowIfNull("resultSelector");

            return ZipIter(first, second, third, fourth, fifth, sixth, resultSelector, ImbalancedZipStrategy.Truncate);
        }

		/// <summary>Returns a projection of tuples, where each tuple contains the N-th element from each of the argument sequences.</summary>
        /// <remarks>
        /// If the six input sequences are of different lengths, then <see cref="InvalidOperationException"/> is thrown.
		/// A global index over all sequences is passed into the result selector function.
        /// This operator uses deferred execution and streams its results.
        /// </remarks>
        /// <example>
        /// <code>
        /// int[] numbers = { 1, 2, 3 };
        /// string[] letters = { "A", "B", "C", "D" };
        /// var zipped = numbers.ZipEven(letters, (n, l) => n + l);
        /// </code>
        /// The <c>zipped</c> variable, when iterated over, will yield "1A", "2B", "3C", in turn.
        /// </example>
		/// <typeparam name="TFirst">Type of elements in the first sequence.</typeparam>
		/// <typeparam name="TSecond">Type of elements in the second sequence.</typeparam>
		/// <typeparam name="TThird">Type of elements in the third sequence.</typeparam>
		/// <typeparam name="TFourth">Type of elements in the fourth sequence.</typeparam>
		/// <typeparam name="TFifth">Type of elements in the fifth sequence.</typeparam>
		/// <typeparam name="TSixth">Type of elements in the sixth sequence.</typeparam>
		/// <typeparam name="TResult">Type of elements in result sequence</typeparam>
		/// <param name="first">First sequence</param>
		/// <param name="second">Second sequence</param>
		/// <param name="third">Third sequence</param>
		/// <param name="fourth">Fourth sequence</param>
		/// <param name="fifth">Fifth sequence</param>
		/// <param name="sixth">Sixth sequence</param>
		/// <param name="resultSelector">Function to apply to each group of elements</param>
		public static IEnumerable<TResult> ZipEven<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TResult>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second, IEnumerable<TThird> third, IEnumerable<TFourth> fourth, IEnumerable<TFifth> fifth, IEnumerable<TSixth> sixth,  Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, int, TResult> resultSelector)
        {
			first.ThrowIfNull("first");
			second.ThrowIfNull("second");
			third.ThrowIfNull("third");
			fourth.ThrowIfNull("fourth");
			fifth.ThrowIfNull("fifth");
			sixth.ThrowIfNull("sixth");
            resultSelector.ThrowIfNull("resultSelector");

            return ZipIter(first, second, third, fourth, fifth, sixth, resultSelector, ImbalancedZipStrategy.Fail);
        }

		/// <summary>Returns a projection of tuples, where each tuple contains the N-th element from each of the argument sequences.</summary>
        /// <remarks>
        /// If the six input sequences are of different lengths, then the result sequence will always be as long as the longest of the input sequences.
        /// The default values of the shorter sequence element types are used for padding.
		/// A global index over all sequences is passed into the result selector function.
        /// This operator uses deferred execution and streams its results.
        /// </remarks>
        /// <example>
        /// <code>
        /// int[] numbers = { 1, 2, 3 };
        /// string[] letters = { "A", "B", "C", "D" };
        /// var zipped = numbers.ZipLongest(letters, (n, l) => n + l);
        /// </code>
        /// The <c>zipped</c> variable, when iterated over, will yield "1A", "2B", "3C", in turn.
        /// </example>
		/// <typeparam name="TFirst">Type of elements in the first sequence.</typeparam>
		/// <typeparam name="TSecond">Type of elements in the second sequence.</typeparam>
		/// <typeparam name="TThird">Type of elements in the third sequence.</typeparam>
		/// <typeparam name="TFourth">Type of elements in the fourth sequence.</typeparam>
		/// <typeparam name="TFifth">Type of elements in the fifth sequence.</typeparam>
		/// <typeparam name="TSixth">Type of elements in the sixth sequence.</typeparam>
		/// <typeparam name="TResult">Type of elements in result sequence</typeparam>
		/// <param name="first">First sequence</param>
		/// <param name="second">Second sequence</param>
		/// <param name="third">Third sequence</param>
		/// <param name="fourth">Fourth sequence</param>
		/// <param name="fifth">Fifth sequence</param>
		/// <param name="sixth">Sixth sequence</param>
		/// <param name="resultSelector">Function to apply to each group of elements</param>
		public static IEnumerable<TResult> ZipLongest<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TResult>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second, IEnumerable<TThird> third, IEnumerable<TFourth> fourth, IEnumerable<TFifth> fifth, IEnumerable<TSixth> sixth,  Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, int, TResult> resultSelector)
        {
			first.ThrowIfNull("first");
			second.ThrowIfNull("second");
			third.ThrowIfNull("third");
			fourth.ThrowIfNull("fourth");
			fifth.ThrowIfNull("fifth");
			sixth.ThrowIfNull("sixth");
            resultSelector.ThrowIfNull("resultSelector");

            return ZipIter(first, second, third, fourth, fifth, sixth, resultSelector, ImbalancedZipStrategy.Pad);
        }

		private static IEnumerable<TResult> ZipIter<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TResult>(IEnumerable<TFirst> first, IEnumerable<TSecond> second, IEnumerable<TThird> third, IEnumerable<TFourth> fourth, IEnumerable<TFifth> fifth, IEnumerable<TSixth> sixth,  Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, int, TResult> resultSelector, ImbalancedZipStrategy imbalanceStrategy)
        {
			var count = 0;
			using (var e1 = first.GetEnumerator())
			using (var e2 = second.GetEnumerator())
			using (var e3 = third.GetEnumerator())
			using (var e4 = fourth.GetEnumerator())
			using (var e5 = fifth.GetEnumerator())
			using (var e6 = sixth.GetEnumerator())
            {
                bool end;
				bool fail;

			    TFirst v1 = default(TFirst);
			    TSecond v2 = default(TSecond);
			    TThird v3 = default(TThird);
			    TFourth v4 = default(TFourth);
			    TFifth v5 = default(TFifth);
			    TSixth v6 = default(TSixth);

                while (true)
                {
                    end = true;
					fail = false;


                    if (e1.MoveNext())
                    {
                        v1 = e1.Current;
                        end = false;
                    }
                    else
                    {
                        switch (imbalanceStrategy)
                        {
                            case ImbalancedZipStrategy.Fail:
                                fail = true;
								break;
                            case ImbalancedZipStrategy.Truncate:
                                yield break;
                            case ImbalancedZipStrategy.Pad:
								v1 = default(TFirst);
                                break;
                        }
                    }

                    if (e2.MoveNext())
                    {
                        v2 = e2.Current;
                        end = false;
                    }
                    else
                    {
                        switch (imbalanceStrategy)
                        {
                            case ImbalancedZipStrategy.Fail:
                                fail = true;
								break;
                            case ImbalancedZipStrategy.Truncate:
                                yield break;
                            case ImbalancedZipStrategy.Pad:
								v2 = default(TSecond);
                                break;
                        }
                    }

                    if (e3.MoveNext())
                    {
                        v3 = e3.Current;
                        end = false;
                    }
                    else
                    {
                        switch (imbalanceStrategy)
                        {
                            case ImbalancedZipStrategy.Fail:
                                fail = true;
								break;
                            case ImbalancedZipStrategy.Truncate:
                                yield break;
                            case ImbalancedZipStrategy.Pad:
								v3 = default(TThird);
                                break;
                        }
                    }

                    if (e4.MoveNext())
                    {
                        v4 = e4.Current;
                        end = false;
                    }
                    else
                    {
                        switch (imbalanceStrategy)
                        {
                            case ImbalancedZipStrategy.Fail:
                                fail = true;
								break;
                            case ImbalancedZipStrategy.Truncate:
                                yield break;
                            case ImbalancedZipStrategy.Pad:
								v4 = default(TFourth);
                                break;
                        }
                    }

                    if (e5.MoveNext())
                    {
                        v5 = e5.Current;
                        end = false;
                    }
                    else
                    {
                        switch (imbalanceStrategy)
                        {
                            case ImbalancedZipStrategy.Fail:
                                fail = true;
								break;
                            case ImbalancedZipStrategy.Truncate:
                                yield break;
                            case ImbalancedZipStrategy.Pad:
								v5 = default(TFifth);
                                break;
                        }
                    }

                    if (e6.MoveNext())
                    {
                        v6 = e6.Current;
                        end = false;
                    }
                    else
                    {
                        switch (imbalanceStrategy)
                        {
                            case ImbalancedZipStrategy.Fail:
                                fail = true;
								break;
                            case ImbalancedZipStrategy.Truncate:
                                yield break;
                            case ImbalancedZipStrategy.Pad:
								v6 = default(TSixth);
                                break;
                        }
                    }

					if (end)
						yield break;
					else if (fail)
						throw new InvalidOperationException("Sequences were not all same length.");
					else
						yield return resultSelector(v1, v2, v3, v4, v5, v6, count++);
                }
            }            
        }

		/// <summary>Returns a projection of tuples, where each tuple contains the N-th element from each of the argument sequences.</summary>
        /// <remarks>
        /// If the seven input sequences are of different lengths, the result sequence is terminated as soon as the shortest input sequence is exhausted.
        /// This operator uses deferred execution and streams its results.
        /// </remarks>
        /// <example>
        /// <code>
        /// int[] numbers = { 1, 2, 3 };
        /// string[] letters = { "A", "B", "C", "D" };
        /// var zipped = numbers.ZipShortest(letters, (n, l) => n + l);
        /// </code>
        /// The <c>zipped</c> variable, when iterated over, will yield "1A", "2B", "3C", in turn.
        /// </example>
		/// <typeparam name="TFirst">Type of elements in the first sequence.</typeparam>
		/// <typeparam name="TSecond">Type of elements in the second sequence.</typeparam>
		/// <typeparam name="TThird">Type of elements in the third sequence.</typeparam>
		/// <typeparam name="TFourth">Type of elements in the fourth sequence.</typeparam>
		/// <typeparam name="TFifth">Type of elements in the fifth sequence.</typeparam>
		/// <typeparam name="TSixth">Type of elements in the sixth sequence.</typeparam>
		/// <typeparam name="TSeventh">Type of elements in the seventh sequence.</typeparam>
		/// <typeparam name="TResult">Type of elements in result sequence</typeparam>
		/// <param name="first">First sequence</param>
		/// <param name="second">Second sequence</param>
		/// <param name="third">Third sequence</param>
		/// <param name="fourth">Fourth sequence</param>
		/// <param name="fifth">Fifth sequence</param>
		/// <param name="sixth">Sixth sequence</param>
		/// <param name="seventh">Seventh sequence</param>
		/// <param name="resultSelector">Function to apply to each group of elements</param>
		public static IEnumerable<TResult> ZipShortest<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TResult>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second, IEnumerable<TThird> third, IEnumerable<TFourth> fourth, IEnumerable<TFifth> fifth, IEnumerable<TSixth> sixth, IEnumerable<TSeventh> seventh,  Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TResult> resultSelector)
        {
			first.ThrowIfNull("first");
			second.ThrowIfNull("second");
			third.ThrowIfNull("third");
			fourth.ThrowIfNull("fourth");
			fifth.ThrowIfNull("fifth");
			sixth.ThrowIfNull("sixth");
			seventh.ThrowIfNull("seventh");
            resultSelector.ThrowIfNull("resultSelector");

            return ZipIter(first, second, third, fourth, fifth, sixth, seventh, resultSelector, ImbalancedZipStrategy.Truncate);
        }

		/// <summary>Returns a projection of tuples, where each tuple contains the N-th element from each of the argument sequences.</summary>
        /// <remarks>
        /// If the seven input sequences are of different lengths, then <see cref="InvalidOperationException"/> is thrown.
        /// This operator uses deferred execution and streams its results.
        /// </remarks>
        /// <example>
        /// <code>
        /// int[] numbers = { 1, 2, 3 };
        /// string[] letters = { "A", "B", "C", "D" };
        /// var zipped = numbers.ZipEven(letters, (n, l) => n + l);
        /// </code>
        /// The <c>zipped</c> variable, when iterated over, will yield "1A", "2B", "3C", in turn.
        /// </example>
		/// <typeparam name="TFirst">Type of elements in the first sequence.</typeparam>
		/// <typeparam name="TSecond">Type of elements in the second sequence.</typeparam>
		/// <typeparam name="TThird">Type of elements in the third sequence.</typeparam>
		/// <typeparam name="TFourth">Type of elements in the fourth sequence.</typeparam>
		/// <typeparam name="TFifth">Type of elements in the fifth sequence.</typeparam>
		/// <typeparam name="TSixth">Type of elements in the sixth sequence.</typeparam>
		/// <typeparam name="TSeventh">Type of elements in the seventh sequence.</typeparam>
		/// <typeparam name="TResult">Type of elements in result sequence</typeparam>
		/// <param name="first">First sequence</param>
		/// <param name="second">Second sequence</param>
		/// <param name="third">Third sequence</param>
		/// <param name="fourth">Fourth sequence</param>
		/// <param name="fifth">Fifth sequence</param>
		/// <param name="sixth">Sixth sequence</param>
		/// <param name="seventh">Seventh sequence</param>
		/// <param name="resultSelector">Function to apply to each group of elements</param>
		public static IEnumerable<TResult> ZipEven<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TResult>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second, IEnumerable<TThird> third, IEnumerable<TFourth> fourth, IEnumerable<TFifth> fifth, IEnumerable<TSixth> sixth, IEnumerable<TSeventh> seventh,  Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TResult> resultSelector)
        {
			first.ThrowIfNull("first");
			second.ThrowIfNull("second");
			third.ThrowIfNull("third");
			fourth.ThrowIfNull("fourth");
			fifth.ThrowIfNull("fifth");
			sixth.ThrowIfNull("sixth");
			seventh.ThrowIfNull("seventh");
            resultSelector.ThrowIfNull("resultSelector");

            return ZipIter(first, second, third, fourth, fifth, sixth, seventh, resultSelector, ImbalancedZipStrategy.Fail);
        }

		/// <summary>Returns a projection of tuples, where each tuple contains the N-th element from each of the argument sequences.</summary>
        /// <remarks>
        /// If the seven input sequences are of different lengths, then the result sequence will always be as long as the longest of the input sequences.
        /// The default values of the shorter sequence element types are used for padding.
        /// This operator uses deferred execution and streams its results.
        /// </remarks>
        /// <example>
        /// <code>
        /// int[] numbers = { 1, 2, 3 };
        /// string[] letters = { "A", "B", "C", "D" };
        /// var zipped = numbers.ZipLongest(letters, (n, l) => n + l);
        /// </code>
        /// The <c>zipped</c> variable, when iterated over, will yield "1A", "2B", "3C", in turn.
        /// </example>
		/// <typeparam name="TFirst">Type of elements in the first sequence.</typeparam>
		/// <typeparam name="TSecond">Type of elements in the second sequence.</typeparam>
		/// <typeparam name="TThird">Type of elements in the third sequence.</typeparam>
		/// <typeparam name="TFourth">Type of elements in the fourth sequence.</typeparam>
		/// <typeparam name="TFifth">Type of elements in the fifth sequence.</typeparam>
		/// <typeparam name="TSixth">Type of elements in the sixth sequence.</typeparam>
		/// <typeparam name="TSeventh">Type of elements in the seventh sequence.</typeparam>
		/// <typeparam name="TResult">Type of elements in result sequence</typeparam>
		/// <param name="first">First sequence</param>
		/// <param name="second">Second sequence</param>
		/// <param name="third">Third sequence</param>
		/// <param name="fourth">Fourth sequence</param>
		/// <param name="fifth">Fifth sequence</param>
		/// <param name="sixth">Sixth sequence</param>
		/// <param name="seventh">Seventh sequence</param>
		/// <param name="resultSelector">Function to apply to each group of elements</param>
		public static IEnumerable<TResult> ZipLongest<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TResult>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second, IEnumerable<TThird> third, IEnumerable<TFourth> fourth, IEnumerable<TFifth> fifth, IEnumerable<TSixth> sixth, IEnumerable<TSeventh> seventh,  Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TResult> resultSelector)
        {
			first.ThrowIfNull("first");
			second.ThrowIfNull("second");
			third.ThrowIfNull("third");
			fourth.ThrowIfNull("fourth");
			fifth.ThrowIfNull("fifth");
			sixth.ThrowIfNull("sixth");
			seventh.ThrowIfNull("seventh");
            resultSelector.ThrowIfNull("resultSelector");

            return ZipIter(first, second, third, fourth, fifth, sixth, seventh, resultSelector, ImbalancedZipStrategy.Pad);
        }

		private static IEnumerable<TResult> ZipIter<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TResult>(IEnumerable<TFirst> first, IEnumerable<TSecond> second, IEnumerable<TThird> third, IEnumerable<TFourth> fourth, IEnumerable<TFifth> fifth, IEnumerable<TSixth> sixth, IEnumerable<TSeventh> seventh,  Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TResult> resultSelector, ImbalancedZipStrategy imbalanceStrategy)
        {
			using (var e1 = first.GetEnumerator())
			using (var e2 = second.GetEnumerator())
			using (var e3 = third.GetEnumerator())
			using (var e4 = fourth.GetEnumerator())
			using (var e5 = fifth.GetEnumerator())
			using (var e6 = sixth.GetEnumerator())
			using (var e7 = seventh.GetEnumerator())
            {
                bool end;
				bool fail;

			    TFirst v1 = default(TFirst);
			    TSecond v2 = default(TSecond);
			    TThird v3 = default(TThird);
			    TFourth v4 = default(TFourth);
			    TFifth v5 = default(TFifth);
			    TSixth v6 = default(TSixth);
			    TSeventh v7 = default(TSeventh);

                while (true)
                {
                    end = true;
					fail = false;


                    if (e1.MoveNext())
                    {
                        v1 = e1.Current;
                        end = false;
                    }
                    else
                    {
                        switch (imbalanceStrategy)
                        {
                            case ImbalancedZipStrategy.Fail:
								fail = true;
								break;
                            case ImbalancedZipStrategy.Truncate:
                                yield break;
                            case ImbalancedZipStrategy.Pad:
								v1 = default(TFirst);
                                break;
                        }
                    }

                    if (e2.MoveNext())
                    {
                        v2 = e2.Current;
                        end = false;
                    }
                    else
                    {
                        switch (imbalanceStrategy)
                        {
                            case ImbalancedZipStrategy.Fail:
								fail = true;
								break;
                            case ImbalancedZipStrategy.Truncate:
                                yield break;
                            case ImbalancedZipStrategy.Pad:
								v2 = default(TSecond);
                                break;
                        }
                    }

                    if (e3.MoveNext())
                    {
                        v3 = e3.Current;
                        end = false;
                    }
                    else
                    {
                        switch (imbalanceStrategy)
                        {
                            case ImbalancedZipStrategy.Fail:
								fail = true;
								break;
                            case ImbalancedZipStrategy.Truncate:
                                yield break;
                            case ImbalancedZipStrategy.Pad:
								v3 = default(TThird);
                                break;
                        }
                    }

                    if (e4.MoveNext())
                    {
                        v4 = e4.Current;
                        end = false;
                    }
                    else
                    {
                        switch (imbalanceStrategy)
                        {
                            case ImbalancedZipStrategy.Fail:
								fail = true;
								break;
                            case ImbalancedZipStrategy.Truncate:
                                yield break;
                            case ImbalancedZipStrategy.Pad:
								v4 = default(TFourth);
                                break;
                        }
                    }

                    if (e5.MoveNext())
                    {
                        v5 = e5.Current;
                        end = false;
                    }
                    else
                    {
                        switch (imbalanceStrategy)
                        {
                            case ImbalancedZipStrategy.Fail:
								fail = true;
								break;
                            case ImbalancedZipStrategy.Truncate:
                                yield break;
                            case ImbalancedZipStrategy.Pad:
								v5 = default(TFifth);
                                break;
                        }
                    }

                    if (e6.MoveNext())
                    {
                        v6 = e6.Current;
                        end = false;
                    }
                    else
                    {
                        switch (imbalanceStrategy)
                        {
                            case ImbalancedZipStrategy.Fail:
								fail = true;
								break;
                            case ImbalancedZipStrategy.Truncate:
                                yield break;
                            case ImbalancedZipStrategy.Pad:
								v6 = default(TSixth);
                                break;
                        }
                    }

                    if (e7.MoveNext())
                    {
                        v7 = e7.Current;
                        end = false;
                    }
                    else
                    {
                        switch (imbalanceStrategy)
                        {
                            case ImbalancedZipStrategy.Fail:
								fail = true;
								break;
                            case ImbalancedZipStrategy.Truncate:
                                yield break;
                            case ImbalancedZipStrategy.Pad:
								v7 = default(TSeventh);
                                break;
                        }
                    }

					if (end)
						yield break;
					else if (fail)
						throw new InvalidOperationException("Sequences were not all same length.");
					else
						yield return resultSelector(v1, v2, v3, v4, v5, v6, v7);
                }
            }            
        }

		/// <summary>Returns a projection of tuples, where each tuple contains the N-th element from each of the argument sequences.</summary>
        /// <remarks>
        /// If the seven input sequences are of different lengths, the result sequence is terminated as soon as the shortest input sequence is exhausted.
		/// A global index over all sequences is passed into the result selector function.
        /// This operator uses deferred execution and streams its results.
        /// </remarks>
        /// <example>
        /// <code>
        /// int[] numbers = { 1, 2, 3 };
        /// string[] letters = { "A", "B", "C", "D" };
        /// var zipped = numbers.ZipShortest(letters, (n, l) => n + l);
        /// </code>
        /// The <c>zipped</c> variable, when iterated over, will yield "1A", "2B", "3C", in turn.
        /// </example>
		/// <typeparam name="TFirst">Type of elements in the first sequence.</typeparam>
		/// <typeparam name="TSecond">Type of elements in the second sequence.</typeparam>
		/// <typeparam name="TThird">Type of elements in the third sequence.</typeparam>
		/// <typeparam name="TFourth">Type of elements in the fourth sequence.</typeparam>
		/// <typeparam name="TFifth">Type of elements in the fifth sequence.</typeparam>
		/// <typeparam name="TSixth">Type of elements in the sixth sequence.</typeparam>
		/// <typeparam name="TSeventh">Type of elements in the seventh sequence.</typeparam>
		/// <typeparam name="TResult">Type of elements in result sequence</typeparam>
		/// <param name="first">First sequence</param>
		/// <param name="second">Second sequence</param>
		/// <param name="third">Third sequence</param>
		/// <param name="fourth">Fourth sequence</param>
		/// <param name="fifth">Fifth sequence</param>
		/// <param name="sixth">Sixth sequence</param>
		/// <param name="seventh">Seventh sequence</param>
		/// <param name="resultSelector">Function to apply to each group of elements</param>
		public static IEnumerable<TResult> ZipShortest<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TResult>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second, IEnumerable<TThird> third, IEnumerable<TFourth> fourth, IEnumerable<TFifth> fifth, IEnumerable<TSixth> sixth, IEnumerable<TSeventh> seventh,  Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, int, TResult> resultSelector)
        {
			first.ThrowIfNull("first");
			second.ThrowIfNull("second");
			third.ThrowIfNull("third");
			fourth.ThrowIfNull("fourth");
			fifth.ThrowIfNull("fifth");
			sixth.ThrowIfNull("sixth");
			seventh.ThrowIfNull("seventh");
            resultSelector.ThrowIfNull("resultSelector");

            return ZipIter(first, second, third, fourth, fifth, sixth, seventh, resultSelector, ImbalancedZipStrategy.Truncate);
        }

		/// <summary>Returns a projection of tuples, where each tuple contains the N-th element from each of the argument sequences.</summary>
        /// <remarks>
        /// If the seven input sequences are of different lengths, then <see cref="InvalidOperationException"/> is thrown.
		/// A global index over all sequences is passed into the result selector function.
        /// This operator uses deferred execution and streams its results.
        /// </remarks>
        /// <example>
        /// <code>
        /// int[] numbers = { 1, 2, 3 };
        /// string[] letters = { "A", "B", "C", "D" };
        /// var zipped = numbers.ZipEven(letters, (n, l) => n + l);
        /// </code>
        /// The <c>zipped</c> variable, when iterated over, will yield "1A", "2B", "3C", in turn.
        /// </example>
		/// <typeparam name="TFirst">Type of elements in the first sequence.</typeparam>
		/// <typeparam name="TSecond">Type of elements in the second sequence.</typeparam>
		/// <typeparam name="TThird">Type of elements in the third sequence.</typeparam>
		/// <typeparam name="TFourth">Type of elements in the fourth sequence.</typeparam>
		/// <typeparam name="TFifth">Type of elements in the fifth sequence.</typeparam>
		/// <typeparam name="TSixth">Type of elements in the sixth sequence.</typeparam>
		/// <typeparam name="TSeventh">Type of elements in the seventh sequence.</typeparam>
		/// <typeparam name="TResult">Type of elements in result sequence</typeparam>
		/// <param name="first">First sequence</param>
		/// <param name="second">Second sequence</param>
		/// <param name="third">Third sequence</param>
		/// <param name="fourth">Fourth sequence</param>
		/// <param name="fifth">Fifth sequence</param>
		/// <param name="sixth">Sixth sequence</param>
		/// <param name="seventh">Seventh sequence</param>
		/// <param name="resultSelector">Function to apply to each group of elements</param>
		public static IEnumerable<TResult> ZipEven<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TResult>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second, IEnumerable<TThird> third, IEnumerable<TFourth> fourth, IEnumerable<TFifth> fifth, IEnumerable<TSixth> sixth, IEnumerable<TSeventh> seventh,  Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, int, TResult> resultSelector)
        {
			first.ThrowIfNull("first");
			second.ThrowIfNull("second");
			third.ThrowIfNull("third");
			fourth.ThrowIfNull("fourth");
			fifth.ThrowIfNull("fifth");
			sixth.ThrowIfNull("sixth");
			seventh.ThrowIfNull("seventh");
            resultSelector.ThrowIfNull("resultSelector");

            return ZipIter(first, second, third, fourth, fifth, sixth, seventh, resultSelector, ImbalancedZipStrategy.Fail);
        }

		/// <summary>Returns a projection of tuples, where each tuple contains the N-th element from each of the argument sequences.</summary>
        /// <remarks>
        /// If the seven input sequences are of different lengths, then the result sequence will always be as long as the longest of the input sequences.
        /// The default values of the shorter sequence element types are used for padding.
		/// A global index over all sequences is passed into the result selector function.
        /// This operator uses deferred execution and streams its results.
        /// </remarks>
        /// <example>
        /// <code>
        /// int[] numbers = { 1, 2, 3 };
        /// string[] letters = { "A", "B", "C", "D" };
        /// var zipped = numbers.ZipLongest(letters, (n, l) => n + l);
        /// </code>
        /// The <c>zipped</c> variable, when iterated over, will yield "1A", "2B", "3C", in turn.
        /// </example>
		/// <typeparam name="TFirst">Type of elements in the first sequence.</typeparam>
		/// <typeparam name="TSecond">Type of elements in the second sequence.</typeparam>
		/// <typeparam name="TThird">Type of elements in the third sequence.</typeparam>
		/// <typeparam name="TFourth">Type of elements in the fourth sequence.</typeparam>
		/// <typeparam name="TFifth">Type of elements in the fifth sequence.</typeparam>
		/// <typeparam name="TSixth">Type of elements in the sixth sequence.</typeparam>
		/// <typeparam name="TSeventh">Type of elements in the seventh sequence.</typeparam>
		/// <typeparam name="TResult">Type of elements in result sequence</typeparam>
		/// <param name="first">First sequence</param>
		/// <param name="second">Second sequence</param>
		/// <param name="third">Third sequence</param>
		/// <param name="fourth">Fourth sequence</param>
		/// <param name="fifth">Fifth sequence</param>
		/// <param name="sixth">Sixth sequence</param>
		/// <param name="seventh">Seventh sequence</param>
		/// <param name="resultSelector">Function to apply to each group of elements</param>
		public static IEnumerable<TResult> ZipLongest<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TResult>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second, IEnumerable<TThird> third, IEnumerable<TFourth> fourth, IEnumerable<TFifth> fifth, IEnumerable<TSixth> sixth, IEnumerable<TSeventh> seventh,  Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, int, TResult> resultSelector)
        {
			first.ThrowIfNull("first");
			second.ThrowIfNull("second");
			third.ThrowIfNull("third");
			fourth.ThrowIfNull("fourth");
			fifth.ThrowIfNull("fifth");
			sixth.ThrowIfNull("sixth");
			seventh.ThrowIfNull("seventh");
            resultSelector.ThrowIfNull("resultSelector");

            return ZipIter(first, second, third, fourth, fifth, sixth, seventh, resultSelector, ImbalancedZipStrategy.Pad);
        }

		private static IEnumerable<TResult> ZipIter<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TResult>(IEnumerable<TFirst> first, IEnumerable<TSecond> second, IEnumerable<TThird> third, IEnumerable<TFourth> fourth, IEnumerable<TFifth> fifth, IEnumerable<TSixth> sixth, IEnumerable<TSeventh> seventh,  Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, int, TResult> resultSelector, ImbalancedZipStrategy imbalanceStrategy)
        {
			var count = 0;
			using (var e1 = first.GetEnumerator())
			using (var e2 = second.GetEnumerator())
			using (var e3 = third.GetEnumerator())
			using (var e4 = fourth.GetEnumerator())
			using (var e5 = fifth.GetEnumerator())
			using (var e6 = sixth.GetEnumerator())
			using (var e7 = seventh.GetEnumerator())
            {
                bool end;
				bool fail;

			    TFirst v1 = default(TFirst);
			    TSecond v2 = default(TSecond);
			    TThird v3 = default(TThird);
			    TFourth v4 = default(TFourth);
			    TFifth v5 = default(TFifth);
			    TSixth v6 = default(TSixth);
			    TSeventh v7 = default(TSeventh);

                while (true)
                {
                    end = true;
					fail = false;


                    if (e1.MoveNext())
                    {
                        v1 = e1.Current;
                        end = false;
                    }
                    else
                    {
                        switch (imbalanceStrategy)
                        {
                            case ImbalancedZipStrategy.Fail:
                                fail = true;
								break;
                            case ImbalancedZipStrategy.Truncate:
                                yield break;
                            case ImbalancedZipStrategy.Pad:
								v1 = default(TFirst);
                                break;
                        }
                    }

                    if (e2.MoveNext())
                    {
                        v2 = e2.Current;
                        end = false;
                    }
                    else
                    {
                        switch (imbalanceStrategy)
                        {
                            case ImbalancedZipStrategy.Fail:
                                fail = true;
								break;
                            case ImbalancedZipStrategy.Truncate:
                                yield break;
                            case ImbalancedZipStrategy.Pad:
								v2 = default(TSecond);
                                break;
                        }
                    }

                    if (e3.MoveNext())
                    {
                        v3 = e3.Current;
                        end = false;
                    }
                    else
                    {
                        switch (imbalanceStrategy)
                        {
                            case ImbalancedZipStrategy.Fail:
                                fail = true;
								break;
                            case ImbalancedZipStrategy.Truncate:
                                yield break;
                            case ImbalancedZipStrategy.Pad:
								v3 = default(TThird);
                                break;
                        }
                    }

                    if (e4.MoveNext())
                    {
                        v4 = e4.Current;
                        end = false;
                    }
                    else
                    {
                        switch (imbalanceStrategy)
                        {
                            case ImbalancedZipStrategy.Fail:
                                fail = true;
								break;
                            case ImbalancedZipStrategy.Truncate:
                                yield break;
                            case ImbalancedZipStrategy.Pad:
								v4 = default(TFourth);
                                break;
                        }
                    }

                    if (e5.MoveNext())
                    {
                        v5 = e5.Current;
                        end = false;
                    }
                    else
                    {
                        switch (imbalanceStrategy)
                        {
                            case ImbalancedZipStrategy.Fail:
                                fail = true;
								break;
                            case ImbalancedZipStrategy.Truncate:
                                yield break;
                            case ImbalancedZipStrategy.Pad:
								v5 = default(TFifth);
                                break;
                        }
                    }

                    if (e6.MoveNext())
                    {
                        v6 = e6.Current;
                        end = false;
                    }
                    else
                    {
                        switch (imbalanceStrategy)
                        {
                            case ImbalancedZipStrategy.Fail:
                                fail = true;
								break;
                            case ImbalancedZipStrategy.Truncate:
                                yield break;
                            case ImbalancedZipStrategy.Pad:
								v6 = default(TSixth);
                                break;
                        }
                    }

                    if (e7.MoveNext())
                    {
                        v7 = e7.Current;
                        end = false;
                    }
                    else
                    {
                        switch (imbalanceStrategy)
                        {
                            case ImbalancedZipStrategy.Fail:
                                fail = true;
								break;
                            case ImbalancedZipStrategy.Truncate:
                                yield break;
                            case ImbalancedZipStrategy.Pad:
								v7 = default(TSeventh);
                                break;
                        }
                    }

					if (end)
						yield break;
					else if (fail)
						throw new InvalidOperationException("Sequences were not all same length.");
					else
						yield return resultSelector(v1, v2, v3, v4, v5, v6, v7, count++);
                }
            }            
        }

		/// <summary>Returns a projection of tuples, where each tuple contains the N-th element from each of the argument sequences.</summary>
        /// <remarks>
        /// If the eight input sequences are of different lengths, the result sequence is terminated as soon as the shortest input sequence is exhausted.
        /// This operator uses deferred execution and streams its results.
        /// </remarks>
        /// <example>
        /// <code>
        /// int[] numbers = { 1, 2, 3 };
        /// string[] letters = { "A", "B", "C", "D" };
        /// var zipped = numbers.ZipShortest(letters, (n, l) => n + l);
        /// </code>
        /// The <c>zipped</c> variable, when iterated over, will yield "1A", "2B", "3C", in turn.
        /// </example>
		/// <typeparam name="TFirst">Type of elements in the first sequence.</typeparam>
		/// <typeparam name="TSecond">Type of elements in the second sequence.</typeparam>
		/// <typeparam name="TThird">Type of elements in the third sequence.</typeparam>
		/// <typeparam name="TFourth">Type of elements in the fourth sequence.</typeparam>
		/// <typeparam name="TFifth">Type of elements in the fifth sequence.</typeparam>
		/// <typeparam name="TSixth">Type of elements in the sixth sequence.</typeparam>
		/// <typeparam name="TSeventh">Type of elements in the seventh sequence.</typeparam>
		/// <typeparam name="TEighth">Type of elements in the eighth sequence.</typeparam>
		/// <typeparam name="TResult">Type of elements in result sequence</typeparam>
		/// <param name="first">First sequence</param>
		/// <param name="second">Second sequence</param>
		/// <param name="third">Third sequence</param>
		/// <param name="fourth">Fourth sequence</param>
		/// <param name="fifth">Fifth sequence</param>
		/// <param name="sixth">Sixth sequence</param>
		/// <param name="seventh">Seventh sequence</param>
		/// <param name="eighth">Eighth sequence</param>
		/// <param name="resultSelector">Function to apply to each group of elements</param>
		public static IEnumerable<TResult> ZipShortest<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TResult>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second, IEnumerable<TThird> third, IEnumerable<TFourth> fourth, IEnumerable<TFifth> fifth, IEnumerable<TSixth> sixth, IEnumerable<TSeventh> seventh, IEnumerable<TEighth> eighth,  Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TResult> resultSelector)
        {
			first.ThrowIfNull("first");
			second.ThrowIfNull("second");
			third.ThrowIfNull("third");
			fourth.ThrowIfNull("fourth");
			fifth.ThrowIfNull("fifth");
			sixth.ThrowIfNull("sixth");
			seventh.ThrowIfNull("seventh");
			eighth.ThrowIfNull("eighth");
            resultSelector.ThrowIfNull("resultSelector");

            return ZipIter(first, second, third, fourth, fifth, sixth, seventh, eighth, resultSelector, ImbalancedZipStrategy.Truncate);
        }

		/// <summary>Returns a projection of tuples, where each tuple contains the N-th element from each of the argument sequences.</summary>
        /// <remarks>
        /// If the eight input sequences are of different lengths, then <see cref="InvalidOperationException"/> is thrown.
        /// This operator uses deferred execution and streams its results.
        /// </remarks>
        /// <example>
        /// <code>
        /// int[] numbers = { 1, 2, 3 };
        /// string[] letters = { "A", "B", "C", "D" };
        /// var zipped = numbers.ZipEven(letters, (n, l) => n + l);
        /// </code>
        /// The <c>zipped</c> variable, when iterated over, will yield "1A", "2B", "3C", in turn.
        /// </example>
		/// <typeparam name="TFirst">Type of elements in the first sequence.</typeparam>
		/// <typeparam name="TSecond">Type of elements in the second sequence.</typeparam>
		/// <typeparam name="TThird">Type of elements in the third sequence.</typeparam>
		/// <typeparam name="TFourth">Type of elements in the fourth sequence.</typeparam>
		/// <typeparam name="TFifth">Type of elements in the fifth sequence.</typeparam>
		/// <typeparam name="TSixth">Type of elements in the sixth sequence.</typeparam>
		/// <typeparam name="TSeventh">Type of elements in the seventh sequence.</typeparam>
		/// <typeparam name="TEighth">Type of elements in the eighth sequence.</typeparam>
		/// <typeparam name="TResult">Type of elements in result sequence</typeparam>
		/// <param name="first">First sequence</param>
		/// <param name="second">Second sequence</param>
		/// <param name="third">Third sequence</param>
		/// <param name="fourth">Fourth sequence</param>
		/// <param name="fifth">Fifth sequence</param>
		/// <param name="sixth">Sixth sequence</param>
		/// <param name="seventh">Seventh sequence</param>
		/// <param name="eighth">Eighth sequence</param>
		/// <param name="resultSelector">Function to apply to each group of elements</param>
		public static IEnumerable<TResult> ZipEven<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TResult>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second, IEnumerable<TThird> third, IEnumerable<TFourth> fourth, IEnumerable<TFifth> fifth, IEnumerable<TSixth> sixth, IEnumerable<TSeventh> seventh, IEnumerable<TEighth> eighth,  Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TResult> resultSelector)
        {
			first.ThrowIfNull("first");
			second.ThrowIfNull("second");
			third.ThrowIfNull("third");
			fourth.ThrowIfNull("fourth");
			fifth.ThrowIfNull("fifth");
			sixth.ThrowIfNull("sixth");
			seventh.ThrowIfNull("seventh");
			eighth.ThrowIfNull("eighth");
            resultSelector.ThrowIfNull("resultSelector");

            return ZipIter(first, second, third, fourth, fifth, sixth, seventh, eighth, resultSelector, ImbalancedZipStrategy.Fail);
        }

		/// <summary>Returns a projection of tuples, where each tuple contains the N-th element from each of the argument sequences.</summary>
        /// <remarks>
        /// If the eight input sequences are of different lengths, then the result sequence will always be as long as the longest of the input sequences.
        /// The default values of the shorter sequence element types are used for padding.
        /// This operator uses deferred execution and streams its results.
        /// </remarks>
        /// <example>
        /// <code>
        /// int[] numbers = { 1, 2, 3 };
        /// string[] letters = { "A", "B", "C", "D" };
        /// var zipped = numbers.ZipLongest(letters, (n, l) => n + l);
        /// </code>
        /// The <c>zipped</c> variable, when iterated over, will yield "1A", "2B", "3C", in turn.
        /// </example>
		/// <typeparam name="TFirst">Type of elements in the first sequence.</typeparam>
		/// <typeparam name="TSecond">Type of elements in the second sequence.</typeparam>
		/// <typeparam name="TThird">Type of elements in the third sequence.</typeparam>
		/// <typeparam name="TFourth">Type of elements in the fourth sequence.</typeparam>
		/// <typeparam name="TFifth">Type of elements in the fifth sequence.</typeparam>
		/// <typeparam name="TSixth">Type of elements in the sixth sequence.</typeparam>
		/// <typeparam name="TSeventh">Type of elements in the seventh sequence.</typeparam>
		/// <typeparam name="TEighth">Type of elements in the eighth sequence.</typeparam>
		/// <typeparam name="TResult">Type of elements in result sequence</typeparam>
		/// <param name="first">First sequence</param>
		/// <param name="second">Second sequence</param>
		/// <param name="third">Third sequence</param>
		/// <param name="fourth">Fourth sequence</param>
		/// <param name="fifth">Fifth sequence</param>
		/// <param name="sixth">Sixth sequence</param>
		/// <param name="seventh">Seventh sequence</param>
		/// <param name="eighth">Eighth sequence</param>
		/// <param name="resultSelector">Function to apply to each group of elements</param>
		public static IEnumerable<TResult> ZipLongest<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TResult>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second, IEnumerable<TThird> third, IEnumerable<TFourth> fourth, IEnumerable<TFifth> fifth, IEnumerable<TSixth> sixth, IEnumerable<TSeventh> seventh, IEnumerable<TEighth> eighth,  Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TResult> resultSelector)
        {
			first.ThrowIfNull("first");
			second.ThrowIfNull("second");
			third.ThrowIfNull("third");
			fourth.ThrowIfNull("fourth");
			fifth.ThrowIfNull("fifth");
			sixth.ThrowIfNull("sixth");
			seventh.ThrowIfNull("seventh");
			eighth.ThrowIfNull("eighth");
            resultSelector.ThrowIfNull("resultSelector");

            return ZipIter(first, second, third, fourth, fifth, sixth, seventh, eighth, resultSelector, ImbalancedZipStrategy.Pad);
        }

		private static IEnumerable<TResult> ZipIter<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TResult>(IEnumerable<TFirst> first, IEnumerable<TSecond> second, IEnumerable<TThird> third, IEnumerable<TFourth> fourth, IEnumerable<TFifth> fifth, IEnumerable<TSixth> sixth, IEnumerable<TSeventh> seventh, IEnumerable<TEighth> eighth,  Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TResult> resultSelector, ImbalancedZipStrategy imbalanceStrategy)
        {
			using (var e1 = first.GetEnumerator())
			using (var e2 = second.GetEnumerator())
			using (var e3 = third.GetEnumerator())
			using (var e4 = fourth.GetEnumerator())
			using (var e5 = fifth.GetEnumerator())
			using (var e6 = sixth.GetEnumerator())
			using (var e7 = seventh.GetEnumerator())
			using (var e8 = eighth.GetEnumerator())
            {
                bool end;
				bool fail;

			    TFirst v1 = default(TFirst);
			    TSecond v2 = default(TSecond);
			    TThird v3 = default(TThird);
			    TFourth v4 = default(TFourth);
			    TFifth v5 = default(TFifth);
			    TSixth v6 = default(TSixth);
			    TSeventh v7 = default(TSeventh);
			    TEighth v8 = default(TEighth);

                while (true)
                {
                    end = true;
					fail = false;


                    if (e1.MoveNext())
                    {
                        v1 = e1.Current;
                        end = false;
                    }
                    else
                    {
                        switch (imbalanceStrategy)
                        {
                            case ImbalancedZipStrategy.Fail:
								fail = true;
								break;
                            case ImbalancedZipStrategy.Truncate:
                                yield break;
                            case ImbalancedZipStrategy.Pad:
								v1 = default(TFirst);
                                break;
                        }
                    }

                    if (e2.MoveNext())
                    {
                        v2 = e2.Current;
                        end = false;
                    }
                    else
                    {
                        switch (imbalanceStrategy)
                        {
                            case ImbalancedZipStrategy.Fail:
								fail = true;
								break;
                            case ImbalancedZipStrategy.Truncate:
                                yield break;
                            case ImbalancedZipStrategy.Pad:
								v2 = default(TSecond);
                                break;
                        }
                    }

                    if (e3.MoveNext())
                    {
                        v3 = e3.Current;
                        end = false;
                    }
                    else
                    {
                        switch (imbalanceStrategy)
                        {
                            case ImbalancedZipStrategy.Fail:
								fail = true;
								break;
                            case ImbalancedZipStrategy.Truncate:
                                yield break;
                            case ImbalancedZipStrategy.Pad:
								v3 = default(TThird);
                                break;
                        }
                    }

                    if (e4.MoveNext())
                    {
                        v4 = e4.Current;
                        end = false;
                    }
                    else
                    {
                        switch (imbalanceStrategy)
                        {
                            case ImbalancedZipStrategy.Fail:
								fail = true;
								break;
                            case ImbalancedZipStrategy.Truncate:
                                yield break;
                            case ImbalancedZipStrategy.Pad:
								v4 = default(TFourth);
                                break;
                        }
                    }

                    if (e5.MoveNext())
                    {
                        v5 = e5.Current;
                        end = false;
                    }
                    else
                    {
                        switch (imbalanceStrategy)
                        {
                            case ImbalancedZipStrategy.Fail:
								fail = true;
								break;
                            case ImbalancedZipStrategy.Truncate:
                                yield break;
                            case ImbalancedZipStrategy.Pad:
								v5 = default(TFifth);
                                break;
                        }
                    }

                    if (e6.MoveNext())
                    {
                        v6 = e6.Current;
                        end = false;
                    }
                    else
                    {
                        switch (imbalanceStrategy)
                        {
                            case ImbalancedZipStrategy.Fail:
								fail = true;
								break;
                            case ImbalancedZipStrategy.Truncate:
                                yield break;
                            case ImbalancedZipStrategy.Pad:
								v6 = default(TSixth);
                                break;
                        }
                    }

                    if (e7.MoveNext())
                    {
                        v7 = e7.Current;
                        end = false;
                    }
                    else
                    {
                        switch (imbalanceStrategy)
                        {
                            case ImbalancedZipStrategy.Fail:
								fail = true;
								break;
                            case ImbalancedZipStrategy.Truncate:
                                yield break;
                            case ImbalancedZipStrategy.Pad:
								v7 = default(TSeventh);
                                break;
                        }
                    }

                    if (e8.MoveNext())
                    {
                        v8 = e8.Current;
                        end = false;
                    }
                    else
                    {
                        switch (imbalanceStrategy)
                        {
                            case ImbalancedZipStrategy.Fail:
								fail = true;
								break;
                            case ImbalancedZipStrategy.Truncate:
                                yield break;
                            case ImbalancedZipStrategy.Pad:
								v8 = default(TEighth);
                                break;
                        }
                    }

					if (end)
						yield break;
					else if (fail)
						throw new InvalidOperationException("Sequences were not all same length.");
					else
						yield return resultSelector(v1, v2, v3, v4, v5, v6, v7, v8);
                }
            }            
        }

		/// <summary>Returns a projection of tuples, where each tuple contains the N-th element from each of the argument sequences.</summary>
        /// <remarks>
        /// If the eight input sequences are of different lengths, the result sequence is terminated as soon as the shortest input sequence is exhausted.
		/// A global index over all sequences is passed into the result selector function.
        /// This operator uses deferred execution and streams its results.
        /// </remarks>
        /// <example>
        /// <code>
        /// int[] numbers = { 1, 2, 3 };
        /// string[] letters = { "A", "B", "C", "D" };
        /// var zipped = numbers.ZipShortest(letters, (n, l) => n + l);
        /// </code>
        /// The <c>zipped</c> variable, when iterated over, will yield "1A", "2B", "3C", in turn.
        /// </example>
		/// <typeparam name="TFirst">Type of elements in the first sequence.</typeparam>
		/// <typeparam name="TSecond">Type of elements in the second sequence.</typeparam>
		/// <typeparam name="TThird">Type of elements in the third sequence.</typeparam>
		/// <typeparam name="TFourth">Type of elements in the fourth sequence.</typeparam>
		/// <typeparam name="TFifth">Type of elements in the fifth sequence.</typeparam>
		/// <typeparam name="TSixth">Type of elements in the sixth sequence.</typeparam>
		/// <typeparam name="TSeventh">Type of elements in the seventh sequence.</typeparam>
		/// <typeparam name="TEighth">Type of elements in the eighth sequence.</typeparam>
		/// <typeparam name="TResult">Type of elements in result sequence</typeparam>
		/// <param name="first">First sequence</param>
		/// <param name="second">Second sequence</param>
		/// <param name="third">Third sequence</param>
		/// <param name="fourth">Fourth sequence</param>
		/// <param name="fifth">Fifth sequence</param>
		/// <param name="sixth">Sixth sequence</param>
		/// <param name="seventh">Seventh sequence</param>
		/// <param name="eighth">Eighth sequence</param>
		/// <param name="resultSelector">Function to apply to each group of elements</param>
		public static IEnumerable<TResult> ZipShortest<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TResult>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second, IEnumerable<TThird> third, IEnumerable<TFourth> fourth, IEnumerable<TFifth> fifth, IEnumerable<TSixth> sixth, IEnumerable<TSeventh> seventh, IEnumerable<TEighth> eighth,  Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, int, TResult> resultSelector)
        {
			first.ThrowIfNull("first");
			second.ThrowIfNull("second");
			third.ThrowIfNull("third");
			fourth.ThrowIfNull("fourth");
			fifth.ThrowIfNull("fifth");
			sixth.ThrowIfNull("sixth");
			seventh.ThrowIfNull("seventh");
			eighth.ThrowIfNull("eighth");
            resultSelector.ThrowIfNull("resultSelector");

            return ZipIter(first, second, third, fourth, fifth, sixth, seventh, eighth, resultSelector, ImbalancedZipStrategy.Truncate);
        }

		/// <summary>Returns a projection of tuples, where each tuple contains the N-th element from each of the argument sequences.</summary>
        /// <remarks>
        /// If the eight input sequences are of different lengths, then <see cref="InvalidOperationException"/> is thrown.
		/// A global index over all sequences is passed into the result selector function.
        /// This operator uses deferred execution and streams its results.
        /// </remarks>
        /// <example>
        /// <code>
        /// int[] numbers = { 1, 2, 3 };
        /// string[] letters = { "A", "B", "C", "D" };
        /// var zipped = numbers.ZipEven(letters, (n, l) => n + l);
        /// </code>
        /// The <c>zipped</c> variable, when iterated over, will yield "1A", "2B", "3C", in turn.
        /// </example>
		/// <typeparam name="TFirst">Type of elements in the first sequence.</typeparam>
		/// <typeparam name="TSecond">Type of elements in the second sequence.</typeparam>
		/// <typeparam name="TThird">Type of elements in the third sequence.</typeparam>
		/// <typeparam name="TFourth">Type of elements in the fourth sequence.</typeparam>
		/// <typeparam name="TFifth">Type of elements in the fifth sequence.</typeparam>
		/// <typeparam name="TSixth">Type of elements in the sixth sequence.</typeparam>
		/// <typeparam name="TSeventh">Type of elements in the seventh sequence.</typeparam>
		/// <typeparam name="TEighth">Type of elements in the eighth sequence.</typeparam>
		/// <typeparam name="TResult">Type of elements in result sequence</typeparam>
		/// <param name="first">First sequence</param>
		/// <param name="second">Second sequence</param>
		/// <param name="third">Third sequence</param>
		/// <param name="fourth">Fourth sequence</param>
		/// <param name="fifth">Fifth sequence</param>
		/// <param name="sixth">Sixth sequence</param>
		/// <param name="seventh">Seventh sequence</param>
		/// <param name="eighth">Eighth sequence</param>
		/// <param name="resultSelector">Function to apply to each group of elements</param>
		public static IEnumerable<TResult> ZipEven<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TResult>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second, IEnumerable<TThird> third, IEnumerable<TFourth> fourth, IEnumerable<TFifth> fifth, IEnumerable<TSixth> sixth, IEnumerable<TSeventh> seventh, IEnumerable<TEighth> eighth,  Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, int, TResult> resultSelector)
        {
			first.ThrowIfNull("first");
			second.ThrowIfNull("second");
			third.ThrowIfNull("third");
			fourth.ThrowIfNull("fourth");
			fifth.ThrowIfNull("fifth");
			sixth.ThrowIfNull("sixth");
			seventh.ThrowIfNull("seventh");
			eighth.ThrowIfNull("eighth");
            resultSelector.ThrowIfNull("resultSelector");

            return ZipIter(first, second, third, fourth, fifth, sixth, seventh, eighth, resultSelector, ImbalancedZipStrategy.Fail);
        }

		/// <summary>Returns a projection of tuples, where each tuple contains the N-th element from each of the argument sequences.</summary>
        /// <remarks>
        /// If the eight input sequences are of different lengths, then the result sequence will always be as long as the longest of the input sequences.
        /// The default values of the shorter sequence element types are used for padding.
		/// A global index over all sequences is passed into the result selector function.
        /// This operator uses deferred execution and streams its results.
        /// </remarks>
        /// <example>
        /// <code>
        /// int[] numbers = { 1, 2, 3 };
        /// string[] letters = { "A", "B", "C", "D" };
        /// var zipped = numbers.ZipLongest(letters, (n, l) => n + l);
        /// </code>
        /// The <c>zipped</c> variable, when iterated over, will yield "1A", "2B", "3C", in turn.
        /// </example>
		/// <typeparam name="TFirst">Type of elements in the first sequence.</typeparam>
		/// <typeparam name="TSecond">Type of elements in the second sequence.</typeparam>
		/// <typeparam name="TThird">Type of elements in the third sequence.</typeparam>
		/// <typeparam name="TFourth">Type of elements in the fourth sequence.</typeparam>
		/// <typeparam name="TFifth">Type of elements in the fifth sequence.</typeparam>
		/// <typeparam name="TSixth">Type of elements in the sixth sequence.</typeparam>
		/// <typeparam name="TSeventh">Type of elements in the seventh sequence.</typeparam>
		/// <typeparam name="TEighth">Type of elements in the eighth sequence.</typeparam>
		/// <typeparam name="TResult">Type of elements in result sequence</typeparam>
		/// <param name="first">First sequence</param>
		/// <param name="second">Second sequence</param>
		/// <param name="third">Third sequence</param>
		/// <param name="fourth">Fourth sequence</param>
		/// <param name="fifth">Fifth sequence</param>
		/// <param name="sixth">Sixth sequence</param>
		/// <param name="seventh">Seventh sequence</param>
		/// <param name="eighth">Eighth sequence</param>
		/// <param name="resultSelector">Function to apply to each group of elements</param>
		public static IEnumerable<TResult> ZipLongest<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TResult>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second, IEnumerable<TThird> third, IEnumerable<TFourth> fourth, IEnumerable<TFifth> fifth, IEnumerable<TSixth> sixth, IEnumerable<TSeventh> seventh, IEnumerable<TEighth> eighth,  Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, int, TResult> resultSelector)
        {
			first.ThrowIfNull("first");
			second.ThrowIfNull("second");
			third.ThrowIfNull("third");
			fourth.ThrowIfNull("fourth");
			fifth.ThrowIfNull("fifth");
			sixth.ThrowIfNull("sixth");
			seventh.ThrowIfNull("seventh");
			eighth.ThrowIfNull("eighth");
            resultSelector.ThrowIfNull("resultSelector");

            return ZipIter(first, second, third, fourth, fifth, sixth, seventh, eighth, resultSelector, ImbalancedZipStrategy.Pad);
        }

		private static IEnumerable<TResult> ZipIter<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TResult>(IEnumerable<TFirst> first, IEnumerable<TSecond> second, IEnumerable<TThird> third, IEnumerable<TFourth> fourth, IEnumerable<TFifth> fifth, IEnumerable<TSixth> sixth, IEnumerable<TSeventh> seventh, IEnumerable<TEighth> eighth,  Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, int, TResult> resultSelector, ImbalancedZipStrategy imbalanceStrategy)
        {
			var count = 0;
			using (var e1 = first.GetEnumerator())
			using (var e2 = second.GetEnumerator())
			using (var e3 = third.GetEnumerator())
			using (var e4 = fourth.GetEnumerator())
			using (var e5 = fifth.GetEnumerator())
			using (var e6 = sixth.GetEnumerator())
			using (var e7 = seventh.GetEnumerator())
			using (var e8 = eighth.GetEnumerator())
            {
                bool end;
				bool fail;

			    TFirst v1 = default(TFirst);
			    TSecond v2 = default(TSecond);
			    TThird v3 = default(TThird);
			    TFourth v4 = default(TFourth);
			    TFifth v5 = default(TFifth);
			    TSixth v6 = default(TSixth);
			    TSeventh v7 = default(TSeventh);
			    TEighth v8 = default(TEighth);

                while (true)
                {
                    end = true;
					fail = false;


                    if (e1.MoveNext())
                    {
                        v1 = e1.Current;
                        end = false;
                    }
                    else
                    {
                        switch (imbalanceStrategy)
                        {
                            case ImbalancedZipStrategy.Fail:
                                fail = true;
								break;
                            case ImbalancedZipStrategy.Truncate:
                                yield break;
                            case ImbalancedZipStrategy.Pad:
								v1 = default(TFirst);
                                break;
                        }
                    }

                    if (e2.MoveNext())
                    {
                        v2 = e2.Current;
                        end = false;
                    }
                    else
                    {
                        switch (imbalanceStrategy)
                        {
                            case ImbalancedZipStrategy.Fail:
                                fail = true;
								break;
                            case ImbalancedZipStrategy.Truncate:
                                yield break;
                            case ImbalancedZipStrategy.Pad:
								v2 = default(TSecond);
                                break;
                        }
                    }

                    if (e3.MoveNext())
                    {
                        v3 = e3.Current;
                        end = false;
                    }
                    else
                    {
                        switch (imbalanceStrategy)
                        {
                            case ImbalancedZipStrategy.Fail:
                                fail = true;
								break;
                            case ImbalancedZipStrategy.Truncate:
                                yield break;
                            case ImbalancedZipStrategy.Pad:
								v3 = default(TThird);
                                break;
                        }
                    }

                    if (e4.MoveNext())
                    {
                        v4 = e4.Current;
                        end = false;
                    }
                    else
                    {
                        switch (imbalanceStrategy)
                        {
                            case ImbalancedZipStrategy.Fail:
                                fail = true;
								break;
                            case ImbalancedZipStrategy.Truncate:
                                yield break;
                            case ImbalancedZipStrategy.Pad:
								v4 = default(TFourth);
                                break;
                        }
                    }

                    if (e5.MoveNext())
                    {
                        v5 = e5.Current;
                        end = false;
                    }
                    else
                    {
                        switch (imbalanceStrategy)
                        {
                            case ImbalancedZipStrategy.Fail:
                                fail = true;
								break;
                            case ImbalancedZipStrategy.Truncate:
                                yield break;
                            case ImbalancedZipStrategy.Pad:
								v5 = default(TFifth);
                                break;
                        }
                    }

                    if (e6.MoveNext())
                    {
                        v6 = e6.Current;
                        end = false;
                    }
                    else
                    {
                        switch (imbalanceStrategy)
                        {
                            case ImbalancedZipStrategy.Fail:
                                fail = true;
								break;
                            case ImbalancedZipStrategy.Truncate:
                                yield break;
                            case ImbalancedZipStrategy.Pad:
								v6 = default(TSixth);
                                break;
                        }
                    }

                    if (e7.MoveNext())
                    {
                        v7 = e7.Current;
                        end = false;
                    }
                    else
                    {
                        switch (imbalanceStrategy)
                        {
                            case ImbalancedZipStrategy.Fail:
                                fail = true;
								break;
                            case ImbalancedZipStrategy.Truncate:
                                yield break;
                            case ImbalancedZipStrategy.Pad:
								v7 = default(TSeventh);
                                break;
                        }
                    }

                    if (e8.MoveNext())
                    {
                        v8 = e8.Current;
                        end = false;
                    }
                    else
                    {
                        switch (imbalanceStrategy)
                        {
                            case ImbalancedZipStrategy.Fail:
                                fail = true;
								break;
                            case ImbalancedZipStrategy.Truncate:
                                yield break;
                            case ImbalancedZipStrategy.Pad:
								v8 = default(TEighth);
                                break;
                        }
                    }

					if (end)
						yield break;
					else if (fail)
						throw new InvalidOperationException("Sequences were not all same length.");
					else
						yield return resultSelector(v1, v2, v3, v4, v5, v6, v7, v8, count++);
                }
            }            
        }

		/// <summary>Returns a projection of tuples, where each tuple contains the N-th element from each of the argument sequences.</summary>
        /// <remarks>
        /// If the nine input sequences are of different lengths, the result sequence is terminated as soon as the shortest input sequence is exhausted.
        /// This operator uses deferred execution and streams its results.
        /// </remarks>
        /// <example>
        /// <code>
        /// int[] numbers = { 1, 2, 3 };
        /// string[] letters = { "A", "B", "C", "D" };
        /// var zipped = numbers.ZipShortest(letters, (n, l) => n + l);
        /// </code>
        /// The <c>zipped</c> variable, when iterated over, will yield "1A", "2B", "3C", in turn.
        /// </example>
		/// <typeparam name="TFirst">Type of elements in the first sequence.</typeparam>
		/// <typeparam name="TSecond">Type of elements in the second sequence.</typeparam>
		/// <typeparam name="TThird">Type of elements in the third sequence.</typeparam>
		/// <typeparam name="TFourth">Type of elements in the fourth sequence.</typeparam>
		/// <typeparam name="TFifth">Type of elements in the fifth sequence.</typeparam>
		/// <typeparam name="TSixth">Type of elements in the sixth sequence.</typeparam>
		/// <typeparam name="TSeventh">Type of elements in the seventh sequence.</typeparam>
		/// <typeparam name="TEighth">Type of elements in the eighth sequence.</typeparam>
		/// <typeparam name="TNinth">Type of elements in the ninth sequence.</typeparam>
		/// <typeparam name="TResult">Type of elements in result sequence</typeparam>
		/// <param name="first">First sequence</param>
		/// <param name="second">Second sequence</param>
		/// <param name="third">Third sequence</param>
		/// <param name="fourth">Fourth sequence</param>
		/// <param name="fifth">Fifth sequence</param>
		/// <param name="sixth">Sixth sequence</param>
		/// <param name="seventh">Seventh sequence</param>
		/// <param name="eighth">Eighth sequence</param>
		/// <param name="ninth">Ninth sequence</param>
		/// <param name="resultSelector">Function to apply to each group of elements</param>
		public static IEnumerable<TResult> ZipShortest<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth, TResult>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second, IEnumerable<TThird> third, IEnumerable<TFourth> fourth, IEnumerable<TFifth> fifth, IEnumerable<TSixth> sixth, IEnumerable<TSeventh> seventh, IEnumerable<TEighth> eighth, IEnumerable<TNinth> ninth,  Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth, TResult> resultSelector)
        {
			first.ThrowIfNull("first");
			second.ThrowIfNull("second");
			third.ThrowIfNull("third");
			fourth.ThrowIfNull("fourth");
			fifth.ThrowIfNull("fifth");
			sixth.ThrowIfNull("sixth");
			seventh.ThrowIfNull("seventh");
			eighth.ThrowIfNull("eighth");
			ninth.ThrowIfNull("ninth");
            resultSelector.ThrowIfNull("resultSelector");

            return ZipIter(first, second, third, fourth, fifth, sixth, seventh, eighth, ninth, resultSelector, ImbalancedZipStrategy.Truncate);
        }

		/// <summary>Returns a projection of tuples, where each tuple contains the N-th element from each of the argument sequences.</summary>
        /// <remarks>
        /// If the nine input sequences are of different lengths, then <see cref="InvalidOperationException"/> is thrown.
        /// This operator uses deferred execution and streams its results.
        /// </remarks>
        /// <example>
        /// <code>
        /// int[] numbers = { 1, 2, 3 };
        /// string[] letters = { "A", "B", "C", "D" };
        /// var zipped = numbers.ZipEven(letters, (n, l) => n + l);
        /// </code>
        /// The <c>zipped</c> variable, when iterated over, will yield "1A", "2B", "3C", in turn.
        /// </example>
		/// <typeparam name="TFirst">Type of elements in the first sequence.</typeparam>
		/// <typeparam name="TSecond">Type of elements in the second sequence.</typeparam>
		/// <typeparam name="TThird">Type of elements in the third sequence.</typeparam>
		/// <typeparam name="TFourth">Type of elements in the fourth sequence.</typeparam>
		/// <typeparam name="TFifth">Type of elements in the fifth sequence.</typeparam>
		/// <typeparam name="TSixth">Type of elements in the sixth sequence.</typeparam>
		/// <typeparam name="TSeventh">Type of elements in the seventh sequence.</typeparam>
		/// <typeparam name="TEighth">Type of elements in the eighth sequence.</typeparam>
		/// <typeparam name="TNinth">Type of elements in the ninth sequence.</typeparam>
		/// <typeparam name="TResult">Type of elements in result sequence</typeparam>
		/// <param name="first">First sequence</param>
		/// <param name="second">Second sequence</param>
		/// <param name="third">Third sequence</param>
		/// <param name="fourth">Fourth sequence</param>
		/// <param name="fifth">Fifth sequence</param>
		/// <param name="sixth">Sixth sequence</param>
		/// <param name="seventh">Seventh sequence</param>
		/// <param name="eighth">Eighth sequence</param>
		/// <param name="ninth">Ninth sequence</param>
		/// <param name="resultSelector">Function to apply to each group of elements</param>
		public static IEnumerable<TResult> ZipEven<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth, TResult>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second, IEnumerable<TThird> third, IEnumerable<TFourth> fourth, IEnumerable<TFifth> fifth, IEnumerable<TSixth> sixth, IEnumerable<TSeventh> seventh, IEnumerable<TEighth> eighth, IEnumerable<TNinth> ninth,  Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth, TResult> resultSelector)
        {
			first.ThrowIfNull("first");
			second.ThrowIfNull("second");
			third.ThrowIfNull("third");
			fourth.ThrowIfNull("fourth");
			fifth.ThrowIfNull("fifth");
			sixth.ThrowIfNull("sixth");
			seventh.ThrowIfNull("seventh");
			eighth.ThrowIfNull("eighth");
			ninth.ThrowIfNull("ninth");
            resultSelector.ThrowIfNull("resultSelector");

            return ZipIter(first, second, third, fourth, fifth, sixth, seventh, eighth, ninth, resultSelector, ImbalancedZipStrategy.Fail);
        }

		/// <summary>Returns a projection of tuples, where each tuple contains the N-th element from each of the argument sequences.</summary>
        /// <remarks>
        /// If the nine input sequences are of different lengths, then the result sequence will always be as long as the longest of the input sequences.
        /// The default values of the shorter sequence element types are used for padding.
        /// This operator uses deferred execution and streams its results.
        /// </remarks>
        /// <example>
        /// <code>
        /// int[] numbers = { 1, 2, 3 };
        /// string[] letters = { "A", "B", "C", "D" };
        /// var zipped = numbers.ZipLongest(letters, (n, l) => n + l);
        /// </code>
        /// The <c>zipped</c> variable, when iterated over, will yield "1A", "2B", "3C", in turn.
        /// </example>
		/// <typeparam name="TFirst">Type of elements in the first sequence.</typeparam>
		/// <typeparam name="TSecond">Type of elements in the second sequence.</typeparam>
		/// <typeparam name="TThird">Type of elements in the third sequence.</typeparam>
		/// <typeparam name="TFourth">Type of elements in the fourth sequence.</typeparam>
		/// <typeparam name="TFifth">Type of elements in the fifth sequence.</typeparam>
		/// <typeparam name="TSixth">Type of elements in the sixth sequence.</typeparam>
		/// <typeparam name="TSeventh">Type of elements in the seventh sequence.</typeparam>
		/// <typeparam name="TEighth">Type of elements in the eighth sequence.</typeparam>
		/// <typeparam name="TNinth">Type of elements in the ninth sequence.</typeparam>
		/// <typeparam name="TResult">Type of elements in result sequence</typeparam>
		/// <param name="first">First sequence</param>
		/// <param name="second">Second sequence</param>
		/// <param name="third">Third sequence</param>
		/// <param name="fourth">Fourth sequence</param>
		/// <param name="fifth">Fifth sequence</param>
		/// <param name="sixth">Sixth sequence</param>
		/// <param name="seventh">Seventh sequence</param>
		/// <param name="eighth">Eighth sequence</param>
		/// <param name="ninth">Ninth sequence</param>
		/// <param name="resultSelector">Function to apply to each group of elements</param>
		public static IEnumerable<TResult> ZipLongest<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth, TResult>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second, IEnumerable<TThird> third, IEnumerable<TFourth> fourth, IEnumerable<TFifth> fifth, IEnumerable<TSixth> sixth, IEnumerable<TSeventh> seventh, IEnumerable<TEighth> eighth, IEnumerable<TNinth> ninth,  Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth, TResult> resultSelector)
        {
			first.ThrowIfNull("first");
			second.ThrowIfNull("second");
			third.ThrowIfNull("third");
			fourth.ThrowIfNull("fourth");
			fifth.ThrowIfNull("fifth");
			sixth.ThrowIfNull("sixth");
			seventh.ThrowIfNull("seventh");
			eighth.ThrowIfNull("eighth");
			ninth.ThrowIfNull("ninth");
            resultSelector.ThrowIfNull("resultSelector");

            return ZipIter(first, second, third, fourth, fifth, sixth, seventh, eighth, ninth, resultSelector, ImbalancedZipStrategy.Pad);
        }

		private static IEnumerable<TResult> ZipIter<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth, TResult>(IEnumerable<TFirst> first, IEnumerable<TSecond> second, IEnumerable<TThird> third, IEnumerable<TFourth> fourth, IEnumerable<TFifth> fifth, IEnumerable<TSixth> sixth, IEnumerable<TSeventh> seventh, IEnumerable<TEighth> eighth, IEnumerable<TNinth> ninth,  Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth, TResult> resultSelector, ImbalancedZipStrategy imbalanceStrategy)
        {
			using (var e1 = first.GetEnumerator())
			using (var e2 = second.GetEnumerator())
			using (var e3 = third.GetEnumerator())
			using (var e4 = fourth.GetEnumerator())
			using (var e5 = fifth.GetEnumerator())
			using (var e6 = sixth.GetEnumerator())
			using (var e7 = seventh.GetEnumerator())
			using (var e8 = eighth.GetEnumerator())
			using (var e9 = ninth.GetEnumerator())
            {
                bool end;
				bool fail;

			    TFirst v1 = default(TFirst);
			    TSecond v2 = default(TSecond);
			    TThird v3 = default(TThird);
			    TFourth v4 = default(TFourth);
			    TFifth v5 = default(TFifth);
			    TSixth v6 = default(TSixth);
			    TSeventh v7 = default(TSeventh);
			    TEighth v8 = default(TEighth);
			    TNinth v9 = default(TNinth);

                while (true)
                {
                    end = true;
					fail = false;


                    if (e1.MoveNext())
                    {
                        v1 = e1.Current;
                        end = false;
                    }
                    else
                    {
                        switch (imbalanceStrategy)
                        {
                            case ImbalancedZipStrategy.Fail:
								fail = true;
								break;
                            case ImbalancedZipStrategy.Truncate:
                                yield break;
                            case ImbalancedZipStrategy.Pad:
								v1 = default(TFirst);
                                break;
                        }
                    }

                    if (e2.MoveNext())
                    {
                        v2 = e2.Current;
                        end = false;
                    }
                    else
                    {
                        switch (imbalanceStrategy)
                        {
                            case ImbalancedZipStrategy.Fail:
								fail = true;
								break;
                            case ImbalancedZipStrategy.Truncate:
                                yield break;
                            case ImbalancedZipStrategy.Pad:
								v2 = default(TSecond);
                                break;
                        }
                    }

                    if (e3.MoveNext())
                    {
                        v3 = e3.Current;
                        end = false;
                    }
                    else
                    {
                        switch (imbalanceStrategy)
                        {
                            case ImbalancedZipStrategy.Fail:
								fail = true;
								break;
                            case ImbalancedZipStrategy.Truncate:
                                yield break;
                            case ImbalancedZipStrategy.Pad:
								v3 = default(TThird);
                                break;
                        }
                    }

                    if (e4.MoveNext())
                    {
                        v4 = e4.Current;
                        end = false;
                    }
                    else
                    {
                        switch (imbalanceStrategy)
                        {
                            case ImbalancedZipStrategy.Fail:
								fail = true;
								break;
                            case ImbalancedZipStrategy.Truncate:
                                yield break;
                            case ImbalancedZipStrategy.Pad:
								v4 = default(TFourth);
                                break;
                        }
                    }

                    if (e5.MoveNext())
                    {
                        v5 = e5.Current;
                        end = false;
                    }
                    else
                    {
                        switch (imbalanceStrategy)
                        {
                            case ImbalancedZipStrategy.Fail:
								fail = true;
								break;
                            case ImbalancedZipStrategy.Truncate:
                                yield break;
                            case ImbalancedZipStrategy.Pad:
								v5 = default(TFifth);
                                break;
                        }
                    }

                    if (e6.MoveNext())
                    {
                        v6 = e6.Current;
                        end = false;
                    }
                    else
                    {
                        switch (imbalanceStrategy)
                        {
                            case ImbalancedZipStrategy.Fail:
								fail = true;
								break;
                            case ImbalancedZipStrategy.Truncate:
                                yield break;
                            case ImbalancedZipStrategy.Pad:
								v6 = default(TSixth);
                                break;
                        }
                    }

                    if (e7.MoveNext())
                    {
                        v7 = e7.Current;
                        end = false;
                    }
                    else
                    {
                        switch (imbalanceStrategy)
                        {
                            case ImbalancedZipStrategy.Fail:
								fail = true;
								break;
                            case ImbalancedZipStrategy.Truncate:
                                yield break;
                            case ImbalancedZipStrategy.Pad:
								v7 = default(TSeventh);
                                break;
                        }
                    }

                    if (e8.MoveNext())
                    {
                        v8 = e8.Current;
                        end = false;
                    }
                    else
                    {
                        switch (imbalanceStrategy)
                        {
                            case ImbalancedZipStrategy.Fail:
								fail = true;
								break;
                            case ImbalancedZipStrategy.Truncate:
                                yield break;
                            case ImbalancedZipStrategy.Pad:
								v8 = default(TEighth);
                                break;
                        }
                    }

                    if (e9.MoveNext())
                    {
                        v9 = e9.Current;
                        end = false;
                    }
                    else
                    {
                        switch (imbalanceStrategy)
                        {
                            case ImbalancedZipStrategy.Fail:
								fail = true;
								break;
                            case ImbalancedZipStrategy.Truncate:
                                yield break;
                            case ImbalancedZipStrategy.Pad:
								v9 = default(TNinth);
                                break;
                        }
                    }

					if (end)
						yield break;
					else if (fail)
						throw new InvalidOperationException("Sequences were not all same length.");
					else
						yield return resultSelector(v1, v2, v3, v4, v5, v6, v7, v8, v9);
                }
            }            
        }

		/// <summary>Returns a projection of tuples, where each tuple contains the N-th element from each of the argument sequences.</summary>
        /// <remarks>
        /// If the nine input sequences are of different lengths, the result sequence is terminated as soon as the shortest input sequence is exhausted.
		/// A global index over all sequences is passed into the result selector function.
        /// This operator uses deferred execution and streams its results.
        /// </remarks>
        /// <example>
        /// <code>
        /// int[] numbers = { 1, 2, 3 };
        /// string[] letters = { "A", "B", "C", "D" };
        /// var zipped = numbers.ZipShortest(letters, (n, l) => n + l);
        /// </code>
        /// The <c>zipped</c> variable, when iterated over, will yield "1A", "2B", "3C", in turn.
        /// </example>
		/// <typeparam name="TFirst">Type of elements in the first sequence.</typeparam>
		/// <typeparam name="TSecond">Type of elements in the second sequence.</typeparam>
		/// <typeparam name="TThird">Type of elements in the third sequence.</typeparam>
		/// <typeparam name="TFourth">Type of elements in the fourth sequence.</typeparam>
		/// <typeparam name="TFifth">Type of elements in the fifth sequence.</typeparam>
		/// <typeparam name="TSixth">Type of elements in the sixth sequence.</typeparam>
		/// <typeparam name="TSeventh">Type of elements in the seventh sequence.</typeparam>
		/// <typeparam name="TEighth">Type of elements in the eighth sequence.</typeparam>
		/// <typeparam name="TNinth">Type of elements in the ninth sequence.</typeparam>
		/// <typeparam name="TResult">Type of elements in result sequence</typeparam>
		/// <param name="first">First sequence</param>
		/// <param name="second">Second sequence</param>
		/// <param name="third">Third sequence</param>
		/// <param name="fourth">Fourth sequence</param>
		/// <param name="fifth">Fifth sequence</param>
		/// <param name="sixth">Sixth sequence</param>
		/// <param name="seventh">Seventh sequence</param>
		/// <param name="eighth">Eighth sequence</param>
		/// <param name="ninth">Ninth sequence</param>
		/// <param name="resultSelector">Function to apply to each group of elements</param>
		public static IEnumerable<TResult> ZipShortest<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth, TResult>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second, IEnumerable<TThird> third, IEnumerable<TFourth> fourth, IEnumerable<TFifth> fifth, IEnumerable<TSixth> sixth, IEnumerable<TSeventh> seventh, IEnumerable<TEighth> eighth, IEnumerable<TNinth> ninth,  Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth, int, TResult> resultSelector)
        {
			first.ThrowIfNull("first");
			second.ThrowIfNull("second");
			third.ThrowIfNull("third");
			fourth.ThrowIfNull("fourth");
			fifth.ThrowIfNull("fifth");
			sixth.ThrowIfNull("sixth");
			seventh.ThrowIfNull("seventh");
			eighth.ThrowIfNull("eighth");
			ninth.ThrowIfNull("ninth");
            resultSelector.ThrowIfNull("resultSelector");

            return ZipIter(first, second, third, fourth, fifth, sixth, seventh, eighth, ninth, resultSelector, ImbalancedZipStrategy.Truncate);
        }

		/// <summary>Returns a projection of tuples, where each tuple contains the N-th element from each of the argument sequences.</summary>
        /// <remarks>
        /// If the nine input sequences are of different lengths, then <see cref="InvalidOperationException"/> is thrown.
		/// A global index over all sequences is passed into the result selector function.
        /// This operator uses deferred execution and streams its results.
        /// </remarks>
        /// <example>
        /// <code>
        /// int[] numbers = { 1, 2, 3 };
        /// string[] letters = { "A", "B", "C", "D" };
        /// var zipped = numbers.ZipEven(letters, (n, l) => n + l);
        /// </code>
        /// The <c>zipped</c> variable, when iterated over, will yield "1A", "2B", "3C", in turn.
        /// </example>
		/// <typeparam name="TFirst">Type of elements in the first sequence.</typeparam>
		/// <typeparam name="TSecond">Type of elements in the second sequence.</typeparam>
		/// <typeparam name="TThird">Type of elements in the third sequence.</typeparam>
		/// <typeparam name="TFourth">Type of elements in the fourth sequence.</typeparam>
		/// <typeparam name="TFifth">Type of elements in the fifth sequence.</typeparam>
		/// <typeparam name="TSixth">Type of elements in the sixth sequence.</typeparam>
		/// <typeparam name="TSeventh">Type of elements in the seventh sequence.</typeparam>
		/// <typeparam name="TEighth">Type of elements in the eighth sequence.</typeparam>
		/// <typeparam name="TNinth">Type of elements in the ninth sequence.</typeparam>
		/// <typeparam name="TResult">Type of elements in result sequence</typeparam>
		/// <param name="first">First sequence</param>
		/// <param name="second">Second sequence</param>
		/// <param name="third">Third sequence</param>
		/// <param name="fourth">Fourth sequence</param>
		/// <param name="fifth">Fifth sequence</param>
		/// <param name="sixth">Sixth sequence</param>
		/// <param name="seventh">Seventh sequence</param>
		/// <param name="eighth">Eighth sequence</param>
		/// <param name="ninth">Ninth sequence</param>
		/// <param name="resultSelector">Function to apply to each group of elements</param>
		public static IEnumerable<TResult> ZipEven<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth, TResult>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second, IEnumerable<TThird> third, IEnumerable<TFourth> fourth, IEnumerable<TFifth> fifth, IEnumerable<TSixth> sixth, IEnumerable<TSeventh> seventh, IEnumerable<TEighth> eighth, IEnumerable<TNinth> ninth,  Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth, int, TResult> resultSelector)
        {
			first.ThrowIfNull("first");
			second.ThrowIfNull("second");
			third.ThrowIfNull("third");
			fourth.ThrowIfNull("fourth");
			fifth.ThrowIfNull("fifth");
			sixth.ThrowIfNull("sixth");
			seventh.ThrowIfNull("seventh");
			eighth.ThrowIfNull("eighth");
			ninth.ThrowIfNull("ninth");
            resultSelector.ThrowIfNull("resultSelector");

            return ZipIter(first, second, third, fourth, fifth, sixth, seventh, eighth, ninth, resultSelector, ImbalancedZipStrategy.Fail);
        }

		/// <summary>Returns a projection of tuples, where each tuple contains the N-th element from each of the argument sequences.</summary>
        /// <remarks>
        /// If the nine input sequences are of different lengths, then the result sequence will always be as long as the longest of the input sequences.
        /// The default values of the shorter sequence element types are used for padding.
		/// A global index over all sequences is passed into the result selector function.
        /// This operator uses deferred execution and streams its results.
        /// </remarks>
        /// <example>
        /// <code>
        /// int[] numbers = { 1, 2, 3 };
        /// string[] letters = { "A", "B", "C", "D" };
        /// var zipped = numbers.ZipLongest(letters, (n, l) => n + l);
        /// </code>
        /// The <c>zipped</c> variable, when iterated over, will yield "1A", "2B", "3C", in turn.
        /// </example>
		/// <typeparam name="TFirst">Type of elements in the first sequence.</typeparam>
		/// <typeparam name="TSecond">Type of elements in the second sequence.</typeparam>
		/// <typeparam name="TThird">Type of elements in the third sequence.</typeparam>
		/// <typeparam name="TFourth">Type of elements in the fourth sequence.</typeparam>
		/// <typeparam name="TFifth">Type of elements in the fifth sequence.</typeparam>
		/// <typeparam name="TSixth">Type of elements in the sixth sequence.</typeparam>
		/// <typeparam name="TSeventh">Type of elements in the seventh sequence.</typeparam>
		/// <typeparam name="TEighth">Type of elements in the eighth sequence.</typeparam>
		/// <typeparam name="TNinth">Type of elements in the ninth sequence.</typeparam>
		/// <typeparam name="TResult">Type of elements in result sequence</typeparam>
		/// <param name="first">First sequence</param>
		/// <param name="second">Second sequence</param>
		/// <param name="third">Third sequence</param>
		/// <param name="fourth">Fourth sequence</param>
		/// <param name="fifth">Fifth sequence</param>
		/// <param name="sixth">Sixth sequence</param>
		/// <param name="seventh">Seventh sequence</param>
		/// <param name="eighth">Eighth sequence</param>
		/// <param name="ninth">Ninth sequence</param>
		/// <param name="resultSelector">Function to apply to each group of elements</param>
		public static IEnumerable<TResult> ZipLongest<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth, TResult>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second, IEnumerable<TThird> third, IEnumerable<TFourth> fourth, IEnumerable<TFifth> fifth, IEnumerable<TSixth> sixth, IEnumerable<TSeventh> seventh, IEnumerable<TEighth> eighth, IEnumerable<TNinth> ninth,  Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth, int, TResult> resultSelector)
        {
			first.ThrowIfNull("first");
			second.ThrowIfNull("second");
			third.ThrowIfNull("third");
			fourth.ThrowIfNull("fourth");
			fifth.ThrowIfNull("fifth");
			sixth.ThrowIfNull("sixth");
			seventh.ThrowIfNull("seventh");
			eighth.ThrowIfNull("eighth");
			ninth.ThrowIfNull("ninth");
            resultSelector.ThrowIfNull("resultSelector");

            return ZipIter(first, second, third, fourth, fifth, sixth, seventh, eighth, ninth, resultSelector, ImbalancedZipStrategy.Pad);
        }

		private static IEnumerable<TResult> ZipIter<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth, TResult>(IEnumerable<TFirst> first, IEnumerable<TSecond> second, IEnumerable<TThird> third, IEnumerable<TFourth> fourth, IEnumerable<TFifth> fifth, IEnumerable<TSixth> sixth, IEnumerable<TSeventh> seventh, IEnumerable<TEighth> eighth, IEnumerable<TNinth> ninth,  Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth, int, TResult> resultSelector, ImbalancedZipStrategy imbalanceStrategy)
        {
			var count = 0;
			using (var e1 = first.GetEnumerator())
			using (var e2 = second.GetEnumerator())
			using (var e3 = third.GetEnumerator())
			using (var e4 = fourth.GetEnumerator())
			using (var e5 = fifth.GetEnumerator())
			using (var e6 = sixth.GetEnumerator())
			using (var e7 = seventh.GetEnumerator())
			using (var e8 = eighth.GetEnumerator())
			using (var e9 = ninth.GetEnumerator())
            {
                bool end;
				bool fail;

			    TFirst v1 = default(TFirst);
			    TSecond v2 = default(TSecond);
			    TThird v3 = default(TThird);
			    TFourth v4 = default(TFourth);
			    TFifth v5 = default(TFifth);
			    TSixth v6 = default(TSixth);
			    TSeventh v7 = default(TSeventh);
			    TEighth v8 = default(TEighth);
			    TNinth v9 = default(TNinth);

                while (true)
                {
                    end = true;
					fail = false;


                    if (e1.MoveNext())
                    {
                        v1 = e1.Current;
                        end = false;
                    }
                    else
                    {
                        switch (imbalanceStrategy)
                        {
                            case ImbalancedZipStrategy.Fail:
                                fail = true;
								break;
                            case ImbalancedZipStrategy.Truncate:
                                yield break;
                            case ImbalancedZipStrategy.Pad:
								v1 = default(TFirst);
                                break;
                        }
                    }

                    if (e2.MoveNext())
                    {
                        v2 = e2.Current;
                        end = false;
                    }
                    else
                    {
                        switch (imbalanceStrategy)
                        {
                            case ImbalancedZipStrategy.Fail:
                                fail = true;
								break;
                            case ImbalancedZipStrategy.Truncate:
                                yield break;
                            case ImbalancedZipStrategy.Pad:
								v2 = default(TSecond);
                                break;
                        }
                    }

                    if (e3.MoveNext())
                    {
                        v3 = e3.Current;
                        end = false;
                    }
                    else
                    {
                        switch (imbalanceStrategy)
                        {
                            case ImbalancedZipStrategy.Fail:
                                fail = true;
								break;
                            case ImbalancedZipStrategy.Truncate:
                                yield break;
                            case ImbalancedZipStrategy.Pad:
								v3 = default(TThird);
                                break;
                        }
                    }

                    if (e4.MoveNext())
                    {
                        v4 = e4.Current;
                        end = false;
                    }
                    else
                    {
                        switch (imbalanceStrategy)
                        {
                            case ImbalancedZipStrategy.Fail:
                                fail = true;
								break;
                            case ImbalancedZipStrategy.Truncate:
                                yield break;
                            case ImbalancedZipStrategy.Pad:
								v4 = default(TFourth);
                                break;
                        }
                    }

                    if (e5.MoveNext())
                    {
                        v5 = e5.Current;
                        end = false;
                    }
                    else
                    {
                        switch (imbalanceStrategy)
                        {
                            case ImbalancedZipStrategy.Fail:
                                fail = true;
								break;
                            case ImbalancedZipStrategy.Truncate:
                                yield break;
                            case ImbalancedZipStrategy.Pad:
								v5 = default(TFifth);
                                break;
                        }
                    }

                    if (e6.MoveNext())
                    {
                        v6 = e6.Current;
                        end = false;
                    }
                    else
                    {
                        switch (imbalanceStrategy)
                        {
                            case ImbalancedZipStrategy.Fail:
                                fail = true;
								break;
                            case ImbalancedZipStrategy.Truncate:
                                yield break;
                            case ImbalancedZipStrategy.Pad:
								v6 = default(TSixth);
                                break;
                        }
                    }

                    if (e7.MoveNext())
                    {
                        v7 = e7.Current;
                        end = false;
                    }
                    else
                    {
                        switch (imbalanceStrategy)
                        {
                            case ImbalancedZipStrategy.Fail:
                                fail = true;
								break;
                            case ImbalancedZipStrategy.Truncate:
                                yield break;
                            case ImbalancedZipStrategy.Pad:
								v7 = default(TSeventh);
                                break;
                        }
                    }

                    if (e8.MoveNext())
                    {
                        v8 = e8.Current;
                        end = false;
                    }
                    else
                    {
                        switch (imbalanceStrategy)
                        {
                            case ImbalancedZipStrategy.Fail:
                                fail = true;
								break;
                            case ImbalancedZipStrategy.Truncate:
                                yield break;
                            case ImbalancedZipStrategy.Pad:
								v8 = default(TEighth);
                                break;
                        }
                    }

                    if (e9.MoveNext())
                    {
                        v9 = e9.Current;
                        end = false;
                    }
                    else
                    {
                        switch (imbalanceStrategy)
                        {
                            case ImbalancedZipStrategy.Fail:
                                fail = true;
								break;
                            case ImbalancedZipStrategy.Truncate:
                                yield break;
                            case ImbalancedZipStrategy.Pad:
								v9 = default(TNinth);
                                break;
                        }
                    }

					if (end)
						yield break;
					else if (fail)
						throw new InvalidOperationException("Sequences were not all same length.");
					else
						yield return resultSelector(v1, v2, v3, v4, v5, v6, v7, v8, v9, count++);
                }
            }            
        }

		/// <summary>Returns a projection of tuples, where each tuple contains the N-th element from each of the argument sequences.</summary>
        /// <remarks>
        /// If the ten input sequences are of different lengths, the result sequence is terminated as soon as the shortest input sequence is exhausted.
        /// This operator uses deferred execution and streams its results.
        /// </remarks>
        /// <example>
        /// <code>
        /// int[] numbers = { 1, 2, 3 };
        /// string[] letters = { "A", "B", "C", "D" };
        /// var zipped = numbers.ZipShortest(letters, (n, l) => n + l);
        /// </code>
        /// The <c>zipped</c> variable, when iterated over, will yield "1A", "2B", "3C", in turn.
        /// </example>
		/// <typeparam name="TFirst">Type of elements in the first sequence.</typeparam>
		/// <typeparam name="TSecond">Type of elements in the second sequence.</typeparam>
		/// <typeparam name="TThird">Type of elements in the third sequence.</typeparam>
		/// <typeparam name="TFourth">Type of elements in the fourth sequence.</typeparam>
		/// <typeparam name="TFifth">Type of elements in the fifth sequence.</typeparam>
		/// <typeparam name="TSixth">Type of elements in the sixth sequence.</typeparam>
		/// <typeparam name="TSeventh">Type of elements in the seventh sequence.</typeparam>
		/// <typeparam name="TEighth">Type of elements in the eighth sequence.</typeparam>
		/// <typeparam name="TNinth">Type of elements in the ninth sequence.</typeparam>
		/// <typeparam name="TTenth">Type of elements in the tenth sequence.</typeparam>
		/// <typeparam name="TResult">Type of elements in result sequence</typeparam>
		/// <param name="first">First sequence</param>
		/// <param name="second">Second sequence</param>
		/// <param name="third">Third sequence</param>
		/// <param name="fourth">Fourth sequence</param>
		/// <param name="fifth">Fifth sequence</param>
		/// <param name="sixth">Sixth sequence</param>
		/// <param name="seventh">Seventh sequence</param>
		/// <param name="eighth">Eighth sequence</param>
		/// <param name="ninth">Ninth sequence</param>
		/// <param name="tenth">Tenth sequence</param>
		/// <param name="resultSelector">Function to apply to each group of elements</param>
		public static IEnumerable<TResult> ZipShortest<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth, TTenth, TResult>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second, IEnumerable<TThird> third, IEnumerable<TFourth> fourth, IEnumerable<TFifth> fifth, IEnumerable<TSixth> sixth, IEnumerable<TSeventh> seventh, IEnumerable<TEighth> eighth, IEnumerable<TNinth> ninth, IEnumerable<TTenth> tenth,  Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth, TTenth, TResult> resultSelector)
        {
			first.ThrowIfNull("first");
			second.ThrowIfNull("second");
			third.ThrowIfNull("third");
			fourth.ThrowIfNull("fourth");
			fifth.ThrowIfNull("fifth");
			sixth.ThrowIfNull("sixth");
			seventh.ThrowIfNull("seventh");
			eighth.ThrowIfNull("eighth");
			ninth.ThrowIfNull("ninth");
			tenth.ThrowIfNull("tenth");
            resultSelector.ThrowIfNull("resultSelector");

            return ZipIter(first, second, third, fourth, fifth, sixth, seventh, eighth, ninth, tenth, resultSelector, ImbalancedZipStrategy.Truncate);
        }

		/// <summary>Returns a projection of tuples, where each tuple contains the N-th element from each of the argument sequences.</summary>
        /// <remarks>
        /// If the ten input sequences are of different lengths, then <see cref="InvalidOperationException"/> is thrown.
        /// This operator uses deferred execution and streams its results.
        /// </remarks>
        /// <example>
        /// <code>
        /// int[] numbers = { 1, 2, 3 };
        /// string[] letters = { "A", "B", "C", "D" };
        /// var zipped = numbers.ZipEven(letters, (n, l) => n + l);
        /// </code>
        /// The <c>zipped</c> variable, when iterated over, will yield "1A", "2B", "3C", in turn.
        /// </example>
		/// <typeparam name="TFirst">Type of elements in the first sequence.</typeparam>
		/// <typeparam name="TSecond">Type of elements in the second sequence.</typeparam>
		/// <typeparam name="TThird">Type of elements in the third sequence.</typeparam>
		/// <typeparam name="TFourth">Type of elements in the fourth sequence.</typeparam>
		/// <typeparam name="TFifth">Type of elements in the fifth sequence.</typeparam>
		/// <typeparam name="TSixth">Type of elements in the sixth sequence.</typeparam>
		/// <typeparam name="TSeventh">Type of elements in the seventh sequence.</typeparam>
		/// <typeparam name="TEighth">Type of elements in the eighth sequence.</typeparam>
		/// <typeparam name="TNinth">Type of elements in the ninth sequence.</typeparam>
		/// <typeparam name="TTenth">Type of elements in the tenth sequence.</typeparam>
		/// <typeparam name="TResult">Type of elements in result sequence</typeparam>
		/// <param name="first">First sequence</param>
		/// <param name="second">Second sequence</param>
		/// <param name="third">Third sequence</param>
		/// <param name="fourth">Fourth sequence</param>
		/// <param name="fifth">Fifth sequence</param>
		/// <param name="sixth">Sixth sequence</param>
		/// <param name="seventh">Seventh sequence</param>
		/// <param name="eighth">Eighth sequence</param>
		/// <param name="ninth">Ninth sequence</param>
		/// <param name="tenth">Tenth sequence</param>
		/// <param name="resultSelector">Function to apply to each group of elements</param>
		public static IEnumerable<TResult> ZipEven<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth, TTenth, TResult>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second, IEnumerable<TThird> third, IEnumerable<TFourth> fourth, IEnumerable<TFifth> fifth, IEnumerable<TSixth> sixth, IEnumerable<TSeventh> seventh, IEnumerable<TEighth> eighth, IEnumerable<TNinth> ninth, IEnumerable<TTenth> tenth,  Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth, TTenth, TResult> resultSelector)
        {
			first.ThrowIfNull("first");
			second.ThrowIfNull("second");
			third.ThrowIfNull("third");
			fourth.ThrowIfNull("fourth");
			fifth.ThrowIfNull("fifth");
			sixth.ThrowIfNull("sixth");
			seventh.ThrowIfNull("seventh");
			eighth.ThrowIfNull("eighth");
			ninth.ThrowIfNull("ninth");
			tenth.ThrowIfNull("tenth");
            resultSelector.ThrowIfNull("resultSelector");

            return ZipIter(first, second, third, fourth, fifth, sixth, seventh, eighth, ninth, tenth, resultSelector, ImbalancedZipStrategy.Fail);
        }

		/// <summary>Returns a projection of tuples, where each tuple contains the N-th element from each of the argument sequences.</summary>
        /// <remarks>
        /// If the ten input sequences are of different lengths, then the result sequence will always be as long as the longest of the input sequences.
        /// The default values of the shorter sequence element types are used for padding.
        /// This operator uses deferred execution and streams its results.
        /// </remarks>
        /// <example>
        /// <code>
        /// int[] numbers = { 1, 2, 3 };
        /// string[] letters = { "A", "B", "C", "D" };
        /// var zipped = numbers.ZipLongest(letters, (n, l) => n + l);
        /// </code>
        /// The <c>zipped</c> variable, when iterated over, will yield "1A", "2B", "3C", in turn.
        /// </example>
		/// <typeparam name="TFirst">Type of elements in the first sequence.</typeparam>
		/// <typeparam name="TSecond">Type of elements in the second sequence.</typeparam>
		/// <typeparam name="TThird">Type of elements in the third sequence.</typeparam>
		/// <typeparam name="TFourth">Type of elements in the fourth sequence.</typeparam>
		/// <typeparam name="TFifth">Type of elements in the fifth sequence.</typeparam>
		/// <typeparam name="TSixth">Type of elements in the sixth sequence.</typeparam>
		/// <typeparam name="TSeventh">Type of elements in the seventh sequence.</typeparam>
		/// <typeparam name="TEighth">Type of elements in the eighth sequence.</typeparam>
		/// <typeparam name="TNinth">Type of elements in the ninth sequence.</typeparam>
		/// <typeparam name="TTenth">Type of elements in the tenth sequence.</typeparam>
		/// <typeparam name="TResult">Type of elements in result sequence</typeparam>
		/// <param name="first">First sequence</param>
		/// <param name="second">Second sequence</param>
		/// <param name="third">Third sequence</param>
		/// <param name="fourth">Fourth sequence</param>
		/// <param name="fifth">Fifth sequence</param>
		/// <param name="sixth">Sixth sequence</param>
		/// <param name="seventh">Seventh sequence</param>
		/// <param name="eighth">Eighth sequence</param>
		/// <param name="ninth">Ninth sequence</param>
		/// <param name="tenth">Tenth sequence</param>
		/// <param name="resultSelector">Function to apply to each group of elements</param>
		public static IEnumerable<TResult> ZipLongest<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth, TTenth, TResult>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second, IEnumerable<TThird> third, IEnumerable<TFourth> fourth, IEnumerable<TFifth> fifth, IEnumerable<TSixth> sixth, IEnumerable<TSeventh> seventh, IEnumerable<TEighth> eighth, IEnumerable<TNinth> ninth, IEnumerable<TTenth> tenth,  Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth, TTenth, TResult> resultSelector)
        {
			first.ThrowIfNull("first");
			second.ThrowIfNull("second");
			third.ThrowIfNull("third");
			fourth.ThrowIfNull("fourth");
			fifth.ThrowIfNull("fifth");
			sixth.ThrowIfNull("sixth");
			seventh.ThrowIfNull("seventh");
			eighth.ThrowIfNull("eighth");
			ninth.ThrowIfNull("ninth");
			tenth.ThrowIfNull("tenth");
            resultSelector.ThrowIfNull("resultSelector");

            return ZipIter(first, second, third, fourth, fifth, sixth, seventh, eighth, ninth, tenth, resultSelector, ImbalancedZipStrategy.Pad);
        }

		private static IEnumerable<TResult> ZipIter<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth, TTenth, TResult>(IEnumerable<TFirst> first, IEnumerable<TSecond> second, IEnumerable<TThird> third, IEnumerable<TFourth> fourth, IEnumerable<TFifth> fifth, IEnumerable<TSixth> sixth, IEnumerable<TSeventh> seventh, IEnumerable<TEighth> eighth, IEnumerable<TNinth> ninth, IEnumerable<TTenth> tenth,  Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth, TTenth, TResult> resultSelector, ImbalancedZipStrategy imbalanceStrategy)
        {
			using (var e1 = first.GetEnumerator())
			using (var e2 = second.GetEnumerator())
			using (var e3 = third.GetEnumerator())
			using (var e4 = fourth.GetEnumerator())
			using (var e5 = fifth.GetEnumerator())
			using (var e6 = sixth.GetEnumerator())
			using (var e7 = seventh.GetEnumerator())
			using (var e8 = eighth.GetEnumerator())
			using (var e9 = ninth.GetEnumerator())
			using (var e10 = tenth.GetEnumerator())
            {
                bool end;
				bool fail;

			    TFirst v1 = default(TFirst);
			    TSecond v2 = default(TSecond);
			    TThird v3 = default(TThird);
			    TFourth v4 = default(TFourth);
			    TFifth v5 = default(TFifth);
			    TSixth v6 = default(TSixth);
			    TSeventh v7 = default(TSeventh);
			    TEighth v8 = default(TEighth);
			    TNinth v9 = default(TNinth);
			    TTenth v10 = default(TTenth);

                while (true)
                {
                    end = true;
					fail = false;


                    if (e1.MoveNext())
                    {
                        v1 = e1.Current;
                        end = false;
                    }
                    else
                    {
                        switch (imbalanceStrategy)
                        {
                            case ImbalancedZipStrategy.Fail:
								fail = true;
								break;
                            case ImbalancedZipStrategy.Truncate:
                                yield break;
                            case ImbalancedZipStrategy.Pad:
								v1 = default(TFirst);
                                break;
                        }
                    }

                    if (e2.MoveNext())
                    {
                        v2 = e2.Current;
                        end = false;
                    }
                    else
                    {
                        switch (imbalanceStrategy)
                        {
                            case ImbalancedZipStrategy.Fail:
								fail = true;
								break;
                            case ImbalancedZipStrategy.Truncate:
                                yield break;
                            case ImbalancedZipStrategy.Pad:
								v2 = default(TSecond);
                                break;
                        }
                    }

                    if (e3.MoveNext())
                    {
                        v3 = e3.Current;
                        end = false;
                    }
                    else
                    {
                        switch (imbalanceStrategy)
                        {
                            case ImbalancedZipStrategy.Fail:
								fail = true;
								break;
                            case ImbalancedZipStrategy.Truncate:
                                yield break;
                            case ImbalancedZipStrategy.Pad:
								v3 = default(TThird);
                                break;
                        }
                    }

                    if (e4.MoveNext())
                    {
                        v4 = e4.Current;
                        end = false;
                    }
                    else
                    {
                        switch (imbalanceStrategy)
                        {
                            case ImbalancedZipStrategy.Fail:
								fail = true;
								break;
                            case ImbalancedZipStrategy.Truncate:
                                yield break;
                            case ImbalancedZipStrategy.Pad:
								v4 = default(TFourth);
                                break;
                        }
                    }

                    if (e5.MoveNext())
                    {
                        v5 = e5.Current;
                        end = false;
                    }
                    else
                    {
                        switch (imbalanceStrategy)
                        {
                            case ImbalancedZipStrategy.Fail:
								fail = true;
								break;
                            case ImbalancedZipStrategy.Truncate:
                                yield break;
                            case ImbalancedZipStrategy.Pad:
								v5 = default(TFifth);
                                break;
                        }
                    }

                    if (e6.MoveNext())
                    {
                        v6 = e6.Current;
                        end = false;
                    }
                    else
                    {
                        switch (imbalanceStrategy)
                        {
                            case ImbalancedZipStrategy.Fail:
								fail = true;
								break;
                            case ImbalancedZipStrategy.Truncate:
                                yield break;
                            case ImbalancedZipStrategy.Pad:
								v6 = default(TSixth);
                                break;
                        }
                    }

                    if (e7.MoveNext())
                    {
                        v7 = e7.Current;
                        end = false;
                    }
                    else
                    {
                        switch (imbalanceStrategy)
                        {
                            case ImbalancedZipStrategy.Fail:
								fail = true;
								break;
                            case ImbalancedZipStrategy.Truncate:
                                yield break;
                            case ImbalancedZipStrategy.Pad:
								v7 = default(TSeventh);
                                break;
                        }
                    }

                    if (e8.MoveNext())
                    {
                        v8 = e8.Current;
                        end = false;
                    }
                    else
                    {
                        switch (imbalanceStrategy)
                        {
                            case ImbalancedZipStrategy.Fail:
								fail = true;
								break;
                            case ImbalancedZipStrategy.Truncate:
                                yield break;
                            case ImbalancedZipStrategy.Pad:
								v8 = default(TEighth);
                                break;
                        }
                    }

                    if (e9.MoveNext())
                    {
                        v9 = e9.Current;
                        end = false;
                    }
                    else
                    {
                        switch (imbalanceStrategy)
                        {
                            case ImbalancedZipStrategy.Fail:
								fail = true;
								break;
                            case ImbalancedZipStrategy.Truncate:
                                yield break;
                            case ImbalancedZipStrategy.Pad:
								v9 = default(TNinth);
                                break;
                        }
                    }

                    if (e10.MoveNext())
                    {
                        v10 = e10.Current;
                        end = false;
                    }
                    else
                    {
                        switch (imbalanceStrategy)
                        {
                            case ImbalancedZipStrategy.Fail:
								fail = true;
								break;
                            case ImbalancedZipStrategy.Truncate:
                                yield break;
                            case ImbalancedZipStrategy.Pad:
								v10 = default(TTenth);
                                break;
                        }
                    }

					if (end)
						yield break;
					else if (fail)
						throw new InvalidOperationException("Sequences were not all same length.");
					else
						yield return resultSelector(v1, v2, v3, v4, v5, v6, v7, v8, v9, v10);
                }
            }            
        }

		/// <summary>Returns a projection of tuples, where each tuple contains the N-th element from each of the argument sequences.</summary>
        /// <remarks>
        /// If the ten input sequences are of different lengths, the result sequence is terminated as soon as the shortest input sequence is exhausted.
		/// A global index over all sequences is passed into the result selector function.
        /// This operator uses deferred execution and streams its results.
        /// </remarks>
        /// <example>
        /// <code>
        /// int[] numbers = { 1, 2, 3 };
        /// string[] letters = { "A", "B", "C", "D" };
        /// var zipped = numbers.ZipShortest(letters, (n, l) => n + l);
        /// </code>
        /// The <c>zipped</c> variable, when iterated over, will yield "1A", "2B", "3C", in turn.
        /// </example>
		/// <typeparam name="TFirst">Type of elements in the first sequence.</typeparam>
		/// <typeparam name="TSecond">Type of elements in the second sequence.</typeparam>
		/// <typeparam name="TThird">Type of elements in the third sequence.</typeparam>
		/// <typeparam name="TFourth">Type of elements in the fourth sequence.</typeparam>
		/// <typeparam name="TFifth">Type of elements in the fifth sequence.</typeparam>
		/// <typeparam name="TSixth">Type of elements in the sixth sequence.</typeparam>
		/// <typeparam name="TSeventh">Type of elements in the seventh sequence.</typeparam>
		/// <typeparam name="TEighth">Type of elements in the eighth sequence.</typeparam>
		/// <typeparam name="TNinth">Type of elements in the ninth sequence.</typeparam>
		/// <typeparam name="TTenth">Type of elements in the tenth sequence.</typeparam>
		/// <typeparam name="TResult">Type of elements in result sequence</typeparam>
		/// <param name="first">First sequence</param>
		/// <param name="second">Second sequence</param>
		/// <param name="third">Third sequence</param>
		/// <param name="fourth">Fourth sequence</param>
		/// <param name="fifth">Fifth sequence</param>
		/// <param name="sixth">Sixth sequence</param>
		/// <param name="seventh">Seventh sequence</param>
		/// <param name="eighth">Eighth sequence</param>
		/// <param name="ninth">Ninth sequence</param>
		/// <param name="tenth">Tenth sequence</param>
		/// <param name="resultSelector">Function to apply to each group of elements</param>
		public static IEnumerable<TResult> ZipShortest<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth, TTenth, TResult>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second, IEnumerable<TThird> third, IEnumerable<TFourth> fourth, IEnumerable<TFifth> fifth, IEnumerable<TSixth> sixth, IEnumerable<TSeventh> seventh, IEnumerable<TEighth> eighth, IEnumerable<TNinth> ninth, IEnumerable<TTenth> tenth,  Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth, TTenth, int, TResult> resultSelector)
        {
			first.ThrowIfNull("first");
			second.ThrowIfNull("second");
			third.ThrowIfNull("third");
			fourth.ThrowIfNull("fourth");
			fifth.ThrowIfNull("fifth");
			sixth.ThrowIfNull("sixth");
			seventh.ThrowIfNull("seventh");
			eighth.ThrowIfNull("eighth");
			ninth.ThrowIfNull("ninth");
			tenth.ThrowIfNull("tenth");
            resultSelector.ThrowIfNull("resultSelector");

            return ZipIter(first, second, third, fourth, fifth, sixth, seventh, eighth, ninth, tenth, resultSelector, ImbalancedZipStrategy.Truncate);
        }

		/// <summary>Returns a projection of tuples, where each tuple contains the N-th element from each of the argument sequences.</summary>
        /// <remarks>
        /// If the ten input sequences are of different lengths, then <see cref="InvalidOperationException"/> is thrown.
		/// A global index over all sequences is passed into the result selector function.
        /// This operator uses deferred execution and streams its results.
        /// </remarks>
        /// <example>
        /// <code>
        /// int[] numbers = { 1, 2, 3 };
        /// string[] letters = { "A", "B", "C", "D" };
        /// var zipped = numbers.ZipEven(letters, (n, l) => n + l);
        /// </code>
        /// The <c>zipped</c> variable, when iterated over, will yield "1A", "2B", "3C", in turn.
        /// </example>
		/// <typeparam name="TFirst">Type of elements in the first sequence.</typeparam>
		/// <typeparam name="TSecond">Type of elements in the second sequence.</typeparam>
		/// <typeparam name="TThird">Type of elements in the third sequence.</typeparam>
		/// <typeparam name="TFourth">Type of elements in the fourth sequence.</typeparam>
		/// <typeparam name="TFifth">Type of elements in the fifth sequence.</typeparam>
		/// <typeparam name="TSixth">Type of elements in the sixth sequence.</typeparam>
		/// <typeparam name="TSeventh">Type of elements in the seventh sequence.</typeparam>
		/// <typeparam name="TEighth">Type of elements in the eighth sequence.</typeparam>
		/// <typeparam name="TNinth">Type of elements in the ninth sequence.</typeparam>
		/// <typeparam name="TTenth">Type of elements in the tenth sequence.</typeparam>
		/// <typeparam name="TResult">Type of elements in result sequence</typeparam>
		/// <param name="first">First sequence</param>
		/// <param name="second">Second sequence</param>
		/// <param name="third">Third sequence</param>
		/// <param name="fourth">Fourth sequence</param>
		/// <param name="fifth">Fifth sequence</param>
		/// <param name="sixth">Sixth sequence</param>
		/// <param name="seventh">Seventh sequence</param>
		/// <param name="eighth">Eighth sequence</param>
		/// <param name="ninth">Ninth sequence</param>
		/// <param name="tenth">Tenth sequence</param>
		/// <param name="resultSelector">Function to apply to each group of elements</param>
		public static IEnumerable<TResult> ZipEven<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth, TTenth, TResult>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second, IEnumerable<TThird> third, IEnumerable<TFourth> fourth, IEnumerable<TFifth> fifth, IEnumerable<TSixth> sixth, IEnumerable<TSeventh> seventh, IEnumerable<TEighth> eighth, IEnumerable<TNinth> ninth, IEnumerable<TTenth> tenth,  Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth, TTenth, int, TResult> resultSelector)
        {
			first.ThrowIfNull("first");
			second.ThrowIfNull("second");
			third.ThrowIfNull("third");
			fourth.ThrowIfNull("fourth");
			fifth.ThrowIfNull("fifth");
			sixth.ThrowIfNull("sixth");
			seventh.ThrowIfNull("seventh");
			eighth.ThrowIfNull("eighth");
			ninth.ThrowIfNull("ninth");
			tenth.ThrowIfNull("tenth");
            resultSelector.ThrowIfNull("resultSelector");

            return ZipIter(first, second, third, fourth, fifth, sixth, seventh, eighth, ninth, tenth, resultSelector, ImbalancedZipStrategy.Fail);
        }

		/// <summary>Returns a projection of tuples, where each tuple contains the N-th element from each of the argument sequences.</summary>
        /// <remarks>
        /// If the ten input sequences are of different lengths, then the result sequence will always be as long as the longest of the input sequences.
        /// The default values of the shorter sequence element types are used for padding.
		/// A global index over all sequences is passed into the result selector function.
        /// This operator uses deferred execution and streams its results.
        /// </remarks>
        /// <example>
        /// <code>
        /// int[] numbers = { 1, 2, 3 };
        /// string[] letters = { "A", "B", "C", "D" };
        /// var zipped = numbers.ZipLongest(letters, (n, l) => n + l);
        /// </code>
        /// The <c>zipped</c> variable, when iterated over, will yield "1A", "2B", "3C", in turn.
        /// </example>
		/// <typeparam name="TFirst">Type of elements in the first sequence.</typeparam>
		/// <typeparam name="TSecond">Type of elements in the second sequence.</typeparam>
		/// <typeparam name="TThird">Type of elements in the third sequence.</typeparam>
		/// <typeparam name="TFourth">Type of elements in the fourth sequence.</typeparam>
		/// <typeparam name="TFifth">Type of elements in the fifth sequence.</typeparam>
		/// <typeparam name="TSixth">Type of elements in the sixth sequence.</typeparam>
		/// <typeparam name="TSeventh">Type of elements in the seventh sequence.</typeparam>
		/// <typeparam name="TEighth">Type of elements in the eighth sequence.</typeparam>
		/// <typeparam name="TNinth">Type of elements in the ninth sequence.</typeparam>
		/// <typeparam name="TTenth">Type of elements in the tenth sequence.</typeparam>
		/// <typeparam name="TResult">Type of elements in result sequence</typeparam>
		/// <param name="first">First sequence</param>
		/// <param name="second">Second sequence</param>
		/// <param name="third">Third sequence</param>
		/// <param name="fourth">Fourth sequence</param>
		/// <param name="fifth">Fifth sequence</param>
		/// <param name="sixth">Sixth sequence</param>
		/// <param name="seventh">Seventh sequence</param>
		/// <param name="eighth">Eighth sequence</param>
		/// <param name="ninth">Ninth sequence</param>
		/// <param name="tenth">Tenth sequence</param>
		/// <param name="resultSelector">Function to apply to each group of elements</param>
		public static IEnumerable<TResult> ZipLongest<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth, TTenth, TResult>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second, IEnumerable<TThird> third, IEnumerable<TFourth> fourth, IEnumerable<TFifth> fifth, IEnumerable<TSixth> sixth, IEnumerable<TSeventh> seventh, IEnumerable<TEighth> eighth, IEnumerable<TNinth> ninth, IEnumerable<TTenth> tenth,  Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth, TTenth, int, TResult> resultSelector)
        {
			first.ThrowIfNull("first");
			second.ThrowIfNull("second");
			third.ThrowIfNull("third");
			fourth.ThrowIfNull("fourth");
			fifth.ThrowIfNull("fifth");
			sixth.ThrowIfNull("sixth");
			seventh.ThrowIfNull("seventh");
			eighth.ThrowIfNull("eighth");
			ninth.ThrowIfNull("ninth");
			tenth.ThrowIfNull("tenth");
            resultSelector.ThrowIfNull("resultSelector");

            return ZipIter(first, second, third, fourth, fifth, sixth, seventh, eighth, ninth, tenth, resultSelector, ImbalancedZipStrategy.Pad);
        }

		private static IEnumerable<TResult> ZipIter<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth, TTenth, TResult>(IEnumerable<TFirst> first, IEnumerable<TSecond> second, IEnumerable<TThird> third, IEnumerable<TFourth> fourth, IEnumerable<TFifth> fifth, IEnumerable<TSixth> sixth, IEnumerable<TSeventh> seventh, IEnumerable<TEighth> eighth, IEnumerable<TNinth> ninth, IEnumerable<TTenth> tenth,  Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth, TTenth, int, TResult> resultSelector, ImbalancedZipStrategy imbalanceStrategy)
        {
			var count = 0;
			using (var e1 = first.GetEnumerator())
			using (var e2 = second.GetEnumerator())
			using (var e3 = third.GetEnumerator())
			using (var e4 = fourth.GetEnumerator())
			using (var e5 = fifth.GetEnumerator())
			using (var e6 = sixth.GetEnumerator())
			using (var e7 = seventh.GetEnumerator())
			using (var e8 = eighth.GetEnumerator())
			using (var e9 = ninth.GetEnumerator())
			using (var e10 = tenth.GetEnumerator())
            {
                bool end;
				bool fail;

			    TFirst v1 = default(TFirst);
			    TSecond v2 = default(TSecond);
			    TThird v3 = default(TThird);
			    TFourth v4 = default(TFourth);
			    TFifth v5 = default(TFifth);
			    TSixth v6 = default(TSixth);
			    TSeventh v7 = default(TSeventh);
			    TEighth v8 = default(TEighth);
			    TNinth v9 = default(TNinth);
			    TTenth v10 = default(TTenth);

                while (true)
                {
                    end = true;
					fail = false;


                    if (e1.MoveNext())
                    {
                        v1 = e1.Current;
                        end = false;
                    }
                    else
                    {
                        switch (imbalanceStrategy)
                        {
                            case ImbalancedZipStrategy.Fail:
                                fail = true;
								break;
                            case ImbalancedZipStrategy.Truncate:
                                yield break;
                            case ImbalancedZipStrategy.Pad:
								v1 = default(TFirst);
                                break;
                        }
                    }

                    if (e2.MoveNext())
                    {
                        v2 = e2.Current;
                        end = false;
                    }
                    else
                    {
                        switch (imbalanceStrategy)
                        {
                            case ImbalancedZipStrategy.Fail:
                                fail = true;
								break;
                            case ImbalancedZipStrategy.Truncate:
                                yield break;
                            case ImbalancedZipStrategy.Pad:
								v2 = default(TSecond);
                                break;
                        }
                    }

                    if (e3.MoveNext())
                    {
                        v3 = e3.Current;
                        end = false;
                    }
                    else
                    {
                        switch (imbalanceStrategy)
                        {
                            case ImbalancedZipStrategy.Fail:
                                fail = true;
								break;
                            case ImbalancedZipStrategy.Truncate:
                                yield break;
                            case ImbalancedZipStrategy.Pad:
								v3 = default(TThird);
                                break;
                        }
                    }

                    if (e4.MoveNext())
                    {
                        v4 = e4.Current;
                        end = false;
                    }
                    else
                    {
                        switch (imbalanceStrategy)
                        {
                            case ImbalancedZipStrategy.Fail:
                                fail = true;
								break;
                            case ImbalancedZipStrategy.Truncate:
                                yield break;
                            case ImbalancedZipStrategy.Pad:
								v4 = default(TFourth);
                                break;
                        }
                    }

                    if (e5.MoveNext())
                    {
                        v5 = e5.Current;
                        end = false;
                    }
                    else
                    {
                        switch (imbalanceStrategy)
                        {
                            case ImbalancedZipStrategy.Fail:
                                fail = true;
								break;
                            case ImbalancedZipStrategy.Truncate:
                                yield break;
                            case ImbalancedZipStrategy.Pad:
								v5 = default(TFifth);
                                break;
                        }
                    }

                    if (e6.MoveNext())
                    {
                        v6 = e6.Current;
                        end = false;
                    }
                    else
                    {
                        switch (imbalanceStrategy)
                        {
                            case ImbalancedZipStrategy.Fail:
                                fail = true;
								break;
                            case ImbalancedZipStrategy.Truncate:
                                yield break;
                            case ImbalancedZipStrategy.Pad:
								v6 = default(TSixth);
                                break;
                        }
                    }

                    if (e7.MoveNext())
                    {
                        v7 = e7.Current;
                        end = false;
                    }
                    else
                    {
                        switch (imbalanceStrategy)
                        {
                            case ImbalancedZipStrategy.Fail:
                                fail = true;
								break;
                            case ImbalancedZipStrategy.Truncate:
                                yield break;
                            case ImbalancedZipStrategy.Pad:
								v7 = default(TSeventh);
                                break;
                        }
                    }

                    if (e8.MoveNext())
                    {
                        v8 = e8.Current;
                        end = false;
                    }
                    else
                    {
                        switch (imbalanceStrategy)
                        {
                            case ImbalancedZipStrategy.Fail:
                                fail = true;
								break;
                            case ImbalancedZipStrategy.Truncate:
                                yield break;
                            case ImbalancedZipStrategy.Pad:
								v8 = default(TEighth);
                                break;
                        }
                    }

                    if (e9.MoveNext())
                    {
                        v9 = e9.Current;
                        end = false;
                    }
                    else
                    {
                        switch (imbalanceStrategy)
                        {
                            case ImbalancedZipStrategy.Fail:
                                fail = true;
								break;
                            case ImbalancedZipStrategy.Truncate:
                                yield break;
                            case ImbalancedZipStrategy.Pad:
								v9 = default(TNinth);
                                break;
                        }
                    }

                    if (e10.MoveNext())
                    {
                        v10 = e10.Current;
                        end = false;
                    }
                    else
                    {
                        switch (imbalanceStrategy)
                        {
                            case ImbalancedZipStrategy.Fail:
                                fail = true;
								break;
                            case ImbalancedZipStrategy.Truncate:
                                yield break;
                            case ImbalancedZipStrategy.Pad:
								v10 = default(TTenth);
                                break;
                        }
                    }

					if (end)
						yield break;
					else if (fail)
						throw new InvalidOperationException("Sequences were not all same length.");
					else
						yield return resultSelector(v1, v2, v3, v4, v5, v6, v7, v8, v9, v10, count++);
                }
            }            
        }

#if V20 || V30 || V35
		/// <summary>Returns a projection of tuples, where each tuple contains the N-th element from each of the argument sequences.</summary>
        /// <remarks>
        /// If the two input sequences are of different lengths, the result sequence is terminated as soon as the shortest input sequence is exhausted.
        /// This operator uses deferred execution and streams its results.
        /// </remarks>
        /// <example>
        /// <code>
        /// int[] numbers = { 1, 2, 3 };
        /// string[] letters = { "A", "B", "C", "D" };
        /// var zipped = numbers.ZipShortest(letters, (n, l) => n + l);
        /// </code>
        /// The <c>zipped</c> variable, when iterated over, will yield "1A", "2B", "3C", in turn.
        /// </example>
		/// <typeparam name="TFirst">Type of elements in the first sequence.</typeparam>
		/// <typeparam name="TSecond">Type of elements in the second sequence.</typeparam>
		/// <typeparam name="TResult">Type of elements in result sequence</typeparam>
		/// <param name="first">First sequence</param>
		/// <param name="second">Second sequence</param>
		/// <param name="resultSelector">Function to apply to each group of elements</param>
		public static IEnumerable<TResult> Zip<TFirst, TSecond, TResult>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second,  Func<TFirst, TSecond, TResult> resultSelector)
        {
			return ZipShortest(first, second, resultSelector);
        }
#endif
		/// <summary>Returns a projection of tuples, where each tuple contains the N-th element from each of the argument sequences.</summary>
        /// <remarks>
        /// If the two input sequences are of different lengths, the result sequence is terminated as soon as the shortest input sequence is exhausted.
		/// A global index over all sequences is passed into the result selector function.
        /// This operator uses deferred execution and streams its results.
        /// </remarks>
        /// <example>
        /// <code>
        /// int[] numbers = { 1, 2, 3 };
        /// string[] letters = { "A", "B", "C", "D" };
        /// var zipped = numbers.ZipShortest(letters, (n, l) => n + l);
        /// </code>
        /// The <c>zipped</c> variable, when iterated over, will yield "1A", "2B", "3C", in turn.
        /// </example>
		/// <typeparam name="TFirst">Type of elements in the first sequence.</typeparam>
		/// <typeparam name="TSecond">Type of elements in the second sequence.</typeparam>
		/// <typeparam name="TResult">Type of elements in result sequence</typeparam>
		/// <param name="first">First sequence</param>
		/// <param name="second">Second sequence</param>
		/// <param name="resultSelector">Function to apply to each group of elements</param>
		public static IEnumerable<TResult> Zip<TFirst, TSecond, TResult>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second,  Func<TFirst, TSecond, int, TResult> resultSelector)
        {
			return ZipShortest(first, second, resultSelector);
        }
#if V20 || V30 || V35
		/// <summary>Returns a projection of tuples, where each tuple contains the N-th element from each of the argument sequences.</summary>
        /// <remarks>
        /// If the three input sequences are of different lengths, the result sequence is terminated as soon as the shortest input sequence is exhausted.
        /// This operator uses deferred execution and streams its results.
        /// </remarks>
        /// <example>
        /// <code>
        /// int[] numbers = { 1, 2, 3 };
        /// string[] letters = { "A", "B", "C", "D" };
        /// var zipped = numbers.ZipShortest(letters, (n, l) => n + l);
        /// </code>
        /// The <c>zipped</c> variable, when iterated over, will yield "1A", "2B", "3C", in turn.
        /// </example>
		/// <typeparam name="TFirst">Type of elements in the first sequence.</typeparam>
		/// <typeparam name="TSecond">Type of elements in the second sequence.</typeparam>
		/// <typeparam name="TThird">Type of elements in the third sequence.</typeparam>
		/// <typeparam name="TResult">Type of elements in result sequence</typeparam>
		/// <param name="first">First sequence</param>
		/// <param name="second">Second sequence</param>
		/// <param name="third">Third sequence</param>
		/// <param name="resultSelector">Function to apply to each group of elements</param>
		public static IEnumerable<TResult> Zip<TFirst, TSecond, TThird, TResult>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second, IEnumerable<TThird> third,  Func<TFirst, TSecond, TThird, TResult> resultSelector)
        {
			return ZipShortest(first, second, third, resultSelector);
        }
#endif
		/// <summary>Returns a projection of tuples, where each tuple contains the N-th element from each of the argument sequences.</summary>
        /// <remarks>
        /// If the three input sequences are of different lengths, the result sequence is terminated as soon as the shortest input sequence is exhausted.
		/// A global index over all sequences is passed into the result selector function.
        /// This operator uses deferred execution and streams its results.
        /// </remarks>
        /// <example>
        /// <code>
        /// int[] numbers = { 1, 2, 3 };
        /// string[] letters = { "A", "B", "C", "D" };
        /// var zipped = numbers.ZipShortest(letters, (n, l) => n + l);
        /// </code>
        /// The <c>zipped</c> variable, when iterated over, will yield "1A", "2B", "3C", in turn.
        /// </example>
		/// <typeparam name="TFirst">Type of elements in the first sequence.</typeparam>
		/// <typeparam name="TSecond">Type of elements in the second sequence.</typeparam>
		/// <typeparam name="TThird">Type of elements in the third sequence.</typeparam>
		/// <typeparam name="TResult">Type of elements in result sequence</typeparam>
		/// <param name="first">First sequence</param>
		/// <param name="second">Second sequence</param>
		/// <param name="third">Third sequence</param>
		/// <param name="resultSelector">Function to apply to each group of elements</param>
		public static IEnumerable<TResult> Zip<TFirst, TSecond, TThird, TResult>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second, IEnumerable<TThird> third,  Func<TFirst, TSecond, TThird, int, TResult> resultSelector)
        {
			return ZipShortest(first, second, third, resultSelector);
        }
#if V20 || V30 || V35
		/// <summary>Returns a projection of tuples, where each tuple contains the N-th element from each of the argument sequences.</summary>
        /// <remarks>
        /// If the four input sequences are of different lengths, the result sequence is terminated as soon as the shortest input sequence is exhausted.
        /// This operator uses deferred execution and streams its results.
        /// </remarks>
        /// <example>
        /// <code>
        /// int[] numbers = { 1, 2, 3 };
        /// string[] letters = { "A", "B", "C", "D" };
        /// var zipped = numbers.ZipShortest(letters, (n, l) => n + l);
        /// </code>
        /// The <c>zipped</c> variable, when iterated over, will yield "1A", "2B", "3C", in turn.
        /// </example>
		/// <typeparam name="TFirst">Type of elements in the first sequence.</typeparam>
		/// <typeparam name="TSecond">Type of elements in the second sequence.</typeparam>
		/// <typeparam name="TThird">Type of elements in the third sequence.</typeparam>
		/// <typeparam name="TFourth">Type of elements in the fourth sequence.</typeparam>
		/// <typeparam name="TResult">Type of elements in result sequence</typeparam>
		/// <param name="first">First sequence</param>
		/// <param name="second">Second sequence</param>
		/// <param name="third">Third sequence</param>
		/// <param name="fourth">Fourth sequence</param>
		/// <param name="resultSelector">Function to apply to each group of elements</param>
		public static IEnumerable<TResult> Zip<TFirst, TSecond, TThird, TFourth, TResult>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second, IEnumerable<TThird> third, IEnumerable<TFourth> fourth,  Func<TFirst, TSecond, TThird, TFourth, TResult> resultSelector)
        {
			return ZipShortest(first, second, third, fourth, resultSelector);
        }
#endif
		/// <summary>Returns a projection of tuples, where each tuple contains the N-th element from each of the argument sequences.</summary>
        /// <remarks>
        /// If the four input sequences are of different lengths, the result sequence is terminated as soon as the shortest input sequence is exhausted.
		/// A global index over all sequences is passed into the result selector function.
        /// This operator uses deferred execution and streams its results.
        /// </remarks>
        /// <example>
        /// <code>
        /// int[] numbers = { 1, 2, 3 };
        /// string[] letters = { "A", "B", "C", "D" };
        /// var zipped = numbers.ZipShortest(letters, (n, l) => n + l);
        /// </code>
        /// The <c>zipped</c> variable, when iterated over, will yield "1A", "2B", "3C", in turn.
        /// </example>
		/// <typeparam name="TFirst">Type of elements in the first sequence.</typeparam>
		/// <typeparam name="TSecond">Type of elements in the second sequence.</typeparam>
		/// <typeparam name="TThird">Type of elements in the third sequence.</typeparam>
		/// <typeparam name="TFourth">Type of elements in the fourth sequence.</typeparam>
		/// <typeparam name="TResult">Type of elements in result sequence</typeparam>
		/// <param name="first">First sequence</param>
		/// <param name="second">Second sequence</param>
		/// <param name="third">Third sequence</param>
		/// <param name="fourth">Fourth sequence</param>
		/// <param name="resultSelector">Function to apply to each group of elements</param>
		public static IEnumerable<TResult> Zip<TFirst, TSecond, TThird, TFourth, TResult>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second, IEnumerable<TThird> third, IEnumerable<TFourth> fourth,  Func<TFirst, TSecond, TThird, TFourth, int, TResult> resultSelector)
        {
			return ZipShortest(first, second, third, fourth, resultSelector);
        }
#if V20 || V30 || V35
		/// <summary>Returns a projection of tuples, where each tuple contains the N-th element from each of the argument sequences.</summary>
        /// <remarks>
        /// If the five input sequences are of different lengths, the result sequence is terminated as soon as the shortest input sequence is exhausted.
        /// This operator uses deferred execution and streams its results.
        /// </remarks>
        /// <example>
        /// <code>
        /// int[] numbers = { 1, 2, 3 };
        /// string[] letters = { "A", "B", "C", "D" };
        /// var zipped = numbers.ZipShortest(letters, (n, l) => n + l);
        /// </code>
        /// The <c>zipped</c> variable, when iterated over, will yield "1A", "2B", "3C", in turn.
        /// </example>
		/// <typeparam name="TFirst">Type of elements in the first sequence.</typeparam>
		/// <typeparam name="TSecond">Type of elements in the second sequence.</typeparam>
		/// <typeparam name="TThird">Type of elements in the third sequence.</typeparam>
		/// <typeparam name="TFourth">Type of elements in the fourth sequence.</typeparam>
		/// <typeparam name="TFifth">Type of elements in the fifth sequence.</typeparam>
		/// <typeparam name="TResult">Type of elements in result sequence</typeparam>
		/// <param name="first">First sequence</param>
		/// <param name="second">Second sequence</param>
		/// <param name="third">Third sequence</param>
		/// <param name="fourth">Fourth sequence</param>
		/// <param name="fifth">Fifth sequence</param>
		/// <param name="resultSelector">Function to apply to each group of elements</param>
		public static IEnumerable<TResult> Zip<TFirst, TSecond, TThird, TFourth, TFifth, TResult>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second, IEnumerable<TThird> third, IEnumerable<TFourth> fourth, IEnumerable<TFifth> fifth,  Func<TFirst, TSecond, TThird, TFourth, TFifth, TResult> resultSelector)
        {
			return ZipShortest(first, second, third, fourth, fifth, resultSelector);
        }
#endif
		/// <summary>Returns a projection of tuples, where each tuple contains the N-th element from each of the argument sequences.</summary>
        /// <remarks>
        /// If the five input sequences are of different lengths, the result sequence is terminated as soon as the shortest input sequence is exhausted.
		/// A global index over all sequences is passed into the result selector function.
        /// This operator uses deferred execution and streams its results.
        /// </remarks>
        /// <example>
        /// <code>
        /// int[] numbers = { 1, 2, 3 };
        /// string[] letters = { "A", "B", "C", "D" };
        /// var zipped = numbers.ZipShortest(letters, (n, l) => n + l);
        /// </code>
        /// The <c>zipped</c> variable, when iterated over, will yield "1A", "2B", "3C", in turn.
        /// </example>
		/// <typeparam name="TFirst">Type of elements in the first sequence.</typeparam>
		/// <typeparam name="TSecond">Type of elements in the second sequence.</typeparam>
		/// <typeparam name="TThird">Type of elements in the third sequence.</typeparam>
		/// <typeparam name="TFourth">Type of elements in the fourth sequence.</typeparam>
		/// <typeparam name="TFifth">Type of elements in the fifth sequence.</typeparam>
		/// <typeparam name="TResult">Type of elements in result sequence</typeparam>
		/// <param name="first">First sequence</param>
		/// <param name="second">Second sequence</param>
		/// <param name="third">Third sequence</param>
		/// <param name="fourth">Fourth sequence</param>
		/// <param name="fifth">Fifth sequence</param>
		/// <param name="resultSelector">Function to apply to each group of elements</param>
		public static IEnumerable<TResult> Zip<TFirst, TSecond, TThird, TFourth, TFifth, TResult>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second, IEnumerable<TThird> third, IEnumerable<TFourth> fourth, IEnumerable<TFifth> fifth,  Func<TFirst, TSecond, TThird, TFourth, TFifth, int, TResult> resultSelector)
        {
			return ZipShortest(first, second, third, fourth, fifth, resultSelector);
        }
#if V20 || V30 || V35
		/// <summary>Returns a projection of tuples, where each tuple contains the N-th element from each of the argument sequences.</summary>
        /// <remarks>
        /// If the six input sequences are of different lengths, the result sequence is terminated as soon as the shortest input sequence is exhausted.
        /// This operator uses deferred execution and streams its results.
        /// </remarks>
        /// <example>
        /// <code>
        /// int[] numbers = { 1, 2, 3 };
        /// string[] letters = { "A", "B", "C", "D" };
        /// var zipped = numbers.ZipShortest(letters, (n, l) => n + l);
        /// </code>
        /// The <c>zipped</c> variable, when iterated over, will yield "1A", "2B", "3C", in turn.
        /// </example>
		/// <typeparam name="TFirst">Type of elements in the first sequence.</typeparam>
		/// <typeparam name="TSecond">Type of elements in the second sequence.</typeparam>
		/// <typeparam name="TThird">Type of elements in the third sequence.</typeparam>
		/// <typeparam name="TFourth">Type of elements in the fourth sequence.</typeparam>
		/// <typeparam name="TFifth">Type of elements in the fifth sequence.</typeparam>
		/// <typeparam name="TSixth">Type of elements in the sixth sequence.</typeparam>
		/// <typeparam name="TResult">Type of elements in result sequence</typeparam>
		/// <param name="first">First sequence</param>
		/// <param name="second">Second sequence</param>
		/// <param name="third">Third sequence</param>
		/// <param name="fourth">Fourth sequence</param>
		/// <param name="fifth">Fifth sequence</param>
		/// <param name="sixth">Sixth sequence</param>
		/// <param name="resultSelector">Function to apply to each group of elements</param>
		public static IEnumerable<TResult> Zip<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TResult>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second, IEnumerable<TThird> third, IEnumerable<TFourth> fourth, IEnumerable<TFifth> fifth, IEnumerable<TSixth> sixth,  Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TResult> resultSelector)
        {
			return ZipShortest(first, second, third, fourth, fifth, sixth, resultSelector);
        }
#endif
		/// <summary>Returns a projection of tuples, where each tuple contains the N-th element from each of the argument sequences.</summary>
        /// <remarks>
        /// If the six input sequences are of different lengths, the result sequence is terminated as soon as the shortest input sequence is exhausted.
		/// A global index over all sequences is passed into the result selector function.
        /// This operator uses deferred execution and streams its results.
        /// </remarks>
        /// <example>
        /// <code>
        /// int[] numbers = { 1, 2, 3 };
        /// string[] letters = { "A", "B", "C", "D" };
        /// var zipped = numbers.ZipShortest(letters, (n, l) => n + l);
        /// </code>
        /// The <c>zipped</c> variable, when iterated over, will yield "1A", "2B", "3C", in turn.
        /// </example>
		/// <typeparam name="TFirst">Type of elements in the first sequence.</typeparam>
		/// <typeparam name="TSecond">Type of elements in the second sequence.</typeparam>
		/// <typeparam name="TThird">Type of elements in the third sequence.</typeparam>
		/// <typeparam name="TFourth">Type of elements in the fourth sequence.</typeparam>
		/// <typeparam name="TFifth">Type of elements in the fifth sequence.</typeparam>
		/// <typeparam name="TSixth">Type of elements in the sixth sequence.</typeparam>
		/// <typeparam name="TResult">Type of elements in result sequence</typeparam>
		/// <param name="first">First sequence</param>
		/// <param name="second">Second sequence</param>
		/// <param name="third">Third sequence</param>
		/// <param name="fourth">Fourth sequence</param>
		/// <param name="fifth">Fifth sequence</param>
		/// <param name="sixth">Sixth sequence</param>
		/// <param name="resultSelector">Function to apply to each group of elements</param>
		public static IEnumerable<TResult> Zip<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TResult>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second, IEnumerable<TThird> third, IEnumerable<TFourth> fourth, IEnumerable<TFifth> fifth, IEnumerable<TSixth> sixth,  Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, int, TResult> resultSelector)
        {
			return ZipShortest(first, second, third, fourth, fifth, sixth, resultSelector);
        }
#if V20 || V30 || V35
		/// <summary>Returns a projection of tuples, where each tuple contains the N-th element from each of the argument sequences.</summary>
        /// <remarks>
        /// If the seven input sequences are of different lengths, the result sequence is terminated as soon as the shortest input sequence is exhausted.
        /// This operator uses deferred execution and streams its results.
        /// </remarks>
        /// <example>
        /// <code>
        /// int[] numbers = { 1, 2, 3 };
        /// string[] letters = { "A", "B", "C", "D" };
        /// var zipped = numbers.ZipShortest(letters, (n, l) => n + l);
        /// </code>
        /// The <c>zipped</c> variable, when iterated over, will yield "1A", "2B", "3C", in turn.
        /// </example>
		/// <typeparam name="TFirst">Type of elements in the first sequence.</typeparam>
		/// <typeparam name="TSecond">Type of elements in the second sequence.</typeparam>
		/// <typeparam name="TThird">Type of elements in the third sequence.</typeparam>
		/// <typeparam name="TFourth">Type of elements in the fourth sequence.</typeparam>
		/// <typeparam name="TFifth">Type of elements in the fifth sequence.</typeparam>
		/// <typeparam name="TSixth">Type of elements in the sixth sequence.</typeparam>
		/// <typeparam name="TSeventh">Type of elements in the seventh sequence.</typeparam>
		/// <typeparam name="TResult">Type of elements in result sequence</typeparam>
		/// <param name="first">First sequence</param>
		/// <param name="second">Second sequence</param>
		/// <param name="third">Third sequence</param>
		/// <param name="fourth">Fourth sequence</param>
		/// <param name="fifth">Fifth sequence</param>
		/// <param name="sixth">Sixth sequence</param>
		/// <param name="seventh">Seventh sequence</param>
		/// <param name="resultSelector">Function to apply to each group of elements</param>
		public static IEnumerable<TResult> Zip<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TResult>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second, IEnumerable<TThird> third, IEnumerable<TFourth> fourth, IEnumerable<TFifth> fifth, IEnumerable<TSixth> sixth, IEnumerable<TSeventh> seventh,  Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TResult> resultSelector)
        {
			return ZipShortest(first, second, third, fourth, fifth, sixth, seventh, resultSelector);
        }
#endif
		/// <summary>Returns a projection of tuples, where each tuple contains the N-th element from each of the argument sequences.</summary>
        /// <remarks>
        /// If the seven input sequences are of different lengths, the result sequence is terminated as soon as the shortest input sequence is exhausted.
		/// A global index over all sequences is passed into the result selector function.
        /// This operator uses deferred execution and streams its results.
        /// </remarks>
        /// <example>
        /// <code>
        /// int[] numbers = { 1, 2, 3 };
        /// string[] letters = { "A", "B", "C", "D" };
        /// var zipped = numbers.ZipShortest(letters, (n, l) => n + l);
        /// </code>
        /// The <c>zipped</c> variable, when iterated over, will yield "1A", "2B", "3C", in turn.
        /// </example>
		/// <typeparam name="TFirst">Type of elements in the first sequence.</typeparam>
		/// <typeparam name="TSecond">Type of elements in the second sequence.</typeparam>
		/// <typeparam name="TThird">Type of elements in the third sequence.</typeparam>
		/// <typeparam name="TFourth">Type of elements in the fourth sequence.</typeparam>
		/// <typeparam name="TFifth">Type of elements in the fifth sequence.</typeparam>
		/// <typeparam name="TSixth">Type of elements in the sixth sequence.</typeparam>
		/// <typeparam name="TSeventh">Type of elements in the seventh sequence.</typeparam>
		/// <typeparam name="TResult">Type of elements in result sequence</typeparam>
		/// <param name="first">First sequence</param>
		/// <param name="second">Second sequence</param>
		/// <param name="third">Third sequence</param>
		/// <param name="fourth">Fourth sequence</param>
		/// <param name="fifth">Fifth sequence</param>
		/// <param name="sixth">Sixth sequence</param>
		/// <param name="seventh">Seventh sequence</param>
		/// <param name="resultSelector">Function to apply to each group of elements</param>
		public static IEnumerable<TResult> Zip<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TResult>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second, IEnumerable<TThird> third, IEnumerable<TFourth> fourth, IEnumerable<TFifth> fifth, IEnumerable<TSixth> sixth, IEnumerable<TSeventh> seventh,  Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, int, TResult> resultSelector)
        {
			return ZipShortest(first, second, third, fourth, fifth, sixth, seventh, resultSelector);
        }
#if V20 || V30 || V35
		/// <summary>Returns a projection of tuples, where each tuple contains the N-th element from each of the argument sequences.</summary>
        /// <remarks>
        /// If the eight input sequences are of different lengths, the result sequence is terminated as soon as the shortest input sequence is exhausted.
        /// This operator uses deferred execution and streams its results.
        /// </remarks>
        /// <example>
        /// <code>
        /// int[] numbers = { 1, 2, 3 };
        /// string[] letters = { "A", "B", "C", "D" };
        /// var zipped = numbers.ZipShortest(letters, (n, l) => n + l);
        /// </code>
        /// The <c>zipped</c> variable, when iterated over, will yield "1A", "2B", "3C", in turn.
        /// </example>
		/// <typeparam name="TFirst">Type of elements in the first sequence.</typeparam>
		/// <typeparam name="TSecond">Type of elements in the second sequence.</typeparam>
		/// <typeparam name="TThird">Type of elements in the third sequence.</typeparam>
		/// <typeparam name="TFourth">Type of elements in the fourth sequence.</typeparam>
		/// <typeparam name="TFifth">Type of elements in the fifth sequence.</typeparam>
		/// <typeparam name="TSixth">Type of elements in the sixth sequence.</typeparam>
		/// <typeparam name="TSeventh">Type of elements in the seventh sequence.</typeparam>
		/// <typeparam name="TEighth">Type of elements in the eighth sequence.</typeparam>
		/// <typeparam name="TResult">Type of elements in result sequence</typeparam>
		/// <param name="first">First sequence</param>
		/// <param name="second">Second sequence</param>
		/// <param name="third">Third sequence</param>
		/// <param name="fourth">Fourth sequence</param>
		/// <param name="fifth">Fifth sequence</param>
		/// <param name="sixth">Sixth sequence</param>
		/// <param name="seventh">Seventh sequence</param>
		/// <param name="eighth">Eighth sequence</param>
		/// <param name="resultSelector">Function to apply to each group of elements</param>
		public static IEnumerable<TResult> Zip<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TResult>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second, IEnumerable<TThird> third, IEnumerable<TFourth> fourth, IEnumerable<TFifth> fifth, IEnumerable<TSixth> sixth, IEnumerable<TSeventh> seventh, IEnumerable<TEighth> eighth,  Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TResult> resultSelector)
        {
			return ZipShortest(first, second, third, fourth, fifth, sixth, seventh, eighth, resultSelector);
        }
#endif
		/// <summary>Returns a projection of tuples, where each tuple contains the N-th element from each of the argument sequences.</summary>
        /// <remarks>
        /// If the eight input sequences are of different lengths, the result sequence is terminated as soon as the shortest input sequence is exhausted.
		/// A global index over all sequences is passed into the result selector function.
        /// This operator uses deferred execution and streams its results.
        /// </remarks>
        /// <example>
        /// <code>
        /// int[] numbers = { 1, 2, 3 };
        /// string[] letters = { "A", "B", "C", "D" };
        /// var zipped = numbers.ZipShortest(letters, (n, l) => n + l);
        /// </code>
        /// The <c>zipped</c> variable, when iterated over, will yield "1A", "2B", "3C", in turn.
        /// </example>
		/// <typeparam name="TFirst">Type of elements in the first sequence.</typeparam>
		/// <typeparam name="TSecond">Type of elements in the second sequence.</typeparam>
		/// <typeparam name="TThird">Type of elements in the third sequence.</typeparam>
		/// <typeparam name="TFourth">Type of elements in the fourth sequence.</typeparam>
		/// <typeparam name="TFifth">Type of elements in the fifth sequence.</typeparam>
		/// <typeparam name="TSixth">Type of elements in the sixth sequence.</typeparam>
		/// <typeparam name="TSeventh">Type of elements in the seventh sequence.</typeparam>
		/// <typeparam name="TEighth">Type of elements in the eighth sequence.</typeparam>
		/// <typeparam name="TResult">Type of elements in result sequence</typeparam>
		/// <param name="first">First sequence</param>
		/// <param name="second">Second sequence</param>
		/// <param name="third">Third sequence</param>
		/// <param name="fourth">Fourth sequence</param>
		/// <param name="fifth">Fifth sequence</param>
		/// <param name="sixth">Sixth sequence</param>
		/// <param name="seventh">Seventh sequence</param>
		/// <param name="eighth">Eighth sequence</param>
		/// <param name="resultSelector">Function to apply to each group of elements</param>
		public static IEnumerable<TResult> Zip<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TResult>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second, IEnumerable<TThird> third, IEnumerable<TFourth> fourth, IEnumerable<TFifth> fifth, IEnumerable<TSixth> sixth, IEnumerable<TSeventh> seventh, IEnumerable<TEighth> eighth,  Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, int, TResult> resultSelector)
        {
			return ZipShortest(first, second, third, fourth, fifth, sixth, seventh, eighth, resultSelector);
        }
#if V20 || V30 || V35
		/// <summary>Returns a projection of tuples, where each tuple contains the N-th element from each of the argument sequences.</summary>
        /// <remarks>
        /// If the nine input sequences are of different lengths, the result sequence is terminated as soon as the shortest input sequence is exhausted.
        /// This operator uses deferred execution and streams its results.
        /// </remarks>
        /// <example>
        /// <code>
        /// int[] numbers = { 1, 2, 3 };
        /// string[] letters = { "A", "B", "C", "D" };
        /// var zipped = numbers.ZipShortest(letters, (n, l) => n + l);
        /// </code>
        /// The <c>zipped</c> variable, when iterated over, will yield "1A", "2B", "3C", in turn.
        /// </example>
		/// <typeparam name="TFirst">Type of elements in the first sequence.</typeparam>
		/// <typeparam name="TSecond">Type of elements in the second sequence.</typeparam>
		/// <typeparam name="TThird">Type of elements in the third sequence.</typeparam>
		/// <typeparam name="TFourth">Type of elements in the fourth sequence.</typeparam>
		/// <typeparam name="TFifth">Type of elements in the fifth sequence.</typeparam>
		/// <typeparam name="TSixth">Type of elements in the sixth sequence.</typeparam>
		/// <typeparam name="TSeventh">Type of elements in the seventh sequence.</typeparam>
		/// <typeparam name="TEighth">Type of elements in the eighth sequence.</typeparam>
		/// <typeparam name="TNinth">Type of elements in the ninth sequence.</typeparam>
		/// <typeparam name="TResult">Type of elements in result sequence</typeparam>
		/// <param name="first">First sequence</param>
		/// <param name="second">Second sequence</param>
		/// <param name="third">Third sequence</param>
		/// <param name="fourth">Fourth sequence</param>
		/// <param name="fifth">Fifth sequence</param>
		/// <param name="sixth">Sixth sequence</param>
		/// <param name="seventh">Seventh sequence</param>
		/// <param name="eighth">Eighth sequence</param>
		/// <param name="ninth">Ninth sequence</param>
		/// <param name="resultSelector">Function to apply to each group of elements</param>
		public static IEnumerable<TResult> Zip<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth, TResult>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second, IEnumerable<TThird> third, IEnumerable<TFourth> fourth, IEnumerable<TFifth> fifth, IEnumerable<TSixth> sixth, IEnumerable<TSeventh> seventh, IEnumerable<TEighth> eighth, IEnumerable<TNinth> ninth,  Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth, TResult> resultSelector)
        {
			return ZipShortest(first, second, third, fourth, fifth, sixth, seventh, eighth, ninth, resultSelector);
        }
#endif
		/// <summary>Returns a projection of tuples, where each tuple contains the N-th element from each of the argument sequences.</summary>
        /// <remarks>
        /// If the nine input sequences are of different lengths, the result sequence is terminated as soon as the shortest input sequence is exhausted.
		/// A global index over all sequences is passed into the result selector function.
        /// This operator uses deferred execution and streams its results.
        /// </remarks>
        /// <example>
        /// <code>
        /// int[] numbers = { 1, 2, 3 };
        /// string[] letters = { "A", "B", "C", "D" };
        /// var zipped = numbers.ZipShortest(letters, (n, l) => n + l);
        /// </code>
        /// The <c>zipped</c> variable, when iterated over, will yield "1A", "2B", "3C", in turn.
        /// </example>
		/// <typeparam name="TFirst">Type of elements in the first sequence.</typeparam>
		/// <typeparam name="TSecond">Type of elements in the second sequence.</typeparam>
		/// <typeparam name="TThird">Type of elements in the third sequence.</typeparam>
		/// <typeparam name="TFourth">Type of elements in the fourth sequence.</typeparam>
		/// <typeparam name="TFifth">Type of elements in the fifth sequence.</typeparam>
		/// <typeparam name="TSixth">Type of elements in the sixth sequence.</typeparam>
		/// <typeparam name="TSeventh">Type of elements in the seventh sequence.</typeparam>
		/// <typeparam name="TEighth">Type of elements in the eighth sequence.</typeparam>
		/// <typeparam name="TNinth">Type of elements in the ninth sequence.</typeparam>
		/// <typeparam name="TResult">Type of elements in result sequence</typeparam>
		/// <param name="first">First sequence</param>
		/// <param name="second">Second sequence</param>
		/// <param name="third">Third sequence</param>
		/// <param name="fourth">Fourth sequence</param>
		/// <param name="fifth">Fifth sequence</param>
		/// <param name="sixth">Sixth sequence</param>
		/// <param name="seventh">Seventh sequence</param>
		/// <param name="eighth">Eighth sequence</param>
		/// <param name="ninth">Ninth sequence</param>
		/// <param name="resultSelector">Function to apply to each group of elements</param>
		public static IEnumerable<TResult> Zip<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth, TResult>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second, IEnumerable<TThird> third, IEnumerable<TFourth> fourth, IEnumerable<TFifth> fifth, IEnumerable<TSixth> sixth, IEnumerable<TSeventh> seventh, IEnumerable<TEighth> eighth, IEnumerable<TNinth> ninth,  Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth, int, TResult> resultSelector)
        {
			return ZipShortest(first, second, third, fourth, fifth, sixth, seventh, eighth, ninth, resultSelector);
        }
#if V20 || V30 || V35
		/// <summary>Returns a projection of tuples, where each tuple contains the N-th element from each of the argument sequences.</summary>
        /// <remarks>
        /// If the ten input sequences are of different lengths, the result sequence is terminated as soon as the shortest input sequence is exhausted.
        /// This operator uses deferred execution and streams its results.
        /// </remarks>
        /// <example>
        /// <code>
        /// int[] numbers = { 1, 2, 3 };
        /// string[] letters = { "A", "B", "C", "D" };
        /// var zipped = numbers.ZipShortest(letters, (n, l) => n + l);
        /// </code>
        /// The <c>zipped</c> variable, when iterated over, will yield "1A", "2B", "3C", in turn.
        /// </example>
		/// <typeparam name="TFirst">Type of elements in the first sequence.</typeparam>
		/// <typeparam name="TSecond">Type of elements in the second sequence.</typeparam>
		/// <typeparam name="TThird">Type of elements in the third sequence.</typeparam>
		/// <typeparam name="TFourth">Type of elements in the fourth sequence.</typeparam>
		/// <typeparam name="TFifth">Type of elements in the fifth sequence.</typeparam>
		/// <typeparam name="TSixth">Type of elements in the sixth sequence.</typeparam>
		/// <typeparam name="TSeventh">Type of elements in the seventh sequence.</typeparam>
		/// <typeparam name="TEighth">Type of elements in the eighth sequence.</typeparam>
		/// <typeparam name="TNinth">Type of elements in the ninth sequence.</typeparam>
		/// <typeparam name="TTenth">Type of elements in the tenth sequence.</typeparam>
		/// <typeparam name="TResult">Type of elements in result sequence</typeparam>
		/// <param name="first">First sequence</param>
		/// <param name="second">Second sequence</param>
		/// <param name="third">Third sequence</param>
		/// <param name="fourth">Fourth sequence</param>
		/// <param name="fifth">Fifth sequence</param>
		/// <param name="sixth">Sixth sequence</param>
		/// <param name="seventh">Seventh sequence</param>
		/// <param name="eighth">Eighth sequence</param>
		/// <param name="ninth">Ninth sequence</param>
		/// <param name="tenth">Tenth sequence</param>
		/// <param name="resultSelector">Function to apply to each group of elements</param>
		public static IEnumerable<TResult> Zip<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth, TTenth, TResult>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second, IEnumerable<TThird> third, IEnumerable<TFourth> fourth, IEnumerable<TFifth> fifth, IEnumerable<TSixth> sixth, IEnumerable<TSeventh> seventh, IEnumerable<TEighth> eighth, IEnumerable<TNinth> ninth, IEnumerable<TTenth> tenth,  Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth, TTenth, TResult> resultSelector)
        {
			return ZipShortest(first, second, third, fourth, fifth, sixth, seventh, eighth, ninth, tenth, resultSelector);
        }
#endif
		/// <summary>Returns a projection of tuples, where each tuple contains the N-th element from each of the argument sequences.</summary>
        /// <remarks>
        /// If the ten input sequences are of different lengths, the result sequence is terminated as soon as the shortest input sequence is exhausted.
		/// A global index over all sequences is passed into the result selector function.
        /// This operator uses deferred execution and streams its results.
        /// </remarks>
        /// <example>
        /// <code>
        /// int[] numbers = { 1, 2, 3 };
        /// string[] letters = { "A", "B", "C", "D" };
        /// var zipped = numbers.ZipShortest(letters, (n, l) => n + l);
        /// </code>
        /// The <c>zipped</c> variable, when iterated over, will yield "1A", "2B", "3C", in turn.
        /// </example>
		/// <typeparam name="TFirst">Type of elements in the first sequence.</typeparam>
		/// <typeparam name="TSecond">Type of elements in the second sequence.</typeparam>
		/// <typeparam name="TThird">Type of elements in the third sequence.</typeparam>
		/// <typeparam name="TFourth">Type of elements in the fourth sequence.</typeparam>
		/// <typeparam name="TFifth">Type of elements in the fifth sequence.</typeparam>
		/// <typeparam name="TSixth">Type of elements in the sixth sequence.</typeparam>
		/// <typeparam name="TSeventh">Type of elements in the seventh sequence.</typeparam>
		/// <typeparam name="TEighth">Type of elements in the eighth sequence.</typeparam>
		/// <typeparam name="TNinth">Type of elements in the ninth sequence.</typeparam>
		/// <typeparam name="TTenth">Type of elements in the tenth sequence.</typeparam>
		/// <typeparam name="TResult">Type of elements in result sequence</typeparam>
		/// <param name="first">First sequence</param>
		/// <param name="second">Second sequence</param>
		/// <param name="third">Third sequence</param>
		/// <param name="fourth">Fourth sequence</param>
		/// <param name="fifth">Fifth sequence</param>
		/// <param name="sixth">Sixth sequence</param>
		/// <param name="seventh">Seventh sequence</param>
		/// <param name="eighth">Eighth sequence</param>
		/// <param name="ninth">Ninth sequence</param>
		/// <param name="tenth">Tenth sequence</param>
		/// <param name="resultSelector">Function to apply to each group of elements</param>
		public static IEnumerable<TResult> Zip<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth, TTenth, TResult>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second, IEnumerable<TThird> third, IEnumerable<TFourth> fourth, IEnumerable<TFifth> fifth, IEnumerable<TSixth> sixth, IEnumerable<TSeventh> seventh, IEnumerable<TEighth> eighth, IEnumerable<TNinth> ninth, IEnumerable<TTenth> tenth,  Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth, TTenth, int, TResult> resultSelector)
        {
			return ZipShortest(first, second, third, fourth, fifth, sixth, seventh, eighth, ninth, tenth, resultSelector);
        }
	}
}

