// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ViewableTimerInfo.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass
{
    /// <summary>
    /// A representation of the state of a <see cref="ViewableTimer"/>.
    /// </summary>
    public abstract class ViewableTimerInfo : CommandTimerInfo
    {
        /// <summary>
        /// Gets or sets the configuration data for this timer.
        /// </summary>
        public TimerOptionsInfo Options { get; set; }
    }
}
