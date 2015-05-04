// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HourglassTimerInfo.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass.Serialization
{
    /// <summary>
    /// A representation of the state of a <see cref="HourglassTimer"/>.
    /// </summary>
    public abstract class HourglassTimerInfo : TimerInfo
    {
        /// <summary>
        /// Gets or sets the configuration data for this timer.
        /// </summary>
        public TimerOptionsInfo Options { get; set; }

        /// <summary>
        /// Gets or sets the input used to start this timer, or <c>null</c> if the <see cref="TimerInfo.State"/> is
        /// <see cref="TimerState.Stopped"/>.
        /// </summary>
        public TimerInputInfo Input { get; set; }
    }
}
