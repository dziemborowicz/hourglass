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
        /// <param name="timerInput">A <see cref="TimeSpanTimerInput"/>.</param>
        /// <exception cref="ObjectDisposedException">If the <see cref="Timer"/> has been disposed.</exception>
        public override void Start(TimerInput timerInput)
        {
            TimeSpanTimerInput timeSpanTimerInput = (TimeSpanTimerInput)timerInput;

            DateTime startTime = DateTime.Now;
            DateTime endTime = startTime.Add(timeSpanTimerInput.TimeSpan);

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
            return new TimeSpanTimerInfo();
        }

        #endregion
    }
}
