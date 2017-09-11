 

using System;

namespace CommonExtensions.NumericExtensions
{
    public static partial class NumericEx
    {
		private static string[] Numbers = new[] { "Zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen", "Twenty" };
		private static string[] NumbersByTen = new[] { "Zero", "Ten", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };
		private static string[] PowersOfTen = new[] { "Hundred", "Thousand", "Million", "Billion", "Trillion", "Quadrillion" };

		public static string ToText(this SByte value)
        {
            if (value == 0) return Numbers[value];
			else if (value < 0) return "Negative " + ToText(Math.Abs(value));

			string text = string.Empty;

			for (int i = PowersOfTen.Length-1; i >= 0; i--)
			{
				double power;
				if (i == 0) power = 100;
				else power = Math.Pow(10, i*3);
		
				if (SByte.MaxValue < power) continue;

				if ((value / (SByte)power) > 0)
				{
					text += ToText(value / (SByte)power) + " " + PowersOfTen[i] + " ";
					value %= (SByte)power;
				}
			}

			if (value > 0) 
			{
				if (text != "") text += "and ";
		
				if (value < 20)
					text += Numbers[value];
				else 
				{
					text += NumbersByTen[value/10];
					if ((value %= 10) > 0) text += "-" + Numbers[value];
				}
			}
			return text.Trim();
        }

		public static string ToOrdinalText(this SByte value)
		{
			string text = ToText(value);
			if(text.EndsWith("One"))
				return text.Substring(0, text.Length-3) + "First";
			else if(text.EndsWith("Two"))
				return text.Substring(0, text.Length-3) + "Second";
			else if(text.EndsWith("Three"))
				return text.Substring(0, text.Length-5) + "Third";
			else if(text.EndsWith("ve"))
				return text.Substring(0, text.Length-2) + "fth";
			else if(text.EndsWith("e"))
				return text.Substring(0, text.Length-1) + "th";
			else if(text.EndsWith("y"))
				return text.Substring(0, text.Length-1) + "ieth";
			else if(text.EndsWith("t"))
				return text + "h";
			else
				return text + "th";
		}
		
		public static string ToOrdinal(this SByte value)
        { 
            switch (value % 100)
            {
                case 11:
                case 12:
                case 13:
                    return value.ToString() + "th";
            }

            switch (value % 10)
            {
                case 1:
                    return value.ToString() + "st";
                case 2:
                    return value.ToString() + "nd";
                case 3:
                    return value.ToString() + "rd";
                default:
                    return value.ToString() + "th";
            }
        }
		public static string ToText(this Int16 value)
        {
            if (value == 0) return Numbers[value];
			else if (value < 0) return "Negative " + ToText(Math.Abs(value));

			string text = string.Empty;

			for (int i = PowersOfTen.Length-1; i >= 0; i--)
			{
				double power;
				if (i == 0) power = 100;
				else power = Math.Pow(10, i*3);
		
				if (Int16.MaxValue < power) continue;

				if ((value / (Int16)power) > 0)
				{
					text += ToText(value / (Int16)power) + " " + PowersOfTen[i] + " ";
					value %= (Int16)power;
				}
			}

			if (value > 0) 
			{
				if (text != "") text += "and ";
		
				if (value < 20)
					text += Numbers[value];
				else 
				{
					text += NumbersByTen[value/10];
					if ((value %= 10) > 0) text += "-" + Numbers[value];
				}
			}
			return text.Trim();
        }

		public static string ToOrdinalText(this Int16 value)
		{
			string text = ToText(value);
			if(text.EndsWith("One"))
				return text.Substring(0, text.Length-3) + "First";
			else if(text.EndsWith("Two"))
				return text.Substring(0, text.Length-3) + "Second";
			else if(text.EndsWith("Three"))
				return text.Substring(0, text.Length-5) + "Third";
			else if(text.EndsWith("ve"))
				return text.Substring(0, text.Length-2) + "fth";
			else if(text.EndsWith("e"))
				return text.Substring(0, text.Length-1) + "th";
			else if(text.EndsWith("y"))
				return text.Substring(0, text.Length-1) + "ieth";
			else if(text.EndsWith("t"))
				return text + "h";
			else
				return text + "th";
		}
		
