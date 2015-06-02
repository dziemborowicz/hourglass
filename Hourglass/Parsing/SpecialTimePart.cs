// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SpecialTimePart.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass.Parsing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Represents a special time of day.
    /// </summary>
    public enum SpecialTime
    {
        /// <summary>
        /// Represents 12 noon.
        /// </summary>
        Midday,

        /// <summary>
        /// Represents 12 midnight.
        /// </summary>
        Midnight
    }

    /// <summary>
    /// Represents a special time of day.
    /// </summary>
    public class SpecialTimePart : TimePart
    {
        /// <summary>
        /// A list of supported special times.
        /// </summary>
        private static readonly SpecialTimeInfo[] SpecialTimes =
        {
            new SpecialTimeInfo(
                SpecialTime.Midday,
                "12 noon",
                12 /* hour */,
                0 /* minute */,
                0 /* second */,
                @"(12([.:]00([.:]00)?)?\s*)?(noon|mid(-?d)?ay)"),

            new SpecialTimeInfo(
                SpecialTime.Midnight,
                "12 midnight",
                0 /* hour */,
                0 /* minute */,
                0 /* second */,
                @"(12([.:]00([.:]00)?)?\s*)?mid-?night")
        };

        /// <summary>
        /// Gets or sets the <see cref="SpecialTime"/> represented by this part.
        /// </summary>
        public SpecialTime SpecialTime { get; set; }

        /// <summary>
        /// Gets a value indicating whether the part is valid.
        /// </summary>
        public override bool IsValid
        {
            get { return this.GetSpecialTimeInfo() != null; }
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
            
            SpecialTimeInfo specialTimeInfo = this.GetSpecialTimeInfo();

            return new DateTime(
                datePart.Year,
                datePart.Month,
                datePart.Day,
                specialTimeInfo.Hour,
                specialTimeInfo.Minute,
                specialTimeInfo.Second);
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            SpecialTimeInfo specialTimeInfo = this.GetSpecialTimeInfo();

            if (specialTimeInfo == null)
            {
                return string.Empty;
            }

            return specialTimeInfo.Name;
        }

        /// <summary>
        /// Returns the <see cref="SpecialTimeInfo"/> object for this part.
        /// </summary>
        /// <returns>The <see cref="SpecialTimeInfo"/> object for this part.</returns>
        private SpecialTimeInfo GetSpecialTimeInfo()
        {
            return SpecialTimes.FirstOrDefault(e => e.SpecialTime == this.SpecialTime);
        }

        /// <summary>
        /// Parses <see cref="SpecialTimePart"/>s from <see cref="string"/>s.
        /// </summary>
        public new class Parser : TimePart.Parser
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
            /// Returns the regular expressions supported by this <see cref="Parser"/>.
            /// </summary>
            /// <param name="provider">An <see cref="IFormatProvider"/> to use when parsing.</param>
            /// <returns>The regular expressions supported by this <see cref="Parser"/>.</returns>
            public override IEnumerable<string> GetPatterns(IFormatProvider provider)
            {
                return SpecialTimes.Select(e => e.Pattern);
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
                SpecialTimeInfo specialTimeInfo = GetSpecialTimeInfoForMatch(match);

                if (specialTimeInfo == null)
                {
                    throw new FormatException();
                }

                return new SpecialTimePart { SpecialTime = specialTimeInfo.SpecialTime };
            }

            /// <summary>
            /// Returns the <see cref="SpecialTimeInfo"/> object for A <see cref="Match"/>.
            /// </summary>
            /// <param name="match">A <see cref="Match"/>.</param>
            /// <returns>The <see cref="SpecialTimeInfo"/> object for A <see cref="Match"/>.</returns>
            private static SpecialTimeInfo GetSpecialTimeInfoForMatch(Match match)
            {
                return SpecialTimes.FirstOrDefault(e => match.Groups[e.MatchGroup].Success);
            }
        }

        /// <summary>
        /// Contains data related to a <see cref="SpecialTime"/>.
        /// </summary>
        private class SpecialTimeInfo
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="SpecialTimeInfo"/> class.
            /// </summary>
            /// <param name="specialTime">The <see cref="SpecialTime"/>.</param>
            /// <param name="name">The friendly name for the <see cref="SpecialTime"/>.</param>
            /// <param name="hour">The hour for the <see cref="SpecialTime"/>.</param>
            /// <param name="minute">The minute for the <see cref="SpecialTime"/>.</param>
            /// <param name="second">The second for the <see cref="SpecialTime"/>.</param>
            /// <param name="pattern">The regular expression for the <see cref="SpecialTime"/>.</param>
            public SpecialTimeInfo(SpecialTime specialTime, string name, int hour, int minute, int second, string pattern)
            {
                this.SpecialTime = specialTime;
                this.Name = name;

                this.Hour = hour;
                this.Minute = minute;
                this.Second = second;

                this.Pattern = string.Format(@"(?<{0}>{1})", specialTime, pattern);
                this.MatchGroup = specialTime.ToString();
            }

            /// <summary>
            /// Gets the <see cref="SpecialTime"/>.
            /// </summary>
            public SpecialTime SpecialTime { get; private set; }

            /// <summary>
            /// Gets the friendly name for the <see cref="SpecialTime"/>.
            /// </summary>
            public string Name { get; private set; }

            /// <summary>
            /// Gets the hour for the <see cref="SpecialTime"/>.
            /// </summary>
            public int Hour { get; private set; }

            /// <summary>
            /// Gets the minute for the <see cref="SpecialTime"/>.
            /// </summary>
            public int Minute { get; private set; }

            /// <summary>
            /// Gets the second for the <see cref="SpecialTime"/>.
            /// </summary>
            public int Second { get; private set; }

            /// <summary>
            /// Gets the regular expression for the <see cref="SpecialTime"/>.
            /// </summary>
            public string Pattern { get; private set; }

            /// <summary>
            /// Gets the name of the regular expression match group that identifies the <see cref="SpecialTime"/> in
            /// the match.
            /// </summary>
            public string MatchGroup { get; private set; }
        }
    }
}
