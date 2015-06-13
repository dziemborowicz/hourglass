// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CultureInfoExtensions.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass.Extensions
{
    using System;
    using System.Globalization;
    using System.Text.RegularExpressions;
    
    /// <summary>
    /// Provides extensions methods for the <see cref="CultureInfo"/> class and the related <see
    /// cref="IFormatProvider"/> interface.
    /// </summary>
    public static class CultureInfoExtensions
    {
        /// <summary>
        /// Returns a value indicating whether an <see cref="IFormatProvider"/> prefers the month-day-year ordering in
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
        /// Returns a value indicating whether an <see cref="IFormatProvider"/> prefers the year-month-day ordering in
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
