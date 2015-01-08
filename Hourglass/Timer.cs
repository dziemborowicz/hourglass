using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Hourglass
{
    public class Timer : IDisposable, INotifyPropertyChanged
    {
        #region Internal State

        private DispatcherTimer ticker;

        private string title;

        private TimerState state = TimerState.Stopped;
        private DateTime? startTime;
        private DateTime? endTime;
        private TimeSpan? timeLeft;
        private TimeSpan? totalTime;

        #endregion

        #region Constructors

        public Timer(Dispatcher dispatcher)
        {
            ticker = new DispatcherTimer(DispatcherPriority.Normal, dispatcher);
            ticker.Tick += (s, e) => DispatcherTimerTick();
            ticker.Interval = new TimeSpan(0, 0, 0, 0, 100);
        }

        #endregion

        #region Event Methods

        public void Start(DateTime dateTime)
        {
            state = TimerState.Running;
            startTime = null;
            endTime = dateTime;
            timeLeft = dateTime - DateTime.Now;
            timeLeft = (timeLeft > TimeSpan.Zero) ? timeLeft : TimeSpan.Zero;
            totalTime = null;

            StartDispatcherTimer();
            OnPropertyChanged("State", "StartTime", "EndTime", "TimeLeft", "TotalTime");
            OnStarted();
        }

        public void Start(TimeSpan timeSpan)
        {
            state = TimerState.Running;
            startTime = DateTime.Now;
            endTime = startTime + timeSpan;
            timeLeft = timeSpan;
            totalTime = timeSpan;

            StartDispatcherTimer();
            OnPropertyChanged("State", "StartTime", "EndTime", "TimeLeft", "TotalTime");
            OnStarted();
        }

        public void Pause()
        {
            if (state != TimerState.Running || !totalTime.HasValue)
                return;

            state = TimerState.Paused;
            timeLeft = endTime - DateTime.Now;

            StopDispatcherTimer();
            OnPropertyChanged("State", "TimeLeft");
            OnPaused();
        }

        public void Resume()
        {
            if (state != TimerState.Paused)
                return;

            state = TimerState.Running;
            endTime = DateTime.Now + timeLeft;
            startTime = endTime - totalTime;

            StartDispatcherTimer();
            OnPropertyChanged("State", "StartTime", "EndTime");
            OnResumed();
        }

        public void Stop()
        {
            state = TimerState.Stopped;
            startTime = null;
            endTime = null;
            timeLeft = null;
            totalTime = null;

            StopDispatcherTimer();
            OnPropertyChanged("State", "StartTime", "EndTime", "TimeLeft", "TotalTime");
            OnStopped();
        }

        public void Reset()
        {
            Stop();
            OnReseted();
        }

        private void DispatcherTimerTick()
        {
            if (state != TimerState.Running)
                return;

            timeLeft = endTime - DateTime.Now;
            timeLeft = (timeLeft > TimeSpan.Zero) ? timeLeft : TimeSpan.Zero;

            if (timeLeft > TimeSpan.Zero)
            {
                OnPropertyChanged("TimeLeft");
                OnTick();
            }
            else
            {
                state = TimerState.Expired;

                StopDispatcherTimer();
                OnPropertyChanged("State", "TimeLeft");
                OnExpired();
            }
        }

        #endregion

        #region Dispatcher Timer Methods

        private void StartDispatcherTimer()
        {
            DispatcherTimerTick();
            ticker.Start();
        }

        private void StopDispatcherTimer()
        {
            ticker.Stop();
        }

        #endregion

        #region Properties

        public string Title
        {
            get { return title; }
            set
            {
                if (title == value) return;
                title = value;
                OnPropertyChanged("Title");
            }
        }

        public TimerState State
        {
            get { return state; }
        }

        public DateTime? StartTime
        {
            get { return startTime; }
        }

        public DateTime? EndTime
        {
            get { return endTime; }
        }

        public TimeSpan? TimeLeft
        {
            get { return timeLeft; }
        }

        public TimeSpan? TotalTime
        {
            get { return totalTime; }
        }

        public TimeSpan Interval
        {
            get { return ticker.Interval; }
            set
            {
                if (ticker.Interval == value) return;
                ticker.Interval = value;
                OnPropertyChanged("Interval");
            }
        }

        #endregion

        #region Events

        public event EventHandler Started;

        protected void OnStarted()
        {
            if (Started != null)
                Started(this, EventArgs.Empty);
        }

        public event EventHandler Paused;

        protected void OnPaused()
        {
            if (Paused != null)
                Paused(this, EventArgs.Empty);
        }

        public event EventHandler Resumed;

        protected void OnResumed()
        {
            if (Resumed != null)
                Resumed(this, EventArgs.Empty);
        }

        public event EventHandler Stopped;

        protected void OnStopped()
        {
            if (Stopped != null)
                Stopped(this, EventArgs.Empty);
        }

        public event EventHandler Reseted;

        protected void OnReseted()
        {
            if (Reseted != null)
                Reseted(this, EventArgs.Empty);
        }

        public event EventHandler Expired;

        protected void OnExpired()
        {
            if (Expired != null)
                Expired(this, EventArgs.Empty);
        }

        public event EventHandler Tick;

        protected void OnTick()
        {
            if (Tick != null)
                Tick(this, EventArgs.Empty);
        }

        #endregion

        #region IDisposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing && ticker != null)
            {
                ticker.Stop();
                ticker = null;
            }
        }

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(params string[] propertyNames)
        {
            if (PropertyChanged != null)
                foreach (string propertyName in propertyNames)
                    PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
