// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DateTimeTimerInput.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass
{
    using System;

    using Hourglass.Serialization;

    /// <summary>
    /// A representation of an input for a <see cref="DateTimeTimer"/>.
    /// </summary>
    public class DateTimeTimerInput : TimerInput
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DateTimeTimerInput"/> class.
        /// </summary>
        /// <param name="dateTime">The <see cref="DateTime"/> until which the <see cref="DateTimeTimer"/> should count
        /// down.</param>
        public DateTimeTimerInput(DateTime dateTime)
        {
            this.DateTime = dateTime;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DateTimeTimerInput"/> class from a <see
        /// cref="DateTimeTimerInputInfo"/>.
        /// </summary>
        /// <param name="inputInfo">A <see cref="DateTimeTimerInputInfo"/>.</param>
        public DateTimeTimerInput(DateTimeTimerInputInfo inputInfo)
        {
            this.DateTime = inputInfo.DateTime;
        }

        /// <summary>
        /// Gets the <see cref="DateTime"/> until which the <see cref="DateTimeTimer"/> should count down.
        /// </summary>
        public DateTime DateTime { get; private set; }

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

            DateTimeTimerInput dateTimeTimerInput = (DateTimeTimerInput)obj;
            return this.DateTime.Equals(dateTimeTimerInput.DateTime);
        }

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            return this.DateTime.GetHashCode();
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return DateTimeUtility.ToNaturalString(this.DateTime);
        }

        /// <summary>
        /// Returns the representation of the <see cref="TimerInput"/> used for XML serialization.
        /// </summary>
        /// <returns>The representation of the <see cref="TimerInput"/> used for XML serialization.</returns>
        public override TimerInputInfo ToTimerInputInfo()
        {
            return new DateTimeTimerInputInfo { DateTime = this.DateTime };
        }
    }
}
