// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DateTimeToken.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass.Parsing
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    using Hourglass.Extensions;
    using Hourglass.Properties;

    /// <summary>
    /// Represents an instant in time.
    /// </summary>
    public class DateTimeToken : TimerStartToken
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DateTimeToken"/> class.
        /// </summary>
        public DateTimeToken()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DateTimeToken"/> class.
        /// </summary>
        /// <param name="dateToken">The date part of an instant in time.</param>
        /// <param name="timeToken">The time part of an instant in time.</param>
        public DateTimeToken(DateToken dateToken, TimeToken timeToken)
        {
            this.DateToken = dateToken;
            this.TimeToken = timeToken;
        }

        /// <summary>
        /// Gets or sets the date part of the date and time represented by this token.
        /// </summary>
        public DateToken DateToken { get; set; }

        /// <summary>
        /// Gets or sets the time part of the date and time represented by this token.
        /// </summary>
        public TimeToken TimeToken { get; set; }

        /// <summary>
        /// Gets a value indicating whether the token is valid.
        /// </summary>
        public override bool IsValid
        {
            get
            {
                return this.DateToken != null && this.DateToken.IsValid
                    && this.TimeToken != null && this.TimeToken.IsValid;
            }
        }

        /// <summary>
        /// Returns the end time for a timer started with this token at a specified time.
        /// </summary>
        /// <param name="startTime">The time the timer is started.</param>
        /// <returns>The end time for a timer started with this token at the specified time.</returns>
        public override DateTime GetEndTime(DateTime startTime)
        {
            this.ThrowIfNotValid();

            DateTime datePart = this.DateToken.ToDateTime(startTime, true /* inclusive */);
            DateTime dateTime = this.TimeToken.ToDateTime(startTime, datePart);

            if (dateTime <= startTime)
            {
                datePart = this.DateToken.ToDateTime(startTime, false /* inclusive */);
                dateTime = this.TimeToken.ToDateTime(startTime, datePart);
            }

            if (dateTime < startTime)
            {
                throw new InvalidOperationException();
            }

            return dateTime;
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

                string datePart = this.DateToken.ToString(provider);
                string timePart = this.TimeToken.ToString(provider);

                // Date and time
                if (!string.IsNullOrWhiteSpace(datePart) && !string.IsNullOrWhiteSpace(timePart))
                {
                    return string.Format(
                        Resources.ResourceManager.GetEffectiveProvider(provider),
                        Resources.ResourceManager.GetString("DateTimeTokenDateTimeFormatString", provider),
                        datePart,
                        timePart);
                }

                // Date only
                if (!string.IsNullOrWhiteSpace(datePart))
                {
                    return string.Format(
                        Resources.ResourceManager.GetEffectiveProvider(provider),
                        Resources.ResourceManager.GetString("DateTimeTokenDateOnlyFormatString", provider),
                        datePart);
                }

                // Time only
                if (!string.IsNullOrWhiteSpace(timePart))
                {
                    return string.Format(
                        Resources.ResourceManager.GetEffectiveProvider(provider),
                        Resources.ResourceManager.GetString("DateTimeTokenTimeOnlyFormatString", provider),
                        timePart);
                }

                // Empty
                return string.Empty;
            }
            catch
            {
                return this.GetType().ToString();
            }
        }

        /// <summary>
        /// Parses <see cref="DateTimeToken"/> strings.
        /// </summary>
        public new class Parser : TimerStartToken.Parser
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
            /// Parses a string into a <see cref="TimerStartToken"/>.
            /// </summary>
            /// <param name="str">A string representation of a <see cref="TimerStartToken"/>.</param>
            /// <param name="provider">An <see cref="IFormatProvider"/>.</param>
            /// <returns>The <see cref="TimerStartToken"/> parsed from the string.</returns>
            /// <exception cref="ArgumentNullException">If <paramref name="str"/> or <paramref name="provider"/> is
            /// <c>null</c>.</exception>
            /// <exception cref="FormatException">If <paramref name="str"/> is not a supported representation of a <see
            /// cref="TimerStartToken"/>.</exception>
            protected override TimerStartToken ParseInternal(string str, IFormatProvider provider)
            {
                foreach (PatternDefinition patternDefinition in GetAllDateTimePatternDefinitions(provider))
                {
                    try
                    {
                        Match match = Regex.Match(str, patternDefinition.Pattern, RegexOptions);
                        if (match.Success)
                        {
                            DateTimeToken dateTimeToken = new DateTimeToken(
                                patternDefinition.DateTokenParser.Parse(match, provider),
                                patternDefinition.TimeTokenParser.Parse(match, provider));

                            if (dateTimeToken.IsValid)
                            {
                                return dateTimeToken;
                            }
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
            /// Returns a list of <see cref="PatternDefinition"/> objects representing the patterns supported by the
            /// combination of each of the <see cref="DateToken.Parser"/>s and <see cref="TimeToken.Parser"/>s, but
            /// favoring <see cref="PatternDefinition"/> objects where one of the parsers is the <see
            /// cref="EmptyTimeToken.Parser"/> or the <see cref="EmptyDateToken.Parser"/>.
            /// </summary>
            /// <param name="provider">The <see cref="IFormatProvider"/> that will be used when parsing.</param>
            /// <returns>A list of <see cref="PatternDefinition"/> objects.</returns>
            private static List<PatternDefinition> GetAllDateTimePatternDefinitions(IFormatProvider provider)
            {
                List<PatternDefinition> list = new List<PatternDefinition>();
                list.AddRange(GetDatePatternDefinitions(provider));
                list.AddRange(GetTimePatternDefinitions(provider));
                list.AddRange(GetDateTimePatternDefinitions(provider));
                return list;
            }

            /// <summary>
            /// Returns a list of <see cref="PatternDefinition"/> objects representing the patterns supported by each
            /// of the <see cref="DateToken.Parser"/>s alone.
            /// </summary>
            /// <param name="provider">The <see cref="IFormatProvider"/> that will be used when parsing.</param>
            /// <returns>A list of <see cref="PatternDefinition"/> objects.</returns>
            private static List<PatternDefinition> GetDatePatternDefinitions(IFormatProvider provider)
            {
                List<PatternDefinition> list = new List<PatternDefinition>();

                foreach (DateToken.Parser dateTokenParser in DateToken.Parsers)
                {
                    list.AddRange(GetDateTimePatternDefinitions(dateTokenParser, EmptyTimeToken.Parser.Instance, provider));
                }

                return list;
            }

            /// <summary>
            /// Returns a list of <see cref="PatternDefinition"/> objects representing the patterns supported by each
            /// of the <see cref="TimeToken.Parser"/>s alone.
            /// </summary>
            /// <param name="provider">The <see cref="IFormatProvider"/> that will be used when parsing.</param>
            /// <returns>A list of <see cref="PatternDefinition"/> objects.</returns>
            private static List<PatternDefinition> GetTimePatternDefinitions(IFormatProvider provider)
            {
                List<PatternDefinition> list = new List<PatternDefinition>();

                foreach (TimeToken.Parser timeTokenParser in TimeToken.Parsers)
                {
                    list.AddRange(GetDateTimePatternDefinitions(EmptyDateToken.Parser.Instance, timeTokenParser, provider));
                }

                return list;
            }

            /// <summary>
            /// Returns a list of <see cref="PatternDefinition"/> objects representing the patterns supported by the
            /// combination of each of the <see cref="DateToken.Parser"/>s and <see cref="TimeToken.Parser"/>s.
            /// </summary>
            /// <param name="provider">The <see cref="IFormatProvider"/> that will be used when parsing.</param>
            /// <returns>A list of <see cref="PatternDefinition"/> objects.</returns>
            private static List<PatternDefinition> GetDateTimePatternDefinitions(IFormatProvider provider)
            {
                List<PatternDefinition> list = new List<PatternDefinition>();

                foreach (DateToken.Parser dateTokenParser in DateToken.Parsers)
                {
                    foreach (TimeToken.Parser timeTokenParser in TimeToken.Parsers)
                    {
                        list.AddRange(GetDateTimePatternDefinitions(dateTokenParser, timeTokenParser, provider));
                    }
                }

                return list;
            }

            /// <summary>
            /// Returns a list of <see cref="PatternDefinition"/> objects representing the patterns supported by the
            /// combination of the specified <see cref="DateToken.Parser"/> and <see cref="TimeToken.Parser"/>.
            /// </summary>
            /// <param name="dateTokenParser">A <see cref="DateToken.Parser"/>.</param>
            /// <param name="timeTokenParser">A <see cref="TimeToken.Parser"/>.</param>
            /// <param name="provider">The <see cref="IFormatProvider"/> that will be used when parsing.</param>
            /// <returns>A list of <see cref="PatternDefinition"/> objects.</returns>
            private static List<PatternDefinition> GetDateTimePatternDefinitions(DateToken.Parser dateTokenParser, TimeToken.Parser timeTokenParser, IFormatProvider provider)
            {
                if (!dateTokenParser.IsCompatibleWith(timeTokenParser) || !timeTokenParser.IsCompatibleWith(dateTokenParser))
                {
                    return new List<PatternDefinition>();
                }

                List<PatternDefinition> list = new List<PatternDefinition>();

                foreach (string datePartPattern in dateTokenParser.GetPatterns(provider))
                {
                    foreach (string timePartPattern in timeTokenParser.GetPatterns(provider))
                    {
                        string dateTimePattern = GetDateTimePattern(datePartPattern, timePartPattern, provider);
                        list.Add(new PatternDefinition(dateTokenParser, timeTokenParser, dateTimePattern));

                        string timeDatePattern = GetTimeDatePattern(timePartPattern, datePartPattern, provider);
                        list.Add(new PatternDefinition(dateTokenParser, timeTokenParser, timeDatePattern));
                    }
                }

                return list;
            }

            /// <summary>
            /// Returns a regular expression that is the concatenation of a date token pattern and a time token
            /// pattern, with an appropriate separator.
            /// </summary>
            /// <param name="datePartPattern">A date part regular expression.</param>
            /// <param name="timePartPattern">A time part regular expression.</param>
            /// <param name="provider">The <see cref="IFormatProvider"/> that will be used when parsing.</param>
            /// <returns>A regular expression that is the concatenation of a date token pattern and a time token
            /// pattern.</returns>
            private static string GetDateTimePattern(string datePartPattern, string timePartPattern, IFormatProvider provider)
            {
                if (string.IsNullOrWhiteSpace(datePartPattern) && string.IsNullOrWhiteSpace(timePartPattern))
                {
                    throw new ArgumentException();
                }

                // Date pattern only
                if (string.IsNullOrWhiteSpace(timePartPattern))
                {
                    return string.Format(
                        Resources.ResourceManager.GetEffectiveProvider(provider),
                        Resources.ResourceManager.GetString("DateTimeTokenDateOnlyPatternFormatString", provider),
                        datePartPattern);
                }

                // Time pattern only
                if (string.IsNullOrWhiteSpace(datePartPattern))
                {
                    return string.Format(
                        Resources.ResourceManager.GetEffectiveProvider(provider),
                        Resources.ResourceManager.GetString("DateTimeTokenTimeOnlyPatternFormatString", provider),
                        timePartPattern);
                }

                // Date and time pattern
                return string.Format(
                    Resources.ResourceManager.GetEffectiveProvider(provider),
                    Resources.ResourceManager.GetString("DateTimeTokenDateTimePatternFormatString", provider),
                    datePartPattern,
                    timePartPattern);
            }

            /// <summary>
            /// Returns a regular expression that is the concatenation of a time token pattern and a date token
            /// pattern, with an appropriate separator.
            /// </summary>
            /// <param name="timePartPattern">A time part regular expression.</param>
            /// <param name="datePartPattern">A date part regular expression.</param>
            /// <param name="provider">The <see cref="IFormatProvider"/> that will be used when parsing.</param>
            /// <returns>A regular expression that is the concatenation of a time token pattern and a date token
            /// pattern.</returns>
            private static string GetTimeDatePattern(string timePartPattern, string datePartPattern, IFormatProvider provider)
            {
                if (string.IsNullOrWhiteSpace(timePartPattern) && string.IsNullOrWhiteSpace(datePartPattern))
                {
                    throw new ArgumentException();
                }

                // Time pattern only
                if (string.IsNullOrWhiteSpace(datePartPattern))
                {
                    return string.Format(
                        Resources.ResourceManager.GetEffectiveProvider(provider),
                        Resources.ResourceManager.GetString("DateTimeTokenTimeOnlyPatternFormatString", provider),
                        timePartPattern);
                }

                // Date pattern only
                if (string.IsNullOrWhiteSpace(timePartPattern))
                {
                    return string.Format(
                        Resources.ResourceManager.GetEffectiveProvider(provider),
                        Resources.ResourceManager.GetString("DateTimeTokenDateOnlyPatternFormatString", provider),
                        datePartPattern);
                }

                // Time and date pattern
                return string.Format(
                    Resources.ResourceManager.GetEffectiveProvider(provider),
                    Resources.ResourceManager.GetString("DateTimeTokenTimeDatePatternFormatString", provider),
                    timePartPattern,
                    datePartPattern);
            }

            /// <summary>
            /// Defines a pattern that matches a <see cref="DateTimeToken"/>.
            /// </summary>
            private class PatternDefinition
            {
                /// <summary>
                /// Initializes a new instance of the <see cref="PatternDefinition"/> class.
                /// </summary>
                /// <param name="dateTokenParser">The <see cref="DateToken.Parser"/> for the date token part of the
                /// pattern.</param>
                /// <param name="timeTokenParser">The <see cref="TimeToken.Parser"/> for the time token part of the
                /// pattern.</param>
                /// <param name="pattern">The regular expression that matches a <see cref="DateTimeToken"/>.</param>
                public PatternDefinition(DateToken.Parser dateTokenParser, TimeToken.Parser timeTokenParser, string pattern)
                {
                    this.DateTokenParser = dateTokenParser;
                    this.TimeTokenParser = timeTokenParser;
                    this.Pattern = pattern;
                }

                /// <summary>
                /// Gets the <see cref="DateToken.Parser"/> for the date token part of the pattern.
                /// </summary>
                public DateToken.Parser DateTokenParser { get; private set; }

                /// <summary>
                /// Gets the <see cref="TimeToken.Parser"/> for the time token part of the pattern.
                /// </summary>
                public TimeToken.Parser TimeTokenParser { get; private set; }

                /// <summary>
                /// Gets the regular expression that matches a <see cref="DateTimeToken"/>.
                /// </summary>
                public string Pattern { get; private set; }
            }
        }
    }
}
