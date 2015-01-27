using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Hourglass
{
    public static class DateTimeUtility
    {
        private static readonly string[] NaturalDateFormats =
        {
            @"((this|next)\s+)?(?<weekday>(Sun|Mon|Tue|Wed|Thu|Fri|Sat))\w*",
            @"(?<weekday>(Sun|Mon|Tue|Wed|Thu|Fri|Sat))\w*(\s+after)?\s+(?<next>next)",
            @"(?<day>\d\d?)(\s*(st|nd|rd|th))?(((\s+|-)of)?(\s+|-)(?<month>(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))\w*((\s+|-)(?<year>(\d\d)?\d\d))?)?",
            @"(?<month>(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))\w*(\s+|-)(?<day>\d\d?)(\s*(st|nd|rd|th))?(,?(\s+|-)(?<year>(\d\d)?\d\d))?",
            @"(((?<year>\d\d\d\d)[.\-/])?(?<month>\d\d?)[.\-/])?(?<day>\d\d?)",
            @"(?<day>\d\d?)([.\-/](?<month>\d\d?)([.\-/](?<year>(\d\d)?\d\d))?)?"
        };

        private static readonly string[] NaturalDateFormatsWithMonthFirst =
        {
            @"((this|next)\s+)?(?<weekday>(Sun|Mon|Tue|Wed|Thu|Fri|Sat))\w*",
            @"(?<weekday>(Sun|Mon|Tue|Wed|Thu|Fri|Sat))\w*(\s+after)?\s+(?<next>next)",
            @"(?<day>\d\d?)(\s*(st|nd|rd|th))?(((\s+|-)of)?(\s+|-)(?<month>(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))\w*((\s+|-)(?<year>(\d\d)?\d\d))?)?",
            @"(?<month>(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))\w*(\s+|-)(?<day>\d\d?)(\s*(st|nd|rd|th))?(,?(\s+|-)(?<year>(\d\d)?\d\d))?",
            @"(((?<year>\d\d\d\d)[.\-/])?(?<month>\d\d?)[.\-/])?(?<day>\d\d?)",
            @"(?<month>\d\d?)([.\-/](?<day>\d\d?)([.\-/](?<year>(\d\d)?\d\d))?)?"
        };

        private static readonly string[] NaturalTimeFormats =
        {
            @"(?<hour>\d\d?)([.:](?<minute>\d\d?)([.:](?<second>\d\d?))?)?\s*(?<ampm>((a|p).?\s*m.?|h\w*))?",
            @"(?<hour>\d\d?)((?<minute>\d\d)((?<second>\d\d))?)?\s*(?<ampm>((a|p).?\s*m.?|h\w*))?"
        };

        private static IEnumerable<string> GetNaturalFormats(IFormatProvider provider)
        {
            foreach (string tf in NaturalTimeFormats)
                yield return "^" + tf + "$";

            foreach (string df in IsMonthFirst(provider) ? NaturalDateFormatsWithMonthFirst : NaturalDateFormats)
                yield return "^" + df + "$";

            foreach (string df in IsMonthFirst(provider) ? NaturalDateFormatsWithMonthFirst : NaturalDateFormats)
                foreach (string tf in NaturalTimeFormats)
                {
                    yield return "^" + df + @"\s+(at\s+)?" + tf + "$";
                    yield return "^" + tf + @"\s+(on\s+)?" + df + "$";
                }
        }

        private static bool IsMonthFirst(IFormatProvider provider)
        {
            DateTimeFormatInfo fi = (DateTimeFormatInfo)provider.GetFormat(typeof(DateTimeFormatInfo));
            return Regex.IsMatch(fi.ShortDatePattern, @"^.*M.*d.*y.*$");
        }

        private static readonly string[] Months = new string[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };

        private static int ParseMonth(string s)
        {
            for (int i = 0; i < Months.Length; i++)
                if (s.StartsWith(Months[i], StringComparison.InvariantCultureIgnoreCase))
                    return i + 1;

            throw new FormatException();
        }

        private static DayOfWeek ParseDayOfWeek(string s)
        {
            foreach (DayOfWeek day in Enum.GetValues(typeof(DayOfWeek)))
                if (s.StartsWith(day.ToString().Substring(0, 3), StringComparison.InvariantCultureIgnoreCase))
                    return day;

            throw new FormatException();
        }

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
            if (Regex.IsMatch(str, @"^(nye?|new year('?s)?( eve)?)$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant))
                return new DateTime(referenceDate.Year + 1, 1, 1);

            // Normalize noon, midday, or midnight
            str = Regex.Replace(str, @"(?<=(^|\s))(12(:00(:00)?)?\s*)?noon(?=($|\s))", "12:00:00 PM", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
            str = Regex.Replace(str, @"(?<=(^|\s))(12(:00(:00)?)?\s*)?mid(-?d)?ay(?=($|\s))", "12:00:00 PM", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
            str = Regex.Replace(str, @"(?<=(^|\s))(12(:00(:00)?)?\s*)?mid-?night(?=($|\s))", "12:00:00 AM", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

            // Normalize relative dates
            str = Regex.Replace(str, @"(?<=(^|\s))todd?ay(?=($|\s))", referenceDate.ToString("yyyy-MM-dd"), RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
            str = Regex.Replace(str, @"(?<=(^|\s))tomm?orr?ow(?=($|\s))", referenceDate.AddDays(1).ToString("yyyy-MM-dd"), RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

            // Try each format
            foreach (string format in GetNaturalFormats(provider))
                try
                {
                    Match match = Regex.Match(str, format, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
                    if (match.Success)
                    {
                        int day = referenceDate.Day, month = referenceDate.Month, year = referenceDate.Year;
                        int hour = 0, minute = 0, second = 0;

                        if (match.Groups["weekday"].Success)
                        {
                            // Parse weekday
                            DayOfWeek dayOfWeek = ParseDayOfWeek(match.Groups["weekday"].Value);

                            DateTime nextMatchingDate = referenceDate.AddDays(1);
                            while (nextMatchingDate.DayOfWeek != dayOfWeek)
                                nextMatchingDate = nextMatchingDate.AddDays(1);

                            if (match.Groups["next"].Success)
                                nextMatchingDate = nextMatchingDate.AddDays(7);

                            day = nextMatchingDate.Day; month = nextMatchingDate.Month; year = nextMatchingDate.Year;
                        }
                        else
                        {
                            // Parse day
                            if (match.Groups["day"].Success)
                                day = int.Parse(match.Groups["day"].Value, provider);

                            // Parse month
                            if (match.Groups["month"].Success)
                            {
                                if (Regex.IsMatch(match.Groups["month"].Value, @"^\d+$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant))
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
                        if (match.Groups["ampm"].Success)
                        {
                            if (hour == 12 && match.Groups["ampm"].Value.StartsWith("a", StringComparison.InvariantCultureIgnoreCase))
                                hour = 0;
                            else if (hour != 12 && match.Groups["ampm"].Value.StartsWith("p", StringComparison.InvariantCultureIgnoreCase))
                                hour += 12;
                        }

                        DateTime dateTime = new DateTime(year, month, day, hour, minute, second);

                        // Prefer output dates after reference date
                        if (dateTime <= referenceDate)
                        {
                            if (match.Groups["hour"].Success && !match.Groups["ampm"].Success && hour != 0 && hour < 12 && dateTime.AddHours(12) > referenceDate)
                                dateTime = dateTime.AddHours(12);
                            else if (!match.Groups["day"].Success)
                                dateTime = dateTime.AddDays(1);
                            else if (!match.Groups["month"].Success)
                                dateTime = dateTime.AddMonths(1);
                            else if (!match.Groups["year"].Success)
                                dateTime = dateTime.AddYears(1);
                        }

                        return dateTime;
                    }
                }
                catch (Exception)
                {
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
            catch (FormatException)
            {
                dateTime = DateTime.MinValue;
                return false;
            }
        }

        public static string ToNaturalString(DateTime dateTime)
        {
            if (dateTime.Second != 0)
                return dateTime.ToString("d MMMM yyyy h:mm:ss tt");
            else if (dateTime.Minute != 0 || dateTime.Hour != 0)
                return dateTime.ToString("d MMMM yyyy h:mm tt");
            else
                return dateTime.ToString("d MMMM yyyy");
        }
    }
}
