using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Hourglass
{
    public class TimeSpanUtility
    {
        public static TimeSpan ParseNatural(string str)
        {
            return ParseNatural(str, CultureInfo.CurrentCulture);
        }

        public static TimeSpan ParseNatural(string str, IFormatProvider provider)
        {
            TimeSpan timeSpan;
            if (!TryParseNatural(str, provider, out timeSpan))
                throw new FormatException();
            return timeSpan;
        }

        public static bool TryParseNatural(string str, out TimeSpan timeSpan)
        {
            return TryParseNatural(str, CultureInfo.CurrentCulture, out timeSpan);
        }

        public static bool TryParseNatural(string str, IFormatProvider provider, out TimeSpan timeSpan)
        {
            timeSpan = TimeSpan.Zero;

            // Null or empty input
            if (string.IsNullOrWhiteSpace(str))
                return false;

            // Trim whitespace
            str = str.Trim(' ', '\t', '\r', '\n');

            // Integer input
            if (Regex.IsMatch(str, @"^\d+$"))
            {
                int minutes;
                if (!int.TryParse(str, out minutes))
                    return false;
                timeSpan = new TimeSpan(0, minutes, 0);
                return true;
            }

            // Multi-part input
            string[] parts;
            if (Regex.IsMatch(str, @"^[\d.,;:]+$"))
                parts = Regex.Split(str, @"[.,;:]");
            else
                parts = Regex.Split(str, @"\s+(?=[+\-\d\.])|(?<![+\-\d\.])(?=[+\-\d\.])");

            // Get rid of empty parts
            parts = parts.Where(s => !string.IsNullOrEmpty(s)).ToArray();

            // Get values
            var values = new double[parts.Length];
            for (int i = 0; i < parts.Length; i++)
            {
                var part = parts[i];
                var match = Regex.Match(part, @"^[+\-]?\d+(\.\d*)?|^[+\-]?\.\d+");
                if (match.Success)
                {
                    if (!double.TryParse(match.Value, out values[i]))
                        return false;
                }
                else
                    return false;
            }

            // Get explicit units
            var units = new int[parts.Length];
            for (int i = 0; i < parts.Length; i++)
            {
                var part = parts[i];
                if (Regex.IsMatch(part, @"^([+-])?(\d+(\.\d*)?|\.\d+?)\s*(d|dys?|days?)$"))
                    units[i] = 24 * 60 * 60;
                else if (Regex.IsMatch(part, @"^([+-])?(\d+(\.\d*)?|\.\d+?)\s*(h|hrs?|hours?)$"))
                    units[i] = 60 * 60;
                else if (Regex.IsMatch(part, @"^([+-])?(\d+(\.\d*)?|\.\d+?)\s*(m|mins?|minutes?)$"))
                    units[i] = 60;
                else if (Regex.IsMatch(part, @"^([+-])?(\d+(\.\d*)?|\.\d+?)\s*(s|secs?|seconds?)$"))
                    units[i] = 1;
                else if (Regex.IsMatch(part, @"^([+-])?(\d+(\.\d*)?|\.\d+?)$"))
                    units[i] = 0;
                else
                    return false;
            }

            // Fill units implicitly left
            int lastUnit = units[0];
            for (int i = 1; i < units.Length; i++)
            {
                if (units[i] == 0)
                {
                    if (lastUnit == 24 * 60 * 60)
                        units[i] = 60 * 60;
                    else if (lastUnit == 60 * 60)
                        units[i] = 60;
                    else if (lastUnit == 60)
                        units[i] = 1;
                    else if (lastUnit != 0)
                        return false;
                }
                lastUnit = units[i];
            }

            // Fill units positionally if required
            if (lastUnit == 0)
                lastUnit = units[units.Length - 1] = 1;

            // Fill units implicitly right
            for (int i = units.Length - 2; i >= 0; i--)
            {
                if (units[i] == 0)
                {
                    if (lastUnit == 1)
                        units[i] = 60;
                    else if (lastUnit == 60)
                        units[i] = 60 * 60;
                    else if (lastUnit == 60 * 60)
                        units[i] = 24 * 60 * 60;
                    else if (lastUnit != 0)
                        return false;
                }
                lastUnit = units[i];
            }

            // Calculate time
            long ticks = 0L;
            for (int i = 0; i < parts.Length; i++)
            {
                ticks += (long)(values[i] * units[i] * 10000000L);
            }
            timeSpan = new TimeSpan(ticks);
            return true;
        }

        public static string ToNaturalString(TimeSpan timeSpan)
        {
            // Reject negative values
            if (timeSpan.Ticks < 0)
                throw new ArgumentOutOfRangeException("timeSpan", @"timeSpan must be at least zero.");

            // Breakdown time interval
            long totalSeconds = timeSpan.Ticks / 10000000L;
            long days = totalSeconds / 60 / 60 / 24;
            long hours = totalSeconds / 60 / 60 - days * 24;
            long minutes = totalSeconds / 60 - days * 24 * 60 - hours * 60;
            long seconds = totalSeconds % 60;

            // Build string
            var stringBuilder = new StringBuilder();
            if (days == 1)
                stringBuilder.AppendFormat("{0} day ", days);
            else if (days != 0)
                stringBuilder.AppendFormat("{0} days ", days);

            if (hours == 1)
                stringBuilder.AppendFormat("{0} hour ", hours);
            else if (hours != 0 || days != 0)
                stringBuilder.AppendFormat("{0} hours ", hours);

            if (minutes == 1)
                stringBuilder.AppendFormat("{0} minute ", minutes);
            else if (minutes != 0 || hours != 0 || days != 0)
                stringBuilder.AppendFormat("{0} minutes ", minutes);

            if (seconds == 1)
                stringBuilder.AppendFormat("{0} second", seconds);
            else
                stringBuilder.AppendFormat("{0} seconds", seconds);

            return stringBuilder.ToString();
        }

        public static string ToShortNaturalString(TimeSpan timeSpan)
        {
            // Reject negative values
            if (timeSpan.Ticks < 0)
                throw new ArgumentOutOfRangeException("timeSpan", @"timeSpan must be at least zero.");

            // Breakdown time interval
            long totalSeconds = timeSpan.Ticks / 10000000L;
            long days = totalSeconds / 60 / 60 / 24;
            long hours = totalSeconds / 60 / 60 - days * 24;
            long minutes = totalSeconds / 60 - days * 24 * 60 - hours * 60;
            long seconds = totalSeconds % 60;

            // Build string
            var stringBuilder = new StringBuilder();
            if (days == 1)
                stringBuilder.AppendFormat("{0} day ", days);
            else if (days != 0)
                stringBuilder.AppendFormat("{0} days ", days);

            if (hours == 1)
                stringBuilder.AppendFormat("{0} hour ", hours);
            else if (hours != 0)
                stringBuilder.AppendFormat("{0} hours ", hours);

            if (minutes == 1)
                stringBuilder.AppendFormat("{0} minute ", minutes);
            else if (minutes != 0)
                stringBuilder.AppendFormat("{0} minutes ", minutes);

            if (seconds == 1)
                stringBuilder.AppendFormat("{0} second ", seconds);
            else if (seconds != 0 || (days == 0 && hours == 0 && minutes == 0))
                stringBuilder.AppendFormat("{0} seconds ", seconds);

            stringBuilder.Remove(stringBuilder.Length - 1, 1);
            return stringBuilder.ToString();
        }
    }
}
