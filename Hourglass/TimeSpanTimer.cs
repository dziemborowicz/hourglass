// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimeSpanTimer.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass
{
    using System;

    /// <summary>
    /// A <see cref="ViewableTimer"/> that counts down for a specified span of time.
    /// </summary>
    public class TimeSpanTimer : ViewableTimer
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeSpanTimer"/> class.
        /// </summary>
        public TimeSpanTimer()
            : base(new TimeSpanTimerOptions())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeSpanTimer"/> class.
        /// </summary>
        /// <param name="timerInfo">A <see cref="TimeSpanTimerInfo"/> representing the state of the <see
        /// cref="TimeSpanTimer"/>.</param>
        public TimeSpanTimer(TimeSpanTimerInfo timerInfo)
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
            get { return true; }
        }

        /// <summary>
        /// Gets a value indicating whether the timer supports displaying the elapsed time since the timer was started.
        /// </summary>
        public override bool SupportsTimeElapsed
        {
            get { return true; }
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Starts the timer.
        /// </summary>
        /// <param name="parameter">A <see cref="TimerInput"/> used to start the timer.</param>
        protected override void ExecuteStart(object parameter)
        {
            TimeSpanTimerInput input = (TimeSpanTimerInput)parameter;

            DateTime startTime = DateTime.Now;
            DateTime endTime = startTime.Add(input.TimeSpan);

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
            this.Pause();
        }

        /// <summary>
        /// Returns a value indicating whether the timer can be paused.
        /// </summary>
        /// <returns>A value indicating whether the timer can be paused.</returns>
        protected override bool CanExecutePause()
        {
            return this.State == TimerState.Running;
        }

        /// <summary>
        /// Resumes the timer.
        /// </summary>
        protected override void ExecuteResume()
        {
            this.Resume();
        }

        /// <summary>
        /// Returns a value indicating whether the timer can be resumed.
        /// </summary>
        /// <returns>A value indicating whether the timer can be resumed.</returns>
        protected override bool CanExecuteResume()
        {
            return this.State == TimerState.Paused;
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
            return new TimeSpanTimerInfo();
        }

        #endregion
    }
}
