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
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns><c>true</c> if the specified object is equal to the current object, or <c>false</c> otherwise.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (object.ReferenceEquals(this, obj))
            {
                return true;
            }

            if (object.ReferenceEquals(obj, null))
            {
                return false;
            }

            if (this.GetType() != obj.GetType())
            {
                return false;
            }

            TimeSpanTimerInput timeSpanTimerInput = (TimeSpanTimerInput)obj;
            return this.TimeSpan.Equals(timeSpanTimerInput.TimeSpan);
        }

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            return this.TimeSpan.GetHashCode();
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return TimeSpanUtility.ToShortNaturalString(this.TimeSpan);
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