		public static string ToOrdinal(this Int16 value)
        { 
            switch (value % 100)
            {
                case 11:
                case 12:
                case 13:
                    return value.ToString() + "th";
            }

            switch (value % 10)
            {
                case 1:
                    return value.ToString() + "st";
                case 2:
                    return value.ToString() + "nd";
                case 3:
                    return value.ToString() + "rd";
                default:
                    return value.ToString() + "th";
            }
        }
		public static string ToText(this Int32 value)
        {
            if (value == 0) return Numbers[value];
			else if (value < 0) return "Negative " + ToText(Math.Abs(value));

			string text = string.Empty;

			for (int i = PowersOfTen.Length-1; i >= 0; i--)
			{
				double power;
				if (i == 0) power = 100;
				else power = Math.Pow(10, i*3);
		
				if (Int32.MaxValue < power) continue;

				if ((value / (Int32)power) > 0)
				{
					text += ToText(value / (Int32)power) + " " + PowersOfTen[i] + " ";
					value %= (Int32)power;
				}
			}

			if (value > 0) 
			{
				if (text != "") text += "and ";
		
				if (value < 20)
					text += Numbers[value];
				else 
				{
					text += NumbersByTen[value/10];
					if ((value %= 10) > 0) text += "-" + Numbers[value];
				}
			}
			return text.Trim();
        }

		public static string ToOrdinalText(this Int32 value)
		{
			string text = ToText(value);
			if(text.EndsWith("One"))
				return text.Substring(0, text.Length-3) + "First";
			else if(text.EndsWith("Two"))
				return text.Substring(0, text.Length-3) + "Second";
			else if(text.EndsWith("Three"))
				return text.Substring(0, text.Length-5) + "Third";
			else if(text.EndsWith("ve"))
				return text.Substring(0, text.Length-2) + "fth";
			else if(text.EndsWith("e"))
				return text.Substring(0, text.Length-1) + "th";
			else if(text.EndsWith("y"))
				return text.Substring(0, text.Length-1) + "ieth";
			else if(text.EndsWith("t"))
				return text + "h";
			else
				return text + "th";
		}
		
		public static string ToOrdinal(this Int32 value)
        { 
            switch (value % 100)
            {
                case 11:
                case 12:
                case 13:
                    return value.ToString() + "th";
            }

            switch (value % 10)
            {
                case 1:
                    return value.ToString() + "st";
                case 2:
                    return value.ToString() + "nd";
                case 3:
                    return value.ToString() + "rd";
                default:
                    return value.ToString() + "th";
            }
        }
		public static string ToText(this Int64 value)
        {
            if (value == 0) return Numbers[value];
			else if (value < 0) return "Negative " + ToText(Math.Abs(value));

			string text = string.Empty;

			for (int i = PowersOfTen.Length-1; i >= 0; i--)
			{
				double power;
				if (i == 0) power = 100;
				else power = Math.Pow(10, i*3);
		
				if (Int64.MaxValue < power) continue;

				if ((value / (Int64)power) > 0)
				{
					text += ToText(value / (Int64)power) + " " + PowersOfTen[i] + " ";
					value %= (Int64)power;
				}
			}

			if (value > 0) 
			{
				if (text != "") text += "and ";
		
				if (value < 20)
					text += Numbers[value];
				else 
				{
					text += NumbersByTen[value/10];
					if ((value %= 10) > 0) text += "-" + Numbers[value];
				}
			}
			return text.Trim();
        }

		public static string ToOrdinalText(this Int64 value)
		{
			string text = ToText(value);
			if(text.EndsWith("One"))
				return text.Substring(0, text.Length-3) + "First";
			else if(text.EndsWith("Two"))
				return text.Substring(0, text.Length-3) + "Second";
			else if(text.EndsWith("Three"))
				return text.Substring(0, text.Length-5) + "Third";
			else if(text.EndsWith("ve"))
				return text.Substring(0, text.Length-2) + "fth";
			else if(text.EndsWith("e"))
				return text.Substring(0, text.Length-1) + "th";
			else if(text.EndsWith("y"))
				return text.Substring(0, text.Length-1) + "ieth";
			else if(text.EndsWith("t"))
				return text + "h";
			else
				return text + "th";
		}
		
