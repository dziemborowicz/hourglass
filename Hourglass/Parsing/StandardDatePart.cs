// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StandardDatePart.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass.Parsing
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Text;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Represents a date specified as a year, month, and day.
    /// </summary>
    public class StandardDatePart : DatePart
    {
        /// <summary>
        /// The months of the year.
        /// </summary>
        private static readonly string[] Months = 
        {
            "January",
            "February",
            "March",
            "April",
            "May",
            "June",
            "July",
            "August",
            "September",
            "October",
            "November",
            "December"
        };

        /// <summary>
        /// Gets or sets the year represented by this part.
        /// </summary>
        public int? Year { get; set; }

        /// <summary>
        /// Gets or sets the month represented by this part.
        /// </summary>
        public int? Month { get; set; }

        /// <summary>
        /// Gets or sets the day represented by this part.
        /// </summary>
        public int? Day { get; set; }

        /// <summary>
        /// Gets a value indicating whether the part is valid.
        /// </summary>
        public override bool IsValid
        {
            get
            {
                try
                {
                    DateTime date = new DateTime(
                        this.Year ?? 2000,
                        this.Month ?? 1,
                        this.Day ?? 1);

                    return date != DateTime.MinValue && date != DateTime.MaxValue;
                }
                catch
                {
                    return false;
                }
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

            DateTime date;

            int year = this.Year ?? referenceDate.Year;
            int month = this.Month ?? (!this.Year.HasValue ? referenceDate.Month : 1);
            int day = this.Day ?? (!this.Year.HasValue && !this.Month.HasValue ? referenceDate.Day : 1);

            while (!TryToDateTime(year, month, day, out date)
                || date < referenceDate.Date
                || (date == referenceDate.Date && tryExcludeReferenceDate))
            {
                // Try the next month if we only have a day
                if (!this.Month.HasValue && !this.Year.HasValue)
                {
                    if (month < 12)
                    {
                        month++;
                    }
                    else
                    {
                        month = 1;
                        year++;
                    }

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
            StringBuilder stringBuilder = new StringBuilder();

            // Day and month
            if (this.Day.HasValue && this.Month.HasValue)
            {
                string format = DateTimeUtility.IsMonthFirst(provider) ? "{1} {0}" : "{0} {1}";
                stringBuilder.AppendFormat(format, this.Day, ToMonthString(this.Month.Value));
            }
            else if (this.Day.HasValue)
            {
                stringBuilder.Append(ToOrdinalDayString(this.Day.Value));
            }
            else if (this.Month.HasValue)
            {
                stringBuilder.Append(ToMonthString(this.Month.Value));
            }

            // Year
            if (this.Year.HasValue)
            {
                if (stringBuilder.Length > 0)
                {
                    if (DateTimeUtility.IsMonthFirst(provider) && this.Day.HasValue && this.Month.HasValue)
                    {
                        stringBuilder.Append(", ");
                    }
                    else
                    {
                        stringBuilder.Append(" ");
                    }
                }

                stringBuilder.Append(this.Year);
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Tries to create a new <see cref="DateTime"/> from a year, month, and day.
        /// </summary>
        /// <param name="year">A year.</param>
        /// <param name="month">A month.</param>
        /// <param name="day">A day.</param>
        /// <param name="dateTime">The <see cref="DateTime"/>.</param>
        /// <returns><c>true</c> if a new <see cref="DateTime"/> is successfully created from the year, month, and day,
        /// or <c>false</c> otherwise.</returns>
        private static bool TryToDateTime(int year, int month, int day, out DateTime dateTime)
        {
            try
            {
                dateTime = new DateTime(year, month, day);
                return true;
            }
            catch
            {
                dateTime = DateTime.MinValue;
                return false;
            }
        }

        /// <summary>
        /// Converts a day number to a ordinal string representation of the day.
        /// </summary>
        /// <param name="day">A day number between 1 and 31 inclusive.</param>
        /// <returns>The ordinal string representation of the day.</returns>
        private static string ToOrdinalDayString(int day)
        {
            if (day % 10 == 1 && day / 10 != 1)
            {
                return string.Format("{0}st", day);
            }

            if (day % 10 == 1 && day / 10 != 1)
            {
                return string.Format("{0}nd", day);
            }

            if (day % 10 == 1 && day / 10 != 1)
            {
                return string.Format("{0}rd", day);
            }

            return string.Format("{0}th", day);
        }

        /// <summary>
        /// Converts a month number to a string representation of the month.
        /// </summary>
        /// <param name="month">A month number between 1 and 12 inclusive.</param>
        /// <returns>The string representation of the month.</returns>
        private static string ToMonthString(int month)
        {
            return Months[month - 1];
        }

        /// <summary>
        /// Parses <see cref="StandardDatePart"/>s from <see cref="string"/>s.
        /// </summary>
        public new class Parser : DatePart.Parser
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
                    \s*(?<month>Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec)[a-z]*
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
                @"  (?<month>Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec)[a-z]*
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
                @"  (?<year>(\d\d)?\d\d)
                    [.\-/]
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
            /// A regular expression that matches months with optional years (e.g., "Jan", "Feb 2015").
            /// </summary>
            private const string MonthAndOptionalYearPattern =
                @"  (?<month>(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))[a-z]*
                    (
                        \s*,?\s*
                        (?<year>\d\d\d\d)
                    )?
                ";

            /// <summary>
            /// A regular expression that matches years only (e.g., "2003", "2015").
            /// </summary>
            private const string YearOnlyPattern =
                @"  (?<year>20\d\d)
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
                MonthAndOptionalYearPattern,
                YearOnlyPattern
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
                MonthAndOptionalYearPattern,
                YearOnlyPattern
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
                MonthAndOptionalYearPattern,
                YearOnlyPattern
            };

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
                if (DateTimeUtility.IsMonthFirst(provider))
                {
                    return PatternsWithMonthFirst;
                }

                if (DateTimeUtility.IsYearFirst(provider))
                {
                    return PatternsWithYearFirst;
                }

                return PatternsWithDayFirst;
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
                StandardDatePart datePart = new StandardDatePart();

                // Parse day
                if (match.Groups["day"].Success)
                {
                    datePart.Day = int.Parse(match.Groups["day"].Value, provider);
                }

                // Parse month
                if (match.Groups["month"].Success)
                {
                    if (Regex.IsMatch(match.Groups["month"].Value, @"^\d+$", DateTimePart.Parser.RegexOptions))
                    {
                        datePart.Month = int.Parse(match.Groups["month"].Value, provider);
                    }
                    else
                    {
                        datePart.Month = ParseMonth(match.Groups["month"].Value);
                    }
                }

                // Parse year
                if (match.Groups["year"].Success)
                {
                    datePart.Year = int.Parse(match.Groups["year"].Value, provider);

                    if (datePart.Year.Value < 100)
                    {
                        datePart.Year += 2000;
                    }
                }

                return datePart;
            }

            /// <summary>
            /// Returns the month number for a given <see cref="string"/> representation of a month.
            /// </summary>
            /// <remarks>
            /// This method only checks the first three characters of the <see cref="string"/> against the first three
            /// characters of the strings in the <see cref="Months"/> list.
            /// </remarks>
            /// <param name="str">A <see cref="string"/> representation of a month.</param>
            /// <returns>The month number for the <see cref="string"/> representation of the month.</returns>
            /// <exception cref="FormatException">If <paramref name="str"/> is not a valid <see cref="string"/>
            /// representation of a month.</exception>
            /// <seealso cref="Months"/>
            private static int ParseMonth(string str)
            {
                for (int i = 0; i < Months.Length; i++)
                {
                    if (str.StartsWith(Months[i].Substring(0, 3), StringComparison.InvariantCultureIgnoreCase))
                    {
                        return i + 1;
                    }
                }

                throw new FormatException();
            }
        }
    }
}
