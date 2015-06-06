// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimeSpanUtility.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass
{
    using System;
    using System.Globalization;
    using System.Text;

    /// <summary>
    /// A utility class for converting a <see cref="string"/> representation of a time interval to a <see
    /// cref="TimeSpan"/>, and for converting a <see cref="TimeSpan"/> to a natural <see cref="string"/>
    /// representation.
    /// </summary>
    public static class TimeSpanUtility
    {
        /// <summary>
        /// Converts a <see cref="TimeSpan"/> to a natural <see cref="string"/> representation (e.g., "10 minutes 0
        /// seconds").
        /// </summary>
        /// <param name="timeSpan">A <see cref="TimeSpan"/>.</param>
        /// <returns>A natural <see cref="string"/> representation of the <see cref="TimeSpan"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">If the <see cref="TimeSpan"/> is less than <see
        /// cref="TimeSpan.Zero"/>.</exception>
        public static string ToNaturalString(TimeSpan timeSpan)
        {
            // Reject negative values
            if (timeSpan < TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException("timeSpan");
            }

            // Breakdown time interval
            long totalSeconds = timeSpan.Ticks / 10000000L;

            long days = totalSeconds / 60 / 60 / 24;
            long hours = (totalSeconds / 60 / 60) - (days * 24);
            long minutes = (totalSeconds / 60) - (days * 24 * 60) - (hours * 60);
            long seconds = totalSeconds % 60;

            // Build string
            StringBuilder stringBuilder = new StringBuilder();

            // Days
            if (days == 1)
            {
                stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "{0} day ", days);
            }
            else if (days != 0)
            {
                stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "{0} days ", days);
            }

            // Hours
            if (hours == 1)
            {
                stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "{0} hour ", hours);
            }
            else if (hours != 0 || days != 0)
            {
                stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "{0} hours ", hours);
            }

            // Minutes
            if (minutes == 1)
            {
                stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "{0} minute ", minutes);
            }
            else if (minutes != 0 || hours != 0 || days != 0)
            {
                stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "{0} minutes ", minutes);
            }

            // Seconds
            if (seconds == 1)
            {
                stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "{0} second", seconds);
            }
            else
            {
                stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "{0} seconds", seconds);
            }

            return stringBuilder.ToString();
        }
        
        /// <summary>
        /// Converts a <see cref="Nullable{TimeSpan}"/> to a natural <see cref="string"/> representation (e.g., "10
        /// minutes 0 seconds").
        /// </summary>
        /// <param name="timeSpan">A <see cref="Nullable{TimeSpan}"/>.</param>
        /// <returns>A natural <see cref="string"/> representation of the <see cref="Nullable{TimeSpan}"/>, or "null"
        /// if <paramref name="timeSpan"/> is <c>null</c>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">If the <see cref="Nullable{TimeSpan}"/> is less than <see
        /// cref="TimeSpan.Zero"/>.</exception>
        public static string ToNaturalString(TimeSpan? timeSpan)
        {
            return timeSpan.HasValue ? ToNaturalString(timeSpan.Value) : "null";
        }

        /// <summary>
        /// Converts a <see cref="TimeSpan"/> to a short natural <see cref="string"/> representation (e.g., "10
        /// minutes").
        /// </summary>
        /// <param name="timeSpan">A <see cref="TimeSpan"/>.</param>
        /// <returns>A short natural <see cref="string"/> representation of the <see cref="TimeSpan"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">If the <see cref="TimeSpan"/> is less than <see
        /// cref="TimeSpan.Zero"/>.</exception>
        public static string ToShortNaturalString(TimeSpan timeSpan)
        {
            // Reject negative values
            if (timeSpan < TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException("timeSpan");
            }

            // Breakdown time interval
            long totalSeconds = timeSpan.Ticks / 10000000L;

            long days = totalSeconds / 60 / 60 / 24;
            long hours = (totalSeconds / 60 / 60) - (days * 24);
            long minutes = (totalSeconds / 60) - (days * 24 * 60) - (hours * 60);
            long seconds = totalSeconds % 60;

            // Build string
            StringBuilder stringBuilder = new StringBuilder();

            // Days
            if (days == 1)
            {
                stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "{0} day ", days);
            }
            else if (days != 0)
            {
                stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "{0} days ", days);
            }

            // Hours
            if (hours == 1)
            {
                stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "{0} hour ", hours);
            }
            else if (hours != 0)
            {
                stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "{0} hours ", hours);
            }

            // Minutes
            if (minutes == 1)
            {
                stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "{0} minute ", minutes);
            }
            else if (minutes != 0)
            {
                stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "{0} minutes ", minutes);
            }

            // Seconds
            if (seconds == 1)
            {
                stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "{0} second ", seconds);
            }
            else if (seconds != 0 || (days == 0 && hours == 0 && minutes == 0))
            {
                stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "{0} seconds ", seconds);
            }

            // Trim the last character
            return stringBuilder.ToString(0, stringBuilder.Length - 1);
        }

        /// <summary>
        /// Converts a <see cref="Nullable{TimeSpan}"/> to a short natural <see cref="string"/> representation (e.g.,
        /// "10 minutes").
        /// </summary>
        /// <param name="timeSpan">A <see cref="Nullable{TimeSpan}"/>.</param>
        /// <returns>A short natural <see cref="string"/> representation of the <see cref="Nullable{TimeSpan}"/>, or
        /// "null" if <paramref name="timeSpan"/> is <c>null</c>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">If the <see cref="Nullable{TimeSpan}"/> is less than <see
        /// cref="TimeSpan.Zero"/>.</exception>
        public static string ToShortNaturalString(TimeSpan? timeSpan)
        {
            return timeSpan.HasValue ? ToShortNaturalString(timeSpan.Value) : "null";
        }
    }
}
