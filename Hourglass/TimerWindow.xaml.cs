using System;
using System.ComponentModel;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Hourglass
{
    public partial class TimerWindow : INotifyPropertyChanged
    {
        private Timer _timer;
        private object _lastInput;
        private bool _isEditing;

        public TimerWindow(string[] args)
        {
            if (args == null)
                throw new ArgumentNullException("args");

            InitializeComponent();

            Timer = new Timer(Dispatcher);
            Timer.Started += Timer_Started;
            Timer.Paused += Timer_Paused;
            Timer.Resumed += Timer_Resumed;
            Timer.Stopped += Timer_Stopped;
            Timer.Reseted += Timer_Reseted;
            Timer.Expired += Timer_Expired;
            Timer.Tick += Timer_Tick;

            TimerTextBox.Loaded += (s, e) => ResetInterface();
        }

        #region Interface Scaling

        private static readonly FontFamily RegularFontFamily = new FontFamily("Segoe UI");
        private static readonly FontFamily LightFontFamily = new FontFamily("Segoe UI Light, Segoe UI");
        private static readonly FormattedText[] ReferenceText = new FormattedText[3];

        private const double BaseButtonMargin = 7;
        private const double BaseControlsGridMargin = 10;
        private const double BaseFontSize = 12;
        private const double BaseInputFontSize = 18;
        private const double BaseInputTopMargin = 1;
        private const double BaseInputBottomMargin = 4;
        private const double BaseWindowHeight = 150;
        private const double MinLightFontSize = 14;

        private double _lastInputScaleFactor = Double.NaN;
        private double _lastOtherScaleFactor = Double.NaN;

        private double GetInputScaleFactor()
        {
            return Math.Min(ActualHeight / BaseWindowHeight, 0.8 * TimerTextBox.ActualWidth / GetReferenceTextWidth());
        }

        private double GetOtherScaleFactor(double inputScaleFactor)
        {
            return Math.Max(1, 1 + Math.Log(inputScaleFactor));
        }

        private double GetReferenceTextWidth()
        {
            if (ReferenceText[0] == null)
            {
                ReferenceText[0] = new FormattedText("88 minutes 88 seconds", CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface(RegularFontFamily, TimerTextBox.FontStyle, TimerTextBox.FontWeight, TimerTextBox.FontStretch), BaseInputFontSize, TimerTextBox.Foreground);
                ReferenceText[1] = new FormattedText("88 hours 88 minutes 88 seconds", CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface(RegularFontFamily, TimerTextBox.FontStyle, TimerTextBox.FontWeight, TimerTextBox.FontStretch), BaseInputFontSize, TimerTextBox.Foreground);
                ReferenceText[2] = new FormattedText("888 days 88 hours 88 minutes 88 seconds", CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface(RegularFontFamily, TimerTextBox.FontStyle, TimerTextBox.FontWeight, TimerTextBox.FontStretch), BaseInputFontSize, TimerTextBox.Foreground);
            }

            if (TimerTextBox.Text.Contains("day"))
                return ReferenceText[2].Width;
            if (TimerTextBox.Text.Contains("hour"))
                return ReferenceText[1].Width;
            return ReferenceText[0].Width;
        }

        private void UpdateScale()
        {
            double inputScaleFactor = GetInputScaleFactor();
            double otherScaleFactor = GetOtherScaleFactor(inputScaleFactor);

            if (inputScaleFactor > 0 && (!inputScaleFactor.Equals(_lastInputScaleFactor) || !otherScaleFactor.Equals(_lastOtherScaleFactor)))
            {
                ControlsGrid.Margin = new Thickness(otherScaleFactor * BaseControlsGridMargin);

                TimerTextBox.FontSize = inputScaleFactor * BaseInputFontSize;
                TimerTextBox.FontFamily = TimerTextBox.FontSize < MinLightFontSize ? RegularFontFamily : LightFontFamily;
                TimerTextBox.Margin = new Thickness(0, inputScaleFactor * BaseInputTopMargin, 0, inputScaleFactor * BaseInputBottomMargin);

                TitleTextBox.FontSize = otherScaleFactor * BaseFontSize;
                TitleTextBox.FontFamily = TitleTextBox.FontSize < MinLightFontSize ? RegularFontFamily : LightFontFamily;

                foreach (Button button in VisualTreeUtility.GetVisualChildren<Button>(this))
                {
                    button.FontSize = otherScaleFactor * BaseFontSize;
                    button.FontFamily = button.FontSize < MinLightFontSize ? RegularFontFamily : LightFontFamily;
                    button.Margin = new Thickness(otherScaleFactor * otherScaleFactor * BaseButtonMargin, 0, otherScaleFactor * otherScaleFactor * BaseButtonMargin, 0);
                }

                _lastInputScaleFactor = inputScaleFactor;
                _lastOtherScaleFactor = otherScaleFactor;
            }
        }

        private void UpdateTimerInterval()
        {
            int interval = 100;

            if (Timer.TotalTime.HasValue)
            {
                interval = (int)(1000 * Timer.TotalTime.Value.TotalSeconds / ActualWidth / 2);
                if (interval < 10) interval = 10;
                if (interval > 100) interval = 100;
            }

            Timer.Interval = new TimeSpan(0, 0, 0, 0, interval);
        }

        #endregion

        #region UI Helpers

        private void CancelEditing()
        {
            FocusUtility.RemoveFocus(TimerTextBox);
            IsEditing = false;
            UpdateAvailableCommands();
        }

        private object GetInput()
        {
            string input = TimerTextBox.Text;

            if (Regex.IsMatch(input, @"^\s*(un)?till?\s*", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant))
            {
                input = Regex.Replace(input, @"^\s*(un)?till?\s*", string.Empty, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
                return GetInputFromDateTimeOrTimeSpan(input);
            }
            
            return GetInputFromTimeSpanOrDateTime(input);
        }

        private static object GetInputFromDateTimeOrTimeSpan(string str)
        {
            DateTime dateTime;
            if (DateTimeUtility.TryParseNatural(str, out dateTime))
                return dateTime;

            TimeSpan timeSpan;
            if (TimeSpanUtility.TryParseNatural(str, out timeSpan))
                return timeSpan;

            return null;
        }

        private static object GetInputFromTimeSpanOrDateTime(string str)
        {
            TimeSpan timeSpan;
            if (TimeSpanUtility.TryParseNatural(str, out timeSpan))
                return timeSpan;

            DateTime dateTime;
            if (DateTimeUtility.TryParseNatural(str, out dateTime))
                return dateTime;

            return null;
        }

        private string GetTimerStringHint()
        {
            if (_lastInput is TimeSpan)
                return TimeSpanUtility.ToShortNaturalString((TimeSpan)_lastInput);
            if (_lastInput is DateTime)
                return DateTimeUtility.ToNaturalString((DateTime)_lastInput);
            return string.Empty;
        }

        private void ResetInterface()
        {
            TimerTextBox.Text = GetTimerStringHint();
            TimerTextBox.Focus();
            TimerTextBox.SelectAll();
            UpdateAvailableCommands();
        }

        private void UpdateAvailableCommands()
        {
            StartButton.IsEnabled = IsEditing;
            PauseButton.IsEnabled = !IsEditing && Timer.State == TimerState.Running && Timer.StartTime.HasValue;
            ResumeButton.IsEnabled = !IsEditing && Timer.State == TimerState.Paused;
            StopButton.IsEnabled = !IsEditing && (Timer.State == TimerState.Running || Timer.State == TimerState.Paused);
            ResetButton.IsEnabled = !IsEditing && Timer.State == TimerState.Expired;
            CancelButton.IsEnabled = IsEditing && Timer.State != TimerState.Stopped;
        }

        #endregion

        #region UI Events

        private void TimerTextBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (Timer.State == TimerState.Expired)
                Timer.Reset();

            IsEditing = true;
            UpdateAvailableCommands();
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            var input = GetInput();
            if (input == null)
                return;

            FocusUtility.RemoveFocus(TimerTextBox);
            IsEditing = false;

            Timer.Start(input);
            UpdateScale();
            UpdateTimerInterval();

            _lastInput = input;
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            Timer.Pause();
        }

        private void ResumeButton_Click(object sender, RoutedEventArgs e)
        {
            Timer.Resume();
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            Timer.Stop();
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            Timer.Reset();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            CancelEditing();
        }

        private void Background_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (IsEditing && Timer.State != TimerState.Stopped)
                CancelEditing();
            else if (TitleTextBox.IsFocused)
                FocusUtility.RemoveFocus(TitleTextBox);
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateScale();
            UpdateTimerInterval();
        }

        #endregion

        #region Timer Events

        private void Timer_Started(object sender, EventArgs e)
        {
            UpdateAvailableCommands();
        }

        private void Timer_Paused(object sender, EventArgs e)
        {
            UpdateAvailableCommands();
        }

        private void Timer_Resumed(object sender, EventArgs e)
        {
            UpdateAvailableCommands();
        }

        private void Timer_Stopped(object sender, EventArgs e)
        {
            ResetInterface();
        }

        private void Timer_Reseted(object sender, EventArgs e)
        {
            ResetInterface();
        }

        private void Timer_Expired(object sender, EventArgs e)
        {
            TimerTextBox.Text = "Timer expired";
            TimerProgressBar.Value = 100.0;
            UpdateAvailableCommands();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (!IsEditing && Timer.TimeLeft.HasValue)
                TimerTextBox.Text = TimeSpanUtility.ToNaturalString(Timer.TimeLeft.Value);

            if (Timer.TimeLeft.HasValue && Timer.TotalTime.HasValue)
            {
                long timeLeft = Timer.TimeLeft.Value.Ticks;
                long totalTime = Timer.TotalTime.Value.Ticks;
                double progress = 100.0 * (totalTime - timeLeft) / totalTime;
                TimerProgressBar.Value = progress;
            }
            else
                TimerProgressBar.Value = 0.0;
        }

        #endregion

        #region Properties

        public Timer Timer
        {
            get { return _timer; }
            private set
            {
                if (value == _timer) return;
                _timer = value;
                OnPropertyChanged("Timer");
            }
        }

        public bool IsEditing
        {
            get { return _isEditing; }
            private set
            {
                if (value == _isEditing) return;
                _isEditing = value;
                OnPropertyChanged("IsEditing");
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
