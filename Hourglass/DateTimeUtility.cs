using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Hourglass
{
    public static class DateTimeUtility
    {
        #region Patterns

        private static readonly RegexOptions RegexOptions = RegexOptions.CultureInvariant | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace;

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

        #region Helper Methods

        private static IEnumerable<string> GetNaturalFormats(IFormatProvider provider)
        {
            foreach (var dateFormat in GetNaturalDateFormats(provider))
                yield return "^" + dateFormat + "$";

            foreach (var timeFormat in NaturalTimeFormats)
                yield return "^" + timeFormat + "$";

            foreach (var dateFormat in GetNaturalDateFormats(provider))
                foreach (var timeFormat in NaturalTimeFormats)
                {
                    yield return "^" + dateFormat + @"\s+(at\s+)?" + timeFormat + "$";
                    yield return "^" + timeFormat + @"\s+(on\s+)?" + dateFormat + "$";
                }
        }

        private static IEnumerable<string> GetNaturalDateFormats(IFormatProvider provider)
        {
            if (IsMonthFirst(provider))
                return NaturalDateFormatsWithMonthFirst;

            if (IsYearFirst(provider))
                return NaturalDateFormatsWithYearFirst;

            return NaturalDateFormats;
        }

        private static bool IsMonthFirst(IFormatProvider provider)
        {
            var formatInfo = (DateTimeFormatInfo)provider.GetFormat(typeof(DateTimeFormatInfo));
            return Regex.IsMatch(formatInfo.ShortDatePattern, @"^.*M.*d.*y.*$");
        }

        private static bool IsYearFirst(IFormatProvider provider)
        {
            var formatInfo = (DateTimeFormatInfo)provider.GetFormat(typeof(DateTimeFormatInfo));
            return Regex.IsMatch(formatInfo.ShortDatePattern, @"^.*y.*M.*d.*$");
        }

        private static readonly string[] Months = { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };

        private static int ParseMonth(string s)
        {
            for (int i = 0; i < Months.Length; i++)
            {
                if (s.StartsWith(Months[i], StringComparison.InvariantCultureIgnoreCase))
                    return i + 1;
            }

            throw new FormatException();
        }

        private static DayOfWeek ParseDayOfWeek(string s)
        {
            foreach (DayOfWeek day in Enum.GetValues(typeof(DayOfWeek)))
            {
                if (s.StartsWith(day.ToString().Substring(0, 3), StringComparison.InvariantCultureIgnoreCase))
                    return day;
            }

            throw new FormatException();
        }

        #endregion

        #region Public Methods

        public static DateTime ParseNatural(string str)
        {
            return ParseNatural(str, DateTime.Now);
        }

        public static DateTime ParseNatural(string str, DateTime referenceDate)
        {
            return ParseNatural(str, referenceDate, CultureInfo.CurrentCulture);
        }

        public static DateTime ParseNatural(string str, DateTime referenceDate, IFormatProvider provider)
        {
            // Null or empty input
            if (str == null)
                throw new ArgumentNullException("str");

            if (string.IsNullOrWhiteSpace(str))
                throw new FormatException();

            // Special dates
            if (Regex.IsMatch(str, @"^(nye?|new\s*year('?s)?(\s*eve)?)$", RegexOptions))
                return new DateTime(referenceDate.Year + 1, 1, 1);

            // Month only
            var monthRegex = new Regex(@"^(?<month>(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))[a-z]*$", RegexOptions);
            var monthMatch = monthRegex.Match(str);
            if (monthMatch.Success)
            {
                int month = ParseMonth(str);
                var dateTime = new DateTime(referenceDate.Year, month, 1);
                return dateTime < referenceDate ? dateTime.AddYears(1) : dateTime;
            }

            // Normalize noon, midday, or midnight
            str = Regex.Replace(str, @"(?<=(^|\s))(12(:00(:00)?)?\s*)?noon(?=($|\s))", "12:00:00 PM", RegexOptions);
            str = Regex.Replace(str, @"(?<=(^|\s))(12(:00(:00)?)?\s*)?mid(-?d)?ay(?=($|\s))", "12:00:00 PM", RegexOptions);
            str = Regex.Replace(str, @"(?<=(^|\s))(12(:00(:00)?)?\s*)?mid-?night(?=($|\s))", "12:00:00 AM", RegexOptions);

            // Normalize relative dates
            str = Regex.Replace(str, @"(?<=(^|\s))todd?ay(?=($|\s))", referenceDate.ToString("yyyy-MM-dd"), RegexOptions);
            str = Regex.Replace(str, @"(?<=(^|\s))tomm?orr?ow(?=($|\s))", referenceDate.AddDays(1).ToString("yyyy-MM-dd"), RegexOptions);

            // Try each format
            foreach (var format in GetNaturalFormats(provider))
            {
                try
                {
                    var match = Regex.Match(str, format, RegexOptions);
                    if (match.Success)
                    {
                        int day = referenceDate.Day, month = referenceDate.Month, year = referenceDate.Year;
                        int hour = 0, minute = 0, second = 0;

                        if (match.Groups["weekday"].Success)
                        {
                            // Parse weekday
                            var dayOfWeek = ParseDayOfWeek(match.Groups["weekday"].Value);

                            // Find next matching date
                            var nextMatchingDate = referenceDate.AddDays(1);
                            while (nextMatchingDate.DayOfWeek != dayOfWeek)
                                nextMatchingDate = nextMatchingDate.AddDays(1);

                            // Advance by one week if necessary
                            if (match.Groups["next"].Success)
                                nextMatchingDate = nextMatchingDate.AddDays(7);

                            // Advance to next week if necessary
                            if (match.Groups["nextweek"].Success && dayOfWeek > referenceDate.DayOfWeek)
                                nextMatchingDate = nextMatchingDate.AddDays(7);

                            day = nextMatchingDate.Day;
                            month = nextMatchingDate.Month;
                            year = nextMatchingDate.Year;
                        }
                        else
                        {
                            // Parse day
                            if (match.Groups["day"].Success)
                                day = int.Parse(match.Groups["day"].Value, provider);

                            // Parse month
                            if (match.Groups["month"].Success)
                            {
                                if (Regex.IsMatch(match.Groups["month"].Value, @"^\d+$", RegexOptions))
                                    month = int.Parse(match.Groups["month"].Value, provider);
                                else
                                    month = ParseMonth(match.Groups["month"].Value);
                            }

                            // Parse year
                            if (match.Groups["year"].Success)
                            {
                                year = int.Parse(match.Groups["year"].Value, provider);
                                if (year < 100)
                                    year += referenceDate.Year / 1000 * 1000;
                            }
                        }

                        // Parse hours
                        if (match.Groups["hour"].Success)
                            hour = int.Parse(match.Groups["hour"].Value, provider);

                        // Parse minutes
                        if (match.Groups["minute"].Success)
                            minute = int.Parse(match.Groups["minute"].Value, provider);

                        // Parse seconds
                        if (match.Groups["second"].Success)
                            second = int.Parse(match.Groups["second"].Value, provider);

                        // Parse AM/PM
                        if (match.Groups["ampm"].Success && match.Groups["ampm"].Value.StartsWith("p", StringComparison.InvariantCultureIgnoreCase) && hour != 12)
                            hour += 12;

                        // Fix reference to midnight as "12"
                        if ((!match.Groups["ampm"].Success || match.Groups["ampm"].Value.StartsWith("a", StringComparison.InvariantCultureIgnoreCase)) && hour == 12)
                            hour = 0;

                        var dateTime = new DateTime(year, month, day, hour, minute, second);

                        // Prefer output dates after reference date
                        if (dateTime <= referenceDate)
                        {
                            if (match.Groups["hour"].Success && !match.Groups["ampm"].Success && hour < 12 && dateTime.AddHours(12) > referenceDate)
                                dateTime = dateTime.AddHours(12);
                            else if (!match.Groups["day"].Success)
                                dateTime = dateTime.AddDays(1);
                            else if (!match.Groups["month"].Success)
                                dateTime = dateTime.AddMonths(1);
                            else if (!match.Groups["year"].Success)
                                dateTime = dateTime.AddYears(1);
                        }

                        // Prefer 8:00:00 AM too 7:59:59 PM for times without AM/PM except on the reference date
                        if (!match.Groups["ampm"].Success && dateTime.Date != referenceDate.Date && dateTime.TimeOfDay != TimeSpan.Zero && dateTime.Hour < 8)
                            dateTime = dateTime.AddHours(12);

                        return dateTime;
                    }
                }
                catch
                {
                }
            }

            throw new FormatException();
        }

        public static bool TryParseNatural(string str, out DateTime dateTime)
        {
            return TryParseNatural(str, DateTime.Now, out dateTime);
        }

        public static bool TryParseNatural(string str, DateTime referenceDate, out DateTime dateTime)
        {
            return TryParseNatural(str, referenceDate, CultureInfo.CurrentCulture, out dateTime);
        }

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

        public static string ToNaturalString(DateTime dateTime)
        {
            if (dateTime.Second != 0)
                return dateTime.ToString("d MMMM yyyy h:mm:ss tt");

            if (dateTime.Minute != 0 || dateTime.Hour != 0)
                return dateTime.ToString("d MMMM yyyy h:mm tt");

            return dateTime.ToString("d MMMM yyyy");
        }

        #endregion
    }
}
