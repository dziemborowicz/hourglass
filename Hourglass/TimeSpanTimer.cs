// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimeSpanTimer.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass
{
    using System;

    using Hourglass.Serialization;

    /// <summary>
    /// A <see cref="HourglassTimer"/> that counts down for a specified span of time.
    /// </summary>
    public class TimeSpanTimer : HourglassTimer
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeSpanTimer"/> class.
        /// </summary>
        public TimeSpanTimer()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeSpanTimer"/> class.
        /// </summary>
        /// <param name="options">A <see cref="TimerOptions"/>.</param>
        public TimeSpanTimer(TimerOptions options)
            : base(options)
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

        #region Public Methods

        /// <summary>
        /// Starts the timer.
        /// </summary>
        /// <param name="timerInput">A <see cref="TimerInput"/>.</param>
        /// <returns>A value indicating whether the timer was started successfully.</returns>
        /// <exception cref="ObjectDisposedException">If the <see cref="Timer"/> has been disposed.</exception>
        public override bool Start(TimerInput timerInput)
        {
            TimeSpanTimerInput timeSpanTimerInput = (TimeSpanTimerInput)timerInput;

            DateTime start = DateTime.Now;
            DateTime end = start.Add(timeSpanTimerInput.TimeSpan);

            this.Start(start, end, timerInput);
            return true;
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Raises the <see cref="Timer.Expired"/> event, and restarts the timer if required.
        /// </summary>
        protected override void OnExpired()
        {
            base.OnExpired();

            // Loop the timer if the option is set and the timer has not been stopped
            if (this.Options.LoopTimer && this.State != TimerState.Stopped)
            {
                if (!this.EndTime.HasValue)
                {
                    throw new InvalidOperationException();
                }

                // Get the input time span in ticks
                TimeSpanTimerInput timeSpanTimerInput = (TimeSpanTimerInput)this.Input;
                long inputTicks = timeSpanTimerInput.TimeSpan.Ticks;

                // Find the next start and end times where the end time is in the future
                long nowTicks = DateTime.Now.Ticks;
                long startTicks = (Math.Max(nowTicks - this.EndTime.Value.Ticks, 0L) / inputTicks * inputTicks) + this.EndTime.Value.Ticks;
                long endTicks = startTicks + inputTicks;

                // Start the timer
                this.Start(new DateTime(startTicks), new DateTime(endTicks), timeSpanTimerInput);
            }
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
