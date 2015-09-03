// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EmptyTimeToken.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass.Parsing
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Represents an unspecified time.
    /// </summary>
    public class EmptyTimeToken : TimeToken
    {
        /// <summary>
        /// Gets a value indicating whether the token is valid.
        /// </summary>
        public override bool IsValid
        {
            get { return true; }
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

            return datePart.Date;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <param name="provider">An <see cref="IFormatProvider"/> to use.</param>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString(IFormatProvider provider)
        {
            return string.Empty;
        }

        /// <summary>
        /// Parses <see cref="EmptyTimeToken"/> strings.
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
            /// Returns a value indicating whether this parser can be used in conjunction with a specified <see
            /// cref="DateToken.Parser"/>.
            /// </summary>
            /// <param name="dateTokenParser">A <see cref="DateToken.Parser"/>.</param>
            /// <returns>A value indicating whether this parser can be used in conjunction with the specified <see
            /// cref="DateToken.Parser"/>.</returns>
            public override bool IsCompatibleWith(DateToken.Parser dateTokenParser)
            {
                return !(dateTokenParser is EmptyDateToken.Parser);
            }

            /// <summary>
            /// Returns a set of regular expressions supported by this parser.
            /// </summary>
            /// <param name="provider">An <see cref="IFormatProvider"/>.</param>
            /// <returns>A set of regular expressions supported by this parser.</returns>
            public override IEnumerable<string> GetPatterns(IFormatProvider provider)
            {
                return new[] { string.Empty };
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
                return new EmptyTimeToken();
            }
        }
    }
}
