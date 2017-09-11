using System;
using System.Collections.Generic;
using System.Linq;

namespace CommonExtensions.EnumerableExtensions
{
    public static partial class EnumerableExt
    {
        /// <summary>
        /// Produces a sequence from an action based on the dynamic generation of N nested loops
        /// who iteration counts are defined by <paramref name="loopCounts"/>.
        /// </summary>
        /// <param name="action">Action delegate for which to produce a nested loop sequence</param>
        /// <param name="loopCounts">A sequence of loop repetition counts</param>
        /// <returns>A sequence of Action representing the expansion of a set of nested loops</returns>
        public static IEnumerable<Action> NestedLoops(this Action action, IEnumerable<int> loopCounts)
        {
            loopCounts.ThrowIfNull("loopCounts");

            using (var iter = loopCounts.GetEnumerator())
            {
                var loopCount = NextLoopCount(iter);
                if (loopCount == null)
                    return Enumerable.Empty<Action>(); // null loop
                var loop = Enumerable.Repeat(action, loopCount.Value);
                while ((loopCount = NextLoopCount(iter)) != null)
                    loop = loop.Repeat(loopCount.Value);
                return loop;
            }
        }

        private static int? NextLoopCount(IEnumerator<int> iter)
        {
            if (!iter.MoveNext())
                return null;
            if (iter.Current < 0)
                throw new ArgumentException("All loop counts must be greater than or equal to zero.", "loopCounts");
            return iter.Current;
        }
    }
}
