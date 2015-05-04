// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DateTimeTimerInput.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass
{
    using System;

    /// <summary>
    /// A representation of an input for a <see cref="DateTimeTimer"/>.
    /// </summary>
    public class DateTimeTimerInput : TimerInput
    {
        /// <summary>
        /// The <see cref="DateTime"/> until which the <see cref="DateTimeTimer"/> should count down.
        /// </summary>
        private readonly DateTime dateTime;

        /// <summary>
        /// Initializes a new instance of the <see cref="DateTimeTimerInput"/> class.
        /// </summary>
        /// <param name="dateTime">The <see cref="DateTime"/> until which the <see cref="DateTimeTimer"/> should count
        /// down.</param>
        public DateTimeTimerInput(DateTime dateTime)
            : this(dateTime, new DateTimeTimerOptions())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DateTimeTimerInput"/> class.
        /// </summary>
        /// <param name="dateTime">The <see cref="DateTime"/> until which the <see cref="DateTimeTimer"/> should count
        /// down.</param>
        /// <param name="options">The configuration data for the timer.</param>
        public DateTimeTimerInput(DateTime dateTime, TimerOptions options)
            : base(options)
        {
            this.dateTime = dateTime;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DateTimeTimerInput"/> class from a <see
        /// cref="DateTimeTimerInputInfo"/>.
        /// </summary>
        /// <param name="inputInfo">A <see cref="DateTimeTimerInputInfo"/>.</param>
        public DateTimeTimerInput(DateTimeTimerInputInfo inputInfo)
            : base(inputInfo)
        {
            this.dateTime = inputInfo.DateTime;
        }

        /// <summary>
        /// Gets the <see cref="DateTime"/> until which the <see cref="DateTimeTimer"/> should count down.
        /// </summary>
        public DateTime DateTime
        {
            get { return this.dateTime; }
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

            DateTimeTimerInput dateTimeTimerInput = (DateTimeTimerInput)input;
            return this.DateTime.Equals(dateTimeTimerInput.DateTime);
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return DateTimeUtility.ToNaturalString(this.dateTime);
        }

        /// <summary>
        /// Returns a new <see cref="TimerInputInfo"/> of the correct type for this class.
        /// </summary>
        /// <returns>A new <see cref="TimerInputInfo"/>.</returns>
        protected override TimerInputInfo GetNewTimerInputInfo()
        {
            return new DateTimeTimerInputInfo();
        }

        /// <summary>
        /// Sets the properties on a <see cref="TimerInputInfo"/> from the values in this class.
        /// </summary>
        /// <param name="timerInputInfo">A <see cref="TimerInputInfo"/>.</param>
        protected override void SetTimerInputInfo(TimerInputInfo timerInputInfo)
        {
            base.SetTimerInputInfo(timerInputInfo);

            DateTimeTimerInputInfo info = (DateTimeTimerInputInfo)timerInputInfo;
            info.DateTime = this.dateTime;
        }
    }
}