		public static string ToOrdinal(this Int64 value)
        { 
            switch (value % 100)
            {
                case 11:
                case 12:
                case 13:
                    return value.ToString() + "th";
            }

            switch (value % 10)
            {
                case 1:
                    return value.ToString() + "st";
                case 2:
                    return value.ToString() + "nd";
                case 3:
                    return value.ToString() + "rd";
                default:
                    return value.ToString() + "th";
            }
        }
		public static string ToText(this Byte value)
        {
            if (value == 0) return Numbers[value];

			string text = string.Empty;

			for (int i = PowersOfTen.Length-1; i >= 0; i--)
			{
				double power;
				if (i == 0) power = 100;
				else power = Math.Pow(10, i*3);
		
				if (Byte.MaxValue < power) continue;

				if ((value / (Byte)power) > 0)
				{
					text += ToText(value / (Byte)power) + " " + PowersOfTen[i] + " ";
					value %= (Byte)power;
				}
			}

			if (value > 0) 
			{
				if (text != "") text += "and ";
		
				if (value < 20)
					text += Numbers[value];
				else 
				{
					text += NumbersByTen[value/10];
					if ((value %= 10) > 0) text += "-" + Numbers[value];
				}
			}
			return text.Trim();
        }

		public static string ToOrdinalText(this Byte value)
		{
			string text = ToText(value);
			if(text.EndsWith("One"))
				return text.Substring(0, text.Length-3) + "First";
			else if(text.EndsWith("Two"))
				return text.Substring(0, text.Length-3) + "Second";
			else if(text.EndsWith("Three"))
				return text.Substring(0, text.Length-5) + "Third";
			else if(text.EndsWith("ve"))
				return text.Substring(0, text.Length-2) + "fth";
			else if(text.EndsWith("e"))
				return text.Substring(0, text.Length-1) + "th";
			else if(text.EndsWith("y"))
				return text.Substring(0, text.Length-1) + "ieth";
			else if(text.EndsWith("t"))
				return text + "h";
			else
				return text + "th";
		}
		
		public static string ToOrdinal(this Byte value)
        { 
            switch (value % 100)
            {
                case 11:
                case 12:
                case 13:
                    return value.ToString() + "th";
            }

            switch (value % 10)
            {
                case 1:
                    return value.ToString() + "st";
                case 2:
                    return value.ToString() + "nd";
                case 3:
                    return value.ToString() + "rd";
                default:
                    return value.ToString() + "th";
            }
        }
		public static string ToText(this UInt16 value)
        {
            if (value == 0) return Numbers[value];

			string text = string.Empty;

			for (int i = PowersOfTen.Length-1; i >= 0; i--)
			{
				double power;
				if (i == 0) power = 100;
				else power = Math.Pow(10, i*3);
		
				if (UInt16.MaxValue < power) continue;

				if ((value / (UInt16)power) > 0)
				{
					text += ToText(value / (UInt16)power) + " " + PowersOfTen[i] + " ";
					value %= (UInt16)power;
				}
			}

			if (value > 0) 
			{
				if (text != "") text += "and ";
		
				if (value < 20)
					text += Numbers[value];
				else 
				{
					text += NumbersByTen[value/10];
					if ((value %= 10) > 0) text += "-" + Numbers[value];
				}
			}
			return text.Trim();
        }

		public static string ToOrdinalText(this UInt16 value)
		{
			string text = ToText(value);
			if(text.EndsWith("One"))
				return text.Substring(0, text.Length-3) + "First";
			else if(text.EndsWith("Two"))
				return text.Substring(0, text.Length-3) + "Second";
			else if(text.EndsWith("Three"))
				return text.Substring(0, text.Length-5) + "Third";
			else if(text.EndsWith("ve"))
				return text.Substring(0, text.Length-2) + "fth";
			else if(text.EndsWith("e"))
				return text.Substring(0, text.Length-1) + "th";
			else if(text.EndsWith("y"))
				return text.Substring(0, text.Length-1) + "ieth";
			else if(text.EndsWith("t"))
				return text + "h";
			else
				return text + "th";
		}
		
