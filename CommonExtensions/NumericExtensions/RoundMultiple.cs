 

using System;

namespace CommonExtensions.NumericExtensions
{
    public static partial class NumericEx
    {
		/// <summary>Rounds up to the next higher number that is a multiple of the value given.</summary>
		/// <param name="value">The initial value to be rounded.</param>
		/// <param name="multiple">The multiple to use as a basis for rounding.</param>
        /// <returns>A value that is a multiple of <see cref="multiple"/> that is higher than <see cref="value"/>.</returns>
		public static SByte RoundHigherMultiple(this SByte value, SByte multiple)
        {
            return (SByte)(Math.Ceiling((double)value / multiple) * multiple);
        }

		/// <summary>Rounds down to the next lower number that is a multiple of the value given.</summary>
		/// <param name="value">The initial value to be rounded.</param>
		/// <param name="multiple">The multiple to use as a basis for rounding.</param>
        /// <returns>A value that is a multiple of <see cref="multiple"/> that is lower than <see cref="value"/>.</returns>
        public static SByte RoundLowerMultiple(this SByte value, SByte multiple)
        {
            return (SByte)(Math.Floor((double)value / multiple) * multiple);
        }

		/// <summary>Rounds to the nearest number that is a multiple of the value given.</summary>
		/// <param name="value">The initial value to be rounded.</param>
		/// <param name="multiple">The multiple to use as a basis for rounding.</param>
        /// <returns>A value that is a multiple of <see cref="multiple"/> that is nearest to <see cref="value"/>.</returns>
        public static SByte RoundNearestMultiple(this SByte value, SByte multiple)
        {
            return (SByte)(Math.Round((double)value / multiple, MidpointRounding.AwayFromZero) * multiple);
        }

		/// <summary>Rounds up to the next higher number that is a multiple of the value given.</summary>
		/// <param name="value">The initial value to be rounded.</param>
		/// <param name="multiple">The multiple to use as a basis for rounding.</param>
        /// <returns>A value that is a multiple of <see cref="multiple"/> that is higher than <see cref="value"/>.</returns>
		public static Int16 RoundHigherMultiple(this Int16 value, Int16 multiple)
        {
            return (Int16)(Math.Ceiling((double)value / multiple) * multiple);
        }

		/// <summary>Rounds down to the next lower number that is a multiple of the value given.</summary>
		/// <param name="value">The initial value to be rounded.</param>
		/// <param name="multiple">The multiple to use as a basis for rounding.</param>
        /// <returns>A value that is a multiple of <see cref="multiple"/> that is lower than <see cref="value"/>.</returns>
        public static Int16 RoundLowerMultiple(this Int16 value, Int16 multiple)
        {
            return (Int16)(Math.Floor((double)value / multiple) * multiple);
        }

		/// <summary>Rounds to the nearest number that is a multiple of the value given.</summary>
		/// <param name="value">The initial value to be rounded.</param>
		/// <param name="multiple">The multiple to use as a basis for rounding.</param>
        /// <returns>A value that is a multiple of <see cref="multiple"/> that is nearest to <see cref="value"/>.</returns>
        public static Int16 RoundNearestMultiple(this Int16 value, Int16 multiple)
        {
            return (Int16)(Math.Round((double)value / multiple, MidpointRounding.AwayFromZero) * multiple);
        }

		/// <summary>Rounds up to the next higher number that is a multiple of the value given.</summary>
		/// <param name="value">The initial value to be rounded.</param>
		/// <param name="multiple">The multiple to use as a basis for rounding.</param>
        /// <returns>A value that is a multiple of <see cref="multiple"/> that is higher than <see cref="value"/>.</returns>
		public static Int32 RoundHigherMultiple(this Int32 value, Int32 multiple)
        {
            return (Int32)(Math.Ceiling((double)value / multiple) * multiple);
        }

		/// <summary>Rounds down to the next lower number that is a multiple of the value given.</summary>
		/// <param name="value">The initial value to be rounded.</param>
		/// <param name="multiple">The multiple to use as a basis for rounding.</param>
        /// <returns>A value that is a multiple of <see cref="multiple"/> that is lower than <see cref="value"/>.</returns>
        public static Int32 RoundLowerMultiple(this Int32 value, Int32 multiple)
        {
            return (Int32)(Math.Floor((double)value / multiple) * multiple);
        }

