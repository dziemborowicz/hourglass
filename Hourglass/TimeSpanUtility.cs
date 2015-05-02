// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimeSpanUtility.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;

    /// <summary>
    /// A utility class for converting a <see cref="string"/> representation of a time interval to a <see
    /// cref="TimeSpan"/>, and for converting a <see cref="TimeSpan"/> to a natural <see cref="string"/>
    /// representation.
    /// </summary>
    public static class TimeSpanUtility
    {
        /// <summary>
        /// Parses a <see cref="string"/> representation of a time interval into a <see cref="TimeSpan"/>.
        /// </summary>
        /// <remarks>
        /// This overload uses the <see cref="CultureInfo.CurrentCulture"/> as the <see cref="IFormatProvider"/> when
        /// parsing.
        /// </remarks>
        /// <param name="str">A <see cref="string"/> representation of a time interval.</param>
        /// <returns>A <see cref="TimeSpan"/> representation of the time interval.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="str"/> is <c>null</c>.</exception>
        /// <exception cref="FormatException">If <paramref name="str"/> is not a valid representations of a time
        /// interval.</exception>
        public static TimeSpan ParseNatural(string str)
        {
            return ParseNatural(str, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Parses a <see cref="string"/> representation of a time interval into a <see cref="TimeSpan"/>.
        /// </summary>
        /// <param name="str">A <see cref="string"/> representation of a time interval.</param>
        /// <param name="provider">An <see cref="IFormatProvider"/> to use when parsing.</param>
        /// <returns>A <see cref="TimeSpan"/> representation of the time interval.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="str"/> is <c>null</c>.</exception>
        /// <exception cref="FormatException">If <paramref name="str"/> is not a valid representations of a time
        /// interval.</exception>
        public static TimeSpan ParseNatural(string str, IFormatProvider provider)
        {
            TimeSpan timeSpan;

            if (str == null)
            {
                throw new ArgumentNullException("str");
            }

            if (!TryParseNatural(str, provider, out timeSpan))
            {
                throw new FormatException();
            }

            return timeSpan;
        }

        /// <summary>
        /// Parses a <see cref="string"/> representation of a time interval into a <see cref="TimeSpan"/>.
        /// </summary>
        /// <remarks>
        /// This overload uses the <see cref="CultureInfo.CurrentCulture"/> as the <see cref="IFormatProvider"/> when
        /// parsing.
        /// </remarks>
        /// <param name="str">A <see cref="string"/> representation of a time interval.</param>
        /// <param name="timeSpan">A <see cref="TimeSpan"/> representation of the time interval, or <see
        /// cref="TimeSpan.Zero"/> if this method returns <c>false</c>.</param>
        /// <returns><c>true</c> if <paramref name="str"/> was successfully parsed into a <see cref="TimeSpan"/>, or
        /// <c>false</c> otherwise.</returns>
        public static bool TryParseNatural(string str, out TimeSpan timeSpan)
        {
            return TryParseNatural(str, CultureInfo.CurrentCulture, out timeSpan);
        }

        /// <summary>
        /// Parses a <see cref="string"/> representation of a time interval into a <see cref="TimeSpan"/>.
        /// </summary>
        /// <param name="str">A <see cref="string"/> representation of a time interval.</param>
        /// <param name="provider">An <see cref="IFormatProvider"/> to use when parsing.</param>
        /// <param name="timeSpan">A <see cref="TimeSpan"/> representation of the time interval, or <see
        /// cref="TimeSpan.Zero"/> if this method returns <c>false</c>.</param>
        /// <returns><c>true</c> if <paramref name="str"/> was successfully parsed into a <see cref="TimeSpan"/>, or
        /// <c>false</c> otherwise.</returns>
        public static bool TryParseNatural(string str, IFormatProvider provider, out TimeSpan timeSpan)
        {
            timeSpan = TimeSpan.Zero;

            // Null or empty input
            if (string.IsNullOrWhiteSpace(str))
            {
                return false;
            }

            // Trim whitespace
            str = str.Trim(' ', '\t', '\r', '\n');

            // Integer input
            if (Regex.IsMatch(str, @"^\d+$"))
            {
                int minutes;
                if (!int.TryParse(str, out minutes))
                {
                    return false;
                }

                timeSpan = new TimeSpan(0, minutes, 0);
                return true;
            }

            // Multi-part input
            string[] parts;
            if (Regex.IsMatch(str, @"^[\d.,;:]+$"))
            {
                parts = Regex.Split(str, @"[.,;:]");
            }
            else
            {
                parts = Regex.Split(str, @"\s+(?=[+\-\d\.])|(?<![+\-\d\.])(?=[+\-\d\.])");
            }

            // Get rid of empty parts
            parts = parts.Where(s => !string.IsNullOrEmpty(s)).ToArray();

            // Get values
            double[] values = new double[parts.Length];
            for (int i = 0; i < parts.Length; i++)
            {
                string part = parts[i];
                Match match = Regex.Match(part, @"^[+\-]?\d+(\.\d*)?|^[+\-]?\.\d+");
                if (match.Success)
                {
                    if (!double.TryParse(match.Value, out values[i]))
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }

            // Get explicit units
            int[] units = new int[parts.Length];
            for (int i = 0; i < parts.Length; i++)
            {
                string part = parts[i];
                if (Regex.IsMatch(part, @"^([+-])?(\d+(\.\d*)?|\.\d+?)\s*(d|dys?|days?)$"))
                {
                    units[i] = 24 * 60 * 60;
                }
                else if (Regex.IsMatch(part, @"^([+-])?(\d+(\.\d*)?|\.\d+?)\s*(h|hrs?|hours?)$"))
                {
                    units[i] = 60 * 60;
                }
                else if (Regex.IsMatch(part, @"^([+-])?(\d+(\.\d*)?|\.\d+?)\s*(m|mins?|minutes?)$"))
                {
                    units[i] = 60;
                }
                else if (Regex.IsMatch(part, @"^([+-])?(\d+(\.\d*)?|\.\d+?)\s*(s|secs?|seconds?)$"))
                {
                    units[i] = 1;
                }
                else if (Regex.IsMatch(part, @"^([+-])?(\d+(\.\d*)?|\.\d+?)$"))
                {
                    units[i] = 0;
                }
                else
                {
                    return false;
                }
            }

            // Fill units implicitly left
            int lastUnit = units[0];
            for (int i = 1; i < units.Length; i++)
            {
                if (units[i] == 0)
                {
                    if (lastUnit == 24 * 60 * 60)
                    {
                        units[i] = 60 * 60;
                    }
                    else if (lastUnit == 60 * 60)
                    {
                        units[i] = 60;
                    }
                    else if (lastUnit == 60)
                    {
                        units[i] = 1;
                    }
                    else if (lastUnit != 0)
                    {
                        return false;
                    }
                }

                lastUnit = units[i];
            }

            // Fill units positionally if required
            if (lastUnit == 0)
            {
                lastUnit = units[units.Length - 1] = 1;
            }

            // Fill units implicitly right
            for (int i = units.Length - 2; i >= 0; i--)
            {
                if (units[i] == 0)
                {
                    if (lastUnit == 1)
                    {
                        units[i] = 60;
                    }
                    else if (lastUnit == 60)
                    {
                        units[i] = 60 * 60;
                    }
                    else if (lastUnit == 60 * 60)
                    {
                        units[i] = 24 * 60 * 60;
                    }
                    else if (lastUnit != 0)
                    {
                        return false;
                    }
                }

                lastUnit = units[i];
            }

            // Calculate time
            long ticks = 0L;
            for (int i = 0; i < parts.Length; i++)
            {
                ticks += (long)(values[i] * units[i] * 10000000L);
            }

            timeSpan = new TimeSpan(ticks);
            return true;
        }

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

        /// <summary>
        /// Returns the larger of two <see cref="TimeSpan"/>s.
        /// </summary>
        /// <param name="a">The first <see cref="TimeSpan"/> to compare.</param>
        /// <param name="b">The second <see cref="TimeSpan"/> to compare.</param>
        /// <returns><paramref name="a"/> or <paramref name="b"/>, whichever is larger.</returns>
        public static TimeSpan Max(TimeSpan a, TimeSpan b)
        {
            return a > b ? a : b;
        }

        /// <summary>
        /// Returns the smaller of two <see cref="TimeSpan"/>s.
        /// </summary>
        /// <param name="a">The first <see cref="TimeSpan"/> to compare.</param>
        /// <param name="b">The second <see cref="TimeSpan"/> to compare.</param>
        /// <returns><paramref name="a"/> or <paramref name="b"/>, whichever is smaller.</returns>
        public static TimeSpan Min(TimeSpan a, TimeSpan b)
        {
            return a < b ? a : b;
        }
    }
}
