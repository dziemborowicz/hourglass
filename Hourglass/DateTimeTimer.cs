// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DateTimeTimer.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass
{
    using System;

    /// <summary>
    /// A <see cref="ViewableTimer"/> that counts down to a specified date and time.
    /// </summary>
    public class DateTimeTimer : ViewableTimer
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

        #region Protected Methods

        /// <summary>
        /// Starts the timer.
        /// </summary>
        /// <param name="parameter">A <see cref="TimerInput"/> used to start the timer.</param>
        protected override void ExecuteStart(object parameter)
        {
            DateTimeTimerInput input = (DateTimeTimerInput)parameter;

            DateTime startTime = DateTime.Now;
            DateTime endTime = input.DateTime;

            this.Start(startTime, endTime);
        }

        /// <summary>
        /// Returns a value indicating whether the timer can be started.
        /// </summary>
        /// <returns>A value indicating whether the timer can be started.</returns>
        /// <param name="parameter">A <see cref="TimerInput"/> used to start the timer.</param>
        protected override bool CanExecuteStart(object parameter)
        {
            return true;
        }

        /// <summary>
        /// Pauses the timer.
        /// </summary>
        protected override void ExecutePause()
        {
            // Do nothing
        }

        /// <summary>
        /// Returns a value indicating whether the timer can be paused.
        /// </summary>
        /// <returns>A value indicating whether the timer can be paused.</returns>
        protected override bool CanExecutePause()
        {
            return false;
        }

        /// <summary>
        /// Resumes the timer.
        /// </summary>
        protected override void ExecuteResume()
        {
            // Do nothing
        }

        /// <summary>
        /// Returns a value indicating whether the timer can be resumed.
        /// </summary>
        /// <returns>A value indicating whether the timer can be resumed.</returns>
        protected override bool CanExecuteResume()
        {
            return false;
        }

        /// <summary>
        /// Stops the timer.
        /// </summary>
        protected override void ExecuteStop()
        {
            this.Stop();
        }

        /// <summary>
        /// Returns a value indicating whether the timer can be stopped.
        /// </summary>
        /// <returns>A value indicating whether the timer can be stopped.</returns>
        protected override bool CanExecuteStop()
        {
            return this.State != TimerState.Stopped && this.State != TimerState.Expired;
        }

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
