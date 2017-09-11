using CommonExtensions.NumericExtensions;
using System;

namespace CommonExtensions.StringExtensions
{
    public static partial class StringExt
    {
        private const string ELLIPSIS = "…";

        /// <summary>
        /// Take the set number of characters from the left side of the string.
        /// </summary>
        /// <param name="value">The String</param>
        /// <param name="length">The size to reduce the string to.</param>
        /// <returns>Truncated string or existing string if length is larger than current string length.</returns>
        /// <remarks></remarks>
        public static string FromLeft(this string value, int length)
        {
            if (value.Length <= length) return value;
            else if (length < 0) return FromRight(value, Math.Abs(length));
            else return value.Substring(0, length);
        }

        /// <summary>
        /// Take the set number of characters from the right side of the string.
        /// </summary>
        /// <param name="value">The String</param>
        /// <param name="length">The size to reduce the string to.</param>
        /// <returns>Truncated string or existing string if length is larger than current string length.</returns>
        /// <remarks></remarks>
        public static string FromRight(this string value, int length)
        {
            if (value.Length <= length) return value;
            else if (length < 0) return FromLeft(value, Math.Abs(length));
            else return value.Substring(value.Length - length, length);
        }

        /// <summary>
        /// Take the set number of characters from the center side of the string.
        /// </summary>
        /// <param name="value">The String</param>
        /// <param name="length">The size to reduce the string to.</param>
        /// <returns>Truncated string or existing string if length is larger than current string length.</returns>
        /// <remarks></remarks>
        public static string FromCenter(this string value, int length)
        {
            if (value.Length <= length) return value;
            else if (length < 0) return FromOutside(value, Math.Abs(length));
            else return value.Substring((value.Length - length) / 2, length);
        }

        /// <summary>
        /// Take the set number of characters from the outsides of the string.
        /// </summary>
        /// <param name="value">The String</param>
        /// <param name="length">The size to reduce the string to.</param>
        /// <returns>Truncated string or existing string if length is larger than current string length.</returns>
        /// <remarks></remarks>
        public static string FromOutside(this string value, int length)
        {
            if (value.Length <= length) return value;
            else if (length < 0) return FromCenter(value, Math.Abs(length));
            else
            {
                var endlength = length - (length / 2);
                return value.Substring(0, length / 2) + value.Substring(value.Length - endlength, endlength);
            }
        }

        /// <summary>
        /// Try to take the set number of characters from the left side of the string, 
        /// but return less with the toAppend value added to the right side if it is longer.
        /// </summary>
        /// <param name="value">The String</param>
        /// <param name="length">The size to reduce the string to.</param>
        /// <param name="toAppend">The string to append onto the right side if the requested length is shorter than the primary string</param>
        /// <returns>Truncated string with appended string or existing string if length is larger than current string length.</returns>
        /// <remarks></remarks>
        public static string FromLeftWith(this string value, int length, string toAppend)
        {
            if (value.Length <= length) return value;
            else if (length.BetweenMin(0, toAppend.Length)) return string.Empty;
            else if (length < 0) return FromRightWith(value, Math.Abs(length), toAppend);
            else return value.Substring(0, length - toAppend.Length) + toAppend;
        }

        /// <summary>
        /// Try to take the set number of characters from the right side of the string, 
        /// but return less with the toAppend value added to the left side if it is longer.
        /// </summary>
        /// <param name="value">The String</param>
        /// <param name="length">The size to reduce the string to.</param>
        /// <param name="toAppend">The string to append onto the left side if the requested length is shorter than the primary string</param>
        /// <returns>Truncated string with appended string or existing string if length is larger than current string length.</returns>
        /// <remarks></remarks>
        public static string FromRightWith(this string value, int length, string toAppend)
        {
            if (value.Length <= length) return value;
            else if (length.BetweenMin(0, toAppend.Length)) return string.Empty;
            else if (length < 0) return FromLeftWith(value, Math.Abs(length), toAppend);
            else return toAppend + value.Substring(value.Length - length + toAppend.Length, length - toAppend.Length);
        }

        /// <summary>
        /// Try to take the set number of characters from the center of the string, 
        /// but return less with the toAppend value added to the outsides if it is longer.
        /// </summary>
        /// <param name="value">The String</param>
        /// <param name="length">The size to reduce the string to.</param>
        /// <param name="toAppend">The string to append onto the left and right sides if the requested length is shorter than the primary string</param>
        /// <returns>Truncated string with appended string or existing string if length is larger than current string length.</returns>
        /// <remarks>Due to rounding, if there is an odd length the string returned will be one closer to the left.</remarks>
        public static string FromCenterWith(this string value, int length, string toAppend)
        {
            if (value.Length <= length)
                return value;
            else if (length.BetweenMin(0, toAppend.Length * 2))
                return string.Empty;
            else if (length < 0)
                return FromOutsideWith(value, Math.Abs(length), toAppend);
            else
            {
                var sublength = length - (toAppend.Length * 2);
                return toAppend + value.Substring((value.Length - sublength) / 2, sublength) + toAppend;
            }
        }

