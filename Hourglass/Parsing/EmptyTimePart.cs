// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EmptyTimePart.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass.Parsing
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Represents an unspecified time of day.
    /// </summary>
    public class EmptyTimePart : TimePart
    {
        /// <summary>
        /// Returns a concrete time represented by this part on or after the reference date and time.
        /// </summary>
        /// <param name="referenceDate">A reference date and time.</param>
        /// <param name="datePart">The concrete date represented by the corresponding <see cref="DatePart"/>.</param>
        /// <returns>A concrete time represented by this part.</returns>
        public override DateTime ToDateTime(DateTime referenceDate, DateTime datePart)
        {
            this.ThrowIfNotValid();

            return new DateTime(
                datePart.Year,
                datePart.Month,
                datePart.Day,
                0 /* hour */,
                0 /* minute */,
                0 /* second */);
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return string.Empty;
        }

        /// <summary>
        /// Parses <see cref="EmptyTimePart"/>s from <see cref="string"/>s.
        /// </summary>
        public new class Parser : TimePart.Parser
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
            /// Returns a value indicating whether this <see cref="Parser"/> is compatible with the specified <see
            /// cref="DatePart.Parser"/>.
            /// </summary>
            /// <param name="datePartParser">A <see cref="DatePart.Parser"/>.</param>
            /// <returns>A value indicating whether this <see cref="Parser"/> is compatible with the specified <see
            /// cref="DatePart.Parser"/>.</returns>
            public override bool IsCompatibleWith(DatePart.Parser datePartParser)
            {
                return !(datePartParser is EmptyDatePart.Parser);
            }

            /// <summary>
            /// Returns the regular expressions supported by this <see cref="Parser"/>.
            /// </summary>
            /// <param name="provider">An <see cref="IFormatProvider"/> to use when parsing.</param>
            /// <returns>The regular expressions supported by this <see cref="Parser"/>.</returns>
            public override IEnumerable<string> GetPatterns(IFormatProvider provider)
            {
                return new[] { string.Empty };
            }

            /// <summary>
            /// Parses a <see cref="TimePart"/> from a regular expression <see cref="Match"/>.
            /// </summary>
            /// <param name="match">A <see cref="Match"/> corresponding to a pattern returned by <see
            /// cref="GetPatterns"/>.</param>
            /// <param name="provider">An <see cref="IFormatProvider"/> to use when parsing.</param>
            /// <returns>aA<see cref="TimePart"/> from the regular expression <see cref="Match"/>.</returns>
            protected override TimePart ParseInternal(Match match, IFormatProvider provider)
            {
                return new EmptyTimePart();
            }
        }
    }
}
