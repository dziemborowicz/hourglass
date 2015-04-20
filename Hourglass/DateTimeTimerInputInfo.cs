// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DateTimeTimerInputInfo.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass
{
    using System;

    /// <summary>
    /// The representation of a <see cref="DateTimeTimerInput"/> used for XML serialization.
    /// </summary>
    public class DateTimeTimerInputInfo : TimerInputInfo
    {
        /// <summary>
        /// Gets or sets the <see cref="DateTimeTimerInput.DateTime"/> for a <see cref="DateTimeTimerInput"/>.
        /// </summary>
        public DateTime DateTime { get; set; }
    }
}
