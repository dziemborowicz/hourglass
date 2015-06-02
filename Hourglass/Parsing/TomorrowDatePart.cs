// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TomorrowDatePart.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass.Parsing
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Represents tomorrow.
    /// </summary>
    public class TomorrowDatePart : DatePart
    {
        /// <summary>
        /// Returns a concrete date represented by this part on or after the reference date.
        /// </summary>
        /// <param name="referenceDate">A reference date and time.</param>
        /// <param name="tryExcludeReferenceDate">A value indicating whether a date after (rather than on or after) the
        /// reference date should be returned if possible.</param>
        /// <returns>A concrete date represented by this part.</returns>
        public override DateTime ToDateTime(DateTime referenceDate, bool tryExcludeReferenceDate)
        {
            this.ThrowIfNotValid();

            return referenceDate.Date.AddDays(1);
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return "Tomorrow";
        }

        /// <summary>
        /// Parses <see cref="TomorrowDatePart"/>s from <see cref="string"/>s.
        /// </summary>
        public new class Parser : DatePart.Parser
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
            /// Returns the regular expressions supported by this <see cref="Parser"/>.
            /// </summary>
            /// <param name="provider">An <see cref="IFormatProvider"/> to use when parsing.</param>
            /// <returns>The regular expressions supported by this <see cref="Parser"/>.</returns>
            public override IEnumerable<string> GetPatterns(IFormatProvider provider)
            {
                return new[] { @"tomm?orr?ow" };
            }

            /// <summary>
            /// Parses a <see cref="DatePart"/> from a regular expression <see cref="Match"/>.
            /// </summary>
            /// <param name="match">A <see cref="Match"/> corresponding to a pattern returned by <see
            /// cref="GetPatterns"/>.</param>
            /// <param name="provider">An <see cref="IFormatProvider"/> to use when parsing.</param>
            /// <returns>aA<see cref="DatePart"/> from the regular expression <see cref="Match"/>.</returns>
            protected override DatePart ParseInternal(Match match, IFormatProvider provider)
            {
                return new TodayDatePart();
            }
        }
    }
}
