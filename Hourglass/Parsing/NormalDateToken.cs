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
    using Hourglass.Properties;

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
        /// <param name="provider">An <see cref="IFormatProvider"/> to use.</param>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString(IFormatProvider provider)
        {
            try
            {
                this.ThrowIfNotValid();

                // Day only
                if (this.Day.HasValue && !this.Month.HasValue && !this.Year.HasValue)
                {
                    return string.Format(
                        Resources.ResourceManager.GetEffectiveProvider(provider),
                        Resources.ResourceManager.GetString("NormalDateTokenDayOnlyFormatString", provider),
                        DateTimeExtensions.GetOrdinalDayString(this.Day.Value, provider));
                }

                // Day and month
                if (this.Day.HasValue && this.Month.HasValue && !this.Year.HasValue)
                {
                    string formatString = provider.IsMonthFirst()
                        ? Resources.ResourceManager.GetString("NormalDateTokenMonthAndDayFormatString", provider)
                        : Resources.ResourceManager.GetString("NormalDateTokenDayAndMonthFormatString", provider);
                    
                    return string.Format(
                        Resources.ResourceManager.GetEffectiveProvider(provider),
                        formatString,
                        this.Day.Value,
                        DateTimeExtensions.GetMonthString(this.Month.Value, provider));
                }

                // Day, month, and year
                if (this.Day.HasValue && this.Month.HasValue && this.Year.HasValue)
                {
                    string formatString = provider.IsMonthFirst()
                        ? Resources.ResourceManager.GetString("NormalDateTokenMonthDayAndYearFormatString", provider)
                        : Resources.ResourceManager.GetString("NormalDateTokenDayMonthAndYearFormatString", provider);

                    return string.Format(
                        Resources.ResourceManager.GetEffectiveProvider(provider),
                        formatString,
                        this.Day.Value,
                        DateTimeExtensions.GetMonthString(this.Month.Value, provider),
                        this.Year.Value);
                }

                // Month only
                if (!this.Day.HasValue && this.Month.HasValue && !this.Year.HasValue)
                {
                    return string.Format(
                        Resources.ResourceManager.GetEffectiveProvider(provider),
                        Resources.ResourceManager.GetString("NormalDateTokenMonthOnlyFormatString", provider),
                        DateTimeExtensions.GetMonthString(this.Month.Value, provider));
                }

                // Month and year
                if (!this.Day.HasValue && this.Month.HasValue && this.Year.HasValue)
                {
                    return string.Format(
                        Resources.ResourceManager.GetEffectiveProvider(provider),
                        Resources.ResourceManager.GetString("NormalDateTokenMonthAndYearFormatString", provider),
                        DateTimeExtensions.GetMonthString(this.Month.Value, provider),
                        this.Year);
                }

                // Year
                if (!this.Day.HasValue && !this.Month.HasValue && this.Year.HasValue)
                {
                    return string.Format(
                        Resources.ResourceManager.GetEffectiveProvider(provider),
                        Resources.ResourceManager.GetString("NormalDateTokenYearOnlyFormatString", provider),
                        this.Year);
                }

                // Unsupported
                return this.GetType().ToString();
            }
            catch
            {
                return this.GetType().ToString();
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
                    return GetPatternsWithMonthFirst(provider);
                }
                else if (provider.IsYearFirst())
                {
                    return GetPatternsWithYearFirst(provider);
                }
                else
                {
                    return GetPatternsWithDayFirst(provider);
                }
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

                provider = Resources.ResourceManager.GetEffectiveProvider(provider);

                // Parse day
                if (match.Groups["day"].Success)
                {
                    dateToken.Day = int.Parse(match.Groups["day"].Value, provider);
                }

                // Parse month
                if (match.Groups["month"].Success)
                {
                    int month;
                    if (int.TryParse(match.Groups["month"].Value, NumberStyles.None, provider, out month))
                    {
                        dateToken.Month = month;
                    }
                    else
                    {
                        dateToken.Month = DateTimeExtensions.ParseMonth(match.Groups["month"].Value, provider);
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

            /// <summary>
            /// Returns a set of regular expressions for matching dates. In the case of ambiguity, these patterns favor
            /// interpreting matching strings specifying a day, month, and year in that order.
            /// </summary>
            /// <param name="provider">An <see cref="IFormatProvider"/>.</param>
            /// <returns>A set of regular expressions supported by this parser.</returns>
            private static IEnumerable<string> GetPatternsWithDayFirst(IFormatProvider provider)
            {
                return new[]
                {
                    Resources.ResourceManager.GetString("NormalDateTokenSpelledDateWithDayFirstPattern", provider),
                    Resources.ResourceManager.GetString("NormalDateTokenSpelledDateWithMonthFirstPattern", provider),
                    Resources.ResourceManager.GetString("NormalDateTokenNumericalDateWithDayFirstPattern", provider),
                    Resources.ResourceManager.GetString("NormalDateTokenNumericalDateWithMonthFirstPattern", provider),
                    Resources.ResourceManager.GetString("NormalDateTokenNumericalDateWithYearFirstPattern", provider),
                    Resources.ResourceManager.GetString("NormalDateTokenDayOnlyPattern", provider),
                    Resources.ResourceManager.GetString("NormalDateTokenSpelledMonthAndOptionalYearPattern", provider),
                    Resources.ResourceManager.GetString("NormalDateTokenNumericalMonthAndYearPattern", provider)
                };
            }

            /// <summary>
            /// Returns a set of regular expressions for matching dates. In the case of ambiguity, these patterns favor
            /// interpreting matching strings specifying a month, day, and year in that order.
            /// </summary>
            /// <param name="provider">An <see cref="IFormatProvider"/>.</param>
            /// <returns>A set of regular expressions supported by this parser.</returns>
            private static IEnumerable<string> GetPatternsWithMonthFirst(IFormatProvider provider)
            {
                return new[]
                {
                    Resources.ResourceManager.GetString("NormalDateTokenSpelledDateWithMonthFirstPattern", provider),
                    Resources.ResourceManager.GetString("NormalDateTokenSpelledDateWithDayFirstPattern", provider),
                    Resources.ResourceManager.GetString("NormalDateTokenNumericalDateWithMonthFirstPattern", provider),
                    Resources.ResourceManager.GetString("NormalDateTokenNumericalDateWithDayFirstPattern", provider),
                    Resources.ResourceManager.GetString("NormalDateTokenNumericalDateWithYearFirstPattern", provider),
                    Resources.ResourceManager.GetString("NormalDateTokenDayOnlyPattern", provider),
                    Resources.ResourceManager.GetString("NormalDateTokenSpelledMonthAndOptionalYearPattern", provider),
                    Resources.ResourceManager.GetString("NormalDateTokenNumericalMonthAndYearPattern", provider)
                };
            }

            /// <summary>
            /// Returns a set of regular expressions for matching dates. In the case of ambiguity, these patterns favor
            /// interpreting matching strings specifying a year, month, and day in that order.
            /// </summary>
            /// <param name="provider">An <see cref="IFormatProvider"/>.</param>
            /// <returns>A set of regular expressions supported by this parser.</returns>
            private static IEnumerable<string> GetPatternsWithYearFirst(IFormatProvider provider)
            {
                return new[]
                {
                    Resources.ResourceManager.GetString("NormalDateTokenSpelledDateWithDayFirstPattern", provider),
                    Resources.ResourceManager.GetString("NormalDateTokenSpelledDateWithMonthFirstPattern", provider),
                    Resources.ResourceManager.GetString("NormalDateTokenNumericalDateWithYearFirstPattern", provider),
                    Resources.ResourceManager.GetString("NormalDateTokenNumericalDateWithDayFirstPattern", provider),
                    Resources.ResourceManager.GetString("NormalDateTokenNumericalDateWithMonthFirstPattern", provider),
                    Resources.ResourceManager.GetString("NormalDateTokenDayOnlyPattern", provider),
                    Resources.ResourceManager.GetString("NormalDateTokenSpelledMonthAndOptionalYearPattern", provider),
                    Resources.ResourceManager.GetString("NormalDateTokenNumericalMonthAndYearPattern", provider)
                };
            }
        }
    }
}
