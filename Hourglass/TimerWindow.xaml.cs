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
    using System.Windows.Shell;

    /// <summary>
    /// The mode of a <see cref="TimerWindow"/>.
    /// </summary>
    public enum TimerWindowMode
    {
        /// <summary>
        /// Indicates that the <see cref="TimerWindow"/> is accepting user input to start a new <see cref="Timer"/>.
        /// </summary>
        Input,

        /// <summary>
        /// Indicates that the <see cref="TimerWindow"/> is displaying the status of a <see cref="Timer"/>.
        /// </summary>
        Status
    }

    /// <summary>
    /// A timer window.
    /// </summary>
    public partial class TimerWindow : INotifyPropertyChanged
    {
        #region Private Members

        /// <summary>
        /// The <see cref="TimerWindowMode"/> of the window.
        /// </summary>
        private TimerWindowMode mode;

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
        private HourglassTimer timer = new TimeSpanTimer(TimerOptionsManager.Instance.DefaultOptions);

        /// <summary>
        /// The last <see cref="TimerInput"/> used to start a timer in the window.
        /// </summary>
        private TimerInput lastInput = TimerInputManager.Instance.LastInput;

        /// <summary>
        /// The <see cref="SoundPlayer"/> used to play notification sounds.
        /// </summary>
        private SoundPlayer soundPlayer = new SoundPlayer();

        /// <summary>
        /// The number of times the flash expiration storyboard has completed since the timer last expired.
        /// </summary>
        private int flashExpirationCount;

        /// <summary>
        /// The storyboard that flashes red to notify the user that the timer has expired.
        /// </summary>
        private Storyboard flashExpirationStoryboard;

        /// <summary>
        /// The storyboard that glows red to notify the user that the timer has expired.
        /// </summary>
        private Storyboard glowExpirationStoryboard;

        /// <summary>
        /// The storyboard that flashes red to notify the user that the input was invalid.
        /// </summary>
        private Storyboard validationErrorStoryboard;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TimerWindow"/> class.
        /// </summary>
        public TimerWindow()
        {
            this.InitializeComponent();
            this.InitializeAnimations();
            this.InitializeSoundPlayer();

            this.BindTimer();
            this.SwitchToInputMode();

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
        /// Gets the <see cref="TimerWindowMode"/> of the window.
        /// </summary>
        public TimerWindowMode Mode
        {
            get
            {
                return this.mode;
            }

            private set
            {
                if (this.mode == value)
                {
                    return;
                }

                this.mode = value;
                this.OnPropertyChanged("mode");
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
        /// Gets the <see cref="TimerMenu"/> for the window.
        /// </summary>
        public TimerMenu Menu
        {
            get { return this.menu; }
        }

        /// <summary>
        /// Gets the <see cref="TimerScaler"/> for the window.
        /// </summary>
        public TimerScaler Scaler
        {
            get { return this.scaler; }
        }

        /// <summary>
        /// Gets the <see cref="HourglassTimer"/> backing the window.
        /// </summary>
        public HourglassTimer Timer
        {
            get
            {
                return this.timer;
            }

            private set
            {
                if (this.timer == value)
                {
                    return;
                }

                this.UnbindTimer();
                this.timer = value;
                this.BindTimer();
                this.OnPropertyChanged("Timer");
            }
        }

        /// <summary>
        /// Gets the last <see cref="TimerInput"/> used to start a timer in the window.
        /// </summary>
        public TimerInput LastInput
        {
            get
            {
                return this.lastInput;
            }

            private set
            {
                if (this.lastInput == value)
                {
                    return;
                }

                this.lastInput = value;
                this.OnPropertyChanged("LastInput");
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Opens the <see cref="TimerWindow"/> if it is not already open and displays a new <see
        /// cref="HourglassTimer"/> started with the specified <see cref="TimerInput"/>.
        /// </summary>
        /// <param name="input">A <see cref="TimerInput"/>.</param>
        public void Show(TimerInput input)
        {
            // Keep track of the input
            this.LastInput = input;
            TimerInputManager.Instance.Add(input);

            // Start and show a new timer
            this.Timer = HourglassTimer.GetTimerForInput(input);
            this.Timer.Start(input);
            this.SwitchToStatusMode();
            TimerManager.Instance.Add(this.Timer);

            // Show the window if it is not already open
            if (!this.IsVisible)
            {
                this.Show();
            }
        }

        /// <summary>
        /// Opens the <see cref="TimerWindow"/> if it is not already open and displays the specified <see
        /// cref="HourglassTimer"/>.
        /// </summary>
        /// <param name="existingTimer">A <see cref="HourglassTimer"/>.</param>
        public void Show(HourglassTimer existingTimer)
        {
            // Show the status of the existing timer
            this.Timer = existingTimer;
            this.SwitchToStatusMode();
            
            // Show the window if it is not already open
            if (!this.IsVisible)
            {
                this.Show();
            }

            // Notify expiration if the existing timer is expired
            if (this.Timer.State == TimerState.Expired)
            {
                if (this.Timer.Options.LoopSound)
                {
                    this.BeginExpirationAnimationAndSound();
                }
                else
                {
                    this.BeginExpirationAnimation(true /* glowOnly */);
                }
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

        #region Private Methods (Modes)

        /// <summary>
        /// Sets the window to accept user input to start a new <see cref="Timer"/>.
        /// </summary>
        private void SwitchToInputMode()
        {
            this.Mode = TimerWindowMode.Input;
            this.AlwaysShowControls = true;

            this.TimerTextBox.Focusable = true;
            this.TimerTextBox.IsReadOnly = false;
            this.TimerTextBox.Text = this.LastInput != null ? this.LastInput.ToString() : string.Empty;
            this.TimerTextBox.SelectAll();
            this.TimerTextBox.Focus();

            this.EndAnimationsAndSounds();
            this.UpdateBoundControls();
        }

        /// <summary>
        /// Sets the window to display the status of a running or paused <see cref="Timer"/>.
        /// </summary>
        private void SwitchToStatusMode()
        {
            this.Mode = TimerWindowMode.Status;
            this.AlwaysShowControls = false;

            this.TimerTextBox.Focusable = false;
            this.TimerTextBox.IsReadOnly = true;

            FocusUtility.RemoveFocus(this.TimerTextBox);
            FocusUtility.RemoveFocus(this.TitleTextBox);

            this.EndAnimationsAndSounds();
            this.UpdateBoundControls();
        }

        #endregion

        #region Private Methods (Animations and Sounds)

        /// <summary>
        /// Initializes the animation members.
        /// </summary>
        private void InitializeAnimations()
        {
            // Flash expiration storyboard
            DoubleAnimation outerFlashAnimation = new DoubleAnimation(1.0, 0.0, new Duration(TimeSpan.FromSeconds(0.2)));
            outerFlashAnimation.EasingFunction = new ExponentialEase { EasingMode = EasingMode.EaseOut };
            Storyboard.SetTarget(outerFlashAnimation, this.OuterNotificationBorder);
            Storyboard.SetTargetProperty(outerFlashAnimation, new PropertyPath(UIElement.OpacityProperty));

            DoubleAnimation innerFlashAnimation = new DoubleAnimation(1.0, 0.0, new Duration(TimeSpan.FromSeconds(0.2)));
            innerFlashAnimation.EasingFunction = new ExponentialEase { EasingMode = EasingMode.EaseOut };
            Storyboard.SetTarget(innerFlashAnimation, this.InnerNotificationBorder);
            Storyboard.SetTargetProperty(innerFlashAnimation, new PropertyPath(UIElement.OpacityProperty));

            this.flashExpirationStoryboard = new Storyboard();
            this.flashExpirationStoryboard.Children.Add(outerFlashAnimation);
            this.flashExpirationStoryboard.Children.Add(innerFlashAnimation);
            this.flashExpirationStoryboard.Completed += this.FlashExpirationStoryboardCompleted;
            Storyboard.SetTarget(this.flashExpirationStoryboard, this);

            // Glow expiration storyboard
            DoubleAnimation outerGlowAnimation = new DoubleAnimation(1.0, 0.5, new Duration(TimeSpan.FromSeconds(1.5)));
            outerGlowAnimation.EasingFunction = new ExponentialEase { EasingMode = EasingMode.EaseInOut };
            Storyboard.SetTarget(outerGlowAnimation, this.OuterNotificationBorder);
            Storyboard.SetTargetProperty(outerGlowAnimation, new PropertyPath(UIElement.OpacityProperty));

            DoubleAnimation innerGlowAnimation = new DoubleAnimation(1.0, 0.5, new Duration(TimeSpan.FromSeconds(1.5)));
            innerGlowAnimation.EasingFunction = new ExponentialEase { EasingMode = EasingMode.EaseInOut };
            Storyboard.SetTarget(innerGlowAnimation, this.InnerNotificationBorder);
            Storyboard.SetTargetProperty(innerGlowAnimation, new PropertyPath(UIElement.OpacityProperty));

            this.glowExpirationStoryboard = new Storyboard();
            this.glowExpirationStoryboard.Children.Add(outerGlowAnimation);
            this.glowExpirationStoryboard.Children.Add(innerGlowAnimation);
            this.glowExpirationStoryboard.AutoReverse = true;
            this.glowExpirationStoryboard.RepeatBehavior = RepeatBehavior.Forever;
            Storyboard.SetTarget(this.glowExpirationStoryboard, this);

            // Validation error storyboard
            DoubleAnimation validationErrorAnimation = new DoubleAnimation(1.0, 0.0, new Duration(TimeSpan.FromSeconds(1)));
            validationErrorAnimation.EasingFunction = new ExponentialEase { EasingMode = EasingMode.EaseOut };
            Storyboard.SetTarget(validationErrorAnimation, this.InnerNotificationBorder);
            Storyboard.SetTargetProperty(validationErrorAnimation, new PropertyPath(UIElement.OpacityProperty));

            this.validationErrorStoryboard = new Storyboard();
            this.validationErrorStoryboard.Children.Add(validationErrorAnimation);
            Storyboard.SetTarget(this.validationErrorStoryboard, this);
        }

        /// <summary>
        /// Initializes the sound player.
        /// </summary>
        private void InitializeSoundPlayer()
        {
            this.soundPlayer.PlaybackStarted += this.SoundPlayerPlaybackStarted;
            this.soundPlayer.PlaybackStopped += this.SoundPlayerPlaybackStopped;
            this.soundPlayer.PlaybackCompleted += this.SoundPlayerPlaybackCompleted;
        }

        /// <summary>
        /// Begins the animation used to notify the user that the timer has expired.
        /// </summary>
        /// <param name="glowOnly"><c>true</c> to show the glow animation only, or <c>false</c> to show the flash
        /// animation followed by the glow animation. Default is <c>false</c>.</param>
        private void BeginExpirationAnimation(bool glowOnly = false)
        {
            // Begin animation
            if (glowOnly)
            {
                this.glowExpirationStoryboard.Stop();
                this.glowExpirationStoryboard.Begin();
            }
            else
            {
                this.flashExpirationCount = 0;
                this.flashExpirationStoryboard.Stop();
                this.flashExpirationStoryboard.Begin();
            }

            // Bring the window to the front if required
            if (this.Timer.Options.PopUpWhenExpired)
            {
                this.Topmost = false;
                this.Topmost = true;
                this.Topmost = this.Timer.Options.AlwaysOnTop;
            }
        }

        /// <summary>
        /// Begins the sound used to notify the user that the timer has expired.
        /// </summary>
        private void BeginExpirationSound()
        {
            this.soundPlayer.Play(this.Timer.Options.Sound, this.Timer.Options.LoopSound);
        }

        /// <summary>
        /// Begins the animation and sound used to notify the user that the timer has expired.
        /// </summary>
        private void BeginExpirationAnimationAndSound()
        {
            this.BeginExpirationAnimation();
            this.BeginExpirationSound();
        }

        /// <summary>
        /// Begins the animation used notify the user that the input was invalid.
        /// </summary>
        private void BeginValidationErrorAnimation()
        {
            this.validationErrorStoryboard.Stop();
            this.validationErrorStoryboard.Begin();
        }

        /// <summary>
        /// Stops all animations and sounds.
        /// </summary>
        private void EndAnimationsAndSounds()
        {
            this.flashExpirationCount = 0;
            this.flashExpirationStoryboard.Stop();
            this.glowExpirationStoryboard.Stop();
            this.validationErrorStoryboard.Stop();

            this.soundPlayer.Stop();
        }

        /// <summary>
        /// Invoked when the flash expiration storyboard has completely finished playing.
        /// </summary>
        /// <param name="sender">The originator of the event.</param>
        /// <param name="e">The event data.</param>
        private void FlashExpirationStoryboardCompleted(object sender, EventArgs e)
        {
            this.flashExpirationCount++;

            switch (this.Mode)
            {
                case TimerWindowMode.Input:
                    // Flash three times, or flash indefinitely if the sound is looped
                    if (this.flashExpirationCount < 3 || this.Timer.Options.LoopSound)
                    {
                        this.flashExpirationStoryboard.Begin();
                    }

                    break;

                case TimerWindowMode.Status:
                    if (this.Timer.Options.LoopTimer && !(this.Timer is DateTimeTimer))
                    {
                        // Flash three times, or flash indefinitely if the sound is looped
                        if (this.flashExpirationCount < 3 || this.Timer.Options.LoopSound)
                        {
                            this.flashExpirationStoryboard.Begin();
                        }
                    }
                    else if (this.Timer.Options.Sound == null && this.Timer.Options.CloseWhenExpired)
                    {
                        // Flash three times and then close
                        if (this.flashExpirationCount < 3)
                        {
                            this.flashExpirationStoryboard.Begin();
                        }
                        else
                        {
                            this.Close();
                        }
                    }
                    else
                    {
                        // Flash three times and then glow, or flash indefinitely if the sound is looped
                        if (this.flashExpirationCount < 2 || this.Timer.Options.LoopSound)
                        {
                            this.flashExpirationStoryboard.Begin();
                        }
                        else
                        {
                            this.glowExpirationStoryboard.Begin();
                        }
                    }

                    break;
            }
        }

        /// <summary>
        /// Invoked when sound playback has started.
        /// </summary>
        /// <param name="sender">A <see cref="SoundPlayer"/>.</param>
        /// <param name="e">The event data.</param>
        private void SoundPlayerPlaybackStarted(object sender, EventArgs e)
        {
            // Do nothing
        }

        /// <summary>
        /// Invoked when sound playback has stopped.
        /// </summary>
        /// <param name="sender">A <see cref="SoundPlayer"/>.</param>
        /// <param name="e">The event data.</param>
        private void SoundPlayerPlaybackStopped(object sender, EventArgs e)
        {
            // Do nothing
        }

        /// <summary>
        /// Invoked when sound playback has completed.
        /// </summary>
        /// <param name="sender">A <see cref="SoundPlayer"/>.</param>
        /// <param name="e">The event data.</param>
        private void SoundPlayerPlaybackCompleted(object sender, EventArgs e)
        {
            if (this.Timer.Options.CloseWhenExpired && !this.Timer.Options.LoopTimer && this.Mode == TimerWindowMode.Status)
            {
                this.Close();
            }
        }

        #endregion

        #region Private Methods (Timer Binding)

        /// <summary>
        /// Binds the <see cref="TimerWindow"/> event handlers and controls to a <see cref="HourglassTimer"/>.
        /// </summary>
        private void BindTimer()
        {
            this.Timer.Started += this.TimerStarted;
            this.Timer.Paused += this.TimerPaused;
            this.Timer.Resumed += this.TimerResumed;
            this.Timer.Stopped += this.TimerStopped;
            this.Timer.Expired += this.TimerExpired;
            this.Timer.Tick += this.TimerTick;
            this.Timer.PropertyChanged += this.TimerPropertyChanged;
            this.Timer.Options.PropertyChanged += this.TimerOptionsPropertyChanged;

            this.UpdateBoundControls();
        }

        /// <summary>
        /// Updates the controls bound to <see cref="HourglassTimer"/> properties.
        /// </summary>
        private void UpdateBoundControls()
        {
            switch (this.Mode)
            {
                case TimerWindowMode.Input:
                    this.ProgressBar.Value = this.Timer.TimeLeftAsPercentage ?? 0.0;
                    this.UpdateTaskbarProgress();

                    this.StartButton.IsEnabled = true;
                    this.PauseButton.IsEnabled = false;
                    this.ResumeButton.IsEnabled = false;
                    this.StopButton.IsEnabled = false;
                    this.ResetButton.IsEnabled = false;
                    this.CloseButton.IsEnabled = false;
                    this.CancelButton.IsEnabled = this.Timer.State != TimerState.Stopped && this.Timer.State != TimerState.Expired;

                    this.Topmost = this.Timer.Options.AlwaysOnTop;
                    return;

                case TimerWindowMode.Status:
                    this.TimerTextBox.Text = this.Timer.TimeLeftAsString;
                    this.ProgressBar.Value = this.Timer.TimeLeftAsPercentage ?? 0.0;
                    this.UpdateTaskbarProgress();

                    this.StartButton.IsEnabled = false;
                    this.PauseButton.IsEnabled = this.Timer.State == TimerState.Running && !(this.Timer is DateTimeTimer);
                    this.ResumeButton.IsEnabled = this.Timer.State == TimerState.Paused;
                    this.StopButton.IsEnabled = this.Timer.State != TimerState.Stopped && this.Timer.State != TimerState.Expired;
                    this.ResetButton.IsEnabled = this.Timer.State == TimerState.Stopped || this.Timer.State == TimerState.Expired;
                    this.CloseButton.IsEnabled = this.Timer.State == TimerState.Stopped || this.Timer.State == TimerState.Expired;
                    this.CancelButton.IsEnabled = false;

                    this.Topmost = this.Timer.Options.AlwaysOnTop;
                    return;
            }
        }

        /// <summary>
        /// Updates the progress shown in the taskbar.
        /// </summary>
        private void UpdateTaskbarProgress()
        {
            switch (this.Timer.State)
            {
                case TimerState.Stopped:
                    this.TaskbarItemInfo.ProgressState = TaskbarItemProgressState.None;
                    this.TaskbarItemInfo.ProgressValue = 0.0;
                    break;

                case TimerState.Running:
                    if (!(this.Timer is DateTimeTimer))
                    {
                        this.TaskbarItemInfo.ProgressState = TaskbarItemProgressState.Normal;
                        this.TaskbarItemInfo.ProgressValue = (this.Timer.TimeLeftAsPercentage ?? 0.0) / 100.0;
                    }
                    else
                    {
                        this.TaskbarItemInfo.ProgressState = TaskbarItemProgressState.None;
                        this.TaskbarItemInfo.ProgressValue = 0.0;
                    }

                    break;

                case TimerState.Paused:
                    if (!(this.Timer is DateTimeTimer))
                    {
                        this.TaskbarItemInfo.ProgressState = TaskbarItemProgressState.Paused;
                        this.TaskbarItemInfo.ProgressValue = (this.Timer.TimeLeftAsPercentage ?? 0.0) / 100.0;
                    }
                    else
                    {
                        this.TaskbarItemInfo.ProgressState = TaskbarItemProgressState.Paused;
                        this.TaskbarItemInfo.ProgressValue = 0.0;
                    }

                    break;

                case TimerState.Expired:
                    this.TaskbarItemInfo.ProgressState = TaskbarItemProgressState.Error;
                    this.TaskbarItemInfo.ProgressValue = 1.0;
                    break;
            }
        }

        /// <summary>
        /// Unbinds the <see cref="TimerWindow"/> event handlers and controls from a <see cref="HourglassTimer"/>.
        /// </summary>
        private void UnbindTimer()
        {
            this.Timer.Started -= this.TimerStarted;
            this.Timer.Paused -= this.TimerPaused;
            this.Timer.Resumed -= this.TimerResumed;
            this.Timer.Stopped -= this.TimerStopped;
            this.Timer.Expired -= this.TimerExpired;
            this.Timer.Tick -= this.TimerTick;
            this.Timer.PropertyChanged -= this.TimerPropertyChanged;
            this.Timer.Options.PropertyChanged -= this.TimerOptionsPropertyChanged;

            if (this.Timer.State == TimerState.Stopped || this.Timer.State == TimerState.Expired)
            {
                TimerManager.Instance.Remove(this.Timer);
            }
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
            this.UpdateBoundControls();
        }

        /// <summary>
        /// Invoked when the <see cref="HourglassTimer"/> is paused.
        /// </summary>
        /// <param name="sender">The <see cref="HourglassTimer"/>.</param>
        /// <param name="e">The event data.</param>
        private void TimerPaused(object sender, EventArgs e)
        {
            this.UpdateBoundControls();
        }

        /// <summary>
        /// Invoked when the <see cref="HourglassTimer"/> is resumed from a paused state.
        /// </summary>
        /// <param name="sender">The <see cref="HourglassTimer"/>.</param>
        /// <param name="e">The event data.</param>
        private void TimerResumed(object sender, EventArgs e)
        {
            this.UpdateBoundControls();
        }

        /// <summary>
        /// Invoked when the <see cref="HourglassTimer"/> is stopped.
        /// </summary>
        /// <param name="sender">The <see cref="HourglassTimer"/>.</param>
        /// <param name="e">The event data.</param>
        private void TimerStopped(object sender, EventArgs e)
        {
            this.UpdateBoundControls();
        }

        /// <summary>
        /// Invoked when the <see cref="HourglassTimer"/> expires.
        /// </summary>
        /// <param name="sender">The <see cref="HourglassTimer"/>.</param>
        /// <param name="e">The event data.</param>
        private void TimerExpired(object sender, EventArgs e)
        {
            this.BeginExpirationAnimationAndSound();
            this.UpdateBoundControls();
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
            this.UpdateBoundControls();
        }

        /// <summary>
        /// Invoked when a <see cref="TimerOptions"/> property value changes.
        /// </summary>
        /// <param name="sender">The <see cref="TimerOptions"/>.</param>
        /// <param name="e">The event data.</param>
        private void TimerOptionsPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.UpdateBoundControls();
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

            input.Options.SetFromTimerOptions(this.Timer.Options);
            this.Show(input);
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
            this.SwitchToInputMode();
        }

        /// <summary>
        /// Invoked when the <see cref="ResetButton"/> is clicked.
        /// </summary>
        /// <param name="sender">The <see cref="ResetButton"/>.</param>
        /// <param name="e">The event data.</param>
        private void ResetButtonClick(object sender, RoutedEventArgs e)
        {
            this.Timer.Stop();
            this.SwitchToInputMode();
        }

        /// <summary>
        /// Invoked when the <see cref="CloseButton"/> is clicked.
        /// </summary>
        /// <param name="sender">The <see cref="CloseButton"/>.</param>
        /// <param name="e">The event data.</param>
        private void CloseButtonClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Invoked when the <see cref="CancelButton"/> is clicked.
        /// </summary>
        /// <param name="sender">The <see cref="CancelButton"/>.</param>
        /// <param name="e">The event data.</param>
        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            if (this.Timer.State != TimerState.Stopped && this.Timer.State != TimerState.Expired)
            {
                this.SwitchToStatusMode();
            }
        }

        /// <summary>
        /// Invoked when a key on the keyboard is pressed in the <see cref="TitleTextBox"/>.
        /// </summary>
        /// <param name="sender">The <see cref="TitleTextBox"/>.</param>
        /// <param name="e">The event data.</param>
        private void TitleTextBoxKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && this.Mode == TimerWindowMode.Status)
            {
                FocusUtility.RemoveFocus(this.TitleTextBox);
                e.Handled = true;
            }
        }

        /// <summary>
        /// Invoked when any mouse button is depressed on the <see cref="TimerTextBox"/>.
        /// </summary>
        /// <param name="sender">The <see cref="TimerTextBox"/>.</param>
        /// <param name="e">The event data.</param>
        private void TimerTextBoxMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (this.Mode != TimerWindowMode.Input)
            {
                this.SwitchToInputMode();
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
            switch (this.Mode)
            {
                case TimerWindowMode.Input:
                    if (this.Timer.State != TimerState.Stopped && this.Timer.State != TimerState.Expired)
                    {
                        this.SwitchToStatusMode();
                    }

                    this.EndAnimationsAndSounds();

                    return;

                case TimerWindowMode.Status:
                    if (this.Timer.State != TimerState.Expired)
                    {
                        FocusUtility.RemoveFocus(this.TimerTextBox);
                        FocusUtility.RemoveFocus(this.TitleTextBox);
                    }
                    else
                    {
                        this.Timer.Stop();
                        this.SwitchToInputMode();
                    }

                    this.EndAnimationsAndSounds();

                    return;
            }
        }

        /// <summary>
        /// Invoked directly after <see cref="Window.Close"/> is called, and can be handled to cancel window closure.
        /// </summary>
        /// <param name="sender">The <see cref="TimerWindow"/>.</param>
        /// <param name="e">The event data.</param>
        private void WindowClosing(object sender, CancelEventArgs e)
        {
            this.UnbindTimer();
            this.soundPlayer.Dispose();
            SettingsManager.Instance.Save();
        }

        #endregion
    }
}
