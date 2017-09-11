using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonExtensions.DateTimeExtensions
{
    public static partial class DateTimeExt
    {
        /// <summary>
        /// Calculates the difference between two dates as it should for a person's birthdate.
        /// </summary>
        /// <param name="birthDate">The initial date to calculate from</param>
        /// <param name="currentDate">The date acting as the "present day"</param>
        /// <returns>A non-negative integer age</returns>
        /// <remarks>An alias for <see cref="ToNaturalAge"/></remarks>
        public static int ToAge(this DateTime birthDate, DateTime currentDate)
        {
            return ToNaturalAge(birthDate, currentDate);
        }

        /// <summary>
        /// Calculates the difference between two dates as it should for a person's birthdate.  The "current date" value is defaulted to DateTime.Today
        /// </summary>
        /// <param name="birthDate">The initial date to calculate from</param>
        /// <returns>A non-negative integer age</returns>
        public static int ToAge(this DateTime birthDate)
        {
            return ToNaturalAge(birthDate, DateTime.Today);
        }

        /// <summary>
        /// Calculates the difference between two dates as it should for a person's birthdate.
        /// </summary>
        /// <param name="birthDate">The initial date to calculate from</param>
        /// <param name="currentDate">The date acting as the "present day"</param>
        /// <returns>An integer age</returns>
        public static int ToIntegerAge(this DateTime birthDate, DateTime currentDate)
        {
            int age = currentDate.Year - birthDate.Year;

            if (currentDate.Month < birthDate.Month || (currentDate.Month == birthDate.Month && currentDate.Day < birthDate.Day))
                age -= 1;

            return age;
        }

        /// <summary>
        /// Calculates the difference between two dates as it should for a person's birthdate.
        /// </summary>
        /// <param name="birthDate">The initial date to calculate from</param>
        /// <param name="currentDate">The date acting as the "present day"</param>
        /// <returns>A non-negative integer age</returns>
        public static int ToNaturalAge(this DateTime birthDate, DateTime currentDate)
        {
            int age = ToIntegerAge(birthDate, currentDate);
            return age < 0 ? 0 : age;
        }
    }
}
