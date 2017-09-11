
namespace CommonExtensions.EnumerableExtensions
{
    //NOTE The Enumerable Extensions are mostly copied or adapted from the MoreLinq library at http://code.google.com/p/morelinq/.
    //     Rather than use the library directly, I thought it better to copy in the parts needed or desired, and leave out some of the more specialized parts of morelinq that we won't use.

    /// <summary>
    /// A container class for extension methods to the IEnumerable types.  
    /// </summary>
    /// <remarks>
    /// Individual extensions have their own files, this just holds a "first reference" to it.
    /// </remarks>
    public static partial class EnumerableExt
    {
        /// <summary>
        /// The Identity function, returns whatever you put in it.  Useful as a helper for certain LINQ operations.
        /// </summary>
        /// <typeparam name="T">Any type</typeparam>
        /// <param name="value">Any value</param>
        /// <returns>The value that went into it</returns>
        public static T Identity<T>(T value) { return value; }
    }
}
