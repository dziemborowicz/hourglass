using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Microsoft.TeamFoundation.MVVM;

namespace Hourglass
{
    public partial class TimerWindow : Window, INotifyPropertyChanged
    {
        #region User Interface

        private static readonly FontFamily RegularFontFamily = new FontFamily("Segoe UI");
        private static readonly FontFamily LightFontFamily = new FontFamily("Segoe UI Light, Segoe UI");

        public TimerWindow(string[] args)
        {
            InitializeComponent();

            // Keep track of keyboard focus
            InputTextBox.GotKeyboardFocus += (s, e) => isEditing = true;
            InputTextBox.Loaded += (s, e) =>
            {
                InputTextBox.Focus();
                InputTextBox.SelectAll();
            };

            // Make the interface scalable
            SizeChanged += (s, e) => UpdateScale();
            UpdateScale();
        }

        private void UpdateScale()
        {
            // Base measurements
            const double baseButtonMargin = 7;
            const double baseControlsGridMargin = 10;
            const double baseFontSize = 12;
            const double baseInputFontSize = 18;
            const double baseInputTopMargin = 1;
            const double baseInputBottomMargin = 4;
            const double baseWindowHeight = 150;
            const double minLightFontSize = 14;

            // Use reference text to prevent resizing on every tick
            string inputText;
            if (Regex.IsMatch(InputTextBox.Text, @"^\d+ days? \d+ hours? \d+ minutes? \d+ seconds?$"))
                inputText = "888 days 88 hours 88 minutes 88 seconds";
            else if (Regex.IsMatch(InputTextBox.Text, @"^\d+ hours? \d+ minutes? \d+ seconds?$"))
                inputText = "88 hours 88 minutes 88 seconds";
            else
                inputText = "88 minutes 88 seconds";

            // Calculate scale factors
            FormattedText inputFormattedText = new FormattedText(inputText, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface(InputTextBox.FontFamily, InputTextBox.FontStyle, InputTextBox.FontWeight, InputTextBox.FontStretch), baseInputFontSize, InputTextBox.Foreground);
            double inputScaleFactor = Math.Min(this.ActualHeight / baseWindowHeight, 0.8 * InputTextBox.ActualWidth / inputFormattedText.Width);
            double otherScaleFactor = Math.Max(1, 1 + Math.Log(inputScaleFactor));

            // Resize elements (but do not shrink them below base measurements)
            if (inputScaleFactor > 0)
            {
                ControlsGrid.Margin = new Thickness(otherScaleFactor * baseControlsGridMargin);

                InputTextBox.FontSize = inputScaleFactor * baseInputFontSize;
                InputTextBox.FontFamily = InputTextBox.FontSize < minLightFontSize ? RegularFontFamily : LightFontFamily;
                InputTextBox.Margin = new Thickness(0, inputScaleFactor * baseInputTopMargin, 0, inputScaleFactor * baseInputBottomMargin);

                TitleTextBox.FontSize = otherScaleFactor * baseFontSize;
                TitleTextBox.FontFamily = TitleTextBox.FontSize < minLightFontSize ? RegularFontFamily : LightFontFamily;

                foreach (Button button in VisualTreeUtility.GetVisualChildren<Button>(this))
                {
                    button.FontSize = otherScaleFactor * baseFontSize;
                    button.FontFamily = button.FontSize < minLightFontSize ? RegularFontFamily : LightFontFamily;
                    button.Margin = new Thickness(otherScaleFactor * otherScaleFactor * baseButtonMargin, 0, otherScaleFactor * otherScaleFactor * baseButtonMargin, 0);
                }
            }
        }

        #endregion

        #region Logic

        private Timer timer;

        private TimerMode mode = TimerMode.None;
        private TimerState state = TimerState.Stopped;
        private bool isEditing;

        private DateTime startTime;
        private DateTime endTime;
        private TimeSpan timeLeft;
        private TimeSpan totalTime;

        public void Start()
        {
            // Check that we have valid input
            object input = GetInput();
            if (input == null)
            {
                InputTextBox.Focus();
                InputTextBox.SelectAll();
                return;
            }

            // Stop running timer (if any)
            Stop();

            // Set new timer parameters
            if (input is TimeSpan)
            {
                mode = TimerMode.TimeSpan;
                state = TimerState.Running;
                isEditing = false;

                startTime = DateTime.Now;
                endTime = startTime.Add((TimeSpan)input);
                timeLeft = totalTime;
                totalTime = (TimeSpan)input;
            }
            else // input is DateTime
            {
                mode = TimerMode.DateTime;
                state = TimerState.Running;
                isEditing = false;

                startTime = DateTime.Now;
                endTime = (DateTime)input;
                timeLeft = endTime - startTime;
                totalTime = endTime - startTime;
            }

            // Start timer
            if (timer == null)
            {
                timer = new Timer();
                timer.AutoReset = false;
                timer.Elapsed += (s, e) => AsyncTick();
            }
            timer.Interval = 1;
            timer.Start();

            // Update interface
            FocusUtility.RemoveFocus(TitleTextBox);
            FocusUtility.RemoveFocus(InputTextBox);
            Tick();
            UpdateScale();
        }

        public void Pause()
        {
            if (mode != TimerMode.TimeSpan || state != TimerState.Running)
                return;

            state = TimerState.Paused;
            timer.Stop();
        }

