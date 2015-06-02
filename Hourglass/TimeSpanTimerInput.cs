// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimeSpanTimerInput.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass
{
    using System;

    using Hourglass.Serialization;

    /// <summary>
    /// A representation of an input for a <see cref="TimeSpanTimer"/>.
    /// </summary>
    public class TimeSpanTimerInput : TimerInput
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TimeSpanTimerInput"/> class.
        /// </summary>
        /// <param name="timeSpan">The <see cref="TimeSpan"/> for which the <see cref="TimeSpanTimer"/> should count
        /// down.</param>
        public TimeSpanTimerInput(TimeSpan timeSpan)
        {
            if (this.TimeSpan < TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException("timeSpan");
            }

            this.TimeSpan = timeSpan;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeSpanTimerInput"/> class from a <see
        /// cref="TimeSpanTimerInputInfo"/>.
        /// </summary>
        /// <param name="inputInfo">A <see cref="TimeSpanTimerInputInfo"/>.</param>
        public TimeSpanTimerInput(TimeSpanTimerInputInfo inputInfo)
        {
            this.TimeSpan = inputInfo.TimeSpan;
        }

        /// <summary>
        /// Gets the <see cref="TimeSpan"/> for which the <see cref="TimeSpanTimer"/> should count down.
        /// </summary>
        public TimeSpan TimeSpan { get; private set; }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            try
            {
                return TimeSpanUtility.ToShortNaturalString(this.TimeSpan);
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
            return new TimeSpanTimerInputInfo { TimeSpan = this.TimeSpan };
        }
    }
}
