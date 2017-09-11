 

using System;

namespace CommonExtensions.NumericExtensions
{
    public static partial class NumericEx
    {
		public static SByte RotateLeft(this SByte value, int count)
		{
			return (SByte)((value << count) | (value >> (8 - count)));
		}

		public static SByte RotateRight(this SByte value, int count)
		{
			return (SByte)((value >> count) | (value << (8 - count)));
		}

		public static Int16 RotateLeft(this Int16 value, int count)
		{
			return (Int16)((value << count) | (value >> (16 - count)));
		}

		public static Int16 RotateRight(this Int16 value, int count)
		{
			return (Int16)((value >> count) | (value << (16 - count)));
		}

		public static Int32 RotateLeft(this Int32 value, int count)
		{
			return (Int32)((value << count) | (value >> (32 - count)));
		}

		public static Int32 RotateRight(this Int32 value, int count)
		{
			return (Int32)((value >> count) | (value << (32 - count)));
		}

		public static Int64 RotateLeft(this Int64 value, int count)
		{
			return (Int64)((value << count) | (value >> (64 - count)));
		}

		public static Int64 RotateRight(this Int64 value, int count)
		{
			return (Int64)((value >> count) | (value << (64 - count)));
		}

		public static Byte RotateLeft(this Byte value, int count)
		{
			return (Byte)((value << count) | (value >> (8 - count)));
		}

		public static Byte RotateRight(this Byte value, int count)
		{
			return (Byte)((value >> count) | (value << (8 - count)));
		}

		public static UInt16 RotateLeft(this UInt16 value, int count)
		{
			return (UInt16)((value << count) | (value >> (16 - count)));
		}

		public static UInt16 RotateRight(this UInt16 value, int count)
		{
			return (UInt16)((value >> count) | (value << (16 - count)));
		}

		public static UInt32 RotateLeft(this UInt32 value, int count)
		{
			return (UInt32)((value << count) | (value >> (32 - count)));
		}

		public static UInt32 RotateRight(this UInt32 value, int count)
		{
			return (UInt32)((value >> count) | (value << (32 - count)));
		}

		public static UInt64 RotateLeft(this UInt64 value, int count)
		{
			return (UInt64)((value << count) | (value >> (64 - count)));
		}

		public static UInt64 RotateRight(this UInt64 value, int count)
		{
			return (UInt64)((value >> count) | (value << (64 - count)));
		}

		public static SByte Scramble(this SByte value)
        {
            unchecked 
			{
				ulong n = (ulong)value;
				n = (n ^ 524287) ^ (n >> 4);
				n = n + (n << 3);
				n = n ^ (n >> 4);
				n = n * 24036583;
				n = n ^ (n >> 3);
				value = (SByte)n;
			}
            return value;
        }

		public static Int16 Scramble(this Int16 value)
        {
            unchecked 
			{
				ulong n = (ulong)value;
				n = (n ^ 524287) ^ (n >> 8);
				n = n + (n << 3);
				n = n ^ (n >> 4);
				n = n * 24036583;
				n = n ^ (n >> 7);
				value = (Int16)n;
			}
            return value;
        }

		public static Int32 Scramble(this Int32 value)
        {
            unchecked 
			{
				ulong n = (ulong)value;
				n = (n ^ 524287) ^ (n >> 16);
				n = n + (n << 3);
				n = n ^ (n >> 4);
				n = n * 24036583;
				n = n ^ (n >> 15);
				value = (Int32)n;
			}
            return value;
        }

		public static Int64 Scramble(this Int64 value)
        {
            unchecked 
			{
				ulong n = (ulong)value;
				n = (n ^ 524287) ^ (n >> 32);
				n = n + (n << 3);
				n = n ^ (n >> 4);
				n = n * 24036583;
				n = n ^ (n >> 31);
				value = (Int64)n;
			}
            return value;
        }

