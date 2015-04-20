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
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">An <see cref="object"/>.</param>
        /// <returns><c>true</c> if the specified object is equal to the current object, or <c>false</c> otherwise.
        /// </returns>
        public override bool Equals(object obj)
        {
            DateTimeTimerInput input = obj as DateTimeTimerInput;
            return input != null && input.dateTime == this.dateTime;
        }

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            return this.dateTime.GetHashCode();
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
        /// Returns the representation of the <see cref="TimerInput"/> used for XML serialization.
        /// </summary>
        /// <returns>The representation of the <see cref="TimerInput"/> used for XML serialization.</returns>
        public override TimerInputInfo ToTimerInputInfo()
        {
            return new DateTimeTimerInputInfo { DateTime = this.dateTime };
        }
    }
}
