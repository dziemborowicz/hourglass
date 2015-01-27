using System;
using System.ComponentModel;
using System.Windows.Threading;

namespace Hourglass
{
    public class Timer : IDisposable, INotifyPropertyChanged
    {
        #region Internal State

        private DispatcherTimer _ticker;

        private string _title;

        private TimerState _state = TimerState.Stopped;
        private DateTime? _startTime;
        private DateTime? _endTime;
        private TimeSpan? _timeLeft;
        private TimeSpan? _totalTime;

        #endregion

        #region Constructors

        public Timer(Dispatcher dispatcher)
        {
            _ticker = new DispatcherTimer(DispatcherPriority.Normal, dispatcher);
            _ticker.Tick += (s, e) => DispatcherTimerTick();
            _ticker.Interval = new TimeSpan(0, 0, 0, 0, 100);
        }

        #endregion

        #region Event Methods

        public void Start(object input)
        {
            if (input is DateTime)
                Start((DateTime)input);
            else if (input is TimeSpan)
                Start((TimeSpan)input);
            else
                throw new ArgumentException(@"input must be a DateTime or TimeSpan", "input");
        }

        public void Start(DateTime dateTime)
        {
            _state = TimerState.Running;
            _startTime = null;
            _endTime = dateTime;
            _timeLeft = dateTime - DateTime.Now;
            _timeLeft = (_timeLeft > TimeSpan.Zero) ? _timeLeft : TimeSpan.Zero;
            _totalTime = null;

            StartDispatcherTimer();
            OnPropertyChanged("State", "StartTime", "EndTime", "TimeLeft", "TotalTime");
            OnStarted();
        }

        public void Start(TimeSpan timeSpan)
        {
            _state = TimerState.Running;
            _startTime = DateTime.Now;
            _endTime = _startTime + timeSpan;
            _timeLeft = timeSpan;
            _totalTime = timeSpan;

            StartDispatcherTimer();
            OnPropertyChanged("State", "StartTime", "EndTime", "TimeLeft", "TotalTime");
            OnStarted();
        }

        public void Pause()
        {
            if (_state != TimerState.Running || !_totalTime.HasValue)
                return;

            _state = TimerState.Paused;
            _timeLeft = _endTime - DateTime.Now;

            StopDispatcherTimer();
            OnPropertyChanged("State", "TimeLeft");
            OnPaused();
        }

        public void Resume()
        {
            if (_state != TimerState.Paused)
                return;

            _state = TimerState.Running;
            _endTime = DateTime.Now + _timeLeft;
            _startTime = _endTime - _totalTime;

            StartDispatcherTimer();
            OnPropertyChanged("State", "StartTime", "EndTime");
            OnResumed();
        }

        public void Stop()
        {
            _state = TimerState.Stopped;
            _startTime = null;
            _endTime = null;
            _timeLeft = null;
            _totalTime = null;

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
            if (_state != TimerState.Running)
                return;

            _timeLeft = _endTime - DateTime.Now;
            _timeLeft = (_timeLeft > TimeSpan.Zero) ? _timeLeft : TimeSpan.Zero;

            if (_timeLeft > TimeSpan.Zero)
            {
                OnPropertyChanged("TimeLeft");
                OnTick();
            }
            else
            {
                _state = TimerState.Expired;

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
            _ticker.Start();
        }

        private void StopDispatcherTimer()
        {
            _ticker.Stop();
        }

        #endregion

        #region Properties

        public string Title
        {
            get { return _title; }
            set
            {
                if (_title == value) return;
                _title = value;
                OnPropertyChanged("Title");
            }
        }

        public TimerState State
        {
            get { return _state; }
        }

        public DateTime? StartTime
        {
            get { return _startTime; }
        }

        public DateTime? EndTime
        {
            get { return _endTime; }
        }

        public TimeSpan? TimeLeft
        {
            get { return _timeLeft; }
        }

        public TimeSpan? TotalTime
        {
            get { return _totalTime; }
        }

        public TimeSpan Interval
        {
            get { return _ticker.Interval; }
            set
            {
                if (_ticker.Interval == value) return;
                _ticker.Interval = value;
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
            if (disposing && _ticker != null)
            {
                _ticker.Stop();
                _ticker = null;
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
