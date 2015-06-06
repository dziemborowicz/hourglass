// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DayOfWeekDateToken.cs" company="Chris Dziemborowicz">
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
    /// Represents the relation between the day of week and a date.
    /// </summary>
    public enum DayOfWeekRelation
    {
        /// <summary>
        /// The next date that is the specified day of the week.
        /// </summary>
        Next,

        /// <summary>
        /// The date that is one week after the next date that is the specified day of the week.
        /// </summary>
        AfterNext,

        /// <summary>
        /// A date next week that is the specified day of the week
        /// </summary>
        NextWeek
    }

    /// <summary>
    /// Represents the date part of an instant in time specified as a day of the week.
    /// </summary>
    public class DayOfWeekDateToken : DateToken
    {
        /// <summary>
        /// Gets or sets the day of week.
        /// </summary>
        public DayOfWeek? DayOfWeek { get; set; }

        /// <summary>
        /// Gets or sets the relation between the day of week and date.
        /// </summary>
        public DayOfWeekRelation? DayOfWeekRelation { get; set; }

        /// <summary>
        /// Gets a value indicating whether the token is valid.
        /// </summary>
        public override bool IsValid
        {
            get
            {
                return this.DayOfWeek.HasValue
                    && Enum.IsDefined(typeof(DayOfWeek), this.DayOfWeek.Value)
                    && this.DayOfWeekRelation.HasValue
                    && Enum.IsDefined(typeof(DayOfWeekRelation), this.DayOfWeekRelation.Value);
            }
        }

        /// <summary>
        /// Returns the next date after <paramref name="minDate"/> that is represented by this token.
        /// </summary>
        /// <remarks>
        /// This method may return a date that is before <paramref name="minDate"/> if there is no date after <paramref
        /// name="minDate"/> that is represented by this token.
        /// </remarks>
        /// <param name="minDate">The minimum date to return. The time part is ignored.</param>
        /// <param name="inclusive">A value indicating whether the returned date should be on or after rather than
        /// strictly after <paramref name="minDate"/>.</param>
        /// <returns>The next date after <paramref name="minDate"/> that is represented by this token.</returns>
        /// <exception cref="InvalidOperationException">If this token is not valid.</exception>
        public override DateTime ToDateTime(DateTime minDate, bool inclusive)
        {
            this.ThrowIfNotValid();

            DateTime date = minDate.Date.AddDays(1);

            // Find the next date with the matching weekday
            DayOfWeek dayOfWeek = this.DayOfWeek ?? System.DayOfWeek.Sunday;
            while (date.DayOfWeek != dayOfWeek)
            {
                date = date.AddDays(1);
            }

            // Advance the date by a week if necessary
            DayOfWeekRelation dayOfWeekRelation = this.DayOfWeekRelation ?? Parsing.DayOfWeekRelation.Next;
            if (dayOfWeekRelation == Parsing.DayOfWeekRelation.AfterNext ||
                (dayOfWeekRelation == Parsing.DayOfWeekRelation.NextWeek && dayOfWeek > minDate.DayOfWeek))
            {
                date = date.AddDays(7);
            }

            return date;
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

                // Day of week
                stringBuilder.Append(this.DayOfWeek);

                // Day of week relation
                if (this.DayOfWeekRelation == Parsing.DayOfWeekRelation.AfterNext)
                {
                    stringBuilder.Append(" after next");
                }
                else if (this.DayOfWeekRelation == Parsing.DayOfWeekRelation.NextWeek)
                {
                    stringBuilder.Append(" next week");
                }

                return stringBuilder.ToString();
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Parses <see cref="DayOfWeekDateToken"/> strings.
        /// </summary>
        public new class Parser : DateToken.Parser
        {
            /// <summary>
            /// Singleton instance of the <see cref="Parser"/> class.
            /// </summary>
            public static readonly Parser Instance = new Parser();

            /// <summary>
            /// A regular expression that matches days of the week (e.g., "Sunday", "this Sunday", "next Sunday").
            /// </summary>
            private const string DaysOfWeekNextPattern =
                @"  ((this|next)\s*)?
                    (?<weekday>Sun|Mon|Tue|Wed|Thu|Fri|Sat)[a-z]*
                ";

            /// <summary>
            /// A regular expression that matches days of the week after next (e.g., "Sunday next", "Sunday after next").
            /// </summary>
            private const string DaysOfWeekAfterNextPattern =
                @"  (?<weekday>Sun|Mon|Tue|Wed|Thu|Fri|Sat)[a-z]*
                    (\s*after)?
                    \s*(?<afternext>next)
                ";

            /// <summary>
            /// A regular expression that matches days of the week next week (e.g., "Sunday next week").
            /// </summary>
            private const string DaysOfWeekNextWeekPattern =
                @"  (?<weekday>Sun|Mon|Tue|Wed|Thu|Fri|Sat)[a-z]*
                    \s*(?<nextweek>next\s*w(ee)?k)
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
                    DaysOfWeekNextPattern,
                    DaysOfWeekAfterNextPattern,
                    DaysOfWeekNextWeekPattern
                };
            }

            /// <summary>
            /// Parses a <see cref="Match"/> into a <see cref="DateToken"/>.
            /// </summary>
            /// <param name="match">A <see cref="Match"/> representation of a <see cref="DateToken"/>.</param>
            /// <param name="provider">An <see cref="IFormatProvider"/>.</param>
            /// <returns>The <see cref="DateToken"/> parsed from the <see cref="Match"/>.</returns>
            /// <exception cref="ArgumentNullException">If <paramref name="match"/> or <paramref name="provider"/> is
            /// <c>null</c>.</exception>
            /// <exception cref="FormatException">If the <paramref name="match"/> is not a supported representation of
            /// a <see cref="DateToken"/>.</exception>
            protected override DateToken ParseInternal(Match match, IFormatProvider provider)
            {
                DayOfWeekDateToken dateToken = new DayOfWeekDateToken();

                // Parse day of week
                if (match.Groups["weekday"].Success)
                {
                    dateToken.DayOfWeek = ParseDayOfWeek(match.Groups["weekday"].Value);
                }

                // Parse day of week relation
                if (match.Groups["afternext"].Success)
                {
                    dateToken.DayOfWeekRelation = Parsing.DayOfWeekRelation.AfterNext;
                }
                else if (match.Groups["nextweek"].Success)
                {
                    dateToken.DayOfWeekRelation = Parsing.DayOfWeekRelation.NextWeek;
                }
                else
                {
                    dateToken.DayOfWeekRelation = Parsing.DayOfWeekRelation.Next;
                }

                return dateToken;
            }

            /// <summary>
            /// Parses a string into a <see cref="DayOfWeek"/>.
            /// </summary>
            /// <remarks>
            /// This method only checks the first three characters of the string against the <see cref="DayOfWeek"/>
            /// <c>enum</c>.
            /// </remarks>
            /// <param name="str">A string representation of a <see cref="DayOfWeek"/>.</param>
            /// <returns>The <see cref="DayOfWeek"/> parsed from the string.</returns>
            /// <exception cref="FormatException">If <paramref name="str"/> is not a supported representation of a day
            /// of the week.</exception>
            private static DayOfWeek ParseDayOfWeek(string str)
            {
                foreach (DayOfWeek day in Enum.GetValues(typeof(DayOfWeek)))
                {
                    if (str.StartsWith(day.ToString().Substring(0, 3), StringComparison.InvariantCultureIgnoreCase))
                    {
                        return day;
                    }
                }

                throw new FormatException();
            }
        }
    }
}
