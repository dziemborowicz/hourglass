// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SpecialTimeToken.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass.Parsing
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text.RegularExpressions;

    using Hourglass.Extensions;
    using Hourglass.Properties;

    /// <summary>
    /// Represents a special time of day.
    /// </summary>
    public enum SpecialTime
    {
        /// <summary>
        /// Represents 12 noon.
        /// </summary>
        Midday,

        /// <summary>
        /// Represents 12 midnight.
        /// </summary>
        Midnight
    }

    /// <summary>
    /// Represents a special time of day.
    /// </summary>
    public class SpecialTimeToken : TimeToken
    {
        /// <summary>
        /// The set of supported special times.
        /// </summary>
        private static readonly SpecialTimeDefinition[] SpecialTimes =
        {
            new SpecialTimeDefinition(
                SpecialTime.Midday,
                12 /* hour */,
                0 /* minute */,
                0 /* second */),

            new SpecialTimeDefinition(
                SpecialTime.Midnight,
                0 /* hour */,
                0 /* minute */,
                0 /* second */)
        };

        /// <summary>
        /// Gets or sets the <see cref="SpecialTime"/> represented by this token.
        /// </summary>
        public SpecialTime SpecialTime { get; set; }

        /// <summary>
        /// Gets a value indicating whether the token is valid.
        /// </summary>
        public override bool IsValid
        {
            get { return this.GetSpecialTimeDefinition() != null; }
        }

        /// <summary>
        /// Returns the next date and time after <paramref name="minDate"/> that is represented by this token.
        /// </summary>
        /// <remarks>
        /// This method may return a date and time that is before <paramref name="minDate"/> if there is no date and
        /// time after <paramref name="minDate"/> that is represented by this token.
        /// </remarks>
        /// <param name="minDate">The minimum date and time to return.</param>
        /// <param name="datePart">The date part of the date and time to return.</param>
        /// <returns>The next date and time after <paramref name="minDate"/> that is represented by this token.
        /// </returns>
        /// <exception cref="InvalidOperationException">If this token is not valid.</exception>
        public override DateTime ToDateTime(DateTime minDate, DateTime datePart)
        {
            this.ThrowIfNotValid();

            SpecialTimeDefinition specialTimeDefinition = this.GetSpecialTimeDefinition();

            return new DateTime(
                datePart.Year,
                datePart.Month,
                datePart.Day,
                specialTimeDefinition.Hour,
                specialTimeDefinition.Minute,
                specialTimeDefinition.Second);
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

                SpecialTimeDefinition specialTimeDefinition = this.GetSpecialTimeDefinition();
                return specialTimeDefinition.GetName(provider);
            }
            catch
            {
                return this.GetType().ToString();
            }
        }

        /// <summary>
        /// Returns the <see cref="SpecialTimeDefinition"/> object for a <see cref="Match"/>.
        /// </summary>
        /// <param name="match">A <see cref="Match"/>.</param>
        /// <returns>The <see cref="SpecialTimeDefinition"/> object for the <see cref="Match"/>.</returns>
        private static SpecialTimeDefinition GetSpecialTimeDefinitionForMatch(Match match)
        {
            return SpecialTimes.FirstOrDefault(e => match.Groups[e.MatchGroup].Success);
        }

        /// <summary>
        /// Returns the <see cref="SpecialTimeDefinition"/> object for this part.
        /// </summary>
        /// <returns>The <see cref="SpecialTimeDefinition"/> object for this part.</returns>
        private SpecialTimeDefinition GetSpecialTimeDefinition()
        {
            return SpecialTimes.FirstOrDefault(e => e.SpecialTime == this.SpecialTime);
        }

        /// <summary>
        /// Parses <see cref="SpecialTimeToken"/> strings.
        /// </summary>
        public new class Parser : TimeToken.Parser
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
                return SpecialTimes.Select(e => e.GetPattern(provider));
            }

            /// <summary>
            /// Parses a <see cref="Match"/> into a <see cref="TimeToken"/>.
            /// </summary>
            /// <param name="match">A <see cref="Match"/> representation of a <see cref="TimeToken"/>.</param>
            /// <param name="provider">An <see cref="IFormatProvider"/>.</param>
            /// <returns>The <see cref="TimeToken"/> parsed from the <see cref="Match"/>.</returns>
            /// <exception cref="ArgumentNullException">If <paramref name="match"/> or <paramref name="provider"/> is
            /// <c>null</c>.</exception>
            /// <exception cref="FormatException">If the <paramref name="match"/> is not a supported representation of
            /// a <see cref="TimeToken"/>.</exception>
            protected override TimeToken ParseInternal(Match match, IFormatProvider provider)
            {
                SpecialTimeDefinition specialTimeDefinition = GetSpecialTimeDefinitionForMatch(match);

                if (specialTimeDefinition == null)
                {
                    throw new FormatException();
                }

                return new SpecialTimeToken { SpecialTime = specialTimeDefinition.SpecialTime };
            }
        }

        /// <summary>
        /// Defines a special time of day.
        /// </summary>
        private class SpecialTimeDefinition
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="SpecialTimeDefinition"/> class.
            /// </summary>
            /// <param name="specialTime">The <see cref="SpecialTime"/>.</param>
            /// <param name="hour">The hour.</param>
            /// <param name="minute">The minute.</param>
            /// <param name="second">The second.</param>
            public SpecialTimeDefinition(SpecialTime specialTime, int hour, int minute, int second)
            {
                this.SpecialTime = specialTime;

                this.Hour = hour;
                this.Minute = minute;
                this.Second = second;

                this.MatchGroup = specialTime.ToString();
            }

            /// <summary>
            /// Gets the <see cref="SpecialTime"/>.
            /// </summary>
            public SpecialTime SpecialTime { get; private set; }

            /// <summary>
            /// Gets the hour.
            /// </summary>
            public int Hour { get; private set; }

            /// <summary>
            /// Gets the minute.
            /// </summary>
            public int Minute { get; private set; }

            /// <summary>
            /// Gets the second.
            /// </summary>
            public int Second { get; private set; }

            /// <summary>
            /// Gets the name of the regular expression match group that identifies the special time in a match.
            /// </summary>
            public string MatchGroup { get; private set; }

            /// <summary>
            /// Returns the friendly name for the special time.
            /// </summary>
            /// <param name="provider">An <see cref="IFormatProvider"/>.</param>
            /// <returns>The friendly name for the special time.</returns>
            public string GetName(IFormatProvider provider)
            {
                string resourceName = string.Format(
                    CultureInfo.InvariantCulture,
                    "SpecialTimeToken{0}Name",
                    this.SpecialTime);

                return Resources.ResourceManager.GetString(resourceName, provider);
            }

            /// <summary>
            /// Returns the regular expression that matches the special time.
            /// </summary>
            /// <param name="provider">An <see cref="IFormatProvider"/>.</param>
            /// <returns>The regular expression that matches the special time.</returns>
            public string GetPattern(IFormatProvider provider)
            {
                string resourceName = string.Format(
                    CultureInfo.InvariantCulture,
                    "SpecialTimeToken{0}Pattern",
                    this.SpecialTime);

                string pattern = Resources.ResourceManager.GetString(resourceName, provider);
                return string.Format(CultureInfo.InvariantCulture, @"(?<{0}>{1})", this.SpecialTime, pattern);
            }
        }
    }
}
