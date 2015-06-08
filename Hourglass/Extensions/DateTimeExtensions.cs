// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DateTimeExtensions.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass.Extensions
{
    using System;
    
    /// <summary>
    /// Provides extensions methods for the <see cref="DateTime"/> class.
    /// </summary>
    public static class DateTimeExtensions
    {
        /// <summary>
        /// The months of the year.
        /// </summary>
        private static readonly string[] Months =
        {
            "January",
            "February",
            "March",
            "April",
            "May",
            "June",
            "July",
            "August",
            "September",
            "October",
            "November",
            "December"
        };

        /// <summary>
        /// Returns a new <see cref="DateTime"/> that adds the specified number of weeks to the value of this instance.
        /// </summary>
        /// <param name="dateTime">A <see cref="DateTime"/>.</param>
        /// <param name="weeks">A number of weeks. The <paramref name="weeks"/> parameter can be negative or positive.
        /// </param>
        /// <returns>An object whose value is the sum of the date and time represented by this instance and <paramref
        /// name="weeks"/>.</returns>
        public static DateTime AddWeeks(this DateTime dateTime, double weeks)
        {
            return dateTime.AddDays(7 * weeks);
        }

        /// <summary>
        /// Returns a new <see cref="DateTime"/> that adds the specified number of months to the value of this instance.
        /// </summary>
        /// <param name="dateTime">A <see cref="DateTime"/>.</param>
        /// <param name="months">A number of months. The <paramref name="months"/> parameter can be negative or
        /// positive.</param>
        /// <returns>An object whose value is the sum of the date and time represented by this instance and <paramref
        /// name="months"/>.</returns>
        public static DateTime AddMonths(this DateTime dateTime, double months)
        {
            int wholeMonths = (int)months;
            double partMonth = months % 1.0;

            // Add whole months
            dateTime = dateTime.AddMonths(wholeMonths);

            // Add part month if required
            if (partMonth > 0.0)
            {
                int monthInDays = (dateTime.AddMonths(1) - dateTime).Days;
                dateTime = dateTime.AddDays(Math.Round(monthInDays * partMonth));
            }
            else if (partMonth < 0.0)
            {
                int monthInDays = (dateTime - dateTime.AddMonths(-1)).Days;
                dateTime = dateTime.AddDays(Math.Round(monthInDays * partMonth));
            }

            return dateTime;
        }

        /// <summary>
        /// Returns a new <see cref="DateTime"/> that adds the specified number of years to the value of this instance.
        /// </summary>
        /// <param name="dateTime">A <see cref="DateTime"/>.</param>
        /// <param name="years">A number of years. The <paramref name="years"/> parameter can be negative or positive.
        /// </param>
        /// <returns>An object whose value is the sum of the date and time represented by this instance and <paramref
        /// name="years"/>.</returns>
        public static DateTime AddYears(this DateTime dateTime, double years)
        {
            return dateTime.AddMonths(12 * years);
        }

        /// <summary>
        /// Increments a month by one.
        /// </summary>
        /// <param name="year">A year.</param>
        /// <param name="month">A month number between 1 and 12 inclusive.</param>
        public static void IncrementMonth(ref int year, ref int month)
        {
            if (month < 1 || month > 12)
            {
                throw new ArgumentOutOfRangeException("month");
            }

            if (month < 12)
            {
                month++;
            }
            else
            {
                month = 1;
                year++;
            }
        }

        /// <summary>
        /// Returns a value indicating whether a year, month, and day form a valid representation of a date.
        /// </summary>
        /// <param name="year">A year.</param>
        /// <param name="month">A month.</param>
        /// <param name="day">A day.</param>
        /// <returns>A value indicating whether a year, month, and day form a valid representation of a date.</returns>
        public static bool IsValid(int? year, int? month, int? day)
        {
            try
            {
                DateTime date = new DateTime(
                    year ?? 2000,
                    month ?? 1,
                    day ?? 1);

                return date != DateTime.MinValue && date != DateTime.MaxValue;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Parses a string into a month number between 1 and 12 inclusive.
        /// </summary>
        /// <remarks>
        /// This method only checks the first three characters of the string against the first three characters of the
        /// strings in the <see cref="Months"/> list.
        /// </remarks>
        /// <param name="str">A string representation of a month.</param>
        /// <returns>The month number parsed from the string.</returns>
        /// <exception cref="FormatException">If <paramref name="str"/> is not a supported representation of a month.
        /// </exception>
        /// <seealso cref="Months"/>
        public static int ParseMonth(string str)
        {
            for (int i = 0; i < Months.Length; i++)
            {
                if (str.StartsWith(Months[i].Substring(0, 3), StringComparison.InvariantCultureIgnoreCase))
                {
                    return i + 1;
                }
            }

            throw new FormatException();
        }

        /// <summary>
        /// Converts a month number to a string representation of the month.
        /// </summary>
        /// <param name="month">A month number between 1 and 12 inclusive.</param>
        /// <returns>The string representation of the month.</returns>
        public static string ToMonthString(int month)
        {
            if (month < 1 || month > 12)
            {
                throw new ArgumentOutOfRangeException("month");
            }

            return Months[month - 1];
        }

        /// <summary>
        /// Converts a day number to a ordinal string representation of the day.
        /// </summary>
        /// <param name="day">A day number between 1 and 31 inclusive.</param>
        /// <returns>The ordinal string representation of the day.</returns>
        public static string ToOrdinalDayString(int day)
        {
            if (day < 1 || day > 31)
            {
                throw new ArgumentOutOfRangeException("day");
            }

            if (day % 10 == 1 && day / 10 != 1)
            {
                return string.Format("{0}st", day);
            }

            if (day % 10 == 1 && day / 10 != 1)
            {
                return string.Format("{0}nd", day);
            }

            if (day % 10 == 1 && day / 10 != 1)
            {
                return string.Format("{0}rd", day);
            }

            return string.Format("{0}th", day);
        }

        /// <summary>
        /// Tries to create a new <see cref="DateTime"/> from a year, month, and day.
        /// </summary>
        /// <param name="year">A year.</param>
        /// <param name="month">A month.</param>
        /// <param name="day">A day.</param>
        /// <param name="dateTime">The <see cref="DateTime"/>.</param>
        /// <returns><c>true</c> if a new <see cref="DateTime"/> is successfully created from the year, month, and day,
        /// or <c>false</c> otherwise.</returns>
        public static bool TryToDateTime(int year, int month, int day, out DateTime dateTime)
        {
            try
            {
                dateTime = new DateTime(year, month, day);
                return true;
            }
            catch
            {
                dateTime = DateTime.MinValue;
                return false;
            }
        }
    }
}
