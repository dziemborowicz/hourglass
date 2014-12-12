using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Hourglass
{
    public class DateTimeUtility
    {
        public static DateTime ParseNatural(string str)
        {
            // Null or empty input
            if (str == null)
                throw new ArgumentNullException("str");

            if (string.IsNullOrWhiteSpace(str))
                throw new FormatException();

            // Trim whitespace
            str = str.Trim(' ', '\t', '\r', '\n');

            // Built-in DateTime parsing
            DateTime dateTime;
            if (DateTime.TryParse(str, out dateTime))
                return dateTime;

            // New Year's Eve
            if (Regex.IsMatch(str, @"^(nye?|new year('?s)?( eve)?)$", RegexOptions.IgnoreCase))
                return new DateTime(DateTime.Today.Year + 1, 1, 1);

            throw new FormatException();
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
