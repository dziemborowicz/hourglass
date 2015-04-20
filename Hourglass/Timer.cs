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
        #region Constants

        /// <summary>
        /// The default period of time between timer ticks.
        /// </summary>
        private static readonly TimeSpan DefaultInterval = new TimeSpan(0, 0, 0, 0, 100);

        #endregion

        #region Private Members

        /// <summary>
        /// A title or description of this timer.
        /// </summary>
        private string title;

        /// <summary>
        /// The <see cref="TimerState"/> of this timer.
        /// </summary>
        private TimerState state = TimerState.Stopped;

        /// <summary>
        /// The <see cref="DateTime"/> that this timer was started if the <see cref="State"/> is not <see
        /// cref="TimerState.Stopped"/> and this timer is counting down a <see cref="TimeSpan"/>, or <c>null</c>
        /// otherwise.
        /// </summary>
        private DateTime? startTime;

        /// <summary>
        /// The <see cref="DateTime"/> that this timer will expire if the <see cref="State"/> is not <see
        /// cref="TimerState.Stopped"/>, or <c>null</c> otherwise.
        /// </summary>
        private DateTime? endTime;

        /// <summary>
        /// A <see cref="TimeSpan"/> representing the time left until this timer expires if the <see cref="State"/> is
        /// not <see cref="TimerState.Stopped"/>, or <c>null</c> otherwise.
        /// </summary>
        private TimeSpan? timeLeft;

        /// <summary>
        /// A <see cref="TimeSpan"/> representing the total time that this timer will run if the <see cref="State"/> is
        /// not <see cref="TimerState.Stopped"/> and this timer is counting down a <see cref="TimeSpan"/>, or
        /// <c>null</c> otherwise.
        /// </summary>
        private TimeSpan? totalTime;

        /// <summary>
        /// A <see cref="TimerInput"/> representing the last input used to start this timer, or <c>null</c> if no input
        /// has been used to start this timer yet.
        /// </summary>
        private TimerInput lastInput;

        /// <summary>
        /// A <see cref="DispatcherTimer"/> used to raise events.
        /// </summary>
        private DispatcherTimer ticker;

        /// <summary>
        /// The period of time between timer ticks.
        /// </summary>
        private TimeSpan interval = DefaultInterval;

        /// <summary>
        /// A <see cref="TimerWindow"/> used to display the progress of this timer.
        /// </summary>
        private TimerWindow timerWindow;

        /// <summary>
        /// A value indicating whether to loop the timer continuously.
        /// </summary>
        private bool loopTimer;

        /// <summary>
        /// A value indicating whether the <see cref="TimerWindow"/> should always be displayed on top of other windows.
        /// </summary>
        private bool alwaysOnTop;

        /// <summary>
        /// A value indicating whether an icon for the <see cref="TimerWindow"/> should be shown in the notification
        /// area (system tray).
        /// </summary>
        private bool showInNotificationArea;

        /// <summary>
        /// A value indicating whether the <see cref="TimerWindow"/> should be brought to the top of other windows when
        /// the timer expires.
        /// </summary>
        private bool popUpWhenExpired;

        /// <summary>
        /// A value indicating whether the <see cref="TimerWindow"/> should be closed when the timer expires.
        /// </summary>
        private bool closeWhenExpired;

        /// <summary>
        /// The sound to play when the timer expires, or <c>null</c> if no sound is to be played.
        /// </summary>
        private Sound sound;

        /// <summary>
        /// A value indicating whether the sound that plays when the timer expires should be looped.
        /// </summary>
        private bool loopSound;

        /// <summary>
        /// Indicates whether this object has been disposed.
        /// </summary>
        private bool disposed;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Timer"/> class.
        /// </summary>
        /// <param name="args">Command-line arguments.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="args"/> is <c>null</c>.</exception>
        public Timer(string[] args)
        {
            if (args == null)
            {
                throw new ArgumentNullException("args");
            }

            // TODO Parse command-line arguments
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Timer"/> class.
        /// </summary>
        /// <param name="input">A <see cref="TimerInput"/>.</param>
        public Timer(TimerInput input)
        {
            if (input == null)
            {
                throw new ArgumentNullException("input");
            }

            this.Start(input);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Timer"/> class.
        /// </summary>
        /// <param name="timerInfo">A <see cref="TimerInfo"/> representing the state of the timer.</param>
        public Timer(TimerInfo timerInfo)
        {
            this.title = timerInfo.Title;
            this.state = timerInfo.State;
            this.startTime = timerInfo.StartTime;
            this.endTime = timerInfo.EndTime;
            this.timeLeft = timerInfo.TimeLeft;
            this.totalTime = timerInfo.TotalTime;
            this.loopTimer = timerInfo.LoopTimer;
            this.alwaysOnTop = timerInfo.AlwaysOnTop;
            this.showInNotificationArea = timerInfo.ShowInNotificationArea;
            this.popUpWhenExpired = timerInfo.PopUpWhenExpired;
            this.closeWhenExpired = timerInfo.CloseWhenExpired;

            if (!string.IsNullOrEmpty(timerInfo.SoundIdentifier))
            {
                this.sound = SoundManager.Instance.GetSoundOrDefault(timerInfo.SoundIdentifier);
            }

            this.loopSound = timerInfo.LoopSound;

            this.Update();
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
        /// Gets the <see cref="DateTime"/> that this timer was started if the <see cref="State"/> is not <see
        /// cref="TimerState.Stopped"/> and this timer is counting down a <see cref="TimeSpan"/>, or <c>null</c>
        /// otherwise.
        /// </summary>
        public DateTime? StartTime
        {
            get { return this.startTime; }
        }

        /// <summary>
        /// Gets the <see cref="DateTime"/> that this timer will expire if the <see cref="State"/> is not <see
        /// cref="TimerState.Stopped"/>, or <c>null</c> otherwise.
        /// </summary>
        public DateTime? EndTime
        {
            get { return this.endTime; }
        }

        /// <summary>
        /// Gets a <see cref="TimeSpan"/> representing the time left until this timer expires if the <see
        /// cref="State"/> is not <see cref="TimerState.Stopped"/>, or <c>null</c> otherwise.
        /// </summary>
        public TimeSpan? TimeLeft
        {
            get { return this.timeLeft; }
        }

        /// <summary>
        /// Gets a <see cref="TimeSpan"/> representing the total time that this timer will run if the <see
        /// cref="State"/> is not <see cref="TimerState.Stopped"/> and this timer is counting down a <see
        /// cref="TimeSpan"/>, or <c>null</c> otherwise.
        /// </summary>
        public TimeSpan? TotalTime
        {
            get { return this.totalTime; }
        }

        /// <summary>
        /// Gets the <see cref="TimerInput"/> representing the last input used to start this timer, or <c>null</c> if
        /// no input has been used to start this timer yet.
        /// </summary>
        public TimerInput LastInput
        {
            get { return this.lastInput; }
        }

        /// <summary>
        /// Gets or sets the period of time between timer ticks.
        /// </summary>
        /// <seealso cref="Tick"/>
        public TimeSpan Interval
        {
            get
            {
                return this.interval;
            }

            set
            {
                if (this.interval == value)
                {
                    return;
                }

                this.interval = value;

                if (this.ticker != null)
                {
                    this.ticker.Interval = value;
                }

                this.OnPropertyChanged("Interval");
            }
        }

        /// <summary>
        /// Gets the <see cref="TimerWindow"/> used to display the progress of this timer.
        /// </summary>
        public TimerWindow TimerWindow
        {
            get { return this.timerWindow; }
        }

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

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="TimerWindow"/> should always be displayed on top of
        /// other windows.
        /// </summary>
        public bool AlwaysOnTop
        {
            get
            {
                return this.alwaysOnTop;
            }

            set
            {
                if (this.alwaysOnTop == value)
                {
                    return;
                }

                this.alwaysOnTop = value;
                this.OnPropertyChanged("AlwaysOnTop");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether an icon for the <see cref="TimerWindow"/> should be shown in the
        /// notification area (system tray).
        /// </summary>
        public bool ShowInNotificationArea
        {
            get
            {
                return this.showInNotificationArea;
            }

            set
            {
                if (this.showInNotificationArea == value)
                {
                    return;
                }

                this.showInNotificationArea = value;
                this.OnPropertyChanged("ShowInNotificationArea");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="TimerWindow"/> should be brought to the top of other
        /// windows when the timer expires.
        /// </summary>
        public bool PopUpWhenExpired
        {
            get
            {
                return this.popUpWhenExpired;
            }

            set
            {
                if (this.popUpWhenExpired == value)
                {
                    return;
                }

                this.popUpWhenExpired = value;
                this.OnPropertyChanged("PopUpWhenExpired");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="TimerWindow"/> should be closed when the timer
        /// expires.
        /// </summary>
        public bool CloseWhenExpired
        {
            get
            {
                return this.closeWhenExpired;
            }

            set
            {
                if (this.closeWhenExpired == value)
                {
                    return;
                }

                this.closeWhenExpired = value;
                this.OnPropertyChanged("CloseWhenExpired");
            }
        }

        /// <summary>
        /// Gets or sets the sound to play when the timer expires. Set to <c>null</c> if no sound is to be played.
        /// </summary>
        public Sound Sound
        {
            get
            {
                return this.sound;
            }

            set
            {
                if (this.sound == value)
                {
                    return;
                }

                this.sound = value;
                this.OnPropertyChanged("Sound");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the sound that plays when the timer expires should be looped.
        /// </summary>
        public bool LoopSound
        {
            get
            {
                return this.loopSound;
            }

            set
            {
                if (this.loopSound == value)
                {
                    return;
                }

                this.loopSound = value;
                this.OnPropertyChanged("LoopSound");
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Starts the timer counting down a <see cref="TimeSpan"/> or to a <see cref="DateTime"/>.
        /// </summary>
        /// <param name="input">A <see cref="TimerInput"/>.</param>
        /// <exception cref="ObjectDisposedException">If the <see cref="Timer"/> has been disposed.</exception>
        public void Start(TimerInput input)
        {
            if (this.disposed)
            {
                throw new ObjectDisposedException(this.GetType().ToString());
            }

            TimeSpanTimerInput timeSpanTimerInput = input as TimeSpanTimerInput;
            if (timeSpanTimerInput != null)
            {
                this.Start(timeSpanTimerInput.TimeSpan);
                return;
            }

            DateTimeTimerInput dateTimeTimerInput = input as DateTimeTimerInput;
            if (dateTimeTimerInput != null)
            {
                this.Start(dateTimeTimerInput.DateTime);
                return;
            }

            throw new ArgumentException("Unsupported TimerInput implementation", "input");
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

            TimerInput input = new TimeSpanTimerInput(timeSpan);

            this.state = TimerState.Running;
            this.startTime = DateTime.Now;
            this.endTime = this.startTime + timeSpan;
            this.timeLeft = timeSpan;
            this.totalTime = timeSpan;
            this.lastInput = input;

            this.StartDispatcherTimer();
            this.OnPropertyChanged("State", "StartTime", "EndTime", "TimeLeft", "TotalTime", "LastInput");
            this.OnStarted();

            TimerInputManager.Instance.Add(input);
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

            TimerInput input = new DateTimeTimerInput(dateTime);

            this.state = TimerState.Running;
            this.startTime = null;
            this.endTime = dateTime;
            this.timeLeft = dateTime - DateTime.Now;
            this.timeLeft = (this.timeLeft > TimeSpan.Zero) ? this.timeLeft : TimeSpan.Zero;
            this.totalTime = null;
            this.lastInput = input;

            this.StartDispatcherTimer();
            this.OnPropertyChanged("State", "StartTime", "EndTime", "TimeLeft", "TotalTime", "LastInput");
            this.OnStarted();

            TimerInputManager.Instance.Add(input);
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
        /// Updates the state of the timer.
        /// </summary>
        /// <remarks>
        /// When the timer is running, this method is periodically invoked to update the state of the timer.
        /// </remarks>
        public void Update()
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
        /// Binds the timer to a <see cref="TimerWindow"/>.
        /// </summary>
        /// <remarks>
        /// The timer must be bound to a <see cref="TimerWindow"/> before it can be started because <see cref="Tick"/>
        /// events are invoked using the <see cref="Dispatcher"/> associated with the bound window.
        /// </remarks>
        /// <param name="window">A <see cref="TimerWindow"/>.</param>
        /// <exception cref="InvalidOperationException">If this method is invoked more than once.</exception>
        public void Bind(TimerWindow window)
        {
            if (this.timerWindow != null)
            {
                throw new InvalidOperationException("A Timer can only be bound to one Window.");
            }
            
            // Bind the window
            this.timerWindow = window;

            // Create a DispatcherTimer
            this.ticker = new DispatcherTimer(DispatcherPriority.Normal, this.timerWindow.Dispatcher);
            this.ticker.Tick += (s, e) => this.DispatcherTimerTick();
            this.ticker.Interval = this.interval;

            // If the timer is running, start raising Tick events
            if (this.state == TimerState.Running)
            {
                this.StartDispatcherTimer();
            }
        }

        /// <summary>
        /// Unbinds the timer from a <see cref="TimerWindow"/>.
        /// </summary>
        public void Unbind()
        {
            if (this.timerWindow == null)
            {
                throw new InvalidOperationException("The timer is not bound.");
            }

            // Unbind the window
            this.timerWindow = null;

            // Dispose the DispatcherTimer
            if (this.ticker != null)
            {
                this.ticker.Stop();
                this.ticker = null;
            }

            // Reset timer interval
            this.Interval = DefaultInterval;
        }

        /// <summary>
        /// Returns a <see cref="TimerInfo"/> representing the state of the timer.
        /// </summary>
        /// <returns>A <see cref="TimerInfo"/> representing the state of the timer.</returns>
        public TimerInfo ToTimerInfo()
        {
            return new TimerInfo
            {
                Title = this.title,
                State = this.state,
                StartTime = this.startTime,
                EndTime = this.endTime,
                TimeLeft = this.timeLeft,
                TotalTime = this.totalTime,
                LoopTimer = this.loopTimer,
                AlwaysOnTop = this.alwaysOnTop,
                ShowInNotificationArea = this.showInNotificationArea,
                PopUpWhenExpired = this.popUpWhenExpired,
                CloseWhenExpired = this.closeWhenExpired,
                SoundIdentifier = this.sound == null ? null : this.sound.Identifier,
                LoopSound = this.loopSound
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
            this.Update();
        }

        /// <summary>
        /// Starts the <see cref="DispatcherTimer"/>.
        /// </summary>
        private void StartDispatcherTimer()
        {
            if (this.ticker == null)
            {
                return;
            }

            this.DispatcherTimerTick();
            this.ticker.Start();
        }

        /// <summary>
        /// Stops the <see cref="DispatcherTimer"/>.
        /// </summary>
        private void StopDispatcherTimer()
        {
            if (this.ticker == null)
            {
                return;
            }

            this.ticker.Stop();
        }

        #endregion
    }
}
