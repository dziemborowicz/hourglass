// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StandardTimePart.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass.Parsing
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Represents the period of an hour.
    /// </summary>
    public enum HourPeriod
    {
        /// <summary>
        /// Ante meridiem.
        /// </summary>
        Am,

        /// <summary>
        /// Post meridiem.
        /// </summary>
        Pm
    }

    /// <summary>
    /// Represents a time of day specified as an hour, minute, and second.
    /// </summary>
    public class StandardTimePart : TimePart
    {
        /// <summary>
        /// Gets or sets the period of an hour (AM or PM) represented by this part.
        /// </summary>
        public HourPeriod? HourPeriod { get; set; }

        /// <summary>
        /// Gets or sets the hour represented by this part.
        /// </summary>
        public int? Hour { get; set; }

        /// <summary>
        /// Gets or sets the minute represented by this part.
        /// </summary>
        public int? Minute { get; set; }

        /// <summary>
        /// Gets or sets the second represented by this part.
        /// </summary>
        public int? Second { get; set; }

        /// <summary>
        /// Gets the <see cref="Hour"/> expressed as a value between 0 and 23 inclusive.
        /// </summary>
        public int? NormalizedHour
        {
            get
            {
                // Convert 12 am to 0000h
                if (this.HourPeriod == Parsing.HourPeriod.Am && this.Hour == 12)
                {
                    return 0;
                }
                
                // Convert 1-11 pm to 1300-2300h
                if (this.HourPeriod == Parsing.HourPeriod.Pm && this.Hour.HasValue && this.Hour < 12)
                {
                    return this.Hour + 12;
                }
                
                // Convert 12 to 0000h
                if (!this.HourPeriod.HasValue && this.Hour == 12)
                {
                    return 0;
                }

                // Other values are already normalized
                return this.Hour;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the part is valid.
        /// </summary>
        public override bool IsValid
        {
            get
            {
                return (!this.Hour.HasValue || (this.Hour >= 0 && this.Hour < 24))
                    && (!this.Minute.HasValue || (this.Minute >= 0 && this.Minute < 60))
                    && (!this.Second.HasValue || (this.Second >= 0 && this.Second < 60));
            }
        }

        /// <summary>
        /// Returns a concrete time represented by this part on or after the reference date and time.
        /// </summary>
        /// <param name="referenceDate">A reference date and time.</param>
        /// <param name="datePart">The concrete date represented by the corresponding <see cref="DatePart"/>.</param>
        /// <returns>A concrete time represented by this part.</returns>
        public override DateTime ToDateTime(DateTime referenceDate, DateTime datePart)
        {
            this.ThrowIfNotValid();

            DateTime dateTime = new DateTime(
                datePart.Year,
                datePart.Month,
                datePart.Day,
                this.NormalizedHour ?? 0,
                this.Minute ?? 0,
                this.Second ?? 0);

            // If hour period is not specified, prefer the one that is after the reference date and time
            if (!this.HourPeriod.HasValue &&
                this.NormalizedHour.HasValue &&
                this.NormalizedHour < 12 &&
                dateTime <= referenceDate &&
                dateTime.AddHours(12) > referenceDate)
            {
                dateTime = dateTime.AddHours(12);
            }

            // If hour period is not specified, prefer daytime hours (except on reference date)
            if (!this.HourPeriod.HasValue &&
                this.NormalizedHour.HasValue &&
                this.NormalizedHour > 0 &&
                this.NormalizedHour < 8 &&
                dateTime.Date != referenceDate.Date)
            {
                dateTime = dateTime.AddHours(12);
            }

            return dateTime;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            // Hour
            if (this.Hour.HasValue)
            {
                stringBuilder.Append(this.Hour == 0 ? 12 : this.Hour.Value);

                // Minute
                if (this.Minute.HasValue)
                {
                    stringBuilder.AppendFormat(":{0:00}", this.Minute.Value);

                    // Second
                    if (this.Second.HasValue)
                    {
                        stringBuilder.AppendFormat(":{0:00}", this.Second.Value);
                    }
                }

                // AM/PM
                if (this.HourPeriod.HasValue)
                {
                    if (this.Hour == 0 || (this.Hour == 12 && this.HourPeriod == Parsing.HourPeriod.Am))
                    {
                        stringBuilder.Append(" midnight");
                    }
                    else if (this.Hour == 12 && this.HourPeriod == Parsing.HourPeriod.Pm)
                    {
                        stringBuilder.Append(" noon");
                    }
                    else
                    {
                        stringBuilder.Append(this.HourPeriod == Parsing.HourPeriod.Am ? " am" : " pm");
                    }
                }
                else if (!this.Minute.HasValue)
                {
                    stringBuilder.Append(" o'clock");
                }
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Parses <see cref="StandardTimePart"/>s from <see cref="string"/>s.
        /// </summary>
        public new class Parser : TimePart.Parser
        {
            /// <summary>
            /// Singleton instance of the <see cref="Parser"/> class.
            /// </summary>
            public static readonly Parser Instance = new Parser();

            /// <summary>
            /// A regular expression that matches times with separators (e.g., "5", "5p", "5 pm", "5:30", "5:30 p.m.",
            /// "5:30:45 p.m.", "17:30h").
            /// </summary>
            private const string TimeWithSeparatorsPattern =
                @"  (?<hour>\d\d?)
                    (
                        [.:]
                        (?<minute>\d\d?)
                        (
                            [.:]
                            (?<second>\d\d?)
                        )?
                    )?
                    \s*
                    (
                        (?<ampm>
                            (a|p)\.?
                            (\s*m\.?)?
                            |
                            h[a-z]*
                        )?
                        |
                        o'?clock
                    )
                ";

            /// <summary>
            /// A regular expression that matches times without separators (e.g., "5", "5p", "5 pm", "530", "530 p.m.",
            /// "53045 p.m.", "1730h").
            /// </summary>
            private const string TimeWithoutSeparatorsPattern =
                @"  (?<hour>\d\d?)
                    (
                        (?<minute>\d\d)
                        (?<second>\d\d)?
                    )?
                    \s*
                    (
                        (?<ampm>
                            (a|p)\.?
                            (\s*m\.?)?
                            |
                            h[a-z]*
                        )?
                        |
                        o'?clock
                    )
                ";

            /// <summary>
            /// Prevents a default instance of the <see cref="Parser"/> class from being created.
            /// </summary>
            private Parser()
            {
            }

            /// <summary>
            /// Returns the regular expressions supported by this <see cref="Parser"/>.
            /// </summary>
            /// <param name="provider">An <see cref="IFormatProvider"/> to use when parsing.</param>
            /// <returns>The regular expressions supported by this <see cref="Parser"/>.</returns>
            public override IEnumerable<string> GetPatterns(IFormatProvider provider)
            {
                return new[]
                {
                    TimeWithSeparatorsPattern,
                    TimeWithoutSeparatorsPattern
                };
            }

            /// <summary>
            /// Parses a <see cref="TimePart"/> from a regular expression <see cref="Match"/>.
            /// </summary>
            /// <param name="match">A <see cref="Match"/> corresponding to a pattern returned by <see
            /// cref="GetPatterns"/>.</param>
            /// <param name="provider">An <see cref="IFormatProvider"/> to use when parsing.</param>
            /// <returns>aA<see cref="TimePart"/> from the regular expression <see cref="Match"/>.</returns>
            protected override TimePart ParseInternal(Match match, IFormatProvider provider)
            {
                StandardTimePart timePart = new StandardTimePart();

                // Parse hour period
                if (match.Groups["ampm"].Success)
                {
                    if (match.Groups["ampm"].Value.StartsWith("a", StringComparison.InvariantCultureIgnoreCase))
                    {
                        timePart.HourPeriod = Parsing.HourPeriod.Am;
                    }
                    else if (match.Groups["ampm"].Value.StartsWith("p", StringComparison.InvariantCultureIgnoreCase))
                    {
                        timePart.HourPeriod = Parsing.HourPeriod.Pm;
                    }
                }

                // Parse hour
                if (match.Groups["hour"].Success)
                {
                    timePart.Hour = int.Parse(match.Groups["hour"].Value, provider);
                }

                // Parse minute
                if (match.Groups["minute"].Success)
                {
                    timePart.Minute = int.Parse(match.Groups["minute"].Value, provider);
                }

                // Parse second
                if (match.Groups["second"].Success)
                {
                    timePart.Second = int.Parse(match.Groups["second"].Value, provider);
                }

                return timePart;
            }
        }
    }
}
