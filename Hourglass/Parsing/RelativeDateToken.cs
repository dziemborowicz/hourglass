// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RelativeDateToken.cs" company="Chris Dziemborowicz">
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
    /// Represents a relative date.
    /// </summary>
    public enum RelativeDate
    {
        /// <summary>
        /// Represents today.
        /// </summary>
        Today,

        /// <summary>
        /// Represents tomorrow.
        /// </summary>
        Tomorrow
    }

    /// <summary>
    /// Represents a relative date.
    /// </summary>
    public class RelativeDateToken : DateToken
    {
        /// <summary>
        /// A list of supported relative dates.
        /// </summary>
        private static readonly RelativeDateDefinition[] RelativeDates =
        {
            new RelativeDateDefinition(
                RelativeDate.Today,
                "today",
                0 /* yearDelta */,
                0 /* monthDelta */,
                0 /* dayDelta */,
                @"todd?ay"),
                
            new RelativeDateDefinition(
                RelativeDate.Tomorrow,
                "tomorrow",
                0 /* yearDelta */,
                0 /* monthDelta */,
                1 /* dayDelta */,
                @"tomm?orr?ow")
        };

        /// <summary>
        /// Gets or sets the <see cref="RelativeDate"/> represented by this token.
        /// </summary>
        public RelativeDate RelativeDate { get; set; }

        /// <summary>
        /// Gets a value indicating whether the token is valid.
        /// </summary>
        public override bool IsValid
        {
            get { return this.GetRelativeDateDefinition() != null; }
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

            RelativeDateDefinition relativeDateDefinition = this.GetRelativeDateDefinition();

            DateTime date = minDate.Date;
            date = date.AddDays(relativeDateDefinition.DayDelta);
            date = date.AddMonths(relativeDateDefinition.MonthDelta);
            date = date.AddYears(relativeDateDefinition.YearDelta);
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

                RelativeDateDefinition relativeDateDefinition = this.GetRelativeDateDefinition();
                return relativeDateDefinition.Name;
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Returns the <see cref="RelativeDateDefinition"/> object for a <see cref="Match"/>.
        /// </summary>
        /// <param name="match">A <see cref="Match"/>.</param>
        /// <returns>The <see cref="RelativeDateDefinition"/> object for a <see cref="Match"/>.</returns>
        private static RelativeDateDefinition GetRelativeDateDefinitionForMatch(Match match)
        {
            return RelativeDates.FirstOrDefault(e => match.Groups[e.MatchGroup].Success);
        }

        /// <summary>
        /// Returns the <see cref="RelativeDateDefinition"/> object for this part.
        /// </summary>
        /// <returns>The <see cref="RelativeDateDefinition"/> object for this part.</returns>
        private RelativeDateDefinition GetRelativeDateDefinition()
        {
            return RelativeDates.FirstOrDefault(e => e.RelativeDate == this.RelativeDate);
        }

        /// <summary>
        /// Parses <see cref="RelativeDateToken"/> strings.
        /// </summary>
        public new class Parser : DateToken.Parser
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
            /// Returns a set of regular expressions supported by this parser.
            /// </summary>
            /// <param name="provider">An <see cref="IFormatProvider"/>.</param>
            /// <returns>A set of regular expressions supported by this parser.</returns>
            public override IEnumerable<string> GetPatterns(IFormatProvider provider)
            {
                return RelativeDates.Select(e => e.Pattern);
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
                RelativeDateDefinition relativeDateDefinition = GetRelativeDateDefinitionForMatch(match);

                if (relativeDateDefinition == null)
                {
                    throw new FormatException();
                }

                return new RelativeDateToken { RelativeDate = relativeDateDefinition.RelativeDate };
            }
        }

        /// <summary>
        /// Defines a relative date.
        /// </summary>
        private class RelativeDateDefinition
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="RelativeDateDefinition"/> class.
            /// </summary>
            /// <param name="relativeDate">The <see cref="relativeDate"/>.</param>
            /// <param name="name">The friendly name for the relative date.</param>
            /// <param name="yearDelta">The year delta.</param>
            /// <param name="monthDelta">The month delta.</param>
            /// <param name="dayDelta">The day delta.</param>
            /// <param name="pattern">A regular expression that matches the relative date.</param>
            public RelativeDateDefinition(RelativeDate relativeDate, string name, int yearDelta, int monthDelta, int dayDelta, string pattern)
            {
                this.RelativeDate = relativeDate;
                this.Name = name;

                this.YearDelta = yearDelta;
                this.MonthDelta = monthDelta;
                this.DayDelta = dayDelta;

                this.Pattern = string.Format(@"(?<{0}>{1})", relativeDate, pattern);
                this.MatchGroup = relativeDate.ToString();
            }

            /// <summary>
            /// Gets the <see cref="RelativeDate"/>.
            /// </summary>
            public RelativeDate RelativeDate { get; private set; }

            /// <summary>
            /// Gets the friendly name for the relative date.
            /// </summary>
            public string Name { get; private set; }

            /// <summary>
            /// Gets the year delta.
            /// </summary>
            public int YearDelta { get; private set; }

            /// <summary>
            /// Gets the month delta.
            /// </summary>
            public int MonthDelta { get; private set; }

            /// <summary>
            /// Gets the day delta.
            /// </summary>
            public int DayDelta { get; private set; }

            /// <summary>
            /// Gets the regular expression that matches the relative date.
            /// </summary>
            public string Pattern { get; private set; }

            /// <summary>
            /// Gets the name of the regular expression match group that identifies the relative date in a match.
            /// </summary>
            public string MatchGroup { get; private set; }
        }
    }
}
