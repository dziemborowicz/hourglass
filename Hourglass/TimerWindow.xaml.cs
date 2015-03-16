// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimerWindow.xaml.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass
{
    using System;
    using System.ComponentModel;
    using System.Text.RegularExpressions;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    /// <summary>
    /// A timer window.
    /// </summary>
    public partial class TimerWindow : INotifyPropertyChanged
    {
        #region Private Members

        /// <summary>
        /// The <see cref="Timer"/> used for this window.
        /// </summary>
        private readonly Timer timer;

        /// <summary>
        /// A <see cref="TimerScaler"/> used to ensure that controls shrink and grow with the window size, and to
        /// update the <see cref="Timer"/> interval to ensure smooth animation of the progress bar.
        /// </summary>
        private readonly TimerScaler scaler;

        /// <summary>
        /// A <see cref="TimeSpan"/> or <see cref="DateTime"/> representing the last input used to start a timer, or
        /// <c>null</c> if no input has been used to start a timer yet.
        /// </summary>
        private object lastInput;

        /// <summary>
        /// A value indicating whether the user is currently editing the input or title.
        /// </summary>
        private bool isEditing;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TimerWindow"/> class.
        /// </summary>
        /// <param name="args">Command-line arguments.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="args"/> is <c>null</c>.</exception>
        public TimerWindow(string[] args)
        {
            if (args == null)
            {
                throw new ArgumentNullException("args");
            }

            InitializeComponent();

            TimerTextBox.Loaded += (s, e) => this.ResetInterface();

            this.timer = new Timer(Dispatcher);
            this.timer.Started += this.TimerStarted;
            this.timer.Paused += this.TimerPaused;
            this.timer.Resumed += this.TimerResumed;
            this.timer.Stopped += this.TimerStopped;
            this.timer.Reseted += this.TimerReseted;
            this.timer.Expired += this.TimerExpired;
            this.timer.Tick += this.TimerTick;

            Button[] buttons = { StartButton, PauseButton, ResumeButton, StopButton, ResetButton, CancelButton };
            this.scaler = new TimerScaler(this.timer, this /* timerWindow */, ControlsGrid, TimerTextBox, TitleTextBox, buttons);
        }

        #endregion

        #region Events

        /// <summary>
        /// Raised when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets a value indicating whether the user is currently editing the input or title.
        /// </summary>
        public bool IsEditing
        {
            get
            {
                return this.isEditing;
            }

            private set
            {
                if (value == this.isEditing)
                {
                    return;
                }

                this.isEditing = value;
                this.OnPropertyChanged("IsEditing");
            }
        }

        #endregion

        #region Protected Methods

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

        #endregion

        #region Private Methods (Helpers)

        /// <summary>
        /// Returns a <see cref="TimeSpan"/> or <see cref="DateTime"/> representing the user's input, or <c>null</c> if
        /// the user has not specified a valid input.
        /// </summary>
        /// <returns>A <see cref="TimeSpan"/> or <see cref="DateTime"/> representing the user's input, or <c>null</c>
        /// if the user has not specified a valid input.</returns>
        private object GetInput()
        {
            string input = TimerTextBox.Text;

            if (Regex.IsMatch(input, @"^\s*(un)?till?\s*", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant))
            {
                input = Regex.Replace(input, @"^\s*(un)?till?\s*", string.Empty, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
                return this.GetInputFromDateTimeOrTimeSpan(input);
            }
            
            return this.GetInputFromTimeSpanOrDateTime(input);
        }

        /// <summary>
        /// Returns a <see cref="TimeSpan"/> or <see cref="DateTime"/> representing the user's input, or <c>null</c> if
        /// the user has not specified a valid input, favoring a <see cref="DateTime"/> in the case of ambiguity.
        /// </summary>
        /// <param name="str">An input <see cref="string"/>.</param>
        /// <returns>A <see cref="TimeSpan"/> or <see cref="DateTime"/> representing the user's input, or <c>null</c>
        /// if the user has not specified a valid input, favoring a <see cref="DateTime"/> in the case of ambiguity.
        /// </returns>
        private object GetInputFromDateTimeOrTimeSpan(string str)
        {
            DateTime dateTime;
            if (DateTimeUtility.TryParseNatural(str, out dateTime))
            {
                return dateTime;
            }

            TimeSpan timeSpan;
            if (TimeSpanUtility.TryParseNatural(str, out timeSpan))
            {
                return timeSpan;
            }

            return null;
        }

        /// <summary>
        /// Returns a <see cref="TimeSpan"/> or <see cref="DateTime"/> representing the user's input, or <c>null</c> if
        /// the user has not specified a valid input, favoring a <see cref="TimeSpan"/> in the case of ambiguity.
        /// </summary>
        /// <param name="str">An input <see cref="string"/>.</param>
        /// <returns>A <see cref="TimeSpan"/> or <see cref="DateTime"/> representing the user's input, or <c>null</c>
        /// if the user has not specified a valid input, favoring a <see cref="TimeSpan"/> in the case of ambiguity.
        /// </returns>
        private object GetInputFromTimeSpanOrDateTime(string str)
        {
            TimeSpan timeSpan;
            if (TimeSpanUtility.TryParseNatural(str, out timeSpan))
            {
                return timeSpan;
            }

            DateTime dateTime;
            if (DateTimeUtility.TryParseNatural(str, out dateTime))
            {
                return dateTime;
            }

            return null;
        }

        /// <summary>
        /// Returns a <see cref="string"/> representation of the user's last input.
        /// </summary>
        /// <returns>A <see cref="string"/> representation of the user's last input, or <see cref="String.Empty"/> if
        /// the user has not specified a valid input.</returns>
        private string GetTimerStringHint()
        {
            if (this.lastInput is TimeSpan)
            {
                return TimeSpanUtility.ToShortNaturalString((TimeSpan)this.lastInput);
            }

            if (this.lastInput is DateTime)
            {
                return DateTimeUtility.ToNaturalString((DateTime)this.lastInput);
            }

            return string.Empty;
        }

        /// <summary>
        /// Ends editing of the input and title, and removes focus from those fields.
        /// </summary>
        private void CancelEditing()
        {
            FocusUtility.RemoveFocus(TimerTextBox);
            this.IsEditing = false;
            this.UpdateAvailableCommands();
        }

        /// <summary>
        /// Resets the user interface.
        /// </summary>
        private void ResetInterface()
        {
            TimerTextBox.Text = this.GetTimerStringHint();
            TimerTextBox.Focus();
            TimerTextBox.SelectAll();
            this.UpdateAvailableCommands();
        }

        /// <summary>
        /// Updates the commands displayed to the user based on the state of the <see cref="Timer"/>.
        /// </summary>
        private void UpdateAvailableCommands()
        {
            StartButton.IsEnabled = this.IsEditing;
            PauseButton.IsEnabled = !this.IsEditing && this.timer.State == TimerState.Running && this.timer.StartTime.HasValue;
            ResumeButton.IsEnabled = !this.IsEditing && this.timer.State == TimerState.Paused;
            StopButton.IsEnabled = !this.IsEditing && (this.timer.State == TimerState.Running || this.timer.State == TimerState.Paused);
            ResetButton.IsEnabled = !this.IsEditing && this.timer.State == TimerState.Expired;
            CancelButton.IsEnabled = this.IsEditing && this.timer.State != TimerState.Stopped;
        }

        #endregion

        #region Private Methods (User Interface Event Handlers)

        /// <summary>
        /// Invoked when the <see cref="TimerTextBox"/> receives keyboard focus.
        /// </summary>
        /// <param name="sender">The <see cref="TimerTextBox"/>.</param>
        /// <param name="e">The event data.</param>
        private void TimerTextBoxGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (this.timer.State == TimerState.Expired)
            {
                this.timer.Reset();
            }

            this.IsEditing = true;
            this.UpdateAvailableCommands();
        }

        /// <summary>
        /// Invoked when the <see cref="StartButton"/> is clicked.
        /// </summary>
        /// <param name="sender">The <see cref="StartButton"/>.</param>
        /// <param name="e">The event data.</param>
        private void StartButtonClick(object sender, RoutedEventArgs e)
        {
            object input = this.GetInput();

            if (input == null)
            {
                return;
            }

            FocusUtility.RemoveFocus(TimerTextBox);
            this.IsEditing = false;

            this.timer.Start(input);
            this.scaler.Scale();

            this.lastInput = input;
        }

        /// <summary>
        /// Invoked when the <see cref="PauseButton"/> is clicked.
        /// </summary>
        /// <param name="sender">The <see cref="StartButton"/>.</param>
        /// <param name="e">The event data.</param>
        private void PauseButtonClick(object sender, RoutedEventArgs e)
        {
            this.timer.Pause();
        }

        /// <summary>
        /// Invoked when the <see cref="ResumeButton"/> is clicked.
        /// </summary>
        /// <param name="sender">The <see cref="ResumeButton"/>.</param>
        /// <param name="e">The event data.</param>
        private void ResumeButtonClick(object sender, RoutedEventArgs e)
        {
            this.timer.Resume();
        }

        /// <summary>
        /// Invoked when the <see cref="StopButton"/> is clicked.
        /// </summary>
        /// <param name="sender">The <see cref="StopButton"/>.</param>
        /// <param name="e">The event data.</param>
        private void StopButtonClick(object sender, RoutedEventArgs e)
        {
            this.timer.Stop();
        }

        /// <summary>
        /// Invoked when the <see cref="ResetButton"/> is clicked.
        /// </summary>
        /// <param name="sender">The <see cref="ResetButton"/>.</param>
        /// <param name="e">The event data.</param>
        private void ResetButtonClick(object sender, RoutedEventArgs e)
        {
            this.timer.Reset();
        }

        /// <summary>
        /// Invoked when the <see cref="CancelButton"/> is clicked.
        /// </summary>
        /// <param name="sender">The <see cref="CancelButton"/>.</param>
        /// <param name="e">The event data.</param>
        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            this.CancelEditing();
        }

        /// <summary>
        /// Invoked when the background area of the window is clicked.
        /// </summary>
        /// <param name="sender">The background <see cref="Grid"/> control.</param>
        /// <param name="e">The event data.</param>
        private void BackgroundMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (this.IsEditing && this.timer.State != TimerState.Stopped)
            {
                this.CancelEditing();
            }
            else if (TitleTextBox.IsFocused)
            {
                FocusUtility.RemoveFocus(TitleTextBox);
            }
        }

        /// <summary>
        /// Invoked when this window has first rendered or has changed its rendering size.
        /// </summary>
        /// <param name="sender">This window.</param>
        /// <param name="e">The event data.</param>
        private void WindowSizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.scaler.Scale();
        }

        #endregion

        #region Private Methods (Timer Event Handlers)

        /// <summary>
        /// Invoked when the <see cref="Timer"/> is started.
        /// </summary>
        /// <param name="sender">The <see cref="Timer"/> that is the source of the event.</param>
        /// <param name="e">An object that contains no event data.</param>
        private void TimerStarted(object sender, EventArgs e)
        {
            this.UpdateAvailableCommands();
        }

        /// <summary>
        /// Invoked when the <see cref="Timer"/> is paused.
        /// </summary>
        /// <param name="sender">The <see cref="Timer"/> that is the source of the event.</param>
        /// <param name="e">An object that contains no event data.</param>
        private void TimerPaused(object sender, EventArgs e)
        {
            this.UpdateAvailableCommands();
        }

        /// <summary>
        /// Invoked when the <see cref="Timer"/> is resumed from a paused state.
        /// </summary>
        /// <param name="sender">The <see cref="Timer"/> that is the source of the event.</param>
        /// <param name="e">An object that contains no event data.</param>
        private void TimerResumed(object sender, EventArgs e)
        {
            this.UpdateAvailableCommands();
        }

        /// <summary>
        /// Invoked when the <see cref="Timer"/> is stopped.
        /// </summary>
        /// <param name="sender">The <see cref="Timer"/> that is the source of the event.</param>
        /// <param name="e">An object that contains no event data.</param>
        private void TimerStopped(object sender, EventArgs e)
        {
            this.ResetInterface();
        }

        /// <summary>
        /// Invoked when the <see cref="Timer"/> is reset.
        /// </summary>
        /// <param name="sender">The <see cref="Timer"/> that is the source of the event.</param>
        /// <param name="e">An object that contains no event data.</param>
        private void TimerReseted(object sender, EventArgs e)
        {
            this.ResetInterface();
        }

        /// <summary>
        /// Invoked when the <see cref="Timer"/> expires.
        /// </summary>
        /// <param name="sender">The <see cref="Timer"/> that is the source of the event.</param>
        /// <param name="e">An object that contains no event data.</param>
        private void TimerExpired(object sender, EventArgs e)
        {
            TimerTextBox.Text = "Timer expired";
            TimerProgressBar.Value = 100.0;
            this.UpdateAvailableCommands();
        }

        /// <summary>
        /// Invoked when the <see cref="Timer"/> ticks.
        /// </summary>
        /// <param name="sender">The <see cref="Timer"/> that is the source of the event.</param>
        /// <param name="e">An object that contains no event data.</param>
        private void TimerTick(object sender, EventArgs e)
        {
            if (!this.IsEditing && this.timer.TimeLeft.HasValue)
            {
                TimerTextBox.Text = TimeSpanUtility.ToNaturalString(this.timer.TimeLeft.Value);
            }

            if (this.timer.TimeLeft.HasValue && this.timer.TotalTime.HasValue)
            {
                long timeLeft = this.timer.TimeLeft.Value.Ticks;
                long totalTime = this.timer.TotalTime.Value.Ticks;
                double progress = 100.0 * (totalTime - timeLeft) / totalTime;
                TimerProgressBar.Value = progress;
            }
            else
            {
                TimerProgressBar.Value = 0.0;
            }
        }

        #endregion
    }
}
