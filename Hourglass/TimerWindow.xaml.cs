using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Hourglass
{
    public partial class TimerWindow : Window
    {
        private Timer timer;
        private object lastInput;
        private bool isEditing;

        public TimerWindow(string[] args)
        {
            InitializeComponent();

            timer = new Timer(Dispatcher);
            timer.Started += Timer_Started;
            timer.Paused += Timer_Paused;
            timer.Resumed += Timer_Resumed;
            timer.Stopped += Timer_Stopped;
            timer.Reseted += Timer_Reseted;
            timer.Expired += Timer_Expired;
            timer.Tick += Timer_Tick;

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

        private double lastInputScaleFactor = Double.NaN;
        private double lastOtherScaleFactor = Double.NaN;

        private double GetInputScaleFactor()
        {
            return Math.Min(this.ActualHeight / BaseWindowHeight, 0.8 * TimerTextBox.ActualWidth / GetReferenceTextWidth());
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

            if (TimerTextBox.Text.Contains(" day"))
                return ReferenceText[2].Width;
            else if (TimerTextBox.Text.Contains(" hour"))
                return ReferenceText[1].Width;
            else
                return ReferenceText[0].Width;
        }

        private void UpdateScale()
        {
            double inputScaleFactor = GetInputScaleFactor();
            double otherScaleFactor = GetOtherScaleFactor(inputScaleFactor);

            if (inputScaleFactor > 0 && (inputScaleFactor != lastInputScaleFactor || otherScaleFactor != lastOtherScaleFactor))
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

                lastInputScaleFactor = inputScaleFactor;
                lastOtherScaleFactor = otherScaleFactor;
            }
        }

        private void UpdateTimerInterval()
        {
            int interval = 100;

            if (timer.StartTime.HasValue)
            {
                interval = (int)(1000 * timer.TotalTime.Value.TotalSeconds / ActualWidth / 2);
                if (interval < 10) interval = 10;
                if (interval > 100) interval = 100;
            }

            timer.Interval = new TimeSpan(0, 0, 0, 0, interval);
        }

        #endregion

        #region UI Helpers

        private string GetTimerStringHint()
        {
            if (lastInput is TimeSpan)
                return TimeSpanUtility.ToShortNaturalString((TimeSpan)lastInput);
            else if (lastInput is DateTime)
                return DateTimeUtility.ToNaturalString((DateTime)lastInput);
            else
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
            StartButton.IsEnabled = isEditing;
            PauseButton.IsEnabled = !isEditing && timer.State == TimerState.Running && timer.TimeLeft.HasValue;
            ResumeButton.IsEnabled = !isEditing && timer.State == TimerState.Paused;
            StopButton.IsEnabled = !isEditing && (timer.State == TimerState.Running || timer.State == TimerState.Paused);
            ResetButton.IsEnabled = !isEditing && timer.State == TimerState.Expired;
            CancelButton.IsEnabled = isEditing && timer.State != TimerState.Stopped;
        }

        #endregion

        #region UI Events

        private void TimerTextBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (timer.State == TimerState.Expired)
                timer.Reset();

            isEditing = true;
            UpdateAvailableCommands();
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DateTime dateTime = DateTimeUtility.ParseNatural(TimerTextBox.Text);
                FocusUtility.RemoveFocus(TimerTextBox);
                lastInput = dateTime;
                isEditing = false;
                timer.Start(dateTime);
                UpdateScale();
                UpdateTimerInterval();
                return;
            }
            catch (Exception)
            {
            }

            try
            {
                TimeSpan timeSpan = TimeSpanUtility.ParseNatural(TimerTextBox.Text);
                FocusUtility.RemoveFocus(TimerTextBox);
                lastInput = timeSpan;
                isEditing = false;
                timer.Start(timeSpan);
                UpdateScale();
                UpdateTimerInterval();
                return;
            }
            catch (Exception)
            {
            }
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            timer.Pause();
        }

        private void ResumeButton_Click(object sender, RoutedEventArgs e)
        {
            timer.Resume();
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            timer.Reset();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            FocusUtility.RemoveFocus(TimerTextBox);
            isEditing = false;
            UpdateAvailableCommands();
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
            if (!isEditing)
                TimerTextBox.Text = TimeSpanUtility.ToNaturalString(timer.TimeLeft.Value);

            if (timer.StartTime.HasValue)
            {
                long timeLeft = timer.TimeLeft.Value.Ticks;
                long totalTime = timer.TotalTime.Value.Ticks;
                double progress = 100.0 * (totalTime - timeLeft) / totalTime;
                TimerProgressBar.Value = progress;
            }
        }

        #endregion
    }
}
