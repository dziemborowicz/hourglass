// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DateTimeExtensions.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using Hourglass.Properties;

    /// <summary>
    /// Provides extensions methods for the <see cref="DateTime"/> class.
    /// </summary>
    public static class DateTimeExtensions
    {
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
        /// Converts a month number to a string representation of the month.
        /// </summary>
        /// <param name="month">A month number between 1 and 12 inclusive.</param>
        /// <param name="provider">An <see cref="IFormatProvider"/>.</param>
        /// <returns>The string representation of the month.</returns>
        public static string GetMonthString(int month, IFormatProvider provider)
        {
            IDictionary<int, string> months = GetMonthStrings(provider);
            
            string monthString;
            if (!months.TryGetValue(month, out monthString))
            {
                throw new ArgumentOutOfRangeException("month");
            }

            return monthString;
        }

        /// <summary>
        /// Converts a day number to a ordinal string representation of the day.
        /// </summary>
        /// <param name="day">A day number between 1 and 31 inclusive.</param>
        /// <param name="provider">An <see cref="IFormatProvider"/>.</param>
        /// <returns>The ordinal string representation of the day.</returns>
        public static string GetOrdinalDayString(int day, IFormatProvider provider)
        {
            if (day < 1 || day > 31)
            {
                throw new ArgumentOutOfRangeException("day");
            }

            if (day % 10 == 1 && day / 10 != 1)
            {
                return string.Format(
                    Resources.ResourceManager.GetEffectiveProvider(provider),
                    Resources.ResourceManager.GetString("DateTimeExtensionsNstFormatString", provider),
                    day);
            }
            else if (day % 10 == 2 && day / 10 != 1)
            {
                return string.Format(
                    Resources.ResourceManager.GetEffectiveProvider(provider),
                    Resources.ResourceManager.GetString("DateTimeExtensionsNndFormatString", provider),
                    day);
            }
            else if (day % 10 == 3 && day / 10 != 1)
            {
                return string.Format(
                    Resources.ResourceManager.GetEffectiveProvider(provider),
                    Resources.ResourceManager.GetString("DateTimeExtensionsNrdFormatString", provider),
                    day);
            }
            else
            {
                return string.Format(
                    Resources.ResourceManager.GetEffectiveProvider(provider),
                    Resources.ResourceManager.GetString("DateTimeExtensionsNthFormatString", provider),
                    day);
            }
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
        /// <param name="str">A string representation of a month.</param>
        /// <param name="provider">An <see cref="IFormatProvider"/>.</param>
        /// <returns>The month number parsed from the string.</returns>
        /// <exception cref="FormatException">If <paramref name="str"/> is not a supported representation of a month.
        /// </exception>
        public static int ParseMonth(string str, IFormatProvider provider)
        {
            IList<KeyValuePair<int, string>> matches = GetMonthStrings(provider)
                .Where(e => e.Value.StartsWith(str, true /* ignoreCase */, (CultureInfo)provider))
                .ToList();

            if (matches.Count != 1)
            {
                throw new FormatException();
            }

            return matches.First().Key;
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

        /// <summary>
        /// Returns a dictionary mapping month values to their localized string representations.
        /// </summary>
        /// <param name="provider">An <see cref="IFormatProvider"/>.</param>
        /// <returns>A dictionary mapping month values to their localized string representations.
        /// </returns>
        private static IDictionary<int, string> GetMonthStrings(IFormatProvider provider)
        {
            return new Dictionary<int, string>
            {
                { 1, Resources.ResourceManager.GetString("DateTimeExtensionsJanuary", provider) },
                { 2, Resources.ResourceManager.GetString("DateTimeExtensionsFebruary", provider) },
                { 3, Resources.ResourceManager.GetString("DateTimeExtensionsMarch", provider) },
                { 4, Resources.ResourceManager.GetString("DateTimeExtensionsApril", provider) },
                { 5, Resources.ResourceManager.GetString("DateTimeExtensionsMay", provider) },
                { 6, Resources.ResourceManager.GetString("DateTimeExtensionsJune", provider) },
                { 7, Resources.ResourceManager.GetString("DateTimeExtensionsJuly", provider) },
                { 8, Resources.ResourceManager.GetString("DateTimeExtensionsAugust", provider) },
                { 9, Resources.ResourceManager.GetString("DateTimeExtensionsSeptember", provider) },
                { 10, Resources.ResourceManager.GetString("DateTimeExtensionsOctober", provider) },
                { 11, Resources.ResourceManager.GetString("DateTimeExtensionsNovember", provider) },
                { 12, Resources.ResourceManager.GetString("DateTimeExtensionsDecember", provider) }
            };
        }
    }
}
