 

using System;

namespace CommonExtensions
{
    public static partial class ThrowExt
    {
		/// <summary>
        /// Throws an ArgumentOutOfRangeException if the value is less than zero.
        /// </summary>
        /// <param name="argument">The value to test</param>
        /// <param name="name">The name of the value, for the exception</param>
        public static void ThrowIfNegative(this SByte argument, string name)
        {
            if (argument < 0)
            {
                throw new ArgumentOutOfRangeException(name);
            }
        }

        /// <summary>
        /// Throws an ArgumentOutOfRangeException if the value is less than or equal to zero.
        /// </summary>
        /// <param name="argument">The value to test</param>
        /// <param name="name">The name of the value, for the exception</param>
        public static void ThrowIfNonPositive(this SByte argument, string name)
        {
            if (argument <= 0)
            {
                throw new ArgumentOutOfRangeException(name);
            }
        }

		/// <summary>
        /// Throws an ArgumentOutOfRangeException if the value is less than zero.
        /// </summary>
        /// <param name="argument">The value to test</param>
        /// <param name="name">The name of the value, for the exception</param>
        public static void ThrowIfNegative(this Int16 argument, string name)
        {
            if (argument < 0)
            {
                throw new ArgumentOutOfRangeException(name);
            }
        }

        /// <summary>
        /// Throws an ArgumentOutOfRangeException if the value is less than or equal to zero.
        /// </summary>
        /// <param name="argument">The value to test</param>
        /// <param name="name">The name of the value, for the exception</param>
        public static void ThrowIfNonPositive(this Int16 argument, string name)
        {
            if (argument <= 0)
            {
                throw new ArgumentOutOfRangeException(name);
            }
        }

		/// <summary>
        /// Throws an ArgumentOutOfRangeException if the value is less than zero.
        /// </summary>
        /// <param name="argument">The value to test</param>
        /// <param name="name">The name of the value, for the exception</param>
        public static void ThrowIfNegative(this Int32 argument, string name)
        {
            if (argument < 0)
            {
                throw new ArgumentOutOfRangeException(name);
            }
        }

        /// <summary>
        /// Throws an ArgumentOutOfRangeException if the value is less than or equal to zero.
        /// </summary>
        /// <param name="argument">The value to test</param>
        /// <param name="name">The name of the value, for the exception</param>
        public static void ThrowIfNonPositive(this Int32 argument, string name)
        {
            if (argument <= 0)
            {
                throw new ArgumentOutOfRangeException(name);
            }
        }

		/// <summary>
        /// Throws an ArgumentOutOfRangeException if the value is less than zero.
        /// </summary>
        /// <param name="argument">The value to test</param>
        /// <param name="name">The name of the value, for the exception</param>
        public static void ThrowIfNegative(this Int64 argument, string name)
        {
            if (argument < 0)
            {
                throw new ArgumentOutOfRangeException(name);
            }
        }

        /// <summary>
        /// Throws an ArgumentOutOfRangeException if the value is less than or equal to zero.
        /// </summary>
        /// <param name="argument">The value to test</param>
        /// <param name="name">The name of the value, for the exception</param>
        public static void ThrowIfNonPositive(this Int64 argument, string name)
        {
            if (argument <= 0)
            {
                throw new ArgumentOutOfRangeException(name);
            }
        }

		/// <summary>
        /// Throws an ArgumentOutOfRangeException if the value is less than zero.
        /// </summary>
        /// <param name="argument">The value to test</param>
        /// <param name="name">The name of the value, for the exception</param>
        public static void ThrowIfNegative(this Byte argument, string name)
        {
            if (argument < 0)
            {
                throw new ArgumentOutOfRangeException(name);
            }
        }

        /// <summary>
        /// Throws an ArgumentOutOfRangeException if the value is less than or equal to zero.
        /// </summary>
        /// <param name="argument">The value to test</param>
        /// <param name="name">The name of the value, for the exception</param>
        public static void ThrowIfNonPositive(this Byte argument, string name)
        {
            if (argument <= 0)
            {
                throw new ArgumentOutOfRangeException(name);
            }
        }

		/// <summary>
        /// Throws an ArgumentOutOfRangeException if the value is less than zero.
        /// </summary>
        /// <param name="argument">The value to test</param>
        /// <param name="name">The name of the value, for the exception</param>
        public static void ThrowIfNegative(this UInt16 argument, string name)
        {
            if (argument < 0)
            {
                throw new ArgumentOutOfRangeException(name);
            }
        }

