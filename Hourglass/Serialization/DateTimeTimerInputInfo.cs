// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DateTimeTimerInputInfo.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass.Serialization
{
    using System;

    /// <summary>
    /// The representation of a <see cref="DateTimeTimerInput"/> used for XML serialization.
    /// </summary>
    public class DateTimeTimerInputInfo : TimerInputInfo
    {
        /// <summary>
        /// Gets or sets the <see cref="DateTime"/> until which the <see cref="DateTimeTimer"/> should count down.
        /// </summary>
        public DateTime DateTime { get; set; }
    }
}
