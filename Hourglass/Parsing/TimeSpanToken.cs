// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimeSpanToken.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass.Parsing
{
    using System;
    using System.Globalization;
    using System.Text;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Represents a time interval.
    /// </summary>
    public class TimeSpanToken : TimerStartToken
    {
        /// <summary>
        /// Gets or sets the number of years.
        /// </summary>
        public double Years { get; set; }

        /// <summary>
        /// Gets or sets the number of months.
        /// </summary>
        public double Months { get; set; }

        /// <summary>
        /// Gets or sets the number of weeks.
        /// </summary>
        public double Weeks { get; set; }

        /// <summary>
        /// Gets or sets the number of days.
        /// </summary>
        public double Days { get; set; }

        /// <summary>
        /// Gets or sets the number of hours.
        /// </summary>
        public double Hours { get; set; }

        /// <summary>
        /// Gets or sets the number of minutes.
        /// </summary>
        public double Minutes { get; set; }

        /// <summary>
        /// Gets or sets the number of seconds.
        /// </summary>
        public double Seconds { get; set; }

        /// <summary>
        /// Gets a value indicating whether the token is valid.
        /// </summary>
        public override bool IsValid
        {
            get
            {
                return this.Years >= 0
                    && this.Months >= 0
                    && this.Weeks >= 0
                    && this.Days >= 0
                    && this.Hours >= 0
                    && this.Minutes >= 0
                    && this.Seconds >= 0;
            }
        }

        /// <summary>
        /// Returns the end time for a timer started with this token at a specified time.
        /// </summary>
        /// <param name="startTime">The time the timer is started.</param>
        /// <returns>The end time for a timer started with this token at the specified time.</returns>
        public override DateTime GetEndTime(DateTime startTime)
        {
            this.ThrowIfNotValid();

            DateTime endTime = startTime;
            endTime = endTime.AddSeconds(this.Seconds);
            endTime = endTime.AddMinutes(this.Minutes);
            endTime = endTime.AddHours(this.Hours);
            endTime = endTime.AddDays(this.Days);
            endTime = endTime.AddWeeks(this.Weeks);
            endTime = endTime.AddMonths(this.Months);
            endTime = endTime.AddYears(this.Years);
            return endTime;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            try
            {
                this.ThrowIfNotValid();

                // Build string
                StringBuilder stringBuilder = new StringBuilder();

                // Years
                if (!double.Equals(this.Years, 0.0))
                {
                    stringBuilder.AppendFormat(
                        CultureInfo.InvariantCulture,
                        double.Equals(this.Years, 1.0) ? "{0} year " : "{0} years ",
                        this.Years);
                }

                // Months
                if (!double.Equals(this.Months, 0.0))
                {
                    stringBuilder.AppendFormat(
                        CultureInfo.InvariantCulture,
                        double.Equals(this.Months, 1.0) ? "{0} month " : "{0} months ",
                        this.Months);
                }

                // Weeks
                if (!double.Equals(this.Weeks, 0.0))
                {
                    stringBuilder.AppendFormat(
                        CultureInfo.InvariantCulture,
                        double.Equals(this.Weeks, 1.0) ? "{0} week " : "{0} weeks ",
                        this.Weeks);
                }

                // Days
                if (!double.Equals(this.Days, 0.0))
                {
                    stringBuilder.AppendFormat(
                        CultureInfo.InvariantCulture,
                        double.Equals(this.Days, 1.0) ? "{0} day " : "{0} days ",
                        this.Days);
                }

                // Hours
                if (!double.Equals(this.Hours, 0.0))
                {
                    stringBuilder.AppendFormat(
                        CultureInfo.InvariantCulture,
                        double.Equals(this.Hours, 1.0) ? "{0} hour " : "{0} hours ",
                        this.Hours);
                }

                // Minutes
                if (!double.Equals(this.Minutes, 0.0))
                {
                    stringBuilder.AppendFormat(
                        CultureInfo.InvariantCulture,
                        double.Equals(this.Minutes, 1.0) ? "{0} minute " : "{0} minutes ",
                        this.Minutes);
                }

                // Seconds
                if (!double.Equals(this.Seconds, 0.0) || stringBuilder.Length == 0)
                {
                    stringBuilder.AppendFormat(
                        CultureInfo.InvariantCulture,
                        double.Equals(this.Seconds, 1.0) ? "{0} second " : "{0} seconds ",
                        this.Seconds);
                }

                // Trim the last character
                return stringBuilder.ToString(0, stringBuilder.Length - 1);
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Parses <see cref="TimeSpanToken"/> strings.
        /// </summary>
        public new class Parser : TimerStartToken.Parser
        {
            /// <summary>
            /// Singleton instance of the <see cref="Parser"/> class.
            /// </summary>
            public static readonly Parser Instance = new Parser();

            /// <summary>
            /// A regular expression that matches integer minutes by themselves (e.g., "5", "15").
            /// </summary>
            private const string MinutesOnlyPattern =
                @"  ^
                    \s*
                    (?<minutes>\d+)
                    \s*
                    $
                ";

            /// <summary>
            /// A regular expression that matches time spans in a short format (e.g., "5:30", "1:15:30:00", "7:30").
            /// </summary>
            private const string ShortFormPattern =
                @"  ^
                    \s*
                    (
                        (
                            (
                                (
                                    (?<years>\d+)
                                    \s*[.,:\s]\s*
                                )?
                                (?<months>\d+)
                                \s*[.,:\s]\s*
                            )?
                            (?<days>\d+)
                            \s*[.,:\s]\s*
                        )?
                        (?<hours>\d+)
                        \s*[.,:\s]\s*
                    )?
                    (?<minutes>\d+)
                    \s*[.,:\s]\s*
                    (?<seconds>\d+)
                    \s*
                    $
                ";

            /// <summary>
            /// A regular expression that matches time spans in a long format (e.g., "5 minutes 30 seconds", "1 day 15
            /// hours 30 minutes", "7 minutes 30", "1.5 hours").
            /// </summary>
            private const string LongFormPattern =
                @"  ^
                    (
                        \s*
                        (
                            (
                                (?<years>\d+|\d*[.,]\d+)
                                \s*
                                (y|yrs?|years?)
                                (
                                    \s*
                                    (?<months>\d+|\d*[.,]\d+)
                                    (\s+|$)
                                )?
                            )
                            |
                            (
                                (?<months>\d+|\d*[.,]\d+)
                                \s*
                                (mo|mon?s?|months?)
                                (
                                    \s*
                                    (?<days>\d+|\d*[.,]\d+)
                                    (\s+|$)
                                )?
                            )
                            |
                            (
                                (?<weeks>\d+|\d*[.,]\d+)
                                \s*
                                (w|wks?|weeks?)
                                (
                                    \s*
                                    (?<days>\d+|\d*[.,]\d+)
                                    (\s+|$)
                                )?
                            )
                            |
                            (
                                (?<days>\d+|\d*[.,]\d+)
                                \s*
                                (d|dys?|days?)
                                (
                                    \s*
                                    (?<hours>\d+|\d*[.,]\d+)
                                    (\s+|$)
                                )?
                            )
                            |
                            (
                                (?<hours>\d+|\d*[.,]\d+)
                                \s*
                                (h|hrs?|hours?)
                                (
                                    \s*
                                    (?<minutes>\d+|\d*[.,]\d+)
                                    (\s+|$)
                                )?
                            )
                            |
                            (
                                (?<minutes>\d+|\d*[.,]\d+)
                                \s*
                                (m|mins?|minutes?)
                                (
                                    \s*
                                    (?<seconds>\d+|\d*[.,]\d+)
                                    (\s+|$)
                                )?
                            )
                            |
                            (
                                (?<seconds>\d+|\d*[.,]\d+)
                                \s*
                                (s|secs?|seconds?)
                            )
                        )
                        \s*
                    )+
                    $
                ";

            /// <summary>
            /// A set of regular expressions for matching time spans.
            /// </summary>
            private static readonly string[] Patterns =
            {
                MinutesOnlyPattern,
                ShortFormPattern,
                LongFormPattern
            };

            /// <summary>
            /// Prevents a default instance of the <see cref="Parser"/> class from being created.
            /// </summary>
            private Parser()
            {
            }

            /// <summary>
            /// Parses a string into a <see cref="TimerStartToken"/>.
            /// </summary>
            /// <param name="str">A string representation of a <see cref="TimerStartToken"/>.</param>
            /// <param name="provider">An <see cref="IFormatProvider"/>.</param>
            /// <returns>The <see cref="TimerStartToken"/> parsed from the string.</returns>
            /// <exception cref="ArgumentNullException">If <paramref name="str"/> or <paramref name="provider"/> is
            /// <c>null</c>.</exception>
            /// <exception cref="FormatException">If <paramref name="str"/> is not a supported representation of a <see
            /// cref="TimerStartToken"/>.</exception>
            protected override TimerStartToken ParseInternal(string str, IFormatProvider provider)
            {
                foreach (string pattern in Patterns)
                {
                    try
                    {
                        Match match = Regex.Match(str, pattern, RegexOptions);
                        if (match.Success)
                        {
                            TimeSpanToken timeSpanToken = new TimeSpanToken();

                            // Years
                            foreach (Capture capture in match.Groups["years"].Captures)
                            {
                                timeSpanToken.Years += double.Parse(capture.Value, provider);
                            }

                            // Months
                            foreach (Capture capture in match.Groups["months"].Captures)
                            {
                                timeSpanToken.Months += double.Parse(capture.Value, provider);
                            }

                            // Weeks
                            foreach (Capture capture in match.Groups["weeks"].Captures)
                            {
                                timeSpanToken.Weeks += double.Parse(capture.Value, provider);
                            }

                            // Days
                            foreach (Capture capture in match.Groups["days"].Captures)
                            {
                                timeSpanToken.Days += double.Parse(capture.Value, provider);
                            }

                            // Hours
                            foreach (Capture capture in match.Groups["hours"].Captures)
                            {
                                timeSpanToken.Hours += double.Parse(capture.Value, provider);
                            }

                            // Minutes
                            foreach (Capture capture in match.Groups["minutes"].Captures)
                            {
                                timeSpanToken.Minutes += double.Parse(capture.Value, provider);
                            }

                            // Seconds
                            foreach (Capture capture in match.Groups["seconds"].Captures)
                            {
                                timeSpanToken.Seconds += double.Parse(capture.Value, provider);
                            }

                            return timeSpanToken;
                        }
                    }
                    catch
                    {
                        // Try the next pattern
                        continue;
                    }
                }

                // Could not find a matching pattern
                throw new FormatException();
            }
        }
    }
}
