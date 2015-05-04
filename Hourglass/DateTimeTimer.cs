// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DateTimeTimer.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass
{
    using System;

    /// <summary>
    /// A <see cref="HourglassTimer"/> that counts down to a specified date and time.
    /// </summary>
    public class DateTimeTimer : HourglassTimer
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DateTimeTimer"/> class.
        /// </summary>
        public DateTimeTimer()
            : base(new DateTimeTimerOptions())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DateTimeTimer"/> class.
        /// </summary>
        /// <param name="timerInfo">A <see cref="DateTimeTimerInfo"/> representing the state of the <see
        /// cref="DateTimeTimer"/>.</param>
        public DateTimeTimer(DateTimeTimerInfo timerInfo)
            : base(timerInfo)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a value indicating whether the timer supports displaying a progress value.
        /// </summary>
        public override bool SupportsProgress
        {
            get { return false; }
        }

        /// <summary>
        /// Gets a value indicating whether the timer supports displaying the elapsed time since the timer was started.
        /// </summary>
        public override bool SupportsTimeElapsed
        {
            get { return false; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Starts the timer.
        /// </summary>
        /// <param name="timerInput">A <see cref="DateTimeTimerInput"/>.</param>
        /// <exception cref="ObjectDisposedException">If the <see cref="Timer"/> has been disposed.</exception>
        public override void Start(TimerInput timerInput)
        {
            DateTimeTimerInput dateTimeTimerInput = (DateTimeTimerInput)timerInput;

            DateTime startTime = DateTime.Now;
            DateTime endTime = dateTimeTimerInput.DateTime;

            this.Start(startTime, endTime);
            base.Start(timerInput);
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Returns a new <see cref="TimerInfo"/> of the correct type for this class.
        /// </summary>
        /// <returns>A new <see cref="TimerInfo"/>.</returns>
        protected override TimerInfo GetNewTimerInfo()
        {
            return new DateTimeTimerInfo();
        }

        #endregion
    }
}
