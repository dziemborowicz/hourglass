// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DateTimeUtility.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass
{
    using System;
    using System.Globalization;
    using System.Text.RegularExpressions;

    using Hourglass.Parsing;

    /// <summary>
    /// A utility class for converting a <see cref="string"/> representation of a date and time (optionally relative to
    /// a reference date) to a <see cref="DateTime"/>, and for converting a <see cref="DateTime"/> to a natural <see
    /// cref="string"/> representation.
    /// </summary>
    public static class DateTimeUtility
    {
        #region Public Methods

        /// <summary>
        /// Parses a <see cref="string"/> representation of a date and time relative to a reference date into a <see
        /// cref="DateTime"/>.
        /// </summary>
        /// <remarks>
        /// This overload uses <see cref="DateTime.Now"/> as the reference date and time and the <see
        /// cref="CultureInfo.CurrentCulture"/> as the <see cref="IFormatProvider"/> when parsing.
        /// </remarks>
        /// <param name="str">A <see cref="string"/> representation of a date and time.</param>
        /// <returns>A <see cref="DateTime"/> representation of the date and time.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="str"/> is <c>null</c>.</exception>
        /// <exception cref="FormatException">If <paramref name="str"/> is not a valid representation of a date and
        /// time.</exception>
        public static DateTime ParseNatural(string str)
        {
            return ParseNatural(str, DateTime.Now);
        }

        /// <summary>
        /// Parses a <see cref="string"/> representation of a date and time relative to a reference date into a <see
        /// cref="DateTime"/>.
        /// </summary>
        /// <remarks>
        /// This overload uses the <see cref="CultureInfo.CurrentCulture"/> as the <see cref="IFormatProvider"/> when
        /// parsing.
        /// </remarks>
        /// <param name="str">A <see cref="string"/> representation of a date and time.</param>
        /// <param name="referenceDate">A <see cref="DateTime"/> representing the reference date.</param>
        /// <returns>A <see cref="DateTime"/> representation of the date and time.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="str"/> is <c>null</c>.</exception>
        /// <exception cref="FormatException">If <paramref name="str"/> is not a valid representation of a date and
        /// time.</exception>
        public static DateTime ParseNatural(string str, DateTime referenceDate)
        {
            return ParseNatural(str, referenceDate, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Parses a <see cref="string"/> representation of a date and time relative to a reference date into a <see
        /// cref="DateTime"/>.
        /// </summary>
        /// <param name="str">A <see cref="string"/> representation of a date and time.</param>
        /// <param name="referenceDate">A <see cref="DateTime"/> representing the reference date.</param>
        /// <param name="provider">An <see cref="IFormatProvider"/> to use when parsing.</param>
        /// <returns>A <see cref="DateTime"/> representation of the date and time.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="str"/> is <c>null</c>.</exception>
        /// <exception cref="FormatException">If <paramref name="str"/> is not a valid representation of a date and
        /// time.</exception>
        public static DateTime ParseNatural(string str, DateTime referenceDate, IFormatProvider provider)
        {
            DateTimeToken dateTimeToken = ParseNaturalPart(str, provider);
            return dateTimeToken.GetEndTime(referenceDate);
        }

        /// <summary>
        /// Parses a <see cref="string"/> representation of a date and time relative to a reference date into a <see
        /// cref="DateTime"/>.
        /// </summary>
        /// <remarks>
        /// This overload uses <see cref="DateTime.Now"/> as the reference date and time and the <see
        /// cref="CultureInfo.CurrentCulture"/> as the <see cref="IFormatProvider"/> when parsing.
        /// </remarks>
        /// <param name="str">A <see cref="string"/> representation of a date and time.</param>
        /// <param name="dateTime">A <see cref="DateTime"/> representation of the date and time, or <see
        /// cref="DateTime.MinValue"/> if this method returns <c>false</c>.</param>
        /// <returns><c>true</c> if <paramref name="str"/> was successfully parsed into a <see cref="DateTime"/>, or
        /// <c>false</c> otherwise.</returns>
        public static bool TryParseNatural(string str, out DateTime dateTime)
        {
            return TryParseNatural(str, DateTime.Now, out dateTime);
        }

        /// <summary>
        /// Parses a <see cref="string"/> representation of a date and time relative to a reference date into a <see
        /// cref="DateTime"/>.
        /// </summary>
        /// <remarks>
        /// This overload uses the <see cref="CultureInfo.CurrentCulture"/> as the <see cref="IFormatProvider"/> when
        /// parsing.
        /// </remarks>
        /// <param name="str">A <see cref="string"/> representation of a date and time.</param>
        /// <param name="referenceDate">A <see cref="DateTime"/> representing the reference date.</param>
        /// <param name="dateTime">A <see cref="DateTime"/> representation of the date and time, or <see
        /// cref="DateTime.MinValue"/> if this method returns <c>false</c>.</param>
        /// <returns><c>true</c> if <paramref name="str"/> was successfully parsed into a <see cref="DateTime"/>, or
        /// <c>false</c> otherwise.</returns>
        public static bool TryParseNatural(string str, DateTime referenceDate, out DateTime dateTime)
        {
            return TryParseNatural(str, referenceDate, CultureInfo.CurrentCulture, out dateTime);
        }

        /// <summary>
        /// Parses a <see cref="string"/> representation of a date and time relative to a reference date into a <see
        /// cref="DateTime"/>.
        /// </summary>
        /// <param name="str">A <see cref="string"/> representation of a date and time.</param>
        /// <param name="referenceDate">A <see cref="DateTime"/> representing the reference date.</param>
        /// <param name="provider">An <see cref="IFormatProvider"/> to use when parsing.</param>
        /// <param name="dateTime">A <see cref="DateTime"/> representation of the date and time, or <see
        /// cref="DateTime.MinValue"/> if this method returns <c>false</c>.</param>
        /// <returns><c>true</c> if <paramref name="str"/> was successfully parsed into a <see cref="DateTime"/>, or
        /// <c>false</c> otherwise.</returns>
        public static bool TryParseNatural(string str, DateTime referenceDate, IFormatProvider provider, out DateTime dateTime)
        {
            try
            {
                dateTime = ParseNatural(str, referenceDate, provider);
                return true;
            }
            catch
            {
                dateTime = DateTime.MinValue;
                return false;
            }
        }

        /// <summary>
        /// Parses a <see cref="string"/> representation of a date and time into a <see cref="DateTimeToken"/>.
        /// </summary>
        /// <remarks>
        /// This overload uses the <see cref="CultureInfo.CurrentCulture"/> as the <see cref="IFormatProvider"/> when
        /// parsing.
        /// </remarks>
        /// <param name="str">A <see cref="string"/> representation of a date and time.</param>
        /// <returns>A <see cref="DateTimeToken"/> representation of the date and time.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="str"/> is <c>null</c>.</exception>
        /// <exception cref="FormatException">If <paramref name="str"/> is not a valid representation of a date and
        /// time.</exception>
        public static DateTimeToken ParseNaturalPart(string str)
        {
            return ParseNaturalPart(str, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Parses a <see cref="string"/> representation of a date and time into a <see cref="DateTimeToken"/>.
        /// </summary>
        /// <param name="str">A <see cref="string"/> representation of a date and time.</param>
        /// <param name="provider">An <see cref="IFormatProvider"/> to use when parsing.</param>
        /// <returns>A <see cref="DateTimeToken"/> representation of the date and time.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="str"/> is <c>null</c>.</exception>
        /// <exception cref="FormatException">If <paramref name="str"/> is not a valid representation of a date and
        /// time.</exception>
        public static DateTimeToken ParseNaturalPart(string str, IFormatProvider provider)
        {
            return (DateTimeToken)DateTimeToken.Parser.Instance.Parse(str, provider);
        }

        /// <summary>
        /// Parses a <see cref="string"/> representation of a date and time into a <see cref="DateTimeToken"/>.
        /// </summary>
        /// <remarks>
        /// This overload uses the <see cref="CultureInfo.CurrentCulture"/> as the <see cref="IFormatProvider"/> when
        /// parsing.
        /// </remarks>
        /// <param name="str">A <see cref="string"/> representation of a date and time.</param>
        /// <param name="dateTimeToken">A <see cref="DateTimeToken"/> representation of the date and time, or
        /// <c>null</c> if this method returns <c>false</c>.</param>
        /// <returns><c>true</c> if <paramref name="str"/> was successfully parsed into a <see cref="DateTimeToken"/>,
        /// or <c>false</c> otherwise.</returns>
        public static bool TryParseNaturalPart(string str, out DateTimeToken dateTimeToken)
        {
            return TryParseNaturalPart(str, CultureInfo.CurrentCulture, out dateTimeToken);
        }

        /// <summary>
        /// Parses a <see cref="string"/> representation of a date and time into a <see cref="DateTimeToken"/>.
        /// </summary>
        /// <param name="str">A <see cref="string"/> representation of a date and time.</param>
        /// <param name="provider">An <see cref="IFormatProvider"/> to use when parsing.</param>
        /// <param name="dateTimeToken">A <see cref="DateTimeToken"/> representation of the date and time, or
        /// <c>null</c> if this method returns <c>false</c>.</param>
        /// <returns><c>true</c> if <paramref name="str"/> was successfully parsed into a <see cref="DateTimeToken"/>,
        /// or <c>false</c> otherwise.</returns>
        public static bool TryParseNaturalPart(string str, IFormatProvider provider, out DateTimeToken dateTimeToken)
        {
            try
            {
                dateTimeToken = ParseNaturalPart(str, provider);
                return true;
            }
            catch
            {
                dateTimeToken = null;
                return false;
            }
        }

        /// <summary>
        /// Converts a <see cref="DateTime"/> to a natural <see cref="string"/> representation.
        /// </summary>
        /// <remarks>
        /// This overload uses the <see cref="CultureInfo.CurrentCulture"/> as the <see cref="IFormatProvider"/> when
        /// converting.
        /// </remarks>
        /// <param name="dateTime">A <see cref="DateTime"/>.</param>
        /// <returns>A natural <see cref="string"/> representation of the <see cref="DateTime"/>.</returns>
        public static string ToNaturalString(DateTime dateTime)
        {
            return ToNaturalString(dateTime, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Converts a <see cref="Nullable{DateTime}"/> to a natural <see cref="string"/> representation.
        /// </summary>
        /// <remarks>
        /// This overload uses the <see cref="CultureInfo.CurrentCulture"/> as the <see cref="IFormatProvider"/> when
        /// converting.
        /// </remarks>
        /// <param name="dateTime">A <see cref="Nullable{DateTime}"/>.</param>
        /// <returns>A natural <see cref="string"/> representation of the <see cref="Nullable{DateTime}"/>, or "null"
        /// if <paramref name="dateTime"/> is <c>null</c>.</returns>
        public static string ToNaturalString(DateTime? dateTime)
        {
            return dateTime.HasValue ? ToNaturalString(dateTime.Value) : "null";
        }

        /// <summary>
        /// Converts a <see cref="DateTime"/> to a natural <see cref="string"/> representation.
        /// </summary>
        /// <param name="dateTime">A <see cref="DateTime"/>.</param>
        /// <param name="provider">An <see cref="IFormatProvider"/> to use when converting. This value is only used to
        /// determine the order of the day, month and year in the date part of the <see cref="string"/>.</param>
        /// <returns>A natural <see cref="string"/> representation of the <see cref="DateTime"/>.</returns>
        public static string ToNaturalString(DateTime dateTime, IFormatProvider provider)
        {
            string monthFormatString = GetMonthFormatString(provider);

            if (dateTime.Second != 0)
            {
                return dateTime.ToString(monthFormatString + " h:mm:ss tt", CultureInfo.InvariantCulture);
            }

            if (dateTime.Minute != 0 || dateTime.Hour != 0)
            {
                return dateTime.ToString(monthFormatString + " h:mm tt", CultureInfo.InvariantCulture);
            }

            return dateTime.ToString(monthFormatString, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Converts a <see cref="Nullable{DateTime}"/> to a natural <see cref="string"/> representation.
        /// </summary>
        /// <param name="dateTime">A <see cref="Nullable{DateTime}"/>.</param>
        /// <param name="provider">An <see cref="IFormatProvider"/> to use when converting. This value is only used to
        /// determine the order of the day, month and year in the date part of the <see cref="string"/>.</param>
        /// <returns>A natural <see cref="string"/> representation of the <see cref="Nullable{DateTime}"/>, or "null"
        /// if <paramref name="dateTime"/> is <c>null</c>.</returns>
        public static string ToNaturalString(DateTime? dateTime, IFormatProvider provider)
        {
            return dateTime.HasValue ? ToNaturalString(dateTime.Value, provider) : "null";
        }

        /// <summary>
        /// Returns the later of two <see cref="DateTime"/>s.
        /// </summary>
        /// <param name="a">The first <see cref="DateTime"/> to compare.</param>
        /// <param name="b">The second <see cref="DateTime"/> to compare.</param>
        /// <returns><paramref name="a"/> or <paramref name="b"/>, whichever is later.</returns>
        public static DateTime Max(DateTime a, DateTime b)
        {
            return a > b ? a : b;
        }

        /// <summary>
        /// Returns the earlier of two <see cref="DateTime"/>s.
        /// </summary>
        /// <param name="a">The first <see cref="DateTime"/> to compare.</param>
        /// <param name="b">The second <see cref="DateTime"/> to compare.</param>
        /// <returns><paramref name="a"/> or <paramref name="b"/>, whichever is earlier.</returns>
        public static DateTime Min(DateTime a, DateTime b)
        {
            return a < b ? a : b;
        }

        /// <summary>
        /// Returns a value indicating whether the specified <see cref="IFormatProvider"/> prefers the month-day-year
        /// ordering in date representations.
        /// </summary>
        /// <param name="provider">An <see cref="IFormatProvider"/>.</param>
        /// <returns>A value indicating whether the specified <see cref="IFormatProvider"/> prefers the month-day-year
        /// ordering in date representations.</returns>
        public static bool IsMonthFirst(IFormatProvider provider)
        {
            DateTimeFormatInfo formatInfo = (DateTimeFormatInfo)provider.GetFormat(typeof(DateTimeFormatInfo));
            return Regex.IsMatch(formatInfo.ShortDatePattern, @"^.*M.*d.*y.*$");
        }

        /// <summary>
        /// Returns a value indicating whether the specified <see cref="IFormatProvider"/> prefers the year-month-day
        /// ordering in date representations.
        /// </summary>
        /// <param name="provider">An <see cref="IFormatProvider"/>.</param>
        /// <returns>A value indicating whether the specified <see cref="IFormatProvider"/> prefers the year-month-day
        /// ordering in date representations.</returns>
        public static bool IsYearFirst(IFormatProvider provider)
        {
            DateTimeFormatInfo formatInfo = (DateTimeFormatInfo)provider.GetFormat(typeof(DateTimeFormatInfo));
            return Regex.IsMatch(formatInfo.ShortDatePattern, @"^.*y.*M.*d.*$");
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Returns the format string for a date.
        /// </summary>
        /// <param name="provider">An <see cref="IFormatProvider"/> to use to determine the preference for the order of
        /// the day, month and year in the format string.</param>
        /// <returns>The format string for a date.</returns>
        private static string GetMonthFormatString(IFormatProvider provider)
        {
            if (IsMonthFirst(provider))
            {
                return "MMMM d, yyyy";
            }

            return "d MMMM yyyy";
        }

        #endregion
    }
}
