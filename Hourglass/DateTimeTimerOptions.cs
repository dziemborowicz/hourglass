// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DateTimeTimerOptions.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass
{
    using Hourglass.Serialization;

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
