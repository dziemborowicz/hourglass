// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimerOptions.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass
{
    using System;
    using System.ComponentModel;

    using Hourglass.Serialization;

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
        /// A value indicating whether to loop the timer continuously.
        /// </summary>
        private bool loopTimer;

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
        public TimerOptions()
        {
            this.title = string.Empty;
            this.loopTimer = false;
            this.alwaysOnTop = false;
            this.showInNotificationArea = false;
            this.popUpWhenExpired = true;
            this.closeWhenExpired = false;
            this.sound = SoundManager.Instance.DefaultSound;
            this.loopSound = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TimerOptions"/> class from another instance of the <see
        /// cref="TimerOptions"/> class.
        /// </summary>
        /// <param name="options">A <see cref="TimerOptions"/>.</param>
        public TimerOptions(TimerOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException("options");
            }

            this.title = options.title;
            this.loopTimer = options.loopTimer;
            this.alwaysOnTop = options.alwaysOnTop;
            this.showInNotificationArea = options.showInNotificationArea;
            this.popUpWhenExpired = options.popUpWhenExpired;
            this.closeWhenExpired = options.closeWhenExpired;
            this.sound = options.sound;
            this.loopSound = options.loopSound;
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
            this.loopTimer = optionsInfo.LoopTimer;
            this.alwaysOnTop = optionsInfo.AlwaysOnTop;
            this.showInNotificationArea = optionsInfo.ShowInNotificationArea;
            this.popUpWhenExpired = optionsInfo.PopUpWhenExpired;
            this.closeWhenExpired = optionsInfo.CloseWhenExpired;
            this.sound = Sound.FromIdentifier(optionsInfo.SoundIdentifier);
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
        public virtual string Title
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
        /// Gets or sets a value indicating whether to loop the timer continuously.
        /// </summary>
        public virtual bool LoopTimer
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
        /// Gets or sets a value indicating whether the timer window should always be displayed on top of other windows.
        /// </summary>
        public virtual bool AlwaysOnTop
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
        public virtual bool ShowInNotificationArea
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
        public virtual bool PopUpWhenExpired
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
        public virtual bool CloseWhenExpired
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
        public virtual Sound Sound
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
        public virtual bool LoopSound
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
        /// <param name="optionsInfo">A <see cref="TimerInputInfo"/>.</param>
        /// <returns>A <see cref="TimerOptions"/> for the specified <see cref="TimerOptionsInfo"/>, or <c>null</c> if
        /// the specified <see cref="TimerOptionsInfo"/> is <c>null</c>.</returns>
        public static TimerOptions FromTimerOptionsInfo(TimerOptionsInfo optionsInfo)
        {
            return optionsInfo != null ? new TimerOptions(optionsInfo) : null;
        }

        /// <summary>
        /// Returns the representation of the <see cref="TimerOptions"/> used for XML serialization.
        /// </summary>
        /// <returns>The representation of the <see cref="TimerOptions"/> used for XML serialization.</returns>
        public TimerOptionsInfo ToTimerOptionsInfo()
        {
            TimerOptionsInfo info = new TimerOptionsInfo();
            info.Title = this.title;
            info.LoopTimer = this.loopTimer;
            info.AlwaysOnTop = this.alwaysOnTop;
            info.ShowInNotificationArea = this.showInNotificationArea;
            info.PopUpWhenExpired = this.popUpWhenExpired;
            info.CloseWhenExpired = this.closeWhenExpired;
            info.SoundIdentifier = this.sound != null ? this.sound.Identifier : null;
            info.LoopSound = this.loopSound;
            return info;
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
