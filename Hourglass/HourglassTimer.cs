// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HourglassTimer.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass
{
    using System;

    using Hourglass.Serialization;

    /// <summary>
    /// A <see cref="Timer"/> that exposes options and properties useful for graphical display.
    /// </summary>
    public abstract class HourglassTimer : Timer
    {
        #region Private Members

        /// <summary>
        /// Configuration data for this timer.
        /// </summary>
        private readonly TimerOptions options;

        /// <summary>
        /// The <see cref="TimerInput"/> used to start this timer, or <c>null</c> if the <see cref="Timer.State"/> is
        /// <see cref="TimerState.Stopped"/>.
        /// </summary>
        private TimerInput input;

        /// <summary>
        /// The percentage of time left until the timer expires.
        /// </summary>
        /// <remarks>
        /// This field is <c>null</c> if <see cref="SupportsProgress"/> is <c>false</c>.
        /// </remarks>
        private double? timeLeftAsPercentage;

        /// <summary>
        /// The percentage of time elapsed since the timer was started.
        /// </summary>
        /// <remarks>
        /// This field is <c>null</c> if <see cref="SupportsProgress"/> or <see cref="SupportsTimeElapsed"/> is
        /// <c>false</c>.
        /// </remarks>
        private double? timeElapsedAsPercentage;

        /// <summary>
        /// The string representation of the time left until the timer expires.
        /// </summary>
        private string timeLeftAsString;

        /// <summary>
        /// The string representation of the time elapsed since the timer was started.
        /// </summary>
        /// <remarks>
        /// This field is <c>null</c> if <see cref="SupportsTimeElapsed"/> is <c>false</c>.
        /// </remarks>
        private string timeElapsedAsString;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="HourglassTimer"/> class.
        /// </summary>
        /// <param name="options">Configuration data for this timer.</param>
        protected HourglassTimer(TimerOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException("options");
            }

            this.options = options;

            this.UpdateHourglassTimer();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HourglassTimer"/> class.
        /// </summary>
        /// <param name="timerInfo">A <see cref="HourglassTimerInfo"/> representing the state of the <see
        /// cref="HourglassTimer"/>.</param>
        protected HourglassTimer(HourglassTimerInfo timerInfo)
            : base(timerInfo)
        {
            if (timerInfo.Options == null)
            {
                throw new ArgumentException("timerInfo");
            }

            this.options = TimerOptions.FromTimerOptionsInfo(timerInfo.Options);
            this.input = TimerInput.FromTimerInputInfo(timerInfo.Input);

            this.UpdateHourglassTimer();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the configuration data for this timer.
        /// </summary>
        public TimerOptions Options
        {
            get { return this.options; }
        }

        /// <summary>
        /// Gets the <see cref="TimerInput"/> used to start this timer, or <c>null</c> if the <see cref="Timer.State"/>
        /// is <see cref="TimerState.Stopped"/>.
        /// </summary>
        public TimerInput Input
        {
            get { return this.input; }
        }

        /// <summary>
        /// Gets the percentage of time left until the timer expires.
        /// </summary>
        /// <remarks>
        /// This property is <c>null</c> if <see cref="SupportsProgress"/> is <c>false</c>.
        /// </remarks>
        public double? TimeLeftAsPercentage
        {
            get { return this.timeLeftAsPercentage; }
        }

        /// <summary>
        /// Gets the percentage of time elapsed since the timer was started.
        /// </summary>
        /// <remarks>
        /// This property is <c>null</c> if <see cref="SupportsProgress"/> or <see cref="SupportsTimeElapsed"/> is
        /// <c>false</c>.
        /// </remarks>
        public double? TimeElapsedAsPercentage
        {
            get { return this.timeElapsedAsPercentage; }
        }

        /// <summary>
        /// Gets the string representation of the time left until the timer expires.
        /// </summary>
        public string TimeLeftAsString
        {
            get { return this.timeLeftAsString; }
        }

        /// <summary>
        /// Gets the string representation of the time elapsed since the timer was started.
        /// </summary>
        /// <remarks>
        /// This property is <c>null</c> if <see cref="SupportsTimeElapsed"/> is <c>false</c>.
        /// </remarks>
        public string TimeElapsedAsString
        {
            get { return this.timeElapsedAsString; }
        }

        /// <summary>
        /// Gets a value indicating whether the timer supports displaying a progress value.
        /// </summary>
        public abstract bool SupportsProgress { get; }

        /// <summary>
        /// Gets a value indicating whether the timer supports displaying the elapsed time since the timer was started.
        /// </summary>
        public abstract bool SupportsTimeElapsed { get; }

        #endregion

        #region Static Methods

        /// <summary>
        /// Returns a new <see cref="HourglassTimer"/> that can be started with the specified <see cref="TimerInput"/>.
        /// </summary>
        /// <param name="input">A <see cref="TimerInput"/>.</param>
        /// <returns>A new <see cref="HourglassTimer"/> that can be started with the specified <see cref="TimerInput"/>.
        /// </returns>
        public static HourglassTimer GetTimerForInput(TimerInput input)
        {
            if (input is DateTimeTimerInput)
            {
                return new DateTimeTimer();
            }

            if (input is TimeSpanTimerInput)
            {
                return new TimeSpanTimer();
            }

            return null;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Starts the timer.
        /// </summary>
        /// <param name="timerInput">A <see cref="TimerInput"/>.</param>
        /// <exception cref="ObjectDisposedException">If the <see cref="Timer"/> has been disposed.</exception>
        public virtual void Start(TimerInput timerInput)
        {
            this.input = timerInput;
            this.OnPropertyChanged("Input");
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            this.Update();

            string format = string.Empty;
            switch (this.State)
            {
                case TimerState.Running:
                    format = "{0} \u2794 {1}";
                    break;

                case TimerState.Paused:
                    format = "{0} \u2794 {1} (Paused)";
                    break;

                case TimerState.Expired:
                    format = "{1} (Expired)";
                    break;
            }

            return string.Format(format, TimeSpanUtility.ToNaturalString(this.TimeLeft), this.input);
        }

        #endregion

        #region Protected Methods (Events)

        /// <summary>
        /// Invoked before the <see cref="Timer.Started"/> event is raised
        /// </summary>
        protected override void OnStarted()
        {
            this.UpdateHourglassTimer();
            base.OnStarted();
        }

        /// <summary>
        /// Invoked before the <see cref="Timer.Paused"/> event is raised
        /// </summary>
        protected override void OnPaused()
        {
            this.UpdateHourglassTimer();
            base.OnPaused();
        }

        /// <summary>
        /// Invoked before the <see cref="Timer.Resumed"/> event is raised
        /// </summary>
        protected override void OnResumed()
        {
            this.UpdateHourglassTimer();
            base.OnResumed();
        }

        /// <summary>
        /// Invoked before the <see cref="Timer.Stopped"/> event is raised
        /// </summary>
        protected override void OnStopped()
        {
            this.UpdateHourglassTimer();
            base.OnStopped();
        }

        /// <summary>
        /// Invoked before the <see cref="Timer.Expired"/> event is raised
        /// </summary>
        protected override void OnExpired()
        {
            this.UpdateHourglassTimer();
            base.OnExpired();
        }

        /// <summary>
        /// Invoked before the <see cref="Timer.Tick"/> event is raised
        /// </summary>
        protected override void OnTick()
        {
            this.UpdateHourglassTimer();
            base.OnTick();
        }

        /// <summary>
        /// Sets the properties on a <see cref="TimerInfo"/> from the values in this class.
        /// </summary>
        /// <param name="timerInfo">A <see cref="TimerInfo"/>.</param>
        protected override void SetTimerInputInfo(TimerInfo timerInfo)
        {
            base.SetTimerInputInfo(timerInfo);

            HourglassTimerInfo info = (HourglassTimerInfo)timerInfo;
            info.Options = TimerOptionsInfo.FromTimerOptions(this.Options);
            info.Input = TimerInputInfo.FromTimerInput(this.Input);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Updates the <see cref="HourglassTimer"/> state.
        /// </summary>
        private void UpdateHourglassTimer()
        {
            this.input = this.State != TimerState.Stopped ? this.input : null;
            this.timeLeftAsPercentage = this.GetTimeLeftAsPercentage();
            this.timeElapsedAsPercentage = this.GetTimeElapsedAsPercentage();
            this.timeLeftAsString = this.GetTimeLeftAsString();
            this.timeElapsedAsString = this.GetTimeElapsedAsString();

            this.OnPropertyChanged("Input", "TimeLeftAsPercentage", "TimeElapsedAsPercentage", "TimeLeftAsString", "TimeElapsedAsString");
        }

        /// <summary>
        /// Returns the percentage of time left until the timer expires.
        /// </summary>
        /// <returns>The percentage of time left until the timer expires.</returns>
        private double? GetTimeLeftAsPercentage()
        {
            if (!this.SupportsProgress || this.State == TimerState.Stopped || !this.TimeElapsed.HasValue || !this.TotalTime.HasValue)
            {
                return null;
            }

            if (this.State == TimerState.Expired)
            {
                return 100.0;
            }

            long timeElapsed = this.TimeElapsed.Value.Ticks;
            long totalTime = this.TotalTime.Value.Ticks;

            if (totalTime == 0)
            {
                return 0.0;
            }

            return 100.0 * timeElapsed / totalTime;
        }

        /// <summary>
        /// Returns the percentage of time elapsed since the timer was started.
        /// </summary>
        /// <returns>The percentage of time elapsed since the timer was started.</returns>
        private double? GetTimeElapsedAsPercentage()
        {
            if (!this.SupportsProgress || !this.SupportsTimeElapsed || this.State == TimerState.Stopped || !this.TimeLeft.HasValue || !this.TotalTime.HasValue)
            {
                return null;
            }

            if (this.State == TimerState.Expired)
            {
                return 0.0;
            }

            long timeLeft = this.TimeLeft.Value.Ticks;
            long totalTime = this.TotalTime.Value.Ticks;

            if (totalTime == 0)
            {
                return 100.0;
            }
            
            return 100.0 * timeLeft / totalTime;
        }

        /// <summary>
        /// Returns the string representation of the time left until the timer expires.
        /// </summary>
        /// <returns>The string representation of the time left until the timer expires.</returns>
        private string GetTimeLeftAsString()
        {
            if (this.State == TimerState.Stopped)
            {
                return "Timer stopped";
            }

            if (this.State == TimerState.Expired)
            {
                return "Timer expired";
            }

            return TimeSpanUtility.ToNaturalString(this.TimeLeft);
        }

        /// <summary>
        /// Returns the string representation of the time elapsed since the timer was started.
        /// </summary>
        /// <returns>The string representation of the time elapsed since the timer was started.</returns>
        private string GetTimeElapsedAsString()
        {
            if (!this.SupportsTimeElapsed)
            {
                return null;
            }

            if (this.State == TimerState.Stopped)
            {
                return "Timer stopped";
            }

            if (this.State == TimerState.Expired)
            {
                return "Timer expired";
            }

            return TimeSpanUtility.ToNaturalString(this.TimeElapsed);
        }

        #endregion
    }
}
