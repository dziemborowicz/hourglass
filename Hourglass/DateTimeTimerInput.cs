// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DateTimeTimerInput.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass
{
    using Hourglass.Parsing;
    using Hourglass.Serialization;

    /// <summary>
    /// A representation of an input for a <see cref="DateTimeTimer"/>.
    /// </summary>
    public class DateTimeTimerInput : TimerInput
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DateTimeTimerInput"/> class.
        /// </summary>
        /// <param name="dateTimeToken">A <see cref="DateTimeToken"/> representing the date and time until which the
        /// <see cref="DateTimeTimer"/> should count down.</param>
        public DateTimeTimerInput(DateTimeToken dateTimeToken)
        {
            this.DateTimeToken = dateTimeToken;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DateTimeTimerInput"/> class from a <see
        /// cref="DateTimeTimerInputInfo"/>.
        /// </summary>
        /// <param name="inputInfo">A <see cref="DateTimeTimerInputInfo"/>.</param>
        public DateTimeTimerInput(DateTimeTimerInputInfo inputInfo)
        {
            this.DateTimeToken = inputInfo.DateTimeToken;
        }

        /// <summary>
        /// Gets the <see cref="DateTimeToken"/> representing the date and time until which the <see
        /// cref="DateTimeTimer"/> should count down.
        /// </summary>
        public DateTimeToken DateTimeToken { get; private set; }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            try
            {
                return this.DateTimeToken.ToString();
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Returns the representation of the <see cref="TimerInput"/> used for XML serialization.
        /// </summary>
        /// <returns>The representation of the <see cref="TimerInput"/> used for XML serialization.</returns>
        public override TimerInputInfo ToTimerInputInfo()
        {
            return new DateTimeTimerInputInfo { DateTimeToken = this.DateTimeToken };
        }
    }
}
