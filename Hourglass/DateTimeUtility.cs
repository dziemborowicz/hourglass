// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DateTimeUtility.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Text.RegularExpressions;

    /// <summary>
    /// A utility class for converting a <see cref="string"/> representation of a date and time (optionally relative to
    /// a reference date) to a <see cref="DateTime"/>, and for converting a <see cref="DateTime"/> to a natural <see
    /// cref="string"/> representation.
    /// </summary>
    public static class DateTimeUtility
    {
        #region Private Members

        /// <summary>
        /// The months of the year. Each month is represented by the first three letters of its name.
        /// </summary>
        private static readonly string[] Months = { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };

        /// <summary>
        /// The <see cref="RegexOptions"/> used by this class by default when matching an input <see cref="string"/>
        /// against a <see cref="Regex"/> pattern.
        /// </summary>
        /// <seealso cref="NaturalDateFormats"/>
        /// <seealso cref="NaturalDateFormatsWithMonthFirst"/>
        /// <seealso cref="NaturalDateFormatsWithYearFirst"/>
        /// <seealso cref="NaturalTimeFormats"/>
        private static readonly RegexOptions RegexOptions = RegexOptions.CultureInvariant | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace;

        /// <summary>
        /// A set of <see cref="Regex"/> patterns for matching the date part of an input <see cref="string"/>. In the
        /// case of ambiguity, these patterns favor interpreting the input <see cref="string"/> tokens as specifying a
        /// day, month and year in that order.
        /// </summary>
        /// <seealso cref="NaturalDateFormatsWithMonthFirst"/>
        /// <seealso cref="NaturalDateFormatsWithYearFirst"/>
        private static readonly string[] NaturalDateFormats =
        {
            // Weekdays only (e.g., "Sunday", "this Sunday", "next Sunday")
            @"  ((this|next)\s*)?
                (?<weekday>Sun|Mon|Tue|Wed|Thu|Fri|Sat)[a-z]*
            ",

            // Weekdays after next (e.g., "Sunday next", "Sunday after next")
            @"  (?<weekday>Sun|Mon|Tue|Wed|Thu|Fri|Sat)[a-z]*
                (\s*after)?
                \s*(?<next>next)
            ",

            // Weekdays next week (e.g., "Sunday next week")
            @"  (?<weekday>Sun|Mon|Tue|Wed|Thu|Fri|Sat)[a-z]*
                \s*(?<nextweek>next\s*week)
            ",

            // Day by itself (e.g., "14th", "the 14th")
            @"  (the\s*)?
                (?<day>\d\d?)
                (\s*(st|nd|rd|th))
            ",

            // Spelled date with day first and optional year (e.g., "14 February", "14 February 2003", "14th of February, 2003")
            @"  (?<day>\d\d?)
                (\s*(st|nd|rd|th))?
                (\s*of)?
                \s*(?<month>Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec)[a-z]*
                (
                    (\s*,?\s*)?
                    (?<year>(\d\d)?\d\d)
                )?
            ",

            // Spelled date with month first and optional year (e.g., "14 February", "14 February 2003", "14th of February, 2003")
            @"  (?<month>Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec)[a-z]*
                \s*(?<day>\d\d?)
                (\s*(st|nd|rd|th))?
                (
                    (\s*,?\s*)?
                    (?<year>(\d\d)?\d\d)
                )?
            ",

            // Numerical date with day first (e.g., "14/02", "14/02/2003")
            @"  (?<day>\d\d?)
                [.\-/]
                (?<month>\d\d?)
                (
                    [.\-/]
                    (?<year>(\d\d)?\d\d)
                )?
            ",

            // Numerical date with month first (e.g., "02/14", "02/14/2003")
            @"  (?<month>\d\d?)
                [.\-/]
                (?<day>\d\d?)
                (
                    [.\-/]
                    (?<year>(\d\d)?\d\d)
                )?
            ",

            // Numerical date with year first (e.g., "03/02/14", "2003/02/14")
            @"  (?<year>(\d\d)?\d\d)
                [.\-/]
                (?<month>\d\d?)
                [.\-/]
                (?<day>\d\d?)
            ",

            // Year only (e.g., "2003", "2015")
            @"  (?<year>20\d\d)
            "
        };

        /// <summary>
        /// A set of <see cref="Regex"/> patterns for matching the date part of an input <see cref="string"/>. In the
        /// case of ambiguity, these patterns favor interpreting the input <see cref="string"/> tokens as specifying a
        /// month, day and year in that order.
        /// </summary>
        /// <seealso cref="NaturalDateFormats"/>
        /// <seealso cref="NaturalDateFormatsWithYearFirst"/>
        private static readonly string[] NaturalDateFormatsWithMonthFirst =
        {
            // Weekdays only (e.g., "Sunday", "this Sunday", "next Sunday")
            @"  ((this|next)\s*)?
                (?<weekday>Sun|Mon|Tue|Wed|Thu|Fri|Sat)[a-z]*
            ",

            // Weekdays after next (e.g., "Sunday next", "Sunday after next")
            @"  (?<weekday>Sun|Mon|Tue|Wed|Thu|Fri|Sat)[a-z]*
                (\s*after)?
                \s*(?<next>next)
            ",

            // Weekdays next week (e.g., "Sunday next week")
            @"  (?<weekday>Sun|Mon|Tue|Wed|Thu|Fri|Sat)[a-z]*
                \s*(?<nextweek>next\s*week)
            ",

            // Day by itself (e.g., "14th", "the 14th")
            @"  (the\s*)?
                (?<day>\d\d?)
                (\s*(st|nd|rd|th))
            ",

            // Spelled date with day first and optional year (e.g., "14 February", "14 February 2003", "14th of February, 2003")
            @"  (?<day>\d\d?)
                (\s*(st|nd|rd|th))?
                (\s*of)?
                \s*(?<month>Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec)[a-z]*
                (
                    (\s*,?\s*)?
                    (?<year>(\d\d)?\d\d)
                )?
            ",

            // Spelled date with month first and optional year (e.g., "14 February", "14 February 2003", "14th of February, 2003")
            @"  (?<month>Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec)[a-z]*
                \s*(?<day>\d\d?)
                (\s*(st|nd|rd|th))?
                (
                    (\s*,?\s*)?
                    (?<year>(\d\d)?\d\d)
                )?
            ",

            // Numerical date with month first (e.g., "02/14", "02/14/2003")
            @"  (?<month>\d\d?)
                [.\-/]
                (?<day>\d\d?)
                (
                    [.\-/]
                    (?<year>(\d\d)?\d\d)
                )?
            ",

            // Numerical date with day first (e.g., "14/02", "14/02/2003")
            @"  (?<day>\d\d?)
                [.\-/]
                (?<month>\d\d?)
                (
                    [.\-/]
                    (?<year>(\d\d)?\d\d)
                )?
            ",

            // Numerical date with year first (e.g., "03/02/14", "2003/02/14")
            @"  (?<year>(\d\d)?\d\d)
                [.\-/]
                (?<month>\d\d?)
                [.\-/]
                (?<day>\d\d?)
            ",

            // Year only (e.g., "2003", "2015")
            @"  (?<year>20\d\d)
            "
        };

        /// <summary>
        /// A set of <see cref="Regex"/> patterns for matching the date part of an input <see cref="string"/>. In the
        /// case of ambiguity, these patterns favor interpreting the input <see cref="string"/> tokens as specifying a
        /// year, month and day in that order.
        /// </summary>
        /// <seealso cref="NaturalDateFormats"/>
        /// <seealso cref="NaturalDateFormatsWithMonthFirst"/>
        private static readonly string[] NaturalDateFormatsWithYearFirst =
        {
            // Weekdays only (e.g., "Sunday", "this Sunday", "next Sunday")
            @"  ((this|next)\s*)?
                (?<weekday>Sun|Mon|Tue|Wed|Thu|Fri|Sat)[a-z]*
            ",

            // Weekdays after next (e.g., "Sunday next", "Sunday after next")
            @"  (?<weekday>Sun|Mon|Tue|Wed|Thu|Fri|Sat)[a-z]*
                (\s*after)?
                \s*(?<next>next)
            ",

            // Weekdays next week (e.g., "Sunday next week")
            @"  (?<weekday>Sun|Mon|Tue|Wed|Thu|Fri|Sat)[a-z]*
                \s*(?<nextweek>next\s*week)
            ",

            // Day by itself (e.g., "14th", "the 14th")
            @"  (the\s*)?
                (?<day>\d\d?)
                (\s*(st|nd|rd|th))
            ",

            // Spelled date with day first and optional year (e.g., "14 February", "14 February 2003", "14th of February, 2003")
            @"  (?<day>\d\d?)
                (\s*(st|nd|rd|th))?
                (\s*of)?
                \s*(?<month>Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec)[a-z]*
                (
                    (\s*,?\s*)?
                    (?<year>(\d\d)?\d\d)
                )?
            ",

            // Spelled date with month first and optional year (e.g., "14 February", "14 February 2003", "14th of February, 2003")
            @"  (?<month>Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec)[a-z]*
                \s*(?<day>\d\d?)
                (\s*(st|nd|rd|th))?
                (
                    (\s*,?\s*)?
                    (?<year>(\d\d)?\d\d)
                )?
            ",

            // Numerical date with year first (e.g., "03/02/14", "2003/02/14")
            @"  (?<year>(\d\d)?\d\d)
                [.\-/]
                (?<month>\d\d?)
                [.\-/]
                (?<day>\d\d?)
            ",

            // Numerical date with day first (e.g., "14/02", "14/02/2003")
            @"  (?<day>\d\d?)
                [.\-/]
                (?<month>\d\d?)
                (
                    [.\-/]
                    (?<year>(\d\d)?\d\d)
                )?
            ",

            // Numerical date with month first (e.g., "02/14", "02/14/2003")
            @"  (?<month>\d\d?)
                [.\-/]
                (?<day>\d\d?)
                (
                    [.\-/]
                    (?<year>(\d\d)?\d\d)
                )?
            ",

            // Year only (e.g., "2003", "2015")
            @"  (?<year>20\d\d)
            "
        };

        /// <summary>
        /// A set of <see cref="Regex"/> patterns for matching the time part of an input <see cref="string"/>.
        /// </summary>
        private static readonly string[] NaturalTimeFormats =
        {
            // Time with separators (e.g., "5", "5 pm", "5:30", "5:30 p.m.", "5:30:45 p.m.", "17:30h")
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
                (?<ampm>
                    (a|p)\.?
                    (\s*m\.?)
                    |
                    h[a-z]*
                )?
            ",
             
            // Time without separators (e.g., "5", "5 pm", "530", "530 p.m.", "53045 p.m.", "1730h")
            @"  (?<hour>\d\d?)
                (
                    (?<minute>\d\d)
                    (?<second>\d\d)?
                )?
                \s*
                (?<ampm>
                    (a|p)\.?
                    (\s*m\.?)
                    |
                    h[a-z]*
                )?
            "
        };

        #endregion

        #region Public Methods

        /// <summary>
        /// Parses a <see cref="string"/> representation of a date and time relative to a reference date into a <see
        /// cref="DateTime"/>.
        /// </summary>
        /// <remarks>
        /// This overload uses <see cref="DateTime.Now"/> as the reference date and time and the <see
        /// cref="CultureInfo.CurrentCulture"/> as the <see cref="IFormatProvider"/> when parsing.
        /// </remarks>
        /// <param name="str">A <see cref="string"/> representation of a date and time.</param>
        /// <returns>A <see cref="DateTime"/> representation of the date and time.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="str"/> is <c>null</c>.</exception>
        /// <exception cref="FormatException">If <paramref name="str"/> is not a valid representation of a date and
        /// time.</exception>
        public static DateTime ParseNatural(string str)
        {
            return ParseNatural(str, DateTime.Now);
        }

        /// <summary>
        /// Parses a <see cref="string"/> representation of a date and time relative to a reference date into a <see
        /// cref="DateTime"/>.
        /// </summary>
        /// <remarks>
        /// This overload uses the <see cref="CultureInfo.CurrentCulture"/> as the <see cref="IFormatProvider"/> when
        /// parsing.
        /// </remarks>
        /// <param name="str">A <see cref="string"/> representation of a date and time.</param>
        /// <param name="referenceDate">A <see cref="DateTime"/> representing the reference date.</param>
        /// <returns>A <see cref="DateTime"/> representation of the date and time.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="str"/> is <c>null</c>.</exception>
        /// <exception cref="FormatException">If <paramref name="str"/> is not a valid representation of a date and
        /// time.</exception>
        public static DateTime ParseNatural(string str, DateTime referenceDate)
        {
            return ParseNatural(str, referenceDate, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Parses a <see cref="string"/> representation of a date and time relative to a reference date into a <see
        /// cref="DateTime"/>.
        /// </summary>
        /// <param name="str">A <see cref="string"/> representation of a date and time.</param>
        /// <param name="referenceDate">A <see cref="DateTime"/> representing the reference date.</param>
        /// <param name="provider">An <see cref="IFormatProvider"/> to use when parsing.</param>
        /// <returns>A <see cref="DateTime"/> representation of the date and time.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="str"/> is <c>null</c>.</exception>
        /// <exception cref="FormatException">If <paramref name="str"/> is not a valid representation of a date and
        /// time.</exception>
        public static DateTime ParseNatural(string str, DateTime referenceDate, IFormatProvider provider)
        {
            // Null or empty input
            if (str == null)
            {
                throw new ArgumentNullException("str");
            }

            if (string.IsNullOrWhiteSpace(str))
            {
                throw new FormatException();
            }

            // Special dates
            if (Regex.IsMatch(str, @"^(nye?|new\s*year('?s)?(\s*eve)?)$", RegexOptions))
            {
                return new DateTime(referenceDate.Year + 1, 1, 1);
            }

            // Month only
            Regex monthRegex = new Regex(@"^(?<month>(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))[a-z]*$", RegexOptions);
            Match monthMatch = monthRegex.Match(str);
            if (monthMatch.Success)
            {
                int month = ParseMonth(str);
                DateTime dateTime = new DateTime(referenceDate.Year, month, 1);
                return dateTime < referenceDate ? dateTime.AddYears(1) : dateTime;
            }

            // Normalize noon, midday, or midnight
            str = Regex.Replace(str, @"\b(12([.:]00([.:]00)?)?\s*)?noon\b", "12:00:00 PM", RegexOptions);
            str = Regex.Replace(str, @"\b(12([.:]00([.:]00)?)?\s*)?mid(-?d)?ay\b", "12:00:00 PM", RegexOptions);
            str = Regex.Replace(str, @"\b(12([.:]00([.:]00)?)?\s*)?mid-?night\b", "12:00:00 AM", RegexOptions);

            // Normalize relative dates
            str = Regex.Replace(str, @"\btodd?ay\b", referenceDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture), RegexOptions);
            str = Regex.Replace(str, @"\btomm?orr?ow\b", referenceDate.AddDays(1).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture), RegexOptions);

            // Try each format
            foreach (string format in GetNaturalFormats(provider))
            {
                try
                {
                    Match match = Regex.Match(str, format, RegexOptions);
                    if (match.Success)
                    {
                        int? year = null, month = null, day = null, hour = null, minute = null, second = null;

                        if (match.Groups["weekday"].Success)
                        {
                            // Parse weekday
                            DayOfWeek dayOfWeek = ParseDayOfWeek(match.Groups["weekday"].Value);

                            // Find next matching date
                            DateTime nextMatchingDate = referenceDate.AddDays(1);
                            while (nextMatchingDate.DayOfWeek != dayOfWeek)
                            {
                                nextMatchingDate = nextMatchingDate.AddDays(1);
                            }

                            // Advance by one week if necessary
                            if (match.Groups["next"].Success)
                            {
                                nextMatchingDate = nextMatchingDate.AddDays(7);
                            }

                            // Advance to next week if necessary
                            if (match.Groups["nextweek"].Success && dayOfWeek > referenceDate.DayOfWeek)
                            {
                                nextMatchingDate = nextMatchingDate.AddDays(7);
                            }

                            day = nextMatchingDate.Day;
                            month = nextMatchingDate.Month;
                            year = nextMatchingDate.Year;
                        }
                        else
                        {
                            // Parse day
                            if (match.Groups["day"].Success)
                            {
                                day = int.Parse(match.Groups["day"].Value, provider);
                            }

                            // Parse month
                            if (match.Groups["month"].Success)
                            {
                                if (Regex.IsMatch(match.Groups["month"].Value, @"^\d+$", RegexOptions))
                                {
                                    month = int.Parse(match.Groups["month"].Value, provider);
                                }
                                else
                                {
                                    month = ParseMonth(match.Groups["month"].Value);
                                }
                            }

                            // Parse year
                            if (match.Groups["year"].Success)
                            {
                                year = int.Parse(match.Groups["year"].Value, provider);
                                if (year < 100)
                                {
                                    year += referenceDate.Year / 1000 * 1000;
                                }
                            }
                        }

                        // Parse hours
                        if (match.Groups["hour"].Success)
                        {
                            hour = int.Parse(match.Groups["hour"].Value, provider);
                        }

                        // Parse minutes
                        if (match.Groups["minute"].Success)
                        {
                            minute = int.Parse(match.Groups["minute"].Value, provider);
                        }

                        // Parse seconds
                        if (match.Groups["second"].Success)
                        {
                            second = int.Parse(match.Groups["second"].Value, provider);
                        }

                        // Parse AM/PM
                        if (match.Groups["ampm"].Success && match.Groups["ampm"].Value.StartsWith("p", StringComparison.InvariantCultureIgnoreCase) && hour.HasValue && hour != 12)
                        {
                            hour += 12;
                        }

                        // Fix reference to midnight as "12"
                        if ((!match.Groups["ampm"].Success || match.Groups["ampm"].Value.StartsWith("a", StringComparison.InvariantCultureIgnoreCase)) && hour.HasValue && hour == 12)
                        {
                            hour = 0;
                        }

                        // Fill fields left
                        if (year.HasValue)
                        {
                            month = month ?? 1;
                        }

                        if (month.HasValue)
                        {
                            day = day ?? 1;
                        }

                        if (day.HasValue)
                        {
                            hour = hour ?? 0;
                        }

                        if (hour.HasValue)
                        {
                            minute = minute ?? 0;
                        }

                        if (minute.HasValue)
                        {
                            second = second ?? 0;
                        }

                        // Fill fields right and create DateTime
                        DateTime dateTime = new DateTime(
                            year ?? referenceDate.Year,
                            month ?? referenceDate.Month,
                            day ?? referenceDate.Day,
                            hour ?? referenceDate.Hour,
                            minute ?? referenceDate.Minute,
                            second ?? referenceDate.Second);

                        // Prefer output dates after reference date
                        if (dateTime <= referenceDate)
                        {
                            if (match.Groups["hour"].Success && !match.Groups["ampm"].Success && hour.HasValue && hour < 12 && dateTime.AddHours(12) > referenceDate)
                            {
                                dateTime = dateTime.AddHours(12);
                            }
                            else if (!day.HasValue)
                            {
                                dateTime = dateTime.AddDays(1);
                            }
                            else if (!month.HasValue)
                            {
                                dateTime = dateTime.AddMonths(1);
                            }
                            else if (!year.HasValue)
                            {
                                dateTime = dateTime.AddYears(1);
                            }
                        }

                        // Prefer 8:00:00 AM to 7:59:59 PM for times without AM/PM except on the reference date
                        if (!match.Groups["ampm"].Success && dateTime.Date != referenceDate.Date && dateTime.TimeOfDay != TimeSpan.Zero && dateTime.Hour < 8)
                        {
                            dateTime = dateTime.AddHours(12);
                        }

                        return dateTime;
                    }
                }
                catch
                {
                    // Try the next pattern
                    continue;
                }
            }

            throw new FormatException();
        }

        /// <summary>
        /// Parses a <see cref="string"/> representation of a date and time relative to a reference date into a <see
        /// cref="DateTime"/>.
        /// </summary>
        /// <remarks>
        /// This overload uses <see cref="DateTime.Now"/> as the reference date and time and the <see
        /// cref="CultureInfo.CurrentCulture"/> as the <see cref="IFormatProvider"/> when parsing.
        /// </remarks>
        /// <param name="str">A <see cref="string"/> representation of a date and time.</param>
        /// <param name="dateTime">A <see cref="DateTime"/> representation of the date and time, or <see
        /// cref="DateTime.MinValue"/> if this method returns <c>false</c>.</param>
        /// <returns><c>true</c> if <paramref name="str"/> was successfully parsed into a <see cref="DateTime"/>, or
        /// <c>false</c> otherwise.</returns>
        public static bool TryParseNatural(string str, out DateTime dateTime)
        {
            return TryParseNatural(str, DateTime.Now, out dateTime);
        }

        /// <summary>
        /// Parses a <see cref="string"/> representation of a date and time relative to a reference date into a <see
        /// cref="DateTime"/>.
        /// </summary>
        /// <remarks>
        /// This overload uses the <see cref="CultureInfo.CurrentCulture"/> as the <see cref="IFormatProvider"/> when
        /// parsing.
        /// </remarks>
        /// <param name="str">A <see cref="string"/> representation of a date and time.</param>
        /// <param name="referenceDate">A <see cref="DateTime"/> representing the reference date.</param>
        /// <param name="dateTime">A <see cref="DateTime"/> representation of the date and time, or <see
        /// cref="DateTime.MinValue"/> if this method returns <c>false</c>.</param>
        /// <returns><c>true</c> if <paramref name="str"/> was successfully parsed into a <see cref="DateTime"/>, or
        /// <c>false</c> otherwise.</returns>
        public static bool TryParseNatural(string str, DateTime referenceDate, out DateTime dateTime)
        {
            return TryParseNatural(str, referenceDate, CultureInfo.CurrentCulture, out dateTime);
        }

        /// <summary>
        /// Parses a <see cref="string"/> representation of a date and time relative to a reference date into a <see
        /// cref="DateTime"/>.
        /// </summary>
        /// <param name="str">A <see cref="string"/> representation of a date and time.</param>
        /// <param name="referenceDate">A <see cref="DateTime"/> representing the reference date.</param>
        /// <param name="provider">An <see cref="IFormatProvider"/> to use when parsing.</param>
        /// <param name="dateTime">A <see cref="DateTime"/> representation of the date and time, or <see
        /// cref="DateTime.MinValue"/> if this method returns <c>false</c>.</param>
        /// <returns><c>true</c> if <paramref name="str"/> was successfully parsed into a <see cref="DateTime"/>, or
        /// <c>false</c> otherwise.</returns>
        public static bool TryParseNatural(string str, DateTime referenceDate, IFormatProvider provider, out DateTime dateTime)
        {
            try
            {
                dateTime = ParseNatural(str, referenceDate, provider);
                return true;
            }
            catch
            {
                dateTime = DateTime.MinValue;
                return false;
            }
        }

        /// <summary>
        /// Converts a <see cref="DateTime"/> to a natural <see cref="string"/> representation.
        /// </summary>
        /// <remarks>
        /// This overload uses the <see cref="CultureInfo.CurrentCulture"/> as the <see cref="IFormatProvider"/> when
        /// converting.
        /// </remarks>
        /// <param name="dateTime">A <see cref="DateTime"/>.</param>
        /// <returns>A natural <see cref="string"/> representation of the <see cref="DateTime"/>.</returns>
        public static string ToNaturalString(DateTime dateTime)
        {
            return ToNaturalString(dateTime, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Converts a <see cref="Nullable{DateTime}"/> to a natural <see cref="string"/> representation.
        /// </summary>
        /// <remarks>
        /// This overload uses the <see cref="CultureInfo.CurrentCulture"/> as the <see cref="IFormatProvider"/> when
        /// converting.
        /// </remarks>
        /// <param name="dateTime">A <see cref="Nullable{DateTime}"/>.</param>
        /// <returns>A natural <see cref="string"/> representation of the <see cref="Nullable{DateTime}"/>, or "null"
        /// if <paramref name="dateTime"/> is <c>null</c>.</returns>
        public static string ToNaturalString(DateTime? dateTime)
        {
            return dateTime.HasValue ? ToNaturalString(dateTime.Value) : "null";
        }

        /// <summary>
        /// Converts a <see cref="DateTime"/> to a natural <see cref="string"/> representation.
        /// </summary>
        /// <param name="dateTime">A <see cref="DateTime"/>.</param>
        /// <param name="provider">An <see cref="IFormatProvider"/> to use when converting. This value is only used to
        /// determine the order of the day, month and year in the date part of the <see cref="string"/>.</param>
        /// <returns>A natural <see cref="string"/> representation of the <see cref="DateTime"/>.</returns>
        public static string ToNaturalString(DateTime dateTime, IFormatProvider provider)
        {
            string monthFormatString = GetMonthFormatString(provider);

            if (dateTime.Second != 0)
            {
                return dateTime.ToString(monthFormatString + " h:mm:ss tt", CultureInfo.InvariantCulture);
            }

            if (dateTime.Minute != 0 || dateTime.Hour != 0)
            {
                return dateTime.ToString(monthFormatString + " h:mm tt", CultureInfo.InvariantCulture);
            }

            return dateTime.ToString(monthFormatString, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Converts a <see cref="Nullable{DateTime}"/> to a natural <see cref="string"/> representation.
        /// </summary>
        /// <param name="dateTime">A <see cref="Nullable{DateTime}"/>.</param>
        /// <param name="provider">An <see cref="IFormatProvider"/> to use when converting. This value is only used to
        /// determine the order of the day, month and year in the date part of the <see cref="string"/>.</param>
        /// <returns>A natural <see cref="string"/> representation of the <see cref="Nullable{DateTime}"/>, or "null"
        /// if <paramref name="dateTime"/> is <c>null</c>.</returns>
        public static string ToNaturalString(DateTime? dateTime, IFormatProvider provider)
        {
            return dateTime.HasValue ? ToNaturalString(dateTime.Value, provider) : "null";
        }

        /// <summary>
        /// Returns the later of two <see cref="DateTime"/>s.
        /// </summary>
        /// <param name="a">The first <see cref="DateTime"/> to compare.</param>
        /// <param name="b">The second <see cref="DateTime"/> to compare.</param>
        /// <returns><paramref name="a"/> or <paramref name="b"/>, whichever is later.</returns>
        public static DateTime Max(DateTime a, DateTime b)
        {
            return a > b ? a : b;
        }

        /// <summary>
        /// Returns the earlier of two <see cref="DateTime"/>s.
        /// </summary>
        /// <param name="a">The first <see cref="DateTime"/> to compare.</param>
        /// <param name="b">The second <see cref="DateTime"/> to compare.</param>
        /// <returns><paramref name="a"/> or <paramref name="b"/>, whichever is earlier.</returns>
        public static DateTime Min(DateTime a, DateTime b)
        {
            return a < b ? a : b;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Returns an enumeration of <see cref="Regex"/> patterns for representations of dates and times by combining
        /// <see cref="NaturalDateFormats"/> (or <see cref="NaturalDateFormatsWithMonthFirst"/> or <see
        /// cref="NaturalDateFormatsWithYearFirst"/>) and <see cref="NaturalTimeFormats"/>.
        /// </summary>
        /// <param name="provider">An <see cref="IFormatProvider"/> to use to determine the preference for the order of
        /// the day, month and year in the date part of the <see cref="Regex"/> patterns.</param>
        /// <returns>An enumeration of <see cref="Regex"/> patterns for representations of dates and times.</returns>
        private static IEnumerable<string> GetNaturalFormats(IFormatProvider provider)
        {
            foreach (string dateFormat in GetNaturalDateFormats(provider))
            {
                yield return "^" + dateFormat + "$";
            }

            foreach (string timeFormat in NaturalTimeFormats)
            {
                yield return "^" + timeFormat + "$";
            }

            foreach (string dateFormat in GetNaturalDateFormats(provider))
            {
                foreach (string timeFormat in NaturalTimeFormats)
                {
                    yield return "^" + dateFormat + @"\s+(at\s+)?" + timeFormat + "$";
                    yield return "^" + timeFormat + @"\s+(on\s+)?" + dateFormat + "$";
                }
            }
        }

        /// <summary>
        /// Returns an enumeration of <see cref="Regex"/> patterns for representations of dates.
        /// </summary>
        /// <param name="provider">An <see cref="IFormatProvider"/> to use to determine the preference for the order of
        /// the day, month and year in the <see cref="Regex"/> patterns.</param>
        /// <returns>An enumeration of <see cref="Regex"/> patterns for representations of dates.</returns>
        private static IEnumerable<string> GetNaturalDateFormats(IFormatProvider provider)
        {
            if (IsMonthFirst(provider))
            {
                return NaturalDateFormatsWithMonthFirst;
            }

            if (IsYearFirst(provider))
            {
                return NaturalDateFormatsWithYearFirst;
            }

            return NaturalDateFormats;
        }

        /// <summary>
        /// Returns a value indicating whether the specified <see cref="IFormatProvider"/> prefers the month-day-year
        /// ordering in date representations.
        /// </summary>
        /// <param name="provider">An <see cref="IFormatProvider"/>.</param>
        /// <returns>A value indicating whether the specified <see cref="IFormatProvider"/> prefers the month-day-year
        /// ordering in date representations.</returns>
        private static bool IsMonthFirst(IFormatProvider provider)
        {
            DateTimeFormatInfo formatInfo = (DateTimeFormatInfo)provider.GetFormat(typeof(DateTimeFormatInfo));
            return Regex.IsMatch(formatInfo.ShortDatePattern, @"^.*M.*d.*y.*$");
        }

        /// <summary>
        /// Returns a value indicating whether the specified <see cref="IFormatProvider"/> prefers the year-month-day
        /// ordering in date representations.
        /// </summary>
        /// <param name="provider">An <see cref="IFormatProvider"/>.</param>
        /// <returns>A value indicating whether the specified <see cref="IFormatProvider"/> prefers the year-month-day
        /// ordering in date representations.</returns>
        private static bool IsYearFirst(IFormatProvider provider)
        {
            DateTimeFormatInfo formatInfo = (DateTimeFormatInfo)provider.GetFormat(typeof(DateTimeFormatInfo));
            return Regex.IsMatch(formatInfo.ShortDatePattern, @"^.*y.*M.*d.*$");
        }

        /// <summary>
        /// Returns the month number for a given <see cref="string"/> representation of a month.
        /// </summary>
        /// <remarks>
        /// This method only checks the first three characters of the <see cref="string"/> against the list of <see
        /// cref="Months"/>.
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
                if (str.StartsWith(Months[i], StringComparison.InvariantCultureIgnoreCase))
                {
                    return i + 1;
                }
            }

            throw new FormatException();
        }

        /// <summary>
        /// Returns the <see cref="DayOfWeek"/> for a given <see cref="string"/> representation of a weekday.
        /// </summary>
        /// <remarks>
        /// This method only checks the first three characters of the <see cref="string"/> against the <see
        /// cref="DayOfWeek"/> <c>enum</c>.
        /// </remarks>
        /// <param name="str">A <see cref="string"/> representation of a weekday.</param>
        /// <returns>The <see cref="DayOfWeek"/> for the <see cref="string"/> representation of the weekday.</returns>
        /// <exception cref="FormatException">If <paramref name="str"/> is not a valid <see cref="string"/>
        /// representation of a weekday.</exception>
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

        /// <summary>
        /// Returns the format string for a date.
        /// </summary>
        /// <param name="provider">An <see cref="IFormatProvider"/> to use to determine the preference for the order of
        /// the day, month and year in the format string.</param>
        /// <returns>The format string for a date.</returns>
        private static string GetMonthFormatString(IFormatProvider provider)
        {
            if (IsMonthFirst(provider))
            {
                return "MMMM d, yyyy";
            }

            if (IsYearFirst(provider))
            {
                return "yyyy MMMM d";
            }

            return "d MMMM yyyy";
        }

        #endregion
    }
}
