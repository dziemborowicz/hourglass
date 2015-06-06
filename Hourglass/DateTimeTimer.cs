// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DateTimeTimer.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass
{
    using System;

    using Hourglass.Serialization;

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
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DateTimeTimer"/> class.
        /// </summary>
        /// <param name="options">A <see cref="TimerOptions"/>.</param>
        public DateTimeTimer(TimerOptions options)
            : base(options)
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
        /// <param name="timerInput">A <see cref="TimerInput"/>.</param>
        /// <returns>A value indicating whether the timer was started successfully.</returns>
        /// <exception cref="ObjectDisposedException">If the <see cref="Timer"/> has been disposed.</exception>
        public override bool Start(TimerInput timerInput)
        {
            DateTimeTimerInput dateTimeTimerInput = (DateTimeTimerInput)timerInput;

            DateTime start = DateTime.Now;
            DateTime end;
            if (!dateTimeTimerInput.DateTimeToken.TryGetEndTime(start, out end) || end <= start)
            {
                return false;
            }

            this.Start(start, end, timerInput);
            return true;
        }

        /// <summary>
        /// Does nothing. The <see cref="DateTimeTimer"/> does not support pausing or resuming the timer.
        /// </summary>
        /// <exception cref="ObjectDisposedException">If the <see cref="Timer"/> has been disposed.</exception>
        public override void Pause()
        {
            this.ThrowIfDisposed();

            // Do nothing
        }

        /// <summary>
        /// Does nothing. The <see cref="DateTimeTimer"/> does not support pausing or resuming the timer.
        /// </summary>
        /// <exception cref="ObjectDisposedException">If the <see cref="Timer"/> has been disposed.</exception>
        public override void Resume()
        {
            this.ThrowIfDisposed();

            // Do nothing
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
