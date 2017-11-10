// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimerWindow.xaml.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass.Windows
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media.Animation;
    using System.Windows.Shell;
    
    using Hourglass.Extensions;
    using Hourglass.Managers;
    using Hourglass.Properties;
    using Hourglass.Timing;

    /// <summary>
    /// The mode of a <see cref="TimerWindow"/>.
    /// </summary>
    public enum TimerWindowMode
    {
        /// <summary>
        /// Indicates that the <see cref="TimerWindow"/> is accepting user input to start a new timer.
        /// </summary>
        Input,

        /// <summary>
        /// Indicates that the <see cref="TimerWindow"/> is displaying the status of a timer.
        /// </summary>
        Status
    }

    /// <summary>
    /// A timer window.
    /// </summary>
    public partial class TimerWindow : INotifyPropertyChanged, IRestorableWindow
    {
        #region Commands

        /// <summary>
        /// Starts a new timer.
        /// </summary>
        public static readonly RoutedCommand StartCommand = new RoutedCommand();

        /// <summary>
        /// Pauses a running timer.
        /// </summary>
        public static readonly RoutedCommand PauseCommand = new RoutedCommand();

        /// <summary>
        /// Resumes a paused timer.
        /// </summary>
        public static readonly RoutedCommand ResumeCommand = new RoutedCommand();

        /// <summary>
        /// Resumes a paused or resumes timer.
        /// </summary>
        public static readonly RoutedCommand PauseResumeCommand = new RoutedCommand();

        /// <summary>
        /// Stops a running timer and enters input mode.
        /// </summary>
        public static readonly RoutedCommand StopCommand = new RoutedCommand();

        /// <summary>
        /// Enters input mode.
        /// </summary>
        public static readonly RoutedCommand ResetCommand = new RoutedCommand();

        /// <summary>
        /// Closes the window.
        /// </summary>
        public static readonly RoutedCommand CloseCommand = new RoutedCommand();

        /// <summary>
        /// Cancels editing.
        /// </summary>
        public static readonly RoutedCommand CancelCommand = new RoutedCommand();

        /// <summary>
        /// Updates the app.
        /// </summary>
        public static readonly RoutedCommand UpdateCommand = new RoutedCommand();

        /// <summary>
        /// Exits input mode, enters input mode, or exits full-screen mode depending on the state of the window.
        /// </summary>
        public static readonly RoutedCommand EscapeCommand = new RoutedCommand();

        /// <summary>
        /// Toggles full-screen mode.
        /// </summary>
        public static readonly RoutedCommand FullScreenCommand = new RoutedCommand();

        #endregion

        #region Private Members

        /// <summary>
        /// The <see cref="ContextMenu"/> for the window.
        /// </summary>
        private readonly ContextMenu menu = new ContextMenu();

        /// <summary>
        /// The <see cref="InterfaceScaler"/> for the window.
        /// </summary>
        private readonly InterfaceScaler scaler = new InterfaceScaler();

        /// <summary>
        /// The <see cref="SoundPlayer"/> used to play notification sounds.
        /// </summary>
        private readonly SoundPlayer soundPlayer = new SoundPlayer();

        /// <summary>
        /// The <see cref="TimerWindowMode"/> of the window.
        /// </summary>
        private TimerWindowMode mode;

        /// <summary>
        /// The timer backing the window.
        /// </summary>
        private Timer timer = new Timer(TimerOptionsManager.Instance.MostRecentOptions);

        /// <summary>
        /// The timer to resume when the window loads, or <c>null</c> if no timer is to be resumed.
        /// </summary>
        private Timer timerToResumeOnLoad;

        /// <summary>
        /// The <see cref="TimerStart"/> to start when the window loads, or <c>null</c> if no <see cref="TimerStart"/>
        /// is to be started.
        /// </summary>
        private TimerStart timerStartToStartOnLoad;

        /// <summary>
        /// The last <see cref="TimerStart"/> used to start a timer in the window.
        /// </summary>
        private TimerStart lastTimerStart = TimerStartManager.Instance.LastTimerStart;

        /// <summary>
        /// The currently loaded theme.
        /// </summary>
        private Theme theme;

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

        /// <summary>
        /// A value indicating whether the window is in full-screen mode.
        /// </summary>
        private bool isFullScreen;

        /// <summary>
        /// The <see cref="Window.WindowState"/> before the window was minimized.
        /// </summary>
        private WindowState restoreWindowState = WindowState.Normal;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TimerWindow"/> class.
        /// </summary>
        public TimerWindow()
        {
            this.InitializeComponent();
            this.InitializeResources();
            this.InitializeAnimations();
            this.InitializeSoundPlayer();
            this.InitializeUpdateButton();

            this.BindTimer();
            this.SwitchToInputMode();

            this.menu.Bind(this /* window */);
            this.scaler.Bind(this /* window */);

            TimerManager.Instance.Add(this.Timer);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TimerWindow"/> class.
        /// </summary>
        /// <param name="timer">The timer to resume when the window loads, or <c>null</c> if no timer is to be resumed.
        /// </param>
        public TimerWindow(Timer timer)
            : this()
        {
            this.timerToResumeOnLoad = timer;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TimerWindow"/> class.
        /// </summary>
        /// <param name="timerStart">The <see cref="TimerStart"/> to start when the window loads, or <c>null</c> if no
        /// <see cref="TimerStart"/> is to be started.</param>
        public TimerWindow(TimerStart timerStart)
            : this()
        {
            this.timerStartToStartOnLoad = timerStart;
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
        /// Gets the <see cref="WindowSize"/> for the window persisted in the settings.
        /// </summary>
        public WindowSize PersistedSize
        {
            get { return Settings.Default.WindowSize; }
        }

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
        /// Gets the <see cref="ContextMenu"/> for the window.
        /// </summary>
        public ContextMenu Menu
        {
            get { return this.menu; }
        }

        /// <summary>
        /// Gets the <see cref="InterfaceScaler"/> for the window.
        /// </summary>
        public InterfaceScaler Scaler
        {
            get { return this.scaler; }
        }

        /// <summary>
        /// Gets the timer backing the window.
        /// </summary>
        public Timer Timer
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
                this.OnPropertyChanged("Timer", "Options");
            }
        }

        /// <summary>
        /// Gets the <see cref="TimerOptions"/> for the timer backing the window.
        /// </summary>
        public TimerOptions Options
        {
            get { return this.Timer.Options; }
        }

        /// <summary>
        /// Gets the last <see cref="TimerStart"/> used to start a timer in the window.
        /// </summary>
        public TimerStart LastTimerStart
        {
            get
            {
                return this.lastTimerStart;
            }

            private set
            {
                if (this.lastTimerStart == value)
                {
                    return;
                }

                this.lastTimerStart = value;
                this.OnPropertyChanged("TimerStart");
            }
        }

        /// <summary>
        /// Gets the currently loaded theme.
        /// </summary>
        public Theme Theme
        {
            get { return this.theme; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the window is in full-screen mode.
        /// </summary>
        public bool IsFullScreen
        {
            get
            {
                return this.isFullScreen;
            }

            set
            {
                if (this.isFullScreen == value)
                {
                    return;
                }

                this.isFullScreen = value;

                if (this.isFullScreen)
                {
                    this.WindowStyle = WindowStyle.None;
                    this.WindowState = WindowState.Normal; // Needed to put the window on top of the taskbar
                    this.WindowState = WindowState.Maximized;
                    this.ResizeMode = ResizeMode.NoResize;
                }
                else
                {
                    this.WindowStyle = WindowStyle.SingleBorderWindow;
                    this.WindowState = this.restoreWindowState;
                    this.ResizeMode = ResizeMode.CanResize;
                }

                this.OnPropertyChanged("IsFullScreen");
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="Window.WindowState"/> before the window was minimized.
        /// </summary>
        public WindowState RestoreWindowState
        {
            get
            {
                return this.restoreWindowState;
            }

            set
            {
                if (this.restoreWindowState == value)
                {
                    return;
                }

                this.restoreWindowState = value;
                this.OnPropertyChanged("RestoreWindowState");
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Opens the <see cref="TimerWindow"/> if it is not already open and displays a new timer started with the
        /// specified <see cref="TimerStart"/>.
        /// </summary>
        /// <param name="timerStart">A <see cref="TimerStart"/>.</param>
        public void Show(TimerStart timerStart)
        {
            // Keep track of the input
            this.LastTimerStart = timerStart;

            // Start a new timer
            Timer newTimer = new Timer(this.Options);
            if (!newTimer.Start(timerStart))
            {
                this.Show();
                this.SwitchToInputMode();
                this.BeginValidationErrorAnimation();
                return;
            }

            TimerManager.Instance.Add(newTimer);
            TimerStartManager.Instance.Add(timerStart);

            // Show the window
            this.Show(newTimer);
        }

        /// <summary>
        /// Opens the <see cref="TimerWindow"/> if it is not already open and displays the specified timer.
        /// </summary>
        /// <param name="existingTimer">A timer.</param>
        public void Show(Timer existingTimer)
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
                if (this.Options.LoopSound)
                {
                    this.BeginExpirationAnimationAndSound();
                }
                else
                {
                    this.BeginExpirationAnimation(true /* glowOnly */);
                }
            }
        }

        /// <summary>
        /// Brings the window to the front.
        /// </summary>
        /// <returns><c>true</c> if the window is brought to the foreground, or <c>false</c> if the window cannot be
        /// brought to the foreground for any reason.</returns>
        public bool BringToFront()
        {
            try
            {
                this.Show();

                if (this.WindowState == WindowState.Minimized)
                {
                    this.WindowState = this.RestoreWindowState;
                }

                this.Topmost = false;
                this.Topmost = true;
                this.Topmost = this.Options.AlwaysOnTop;

                return true;
            }
            catch (InvalidOperationException)
            {
                // This happens if the window is closing (waiting for the user to confirm) when this method is called
                return false;
            }
        }

        /// <summary>
        /// Brings the window to the front, activates it, and focusses it.
        /// </summary>
        public void BringToFrontAndActivate()
        {
            this.BringToFront();
            this.Activate();
        }

        /// <summary>
        /// Minimizes the window to the notification area of the taskbar.
        /// </summary>
        /// <remarks>
        /// This method does nothing if <see cref="Settings.ShowInNotificationArea"/> is <c>false</c>.
        /// </remarks>
        public void MinimizeToNotificationArea()
        {
            if (Settings.Default.ShowInNotificationArea)
            {
                if (this.WindowState != WindowState.Minimized)
                {
                    this.RestoreWindowState = this.WindowState;
                    this.WindowState = WindowState.Minimized;
                }

                this.Hide();
            }
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            if (this.Timer.State == TimerState.Stopped && this.Mode == TimerWindowMode.Input)
            {
                string input = string.IsNullOrWhiteSpace(this.TimerTextBox.Text)
                    ? Properties.Resources.TimerWindowBlankTitlePlaceholder
                    : this.TimerTextBox.Text;

                string title = this.TitleTextBox.Text;

                string format = string.IsNullOrWhiteSpace(title)
                    ? Properties.Resources.TimerWindwoNewTimerFormatString
                    : Properties.Resources.TimerWindowNewTimerWithTitleFormatString;

                return string.Format(format, input, title);
            }

            return this.Timer.ToString();
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
        /// <param name="textBoxToFocus">The <see cref="TextBox"/> to focus. The default is <see cref="TimerTextBox"/>.
        /// </param>
        private void SwitchToInputMode(TextBox textBoxToFocus = null)
        {
            this.Mode = TimerWindowMode.Input;

            this.TimerTextBox.Text = this.LastTimerStart != null ? this.LastTimerStart.ToString() : string.Empty;

            textBoxToFocus = textBoxToFocus ?? this.TimerTextBox;
            textBoxToFocus.SelectAll();
            textBoxToFocus.Focus();

            this.EndAnimationsAndSounds();
            this.UpdateBoundControls();
        }

        /// <summary>
        /// Sets the window to display the status of a running or paused <see cref="Timer"/>.
        /// </summary>
        private void SwitchToStatusMode()
        {
            this.Mode = TimerWindowMode.Status;

            this.UnfocusAll();
            this.EndAnimationsAndSounds();
            this.UpdateBoundControls();
        }

        /// <summary>
        /// <para>
        /// When the window is in input mode, this method switches back to status mode if there is a running or paused
        /// timer or it stops the notification sound if it is playing.
        /// </para><para>
        /// When the window is in status mode, this method switches to input mode if the timer is expired or stops the
        /// notification sound if it is playing.
        /// </para>
        /// </summary>
        /// <remarks>
        /// This is invoked when the user presses the Escape key, or performs an equivalent action.
        /// </remarks>
        /// <returns>A value indicating whether any action was performed.</returns>
        private bool CancelOrReset()
        {
            switch (this.Mode)
            {
                case TimerWindowMode.Input:
                    // Switch back to showing the running timer if there is one
                    if (this.Timer.State != TimerState.Stopped && this.Timer.State != TimerState.Expired)
                    {
                        this.SwitchToStatusMode();
                        return true;
                    }
                    
                    // Stop playing the notification sound if it is playing
                    if (this.soundPlayer.IsPlaying)
                    {
                        this.EndAnimationsAndSounds();
                        return true;
                    }

                    return false;

                case TimerWindowMode.Status:
                    // Switch to input mode if the timer is expired
                    if (this.Timer.State == TimerState.Expired)
                    {
                        this.Timer.Stop();
                        this.SwitchToInputMode();
                        return true;
                    }
                    
                    // Stop playing the notification sound if it is playing
                    if (this.soundPlayer.IsPlaying)
                    {
                        this.EndAnimationsAndSounds();
                        return true;
                    }

                    // Stop editing and unfocus buttons if focused
                    return this.UnfocusAll();
            }

            return false;
        }

        /// <summary>
        /// Removes focus from all controls.
        /// </summary>
        /// <returns>A value indicating whether the focus was removed from any element.</returns>
        private bool UnfocusAll()
        {
            return this.TitleTextBox.Unfocus()
                || this.TimerTextBox.Unfocus()
                || this.StartButton.Unfocus()
                || this.PauseButton.Unfocus()
                || this.ResumeButton.Unfocus()
                || this.StopButton.Unfocus()
                || this.ResetButton.Unfocus()
                || this.CloseButton.Unfocus()
                || this.CancelButton.Unfocus()
                || this.UpdateButton.Unfocus();
        }

        #endregion

        #region Private Methods (Localization)

        /// <summary>
        /// Initializes localized resources.
        /// </summary>
        private void InitializeResources()
        {
            Watermark.SetHint(this.TitleTextBox, Properties.Resources.TimerWindowTitleTextHint);
            Watermark.SetHint(this.TimerTextBox, Properties.Resources.TimerWindowTimerTextHint);

            this.StartButton.Content = Properties.Resources.TimerWindowStartButtonContent;
            this.PauseButton.Content = Properties.Resources.TimerWindowPauseButtonContent;
            this.ResumeButton.Content = Properties.Resources.TimerWindowResumeButtonContent;
            this.StopButton.Content = Properties.Resources.TimerWindowStopButtonContent;
            this.ResetButton.Content = Properties.Resources.TimerWindowResetButtonContent;
            this.CloseButton.Content = Properties.Resources.TimerWindowCloseButtonContent;
            this.CancelButton.Content = Properties.Resources.TimerWindowCancelButtonContent;
            this.UpdateButton.Content = Properties.Resources.TimerWindowUpdateButtonContent;
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
            if (this.Options.PopUpWhenExpired)
            {
                this.BringToFront();
            }
            else if (Settings.Default.ShowInNotificationArea && !this.IsVisible)
            {
                NotificationAreaIconManager.Instance.NotifyIcon.ShowBalloonTipForExpiredTimer();
            }
        }

        /// <summary>
        /// Begins the sound used to notify the user that the timer has expired.
        /// </summary>
        private void BeginExpirationSound()
        {
            this.soundPlayer.Play(this.Options.Sound, this.Options.LoopSound);
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
                    if (this.flashExpirationCount < 3 || this.Options.LoopSound)
                    {
                        this.flashExpirationStoryboard.Begin();
                    }

                    break;

                case TimerWindowMode.Status:
                    if (this.Options.LoopTimer && this.timer.SupportsLooping)
                    {
                        // Flash three times, or flash indefinitely if the sound is looped
                        if (this.flashExpirationCount < 3 || this.Options.LoopSound)
                        {
                            this.flashExpirationStoryboard.Begin();
                        }
                    }
                    else if (this.Options.Sound == null && this.Options.ShutDownWhenExpired)
                    {
                        // Flash three times and then shut down -- see SoundPlayerPlaybackCompleted for case with sound
                        if (this.flashExpirationCount < 3)
                        {
                            this.flashExpirationStoryboard.Begin();
                        }
                        else
                        {
                            WindowsExtensions.ShutDown();
                        }
                    }
                    else if (this.Options.Sound == null && this.Options.CloseWhenExpired)
                    {
                        // Flash three times and then close -- see SoundPlayerPlaybackCompleted for case with sound
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
                        if (this.flashExpirationCount < 2 || this.Options.LoopSound)
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
            if (!this.Options.LoopTimer && this.Mode == TimerWindowMode.Status)
            {
                if (this.Options.ShutDownWhenExpired)
                {
                    WindowsExtensions.ShutDown();
                }
                else if (this.Options.CloseWhenExpired)
                {
                    this.Close();
                }
            }
        }

        #endregion

        #region Private Methods (Update Button)
        
        /// <summary>
        /// Initializes the update button.
        /// </summary>
        private void InitializeUpdateButton()
        {
            UpdateManager.Instance.PropertyChanged += this.UpdateManagerPropertyChanged;
            this.UpdateButton.IsEnabled = UpdateManager.Instance.HasUpdates;
        }
        
        /// <summary>
        /// Invoked when a <see cref="UpdateManager"/> property value changes.
        /// </summary>
        /// <param name="sender">The <see cref="UpdateManager"/>.</param>
        /// <param name="e">The event data.</param>
        private void UpdateManagerPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.Dispatcher.BeginInvoke(new Action(() =>
            {
                this.UpdateButton.IsEnabled = UpdateManager.Instance.HasUpdates;
            }));
        }

        #endregion

        #region Private Methods (Timer Binding)

        /// <summary>
        /// Binds the <see cref="TimerWindow"/> event handlers and controls to a timer.
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
            this.Options.PropertyChanged += this.TimerOptionsPropertyChanged;

            this.theme = this.Options.Theme;
            this.theme.PropertyChanged += this.ThemePropertyChanged;

            this.UpdateBoundControls();
        }

        /// <summary>
        /// Updates the controls bound to timer properties.
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

                    this.Topmost = this.Options.AlwaysOnTop;

                    this.UpdateBoundTheme();
                    this.UpdateKeepAwake();
                    this.UpdateWindowTitle();
                    return;

                case TimerWindowMode.Status:
                    this.TimerTextBox.Text = this.Timer.Options.ShowTimeElapsed
                        ? this.Timer.TimeElapsedAsString
                        : this.Timer.TimeLeftAsString;
                    this.ProgressBar.Value = this.Timer.TimeLeftAsPercentage ?? 0.0;
                    this.UpdateTaskbarProgress();

                    this.StartButton.IsEnabled = false;
                    this.PauseButton.IsEnabled = this.Timer.State == TimerState.Running && this.Timer.SupportsPause;
                    this.ResumeButton.IsEnabled = this.Timer.State == TimerState.Paused;
                    this.StopButton.IsEnabled = this.Timer.State != TimerState.Stopped && this.Timer.State != TimerState.Expired;
                    this.ResetButton.IsEnabled = this.Timer.State == TimerState.Stopped || this.Timer.State == TimerState.Expired;
                    this.CloseButton.IsEnabled = this.Timer.State == TimerState.Stopped || this.Timer.State == TimerState.Expired;
                    this.CancelButton.IsEnabled = false;

                    this.Topmost = this.Options.AlwaysOnTop;

                    this.UpdateBoundTheme();
                    this.UpdateKeepAwake();
                    this.UpdateWindowTitle();
                    return;
            }
        }

        /// <summary>
        /// Updates the control properties set by the bound theme.
        /// </summary>
        private void UpdateBoundTheme()
        {
            this.InnerGrid.Background = this.Theme.BackgroundBrush;
            this.ProgressBar.Foreground = this.Theme.ProgressBarBrush;
            this.ProgressBar.Background  = this.Theme.ProgressBackgroundBrush;
            this.InnerNotificationBorder.BorderBrush = this.Theme.ExpirationFlashBrush;
            this.OuterNotificationBorder.Background = this.Theme.ExpirationFlashBrush;
            this.TimerTextBox.Foreground = this.Theme.PrimaryTextBrush;
            this.TimerTextBox.CaretBrush = this.Theme.PrimaryTextBrush;
            Watermark.SetHintBrush(this.TimerTextBox, this.Theme.PrimaryHintBrush);
            this.TitleTextBox.Foreground = this.Theme.SecondaryTextBrush;
            this.TitleTextBox.CaretBrush = this.Theme.SecondaryTextBrush;
            Watermark.SetHintBrush(this.TitleTextBox, this.Theme.SecondaryHintBrush);
            this.TimeExpiredLabel.Foreground = this.Theme.SecondaryTextBrush;

            foreach (Button button in this.ButtonPanel.GetAllVisualChildren().OfType<Button>())
            {
                button.Foreground = button.IsMouseOver ? this.Theme.ButtonHoverBrush : this.Theme.ButtonBrush;
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
                    if (this.Timer.SupportsProgress)
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
                    if (this.Timer.SupportsProgress)
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
        /// Updates the registration of this window in the <see cref="KeepAwakeManager"/> based on the state of the
        /// timer.
        /// </summary>
        private void UpdateKeepAwake()
        {
            if (this.Timer.State == TimerState.Running && !this.Options.DoNotKeepComputerAwake)
            {
                KeepAwakeManager.Instance.StartKeepAwakeFor(this);
            }
            else
            {
                KeepAwakeManager.Instance.StopKeepAwakeFor(this);
            }
        }

        /// <summary>
        /// Updates the window title.
        /// </summary>
        private void UpdateWindowTitle()
        {
            switch (this.Options.WindowTitleMode)
            {
                case WindowTitleMode.ApplicationName:
                    this.Title = Properties.Resources.TimerWindowTitle;
                    break;

                case WindowTitleMode.TimeLeft:
                    this.Title = this.Timer.State != TimerState.Stopped
                        ? this.Timer.TimeLeftAsString
                        : Properties.Resources.TimerWindowTitle;
                    break;

                case WindowTitleMode.TimeElapsed:
                    this.Title = this.Timer.State != TimerState.Stopped
                        ? this.Timer.TimeElapsedAsString
                        : Properties.Resources.TimerWindowTitle;
                    break;

                case WindowTitleMode.TimerTitle:
                    this.Title = !string.IsNullOrWhiteSpace(this.Options.Title)
                        ? this.Options.Title
                        : Properties.Resources.TimerWindowTitle;
                    break;

                case WindowTitleMode.TimeLeftPlusTimerTitle:
                    if (this.Timer.State != TimerState.Stopped && !string.IsNullOrWhiteSpace(this.Options.Title))
                    {
                        this.Title = string.Join(
                            Properties.Resources.TimerWindowTitleSeparator,
                            this.Timer.TimeLeftAsString,
                            this.Options.Title);
                    }
                    else if (this.Timer.State != TimerState.Stopped)
                    {
                        this.Title = this.Timer.TimeLeftAsString;
                    }
                    else if (!string.IsNullOrWhiteSpace(this.Options.Title))
                    {
                        this.Title = this.Options.Title;
                    }
                    else
                    {
                        this.Title = Properties.Resources.TimerWindowTitle;
                    }

                    break;

                case WindowTitleMode.TimeElapsedPlusTimerTitle:
                    if (this.Timer.State != TimerState.Stopped && !string.IsNullOrWhiteSpace(this.Options.Title))
                    {
                        this.Title = string.Join(
                            Properties.Resources.TimerWindowTitleSeparator,
                            this.Timer.TimeElapsedAsString,
                            this.Options.Title);
                    }
                    else if (this.Timer.State != TimerState.Stopped)
                    {
                        this.Title = this.Timer.TimeElapsedAsString;
                    }
                    else if (!string.IsNullOrWhiteSpace(this.Options.Title))
                    {
                        this.Title = this.Options.Title;
                    }
                    else
                    {
                        this.Title = Properties.Resources.TimerWindowTitle;
                    }

                    break;

                case WindowTitleMode.TimerTitlePlusTimeLeft:
                    if (this.Timer.State != TimerState.Stopped && !string.IsNullOrWhiteSpace(this.Options.Title))
                    {
                        this.Title = string.Join(
                            Properties.Resources.TimerWindowTitleSeparator,
                            this.Options.Title,
                            this.Timer.TimeLeftAsString);
                    }
                    else if (this.Timer.State != TimerState.Stopped)
                    {
                        this.Title = this.Timer.TimeLeftAsString;
                    }
                    else if (!string.IsNullOrWhiteSpace(this.Options.Title))
                    {
                        this.Title = this.Options.Title;
                    }
                    else
                    {
                        this.Title = Properties.Resources.TimerWindowTitle;
                    }

                    break;

                case WindowTitleMode.TimerTitlePlusTimeElapsed:
                    if (this.Timer.State != TimerState.Stopped && !string.IsNullOrWhiteSpace(this.Options.Title))
                    {
                        this.Title = string.Join(
                            Properties.Resources.TimerWindowTitleSeparator,
                            this.Options.Title,
                            this.Timer.TimeElapsedAsString);
                    }
                    else if (this.Timer.State != TimerState.Stopped)
                    {
                        this.Title = this.Timer.TimeElapsedAsString;
                    }
                    else if (!string.IsNullOrWhiteSpace(this.Options.Title))
                    {
                        this.Title = this.Options.Title;
                    }
                    else
                    {
                        this.Title = Properties.Resources.TimerWindowTitle;
                    }

                    break;
            }
        }

        /// <summary>
        /// Unbinds the <see cref="TimerWindow"/> event handlers and controls from a timer.
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
            this.Options.PropertyChanged -= this.TimerOptionsPropertyChanged;

            this.Timer.Interval = TimerBase.DefaultInterval;
            this.Options.WindowSize = WindowSize.FromWindow(this /* window */);

            if (this.Timer.State == TimerState.Stopped || this.Timer.State == TimerState.Expired)
            {
                TimerManager.Instance.Remove(this.Timer);
            }
        }

        #endregion

        #region Private Methods (Timer Events)

        /// <summary>
        /// Invoked when the timer is started.
        /// </summary>
        /// <param name="sender">The timer.</param>
        /// <param name="e">The event data.</param>
        private void TimerStarted(object sender, EventArgs e)
        {
            // Do nothing
        }

        /// <summary>
        /// Invoked when the timer is paused.
        /// </summary>
        /// <param name="sender">The timer.</param>
        /// <param name="e">The event data.</param>
        private void TimerPaused(object sender, EventArgs e)
        {
            // Do nothing
        }

        /// <summary>
        /// Invoked when the timer is resumed from a paused state.
        /// </summary>
        /// <param name="sender">The timer.</param>
        /// <param name="e">The event data.</param>
        private void TimerResumed(object sender, EventArgs e)
        {
            // Do nothing
        }

        /// <summary>
        /// Invoked when the timer is stopped.
        /// </summary>
        /// <param name="sender">The timer.</param>
        /// <param name="e">The event data.</param>
        private void TimerStopped(object sender, EventArgs e)
        {
            // Do nothing
        }

        /// <summary>
        /// Invoked when the timer expires.
        /// </summary>
        /// <param name="sender">The timer.</param>
        /// <param name="e">The event data.</param>
        private void TimerExpired(object sender, EventArgs e)
        {
            this.BeginExpirationAnimationAndSound();
        }

        /// <summary>
        /// Invoked when the timer ticks.
        /// </summary>
        /// <param name="sender">The timer.</param>
        /// <param name="e">The event data.</param>
        private void TimerTick(object sender, EventArgs e)
        {
            // Do nothing
        }

        /// <summary>
        /// Invoked when a timer property value changes.
        /// </summary>
        /// <param name="sender">The timer.</param>
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
            if (e.PropertyName == "Theme")
            {
                this.theme.PropertyChanged -= this.ThemePropertyChanged;
                this.theme = this.Options.Theme;
                this.theme.PropertyChanged += this.ThemePropertyChanged;
            }

            this.UpdateBoundControls();
        }

        /// <summary>
        /// Invoked when a <see cref="Theme"/> property value changes.
        /// </summary>
        /// <param name="sender">The <see cref="Theme"/>.</param>
        /// <param name="e">The event data.</param>
        private void ThemePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.UpdateBoundControls();
        }

        #endregion

        #region Private Methods (Commands)

        /// <summary>
        /// Invoked when the <see cref="StartCommand"/> is executed.
        /// </summary>
        /// <param name="sender">The <see cref="TimerWindow"/>.</param>
        /// <param name="e">The event data.</param>
        private void StartCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            TimerStart timerStart = TimerStart.FromString(this.TimerTextBox.Text);
            if (timerStart == null)
            {
                this.BeginValidationErrorAnimation();
                return;
            }

            this.Show(timerStart);
            this.StartButton.Unfocus();
        }

        /// <summary>
        /// Invoked when the <see cref="PauseCommand"/> is executed.
        /// </summary>
        /// <param name="sender">The <see cref="TimerWindow"/>.</param>
        /// <param name="e">The event data.</param>
        private void PauseCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            this.Timer.Pause();
            this.PauseButton.Unfocus();
        }

        /// <summary>
        /// Invoked when the <see cref="ResumeCommand"/> is executed.
        /// </summary>
        /// <param name="sender">The <see cref="TimerWindow"/>.</param>
        /// <param name="e">The event data.</param>
        private void ResumeCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            this.Timer.Resume();
            this.ResumeButton.Unfocus();
        }

        /// <summary>
        /// Invoked when the <see cref="PauseResumeCommand"/> is executed.
        /// </summary>
        /// <param name="sender">The <see cref="TimerWindow"/>.</param>
        /// <param name="e">The event data.</param>
        private void PauseResumeCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (this.Timer.State == TimerState.Running)
            {
                this.Timer.Pause();
                this.PauseButton.Unfocus();
            }
            else if (this.Timer.State == TimerState.Paused)
            {
                this.Timer.Resume();
                this.ResumeButton.Unfocus();
            }
        }

        /// <summary>
        /// Invoked when the <see cref="StopCommand"/> is executed.
        /// </summary>
        /// <param name="sender">The <see cref="TimerWindow"/>.</param>
        /// <param name="e">The event data.</param>
        private void StopCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            this.Timer = new Timer(this.Options);
            TimerManager.Instance.Add(this.Timer);

            this.SwitchToInputMode();
            this.StopButton.Unfocus();
        }

        /// <summary>
        /// Invoked when the <see cref="ResetCommand"/> is executed.
        /// </summary>
        /// <param name="sender">The <see cref="TimerWindow"/>.</param>
        /// <param name="e">The event data.</param>
        private void ResetCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            this.Timer.Stop();
            this.SwitchToInputMode();
            this.ResetButton.Unfocus();
        }

        /// <summary>
        /// Invoked when the <see cref="CloseCommand"/> is executed.
        /// </summary>
        /// <param name="sender">The <see cref="TimerWindow"/>.</param>
        /// <param name="e">The event data.</param>
        private void CloseCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
            this.CloseButton.Unfocus();
        }

        /// <summary>
        /// Invoked when the <see cref="CancelCommand"/> is executed.
        /// </summary>
        /// <param name="sender">The <see cref="TimerWindow"/>.</param>
        /// <param name="e">The event data.</param>
        private void CancelCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            // Switch back to showing the running timer if there is one
            if (this.Timer.State != TimerState.Stopped && this.Timer.State != TimerState.Expired)
            {
                this.SwitchToStatusMode();
                this.CancelButton.Unfocus();
            }
        }

        /// <summary>
        /// Invoked when the <see cref="UpdateCommand"/> is executed.
        /// </summary>
        /// <param name="sender">The <see cref="TimerWindow"/>.</param>
        /// <param name="e">The event data.</param>
        private void UpdateCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Uri updateUri = UpdateManager.Instance.UpdateUri;
            if (updateUri != null && (updateUri.Scheme == Uri.UriSchemeHttp || updateUri.Scheme == Uri.UriSchemeHttps))
            {
                try
                {
                    Process.Start(updateUri.ToString());
                }
                catch (Exception ex)
                {
                    string message = string.Format(
                        Properties.Resources.TimerWindowCouldNotLaunchWebBrowserErrorMessage,
                        updateUri);

                    ErrorDialog dialog = new ErrorDialog();
                    dialog.ShowDialog(
                        title: Properties.Resources.TimerWindowCouldNotLaunchWebBrowserErrorTitle,
                        message: message,
                        details: ex.ToString());
                }
            }
        }

        /// <summary>
        /// Invoked when the <see cref="EscapeCommand"/> is executed.
        /// </summary>
        /// <param name="sender">The <see cref="TimerWindow"/>.</param>
        /// <param name="e">The event data.</param>
        private void EscapeCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            // Cancel or reset
            bool canceledOrReset = this.CancelOrReset();

            // If nothing changed, exit full-screen mode
            if (!canceledOrReset && this.IsFullScreen)
            {
                this.IsFullScreen = false;
            }
        }

        /// <summary>
        /// Invoked when the <see cref="FullScreenCommand"/> is executed.
        /// </summary>
        /// <param name="sender">The <see cref="TimerWindow"/>.</param>
        /// <param name="e">The event data.</param>
        private void FullScreenCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            this.IsFullScreen = !this.IsFullScreen;
        }

        #endregion

        #region Private Methods (Window Events)

        /// <summary>
        /// Invoked when the mouse pointer enters the bounds of a <see cref="Button"/>.
        /// </summary>
        /// <param name="sender">A <see cref="Button"/>.</param>
        /// <param name="e">The event data.</param>
        private void ButtonMouseEnter(object sender, MouseEventArgs e)
        {
            Button button = (Button)sender;
            button.Foreground = this.Theme.ButtonHoverBrush;
        }

        /// <summary>
        /// Invoked when the mouse pointer leaves the bounds of a <see cref="Button"/>.
        /// </summary>
        /// <param name="sender">A <see cref="Button"/>.</param>
        /// <param name="e">The event data.</param>
        private void ButtonMouseLeave(object sender, MouseEventArgs e)
        {
            Button button = (Button)sender;
            button.Foreground = this.Theme.ButtonBrush;
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
                this.TitleTextBox.Unfocus();
                e.Handled = true;
            }
        }

        /// <summary>
        /// Invoked when any mouse button is pressed while the pointer is over the <see cref="TitleTextBox"/>.
        /// </summary>
        /// <param name="sender">The <see cref="TitleTextBox"/>.</param>
        /// <param name="e">The event data.</param>
        private void TitleTextBoxPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (this.Mode != TimerWindowMode.Input && (this.Timer.State == TimerState.Stopped || this.Timer.State == TimerState.Expired))
            {
                this.SwitchToInputMode(this.TitleTextBox /* textBoxToFocus */);
                e.Handled = true;
            }
            else if (!this.TitleTextBox.IsFocused)
            {
                this.TitleTextBox.SelectAll();
                this.TitleTextBox.Focus();
                e.Handled = true;
            }
        }

        /// <summary>
        /// Invoked when the <see cref="TitleTextBox"/> is in the process of acquiring keyboard focus.
        /// </summary>
        /// <param name="sender">The <see cref="TitleTextBox"/>.</param>
        /// <param name="e">The event data.</param>
        private void TitleTextBoxPreviewGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (this.Mode != TimerWindowMode.Input && (this.Timer.State == TimerState.Stopped || this.Timer.State == TimerState.Expired))
            {
                this.SwitchToInputMode(this.TitleTextBox /* textBoxToFocus */);
                e.Handled = true;
            }
            else
            {
                this.TitleTextBox.SelectAll();
            }
        }

        /// <summary>
        /// Invoked when any mouse button is pressed while the pointer is over the <see cref="TimerTextBox"/>.
        /// </summary>
        /// <param name="sender">The <see cref="TimerTextBox"/>.</param>
        /// <param name="e">The event data.</param>
        private void TimerTextBoxPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (this.Mode != TimerWindowMode.Input)
            {
                this.SwitchToInputMode();
                e.Handled = true;
            }
            else if (!this.TimerTextBox.IsFocused)
            {
                this.TimerTextBox.SelectAll();
                this.TimerTextBox.Focus();
                e.Handled = true;
            }
        }

        /// <summary>
        /// Invoked when the <see cref="TimerTextBox"/> is in the process of acquiring keyboard focus.
        /// </summary>
        /// <param name="sender">The <see cref="TimerTextBox"/>.</param>
        /// <param name="e">The event data.</param>
        private void TimerTextBoxPreviewGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (this.Mode != TimerWindowMode.Input)
            {
                this.SwitchToInputMode();
                e.Handled = true;
            }
            else
            {
                this.TimerTextBox.SelectAll();
            }
        }

        /// <summary>
        /// Invoked when the <see cref="TimerWindow"/> is laid out, rendered, and ready for interaction.
        /// </summary>
        /// <param name="sender">The <see cref="TimerWindow"/>.</param>
        /// <param name="e">The event data.</param>
        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            // Deal with any input or timer set in the constructor
            if (this.timerStartToStartOnLoad != null)
            {
                this.Show(this.timerStartToStartOnLoad);
                this.timerStartToStartOnLoad = null;
                this.timerToResumeOnLoad = null;
            }
            else if (this.timerToResumeOnLoad != null)
            {
                this.Show(this.timerToResumeOnLoad);
                this.timerStartToStartOnLoad = null;
                this.timerToResumeOnLoad = null;
            }

            // Minimize to notification area if required
            if (this.WindowState == WindowState.Minimized && Settings.Default.ShowInNotificationArea)
            {
                this.MinimizeToNotificationArea();
            }
        }

        /// <summary>
        /// Invoked when any mouse button is depressed on the <see cref="TimerWindow"/>.
        /// </summary>
        /// <param name="sender">The <see cref="TimerWindow"/>.</param>
        /// <param name="e">The event data.</param>
        private void WindowMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource is Panel)
            {
                this.CancelOrReset();
                e.Handled = true;
            }
        }

        /// <summary>
        /// Invoked when a mouse button is clicked two or more times on the <see cref="TimerWindow"/>.
        /// </summary>
        /// <param name="sender">The <see cref="TimerWindow"/>.</param>
        /// <param name="e">The event data.</param>
        private void WindowMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource is Panel)
            {
                this.IsFullScreen = !this.IsFullScreen;
                e.Handled = true;
            }
        }

        /// <summary>
        /// Invoked when the window's WindowState property changes.
        /// </summary>
        /// <param name="sender">The <see cref="TimerWindow"/>.</param>
        /// <param name="e">The event data.</param>
        private void WindowStateChanged(object sender, EventArgs e)
        {
            if (this.WindowState != WindowState.Minimized && !this.IsFullScreen)
            {
                this.RestoreWindowState = this.WindowState;
            }

            if (this.WindowState == WindowState.Minimized && Settings.Default.ShowInNotificationArea)
            {
                this.MinimizeToNotificationArea();
            }

            this.UpdateBoundControls();
        }

        /// <summary>
        /// Invoked directly after <see cref="Window.Close"/> is called, and can be handled to cancel window closure.
        /// </summary>
        /// <param name="sender">The <see cref="TimerWindow"/>.</param>
        /// <param name="e">The event data.</param>
        private void WindowClosing(object sender, CancelEventArgs e)
        {
            // Prompt for confirmation if required
            if (this.Options.PromptOnExit && this.Timer.State != TimerState.Stopped && this.Timer.State != TimerState.Expired)
            {
                MessageBoxResult result = MessageBox.Show(
                    Properties.Resources.TimerWindowCloseMessageBoxText,
                    Properties.Resources.MessageBoxTitle,
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result != MessageBoxResult.Yes)
                {
                    e.Cancel = true;
                    return;
                }
            }

            // Clean up
            this.UnbindTimer();
            this.soundPlayer.Dispose();

            Settings.Default.WindowSize = WindowSize.FromWindow(this /* window */);

            KeepAwakeManager.Instance.StopKeepAwakeFor(this);
            AppManager.Instance.Persist();
        }

        #endregion
    }
}
