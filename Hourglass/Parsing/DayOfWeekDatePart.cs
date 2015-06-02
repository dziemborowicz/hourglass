// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DayOfWeekDatePart.cs" company="Chris Dziemborowicz">
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
    /// Represents the relation between the day of week and the reference date.
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
    /// Represents a day of week.
    /// </summary>
    public class DayOfWeekDatePart : DatePart
    {
        /// <summary>
        /// Gets or sets the day of week represented by this part.
        /// </summary>
        public DayOfWeek? DayOfWeek { get; set; }

        /// <summary>
        /// Gets or sets the relation between the day of week represented by this part and the reference date.
        /// </summary>
        public DayOfWeekRelation? DayOfWeekRelation { get; set; }

        /// <summary>
        /// Gets a value indicating whether the part is valid.
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
        /// Returns a concrete date represented by this part on or after the reference date.
        /// </summary>
        /// <param name="referenceDate">A reference date and time.</param>
        /// <param name="tryExcludeReferenceDate">A value indicating whether a date after (rather than on or after) the
        /// reference date should be returned if possible.</param>
        /// <returns>A concrete date represented by this part.</returns>
        public override DateTime ToDateTime(DateTime referenceDate, bool tryExcludeReferenceDate)
        {
            this.ThrowIfNotValid();

            DateTime date = referenceDate.AddDays(1); // TODO Do we really want this?

            // Find the next date with the matching weekday
            DayOfWeek dayOfWeek = this.DayOfWeek ?? System.DayOfWeek.Sunday;
            while (date.DayOfWeek != dayOfWeek)
            {
                date = date.AddDays(1);
            }

            // Advance the date by a week if necessary
            DayOfWeekRelation dayOfWeekRelation = this.DayOfWeekRelation ?? Parsing.DayOfWeekRelation.Next;
            if (dayOfWeekRelation == Parsing.DayOfWeekRelation.AfterNext ||
                (dayOfWeekRelation == Parsing.DayOfWeekRelation.NextWeek && dayOfWeek > referenceDate.DayOfWeek))
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

        /// <summary>
        /// Parses <see cref="DayOfWeekDatePart"/>s from <see cref="string"/>s.
        /// </summary>
        public new class Parser : DatePart.Parser
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
                    \s*(?<nextweek>next\s*week)
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
                    DaysOfWeekNextPattern,
                    DaysOfWeekAfterNextPattern,
                    DaysOfWeekNextWeekPattern
                };
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
                DayOfWeekDatePart datePart = new DayOfWeekDatePart();

                // Parse day of week
                if (match.Groups["weekday"].Success)
                {
                    datePart.DayOfWeek = ParseDayOfWeek(match.Groups["weekday"].Value);
                }

                // Parse day of week relation
                if (match.Groups["afternext"].Success)
                {
                    datePart.DayOfWeekRelation = Parsing.DayOfWeekRelation.AfterNext;
                }
                else if (match.Groups["nextweek"].Success)
                {
                    datePart.DayOfWeekRelation = Parsing.DayOfWeekRelation.NextWeek;
                }
                else
                {
                    datePart.DayOfWeekRelation = Parsing.DayOfWeekRelation.Next;
                }

                return datePart;
            }

            /// <summary>
            /// Returns the <see cref="DayOfWeek"/> for a given <see cref="string"/> representation of a day of the
            /// week.
            /// </summary>
            /// <remarks>
            /// This method only checks the first three characters of the <see cref="string"/> against the <see
            /// cref="DayOfWeek"/> <c>enum</c>.
            /// </remarks>
            /// <param name="str">A <see cref="string"/> representation of a day of the week.</param>
            /// <returns>The <see cref="DayOfWeek"/> for the <see cref="string"/> representation of the day of the
            /// week.</returns>
            /// <exception cref="FormatException">If <paramref name="str"/> is not a valid <see cref="string"/>
            /// representation of a day of the week.</exception>
            /// <seealso cref="DayOfWeek"/>
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
