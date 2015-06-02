// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DateTimeTimerInputInfo.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass.Serialization
{
    using Hourglass.Parsing;

    /// <summary>
    /// The representation of a <see cref="DateTimeTimerInput"/> used for XML serialization.
    /// </summary>
    public class DateTimeTimerInputInfo : TimerInputInfo
    {
        /// <summary>
        /// Gets or sets the <see cref="DateTimePart"/> representing the date and time until which the <see
        /// cref="DateTimeTimer"/> should count down.
        /// </summary>
        public DateTimePart DateTimePart { get; set; }
    }
}
