// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimePart.cs" company="Chris Dziemborowicz">
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
    /// Represents part of a time of day.
    /// </summary>
    [XmlInclude(typeof(EmptyTimePart))]
    [XmlInclude(typeof(SpecialTimePart))]
    [XmlInclude(typeof(StandardTimePart))]
    public abstract class TimePart
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
                    EmptyTimePart.Parser.Instance,
                    StandardTimePart.Parser.Instance,
                    SpecialTimePart.Parser.Instance
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
        /// Returns a concrete time represented by this part on or after the reference date and time.
        /// </summary>
        /// <param name="referenceDate">A reference date and time.</param>
        /// <param name="datePart">The concrete date represented by the corresponding <see cref="DatePart"/>.</param>
        /// <returns>A concrete time represented by this part.</returns>
        public abstract DateTime ToDateTime(DateTime referenceDate, DateTime datePart);

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
        /// Parses <see cref="TimePart"/>s from <see cref="string"/>s.
        /// </summary>
        public abstract class Parser
        {
            /// <summary>
            /// Returns a value indicating whether this <see cref="Parser"/> is compatible with the specified <see
            /// cref="DatePart.Parser"/>.
            /// </summary>
            /// <param name="datePartParser">A <see cref="DatePart.Parser"/>.</param>
            /// <returns>A value indicating whether this <see cref="Parser"/> is compatible with the specified <see
            /// cref="DatePart.Parser"/>.</returns>
            public virtual bool IsCompatibleWith(DatePart.Parser datePartParser)
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
            /// Parses a <see cref="TimePart"/> from a regular expression <see cref="Match"/>.
            /// </summary>
            /// <param name="match">A <see cref="Match"/> corresponding to a pattern returned by <see
            /// cref="GetPatterns"/>.</param>
            /// <param name="provider">An <see cref="IFormatProvider"/> to use when parsing.</param>
            /// <returns>aA<see cref="TimePart"/> from the regular expression <see cref="Match"/>.</returns>
            /// <exception cref="FormatException">If a <see cref="TimePart"/> could not be parsed from the regular
            /// expression <see cref="Match"/>.</exception>
            public TimePart Parse(Match match, IFormatProvider provider)
            {
                if (!match.Success)
                {
                    throw new FormatException();
                }

                TimePart timePart = this.ParseInternal(match, provider);

                if (!timePart.IsValid)
                {
                    throw new FormatException();
                }

                return timePart;
            }

            /// <summary>
            /// Parses a <see cref="TimePart"/> from a regular expression <see cref="Match"/>.
            /// </summary>
            /// <param name="match">A <see cref="Match"/> corresponding to a pattern returned by <see
            /// cref="GetPatterns"/>.</param>
            /// <param name="provider">An <see cref="IFormatProvider"/> to use when parsing.</param>
            /// <returns>aA<see cref="TimePart"/> from the regular expression <see cref="Match"/>.</returns>
            protected abstract TimePart ParseInternal(Match match, IFormatProvider provider);
        }
    }
}
