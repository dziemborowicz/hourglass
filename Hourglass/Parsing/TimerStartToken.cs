// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimerStartToken.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass.Parsing
{
    using System;
    using System.Globalization;
    using System.Text.RegularExpressions;
    using System.Xml.Serialization;

    /// <summary>
    /// Represents a <see cref="TimerStart"/>.
    /// </summary>
    [XmlInclude(typeof(DateTimeToken))]
    [XmlInclude(typeof(TimeSpanToken))]
    public abstract class TimerStartToken
    {
        /// <summary>
        /// Gets a value indicating whether the token is valid.
        /// </summary>
        public abstract bool IsValid { get; }

        /// <summary>
        /// Returns the end time for a timer started with this token at a specified time.
        /// </summary>
        /// <param name="startTime">The time the timer is started.</param>
        /// <returns>The end time for a timer started with this token at the specified time.</returns>
        public abstract DateTime GetEndTime(DateTime startTime);

        /// <summary>
        /// Returns the end time for a timer started with this token at a specified time.
        /// </summary>
        /// <param name="startTime">The time the timer is started.</param>
        /// <param name="endTime">The end time for a timer started with this token at the specified time if the end
        /// time could be computed, or <see cref="DateTime.MinValue"/> otherwise.</param>
        /// <returns><c>true</c> if the end time could be computed, or <c>false</c> otherwise.</returns>
        public bool TryGetEndTime(DateTime startTime, out DateTime endTime)
        {
            try
            {
                endTime = this.GetEndTime(startTime);
                return true;
            }
            catch
            {
                endTime = DateTime.MinValue;
                return false;
            }
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public abstract override string ToString();

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
        /// Parses <see cref="TimerStartToken"/> strings.
        /// </summary>
        public abstract class Parser
        {
            /// <summary>
            /// The <see cref="RegexOptions"/> used when matching regular expressions.
            /// </summary>
            public static readonly RegexOptions RegexOptions =
                RegexOptions.CultureInvariant |
                RegexOptions.IgnoreCase |
                RegexOptions.IgnorePatternWhitespace;

            /// <summary>
            /// Parses a string into a <see cref="TimerStartToken"/>.
            /// </summary>
            /// <remarks>
            /// This overload uses the <see cref="CultureInfo.CurrentCulture"/> as the <see cref="IFormatProvider"/>.
            /// </remarks>
            /// <param name="str">A string representation of a <see cref="TimerStartToken"/>.</param>
            /// <returns>The <see cref="TimerStartToken"/> parsed from the string.</returns>
            /// <exception cref="ArgumentNullException">If <paramref name="str"/> is <c>null</c>.</exception>
            /// <exception cref="FormatException">If <paramref name="str"/> is not a supported representation of a <see
            /// cref="TimerStartToken"/>.</exception>
            public TimerStartToken Parse(string str)
            {
                return this.Parse(str, CultureInfo.CurrentCulture);
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
            public TimerStartToken Parse(string str, IFormatProvider provider)
            {
                if (str == null)
                {
                    throw new ArgumentNullException("str");
                }

                TimerStartToken timerStartToken = this.ParseInternal(str, provider);

                if (!timerStartToken.IsValid)
                {
                    throw new FormatException();
                }

                return timerStartToken;
            }
            
            /// <summary>
            /// Parses a string into a <see cref="TimerStartToken"/>.
            /// </summary>
            /// <remarks>
            /// This overload uses the <see cref="CultureInfo.CurrentCulture"/> as the <see cref="IFormatProvider"/>.
            /// </remarks>
            /// <param name="str">A string representation of a <see cref="TimerStartToken"/>.</param>
            /// <param name="timerStartToken">The <see cref="TimerStartToken"/> parsed from the string, or <c>null</c>
            /// if the string is not a supported representation of a <see cref="TimerStartToken"/>.</param>
            /// <returns><c>true</c> if the <see cref="TimerStartToken"/> was successfully parsed from <paramref
            /// name="str"/>, or <c>false</c> otherwise.</returns>
            public bool TryParse(string str, out TimerStartToken timerStartToken)
            {
                return this.TryParse(str, CultureInfo.CurrentCulture, out timerStartToken);
            }

            /// <summary>
            /// Parses a string into a <see cref="TimerStartToken"/>.
            /// </summary>
            /// <param name="str">A string representation of a <see cref="TimerStartToken"/>.</param>
            /// <param name="provider">An <see cref="IFormatProvider"/>.</param>
            /// <param name="timerStartToken">The <see cref="TimerStartToken"/> parsed from the string, or <c>null</c>
            /// if the string is not a supported representation of a <see cref="TimerStartToken"/>.</param>
            /// <returns><c>true</c> if the <see cref="TimerStartToken"/> was successfully parsed from <paramref
            /// name="str"/>, or <c>false</c> otherwise.</returns>
            public bool TryParse(string str, IFormatProvider provider, out TimerStartToken timerStartToken)
            {
                try
                {
                    timerStartToken = this.Parse(str, provider);
                    return true;
                }
                catch
                {
                    timerStartToken = null;
                    return false;
                }
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
            protected abstract TimerStartToken ParseInternal(string str, IFormatProvider provider);
        }
    }
}
