// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimeSpanToken.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass.Parsing
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Text.RegularExpressions;

    using Hourglass.Extensions;
    using Hourglass.Properties;

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

            if (endTime < startTime)
            {
                throw new InvalidOperationException();
            }

            return endTime;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <param name="provider">An <see cref="IFormatProvider"/> to use.</param>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString(IFormatProvider provider)
        {
            try
            {
                this.ThrowIfNotValid();

                List<string> parts = new List<string>();

                // Years
                if (!double.Equals(this.Years, 0.0))
                {
                    parts.Add(GetStringWithUnits(this.Years, "Year", provider));
                }

                // Months
                if (!double.Equals(this.Months, 0.0))
                {
                    parts.Add(GetStringWithUnits(this.Months, "Month", provider));
                }

                // Weeks
                if (!double.Equals(this.Weeks, 0.0))
                {
                    parts.Add(GetStringWithUnits(this.Weeks, "Week", provider));
                }

                // Days
                if (!double.Equals(this.Days, 0.0))
                {
                    parts.Add(GetStringWithUnits(this.Days, "Day", provider));
                }

                // Hours
                if (!double.Equals(this.Hours, 0.0))
                {
                    parts.Add(GetStringWithUnits(this.Hours, "Hour", provider));
                }

                // Minutes
                if (!double.Equals(this.Minutes, 0.0))
                {
                    parts.Add(GetStringWithUnits(this.Minutes, "Minute", provider));
                }

                // Seconds
                if (!double.Equals(this.Seconds, 0.0) || parts.Count == 0)
                {
                    parts.Add(GetStringWithUnits(this.Seconds, "Second", provider));
                }

                // Join parts
                return string.Join(
                    Resources.ResourceManager.GetString("TimeSpanTokenUnitSeparator", provider),
                    parts);
            }
            catch
            {
                return this.GetType().ToString();
            }
        }

        /// <summary>
        /// Returns a string for the specified value with the specified unit (e.g., "5 minutes").
        /// </summary>
        /// <param name="value">A value.</param>
        /// <param name="unit">The unit part of the resource name for the unit string.</param>
        /// <param name="provider">An <see cref="IFormatProvider"/>.</param>
        /// <returns>A string for the specified value with the specified unit.</returns>
        private static string GetStringWithUnits(double value, string unit, IFormatProvider provider)
        {
            string resourceName = string.Format(
                CultureInfo.InvariantCulture,
                "TimeSpanToken{0}{1}FormatString",
                double.Equals(value, 1.0) ? "1" : "N",
                double.Equals(value, 1.0) ? unit : unit + "s");

            return string.Format(
                Resources.ResourceManager.GetEffectiveProvider(provider),
                Resources.ResourceManager.GetString(resourceName, provider),
                value);
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
                provider = Resources.ResourceManager.GetEffectiveProvider(provider);

                foreach (string pattern in GetPatterns(provider))
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

            /// <summary>
            /// Returns a set of regular expressions for matching time spans.
            /// </summary>
            /// <param name="provider">An <see cref="IFormatProvider"/>.</param>
            /// <returns>A set of regular expressions for matching time spans.</returns>
            private static IEnumerable<string> GetPatterns(IFormatProvider provider)
            {
                return new[]
                {
                    Resources.ResourceManager.GetString("TimeSpanTokenMinutesOnlyPattern", provider),
                    Resources.ResourceManager.GetString("TimeSpanTokenShortFormPattern", provider),
                    Resources.ResourceManager.GetString("TimeSpanTokenLongFormPattern", provider)
                };
            }
        }
    }
}
