// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimerOptions.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass.Timing
{
    using System;
    using System.ComponentModel;
    using System.Windows;

    using Hourglass.Serialization;
    using Hourglass.Windows;

    /// <summary>
    /// Modes indicating what information to display in the timer window title.
    /// </summary>
    public enum WindowTitleMode
    {
        /// <summary>
        /// The timer window title is set to show the application name.
        /// </summary>
        ApplicationName,

        /// <summary>
        /// The timer window title is set to show the time left.
        /// </summary>
        TimeLeft,

        /// <summary>
        /// The timer window title is set to show the time elapsed.
        /// </summary>
        TimeElapsed,

        /// <summary>
        /// The timer window title is set to show the timer title.
        /// </summary>
        TimerTitle,

        /// <summary>
        /// The timer window title is set to show the time left then the timer title.
        /// </summary>
        TimeLeftPlusTimerTitle,

        /// <summary>
        /// The timer window title is set to show the time elapsed then the timer title.
        /// </summary>
        TimeElapsedPlusTimerTitle,

        /// <summary>
        /// The timer window title is set to show the timer title then the time left.
        /// </summary>
        TimerTitlePlusTimeLeft,

        /// <summary>
        /// The timer window title is set to show the timer title then the time elapsed.
        /// </summary>
        TimerTitlePlusTimeElapsed
    }

    /// <summary>
    /// Configuration data for a timer.
    /// </summary>
    public class TimerOptions : INotifyPropertyChanged
    {
        #region Private Members

        /// <summary>
        /// A user-specified title for the timer.
        /// </summary>
        private string title;

        /// <summary>
        /// A value indicating whether the timer window should always be displayed on top of other windows.
        /// </summary>
        private bool alwaysOnTop;

        /// <summary>
        /// A value indicating whether to prompt the user before closing the timer window if the timer is running.
        /// </summary>
        private bool promptOnExit;

        /// <summary>
        /// A value indicating whether to keep the computer awake while the timer is running.
        /// </summary>
        private bool doNotKeepComputerAwake;

        /// <summary>
        /// A value indicating whether to show the time elapsed rather than the time left.
        /// </summary>
        private bool showTimeElapsed;

        /// <summary>
        /// A value indicating whether to loop the timer continuously.
        /// </summary>
        private bool loopTimer;

        /// <summary>
        /// A value indicating whether the timer window should be brought to the top of other windows when the timer
        /// expires.
        /// </summary>
        private bool popUpWhenExpired;

        /// <summary>
        /// A value indicating whether the timer window should be closed when the timer expires.
        /// </summary>
        private bool closeWhenExpired;

        /// <summary>
        /// A value indicating whether Windows should be shut down when the timer expires.
        /// </summary>
        private bool shutDownWhenExpired;

        /// <summary>
        /// The sound to play when the timer expires, or <c>null</c> if no sound is to be played.
        /// </summary>
        private Sound sound;

        /// <summary>
        /// A value indicating whether the sound that plays when the timer expires should be looped until stopped by
        /// the user.
        /// </summary>
        private bool loopSound;

        /// <summary>
        /// The theme of the timer window.
        /// </summary>
        private Theme theme;

        /// <summary>
        /// A value indicating what information to display in the timer window title.
        /// </summary>
        private WindowTitleMode windowTitleMode;

        /// <summary>
        /// The size, position, and state of the timer window.
        /// </summary>
        private WindowSize windowSize;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TimerOptions"/> class.
        /// </summary>
        public TimerOptions()
        {
            this.title = string.Empty;
            this.alwaysOnTop = false;
            this.promptOnExit = true;
            this.doNotKeepComputerAwake = false;
            this.showTimeElapsed = false;
            this.loopTimer = false;
            this.popUpWhenExpired = true;
            this.closeWhenExpired = false;
            this.shutDownWhenExpired = false;
            this.theme = Theme.DefaultTheme;
            this.sound = Sound.DefaultSound;
            this.loopSound = false;
            this.windowTitleMode = WindowTitleMode.ApplicationName;
            this.windowSize = new WindowSize(
                new Rect(double.PositiveInfinity, double.PositiveInfinity, 350, 150),
                WindowState.Normal,
                WindowState.Normal,
                false /* isFullScreen */);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TimerOptions"/> class from another instance of the <see
        /// cref="TimerOptions"/> class.
        /// </summary>
        /// <param name="options">A <see cref="TimerOptions"/>.</param>
        public TimerOptions(TimerOptions options)
        {
            this.Set(options);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TimerOptions"/> class from a <see cref="TimerOptionsInfo"/>.
        /// </summary>
        /// <param name="info">A <see cref="TimerOptionsInfo"/>.</param>
        public TimerOptions(TimerOptionsInfo info)
        {
            this.Set(info);
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
        /// Gets or sets a user-specified title for the timer.
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
        /// Gets or sets a value indicating whether the timer window should always be displayed on top of other windows.
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
        /// Gets or sets a value indicating whether to prompt the user before closing the timer window if the timer is
        /// running.
        /// </summary>
        public bool PromptOnExit
        {
            get
            {
                return this.promptOnExit;
            }

            set
            {
                if (this.promptOnExit == value)
                {
                    return;
                }

                this.promptOnExit = value;
                this.OnPropertyChanged("PromptOnExit");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to keep the computer awake while the timer is running.
        /// </summary>
        public bool DoNotKeepComputerAwake
        {
            get
            {
                return this.doNotKeepComputerAwake;
            }

            set
            {
                if (this.doNotKeepComputerAwake == value)
                {
                    return;
                }

                this.doNotKeepComputerAwake = value;
                this.OnPropertyChanged("DoNotKeepComputerAwake");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to show the time elapsed rather than the time left.
        /// </summary>
        public bool ShowTimeElapsed
        {
            get
            {
                return this.showTimeElapsed;
            }

            set
            {
                if (this.showTimeElapsed == value)
                {
                    return;
                }

                this.showTimeElapsed = value;
                this.OnPropertyChanged("ShowTimeElapsed");
            }
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
        /// Gets or sets a value indicating whether the timer window should be brought to the top of other windows when
        /// the timer expires.
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
        /// Gets or sets a value indicating whether the timer window should be closed when the timer expires.
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
        /// Gets or sets a value indicating whether Windows should be shut down when the timer expires.
        /// </summary>
        public bool ShutDownWhenExpired
        {
            get
            {
                return this.shutDownWhenExpired;
            }

            set
            {
                if (this.shutDownWhenExpired == value)
                {
                    return;
                }

                this.shutDownWhenExpired = value;
                this.OnPropertyChanged("ShutDownWhenExpired");
            }
        }

        /// <summary>
        /// Gets or sets the theme of the timer window.
        /// </summary>
        public Theme Theme
        {
            get
            {
                return this.theme;
            }

            set
            {
                if (object.ReferenceEquals(this.theme, value))
                {
                    return;
                }

                this.theme = value;
                this.OnPropertyChanged("Theme");
            }
        }

        /// <summary>
        /// Gets or sets the sound to play when the timer expires, or <c>null</c> if no sound is to be played.
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
        /// Gets or sets a value indicating whether the sound that plays when the timer expires should be looped until
        /// stopped by the user.
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

        /// <summary>
        /// Gets or sets a value indicating what information to display in the timer window title.
        /// </summary>
        public WindowTitleMode WindowTitleMode
        {
            get
            {
                return this.windowTitleMode;
            }

            set
            {
                if (this.windowTitleMode == value)
                {
                    return;
                }

                this.windowTitleMode = value;
                this.OnPropertyChanged("WindowTitleMode");
            }
        }

        /// <summary>
        /// Gets or sets the size, position, and state of the timer window.
        /// </summary>
        public WindowSize WindowSize
        {
            get
            {
                return this.windowSize;
            }

            set
            {
                if (this.windowSize == value)
                {
                    return;
                }

                this.windowSize = value;
                this.OnPropertyChanged("WindowSize");
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns a <see cref="TimerOptions"/> for the specified <see cref="TimerOptions"/>, or <c>null</c> if the
        /// specified <see cref="TimerOptions"/> is <c>null</c>.
        /// </summary>
        /// <param name="options">A <see cref="TimerOptions"/>.</param>
        /// <returns>A <see cref="TimerOptions"/> for the specified <see cref="TimerOptions"/>, or <c>null</c> if the
        /// specified <see cref="TimerOptions"/> is <c>null</c>.</returns>
        public static TimerOptions FromTimerOptions(TimerOptions options)
        {
            return options != null ? new TimerOptions(options) : null;
        }

        /// <summary>
        /// Returns a <see cref="TimerOptions"/> for the specified <see cref="TimerOptionsInfo"/>, or <c>null</c> if
        /// the specified <see cref="TimerOptionsInfo"/> is <c>null</c>.
        /// </summary>
        /// <param name="info">A <see cref="TimerOptionsInfo"/>.</param>
        /// <returns>A <see cref="TimerOptions"/> for the specified <see cref="TimerOptionsInfo"/>, or <c>null</c> if
        /// the specified <see cref="TimerOptionsInfo"/> is <c>null</c>.</returns>
        public static TimerOptions FromTimerOptionsInfo(TimerOptionsInfo info)
        {
            return info != null ? new TimerOptions(info) : null;
        }

        /// <summary>
        /// Sets all of the options from another instance of the <see cref="TimerOptions"/> class.
        /// </summary>
        /// <param name="options">A <see cref="TimerOptions"/>.</param>
        public void Set(TimerOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException("options");
            }

            this.title = options.title;
            this.alwaysOnTop = options.alwaysOnTop;
            this.promptOnExit = options.promptOnExit;
            this.doNotKeepComputerAwake = options.doNotKeepComputerAwake;
            this.showTimeElapsed = options.showTimeElapsed;
            this.loopTimer = options.loopTimer;
            this.popUpWhenExpired = options.popUpWhenExpired;
            this.closeWhenExpired = options.closeWhenExpired;
            this.shutDownWhenExpired = options.shutDownWhenExpired;
            this.theme = options.theme;
            this.sound = options.sound;
            this.loopSound = options.loopSound;
            this.windowTitleMode = options.windowTitleMode;
            this.windowSize = WindowSize.FromWindowSize(options.WindowSize);

            this.OnPropertyChanged(
                "Title",
                "AlwaysOnTop",
                "PromptOnExit",
                "DoNotKeepComputerAwake",
                "ShowTimeElapsed",
                "LoopTimer",
                "PopUpWhenExpired",
                "CloseWhenExpired",
                "ShutDownWhenExpired",
                "Theme",
                "Sound",
                "LoopSound",
                "WindowTitleMode",
                "WindowSize");
        }

        /// <summary>
        /// Sets all of the options from an instance of the <see cref="TimerOptionsInfo"/> class.
        /// </summary>
        /// <param name="info">A <see cref="TimerOptionsInfo"/>.</param>
        public void Set(TimerOptionsInfo info)
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }

            this.title = info.Title;
            this.alwaysOnTop = info.AlwaysOnTop;
            this.promptOnExit = info.PromptOnExit;
            this.doNotKeepComputerAwake = info.DoNotKeepComputerAwake;
            this.showTimeElapsed = info.ShowTimeElapsed;
            this.loopTimer = info.LoopTimer;
            this.popUpWhenExpired = info.PopUpWhenExpired;
            this.closeWhenExpired = info.CloseWhenExpired;
            this.shutDownWhenExpired = info.ShutDownWhenExpired;
            this.theme = Theme.FromIdentifier(info.ThemeIdentifier);
            this.sound = Sound.FromIdentifier(info.SoundIdentifier);
            this.loopSound = info.LoopSound;
            this.windowTitleMode = info.WindowTitleMode;
            this.windowSize = WindowSize.FromWindowSizeInfo(info.WindowSize);

            this.OnPropertyChanged(
                "Title",
                "AlwaysOnTop",
                "PromptOnExit",
                "DoNotKeepComputerAwake",
                "ShowTimeElapsed",
                "LoopTimer",
                "PopUpWhenExpired",
                "CloseWhenExpired",
                "ShutDownWhenExpired",
                "Theme",
                "Sound",
                "LoopSound",
                "WindowTitleMode",
                "WindowSize");
        }

        /// <summary>
        /// Returns the representation of the <see cref="TimerOptions"/> used for XML serialization.
        /// </summary>
        /// <returns>The representation of the <see cref="TimerOptions"/> used for XML serialization.</returns>
        public TimerOptionsInfo ToTimerOptionsInfo()
        {
            return new TimerOptionsInfo
            {
                Title = this.title,
                AlwaysOnTop = this.alwaysOnTop,
                PromptOnExit = this.promptOnExit,
                DoNotKeepComputerAwake = this.doNotKeepComputerAwake,
                ShowTimeElapsed = this.showTimeElapsed,
                LoopTimer = this.loopTimer,
                PopUpWhenExpired = this.popUpWhenExpired,
                CloseWhenExpired = this.closeWhenExpired,
                ShutDownWhenExpired = this.shutDownWhenExpired,
                ThemeIdentifier = this.theme?.Identifier,
                SoundIdentifier = this.sound?.Identifier,
                LoopSound = this.loopSound,
                WindowTitleMode = this.windowTitleMode,
                WindowSize = WindowSizeInfo.FromWindowSize(this.windowSize)
            };
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
    }
}
