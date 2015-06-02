// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SpecialDatePart.cs" company="Chris Dziemborowicz">
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
    /// Represents a special date.
    /// </summary>
    public enum SpecialDate
    {
        /// <summary>
        /// Represents Christmas Day (December 25).
        /// </summary>
        ChristmasDay,

        /// <summary>
        /// Represents New Year's Eve (December 31).
        /// </summary>
        /// <seealso cref="NewYearDatePart"/>
        NewYearsEve
    }

    /// <summary>
    /// Represents a special date.
    /// </summary>
    public class SpecialDatePart : DatePart
    {
        /// <summary>
        /// A list of supported special dates.
        /// </summary>
        private static readonly SpecialDateInfo[] SpecialDates =
        {
            new SpecialDateInfo(
                SpecialDate.ChristmasDay,
                "Christmas Day",
                12 /* month */,
                25 /* day */,
                @"((c|k)h?rist?|x)-?mass?(\s*day)?"),

            new SpecialDateInfo(
                SpecialDate.NewYearsEve,
                "New Year's Eve",
                12 /* month */,
                31 /* day */,
                @"nye?|new\s*year('?s)?(\s*eve)?")
        };

        /// <summary>
        /// Gets or sets the <see cref="SpecialDate"/> represented by this part.
        /// </summary>
        public SpecialDate SpecialDate { get; set; }

        /// <summary>
        /// Gets a value indicating whether the part is valid.
        /// </summary>
        public override bool IsValid
        {
            get { return this.GetSpecialDateInfo() != null; }
        }

        /// <summary>
        /// Returns a concrete date represented by this part on or after the reference date.
        /// </summary>
        /// <param name="referenceDate">A reference date and time.</param>
        /// <param name="tryExcludeReferenceDate">A value indicating whether a date after (rather than on or after) the
        /// reference date should be returned if possible.</param>
        /// <returns>A concrete date represented by this part.</returns>
        public override DateTime ToDateTime(DateTime referenceDate, bool tryExcludeReferenceDate)
        {
            this.ThrowIfNotValid();

            SpecialDateInfo specialDateInfo = this.GetSpecialDateInfo();

            DateTime date = new DateTime(
                referenceDate.Year,
                specialDateInfo.Month,
                specialDateInfo.Day);

            if (date < referenceDate.Date ||
                (date == referenceDate.Date && tryExcludeReferenceDate))
            {
                date = date.AddYears(1);
            }

            return date;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            SpecialDateInfo specialDateInfo = this.GetSpecialDateInfo();

            if (specialDateInfo == null)
            {
                return string.Empty;
            }

            return specialDateInfo.Name;
        }

        /// <summary>
        /// Returns the <see cref="SpecialDateInfo"/> object for this part.
        /// </summary>
        /// <returns>The <see cref="SpecialDateInfo"/> object for this part.</returns>
        private SpecialDateInfo GetSpecialDateInfo()
        {
            return SpecialDates.FirstOrDefault(e => e.SpecialDate == this.SpecialDate);
        }

        /// <summary>
        /// Parses <see cref="SpecialDatePart"/>s from <see cref="string"/>s.
        /// </summary>
        public new class Parser : DatePart.Parser
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
                return SpecialDates.Select(e => e.Pattern);
            }

            /// <summary>
            /// Parses a <see cref="DatePart"/> from a regular expression <see cref="Match"/>.
            /// </summary>
            /// <param name="match">A <see cref="Match"/> corresponding to a pattern returned by <see
            /// cref="GetPatterns"/>.</param>
            /// <param name="provider">An <see cref="IFormatProvider"/> to use when parsing.</param>
            /// <returns>aA<see cref="DatePart"/> from the regular expression <see cref="Match"/>.</returns>
            protected override DatePart ParseInternal(Match match, IFormatProvider provider)
            {
                SpecialDateInfo specialDateInfo = GetSpecialDateInfoForMatch(match);

                if (specialDateInfo == null)
                {
                    throw new FormatException();
                }

                return new SpecialDatePart { SpecialDate = specialDateInfo.SpecialDate };
            }

            /// <summary>
            /// Returns the <see cref="SpecialDateInfo"/> object for A <see cref="Match"/>.
            /// </summary>
            /// <param name="match">A <see cref="Match"/>.</param>
            /// <returns>The <see cref="SpecialDateInfo"/> object for A <see cref="Match"/>.</returns>
            private static SpecialDateInfo GetSpecialDateInfoForMatch(Match match)
            {
                return SpecialDates.FirstOrDefault(e => match.Groups[e.MatchGroup].Success);
            }
        }

        /// <summary>
        /// Contains data related to a <see cref="SpecialDate"/>.
        /// </summary>
        private class SpecialDateInfo
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="SpecialDateInfo"/> class.
            /// </summary>
            /// <param name="specialDate">The <see cref="SpecialDate"/>.</param>
            /// <param name="name">The friendly name for the <see cref="SpecialDate"/>.</param>
            /// <param name="month">The month for the <see cref="SpecialDate"/>.</param>
            /// <param name="day">The day for the <see cref="SpecialDate"/>.</param>
            /// <param name="pattern">The regular expression for the <see cref="SpecialDate"/>.</param>
            public SpecialDateInfo(SpecialDate specialDate, string name, int month, int day, string pattern)
            {
                this.SpecialDate = specialDate;
                this.Name = name;

                this.Month = month;
                this.Day = day;

                this.Pattern = string.Format(@"(?<{0}>{1})", specialDate, pattern);
                this.MatchGroup = specialDate.ToString();
            }

            /// <summary>
            /// Gets the <see cref="SpecialDate"/>.
            /// </summary>
            public SpecialDate SpecialDate { get; private set; }

            /// <summary>
            /// Gets the friendly name for the <see cref="SpecialDate"/>.
            /// </summary>
            public string Name { get; private set; }

            /// <summary>
            /// Gets the month for the <see cref="SpecialDate"/>.
            /// </summary>
            public int Month { get; private set; }

            /// <summary>
            /// Gets the year for the <see cref="SpecialDate"/>.
            /// </summary>
            public int Day { get; private set; }

            /// <summary>
            /// Gets the regular expression for the <see cref="SpecialDate"/>.
            /// </summary>
            public string Pattern { get; private set; }

            /// <summary>
            /// Gets the name of the regular expression match group that identifies the <see cref="SpecialDate"/> in
            /// the match.
            /// </summary>
            public string MatchGroup { get; private set; }
        }
    }
}
