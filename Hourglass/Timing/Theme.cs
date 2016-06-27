// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Theme.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass.Timing
{
    using System;
    using System.ComponentModel;
    using System.Windows.Media;

    using Hourglass.Extensions;
    using Hourglass.Managers;
    using Hourglass.Serialization;

    /// <summary>
    /// A theme for the timer window.
    /// </summary>
    public class Theme : INotifyPropertyChanged
    {
        #region Private Members

        /// <summary>
        /// The friendly name for this theme, or <c>null</c> if no friendly name is specified.
        /// </summary>
        private readonly string name;

        /// <summary>
        /// A unique identifier for this theme.
        /// </summary>
        private readonly string identifier;

        /// <summary>
        /// A value indicating whether this theme is defined in the assembly.
        /// </summary>
        private readonly bool isBuiltIn;

        /// <summary>
        /// The background color of the window.
        /// </summary>
        private System.Windows.Media.Color backgroundColor;

        /// <summary>
        /// The brush used to paint the background color of the window.
        /// </summary>
        private Brush backgroundBrush;

        /// <summary>
        /// The background color of the progress bar. See <see cref="TimerOptions.Color"/> for the foreground color of
        /// the progress bar.
        /// </summary>
        private System.Windows.Media.Color progressBackgroundColor;

        /// <summary>
        /// The brush used to paint the background color of the progress bar. See <see cref="TimerOptions.Color"/> for
        /// the foreground color of the progress bar.
        /// </summary>
        private Brush progressBackgroundBrush;

        /// <summary>
        /// The color that is flashed on expiration.
        /// </summary>
        private System.Windows.Media.Color expirationFlashColor;

        /// <summary>
        /// The brush used to paint the color that is flashed on expiration.
        /// </summary>
        private Brush expirationFlashBrush;

        /// <summary>
        /// The color of the primary text.
        /// </summary>
        private System.Windows.Media.Color primaryTextColor;

        /// <summary>
        /// The brush used to paint the color of the primary text.
        /// </summary>
        private Brush primaryTextBrush;

        /// <summary>
        /// The color of the watermark in the primary text box.
        /// </summary>
        private System.Windows.Media.Color primaryHintColor;

        /// <summary>
        /// The brush used to paint the color of the watermark in the primary text box.
        /// </summary>
        private Brush primaryHintBrush;

        /// <summary>
        /// The color of any secondary text.
        /// </summary>
        private System.Windows.Media.Color secondaryTextColor;

        /// <summary>
        /// The brush used to paint the color of any secondary text.
        /// </summary>
        private Brush secondaryTextBrush;

        /// <summary>
        /// The color of the watermark in any secondary text box.
        /// </summary>
        private System.Windows.Media.Color secondaryHintColor;

        /// <summary>
        /// The brush used to paint the color of the watermark in any secondary text box.
        /// </summary>
        private Brush secondaryHintBrush;

        /// <summary>
        /// The color of the button text.
        /// </summary>
        private System.Windows.Media.Color buttonColor;

        /// <summary>
        /// The brush used to paint the color of the button text.
        /// </summary>
        private Brush buttonBrush;

        /// <summary>
        /// The color of the button text when the user hovers over the button.
        /// </summary>
        private System.Windows.Media.Color buttonHoverColor;

        /// <summary>
        /// The brush used to paint the color of the button text when the user hovers over the button.
        /// </summary>
        private Brush buttonHoverBrush;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Theme"/> class.
        /// </summary>
        /// <param name="backgroundColor">The background color of the window.</param>
        /// <param name="progressBackgroundColor">The background color of the progress bar. See <see
        /// cref="TimerOptions.Color"/> for the foreground color of</param>
        /// <param name="expirationFlashColor">The color that is flashed on expiration.</param>
        /// <param name="primaryTextColor">The color of the primary text.</param>
        /// <param name="primaryHintColor">The color of the watermark in the primary text box.</param>
        /// <param name="secondaryTextColor">The color of any secondary text.</param>
        /// <param name="secondaryHintColor">The color of the watermark in any secondary text box.</param>
        /// <param name="buttonColor">The color of the button text.</param>
        /// <param name="buttonHoverColor">The color of the button text when the user hovers over the button.</param>
        /// <param name="name">The friendly name for this theme, or <c>null</c> if no friendly name is specified.</param>
        /// <param name="invariantName">A unique identifier for this theme.</param>
        /// <param name="isBuiltIn">A value indicating whether this theme is defined in the assembly.</param>
        public Theme(
            System.Windows.Media.Color backgroundColor,
            System.Windows.Media.Color progressBackgroundColor,
            System.Windows.Media.Color expirationFlashColor,
            System.Windows.Media.Color primaryTextColor,
            System.Windows.Media.Color primaryHintColor,
            System.Windows.Media.Color secondaryTextColor,
            System.Windows.Media.Color secondaryHintColor,
            System.Windows.Media.Color buttonColor,
            System.Windows.Media.Color buttonHoverColor,
            string name,
            string invariantName = null,
            bool isBuiltIn = false)
        {
            this.name = name;
            this.identifier = isBuiltIn ? ("resource:" + invariantName) : ("theme:" + name);
            this.isBuiltIn = isBuiltIn;

            this.backgroundColor = backgroundColor;
            this.progressBackgroundColor = progressBackgroundColor;
            this.expirationFlashColor = expirationFlashColor;
            this.primaryTextColor = primaryTextColor;
            this.primaryHintColor = primaryHintColor;
            this.secondaryTextColor = secondaryTextColor;
            this.secondaryHintColor = secondaryHintColor;
            this.buttonColor = buttonColor;
            this.buttonHoverColor = buttonHoverColor;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Theme"/> class.
        /// </summary>
        /// <param name="backgroundColor">The background color of the window.</param>
        /// <param name="progressBackgroundColor">The background color of the progress bar. See <see
        /// cref="TimerOptions.Color"/> for the foreground color of</param>
        /// <param name="expirationFlashColor">The color that is flashed on expiration.</param>
        /// <param name="primaryTextColor">The color of the primary text.</param>
        /// <param name="primaryHintColor">The color of the watermark in the primary text box.</param>
        /// <param name="secondaryTextColor">The color of any secondary text.</param>
        /// <param name="secondaryHintColor">The color of the watermark in any secondary text box.</param>
        /// <param name="buttonColor">The color of the button text.</param>
        /// <param name="buttonHoverColor">The color of the button text when the user hovers over the button.</param>
        /// <param name="name">The friendly name for this theme, or <c>null</c> if no friendly name is specified.</param>
        /// <param name="invariantName">A unique identifier for this theme.</param>
        /// <param name="isBuiltIn">A value indicating whether this theme is defined in the assembly.</param>
        public Theme(
            string backgroundColor,
            string progressBackgroundColor,
            string expirationFlashColor,
            string primaryTextColor,
            string primaryHintColor,
            string secondaryTextColor,
            string secondaryHintColor,
            string buttonColor,
            string buttonHoverColor,
            string name,
            string invariantName = null,
            bool isBuiltIn = false)
            : this(
                ColorExtensions.ConvertFromString(backgroundColor),
                ColorExtensions.ConvertFromString(progressBackgroundColor),
                ColorExtensions.ConvertFromString(expirationFlashColor),
                ColorExtensions.ConvertFromString(primaryTextColor),
                ColorExtensions.ConvertFromString(primaryHintColor),
                ColorExtensions.ConvertFromString(secondaryTextColor),
                ColorExtensions.ConvertFromString(secondaryHintColor),
                ColorExtensions.ConvertFromString(buttonColor),
                ColorExtensions.ConvertFromString(buttonHoverColor),
                name,
                invariantName,
                isBuiltIn)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Theme"/> class from a <see cref="ThemeInfo"/>.
        /// </summary>
        /// <param name="info">A <see cref="ThemeInfo"/>.</param>
        public Theme(ThemeInfo info)
        {
            this.name = info.Name;
            this.identifier = "theme:" + info.Name;
            this.isBuiltIn = false;

            this.backgroundColor = info.BackgroundColor;
            this.progressBackgroundColor = info.ProgressBackgroundColor;
            this.expirationFlashColor = info.ExpirationFlashColor;
            this.primaryTextColor = info.PrimaryTextColor;
            this.primaryHintColor = info.PrimaryHintColor;
            this.secondaryTextColor = info.SecondaryTextColor;
            this.secondaryHintColor = info.SecondaryHintColor;
            this.buttonColor = info.ButtonColor;
            this.buttonHoverColor = info.ButtonHoverColor;
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
        /// Gets the default theme.
        /// </summary>
        public static Theme DefaultTheme
        {
            get { return ThemeManager.Instance.DefaultTheme; }
        }

        /// <summary>
        /// Gets the friendly name of this theme, or <c>null</c> if no friendly name is specified.
        /// </summary>
        public string Name
        {
            get { return this.name; }
        }

        /// <summary>
        /// Gets the unique identifier for this theme.
        /// </summary>
        public string Identifier
        {
            get { return this.identifier; }
        }

        /// <summary>
        /// Gets a value indicating whether this theme is defined in the assembly.
        /// </summary>
        public bool IsBuiltIn
        {
            get { return this.isBuiltIn; }
        }

        /// <summary>
        /// Gets or sets the background color of the window.
        /// </summary>
        public System.Windows.Media.Color BackgroundColor
        {
            get
            {
                return this.backgroundColor;
            }

            set
            {
                if (this.backgroundColor == value)
                {
                    return;
                }

                this.backgroundColor = value;
                this.backgroundBrush = null;
                this.OnPropertyChanged(nameof(this.BackgroundColor));
            }
        }

        /// <summary>
        /// Gets the brush used to paint the background color of the window.
        /// </summary>
        public Brush BackgroundBrush
        {
            get
            {
                if (this.backgroundBrush == null)
                {
                    this.backgroundBrush = new SolidColorBrush(this.backgroundColor);
                }

                return this.backgroundBrush;
            }
        }

        /// <summary>
        /// Gets or sets the background color of the progress bar. See <see cref="TimerOptions.Color"/> for the
        /// foreground color of the progress bar.
        /// </summary>
        public System.Windows.Media.Color ProgressBackgroundColor
        {
            get
            {
                return this.progressBackgroundColor;
            }

            set
            {
                if (this.progressBackgroundColor == value)
                {
                    return;
                }

                this.progressBackgroundColor = value;
                this.progressBackgroundBrush = null;
                this.OnPropertyChanged(nameof(this.ProgressBackgroundColor));
            }
        }

        /// <summary>
        /// Gets the brush used to paint the background color of the progress bar. See <see cref="TimerOptions.Color"/>
        /// for the foreground color of the progress bar.
        /// </summary>
        public Brush ProgressBackgroundBrush
        {
            get
            {
                if (this.progressBackgroundBrush == null)
                {
                    this.progressBackgroundBrush = new SolidColorBrush(this.progressBackgroundColor);
                }

                return this.progressBackgroundBrush;
            }
        }

        /// <summary>
        /// Gets or sets the color that is flashed on expiration.
        /// </summary>
        public System.Windows.Media.Color ExpirationFlashColor
        {
            get
            {
                return this.expirationFlashColor;
            }

            set
            {
                if (this.expirationFlashColor == value)
                {
                    return;
                }

                this.expirationFlashColor = value;
                this.expirationFlashBrush = null;
                this.OnPropertyChanged(nameof(this.ExpirationFlashColor));
            }
        }

        /// <summary>
        /// Gets the brush used to paint the color that is flashed on expiration.
        /// </summary>
        public Brush ExpirationFlashBrush
        {
            get
            {
                if (this.expirationFlashBrush == null)
                {
                    this.expirationFlashBrush = new SolidColorBrush(this.expirationFlashColor);
                }

                return this.expirationFlashBrush;
            }
        }

        /// <summary>
        /// Gets or sets the color of the primary text.
        /// </summary>
        public System.Windows.Media.Color PrimaryTextColor
        {
            get
            {
                return this.primaryTextColor;
            }

            set
            {
                if (this.primaryTextColor == value)
                {
                    return;
                }

                this.primaryTextColor = value;
                this.primaryTextBrush = null;
                this.OnPropertyChanged(nameof(this.PrimaryTextColor));
            }
        }

        /// <summary>
        /// Gets the brush used to paint the color of the primary text.
        /// </summary>
        public Brush PrimaryTextBrush
        {
            get
            {
                if (this.primaryTextBrush == null)
                {
                    this.primaryTextBrush = new SolidColorBrush(this.primaryTextColor);
                }

                return this.primaryTextBrush;
            }
        }

        /// <summary>
        /// Gets or sets the color of the watermark in the primary text box.
        /// </summary>
        public System.Windows.Media.Color PrimaryHintColor
        {
            get
            {
                return this.primaryHintColor;
            }

            set
            {
                if (this.primaryHintColor == value)
                {
                    return;
                }

                this.primaryHintColor = value;
                this.primaryHintBrush = null;
                this.OnPropertyChanged(nameof(this.PrimaryHintColor));
            }
        }

        /// <summary>
        /// Gets the brush used to paint the color of the watermark in the primary text box.
        /// </summary>
        public Brush PrimaryHintBrush
        {
            get
            {
                if (this.primaryHintBrush == null)
                {
                    this.primaryHintBrush = new SolidColorBrush(this.primaryHintColor);
                }

                return this.primaryHintBrush;
            }
        }

        /// <summary>
        /// Gets or sets the color of any secondary text.
        /// </summary>
        public System.Windows.Media.Color SecondaryTextColor
        {
            get
            {
                return this.secondaryTextColor;
            }

            set
            {
                if (this.secondaryTextColor == value)
                {
                    return;
                }

                this.secondaryTextColor = value;
                this.secondaryTextBrush = null;
                this.OnPropertyChanged(nameof(this.SecondaryTextColor));
            }
        }

        /// <summary>
        /// Gets the brush used to paint the color of any secondary text.
        /// </summary>
        public Brush SecondaryTextBrush
        {
            get
            {
                if (this.secondaryTextBrush == null)
                {
                    this.secondaryTextBrush = new SolidColorBrush(this.secondaryTextColor);
                }

                return this.secondaryTextBrush;
            }
        }

        /// <summary>
        /// Gets or sets the color of the watermark in any secondary text box.
        /// </summary>
        public System.Windows.Media.Color SecondaryHintColor
        {
            get
            {
                return this.secondaryHintColor;
            }

            set
            {
                if (this.secondaryHintColor == value)
                {
                    return;
                }

                this.secondaryHintColor = value;
                this.secondaryHintBrush = null;
                this.OnPropertyChanged(nameof(this.SecondaryHintColor));
            }
        }

        /// <summary>
        /// Gets the brush used to paint the color of the watermark in any secondary text box.
        /// </summary>
        public Brush SecondaryHintBrush
        {
            get
            {
                if (this.secondaryHintBrush == null)
                {
                    this.secondaryHintBrush = new SolidColorBrush(this.secondaryHintColor);
                }

                return this.secondaryHintBrush;
            }
        }

        /// <summary>
        /// Gets or sets the color of the button text.
        /// </summary>
        public System.Windows.Media.Color ButtonColor
        {
            get
            {
                return this.buttonColor;
            }

            set
            {
                if (this.buttonColor == value)
                {
                    return;
                }

                this.buttonColor = value;
                this.buttonBrush = null;
                this.OnPropertyChanged(nameof(this.ButtonColor));
            }
        }

        /// <summary>
        /// Gets the brush used to paint the color of the button text.
        /// </summary>
        public Brush ButtonBrush
        {
            get
            {
                if (this.buttonBrush == null)
                {
                    this.buttonBrush = new SolidColorBrush(this.buttonColor);
                }

                return this.buttonBrush;
            }
        }

        /// <summary>
        /// Gets or sets the color of the button text when the user hovers over the button.
        /// </summary>
        public System.Windows.Media.Color ButtonHoverColor
        {
            get
            {
                return this.buttonHoverColor;
            }

            set
            {
                if (this.buttonHoverColor == value)
                {
                    return;
                }

                this.buttonHoverColor = value;
                this.buttonHoverBrush = null;
                this.OnPropertyChanged(nameof(this.ButtonHoverColor));
            }
        }

        /// <summary>
        /// Gets the brush used to paint the color of the button text when the user hovers over the button.
        /// </summary>
        public Brush ButtonHoverBrush
        {
            get
            {
                if (this.buttonHoverBrush == null)
                {
                    this.buttonHoverBrush = new SolidColorBrush(this.buttonHoverColor);
                }

                return this.buttonHoverBrush;
            }
        }

        #endregion

        #region Public Static Methods

        /// <summary>
        /// Returns the theme for the specified identifier, or <c>null</c> if no such theme is loaded.
        /// </summary>
        /// <param name="identifier">The identifier for the theme.</param>
        /// <returns>The theme for the specified identifier, or <c>null</c> if no such theme is loaded.</returns>
        public static Theme FromIdentifier(string identifier)
        {
            return ThemeManager.Instance.GetThemeOrDefaultByIdentifier(identifier);
        }

        /// <summary>
        /// Returns a <see cref="Theme"/> for the specified <see cref="ThemeInfo"/>, or <c>null</c> if the specified
        /// <see cref="ThemeInfo"/> is <c>null</c>.
        /// </summary>
        /// <param name="info">A <see cref="ThemeInfo"/>.</param>
        /// <returns>A <see cref="Theme"/> for the specified <see cref="ThemeInfo"/>, or <c>null</c> if the specified
        /// <see cref="ThemeInfo"/> is <c>null</c>.</returns>
        public static Theme FromThemeInfo(ThemeInfo info)
        {
            return info != null ? new Theme(info) : null;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns the representation of the <see cref="Theme"/> used for XML serialization.
        /// </summary>
        /// <returns>The representation of the <see cref="Theme"/> used for XML serialization.</returns>
        public ThemeInfo ToThemeInfo()
        {
            return new ThemeInfo
            {
                Name = this.name,
                BackgroundColor = this.backgroundColor,
                ProgressBackgroundColor = this.progressBackgroundColor,
                ExpirationFlashColor = this.expirationFlashColor,
                PrimaryTextColor = this.primaryTextColor,
                PrimaryHintColor = this.primaryHintColor,
                SecondaryTextColor = this.secondaryTextColor,
                SecondaryHintColor = this.secondaryHintColor,
                ButtonColor = this.buttonColor,
                ButtonHoverColor = this.buttonHoverColor
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
