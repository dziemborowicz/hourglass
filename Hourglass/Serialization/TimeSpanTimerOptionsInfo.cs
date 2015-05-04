// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimeSpanTimerOptionsInfo.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass.Serialization
{
    /// <summary>
    /// The representation of a <see cref="TimeSpanTimerOptions"/> used for XML serialization.
    /// </summary>
    public class TimeSpanTimerOptionsInfo : TimerOptionsInfo
    {
        /// <summary>
        /// Gets or sets a value indicating whether to loop the timer continuously.
        /// </summary>
        public bool LoopTimer { get; set; }
    }
}
