// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DateTimePart.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass.Parsing
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Represents part of a date and time.
    /// </summary>
    public class DateTimePart
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DateTimePart"/> class.
        /// </summary>
        public DateTimePart()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DateTimePart"/> class.
        /// </summary>
        /// <param name="datePart">The date part of the date and time represented by this part.</param>
        /// <param name="timePart">The time part of the date and time represented by this part.</param>
        public DateTimePart(DatePart datePart, TimePart timePart)
        {
            this.DatePart = datePart;
            this.TimePart = timePart;
        }

        /// <summary>
        /// Gets or sets the date part of the date and time represented by this part.
        /// </summary>
        public DatePart DatePart { get; set; }

        /// <summary>
        /// Gets or sets the time part of the date and time represented by this part.
        /// </summary>
        public TimePart TimePart { get; set; }

        /// <summary>
        /// Gets a value indicating whether the part is valid.
        /// </summary>
        public bool IsValid
        {
            get
            {
                return this.DatePart != null && this.DatePart.IsValid
                    && this.TimePart != null && this.TimePart.IsValid;
            }
        }

        /// <summary>
        /// Returns a concrete date and time represented by this part.
        /// </summary>
        /// <param name="referenceDate">A reference date and time.</param>
        /// <returns>A concrete date and time represented by this part.</returns>
        public DateTime ToDateTime(DateTime referenceDate)
        {
            this.ThrowIfNotValid();

            DateTime datePart = this.DatePart.ToDateTime(referenceDate, false /* tryExcludeReferenceDate */);
            DateTime dateTime = this.TimePart.ToDateTime(referenceDate, datePart);

            if (dateTime <= referenceDate)
            {
                datePart = this.DatePart.ToDateTime(referenceDate, true /* tryExcludeReferenceDate */);
                dateTime = this.TimePart.ToDateTime(referenceDate, datePart);
            }

            return dateTime;
        }

        /// <summary>
        /// Returns a concrete date and time represented by this part.
        /// </summary>
        /// <param name="referenceDate">A reference date and time.</param>
        /// <param name="dateTime">A concrete date and time represented by this part.</param>
        /// <returns><c>true</c> if the <see cref="DateTimePart"/> was successfully converted to a concrete date and
        /// time, or <c>false</c> otherwise.</returns>
        public bool TryToDateTime(DateTime referenceDate, out DateTime dateTime)
        {
            try
            {
                dateTime = this.ToDateTime(referenceDate);
                return true;
            }
            catch
            {
                dateTime = DateTime.MinValue;
                return false;
            }
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

                string datePart = this.DatePart.ToString();
                string timePart = this.TimePart.ToString();

                // Date and time
                if (!string.IsNullOrWhiteSpace(datePart) && !string.IsNullOrWhiteSpace(timePart))
                {
                    return string.Format("{0} at {1}", this.DatePart, this.TimePart);
                }
                
                // Date only
                if (!string.IsNullOrWhiteSpace(datePart))
                {
                    return datePart;
                }

                // Time only
                if (!string.IsNullOrWhiteSpace(timePart))
                {
                    return string.Format("until {0}", timePart);
                }
                
                // Empty
                return string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Throws an <see cref="InvalidOperationException"/> if <see cref="IsValid"/> is <c>false</c>.
        /// </summary>
        protected void ThrowIfNotValid()
        {
            if (!this.IsValid)
            {
                throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Parses <see cref="DateTimePart"/>s from <see cref="string"/>s.
        /// </summary>
        public class Parser
        {
            /// <summary>
            /// Singleton instance of the <see cref="Parser"/> class.
            /// </summary>
            public static readonly Parser Instance = new Parser();

            /// <summary>
            /// The <see cref="RegexOptions"/> used by this class by default when matching a regular expression.
            /// </summary>
            public static readonly RegexOptions RegexOptions =
                RegexOptions.CultureInvariant |
                RegexOptions.IgnoreCase |
                RegexOptions.IgnorePatternWhitespace;

            /// <summary>
            /// Prevents a default instance of the <see cref="Parser"/> class from being created.
            /// </summary>
            private Parser()
            {
            }

            /// <summary>
            /// Parses a <see cref="string"/> representation of a date and time into a <see cref="DateTimePart"/>.
            /// </summary>
            /// <remarks>
            /// This overload uses the <see cref="CultureInfo.CurrentCulture"/> as the <see cref="IFormatProvider"/> when
            /// parsing.
            /// </remarks>
            /// <param name="str">A <see cref="string"/> representation of a date and time.</param>
            /// <returns>A <see cref="DateTimePart"/> representation of the date and time.</returns>
            /// <exception cref="ArgumentNullException">If <paramref name="str"/> is <c>null</c>.</exception>
            /// <exception cref="FormatException">If <paramref name="str"/> is not a valid representation of a date and
            /// time.</exception>
            public DateTimePart Parse(string str)
            {
                return this.Parse(str, CultureInfo.CurrentCulture);
            }

            /// <summary>
            /// Parses a <see cref="string"/> representation of a date and time into a <see cref="DateTimePart"/>.
            /// </summary>
            /// <param name="str">A <see cref="string"/> representation of a date and time.</param>
            /// <param name="provider">An <see cref="IFormatProvider"/> to use when parsing.</param>
            /// <returns>A <see cref="DateTimePart"/> representation of the date and time.</returns>
            /// <exception cref="ArgumentNullException">If <paramref name="str"/> is <c>null</c>.</exception>
            /// <exception cref="FormatException">If <paramref name="str"/> is not a valid representation of a date and
            /// time.</exception>
            public DateTimePart Parse(string str, IFormatProvider provider)
            {
                foreach (DateTimePattern parserPatternSet in GetParserPatternSets(provider))
                {
                    try
                    {
                        Match match = Regex.Match(str, parserPatternSet.Pattern, RegexOptions);
                        if (match.Success)
                        {
                            return new DateTimePart(
                                parserPatternSet.DatePartParser.Parse(match, provider),
                                parserPatternSet.TimePartParser.Parse(match, provider));
                        }
                    }
                    catch
                    {
                        // Try the next pattern set
                        continue;
                    }
                }

                // Could not find a matching pattern
                throw new FormatException();
            }

            /// <summary>
            /// Parses a <see cref="string"/> representation of a date and time into a <see cref="DateTimePart"/>.
            /// </summary>
            /// <remarks>
            /// This overload uses the <see cref="CultureInfo.CurrentCulture"/> as the <see cref="IFormatProvider"/>
            /// when parsing.
            /// </remarks>
            /// <param name="str">A <see cref="string"/> representation of a date and time.</param>
            /// <param name="dateTimePart">A <see cref="DateTime"/> representation of the date and time, or <c>null</c>
            /// if this method returns <c>false</c>.</param>
            /// <returns>A <see cref="DateTimePart"/> representation of the date and time.</returns>
            /// <returns><c>true</c> if <paramref name="str"/> was successfully parsed into a <see
            /// cref="DateTimePart"/>, or <c>false</c> otherwise.</returns>
            public bool TryParse(string str, out DateTimePart dateTimePart)
            {
                return this.TryParse(str, CultureInfo.CurrentCulture, out dateTimePart);
            }

            /// <summary>
            /// Parses a <see cref="string"/> representation of a date and time into a <see cref="DateTimePart"/>.
            /// </summary>
            /// <param name="str">A <see cref="string"/> representation of a date and time.</param>
            /// <param name="provider">An <see cref="IFormatProvider"/> to use when parsing.</param>
            /// <param name="dateTimePart">A <see cref="DateTime"/> representation of the date and time, or <c>null</c>
            /// if this method returns <c>false</c>.</param>
            /// <returns>A <see cref="DateTimePart"/> representation of the date and time.</returns>
            /// <returns><c>true</c> if <paramref name="str"/> was successfully parsed into a <see
            /// cref="DateTimePart"/>, or <c>false</c> otherwise.</returns>
            public bool TryParse(string str, IFormatProvider provider, out DateTimePart dateTimePart)
            {
                try
                {
                    dateTimePart = this.Parse(str, provider);
                    return true;
                }
                catch
                {
                    dateTimePart = null;
                    return false;
                }
            }

            /// <summary>
            /// Returns a list of <see cref="DateTimePattern"/> objects representing the patterns supported by each
            /// combination of the <see cref="DatePart.Parser"/>s and the <see cref="TimePart.Parser"/>s, but favoring
            /// <see cref="DateTimePattern"/> objects where one of the parsers is the <see cref="EmptyTimePart.Parser"/>
            /// or the <see cref="EmptyDatePart.Parser"/>.
            /// </summary>
            /// <param name="provider">The <see cref="IFormatProvider"/> that will be used when parsing.</param>
            /// <returns>A list of <see cref="DateTimePattern"/> objects.</returns>
            private static List<DateTimePattern> GetParserPatternSets(IFormatProvider provider)
            {
                List<DateTimePattern> list = new List<DateTimePattern>();
                list.AddRange(GetDatePatterns(provider));
                list.AddRange(GetTimePatterns(provider));
                list.AddRange(GetDateTimePatterns(provider));
                return list;
            }

            /// <summary>
            /// Returns a list of <see cref="DateTimePattern"/> objects representing the patterns supported by each of
            /// the <see cref="DatePart.Parser"/>s alone.
            /// </summary>
            /// <param name="provider">The <see cref="IFormatProvider"/> that will be used when parsing.</param>
            /// <returns>A list of <see cref="DateTimePattern"/> objects.</returns>
            private static List<DateTimePattern> GetDatePatterns(IFormatProvider provider)
            {
                List<DateTimePattern> list = new List<DateTimePattern>();

                foreach (DatePart.Parser datePartParser in DatePart.Parsers)
                {
                    list.AddRange(GetDateTimePatterns(datePartParser, EmptyTimePart.Parser.Instance, provider));
                }

                return list;
            }

            /// <summary>
            /// Returns a list of <see cref="DateTimePattern"/> objects representing the patterns supported by each of
            /// the <see cref="TimePart.Parser"/>s alone.
            /// </summary>
            /// <param name="provider">The <see cref="IFormatProvider"/> that will be used when parsing.</param>
            /// <returns>A list of <see cref="DateTimePattern"/> objects.</returns>
            private static List<DateTimePattern> GetTimePatterns(IFormatProvider provider)
            {
                List<DateTimePattern> list = new List<DateTimePattern>();

                foreach (TimePart.Parser timePartParser in TimePart.Parsers)
                {
                    list.AddRange(GetDateTimePatterns(EmptyDatePart.Parser.Instance, timePartParser, provider));
                }

                return list;
            }

            /// <summary>
            /// Returns a list of <see cref="DateTimePattern"/> objects representing the patterns supported by each
            /// combination of the <see cref="DatePart.Parser"/>s and the <see cref="TimePart.Parser"/>s.
            /// </summary>
            /// <param name="provider">The <see cref="IFormatProvider"/> that will be used when parsing.</param>
            /// <returns>A list of <see cref="DateTimePattern"/> objects.</returns>
            private static List<DateTimePattern> GetDateTimePatterns(IFormatProvider provider)
            {
                List<DateTimePattern> list = new List<DateTimePattern>();

                foreach (DatePart.Parser datePartParser in DatePart.Parsers)
                {
                    foreach (TimePart.Parser timePartParser in TimePart.Parsers)
                    {
                        list.AddRange(GetDateTimePatterns(datePartParser, timePartParser, provider));
                    }
                }

                return list;
            }

            /// <summary>
            /// Returns a list of <see cref="DateTimePattern"/> objects representing the patterns supported by the
            /// combination of the specified <see cref="DatePart.Parser"/> and <see cref="TimePart.Parser"/>.
            /// </summary>
            /// <param name="datePartParser">A <see cref="DatePart.Parser"/>.</param>
            /// <param name="timePartParser">A <see cref="TimePart.Parser"/>.</param>
            /// <param name="provider">The <see cref="IFormatProvider"/> that will be used when parsing.</param>
            /// <returns>A list of <see cref="DateTimePattern"/> objects.</returns>
            private static List<DateTimePattern> GetDateTimePatterns(DatePart.Parser datePartParser, TimePart.Parser timePartParser, IFormatProvider provider)
            {
                if (!datePartParser.IsCompatibleWith(timePartParser) || !timePartParser.IsCompatibleWith(datePartParser))
                {
                    return new List<DateTimePattern>();
                }

                List<DateTimePattern> list = new List<DateTimePattern>();

                foreach (string datePartPattern in datePartParser.GetPatterns(provider))
                {
                    foreach (string timePartPattern in timePartParser.GetPatterns(provider))
                    {
                        string fullDateTimePattern = GetDateTimePattern(datePartPattern, timePartPattern);
                        list.Add(new DateTimePattern(datePartParser, timePartParser, fullDateTimePattern));

                        string fullTimeDatePattern = GetTimeDatePattern(timePartPattern, datePartPattern);
                        list.Add(new DateTimePattern(datePartParser, timePartParser, fullTimeDatePattern));
                    }
                }

                return list;
            }

            /// <summary>
            /// Returns a regular expression that is the concatenation of a date part pattern and a time part pattern,
            /// with an appropriate separator.
            /// </summary>
            /// <param name="datePartPattern">A date part regular expression.</param>
            /// <param name="timePartPattern">A time part regular expression.</param>
            /// <returns>A regular expression that is the concatenation of a date part pattern and a time part pattern.
            /// </returns>
            private static string GetDateTimePattern(string datePartPattern, string timePartPattern)
            {
                if (string.IsNullOrWhiteSpace(datePartPattern) && string.IsNullOrWhiteSpace(timePartPattern))
                {
                    throw new ArgumentException();
                }

                // Date pattern only
                if (string.IsNullOrWhiteSpace(timePartPattern))
                {
                    return "^(" + datePartPattern + ")$";
                }

                // Time pattern only
                if (string.IsNullOrWhiteSpace(datePartPattern))
                {
                    return "^(" + timePartPattern + ")$";
                }

                // Date and time pattern
                return "^(" + datePartPattern + @")\s+(at\s+)?(" + timePartPattern + ")$";
            }

            /// <summary>
            /// Returns a regular expression that is the concatenation of a time part pattern and a date part pattern,
            /// with an appropriate separator.
            /// </summary>
            /// <param name="timePartPattern">A time part regular expression.</param>
            /// <param name="datePartPattern">A date part regular expression.</param>
            /// <returns>A regular expression that is the concatenation of a time part pattern and a date part pattern.
            /// </returns>
            private static string GetTimeDatePattern(string timePartPattern, string datePartPattern)
            {
                if (string.IsNullOrWhiteSpace(timePartPattern) && string.IsNullOrWhiteSpace(datePartPattern))
                {
                    throw new ArgumentException();
                }

                // Time pattern only
                if (string.IsNullOrWhiteSpace(datePartPattern))
                {
                    return "^(" + timePartPattern + ")$";
                }

                // Date pattern only
                if (string.IsNullOrWhiteSpace(timePartPattern))
                {
                    return "^(" + datePartPattern + ")$";
                }

                // Time and date pattern
                return "^(" + timePartPattern + @")\s+(on\s+)?(" + datePartPattern + ")$";
            }

            /// <summary>
            /// Represents a pattern that matches a <see cref="DateTimePart"/>.
            /// </summary>
            private class DateTimePattern
            {
                /// <summary>
                /// Initializes a new instance of the <see cref="DateTimePattern"/> class.
                /// </summary>
                /// <param name="datePartParser">The <see cref="DatePart.Parser"/> for the date part of the pattern.
                /// </param>
                /// <param name="timePartParser">The <see cref="TimePart.Parser"/> for the time part of the pattern.
                /// </param>
                /// <param name="pattern">The regular expression that matches a <see cref="DateTimePart"/>.</param>
                public DateTimePattern(DatePart.Parser datePartParser, TimePart.Parser timePartParser, string pattern)
                {
                    this.DatePartParser = datePartParser;
                    this.TimePartParser = timePartParser;
                    this.Pattern = pattern;
                }

                /// <summary>
                /// Gets the <see cref="DatePart.Parser"/> for the date part of the pattern.
                /// </summary>
                public DatePart.Parser DatePartParser { get; private set; }

                /// <summary>
                /// Gets the <see cref="TimePart.Parser"/> for the time part of the pattern.
                /// </summary>
                public TimePart.Parser TimePartParser { get; private set; }

                /// <summary>
                /// Gets the regular expression that matches a <see cref="DateTimePart"/>.
                /// </summary>
                public string Pattern { get; private set; }
            }
        }
    }
}
