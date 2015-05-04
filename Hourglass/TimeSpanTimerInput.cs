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
        /// The <see cref="TimeSpan"/> for which the <see cref="TimeSpanTimer"/> should count down.
        /// </summary>
        private readonly TimeSpan timeSpan;

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeSpanTimerInput"/> class.
        /// </summary>
        /// <param name="timeSpan">The <see cref="TimeSpan"/> for which the <see cref="TimeSpanTimer"/> should count
        /// down.</param>
        public TimeSpanTimerInput(TimeSpan timeSpan)
            : this(timeSpan, new TimeSpanTimerOptions())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeSpanTimerInput"/> class.
        /// </summary>
        /// <param name="timeSpan">The <see cref="TimeSpan"/> for which the <see cref="TimeSpanTimer"/> should count
        /// down.</param>
        /// <param name="options">The configuration data for the timer.</param>
        public TimeSpanTimerInput(TimeSpan timeSpan, TimerOptions options)
            : base(options)
        {
            if (this.timeSpan < TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException("timeSpan");
            }

            this.timeSpan = timeSpan;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeSpanTimerInput"/> class from a <see
        /// cref="TimeSpanTimerInputInfo"/>.
        /// </summary>
        /// <param name="inputInfo">A <see cref="TimeSpanTimerInputInfo"/>.</param>
        public TimeSpanTimerInput(TimeSpanTimerInputInfo inputInfo)
            : base(inputInfo)
        {
            this.timeSpan = inputInfo.TimeSpan;
        }

        /// <summary>
        /// Gets the <see cref="TimeSpan"/> for which the <see cref="TimeSpanTimer"/> should count down.
        /// </summary>
        public TimeSpan TimeSpan
        {
            get { return this.timeSpan; }
        }

        /// <summary>
        /// Returns a value indicating whether the <see cref="TimerInput"/> is equivalent to this <see
        /// cref="TimerInput"/> except for the <see cref="Options"/> field, which is ignored.
        /// </summary>
        /// <param name="input">A <see cref="TimerInput"/>.</param>
        /// <returns>A value indicating whether the <see cref="TimerInput"/> is equivalent to this <see
        /// cref="TimerInput"/> except for the <see cref="Options"/> field.</returns>
        public override bool EqualsExceptForOptions(TimerInput input)
        {
            if (object.ReferenceEquals(this, input))
            {
                return true;
            }

            if (object.ReferenceEquals(input, null))
            {
                return false;
            }

            if (this.GetType() != input.GetType())
            {
                return false;
            }

            TimeSpanTimerInput timeSpanTimerInput = (TimeSpanTimerInput)input;
            return this.TimeSpan.Equals(timeSpanTimerInput.TimeSpan);
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return TimeSpanUtility.ToShortNaturalString(this.timeSpan);
        }

        /// <summary>
        /// Returns a new <see cref="TimerInputInfo"/> of the correct type for this class.
        /// </summary>
        /// <returns>A new <see cref="TimerInputInfo"/>.</returns>
        protected override TimerInputInfo GetNewTimerInputInfo()
        {
            return new TimeSpanTimerInputInfo();
        }

        /// <summary>
        /// Sets the properties on a <see cref="TimerInputInfo"/> from the values in this class.
        /// </summary>
        /// <param name="timerInputInfo">A <see cref="TimerInputInfo"/>.</param>
        protected override void SetTimerInputInfo(TimerInputInfo timerInputInfo)
        {
            base.SetTimerInputInfo(timerInputInfo);

            TimeSpanTimerInputInfo info = (TimeSpanTimerInputInfo)timerInputInfo;
            info.TimeSpan = this.timeSpan;
        }
    }
}