		public static string ToOrdinal(this UInt16 value)
        { 
            switch (value % 100)
            {
                case 11:
                case 12:
                case 13:
                    return value.ToString() + "th";
            }

            switch (value % 10)
            {
                case 1:
                    return value.ToString() + "st";
                case 2:
                    return value.ToString() + "nd";
                case 3:
                    return value.ToString() + "rd";
                default:
                    return value.ToString() + "th";
            }
        }
		public static string ToText(this UInt32 value)
        {
            if (value == 0) return Numbers[value];

			string text = string.Empty;

			for (int i = PowersOfTen.Length-1; i >= 0; i--)
			{
				double power;
				if (i == 0) power = 100;
				else power = Math.Pow(10, i*3);
		
				if (UInt32.MaxValue < power) continue;

				if ((value / (UInt32)power) > 0)
				{
					text += ToText(value / (UInt32)power) + " " + PowersOfTen[i] + " ";
					value %= (UInt32)power;
				}
			}

			if (value > 0) 
			{
				if (text != "") text += "and ";
		
				if (value < 20)
					text += Numbers[value];
				else 
				{
					text += NumbersByTen[value/10];
					if ((value %= 10) > 0) text += "-" + Numbers[value];
				}
			}
			return text.Trim();
        }

		public static string ToOrdinalText(this UInt32 value)
		{
			string text = ToText(value);
			if(text.EndsWith("One"))
				return text.Substring(0, text.Length-3) + "First";
			else if(text.EndsWith("Two"))
				return text.Substring(0, text.Length-3) + "Second";
			else if(text.EndsWith("Three"))
				return text.Substring(0, text.Length-5) + "Third";
			else if(text.EndsWith("ve"))
				return text.Substring(0, text.Length-2) + "fth";
			else if(text.EndsWith("e"))
				return text.Substring(0, text.Length-1) + "th";
			else if(text.EndsWith("y"))
				return text.Substring(0, text.Length-1) + "ieth";
			else if(text.EndsWith("t"))
				return text + "h";
			else
				return text + "th";
		}
		
		public static string ToOrdinal(this UInt32 value)
        { 
            switch (value % 100)
            {
                case 11:
                case 12:
                case 13:
                    return value.ToString() + "th";
            }

            switch (value % 10)
            {
                case 1:
                    return value.ToString() + "st";
                case 2:
                    return value.ToString() + "nd";
                case 3:
                    return value.ToString() + "rd";
                default:
                    return value.ToString() + "th";
            }
        }
		public static string ToText(this UInt64 value)
        {
            if (value == 0) return Numbers[value];

			string text = string.Empty;

			for (int i = PowersOfTen.Length-1; i >= 0; i--)
			{
				double power;
				if (i == 0) power = 100;
				else power = Math.Pow(10, i*3);
		
				if (UInt64.MaxValue < power) continue;

				if ((value / (UInt64)power) > 0)
				{
					text += ToText(value / (UInt64)power) + " " + PowersOfTen[i] + " ";
					value %= (UInt64)power;
				}
			}

			if (value > 0) 
			{
				if (text != "") text += "and ";
		
				if (value < 20)
					text += Numbers[value];
				else 
				{
					text += NumbersByTen[value/10];
					if ((value %= 10) > 0) text += "-" + Numbers[value];
				}
			}
			return text.Trim();
        }

		public static string ToOrdinalText(this UInt64 value)
		{
			string text = ToText(value);
			if(text.EndsWith("One"))
				return text.Substring(0, text.Length-3) + "First";
			else if(text.EndsWith("Two"))
				return text.Substring(0, text.Length-3) + "Second";
			else if(text.EndsWith("Three"))
				return text.Substring(0, text.Length-5) + "Third";
			else if(text.EndsWith("ve"))
				return text.Substring(0, text.Length-2) + "fth";
			else if(text.EndsWith("e"))
				return text.Substring(0, text.Length-1) + "th";
			else if(text.EndsWith("y"))
				return text.Substring(0, text.Length-1) + "ieth";
			else if(text.EndsWith("t"))
				return text + "h";
			else
				return text + "th";
		}
		
		public static string ToOrdinal(this UInt64 value)
        { 
            switch (value % 100)
            {
                case 11:
                case 12:
                case 13:
                    return value.ToString() + "th";
            }

            switch (value % 10)
            {
                case 1:
                    return value.ToString() + "st";
                case 2:
                    return value.ToString() + "nd";
                case 3:
                    return value.ToString() + "rd";
                default:
                    return value.ToString() + "th";
            }
        }
    }
}

