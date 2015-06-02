// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DatePart.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass.Parsing
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using System.Xml.Serialization;

    /// <summary>
    /// Represents part of a date.
    /// </summary>
    [XmlInclude(typeof(DayOfWeekDatePart))]
    [XmlInclude(typeof(EmptyDatePart))]
    [XmlInclude(typeof(NewYearDatePart))]
    [XmlInclude(typeof(SpecialDatePart))]
    [XmlInclude(typeof(StandardDatePart))]
    [XmlInclude(typeof(TodayDatePart))]
    [XmlInclude(typeof(TomorrowDatePart))]
    public abstract class DatePart
    {
        /// <summary>
        /// Gets a list of all supported <see cref="Parser"/>s.
        /// </summary>
        public static Parser[] Parsers
        {
            get
            {
                return new Parser[]
                {
                    EmptyDatePart.Parser.Instance,
                    StandardDatePart.Parser.Instance,
                    DayOfWeekDatePart.Parser.Instance,
                    TodayDatePart.Parser.Instance,
                    TomorrowDatePart.Parser.Instance,
                    NewYearDatePart.Parser.Instance,
                    SpecialDatePart.Parser.Instance
                };
            }
        }

        /// <summary>
        /// Gets a value indicating whether the part is valid.
        /// </summary>
        public virtual bool IsValid
        {
            get { return true; }
        }

        /// <summary>
        /// Returns a concrete date represented by this part on or after the reference date.
        /// </summary>
        /// <param name="referenceDate">A reference date and time.</param>
        /// <param name="tryExcludeReferenceDate">A value indicating whether a date after (rather than on or after) the
        /// reference date should be returned if possible.</param>
        /// <returns>A concrete date represented by this part.</returns>
        public abstract DateTime ToDateTime(DateTime referenceDate, bool tryExcludeReferenceDate);

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
        /// Parses <see cref="DatePart"/>s from <see cref="string"/>s.
        /// </summary>
        public abstract class Parser
        {
            /// <summary>
            /// Returns a value indicating whether this <see cref="Parser"/> is compatible with the specified <see
            /// cref="TimePart.Parser"/>.
            /// </summary>
            /// <param name="timePartParser">A <see cref="TimePart.Parser"/>.</param>
            /// <returns>A value indicating whether this <see cref="Parser"/> is compatible with the specified <see
            /// cref="TimePart.Parser"/>.</returns>
            public virtual bool IsCompatibleWith(TimePart.Parser timePartParser)
            {
                return true;
            }

            /// <summary>
            /// Returns the regular expressions supported by this <see cref="Parser"/>.
            /// </summary>
            /// <param name="provider">An <see cref="IFormatProvider"/> to use when parsing.</param>
            /// <returns>The regular expressions supported by this <see cref="Parser"/>.</returns>
            public abstract IEnumerable<string> GetPatterns(IFormatProvider provider);

            /// <summary>
            /// Parses a <see cref="DatePart"/> from a regular expression <see cref="Match"/>.
            /// </summary>
            /// <param name="match">A <see cref="Match"/> corresponding to a pattern returned by <see
            /// cref="GetPatterns"/>.</param>
            /// <param name="provider">An <see cref="IFormatProvider"/> to use when parsing.</param>
            /// <returns>aA<see cref="DatePart"/> from the regular expression <see cref="Match"/>.</returns>
            /// <exception cref="FormatException">If a <see cref="DatePart"/> could not be parsed from the regular
            /// expression <see cref="Match"/>.</exception>
            public DatePart Parse(Match match, IFormatProvider provider)
            {
                if (!match.Success)
                {
                    throw new FormatException();
                }

                DatePart datePart = this.ParseInternal(match, provider);

                if (!datePart.IsValid)
                {
                    throw new FormatException();
                }

                return datePart;
            }

            /// <summary>
            /// Parses a <see cref="DatePart"/> from a regular expression <see cref="Match"/>.
            /// </summary>
            /// <param name="match">A <see cref="Match"/> corresponding to a pattern returned by <see
            /// cref="GetPatterns"/>.</param>
            /// <param name="provider">An <see cref="IFormatProvider"/> to use when parsing.</param>
            /// <returns>aA<see cref="DatePart"/> from the regular expression <see cref="Match"/>.</returns>
            protected abstract DatePart ParseInternal(Match match, IFormatProvider provider);
        }
    }
}
