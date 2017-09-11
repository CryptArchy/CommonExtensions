using System;
using System.Collections.Generic;

namespace CommonExtensions.EnumerableExtensions
{
    public static partial class EnumerableExt
    {
        /// <summary>
        /// Strategy determining the handling of the case where the inputs are of unequal lengths.
        /// </summary>
        internal enum ImbalancedZipStrategy
        {
            /// <summary>
            /// The result sequence ends when either input sequence is exhausted.
            /// </summary>
            Truncate = 0,
            /// <summary>
            /// The result sequence ends when both sequences are exhausted. The shorter sequence is effectively "padded" at the end with the default value for its element type.
            /// </summary>
            Pad = 1,
            /// <summary>
            /// <see cref="InvalidOperationException" /> is thrown if one sequence is exhausted but not the other.
            /// </summary>
            Fail = 2
        }
    }
}