        public void Resume()
        {
            if (mode != TimerMode.TimeSpan || state != TimerState.Paused)
                return;

            // Update timer parameters
            state = TimerState.Running;
            endTime = DateTime.Now + timeLeft;
            startTime = endTime - totalTime;

            // Restart timer
            timer.Start();

            // Update interface
            FocusUtility.RemoveFocus(TitleTextBox);
            FocusUtility.RemoveFocus(InputTextBox);
            Tick();
            UpdateScale();
        }

        public void Stop()
        {
            if (state == TimerState.Stopped)
                return;

            state = TimerState.Stopped;
            timer.Stop();

            // Update interface
            Progress = 0;
            TimerText = mode == TimerMode.TimeSpan ? TimeSpanUtility.ToShortNaturalString(totalTime) : DateTimeUtility.ToNaturalString(endTime);

            InputTextBox.Focus();
            InputTextBox.SelectAll();
            UpdateScale();
        }

        public void OK()
        {
            if (state == TimerState.Stopped)
                return;

            state = TimerState.Stopped;

            // Update interface
            Progress = 0;
            TimerText = mode == TimerMode.TimeSpan ? TimeSpanUtility.ToShortNaturalString(totalTime) : DateTimeUtility.ToNaturalString(endTime);

            InputTextBox.Focus();
            InputTextBox.SelectAll();
            UpdateScale();
        }

        public void Cancel()
        {
            isEditing = false;

            // Cancel editing
            switch (state)
            {
                case TimerState.Stopped:
                    TimerText = mode == TimerMode.TimeSpan ? TimeSpanUtility.ToShortNaturalString(totalTime) : DateTimeUtility.ToNaturalString(endTime);
                    break;

                case TimerState.Running:
                    Tick();
                    break;

                case TimerState.Paused:
                    TimerText = TimeSpanUtility.ToNaturalString(timeLeft);
                    break;

                case TimerState.Expired:
                    TimerText = "Timer expired";
                    break;
            }

            // Update interface
            FocusUtility.RemoveFocus(TitleTextBox);
            FocusUtility.RemoveFocus(InputTextBox);
            UpdateScale();
        }

        private void Tick()
        {
            if (state != TimerState.Running)
                return;

            timeLeft = endTime - DateTime.Now;
            if (timeLeft > TimeSpan.Zero)
            {
                // Update progress
                Progress = 100.0 - 100.0 * timeLeft.Ticks / totalTime.Ticks;
                if (!isEditing)
                    TimerText = TimeSpanUtility.ToNaturalString(timeLeft);
            }
            else
            {
                // Expired
                state = TimerState.Expired;
                Progress = 100.0;
                if (!isEditing)
                    TimerText = "Timer expired";

                CommandManager.InvalidateRequerySuggested();

                // TODO Play notification sound
            }
        }

        private void AsyncTick()
        {
            Dispatcher.BeginInvoke(new Action(() => Tick())).Wait();
            if (mode == TimerMode.TimeSpan)
                timer.Interval = Math.Max(Math.Min(1000 * totalTime.TotalSeconds / ActualWidth / 2, 1000), 10);
            else
                timer.Interval = 1000;
        }

        private object GetInput()
        {
            try
            {
                return TimeSpanUtility.ParseNatural(TimerText);
            }
            catch (Exception)
            {
            }

            try
            {
                return DateTimeUtility.ParseNatural(TimerText);
            }
            catch (Exception)
            {
            }

            return null;
        }

        #endregion

        # region Commands

        private ICommand startCommand;

        public ICommand StartCommand
        {
            get
            {
                if (startCommand == null)
                    startCommand = new RelayCommand(o => Start(), o => isEditing);
                return startCommand;
            }
        }

        private ICommand pauseCommand;

        public ICommand PauseCommand
        {
            get
            {
                if (pauseCommand == null)
                    pauseCommand = new RelayCommand(o => Pause(), o => mode == TimerMode.TimeSpan && state == TimerState.Running && !isEditing);
                return pauseCommand;
            }
        }

        private ICommand resumeCommand;

        public ICommand ResumeCommand
        {
            get
            {
                if (resumeCommand == null)
                    resumeCommand = new RelayCommand(o => Resume(), o => mode == TimerMode.TimeSpan && state == TimerState.Paused && !isEditing);
                return resumeCommand;
            }
        }

        private ICommand stopCommand;

        public ICommand StopCommand
        {
            get
            {
                if (stopCommand == null)
                    stopCommand = new RelayCommand(o => Stop(), o => state != TimerState.Stopped && state != TimerState.Expired && !isEditing);
                return stopCommand;
            }
        }

        private ICommand okCommand;

        public ICommand OkCommand
        {
            get
            {
                if (okCommand == null)
                    okCommand = new RelayCommand(o => OK(), o => state == TimerState.Expired && !isEditing);
                return okCommand;
            }
        }

        private ICommand cancelCommand;

        public ICommand CancelCommand
        {
            get
            {
                if (cancelCommand == null)
                    cancelCommand = new RelayCommand(o => Cancel(), o => state != TimerState.Stopped && isEditing);
                return cancelCommand;
            }
        }

        #endregion

        #region Properties

        private string timerTitle;

        public string TimerTitle
        {
            get { return timerTitle; }
            set
            {
                if (timerTitle == value) return;
                timerTitle = value;
                OnPropertyChanged("TimerTitle");
            }
        }

        private string timerText;

        public string TimerText
        {
            get { return timerText; }
            set
            {
                if (timerText == value) return;
                timerText = value;
                OnPropertyChanged("TimerText");
            }
        }

        private double progress;

        public double Progress
        {
            get { return progress; }
            set
            {
                if (progress == value) return;
                progress = value;
                OnPropertyChanged("Progress");
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
