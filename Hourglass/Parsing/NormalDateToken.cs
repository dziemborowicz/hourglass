// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NormalDateToken.cs" company="Chris Dziemborowicz">
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

    /// <summary>
    /// Represents the date part of an instant in time specified as year, month, and day.
    /// </summary>
    public class NormalDateToken : DateToken
    {
        /// <summary>
        /// Gets or sets the year.
        /// </summary>
        public int? Year { get; set; }

        /// <summary>
        /// Gets or sets the month.
        /// </summary>
        public int? Month { get; set; }

        /// <summary>
        /// Gets or sets the day.
        /// </summary>
        public int? Day { get; set; }

        /// <summary>
        /// Gets a value indicating whether the token is valid.
        /// </summary>
        public override bool IsValid
        {
            get
            {
                return DateTimeExtensions.IsValid(this.Year, this.Month, this.Day)
                    && (this.Year.HasValue || this.Month.HasValue || this.Day.HasValue)
                    && !(this.Year.HasValue && !this.Month.HasValue && this.Day.HasValue);
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

            DateTime date;

            int year = this.Year ?? minDate.Year;
            int month = this.Month ?? (!this.Year.HasValue ? minDate.Month : 1);
            int day = this.Day ?? (!this.Year.HasValue && !this.Month.HasValue ? minDate.Day : 1);

            while (!DateTimeExtensions.TryToDateTime(year, month, day, out date)
                || date < minDate.Date
                || (date == minDate.Date && !inclusive))
            {
                // Try the next month if we only have a day
                if (!this.Month.HasValue && !this.Year.HasValue)
                {
                    DateTimeExtensions.IncrementMonth(ref year, ref month);
                    continue;
                }

                // Try the next year if we only have a month or a day and a month
                if (!this.Year.HasValue)
                {
                    year++;
                    continue;
                }

                // Nothing else to try
                break;
            }

            return date;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return this.ToString(CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <param name="provider">An <see cref="IFormatProvider"/> to use.</param>
        /// <returns>A string that represents the current object.</returns>
        public string ToString(IFormatProvider provider)
        {
            try
            {
                this.ThrowIfNotValid();

                // Day only
                if (this.Day.HasValue && !this.Month.HasValue && !this.Year.HasValue)
                {
                    return DateTimeExtensions.ToOrdinalDayString(this.Day.Value);
                }

                // Day and month
                if (this.Day.HasValue && this.Month.HasValue && !this.Year.HasValue)
                {
                    return string.Format(
                        provider,
                        provider.IsMonthFirst() ? "{1} {0}" : "{0} {1}",
                        this.Day.Value,
                        DateTimeExtensions.ToMonthString(this.Month.Value));
                }

                // Day, month, and year
                if (this.Day.HasValue && this.Month.HasValue && this.Year.HasValue)
                {
                    return string.Format(
                        provider,
                        provider.IsMonthFirst() ? "{1} {0}, {2}" : "{0} {1} {2}",
                        this.Day.Value,
                        DateTimeExtensions.ToMonthString(this.Month.Value),
                        this.Year.Value);
                }

                // Month only
                if (!this.Day.HasValue && this.Month.HasValue && !this.Year.HasValue)
                {
                    return DateTimeExtensions.ToMonthString(this.Month.Value);
                }

                // Month and year
                if (!this.Day.HasValue && this.Month.HasValue && this.Year.HasValue)
                {
                    return string.Format(
                        provider,
                        "{0} {1}",
                        DateTimeExtensions.ToMonthString(this.Month.Value),
                        this.Year.Value);
                }

                // Year
                if (!this.Day.HasValue && !this.Month.HasValue && this.Year.HasValue)
                {
                    return this.Year.Value.ToString(provider);
                }

                // Unsupported
                return string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Parses <see cref="NormalDateToken"/> strings.
        /// </summary>
        public new class Parser : DateToken.Parser
        {
            /// <summary>
            /// Singleton instance of the <see cref="Parser"/> class.
            /// </summary>
            public static readonly Parser Instance = new Parser();

            /// <summary>
            /// A regular expression that matches spelled dates with day first and optional year (e.g., "14 February",
            /// "14 February 2003").
            /// </summary>
            private const string SpelledDateWithDayFirstPattern =
                @"  (?<day>\d\d?)
                    (\s*(st|nd|rd|th))?
                    (\s*of)?
                    \s*(?<month>(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec)[a-z]*)
                    (
                        \s*,?\s*
                        (?<year>(\d\d)?\d\d)
                    )?
                ";

            /// <summary>
            /// A regular expression that matches spelled dates with month first and optional year (e.g., "February 14",
            /// "February 14, 2003").
            /// </summary>
            private const string SpelledDateWithMonthFirstPattern =
                @"  (?<month>(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec)[a-z]*)
                    \s*(?<day>\d\d?)
                    (\s*(st|nd|rd|th))?
                    (
                        \s*[\s,]\s*
                        (?<year>(\d\d)?\d\d)
                    )?
                ";

            /// <summary>
            /// A regular expression that matches numerical dates with day first (e.g., "14/02", "14/02/2003").
            /// </summary>
            private const string NumericalDateWithDayFirstPattern =
                @"  (?<day>\d\d?)
                    [.\-/]
                    (?<month>\d\d?)
                    (
                        [.\-/]
                        (?<year>(\d\d)?\d\d)
                    )?
                ";

            /// <summary>
            /// A regular expression that matches numerical dates with month first (e.g., "02/14", "02/14/2003").
            /// </summary>
            private const string NumericalDateWithMonthFirstPattern =
                @"  (?<month>\d\d?)
                    [.\-/]
                    (?<day>\d\d?)
                    (
                        [.\-/]
                        (?<year>(\d\d)?\d\d)
                    )?
                ";

            /// <summary>
            /// A regular expression that matches numerical dates with year first (e.g., "03.02.14", "2003.02.14").
            /// </summary>
            private const string NumericalDateWithYearFirstPattern =
                @"  (
                        (?<year>(\d\d)?\d\d)
                        [.\-/]
                    )?
                    (?<month>\d\d?)
                    [.\-/]
                    (?<day>\d\d?)
                ";

            /// <summary>
            /// A regular expression that matches days by themselves.
            /// </summary>
            private const string DayOnlyPattern =
                @"  (the\s*)?
                    (?<day>\d\d?)
                    (\s*(st|nd|rd|th))
                ";

            /// <summary>
            /// A regular expression that matches spelled months with optional years (e.g., "Jan", "Feb 2015").
            /// </summary>
            private const string SpelledMonthAndOptionalYearPattern =
                @"  (?<month>(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec)[a-z]*)
                    (
                        \s*,?\s*
                        (?<year>\d\d\d\d)
                    )?
                ";

            /// <summary>
            /// A regular expression that matches months with years (e.g., "2/2015", "02/2015").
            /// </summary>
            private const string NumericalMonthAndYearPattern =
                @"  (?<month>\d\d?)
                    [.\-/]
                    (?<year>\d\d\d\d)
                    |
                    (?<year>\d\d\d\d)
                    [.\-/]
                    (?<month>\d\d?)
                ";

            /// <summary>
            /// A set of regular expressions for matching dates. In the case of ambiguity, these patterns favor
            /// interpreting matching strings specifying a day, month, and year in that order.
            /// </summary>
            /// <seealso cref="PatternsWithMonthFirst"/>
            /// <seealso cref="PatternsWithYearFirst"/>
            private static readonly string[] PatternsWithDayFirst =
            {
                SpelledDateWithDayFirstPattern,
                SpelledDateWithMonthFirstPattern,
                NumericalDateWithDayFirstPattern,
                NumericalDateWithMonthFirstPattern,
                NumericalDateWithYearFirstPattern,
                DayOnlyPattern,
                SpelledMonthAndOptionalYearPattern,
                NumericalMonthAndYearPattern
            };

            /// <summary>
            /// A set of regular expressions for matching dates. In the case of ambiguity, these patterns favor
            /// interpreting matching strings specifying a month, day, and year in that order.
            /// </summary>
            /// <seealso cref="PatternsWithDayFirst"/>
            /// <seealso cref="PatternsWithYearFirst"/>
            private static readonly string[] PatternsWithMonthFirst =
            {
                SpelledDateWithMonthFirstPattern,
                SpelledDateWithDayFirstPattern,
                NumericalDateWithMonthFirstPattern,
                NumericalDateWithDayFirstPattern,
                NumericalDateWithYearFirstPattern,
                DayOnlyPattern,
                SpelledMonthAndOptionalYearPattern,
                NumericalMonthAndYearPattern
            };

            /// <summary>
            /// A set of regular expressions for matching dates. In the case of ambiguity, these patterns favor
            /// interpreting matching strings specifying a year, month, and day in that order.
            /// </summary>
            /// <seealso cref="PatternsWithDayFirst"/>
            /// <seealso cref="PatternsWithMonthFirst"/>
            private static readonly string[] PatternsWithYearFirst =
            {
                SpelledDateWithDayFirstPattern,
                SpelledDateWithMonthFirstPattern,
                NumericalDateWithYearFirstPattern,
                NumericalDateWithDayFirstPattern,
                NumericalDateWithMonthFirstPattern,
                DayOnlyPattern,
                SpelledMonthAndOptionalYearPattern,
                NumericalMonthAndYearPattern
            };

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
                if (provider.IsMonthFirst())
                {
                    return PatternsWithMonthFirst;
                }

                if (provider.IsYearFirst())
                {
                    return PatternsWithYearFirst;
                }

                return PatternsWithDayFirst;
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
                NormalDateToken dateToken = new NormalDateToken();

                // Parse day
                if (match.Groups["day"].Success)
                {
                    dateToken.Day = int.Parse(match.Groups["day"].Value, provider);
                }

                // Parse month
                if (match.Groups["month"].Success)
                {
                    if (Regex.IsMatch(match.Groups["month"].Value, @"^\d+$", TimerStartToken.Parser.RegexOptions))
                    {
                        dateToken.Month = int.Parse(match.Groups["month"].Value, provider);
                    }
                    else
                    {
                        dateToken.Month = DateTimeExtensions.ParseMonth(match.Groups["month"].Value);
                    }
                }

                // Parse year
                if (match.Groups["year"].Success)
                {
                    dateToken.Year = int.Parse(match.Groups["year"].Value, provider);

                    if (dateToken.Year.Value < 100)
                    {
                        dateToken.Year += 2000;
                    }
                }

                return dateToken;
            }
        }
    }
}