		/// <summary>Rounds to the nearest number that is a multiple of the value given.</summary>
		/// <param name="value">The initial value to be rounded.</param>
		/// <param name="multiple">The multiple to use as a basis for rounding.</param>
        /// <returns>A value that is a multiple of <see cref="multiple"/> that is nearest to <see cref="value"/>.</returns>
        public static Int32 RoundNearestMultiple(this Int32 value, Int32 multiple)
        {
            return (Int32)(Math.Round((double)value / multiple, MidpointRounding.AwayFromZero) * multiple);
        }

		/// <summary>Rounds up to the next higher number that is a multiple of the value given.</summary>
		/// <param name="value">The initial value to be rounded.</param>
		/// <param name="multiple">The multiple to use as a basis for rounding.</param>
        /// <returns>A value that is a multiple of <see cref="multiple"/> that is higher than <see cref="value"/>.</returns>
		public static Int64 RoundHigherMultiple(this Int64 value, Int64 multiple)
        {
            return (Int64)(Math.Ceiling((double)value / multiple) * multiple);
        }

		/// <summary>Rounds down to the next lower number that is a multiple of the value given.</summary>
		/// <param name="value">The initial value to be rounded.</param>
		/// <param name="multiple">The multiple to use as a basis for rounding.</param>
        /// <returns>A value that is a multiple of <see cref="multiple"/> that is lower than <see cref="value"/>.</returns>
        public static Int64 RoundLowerMultiple(this Int64 value, Int64 multiple)
        {
            return (Int64)(Math.Floor((double)value / multiple) * multiple);
        }

		/// <summary>Rounds to the nearest number that is a multiple of the value given.</summary>
		/// <param name="value">The initial value to be rounded.</param>
		/// <param name="multiple">The multiple to use as a basis for rounding.</param>
        /// <returns>A value that is a multiple of <see cref="multiple"/> that is nearest to <see cref="value"/>.</returns>
        public static Int64 RoundNearestMultiple(this Int64 value, Int64 multiple)
        {
            return (Int64)(Math.Round((double)value / multiple, MidpointRounding.AwayFromZero) * multiple);
        }

		/// <summary>Rounds up to the next higher number that is a multiple of the value given.</summary>
		/// <param name="value">The initial value to be rounded.</param>
		/// <param name="multiple">The multiple to use as a basis for rounding.</param>
        /// <returns>A value that is a multiple of <see cref="multiple"/> that is higher than <see cref="value"/>.</returns>
		public static Byte RoundHigherMultiple(this Byte value, Byte multiple)
        {
            return (Byte)(Math.Ceiling((double)value / multiple) * multiple);
        }

		/// <summary>Rounds down to the next lower number that is a multiple of the value given.</summary>
		/// <param name="value">The initial value to be rounded.</param>
		/// <param name="multiple">The multiple to use as a basis for rounding.</param>
        /// <returns>A value that is a multiple of <see cref="multiple"/> that is lower than <see cref="value"/>.</returns>
        public static Byte RoundLowerMultiple(this Byte value, Byte multiple)
        {
            return (Byte)(Math.Floor((double)value / multiple) * multiple);
        }

		/// <summary>Rounds to the nearest number that is a multiple of the value given.</summary>
		/// <param name="value">The initial value to be rounded.</param>
		/// <param name="multiple">The multiple to use as a basis for rounding.</param>
        /// <returns>A value that is a multiple of <see cref="multiple"/> that is nearest to <see cref="value"/>.</returns>
        public static Byte RoundNearestMultiple(this Byte value, Byte multiple)
        {
            return (Byte)(Math.Round((double)value / multiple, MidpointRounding.AwayFromZero) * multiple);
        }

		/// <summary>Rounds up to the next higher number that is a multiple of the value given.</summary>
		/// <param name="value">The initial value to be rounded.</param>
		/// <param name="multiple">The multiple to use as a basis for rounding.</param>
        /// <returns>A value that is a multiple of <see cref="multiple"/> that is higher than <see cref="value"/>.</returns>
		public static UInt16 RoundHigherMultiple(this UInt16 value, UInt16 multiple)
        {
            return (UInt16)(Math.Ceiling((double)value / multiple) * multiple);
        }

