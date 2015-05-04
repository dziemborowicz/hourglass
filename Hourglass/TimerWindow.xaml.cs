// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimerWindow.xaml.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass
{
    using System;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Media.Animation;

    /// <summary>
    /// The view of a <see cref="TimerWindow"/>.
    /// </summary>
    public enum TimerWindowView
    {
        /// <summary>
        /// Indicates that the <see cref="TimerWindow"/> is not initialized.
        /// </summary>
        None,

        /// <summary>
        /// Indicates that the <see cref="TimerWindow"/> is accepting user input to start a new <see cref="Timer"/>.
        /// </summary>
        Input,

        /// <summary>
        /// Indicates that the <see cref="TimerWindow"/> is displaying the status of a running or paused <see
        /// cref="Timer"/>.
        /// </summary>
        Status,

        /// <summary>
        /// Indicates that the <see cref="TimerWindow"/> is displaying a notification that a <see cref="Timer"/> has
        /// expired.
        /// </summary>
        Expired
    }

    /// <summary>
    /// A timer window.
    /// </summary>
    public partial class TimerWindow : INotifyPropertyChanged
    {
        #region Private Members

        /// <summary>
        /// The <see cref="TimerWindowView"/> of the window.
        /// </summary>
        private TimerWindowView view = TimerWindowView.None;

        /// <summary>
        /// A value indicating whether controls should be showed even when the mouse is not over the window.
        /// </summary>
        private bool alwaysShowControls;

        /// <summary>
        /// The <see cref="TimerMenu"/> for the window.
        /// </summary>
        private TimerMenu menu = new TimerMenu();

        /// <summary>
        /// The <see cref="TimerScaler"/> for the window.
        /// </summary>
        private TimerScaler scaler = new TimerScaler();

        /// <summary>
        /// The <see cref="HourglassTimer"/> backing the window.
        /// </summary>
        private HourglassTimer timer = new TimeSpanTimer();

        /// <summary>
        /// The animation used notify the user that the timer has expired.
        /// </summary>
        private DoubleAnimation expirationAnimation;

        /// <summary>
        /// The animation used notify the user that the input was invalid.
        /// </summary>
        private DoubleAnimation validationErrorAnimation;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TimerWindow"/> class.
        /// </summary>
        public TimerWindow()
        {
            this.InitializeComponent();
            this.InitializeAnimations();
            this.BindTimer(this.Timer);
            this.menu.Bind(this /* window */);
            this.scaler.Bind(this /* window */);

            TimerManager.Instance.Add(this.Timer);
        }

        #endregion

        #region Events

        /// <summary>
        /// Raised when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the <see cref="TimerWindowView"/> of the window.
        /// </summary>
        public TimerWindowView View
        {
            get
            {
                return this.view;
            }

            private set
            {
                if (this.view == value)
                {
                    return;
                }

                this.view = value;
                this.OnPropertyChanged("View");
            }
        }

        /// <summary>
        /// Gets a value indicating whether controls should be showed even when the mouse is not over the
        /// window.
        /// </summary>
        public bool AlwaysShowControls
        {
            get
            {
                return this.alwaysShowControls;
            }

            private set
            {
                if (this.alwaysShowControls == value)
                {
                    return;
                }

                this.alwaysShowControls = value;
                this.OnPropertyChanged("AlwaysShowControls");
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="HourglassTimer"/> backing the window.
        /// </summary>
        public HourglassTimer Timer
        {
            get
            {
                return this.timer;
            }

            set
            {
                if (this.timer == value)
                {
                    return;
                }

                this.UnbindTimer(this.timer);
                this.timer = value;
                this.BindTimer(this.timer);
                this.OnPropertyChanged("Timer");
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

        #region Private Methods (Animations)

        /// <summary>
        /// Initializes the animation members.
        /// </summary>
        private void InitializeAnimations()
        {
            this.expirationAnimation = new DoubleAnimation(1.0, 0.0, new Duration(TimeSpan.FromSeconds(1)));
            this.expirationAnimation.EasingFunction = new ExponentialEase { EasingMode = EasingMode.EaseOut };
            this.expirationAnimation.RepeatBehavior = RepeatBehavior.Forever;

            this.validationErrorAnimation = new DoubleAnimation(1.0, 0.0, new Duration(TimeSpan.FromSeconds(1)));
            this.validationErrorAnimation.EasingFunction = new ExponentialEase { EasingMode = EasingMode.EaseOut };
        }

        /// <summary>
        /// Begins the animation used notify the user that the input was invalid.
        /// </summary>
        private void BeginExpirationAnimation()
        {
            this.InnerNotificationBorder.BeginAnimation(UIElement.OpacityProperty, this.expirationAnimation);
        }

        /// <summary>
        /// Begins the animation used notify the user that the input was invalid.
        /// </summary>
        private void BeginValidationErrorAnimation()
        {
            this.InnerNotificationBorder.BeginAnimation(UIElement.OpacityProperty, this.validationErrorAnimation);
        }

        /// <summary>
        /// Stops all animations.
        /// </summary>
        private void EndAnimations()
        {
            this.InnerNotificationBorder.BeginAnimation(UIElement.OpacityProperty, null /* animation */);
        }

        #endregion

        #region Private Methods (Views)

        /// <summary>
        /// Sets the window view to correspond to the state of the current timer.
        /// </summary>
        private void ShowViewForTimer()
        {
            switch (this.timer.State)
            {
                case TimerState.Stopped:
                    this.ShowInputView();
                    return;

                case TimerState.Running:
                case TimerState.Paused:
                    this.ShowStatusView();
                    return;

                case TimerState.Expired:
                    this.ShowExpiredView();
                    return;
            }
        }

        /// <summary>
        /// Sets the window to accept user input to start a new <see cref="Timer"/>.
        /// </summary>
        private void ShowInputView()
        {
            this.View = TimerWindowView.Input;
            this.AlwaysShowControls = true;

            this.TimerTextBox.Focusable = true;
            this.TimerTextBox.IsReadOnly = false;
            this.TimerTextBox.Text = "TODO Set last timer text";
            this.TimerTextBox.SelectAll();
            this.TimerTextBox.Focus();

            this.OuterNotificationBorder.Opacity = 0;
            this.EndAnimations();

            this.UpdateButtons();
            this.UpdateTimerBinding();
        }

        /// <summary>
        /// Sets the window to display the status of a running or paused <see cref="Timer"/>.
        /// </summary>
        private void ShowStatusView()
        {
            this.View = TimerWindowView.Status;
            this.AlwaysShowControls = false;

            this.TimerTextBox.Focusable = false;
            this.TimerTextBox.IsReadOnly = true;

            FocusUtility.RemoveFocus(this.TimerTextBox);
            FocusUtility.RemoveFocus(this.TitleTextBox);

            this.OuterNotificationBorder.Opacity = 0;
            this.EndAnimations();

            this.UpdateButtons();
            this.UpdateTimerBinding();
        }

        /// <summary>
        /// Sets the window to display a notification that a <see cref="Timer"/> has expired.
        /// </summary>
        private void ShowExpiredView()
        {
            this.View = TimerWindowView.Expired;
            this.AlwaysShowControls = false;

            this.TimerTextBox.Focusable = false;
            this.TimerTextBox.IsReadOnly = true;

            FocusUtility.RemoveFocus(this.TimerTextBox);
            FocusUtility.RemoveFocus(this.TitleTextBox);

            this.OuterNotificationBorder.Opacity = 1;
            this.EndAnimations();
            this.BeginExpirationAnimation();

            this.UpdateButtons();
            this.UpdateTimerBinding();
        }

        /// <summary>
        /// Sets the visibility of the buttons.
        /// </summary>
        private void UpdateButtons()
        {
            switch (this.View)
            {
                case TimerWindowView.Input:
                    this.StartButton.IsEnabled = true;
                    this.PauseButton.IsEnabled = false;
                    this.ResumeButton.IsEnabled = false;
                    this.StopButton.IsEnabled = false;
                    this.ResetButton.IsEnabled = false;
                    this.CancelButton.IsEnabled = this.Timer.State != TimerState.Stopped && this.Timer.State != TimerState.Expired;
                    return;

                case TimerWindowView.Status:
                    this.StartButton.IsEnabled = false;
                    this.PauseButton.IsEnabled = this.Timer.State == TimerState.Running && !(this.Timer is DateTimeTimer);
                    this.ResumeButton.IsEnabled = this.Timer.State == TimerState.Paused;
                    this.StopButton.IsEnabled = this.Timer.State != TimerState.Stopped && this.Timer.State != TimerState.Expired;
                    this.ResetButton.IsEnabled = false;
                    this.CancelButton.IsEnabled = false;
                    return;

                case TimerWindowView.Expired:
                    this.StartButton.IsEnabled = false;
                    this.PauseButton.IsEnabled = false;
                    this.ResumeButton.IsEnabled = false;
                    this.StopButton.IsEnabled = false;
                    this.ResetButton.IsEnabled = true;
                    this.CancelButton.IsEnabled = false;
                    return;
            }
        }

        #endregion

        #region Private Methods (Timer Binding)

        /// <summary>
        /// Binds the <see cref="TimerWindow"/> event handlers and controls to a <see cref="HourglassTimer"/>.
        /// </summary>
        /// <param name="hourglassTimer">A <see cref="HourglassTimer"/>.</param>
        private void BindTimer(HourglassTimer hourglassTimer)
        {
            hourglassTimer.Started += this.TimerStarted;
            hourglassTimer.Paused += this.TimerPaused;
            hourglassTimer.Resumed += this.TimerResumed;
            hourglassTimer.Stopped += this.TimerStopped;
            hourglassTimer.Expired += this.TimerExpired;
            hourglassTimer.Tick += this.TimerTick;
            hourglassTimer.PropertyChanged += this.TimerPropertyChanged;

            this.ShowViewForTimer();
            this.UpdateTimerBinding();
        }

        /// <summary>
        /// Updates the user interface elements bound to <see cref="HourglassTimer"/> properties.
        /// </summary>
        private void UpdateTimerBinding()
        {
            if (this.View != TimerWindowView.Input)
            {
                this.TimerTextBox.Text = this.Timer.TimeLeftAsString;
            }

            this.ProgressBar.Value = this.Timer.TimeLeftAsPercentage ?? 0;
        }

        /// <summary>
        /// Unbinds the <see cref="TimerWindow"/> event handlers and controls from a <see cref="HourglassTimer"/>.
        /// </summary>
        /// <param name="hourglassTimer">A <see cref="HourglassTimer"/>.</param>
        private void UnbindTimer(HourglassTimer hourglassTimer)
        {
            hourglassTimer.Started -= this.TimerStarted;
            hourglassTimer.Paused -= this.TimerPaused;
            hourglassTimer.Resumed -= this.TimerResumed;
            hourglassTimer.Stopped -= this.TimerStopped;
            hourglassTimer.Expired -= this.TimerExpired;
            hourglassTimer.Tick -= this.TimerTick;
            hourglassTimer.PropertyChanged -= this.TimerPropertyChanged;
        }

        #endregion

        #region Private Methods (Timer Events)

        /// <summary>
        /// Invoked when the <see cref="HourglassTimer"/> is started.
        /// </summary>
        /// <param name="sender">The <see cref="HourglassTimer"/>.</param>
        /// <param name="e">The event data.</param>
        private void TimerStarted(object sender, EventArgs e)
        {
            this.ShowViewForTimer();
        }

        /// <summary>
        /// Invoked when the <see cref="HourglassTimer"/> is paused.
        /// </summary>
        /// <param name="sender">The <see cref="HourglassTimer"/>.</param>
        /// <param name="e">The event data.</param>
        private void TimerPaused(object sender, EventArgs e)
        {
            if (this.View != TimerWindowView.Input)
            {
                this.ShowViewForTimer();
            }
            else
            {
                this.UpdateButtons();
            }
        }

        /// <summary>
        /// Invoked when the <see cref="HourglassTimer"/> is resumed from a paused state.
        /// </summary>
        /// <param name="sender">The <see cref="HourglassTimer"/>.</param>
        /// <param name="e">The event data.</param>
        private void TimerResumed(object sender, EventArgs e)
        {
            if (this.View != TimerWindowView.Input)
            {
                this.ShowViewForTimer();
            }
            else
            {
                this.UpdateButtons();
            }
        }

        /// <summary>
        /// Invoked when the <see cref="HourglassTimer"/> is stopped.
        /// </summary>
        /// <param name="sender">The <see cref="HourglassTimer"/>.</param>
        /// <param name="e">The event data.</param>
        private void TimerStopped(object sender, EventArgs e)
        {
            if (this.View != TimerWindowView.Input)
            {
                this.ShowViewForTimer();
            }
            else
            {
                this.UpdateButtons();
            }
        }

        /// <summary>
        /// Invoked when the <see cref="HourglassTimer"/> expires.
        /// </summary>
        /// <param name="sender">The <see cref="HourglassTimer"/>.</param>
        /// <param name="e">The event data.</param>
        private void TimerExpired(object sender, EventArgs e)
        {
            if (this.View != TimerWindowView.Input)
            {
                this.ShowViewForTimer();
            }
            else
            {
                this.UpdateButtons();
            }
        }

        /// <summary>
        /// Invoked when the <see cref="HourglassTimer"/> ticks.
        /// </summary>
        /// <param name="sender">The <see cref="HourglassTimer"/>.</param>
        /// <param name="e">The event data.</param>
        private void TimerTick(object sender, EventArgs e)
        {
            // Do nothing
        }

        /// <summary>
        /// Invoked when a <see cref="HourglassTimer"/> property value changes.
        /// </summary>
        /// <param name="sender">The <see cref="HourglassTimer"/>.</param>
        /// <param name="e">The event data.</param>
        private void TimerPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.UpdateTimerBinding();
        }

        #endregion

        #region Private Methods (Window Events)

        /// <summary>
        /// Invoked when the <see cref="StartButton"/> is clicked.
        /// </summary>
        /// <param name="sender">The <see cref="StartButton"/>.</param>
        /// <param name="e">The event data.</param>
        private void StartButtonClick(object sender, RoutedEventArgs e)
        {
            TimerInput input = TimerInput.FromString(this.TimerTextBox.Text);
            if (input == null)
            {
                this.BeginValidationErrorAnimation();
                return;
            }

            TimerInputManager.Instance.Add(input);

            this.Timer = HourglassTimer.GetTimerForInput(input);
            this.Timer.Start(input);
            TimerManager.Instance.Add(this.Timer);
        }

        /// <summary>
        /// Invoked when the <see cref="PauseButton"/> is clicked.
        /// </summary>
        /// <param name="sender">The <see cref="PauseButton"/>.</param>
        /// <param name="e">The event data.</param>
        private void PauseButtonClick(object sender, RoutedEventArgs e)
        {
            this.Timer.Pause();
        }

        /// <summary>
        /// Invoked when the <see cref="ResumeButton"/> is clicked.
        /// </summary>
        /// <param name="sender">The <see cref="ResumeButton"/>.</param>
        /// <param name="e">The event data.</param>
        private void ResumeButtonClick(object sender, RoutedEventArgs e)
        {
            this.Timer.Resume();
        }

        /// <summary>
        /// Invoked when the <see cref="StopButton"/> is clicked.
        /// </summary>
        /// <param name="sender">The <see cref="StopButton"/>.</param>
        /// <param name="e">The event data.</param>
        private void StopButtonClick(object sender, RoutedEventArgs e)
        {
            this.Timer.Stop();
        }

        /// <summary>
        /// Invoked when the <see cref="ResetButton"/> is clicked.
        /// </summary>
        /// <param name="sender">The <see cref="ResetButton"/>.</param>
        /// <param name="e">The event data.</param>
        private void ResetButtonClick(object sender, RoutedEventArgs e)
        {
            this.ShowInputView();
        }

        /// <summary>
        /// Invoked when the <see cref="CancelButton"/> is clicked.
        /// </summary>
        /// <param name="sender">The <see cref="CancelButton"/>.</param>
        /// <param name="e">The event data.</param>
        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            if (this.Timer.State == TimerState.Running || this.Timer.State == TimerState.Paused)
            {
                this.ShowStatusView();
            }
        }

        /// <summary>
        /// Invoked when any mouse button is depressed on the <see cref="TimerTextBox"/>.
        /// </summary>
        /// <param name="sender">The <see cref="TimerTextBox"/>.</param>
        /// <param name="e">The event data.</param>
        private void TimerTextBoxMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (this.View != TimerWindowView.Input)
            {
                this.ShowInputView();
                e.Handled = true;
            }
        }

        /// <summary>
        /// Invoked when any mouse button is depressed on the <see cref="TimerWindow"/>.
        /// </summary>
        /// <param name="sender">The <see cref="TimerWindow"/>.</param>
        /// <param name="e">The event data.</param>
        private void WindowMouseDown(object sender, MouseButtonEventArgs e)
        {
            switch (this.View)
            {
                case TimerWindowView.Input:
                    if (this.Timer.State == TimerState.Running || this.Timer.State == TimerState.Paused)
                    {
                        this.ShowStatusView();
                    }

                    return;

                case TimerWindowView.Status:
                    FocusUtility.RemoveFocus(this.TimerTextBox);
                    FocusUtility.RemoveFocus(this.TitleTextBox);
                    return;

                case TimerWindowView.Expired:
                    this.ShowInputView();
                    return;
            }
        }

        /// <summary>
        /// Invoked when the <see cref="TimerWindow"/> is about to close.
        /// </summary>
        /// <param name="sender">The <see cref="TimerWindow"/>.</param>
        /// <param name="e">The event data.</param>
        private void WindowClosed(object sender, EventArgs e)
        {
            if (this.Timer.State == TimerState.Stopped || this.Timer.State == TimerState.Expired)
            {
                TimerManager.Instance.Remove(this.Timer);
            }
        }

        #endregion
    }
}
