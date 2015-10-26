// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimerBase.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass.Timing
{
    using System;
    using System.ComponentModel;
    using System.Windows.Threading;

    using Hourglass.Extensions;
    using Hourglass.Serialization;

    /// <summary>
    /// The state of a timer.
    /// </summary>
    public enum TimerState
    {
        /// <summary>
        /// Indicates that the timer is stopped.
        /// </summary>
        Stopped,

        /// <summary>
        /// Indicates that the timer is running.
        /// </summary>
        Running,

        /// <summary>
        /// Indicates that the timer is paused.
        /// </summary>
        Paused,

        /// <summary>
        /// Indicates that the timer is expired.
        /// </summary>
        Expired
    }

    /// <summary>
    /// A countdown timer.
    /// </summary>
    public abstract class TimerBase : IDisposable, INotifyPropertyChanged
    {
        #region Constants

        /// <summary>
        /// The default period of time between timer ticks.
        /// </summary>
        public static readonly TimeSpan DefaultInterval = TimeSpan.FromMilliseconds(250);

        #endregion

        #region Private Members

        /// <summary>
        /// The <see cref="TimerState"/> of this timer.
        /// </summary>
        private TimerState state = TimerState.Stopped;

        /// <summary>
        /// The <see cref="DateTime"/> that this timer was started if the <see cref="State"/> is <see
        /// cref="TimerState.Running"/> or <see cref="TimerState.Expired"/>, or <c>null</c> otherwise.
        /// </summary>
        private DateTime? startTime;
        
        /// <summary>
        /// The <see cref="DateTime"/> that this timer will expire or has expired if the <see cref="State"/> is <see
        /// cref="TimerState.Running"/> or <see cref="TimerState.Expired"/>, or <c>null</c> otherwise.
        /// </summary>
        private DateTime? endTime;

        /// <summary>
        /// A <see cref="TimeSpan"/> representing the time elapsed since this timer started if the <see cref="State"/>
        /// is <see cref="TimerState.Running"/>, <see cref="TimerState.Paused"/>, or <see cref="TimerState.Expired"/>,
        /// or <c>null</c> otherwise.
        /// </summary>
        private TimeSpan? timeElapsed;

        /// <summary>
        /// A <see cref="TimeSpan"/> representing the time left until this timer expires if the <see cref="State"/> is
        /// <see cref="TimerState.Running"/> or <see cref="TimerState.Paused"/>, <see cref="TimeSpan.Zero"/> if the
        /// <see cref="State"/> is <see cref="TimerState.Expired"/>, or <c>null</c> otherwise.
        /// </summary>
        private TimeSpan? timeLeft;

        /// <summary>
        /// A <see cref="TimeSpan"/> representing the time since this timer has expired if the <see cref="State"/> is
        /// <see cref="TimerState.Expired"/>, <see cref="TimeSpan.Zero"/> if the <see cref="State"/> is <see
        /// cref="TimerState.Running"/> or <see cref="TimerState.Paused"/>, or <c>null</c> otherwise.
        /// </summary>
        private TimeSpan? timeExpired;

        /// <summary>
        /// A <see cref="TimeSpan"/> representing the total time that this timer will run for or has run for if the
        /// <see cref="State"/> is <see cref="TimerState.Running"/> or <see cref="TimerState.Expired"/>, or <c>null</c>
        /// otherwise.
        /// </summary>
        private TimeSpan? totalTime;

        /// <summary>
        /// A <see cref="DispatcherTimer"/>.
        /// </summary>
        private DispatcherTimer dispatcherTimer;

        /// <summary>
        /// Indicates whether this object has been disposed.
        /// </summary>
        private bool disposed;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TimerBase"/> class.
        /// </summary>
        protected TimerBase()
        {
            this.dispatcherTimer = new DispatcherTimer();
            this.dispatcherTimer.Interval = DefaultInterval;
            this.dispatcherTimer.Tick += (s, e) => this.Update();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TimerBase"/> class.
        /// </summary>
        /// <param name="timerInfo">A <see cref="TimerInfo"/> representing the state of the <see cref="TimerBase"/>.</param>
        protected TimerBase(TimerInfo timerInfo)
            : this()
        {
            this.state = timerInfo.State;
            this.startTime = timerInfo.StartTime;
            this.endTime = timerInfo.EndTime;
            this.timeElapsed = timerInfo.TimeElapsed;
            this.timeLeft = timerInfo.TimeLeft;
            this.timeExpired = timerInfo.TimeExpired;
            this.totalTime = timerInfo.TotalTime;

            if (this.state == TimerState.Running)
            {
                this.dispatcherTimer.Start();
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// Raised when the timer is started.
        /// </summary>
        public event EventHandler Started;

        /// <summary>
        /// Raised when the timer is paused.
        /// </summary>
        public event EventHandler Paused;

        /// <summary>
        /// Raised when the timer is resumed from a paused state.
        /// </summary>
        public event EventHandler Resumed;

        /// <summary>
        /// Raised when the timer is stopped.
        /// </summary>
        public event EventHandler Stopped;

        /// <summary>
        /// Raised when the timer expires.
        /// </summary>
        public event EventHandler Expired;

        /// <summary>
        /// Raised when the timer ticks.
        /// </summary>
        public event EventHandler Tick;

        /// <summary>
        /// Raised when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the <see cref="TimerState"/> of this timer.
        /// </summary>
        public TimerState State
        {
            get { return this.state; }
        }

        /// <summary>
        /// Gets the <see cref="DateTime"/> that this timer was started if the <see cref="State"/> is <see
        /// cref="TimerState.Running"/> or <see cref="TimerState.Expired"/>, or <c>null</c> otherwise.
        /// </summary>
        public DateTime? StartTime
        {
            get { return this.startTime; }
        }

        /// <summary>
        /// Gets the <see cref="DateTime"/> that this timer will expire or has expired if the <see cref="State"/> is
        /// <see cref="TimerState.Running"/> or <see cref="TimerState.Expired"/>, or <c>null</c> otherwise.
        /// </summary>
        public DateTime? EndTime
        {
            get { return this.endTime; }
        }

        /// <summary>
        /// Gets a <see cref="TimeSpan"/> representing the time elapsed since this timer started if the <see
        /// cref="State"/> is <see cref="TimerState.Running"/>, <see cref="TimerState.Paused"/>, or <see
        /// cref="TimerState.Expired"/>, or <c>null</c> otherwise.
        /// </summary>
        public TimeSpan? TimeElapsed
        {
            get { return this.timeElapsed; }
        }

        /// <summary>
        /// Gets a <see cref="TimeSpan"/> representing the time left until this timer expires if the <see
        /// cref="State"/> is <see cref="TimerState.Running"/> or <see cref="TimerState.Paused"/>, <see
        /// cref="TimeSpan.Zero"/> if the <see cref="State"/> is <see cref="TimerState.Expired"/>, or <c>null</c>
        /// otherwise.
        /// </summary>
        public TimeSpan? TimeLeft
        {
            get { return this.timeLeft; }
        }

        /// <summary>
        /// Gets a <see cref="TimeSpan"/> representing the time since this timer has expired if the <see cref="State"/>
        /// is <see cref="TimerState.Expired"/>, <see cref="TimeSpan.Zero"/> if the <see cref="State"/> is <see
        /// cref="TimerState.Running"/> or <see cref="TimerState.Paused"/>, or <c>null</c> otherwise.
        /// </summary>
        public TimeSpan? TimeExpired
        {
            get { return this.timeExpired; }
        }

        /// <summary>
        /// Gets a <see cref="TimeSpan"/> representing the total time that this timer will run for or has run for if
        /// the <see cref="State"/> is <see cref="TimerState.Running"/> or <see cref="TimerState.Expired"/>, or
        /// <c>null</c> otherwise.
        /// </summary>
        public TimeSpan? TotalTime
        {
            get { return this.totalTime; }
        }

        /// <summary>
        /// Gets or sets the period of time between timer ticks.
        /// </summary>
        /// <seealso cref="Tick"/>
        public TimeSpan Interval
        {
            get
            {
                return this.dispatcherTimer.Interval; 
            }

            set
            {
                if (this.dispatcherTimer.Interval == value)
                {
                    return;
                }

                this.dispatcherTimer.Interval = value;
                this.OnPropertyChanged("Interval");
            }
        }

        /// <summary>
        /// Gets a value indicating whether this object has been disposed.
        /// </summary>
        public bool Disposed
        {
            get { return this.disposed; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Starts the timer.
        /// </summary>
        /// <param name="start">The <see cref="DateTime"/> the timer was started.</param>
        /// <param name="end">The <see cref="DateTime"/> the timer expires.</param>
        /// <exception cref="ObjectDisposedException">If the timer has been disposed.</exception>
        public virtual void Start(DateTime start, DateTime end)
        {
            this.ThrowIfDisposed();

            this.state = TimerState.Running;
            this.startTime = MathExtensions.Min(start, end);
            this.endTime = end;
            this.timeElapsed = TimeSpan.Zero;
            this.timeLeft = this.endTime - this.startTime;
            this.timeExpired = TimeSpan.Zero;
            this.totalTime = this.timeLeft;

            this.OnPropertyChanged("State", "StartTime", "EndTime", "TimeElapsed", "TimeLeft", "TimeExpired", "TotalTime");
            this.OnStarted();

            this.Update();
            this.dispatcherTimer.Start();
        }

        /// <summary>
        /// Pauses the timer.
        /// </summary>
        /// <remarks>
        /// If the <see cref="State"/> is not <see cref="TimerState.Running"/>, this method does nothing.
        /// </remarks>
        /// <exception cref="ObjectDisposedException">If the timer has been disposed.</exception>
        public virtual void Pause()
        {
            this.ThrowIfDisposed();

            if (this.state != TimerState.Running)
            {
                return;
            }

            DateTime now = DateTime.Now;
            this.state = TimerState.Paused;
            this.timeElapsed = MathExtensions.Min(now - (this.startTime ?? now), this.totalTime ?? TimeSpan.Zero);
            this.timeLeft = MathExtensions.Max((this.endTime ?? now) - now, TimeSpan.Zero);
            this.timeExpired = TimeSpan.Zero;
            this.startTime = null;
            this.endTime = null;

            this.dispatcherTimer.Stop();

            this.OnPropertyChanged("State", "StartTime", "EndTime", "TimeElapsed", "TimeExpired", "TimeLeft");
            this.OnPaused();
        }

        /// <summary>
        /// Resumes the timer if the timer is paused.
        /// </summary>
        /// <remarks>
        /// If the <see cref="State"/> is not <see cref="TimerState.Paused"/>, this method does nothing.
        /// </remarks>
        /// <exception cref="ObjectDisposedException">If the timer has been disposed.</exception>
        public virtual void Resume()
        {
            this.ThrowIfDisposed();

            if (this.state != TimerState.Paused)
            {
                return;
            }

            this.state = TimerState.Running;
            this.endTime = DateTime.Now + this.timeLeft;
            this.startTime = this.endTime - this.totalTime;

            this.OnPropertyChanged("State", "StartTime", "EndTime");
            this.OnResumed();

            this.Update();
            this.dispatcherTimer.Start();
        }

        /// <summary>
        /// Stops the timer.
        /// </summary>
        /// <remarks>
        /// If the <see cref="State"/> is <see cref="TimerState.Stopped"/>, this method does nothing.
        /// </remarks>
        /// <exception cref="ObjectDisposedException">If the timer has been disposed.</exception>
        public virtual void Stop()
        {
            this.ThrowIfDisposed();

            if (this.state == TimerState.Stopped)
            {
                return;
            }

            this.state = TimerState.Stopped;
            this.startTime = null;
            this.endTime = null;
            this.timeElapsed = null;
            this.timeLeft = null;
            this.timeExpired = null;
            this.totalTime = null;

            this.dispatcherTimer.Stop();

            this.OnPropertyChanged("State", "StartTime", "EndTime", "TimeLeft", "TimeExpired", "TotalTime");
            this.OnStopped();
        }

        /// <summary>
        /// Updates the state of the timer.
        /// </summary>
        /// <remarks>
        /// When the timer is running, this method is periodically invoked to update the state of the timer.
        /// </remarks>
        public virtual void Update()
        {
            this.ThrowIfDisposed();

            if (this.state != TimerState.Running && this.state != TimerState.Expired)
            {
                return;
            }

            // Update timer state
            DateTime now = DateTime.Now;
            this.timeElapsed = MathExtensions.Min(now - (this.startTime ?? now), this.totalTime ?? TimeSpan.Zero);
            this.timeLeft = MathExtensions.Max((this.endTime ?? now) - now, TimeSpan.Zero);
            this.timeExpired = MathExtensions.Max(now - (this.endTime ?? now), TimeSpan.Zero);

            // Raise an event when the timer expires
            if (this.timeLeft <= TimeSpan.Zero && this.state == TimerState.Running)
            {
                this.state = TimerState.Expired;

                this.OnPropertyChanged("State");
                this.OnExpired();
            }

            // Raise other events
            this.OnPropertyChanged("TimeElapsed", "TimeLeft", "TimeExpired");
            this.OnTick();
        }

        /// <summary>
        /// Returns the representation of the <see cref="TimerInfo"/> used for XML serialization.
        /// </summary>
        /// <returns>The representation of the <see cref="TimerInfo"/> used for XML serialization.</returns>
        public virtual TimerInfo ToTimerInfo()
        {
            return new TimerInfo
            {
                State = this.state,
                StartTime = this.startTime,
                EndTime = this.endTime,
                TimeElapsed = this.timeElapsed,
                TimeLeft = this.timeLeft,
                TimeExpired = this.timeExpired,
                TotalTime = this.totalTime
            };
        }

        /// <summary>
        /// Disposes the timer.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true /* disposing */);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Raises the <see cref="Started"/> event.
        /// </summary>
        protected virtual void OnStarted()
        {
            EventHandler eventHandler = this.Started;

            if (eventHandler != null)
            {
                eventHandler(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Raises the <see cref="Paused"/> event.
        /// </summary>
        protected virtual void OnPaused()
        {
            EventHandler eventHandler = this.Paused;

            if (eventHandler != null)
            {
                eventHandler(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Raises the <see cref="Resumed"/> event.
        /// </summary>
        protected virtual void OnResumed()
        {
            EventHandler eventHandler = this.Resumed;

            if (eventHandler != null)
            {
                eventHandler(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Raises the <see cref="Stopped"/> event.
        /// </summary>
        protected virtual void OnStopped()
        {
            EventHandler eventHandler = this.Stopped;

            if (eventHandler != null)
            {
                eventHandler(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Raises the <see cref="Expired"/> event.
        /// </summary>
        protected virtual void OnExpired()
        {
            EventHandler eventHandler = this.Expired;

            if (eventHandler != null)
            {
                eventHandler(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Raises the <see cref="Tick"/> event.
        /// </summary>
        protected virtual void OnTick()
        {
            EventHandler eventHandler = this.Tick;

            if (eventHandler != null)
            {
                eventHandler(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <param name="propertyNames">One or more property names.</param>
        protected virtual void OnPropertyChanged(params string[] propertyNames)
        {
            PropertyChangedEventHandler eventHandler = this.PropertyChanged;

            if (eventHandler != null)
            {
                foreach (string propertyName in propertyNames)
                {
                    eventHandler(this, new PropertyChangedEventArgs(propertyName));
                }
            }
        }

        /// <summary>
        /// Disposes the timer.
        /// </summary>
        /// <param name="disposing">A value indicating whether this method was invoked by an explicit call to <see
        /// cref="Dispose"/>.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (this.disposed)
            {
                return;
            }

            this.disposed = true;

            if (disposing)
            {
                if (this.dispatcherTimer != null)
                {
                    this.dispatcherTimer.Stop();
                }
            }
        }

        /// <summary>
        /// Throws a <see cref="ObjectDisposedException"/> if the object has been disposed.
        /// </summary>
        protected void ThrowIfDisposed()
        {
            if (this.disposed)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }
        }

        #endregion
    }
}