		/// <summary>Rounds down to the next lower number that is a multiple of the value given.</summary>
		/// <param name="value">The initial value to be rounded.</param>
		/// <param name="multiple">The multiple to use as a basis for rounding.</param>
        /// <returns>A value that is a multiple of <see cref="multiple"/> that is lower than <see cref="value"/>.</returns>
        public static UInt16 RoundLowerMultiple(this UInt16 value, UInt16 multiple)
        {
            return (UInt16)(Math.Floor((double)value / multiple) * multiple);
        }

		/// <summary>Rounds to the nearest number that is a multiple of the value given.</summary>
		/// <param name="value">The initial value to be rounded.</param>
		/// <param name="multiple">The multiple to use as a basis for rounding.</param>
        /// <returns>A value that is a multiple of <see cref="multiple"/> that is nearest to <see cref="value"/>.</returns>
        public static UInt16 RoundNearestMultiple(this UInt16 value, UInt16 multiple)
        {
            return (UInt16)(Math.Round((double)value / multiple, MidpointRounding.AwayFromZero) * multiple);
        }

		/// <summary>Rounds up to the next higher number that is a multiple of the value given.</summary>
		/// <param name="value">The initial value to be rounded.</param>
		/// <param name="multiple">The multiple to use as a basis for rounding.</param>
        /// <returns>A value that is a multiple of <see cref="multiple"/> that is higher than <see cref="value"/>.</returns>
		public static UInt32 RoundHigherMultiple(this UInt32 value, UInt32 multiple)
        {
            return (UInt32)(Math.Ceiling((double)value / multiple) * multiple);
        }

		/// <summary>Rounds down to the next lower number that is a multiple of the value given.</summary>
		/// <param name="value">The initial value to be rounded.</param>
		/// <param name="multiple">The multiple to use as a basis for rounding.</param>
        /// <returns>A value that is a multiple of <see cref="multiple"/> that is lower than <see cref="value"/>.</returns>
        public static UInt32 RoundLowerMultiple(this UInt32 value, UInt32 multiple)
        {
            return (UInt32)(Math.Floor((double)value / multiple) * multiple);
        }

		/// <summary>Rounds to the nearest number that is a multiple of the value given.</summary>
		/// <param name="value">The initial value to be rounded.</param>
		/// <param name="multiple">The multiple to use as a basis for rounding.</param>
        /// <returns>A value that is a multiple of <see cref="multiple"/> that is nearest to <see cref="value"/>.</returns>
        public static UInt32 RoundNearestMultiple(this UInt32 value, UInt32 multiple)
        {
            return (UInt32)(Math.Round((double)value / multiple, MidpointRounding.AwayFromZero) * multiple);
        }

		/// <summary>Rounds up to the next higher number that is a multiple of the value given.</summary>
		/// <param name="value">The initial value to be rounded.</param>
		/// <param name="multiple">The multiple to use as a basis for rounding.</param>
        /// <returns>A value that is a multiple of <see cref="multiple"/> that is higher than <see cref="value"/>.</returns>
		public static UInt64 RoundHigherMultiple(this UInt64 value, UInt64 multiple)
        {
            return (UInt64)(Math.Ceiling((double)value / multiple) * multiple);
        }

		/// <summary>Rounds down to the next lower number that is a multiple of the value given.</summary>
		/// <param name="value">The initial value to be rounded.</param>
		/// <param name="multiple">The multiple to use as a basis for rounding.</param>
        /// <returns>A value that is a multiple of <see cref="multiple"/> that is lower than <see cref="value"/>.</returns>
        public static UInt64 RoundLowerMultiple(this UInt64 value, UInt64 multiple)
        {
            return (UInt64)(Math.Floor((double)value / multiple) * multiple);
        }

		/// <summary>Rounds to the nearest number that is a multiple of the value given.</summary>
		/// <param name="value">The initial value to be rounded.</param>
		/// <param name="multiple">The multiple to use as a basis for rounding.</param>
        /// <returns>A value that is a multiple of <see cref="multiple"/> that is nearest to <see cref="value"/>.</returns>
        public static UInt64 RoundNearestMultiple(this UInt64 value, UInt64 multiple)
        {
            return (UInt64)(Math.Round((double)value / multiple, MidpointRounding.AwayFromZero) * multiple);
        }

		/// <summary>Rounds up to the next higher number that is a multiple of the value given.</summary>
		/// <param name="value">The initial value to be rounded.</param>
		/// <param name="multiple">The multiple to use as a basis for rounding.</param>
        /// <returns>A value that is a multiple of <see cref="multiple"/> that is higher than <see cref="value"/>.</returns>
		public static Single RoundHigherMultiple(this Single value, Single multiple)
        {
            return (Single)(Math.Ceiling((double)value / multiple) * multiple);
        }

