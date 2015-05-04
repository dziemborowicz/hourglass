// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimerOptions.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass
{
    using System;
    using System.ComponentModel;

    /// <summary>
    /// Configuration data for a timer.
    /// </summary>
    public abstract class TimerOptions : INotifyPropertyChanged
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
        /// A value indicating whether an icon for the timer window should be shown in the notification area (system
        /// tray).
        /// </summary>
        private bool showInNotificationArea;

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
        /// The sound to play when the timer expires, or <c>null</c> if no sound is to be played.
        /// </summary>
        private Sound sound;

        /// <summary>
        /// A value indicating whether the sound that plays when the timer expires should be looped until stopped by
        /// the user.
        /// </summary>
        private bool loopSound;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TimerOptions"/> class.
        /// </summary>
        protected TimerOptions()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TimerOptions"/> class from another instance of the <see
        /// cref="TimerOptions"/> class.
        /// </summary>
        /// <param name="options">A <see cref="TimerOptions"/>.</param>
        protected TimerOptions(TimerOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException("options");
            }

            this.title = options.Title;
            this.alwaysOnTop = options.AlwaysOnTop;
            this.showInNotificationArea = options.ShowInNotificationArea;
            this.popUpWhenExpired = options.PopUpWhenExpired;
            this.closeWhenExpired = options.CloseWhenExpired;
            this.sound = options.Sound;
            this.loopSound = options.LoopSound;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TimerOptions"/> class from a <see cref="TimerOptionsInfo"/>.
        /// </summary>
        /// <param name="optionsInfo">A <see cref="TimerOptionsInfo"/>.</param>
        protected TimerOptions(TimerOptionsInfo optionsInfo)
        {
            if (optionsInfo == null)
            {
                throw new ArgumentNullException("optionsInfo");
            }

            this.title = optionsInfo.Title;
            this.alwaysOnTop = optionsInfo.AlwaysOnTop;
            this.showInNotificationArea = optionsInfo.ShowInNotificationArea;
            this.popUpWhenExpired = optionsInfo.PopUpWhenExpired;
            this.closeWhenExpired = optionsInfo.CloseWhenExpired;
            this.sound = SoundManager.Instance.GetSoundOrDefault(optionsInfo.SoundIdentifier);
            this.loopSound = optionsInfo.LoopSound;
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
        /// Gets or sets a value indicating whether an icon for the timer window should be shown in the notification
        /// area (system tray).
        /// </summary>
        public bool ShowInNotificationArea
        {
            get
            {
                return this.showInNotificationArea;
            }

            set
            {
                if (this.showInNotificationArea == value)
                {
                    return;
                }

                this.showInNotificationArea = value;
                this.OnPropertyChanged("ShowInNotificationArea");
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

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns a copy of the <see cref="TimerOptions"/>, or <c>null</c> if the <see cref="TimerOptions"/> is not a
        /// supported type.
        /// </summary>
        /// <param name="options">A <see cref="TimerOptions"/>.</param>
        /// <returns>A copy of the <see cref="TimerOptions"/>, or <c>null</c> if the <see cref="TimerOptions"/> is not
        /// a supported type.</returns>
        public static TimerOptions FromTimerOptions(TimerOptions options)
        {
            TimeSpanTimerOptions timeSpanTimerOptions = options as TimeSpanTimerOptions;
            if (timeSpanTimerOptions != null)
            {
                return new TimeSpanTimerOptions(timeSpanTimerOptions);
            }

            DateTimeTimerOptions dateTimeTimerOptions = options as DateTimeTimerOptions;
            if (dateTimeTimerOptions != null)
            {
                return new DateTimeTimerOptions(dateTimeTimerOptions);
            }

            return null;
        }

        /// <summary>
        /// Returns a <see cref="TimerOptions"/> for the specified <see cref="TimerOptionsInfo"/>, or <c>null</c> if
        /// the <see cref="TimerOptionsInfo"/> is not a supported type.
        /// </summary>
        /// <param name="optionsInfo">A <see cref="TimerInputInfo"/>.</param>
        /// <returns>A <see cref="TimerOptions"/> for the specified <see cref="TimerOptionsInfo"/>, or <c>null</c> if
        /// the <see cref="TimerOptionsInfo"/> is not a supported type.</returns>
        public static TimerOptions FromTimerOptionsInfo(TimerOptionsInfo optionsInfo)
        {
            TimeSpanTimerOptionsInfo timeSpanTimerOptionsInfo = optionsInfo as TimeSpanTimerOptionsInfo;
            if (timeSpanTimerOptionsInfo != null)
            {
                return new TimeSpanTimerOptions(timeSpanTimerOptionsInfo);
            }

            DateTimeTimerOptionsInfo dateTimeTimerOptionsInfo = optionsInfo as DateTimeTimerOptionsInfo;
            if (dateTimeTimerOptionsInfo != null)
            {
                return new DateTimeTimerOptions(dateTimeTimerOptionsInfo);
            }

            return null;
        }

        /// <summary>
        /// Returns the representation of the <see cref="TimerOptions"/> used for XML serialization.
        /// </summary>
        /// <returns>The representation of the <see cref="TimerOptions"/> used for XML serialization.</returns>
        public TimerOptionsInfo ToTimerOptionsInfo()
        {
            TimerOptionsInfo info = this.GetNewTimerOptionsInfo();
            this.SetTimerOptionsInfo(info);
            return info;
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Returns a new <see cref="TimerOptionsInfo"/> of the correct type for this class.
        /// </summary>
        /// <returns>A new <see cref="TimerOptionsInfo"/>.</returns>
        protected abstract TimerOptionsInfo GetNewTimerOptionsInfo();

        /// <summary>
        /// Sets the properties on a <see cref="TimerOptionsInfo"/> from the values in this class.
        /// </summary>
        /// <param name="timerOptionsInfo">A <see cref="TimerOptionsInfo"/>.</param>
        protected virtual void SetTimerOptionsInfo(TimerOptionsInfo timerOptionsInfo)
        {
            timerOptionsInfo.Title = this.title;
            timerOptionsInfo.AlwaysOnTop = this.alwaysOnTop;
            timerOptionsInfo.ShowInNotificationArea = this.showInNotificationArea;
            timerOptionsInfo.PopUpWhenExpired = this.popUpWhenExpired;
            timerOptionsInfo.CloseWhenExpired = this.closeWhenExpired;
            timerOptionsInfo.SoundIdentifier = this.sound != null ? this.sound.Identifier : null;
            timerOptionsInfo.LoopSound = this.loopSound;
        }

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
