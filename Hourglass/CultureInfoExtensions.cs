// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CultureInfoExtensions.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass
{
    using System;
    using System.Globalization;
    using System.Text.RegularExpressions;

    /// <summary>
    /// <see cref="CultureInfo"/> utility methods.
    /// </summary>
    public static class CultureInfoExtensions
    {
        /// <summary>
        /// Returns a value indicating whether a <see cref="IFormatProvider"/> prefers the month-day-year ordering in
        /// date representations.
        /// </summary>
        /// <param name="provider">An <see cref="IFormatProvider"/>.</param>
        /// <returns>A value indicating whether the specified <see cref="IFormatProvider"/> prefers the month-day-year
        /// ordering in date representations.</returns>
        public static bool IsMonthFirst(this IFormatProvider provider)
        {
            DateTimeFormatInfo formatInfo = (DateTimeFormatInfo)provider.GetFormat(typeof(DateTimeFormatInfo));
            return Regex.IsMatch(formatInfo.ShortDatePattern, @"^.*M.*d.*y.*$");
        }

        /// <summary>
        /// Returns a value indicating whether a <see cref="IFormatProvider"/> prefers the year-month-day ordering in
        /// date representations.
        /// </summary>
        /// <param name="provider">An <see cref="IFormatProvider"/>.</param>
        /// <returns>A value indicating whether the specified <see cref="IFormatProvider"/> prefers the year-month-day
        /// ordering in date representations.</returns>
        public static bool IsYearFirst(this IFormatProvider provider)
        {
            DateTimeFormatInfo formatInfo = (DateTimeFormatInfo)provider.GetFormat(typeof(DateTimeFormatInfo));
            return Regex.IsMatch(formatInfo.ShortDatePattern, @"^.*y.*M.*d.*$");
        }
    }
}