		public static Byte Scramble(this Byte value)
        {
            unchecked 
			{
				ulong n = (ulong)value;
				n = (n ^ 524287) ^ (n >> 4);
				n = n + (n << 3);
				n = n ^ (n >> 4);
				n = n * 24036583;
				n = n ^ (n >> 3);
				value = (Byte)n;
			}
            return value;
        }

		public static UInt16 Scramble(this UInt16 value)
        {
            unchecked 
			{
				ulong n = (ulong)value;
				n = (n ^ 524287) ^ (n >> 8);
				n = n + (n << 3);
				n = n ^ (n >> 4);
				n = n * 24036583;
				n = n ^ (n >> 7);
				value = (UInt16)n;
			}
            return value;
        }

		public static UInt32 Scramble(this UInt32 value)
        {
            unchecked 
			{
				ulong n = (ulong)value;
				n = (n ^ 524287) ^ (n >> 16);
				n = n + (n << 3);
				n = n ^ (n >> 4);
				n = n * 24036583;
				n = n ^ (n >> 15);
				value = (UInt32)n;
			}
            return value;
        }

		public static UInt64 Scramble(this UInt64 value)
        {
            unchecked 
			{
				ulong n = (ulong)value;
				n = (n ^ 524287) ^ (n >> 32);
				n = n + (n << 3);
				n = n ^ (n >> 4);
				n = n * 24036583;
				n = n ^ (n >> 31);
				value = (UInt64)n;
			}
            return value;
        }

        public static string WriteBinary(this SByte value)
        {
 
            return Convert.ToString(value, 2).PadLeft(8, '0');
        }

        public static string WriteBinary(this Int16 value)
        {
 
            return Convert.ToString(value, 2).PadLeft(16, '0');
        }

        public static string WriteBinary(this Int32 value)
        {
 
            return Convert.ToString(value, 2).PadLeft(32, '0');
        }

        public static string WriteBinary(this Int64 value)
        {
 
            return Convert.ToString(value, 2).PadLeft(64, '0');
        }

        public static string WriteBinary(this Byte value)
        {
 
            return Convert.ToString(value, 2).PadLeft(8, '0');
        }

        public static string WriteBinary(this UInt16 value)
        {
 
            return Convert.ToString(value, 2).PadLeft(16, '0');
        }

        public static string WriteBinary(this UInt32 value)
        {
 
            return Convert.ToString(value, 2).PadLeft(32, '0');
        }

        public static string WriteBinary(this UInt64 value)
        {
			return (Convert.ToString((long)(value >> 32), 2)
                   +Convert.ToString((long)(value & 0x00000000FFFFFFFF), 2))
                   .PadLeft(64, '0');
        }

        public static int BitCount(this SByte value)
        {
            int count = 0;
            while (value != 0)
            {
                count++;
                value &= (SByte)(value - 1);
            }
            return count;
        }

        public static int BitCount(this Int16 value)
        {
            int count = 0;
            while (value != 0)
            {
                count++;
                value &= (Int16)(value - 1);
            }
            return count;
        }

        public static int BitCount(this Int32 value)
        {
            int count = 0;
            while (value != 0)
            {
                count++;
                value &= (Int32)(value - 1);
            }
            return count;
        }

        public static int BitCount(this Int64 value)
        {
            int count = 0;
            while (value != 0)
            {
                count++;
                value &= (Int64)(value - 1);
            }
            return count;
        }

        public static int BitCount(this Byte value)
        {
            int count = 0;
            while (value != 0)
            {
                count++;
                value &= (Byte)(value - 1);
            }
            return count;
        }

        public static int BitCount(this UInt16 value)
        {
            int count = 0;
            while (value != 0)
            {
                count++;
                value &= (UInt16)(value - 1);
            }
            return count;
        }

        public static int BitCount(this UInt32 value)
        {
            int count = 0;
            while (value != 0)
            {
                count++;
                value &= (UInt32)(value - 1);
            }
            return count;
        }

        public static int BitCount(this UInt64 value)
        {
            int count = 0;
            while (value != 0)
            {
                count++;
                value &= (UInt64)(value - 1);
            }
            return count;
        }

    }
}