		/// <summary>Rounds down to the next lower number that is a multiple of the value given.</summary>
		/// <param name="value">The initial value to be rounded.</param>
		/// <param name="multiple">The multiple to use as a basis for rounding.</param>
        /// <returns>A value that is a multiple of <see cref="multiple"/> that is lower than <see cref="value"/>.</returns>
        public static Single RoundLowerMultiple(this Single value, Single multiple)
        {
            return (Single)(Math.Floor((double)value / multiple) * multiple);
        }

		/// <summary>Rounds to the nearest number that is a multiple of the value given.</summary>
		/// <param name="value">The initial value to be rounded.</param>
		/// <param name="multiple">The multiple to use as a basis for rounding.</param>
        /// <returns>A value that is a multiple of <see cref="multiple"/> that is nearest to <see cref="value"/>.</returns>
        public static Single RoundNearestMultiple(this Single value, Single multiple)
        {
            return (Single)(Math.Round((double)value / multiple, MidpointRounding.AwayFromZero) * multiple);
        }

		/// <summary>Rounds up to the next higher number that is a multiple of the value given.</summary>
		/// <param name="value">The initial value to be rounded.</param>
		/// <param name="multiple">The multiple to use as a basis for rounding.</param>
        /// <returns>A value that is a multiple of <see cref="multiple"/> that is higher than <see cref="value"/>.</returns>
		public static Double RoundHigherMultiple(this Double value, Double multiple)
        {
            return (Double)(Math.Ceiling(value / multiple) * multiple);
        }

		/// <summary>Rounds down to the next lower number that is a multiple of the value given.</summary>
		/// <param name="value">The initial value to be rounded.</param>
		/// <param name="multiple">The multiple to use as a basis for rounding.</param>
        /// <returns>A value that is a multiple of <see cref="multiple"/> that is lower than <see cref="value"/>.</returns>
        public static Double RoundLowerMultiple(this Double value, Double multiple)
        {
            return (Double)(Math.Floor(value / multiple) * multiple);
        }

		/// <summary>Rounds to the nearest number that is a multiple of the value given.</summary>
		/// <param name="value">The initial value to be rounded.</param>
		/// <param name="multiple">The multiple to use as a basis for rounding.</param>
        /// <returns>A value that is a multiple of <see cref="multiple"/> that is nearest to <see cref="value"/>.</returns>
        public static Double RoundNearestMultiple(this Double value, Double multiple)
        {
            return (Double)(Math.Round(value / multiple, MidpointRounding.AwayFromZero) * multiple);
        }

		/// <summary>Rounds up to the next higher number that is a multiple of the value given.</summary>
		/// <param name="value">The initial value to be rounded.</param>
		/// <param name="multiple">The multiple to use as a basis for rounding.</param>
        /// <returns>A value that is a multiple of <see cref="multiple"/> that is higher than <see cref="value"/>.</returns>
		public static Decimal RoundHigherMultiple(this Decimal value, Decimal multiple)
        {
            return (Decimal)(Math.Ceiling(value / multiple) * multiple);
        }

		/// <summary>Rounds down to the next lower number that is a multiple of the value given.</summary>
		/// <param name="value">The initial value to be rounded.</param>
		/// <param name="multiple">The multiple to use as a basis for rounding.</param>
        /// <returns>A value that is a multiple of <see cref="multiple"/> that is lower than <see cref="value"/>.</returns>
        public static Decimal RoundLowerMultiple(this Decimal value, Decimal multiple)
        {
            return (Decimal)(Math.Floor(value / multiple) * multiple);
        }

		/// <summary>Rounds to the nearest number that is a multiple of the value given.</summary>
		/// <param name="value">The initial value to be rounded.</param>
		/// <param name="multiple">The multiple to use as a basis for rounding.</param>
        /// <returns>A value that is a multiple of <see cref="multiple"/> that is nearest to <see cref="value"/>.</returns>
        public static Decimal RoundNearestMultiple(this Decimal value, Decimal multiple)
        {
            return (Decimal)(Math.Round(value / multiple, MidpointRounding.AwayFromZero) * multiple);
        }

    }
}

