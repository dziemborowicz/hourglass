// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimeSpanExtensions.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass.Extensions
{
    using System;
    using System.Globalization;
    using System.Text;

    /// <summary>
    /// Provides extensions methods for the <see cref="TimeSpan"/> struct.
    /// </summary>
    public static class TimeSpanExtensions
    {
        /// <summary>
        /// Converts the value of a <see cref="TimeSpan"/> object to its equivalent natural string representation.
        /// </summary>
        /// <param name="timeSpan">A <see cref="TimeSpan"/>.</param>
        /// <returns>The natural string representation of the <see cref="TimeSpan"/>.</returns>
        public static string ToNaturalString(this TimeSpan timeSpan)
        {
            // Build string
            StringBuilder stringBuilder = new StringBuilder();

            // Days
            if (timeSpan.Days != 0)
            {
                stringBuilder.AppendFormat(
                    CultureInfo.InvariantCulture,
                    timeSpan.Days == 1 ? "{0} day " : "{0} days ",
                    timeSpan.Days);
            }

            // Hours
            if (timeSpan.Hours != 0)
            {
                stringBuilder.AppendFormat(
                    CultureInfo.InvariantCulture,
                    timeSpan.Hours == 1 ? "{0} hour " : "{0} hours ",
                    timeSpan.Hours);
            }

            // Minutes
            if (timeSpan.Minutes != 0)
            {
                stringBuilder.AppendFormat(
                    CultureInfo.InvariantCulture,
                    timeSpan.Minutes == 1 ? "{0} minute " : "{0} minutes ",
                    timeSpan.Minutes);
            }

            // Seconds
            stringBuilder.AppendFormat(
                CultureInfo.InvariantCulture,
                timeSpan.Seconds == 1 ? "{0} second" : "{0} seconds",
                timeSpan.Seconds);

            // Trim the last character
            return stringBuilder.ToString();
        }

        /// <summary>
        /// Converts the value of a <see cref="Nullable{TimeSpan}"/> object to its equivalent natural string
        /// representation.
        /// </summary>
        /// <param name="timeSpan">A <see cref="Nullable{TimeSpan}"/>.</param>
        /// <returns>The natural string representation of the <see cref="TimeSpan"/> represented by <paramref
        /// name="timeSpan"/>, or <see cref="string.Empty"/> if <paramref name="timeSpan"/> is <c>null</c>.</returns>
        public static string ToNaturalString(this TimeSpan? timeSpan)
        {
            return timeSpan.HasValue ? timeSpan.Value.ToNaturalString() : string.Empty;
        }
    }
}
