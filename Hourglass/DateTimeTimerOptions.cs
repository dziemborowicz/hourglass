// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DateTimeTimerOptions.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass
{
    using System;

    /// <summary>
    /// Configuration data for a <see cref="DateTimeTimer"/>.
    /// </summary>
    public class DateTimeTimerOptions : TimerOptions
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DateTimeTimerOptions"/> class.
        /// </summary>
        public DateTimeTimerOptions()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DateTimeTimerOptions"/> class from a <see
        /// cref="DateTimeTimerOptions"/>.
        /// </summary>
        /// <param name="options">A <see cref="DateTimeTimerOptions"/>.</param>
        public DateTimeTimerOptions(DateTimeTimerOptions options)
            : base(options)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DateTimeTimerOptions"/> class from a <see
        /// cref="DateTimeTimerOptionsInfo"/>.
        /// </summary>
        /// <param name="optionsInfo">A <see cref="DateTimeTimerOptionsInfo"/>.</param>
        public DateTimeTimerOptions(DateTimeTimerOptionsInfo optionsInfo)
            : base(optionsInfo)
        {
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">An <see cref="object"/>.</param>
        /// <returns><c>true</c> if the specified object is equal to the current object, or <c>false</c> otherwise.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (object.ReferenceEquals(obj, null))
            {
                return false;
            }

            if (object.ReferenceEquals(this, obj))
            {
                return true;
            }

            if (this.GetType() != obj.GetType())
            {
                return false;
            }

            if (!base.Equals(obj))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            this.ThrowIfNotFrozen();

            int hashCode = 17;
            hashCode = (31 * hashCode) + base.GetHashCode();
            return hashCode;
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Returns a new <see cref="TimerOptionsInfo"/> of the correct type for this class.
        /// </summary>
        /// <returns>A new <see cref="TimerOptionsInfo"/>.</returns>
        protected override TimerOptionsInfo GetNewTimerOptionsInfo()
        {
            return new DateTimeTimerOptionsInfo();
        }

        #endregion
    }
}