        /// <summary>
        /// Try to take the set number of characters from the center of the string, 
        /// but return less with the toAppend value added to the outsides if it is longer.
        /// </summary>
        /// <param name="value">The String</param>
        /// <param name="length">The size to reduce the string to.</param>
        /// <param name="toAppendLeft">The string to append onto the left side if the length is shorter than the primary string</param>
        /// /// <param name="toAppendRight">The string to append onto the right side if the requested length is shorter than the primary string</param>
        /// <returns>Truncated string with appended string or existing string if length is larger than current string length.</returns>
        /// <remarks>Due to rounding, if there is an odd length the string returned will be one closer to the left.</remarks>
        public static string FromCenterWith(this string value, int length, string toAppendLeft, string toAppendRight)
        {
            if (value.Length <= length)
                return value;
            else if (length.BetweenMin(0, toAppendLeft.Length + toAppendRight.Length))
                return string.Empty;
            else if (length < 0)
                return FromOutsideWith(value, Math.Abs(length), toAppendLeft + toAppendRight);
            else
            {
                var sublength = length - (toAppendLeft.Length + toAppendRight.Length);
                return toAppendLeft + value.Substring((value.Length - sublength) / 2, sublength) + toAppendRight;
            }
        }

        /// <summary>
        /// Try to take the set number of characters from the outsides of the string, 
        /// but return less with the toAppend value inserted in the middle if it is longer.
        /// </summary>
        /// <param name="value">The String</param>
        /// <param name="length">The size to reduce the string to.</param>
        /// <param name="toAppend">The string to insert into the center if the requested length is shorter than the primary string</param>
        /// <returns>Truncated string with appended string or existing string if length is larger than current string length.</returns>
        /// <remarks>Due to rounding, if there is an odd length the result will put the extra character on the right side of the divide.</remarks>
        public static string FromOutsideWith(this string value, int length, string toAppend)
        {
            if (value.Length <= length)
                return value;
            else if (length.BetweenMin(0, toAppend.Length))
                return string.Empty;
            else if (length < 0)
                return FromCenterWith(value, Math.Abs(length), toAppend);
            else
            {
                var sublength = length - toAppend.Length;
                var endlength = sublength - (sublength / 2);
                return value.Substring(0, sublength / 2) + toAppend + value.Substring(value.Length - endlength, endlength);
            }
        }

        /// <summary>
        /// Try to take the set number of characters from the left side of the string, but return less with a "..." if it is longer.
        /// </summary>
        /// <param name="value">The String</param>
        /// <param name="length">The size to reduce the string to.</param>
        /// <returns>Truncated string with ellipsis on end or existing string if length is larger than current string length.</returns>
        /// <remarks></remarks>
        public static string FromLeftWithEllipsis(this string value, int length)
        {
            return FromLeftWith(value, length, ELLIPSIS);
        }

        /// <summary>
        /// Try to take the set number of characters from the right side of the string, but return less with a "..." if it is longer.
        /// </summary>
        /// <param name="value">The String</param>
        /// <param name="length">The size to reduce the string to.</param>
        /// <returns>Truncated string with ellipsis on end or existing string if length is larger than current string length.</returns>
        /// <remarks></remarks>
        public static string FromRightWithEllipsis(this string value, int length)
        {
            return FromRightWith(value, length, ELLIPSIS);
        }

        /// <summary>
        /// Try to take the set number of characters from the center of the string, but return less with a "..." if it is longer.
        /// </summary>
        /// <param name="value">The String</param>
        /// <param name="length">The size to reduce the string to.</param>
        /// <returns>Truncated string with ellipsis on end or existing string if length is larger than current string length.</returns>
        /// <remarks></remarks>
        public static string FromCenterWithEllipsis(this string value, int length)
        {
            return FromCenterWith(value, length, ELLIPSIS);
        }

        /// <summary>
        /// Try to take the set number of characters from the outsides of the string, but return less with a "..." if it is longer.
        /// </summary>
        /// <param name="value">The String</param>
        /// <param name="length">The size to reduce the string to.</param>
        /// <returns>Truncated string with ellipsis on end or existing string if length is larger than current string length.</returns>
        /// <remarks></remarks>
        public static string FromOutsideWithEllipsis(this string value, int length)
        {
            return FromOutsideWith(value, length, ELLIPSIS);
        }

        public static string Interpolate(this string value, System.Text.RegularExpressions.MatchEvaluator evaluator)
        {
            var rxSubstituter = new System.Text.RegularExpressions.Regex(@"\{\#(\w*)\:?([^}]*)}");
            return rxSubstituter.Replace(value, evaluator);
            //return rxSubstituter.Replace(value, (m) =>
            //{
            //    var tag = m.Groups[1].Value.ToUpper();
            //    var format = m.Groups[2].Value;
            //    switch (tag)
            //    {
            //        case "GUID":
            //            return System.Guid.NewGuid().ToString(format);
            //        case "DATE":
            //            return DateTime.Now.ToString(format);
            //        default:
            //            return string.Empty;
            //    }
            //});
        }
    }
}
