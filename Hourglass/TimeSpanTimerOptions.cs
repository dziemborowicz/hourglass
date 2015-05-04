// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimeSpanTimerOptions.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass
{
    using System;

    using Hourglass.Serialization;

    /// <summary>
    /// Configuration data for a <see cref="TimeSpanTimer"/>.
    /// </summary>
    public class TimeSpanTimerOptions : TimerOptions
    {
        #region Private Members

        /// <summary>
        /// A value indicating whether to loop the timer continuously.
        /// </summary>
        private bool loopTimer;

        #endregion
        
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeSpanTimerOptions"/> class.
        /// </summary>
        public TimeSpanTimerOptions()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeSpanTimerOptions"/> class from a <see
        /// cref="TimeSpanTimerOptions"/>.
        /// </summary>
        /// <param name="options">A <see cref="TimeSpanTimerOptions"/>.</param>
        public TimeSpanTimerOptions(TimeSpanTimerOptions options)
            : base(options)
        {
            if (options == null)
            {
                throw new ArgumentNullException("options");
            }

            this.loopTimer = options.LoopTimer;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeSpanTimerOptions"/> class from a <see
        /// cref="TimeSpanTimerOptionsInfo"/>.
        /// </summary>
        /// <param name="optionsInfo">A <see cref="TimeSpanTimerOptionsInfo"/>.</param>
        public TimeSpanTimerOptions(TimeSpanTimerOptionsInfo optionsInfo)
            : base(optionsInfo)
        {
            if (optionsInfo == null)
            {
                throw new ArgumentNullException("optionsInfo");
            }

            this.loopTimer = optionsInfo.LoopTimer;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether to loop the timer continuously.
        /// </summary>
        public bool LoopTimer
        {
            get
            {
                return this.loopTimer;
            }

            set
            {
                if (this.loopTimer == value)
                {
                    return;
                }

                this.loopTimer = value;
                this.OnPropertyChanged("LoopTimer");
            }
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Returns a new <see cref="TimerOptionsInfo"/> of the correct type for this class.
        /// </summary>
        /// <returns>A new <see cref="TimerOptionsInfo"/>.</returns>
        protected override TimerOptionsInfo GetNewTimerOptionsInfo()
        {
            return new TimeSpanTimerOptionsInfo();
        }

        /// <summary>
        /// Sets the properties on a <see cref="TimerOptionsInfo"/> from the values in this class.
        /// </summary>
        /// <param name="timerOptionsInfo">A <see cref="TimerOptionsInfo"/>.</param>
        protected override void SetTimerOptionsInfo(TimerOptionsInfo timerOptionsInfo)
        {
            base.SetTimerOptionsInfo(timerOptionsInfo);

            TimeSpanTimerOptionsInfo info = (TimeSpanTimerOptionsInfo)timerOptionsInfo;
            info.LoopTimer = this.loopTimer;
        }

        #endregion
    }
}