        /// <summary>
        /// Throws an ArgumentOutOfRangeException if the value is less than or equal to zero.
        /// </summary>
        /// <param name="argument">The value to test</param>
        /// <param name="name">The name of the value, for the exception</param>
        public static void ThrowIfNonPositive(this UInt16 argument, string name)
        {
            if (argument <= 0)
            {
                throw new ArgumentOutOfRangeException(name);
            }
        }

		/// <summary>
        /// Throws an ArgumentOutOfRangeException if the value is less than zero.
        /// </summary>
        /// <param name="argument">The value to test</param>
        /// <param name="name">The name of the value, for the exception</param>
        public static void ThrowIfNegative(this UInt32 argument, string name)
        {
            if (argument < 0)
            {
                throw new ArgumentOutOfRangeException(name);
            }
        }

        /// <summary>
        /// Throws an ArgumentOutOfRangeException if the value is less than or equal to zero.
        /// </summary>
        /// <param name="argument">The value to test</param>
        /// <param name="name">The name of the value, for the exception</param>
        public static void ThrowIfNonPositive(this UInt32 argument, string name)
        {
            if (argument <= 0)
            {
                throw new ArgumentOutOfRangeException(name);
            }
        }

		/// <summary>
        /// Throws an ArgumentOutOfRangeException if the value is less than zero.
        /// </summary>
        /// <param name="argument">The value to test</param>
        /// <param name="name">The name of the value, for the exception</param>
        public static void ThrowIfNegative(this UInt64 argument, string name)
        {
            if (argument < 0)
            {
                throw new ArgumentOutOfRangeException(name);
            }
        }

        /// <summary>
        /// Throws an ArgumentOutOfRangeException if the value is less than or equal to zero.
        /// </summary>
        /// <param name="argument">The value to test</param>
        /// <param name="name">The name of the value, for the exception</param>
        public static void ThrowIfNonPositive(this UInt64 argument, string name)
        {
            if (argument <= 0)
            {
                throw new ArgumentOutOfRangeException(name);
            }
        }

		/// <summary>
        /// Throws an ArgumentOutOfRangeException if the value is less than zero.
        /// </summary>
        /// <param name="argument">The value to test</param>
        /// <param name="name">The name of the value, for the exception</param>
        public static void ThrowIfNegative(this Single argument, string name)
        {
            if (argument < 0)
            {
                throw new ArgumentOutOfRangeException(name);
            }
        }

        /// <summary>
        /// Throws an ArgumentOutOfRangeException if the value is less than or equal to zero.
        /// </summary>
        /// <param name="argument">The value to test</param>
        /// <param name="name">The name of the value, for the exception</param>
        public static void ThrowIfNonPositive(this Single argument, string name)
        {
            if (argument <= 0)
            {
                throw new ArgumentOutOfRangeException(name);
            }
        }

		/// <summary>
        /// Throws an ArgumentOutOfRangeException if the value is less than zero.
        /// </summary>
        /// <param name="argument">The value to test</param>
        /// <param name="name">The name of the value, for the exception</param>
        public static void ThrowIfNegative(this Double argument, string name)
        {
            if (argument < 0)
            {
                throw new ArgumentOutOfRangeException(name);
            }
        }

        /// <summary>
        /// Throws an ArgumentOutOfRangeException if the value is less than or equal to zero.
        /// </summary>
        /// <param name="argument">The value to test</param>
        /// <param name="name">The name of the value, for the exception</param>
        public static void ThrowIfNonPositive(this Double argument, string name)
        {
            if (argument <= 0)
            {
                throw new ArgumentOutOfRangeException(name);
            }
        }

		/// <summary>
        /// Throws an ArgumentOutOfRangeException if the value is less than zero.
        /// </summary>
        /// <param name="argument">The value to test</param>
        /// <param name="name">The name of the value, for the exception</param>
        public static void ThrowIfNegative(this Decimal argument, string name)
        {
            if (argument < 0)
            {
                throw new ArgumentOutOfRangeException(name);
            }
        }

        /// <summary>
        /// Throws an ArgumentOutOfRangeException if the value is less than or equal to zero.
        /// </summary>
        /// <param name="argument">The value to test</param>
        /// <param name="name">The name of the value, for the exception</param>
        public static void ThrowIfNonPositive(this Decimal argument, string name)
        {
            if (argument <= 0)
            {
                throw new ArgumentOutOfRangeException(name);
            }
        }

    }
}