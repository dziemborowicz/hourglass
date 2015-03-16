// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Timer.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass
{
    using System;
    using System.ComponentModel;
    using System.Windows.Threading;

    /// <summary>
    /// The state of a <see cref="Timer"/>.
    /// </summary>
    public enum TimerState
    {
        /// <summary>
        /// Indicates that the <see cref="Timer"/> is stopped.
        /// </summary>
        Stopped,

        /// <summary>
        /// Indicates that the <see cref="Timer"/> is running.
        /// </summary>
        Running,

        /// <summary>
        /// Indicates that the <see cref="Timer"/> is paused.
        /// </summary>
        Paused,

        /// <summary>
        /// Indicates that the <see cref="Timer"/> is expired.
        /// </summary>
        Expired
    }

    /// <summary>
    /// A countdown timer that that counts down a specified <see cref="TimeSpan"/> or to a specified <see
    /// cref="DateTime"/>.
    /// </summary>
    public class Timer : IDisposable, INotifyPropertyChanged
    {
        #region Private Members

        /// <summary>
        /// A <see cref="DispatcherTimer"/> used to raise events.
        /// </summary>
        private DispatcherTimer ticker;

        /// <summary>
        /// A <see cref="string"/> title or description of this timer.
        /// </summary>
        private string title;

        /// <summary>
        /// The <see cref="TimerState"/> of this timer.
        /// </summary>
        private TimerState state = TimerState.Stopped;

        /// <summary>
        /// The <see cref="DateTime"/> that this timer was started if the <see cref="State"/> is <see
        /// cref="TimerState.Running"/> and this timer is counting down a <see cref="TimeSpan"/>, or <c>null</c>
        /// otherwise.
        /// </summary>
        private DateTime? startTime;

        /// <summary>
        /// The <see cref="DateTime"/> that this timer will expire if the <see cref="State"/> is <see
        /// cref="TimerState.Running"/>, or <c>null</c> otherwise.
        /// </summary>
        private DateTime? endTime;

        /// <summary>
        /// A <see cref="TimeSpan"/> representing the time left until this timer expires if the <see cref="State"/> is
        /// <see cref="TimerState.Running"/>, or <c>null</c> otherwise.
        /// </summary>
        private TimeSpan? timeLeft;

        /// <summary>
        /// A <see cref="TimeSpan"/> representing the total time that this timer will run if the <see cref="State"/> is
        /// <see cref="TimerState.Running"/> and this timer is counting down a <see cref="TimeSpan"/>, or <c>null</c>
        /// otherwise.
        /// </summary>
        private TimeSpan? totalTime;

        /// <summary>
        /// Indicates whether this object has been disposed.
        /// </summary>
        private bool disposed;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Timer"/> class.
        /// </summary>
        /// <param name="dispatcher">The <see cref="Dispatcher"/> to use when raising events.</param>
        public Timer(Dispatcher dispatcher)
        {
            this.ticker = new DispatcherTimer(DispatcherPriority.Normal, dispatcher);
            this.ticker.Tick += (s, e) => this.DispatcherTimerTick();
            this.ticker.Interval = new TimeSpan(0, 0, 0, 0, 100);
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
        /// Raised when the timer is reset.
        /// </summary>
        public event EventHandler Reseted;

        /// <summary>
        /// Raised when the timer expires.
        /// </summary>
        public event EventHandler Expired;

        /// <summary>
        /// Raised when the timer ticks.
        /// </summary>
        /// <seealso cref="Interval"/>
        public event EventHandler Tick;

        /// <summary>
        /// Raised when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a title or description of this timer.
        /// </summary>
        public string Title
        {
            get
            {
                return this.title;
            }

            set
            {
                if (this.title == value)
                {
                    return;
                }

                this.title = value;
                this.OnPropertyChanged("Title");
            }
        }

        /// <summary>
        /// Gets the <see cref="TimerState"/> of this timer.
        /// </summary>
        public TimerState State
        {
            get { return this.state; }
        }

        /// <summary>
        /// Gets the <see cref="DateTime"/> that this timer was started if the <see cref="State"/> is <see
        /// cref="TimerState.Running"/> and this timer is counting down a <see cref="TimeSpan"/>, or <c>null</c>
        /// otherwise.
        /// </summary>
        public DateTime? StartTime
        {
            get { return this.startTime; }
        }

        /// <summary>
        /// Gets the <see cref="DateTime"/> that this timer will expire if the <see cref="State"/> is <see
        /// cref="TimerState.Running"/>, or <c>null</c> otherwise.
        /// </summary>
        public DateTime? EndTime
        {
            get { return this.endTime; }
        }

        /// <summary>
        /// Gets a <see cref="TimeSpan"/> representing the time left until this timer expires if the <see
        /// cref="State"/> is <see cref="TimerState.Running"/>, or <c>null</c> otherwise.
        /// </summary>
        public TimeSpan? TimeLeft
        {
            get { return this.timeLeft; }
        }

        /// <summary>
        /// Gets a <see cref="TimeSpan"/> representing the total time that this timer will run if the <see
        /// cref="State"/> is <see cref="TimerState.Running"/> and this timer is counting down a <see cref="TimeSpan"/>,
        /// or <c>null</c> otherwise.
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
                return this.ticker.Interval;
            }

            set
            {
                if (this.ticker.Interval == value)
                {
                    return;
                }

                this.ticker.Interval = value;
                this.OnPropertyChanged("Interval");
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Starts the timer counting down a <see cref="TimeSpan"/> or to a <see cref="DateTime"/>.
        /// </summary>
        /// <param name="input">A <see cref="TimeSpan"/> or <see cref="DateTime"/>.</param>
        /// <exception cref="ArgumentException">If the <paramref name="input"/> is not a <see cref="TimeSpan"/> or a
        /// <see cref="DateTime"/>.</exception>
        /// <exception cref="ObjectDisposedException">If the <see cref="Timer"/> has been disposed.</exception>
        public void Start(object input)
        {
            if (this.disposed)
            {
                throw new ObjectDisposedException(this.GetType().ToString());
            }

            if (input is TimeSpan)
            {
                this.Start((TimeSpan)input);
                return;
            }

            if (input is DateTime)
            {
                this.Start((DateTime)input);
                return;
            }

            throw new ArgumentException("input must be a TimeSpan or DateTime.", "input");
        }

        /// <summary>
        /// Starts the timer counting down a <see cref="TimeSpan"/>.
        /// </summary>
        /// <param name="timeSpan">A <see cref="TimeSpan"/>.</param>
        /// <exception cref="ObjectDisposedException">If the <see cref="Timer"/> has been disposed.</exception>
        public void Start(TimeSpan timeSpan)
        {
            if (this.disposed)
            {
                throw new ObjectDisposedException(this.GetType().ToString());
            }

            this.state = TimerState.Running;
            this.startTime = DateTime.Now;
            this.endTime = this.startTime + timeSpan;
            this.timeLeft = timeSpan;
            this.totalTime = timeSpan;

            this.StartDispatcherTimer();
            this.OnPropertyChanged("State", "StartTime", "EndTime", "TimeLeft", "TotalTime");
            this.OnStarted();
        }

        /// <summary>
        /// Starts the timer counting down to a <see cref="DateTime"/>.
        /// </summary>
        /// <param name="dateTime">A <see cref="DateTime"/>.</param>
        /// <exception cref="ObjectDisposedException">If the <see cref="Timer"/> has been disposed.</exception>
        public void Start(DateTime dateTime)
        {
            if (this.disposed)
            {
                throw new ObjectDisposedException(this.GetType().ToString());
            }

            this.state = TimerState.Running;
            this.startTime = null;
            this.endTime = dateTime;
            this.timeLeft = dateTime - DateTime.Now;
            this.timeLeft = (this.timeLeft > TimeSpan.Zero) ? this.timeLeft : TimeSpan.Zero;
            this.totalTime = null;

            this.StartDispatcherTimer();
            this.OnPropertyChanged("State", "StartTime", "EndTime", "TimeLeft", "TotalTime");
            this.OnStarted();
        }

        /// <summary>
        /// Pauses the timer.
        /// </summary>
        /// <remarks>
        /// If the timer is counting down to a specified <see cref="DateTime"/> or the <see cref="State"/> is not
        /// <see cref="TimerState.Running"/>, this method does nothing.
        /// </remarks>
        /// <exception cref="ObjectDisposedException">If the <see cref="Timer"/> has been disposed.</exception>
        public void Pause()
        {
            if (this.disposed)
            {
                throw new ObjectDisposedException(this.GetType().ToString());
            }

            if (this.state != TimerState.Running || !this.totalTime.HasValue)
            {
                return;
            }

            this.state = TimerState.Paused;
            this.timeLeft = this.endTime - DateTime.Now;

            this.StopDispatcherTimer();
            this.OnPropertyChanged("State", "TimeLeft");
            this.OnPaused();
        }

        /// <summary>
        /// Resumes the timer if the timer is paused.
        /// </summary>
        /// <remarks>
        /// If the <see cref="State"/> is not <see cref="TimerState.Paused"/>, this method does nothing.
        /// </remarks>
        /// <exception cref="ObjectDisposedException">If the <see cref="Timer"/> has been disposed.</exception>
        public void Resume()
        {
            if (this.disposed)
            {
                throw new ObjectDisposedException(this.GetType().ToString());
            }

            if (this.state != TimerState.Paused)
            {
                return;
            }

            this.state = TimerState.Running;
            this.endTime = DateTime.Now + this.timeLeft;
            this.startTime = this.endTime - this.totalTime;

            this.StartDispatcherTimer();
            this.OnPropertyChanged("State", "StartTime", "EndTime");
            this.OnResumed();
        }

        /// <summary>
        /// Stops the timer.
        /// </summary>
        /// <remarks>
        /// If the <see cref="State"/> is <see cref="TimerState.Stopped"/>, this method does nothing.
        /// </remarks>
        /// <exception cref="ObjectDisposedException">If the <see cref="Timer"/> has been disposed.</exception>
        public void Stop()
        {
            if (this.disposed)
            {
                throw new ObjectDisposedException(this.GetType().ToString());
            }

            if (this.state == TimerState.Stopped)
            {
                return;
            }

            this.state = TimerState.Stopped;
            this.startTime = null;
            this.endTime = null;
            this.timeLeft = null;
            this.totalTime = null;

            this.StopDispatcherTimer();
            this.OnPropertyChanged("State", "StartTime", "EndTime", "TimeLeft", "TotalTime");
            this.OnStopped();
        }

        /// <summary>
        /// Resets the timer.
        /// </summary>
        /// <remarks>
        /// If the <see cref="State"/> is not <see cref="TimerState.Stopped"/>, this method first stops the timer.
        /// </remarks>
        /// <exception cref="ObjectDisposedException">If the <see cref="Timer"/> has been disposed.</exception>
        public void Reset()
        {
            if (this.disposed)
            {
                throw new ObjectDisposedException(this.GetType().ToString());
            }

            this.Stop();
            this.OnReseted();
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
        protected void OnStarted()
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
        protected void OnPaused()
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
        protected void OnResumed()
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
        protected void OnStopped()
        {
            EventHandler eventHandler = this.Stopped;

            if (eventHandler != null)
            {
                eventHandler(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Raises the <see cref="Reseted"/> event.
        /// </summary>
        protected void OnReseted()
        {
            EventHandler eventHandler = this.Reseted;

            if (eventHandler != null)
            {
                eventHandler(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Raises the <see cref="Expired"/> event.
        /// </summary>
        protected void OnExpired()
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
        protected void OnTick()
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
        protected void OnPropertyChanged(params string[] propertyNames)
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
                if (this.ticker != null)
                {
                    this.ticker.Stop();
                    this.ticker = null;
                }
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Invoked when the timer interval has elapsed.
        /// </summary>
        private void DispatcherTimerTick()
        {
            if (this.state != TimerState.Running)
            {
                return;
            }

            // Update time left
            this.timeLeft = this.endTime - DateTime.Now;
            this.timeLeft = (this.timeLeft > TimeSpan.Zero) ? this.timeLeft : TimeSpan.Zero;

            // Check if expired
            if (this.timeLeft <= TimeSpan.Zero)
            {
                this.state = TimerState.Expired;

                this.StopDispatcherTimer();
                this.OnPropertyChanged("State", "TimeLeft");
                this.OnExpired();
                return;
            }

            // Raise event
            this.OnPropertyChanged("TimeLeft");
            this.OnTick();
        }

        /// <summary>
        /// Starts the <see cref="DispatcherTimer"/>.
        /// </summary>
        private void StartDispatcherTimer()
        {
            this.DispatcherTimerTick();
            this.ticker.Start();
        }

        /// <summary>
        /// Stops the <see cref="DispatcherTimer"/>.
        /// </summary>
        private void StopDispatcherTimer()
        {
            this.ticker.Stop();
        }

        #endregion
    }
}
