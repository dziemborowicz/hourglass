// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NormalTimeToken.cs" company="Chris Dziemborowicz">
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
        Pm,

        /// <summary>
        /// 24-hour time.
        /// </summary>
        Military
    }

    /// <summary>
    /// Represents the time part of an instant in time specified as an hour, minute, and second.
    /// </summary>
    public class NormalTimeToken : TimeToken
    {
        /// <summary>
        /// Gets or sets the period of an hour (AM or PM).
        /// </summary>
        public HourPeriod? HourPeriod { get; set; }

        /// <summary>
        /// Gets or sets the hour.
        /// </summary>
        public int? Hour { get; set; }

        /// <summary>
        /// Gets or sets the minute.
        /// </summary>
        public int? Minute { get; set; }

        /// <summary>
        /// Gets or sets the second.
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
        /// Gets a value indicating whether this token represents midnight.
        /// </summary>
        public bool IsMidnight
        {
            get
            {
                return (this.Hour == 0 || (this.Hour == 12 && this.HourPeriod == Parsing.HourPeriod.Am))
                    && (this.Minute ?? 0) == 0
                    && (this.Second ?? 0) == 0;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this token represents noon.
        /// </summary>
        public bool IsNoon
        {
            get
            {
                return (this.Hour == 12 && (this.HourPeriod == Parsing.HourPeriod.Pm || this.HourPeriod == Parsing.HourPeriod.Military))
                    && (this.Minute ?? 0) == 0
                    && (this.Second ?? 0) == 0;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the token is valid.
        /// </summary>
        public override bool IsValid
        {
            get
            {
                return (this.Hour.HasValue && this.Hour >= 0 && this.Hour < 24)
                    && (!this.Minute.HasValue || (this.Minute >= 0 && this.Minute < 60))
                    && (!this.Second.HasValue || (this.Second >= 0 && this.Second < 60));
            }
        }

        /// <summary>
        /// Returns the next date and time after <paramref name="minDate"/> that is represented by this token.
        /// </summary>
        /// <remarks>
        /// This method may return a date and time that is before <paramref name="minDate"/> if there is no date and
        /// time after <paramref name="minDate"/> that is represented by this token.
        /// </remarks>
        /// <param name="minDate">The minimum date and time to return.</param>
        /// <param name="datePart">The date part of the date and time to return.</param>
        /// <returns>The next date and time after <paramref name="minDate"/> that is represented by this token.
        /// </returns>
        /// <exception cref="InvalidOperationException">If this token is not valid.</exception>
        public override DateTime ToDateTime(DateTime minDate, DateTime datePart)
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
                dateTime <= minDate &&
                dateTime.AddHours(12) > minDate)
            {
                dateTime = dateTime.AddHours(12);
            }

            // If hour period is not specified, prefer daytime hours (except on the same day as the minimum date)
            if (!this.HourPeriod.HasValue &&
                this.NormalizedHour.HasValue &&
                this.NormalizedHour > 0 &&
                this.NormalizedHour < 8 &&
                dateTime.Date != minDate.Date)
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
            try
            {
                this.ThrowIfNotValid();

                StringBuilder stringBuilder = new StringBuilder();

                // Hour
                stringBuilder.Append(this.Hour == 0 ? 12 : this.Hour);

                // Minute
                if ((this.Minute ?? 0) != 0 || (this.Second ?? 0) != 0)
                {
                    stringBuilder.AppendFormat(":{0:00}", this.Minute ?? 0);

                    // Second
                    if ((this.Second ?? 0) != 0)
                    {
                        stringBuilder.AppendFormat(":{0:00}", this.Second ?? 0);
                    }
                }

                // Hour period
                if (this.HourPeriod.HasValue)
                {
                    if (this.IsMidnight)
                    {
                        stringBuilder.Append(" midnight");
                    }
                    else if (this.IsNoon)
                    {
                        stringBuilder.Append(" noon");
                    }
                    else
                    {
                        stringBuilder.Append(this.HourPeriod == Parsing.HourPeriod.Am ? " am" : " pm");
                    }
                }
                else if ((this.Minute ?? 0) == 0 && (this.Second ?? 0) == 0)
                {
                    stringBuilder.Append(" o'clock");
                }

                return stringBuilder.ToString();
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Parses <see cref="NormalTimeToken"/> strings.
        /// </summary>
        public new class Parser : TimeToken.Parser
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
                        (?<period>
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
                        (?<period>
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
            /// Returns a set of regular expressions supported by this parser.
            /// </summary>
            /// <param name="provider">An <see cref="IFormatProvider"/>.</param>
            /// <returns>A set of regular expressions supported by this parser.</returns>
            public override IEnumerable<string> GetPatterns(IFormatProvider provider)
            {
                return new[]
                {
                    TimeWithSeparatorsPattern,
                    TimeWithoutSeparatorsPattern
                };
            }

            /// <summary>
            /// Parses a <see cref="Match"/> into a <see cref="TimeToken"/>.
            /// </summary>
            /// <param name="match">A <see cref="Match"/> representation of a <see cref="TimeToken"/>.</param>
            /// <param name="provider">An <see cref="IFormatProvider"/>.</param>
            /// <returns>The <see cref="TimeToken"/> parsed from the <see cref="Match"/>.</returns>
            /// <exception cref="ArgumentNullException">If <paramref name="match"/> or <paramref name="provider"/> is
            /// <c>null</c>.</exception>
            /// <exception cref="FormatException">If the <paramref name="match"/> is not a supported representation of
            /// a <see cref="TimeToken"/>.</exception>
            protected override TimeToken ParseInternal(Match match, IFormatProvider provider)
            {
                NormalTimeToken timeToken = new NormalTimeToken();

                // Parse hour period
                if (match.Groups["period"].Success)
                {
                    if (match.Groups["period"].Value.StartsWith("a", StringComparison.InvariantCultureIgnoreCase))
                    {
                        timeToken.HourPeriod = Parsing.HourPeriod.Am;
                    }
                    else if (match.Groups["period"].Value.StartsWith("p", StringComparison.InvariantCultureIgnoreCase))
                    {
                        timeToken.HourPeriod = Parsing.HourPeriod.Pm;
                    }
                    else if (match.Groups["period"].Value.StartsWith("h", StringComparison.InvariantCultureIgnoreCase))
                    {
                        timeToken.HourPeriod = Parsing.HourPeriod.Military;
                    }
                }

                // Parse hour
                if (match.Groups["hour"].Success)
                {
                    timeToken.Hour = int.Parse(match.Groups["hour"].Value, provider);
                }

                // Parse minute
                if (match.Groups["minute"].Success)
                {
                    timeToken.Minute = int.Parse(match.Groups["minute"].Value, provider);
                }

                // Parse second
                if (match.Groups["second"].Success)
                {
                    timeToken.Second = int.Parse(match.Groups["second"].Value, provider);
                }

                return timeToken;
            }
        }
    }
}
