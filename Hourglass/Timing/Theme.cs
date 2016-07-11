// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Theme.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass.Timing
{
    using System.ComponentModel;
    using System.Linq;
    using System.Windows.Media;

    using Hourglass.Extensions;
    using Hourglass.Managers;
    using Hourglass.Serialization;

    /// <summary>
    /// The type of theme.
    /// </summary>
    public enum ThemeType
    {
        /// <summary>
        /// A built-in theme with a light background.
        /// </summary>
        BuiltInLight,

        /// <summary>
        /// A built-in theme with a dark background.
        /// </summary>
        BuiltInDark,

        /// <summary>
        /// A theme that is provided by the user.
        /// </summary>
        UserProvided
    }

    /// <summary>
    /// A theme for the timer window.
    /// </summary>
    public class Theme : INotifyPropertyChanged
    {
        #region Private Members

        /// <summary>
        /// The type of this theme.
        /// </summary>
        private readonly ThemeType type;

        /// <summary>
        /// A unique identifier for this theme.
        /// </summary>
        private readonly string identifier;

        /// <summary>
        /// The friendly name for this theme, or <c>null</c> if no friendly name is specified.
        /// </summary>
        private string name;

        /// <summary>
        /// The background color of the window.
        /// </summary>
        private Color backgroundColor;

        /// <summary>
        /// The brush used to paint the background color of the window.
        /// </summary>
        private Brush backgroundBrush;

        /// <summary>
        /// The color of the progress bar.
        /// </summary>
        private Color progressBarColor;

        /// <summary>
        /// The brush used to paint the color of the progress bar.
        /// </summary>
        private Brush progressBarBrush;

        /// <summary>
        /// The background color of the progress bar.
        /// </summary>
        private Color progressBackgroundColor;

        /// <summary>
        /// The brush used to paint the background color of the progress bar.
        /// </summary>
        private Brush progressBackgroundBrush;

        /// <summary>
        /// The color that is flashed on expiration.
        /// </summary>
        private Color expirationFlashColor;

        /// <summary>
        /// The brush used to paint the color that is flashed on expiration.
        /// </summary>
        private Brush expirationFlashBrush;

        /// <summary>
        /// The color of the primary text.
        /// </summary>
        private Color primaryTextColor;

        /// <summary>
        /// The brush used to paint the color of the primary text.
        /// </summary>
        private Brush primaryTextBrush;

        /// <summary>
        /// The color of the watermark in the primary text box.
        /// </summary>
        private Color primaryHintColor;

        /// <summary>
        /// The brush used to paint the color of the watermark in the primary text box.
        /// </summary>
        private Brush primaryHintBrush;

        /// <summary>
        /// The color of any secondary text.
        /// </summary>
        private Color secondaryTextColor;

        /// <summary>
        /// The brush used to paint the color of any secondary text.
        /// </summary>
        private Brush secondaryTextBrush;

        /// <summary>
        /// The color of the watermark in any secondary text box.
        /// </summary>
        private Color secondaryHintColor;

        /// <summary>
        /// The brush used to paint the color of the watermark in any secondary text box.
        /// </summary>
        private Brush secondaryHintBrush;

        /// <summary>
        /// The color of the button text.
        /// </summary>
        private Color buttonColor;

        /// <summary>
        /// The brush used to paint the color of the button text.
        /// </summary>
        private Brush buttonBrush;

        /// <summary>
        /// The color of the button text when the user hovers over the button.
        /// </summary>
        private Color buttonHoverColor;

        /// <summary>
        /// The brush used to paint the color of the button text when the user hovers over the button.
        /// </summary>
        private Brush buttonHoverBrush;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Theme"/> class.
        /// </summary>
        /// <param name="type">The type of this theme.</param>
        /// <param name="identifier">A unique identifier for this theme.</param>
        /// <param name="name">The friendly name for this theme, or <c>null</c> if no friendly name is specified.</param>
        /// <param name="backgroundColor">The background color of the window.</param>
        /// <param name="progressBarColor">The color of the progress bar.</param>
        /// <param name="progressBackgroundColor">The background color of the progress bar.</param>
        /// <param name="expirationFlashColor">The color that is flashed on expiration.</param>
        /// <param name="primaryTextColor">The color of the primary text.</param>
        /// <param name="primaryHintColor">The color of the watermark in the primary text box.</param>
        /// <param name="secondaryTextColor">The color of any secondary text.</param>
        /// <param name="secondaryHintColor">The color of the watermark in any secondary text box.</param>
        /// <param name="buttonColor">The color of the button text.</param>
        /// <param name="buttonHoverColor">The color of the button text when the user hovers over the button.</param>
        public Theme(
            ThemeType type,
            string identifier,
            string name,
            Color backgroundColor,
            Color progressBarColor,
            Color progressBackgroundColor,
            Color expirationFlashColor,
            Color primaryTextColor,
            Color primaryHintColor,
            Color secondaryTextColor,
            Color secondaryHintColor,
            Color buttonColor,
            Color buttonHoverColor)
        {
            this.type = type;
            this.identifier = identifier;
            this.name = name;

            this.backgroundColor = backgroundColor;
            this.progressBarColor = progressBarColor;
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
        /// <param name="type">The type of this theme.</param>
        /// <param name="identifier">A unique identifier for this theme.</param>
        /// <param name="name">The friendly name for this theme, or <c>null</c> if no friendly name is specified.</param>
        /// <param name="backgroundColor">The background color of the window.</param>
        /// <param name="progressBarColor">The color of the progress bar.</param>
        /// <param name="progressBackgroundColor">The background color of the progress bar.</param>
        /// <param name="expirationFlashColor">The color that is flashed on expiration.</param>
        /// <param name="primaryTextColor">The color of the primary text.</param>
        /// <param name="primaryHintColor">The color of the watermark in the primary text box.</param>
        /// <param name="secondaryTextColor">The color of any secondary text.</param>
        /// <param name="secondaryHintColor">The color of the watermark in any secondary text box.</param>
        /// <param name="buttonColor">The color of the button text.</param>
        /// <param name="buttonHoverColor">The color of the button text when the user hovers over the button.</param>
        public Theme(
            ThemeType type,
            string identifier,
            string name,
            string backgroundColor,
            string progressBarColor,
            string progressBackgroundColor,
            string expirationFlashColor,
            string primaryTextColor,
            string primaryHintColor,
            string secondaryTextColor,
            string secondaryHintColor,
            string buttonColor,
            string buttonHoverColor)
            : this(
                type,
                identifier,
                name,
                ColorExtensions.FromString(backgroundColor),
                ColorExtensions.FromString(progressBarColor),
                ColorExtensions.FromString(progressBackgroundColor),
                ColorExtensions.FromString(expirationFlashColor),
                ColorExtensions.FromString(primaryTextColor),
                ColorExtensions.FromString(primaryHintColor),
                ColorExtensions.FromString(secondaryTextColor),
                ColorExtensions.FromString(secondaryHintColor),
                ColorExtensions.FromString(buttonColor),
                ColorExtensions.FromString(buttonHoverColor))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Theme"/> class.
        /// </summary>
        /// <param name="type">The type of this theme.</param>
        /// <param name="identifier">A unique identifier for this theme.</param>
        /// <param name="name">The friendly name for this theme, or <c>null</c> if no friendly name is specified.</param>
        /// <param name="theme">A theme from which to copy colors.</param>
        public Theme(ThemeType type, string identifier, string name, Theme theme)
            : this(
                type,
                identifier,
                name,
                theme.backgroundColor,
                theme.progressBarColor,
                theme.progressBackgroundColor,
                theme.expirationFlashColor,
                theme.primaryTextColor,
                theme.primaryHintColor,
                theme.secondaryTextColor,
                theme.secondaryHintColor,
                theme.buttonColor,
                theme.buttonHoverColor)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Theme"/> class from a <see cref="ThemeInfo"/>.
        /// </summary>
        /// <param name="info">A <see cref="ThemeInfo"/>.</param>
        public Theme(ThemeInfo info)
            : this(
                ThemeType.UserProvided,
                info.Identifier,
                info.Name,
                info.BackgroundColor,
                info.ProgressBarColor,
                info.ProgressBackgroundColor,
                info.ExpirationFlashColor,
                info.PrimaryTextColor,
                info.PrimaryHintColor,
                info.SecondaryTextColor,
                info.SecondaryHintColor,
                info.ButtonColor,
                info.ButtonHoverColor)
        {
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
        /// Gets the type of this theme.
        /// </summary>
        public ThemeType Type
        {
            get { return this.type; }
        }

        /// <summary>
        /// Gets the unique identifier for this theme.
        /// </summary>
        public string Identifier
        {
            get { return this.identifier; }
        }

        /// <summary>
        /// Gets or sets the friendly name of this theme, or <c>null</c> if no friendly name is specified.
        /// </summary>
        public string Name
        {
            get
            {
                return this.name;
            }

            set
            {
                if (this.name == value)
                {
                    return;
                }

                this.name = value;
                this.OnPropertyChanged(nameof(this.Name));
            }
        }

        /// <summary>
        /// Gets or sets the background color of the window.
        /// </summary>
        public Color BackgroundColor
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
                this.OnPropertyChanged(nameof(this.BackgroundBrush));
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
        /// Gets or sets the color of the progress bar.
        /// </summary>
        public Color ProgressBarColor
        {
            get
            {
                return this.progressBarColor;
            }

            set
            {
                if (this.progressBarColor == value)
                {
                    return;
                }

                this.progressBarColor = value;
                this.progressBarBrush = null;
                this.OnPropertyChanged(nameof(this.ProgressBarColor));
                this.OnPropertyChanged(nameof(this.ProgressBarBrush));
            }
        }

        /// <summary>
        /// Gets the brush used to paint the color of the progress bar.
        /// </summary>
        public Brush ProgressBarBrush
        {
            get
            {
                if (this.progressBarBrush == null)
                {
                    this.progressBarBrush = new SolidColorBrush(this.progressBarColor);
                }

                return this.progressBarBrush;
            }
        }

        /// <summary>
        /// Gets or sets the background color of the progress bar.
        /// </summary>
        public Color ProgressBackgroundColor
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
                this.OnPropertyChanged(nameof(this.ProgressBackgroundBrush));
            }
        }

        /// <summary>
        /// Gets the brush used to paint the background color of the progress bar.
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
        public Color ExpirationFlashColor
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
                this.OnPropertyChanged(nameof(this.ExpirationFlashBrush));
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
        public Color PrimaryTextColor
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
                this.OnPropertyChanged(nameof(this.PrimaryTextBrush));
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
        public Color PrimaryHintColor
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
                this.OnPropertyChanged(nameof(this.PrimaryHintBrush));
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
        public Color SecondaryTextColor
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
                this.OnPropertyChanged(nameof(this.SecondaryTextBrush));
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
        public Color SecondaryHintColor
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
                this.OnPropertyChanged(nameof(this.SecondaryHintBrush));
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
        public Color ButtonColor
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
                this.OnPropertyChanged(nameof(this.ButtonBrush));
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
        public Color ButtonHoverColor
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
                this.OnPropertyChanged(nameof(this.ButtonHoverBrush));
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

        /// <summary>
        /// Gets the light variant of this theme.
        /// </summary>
        public Theme LightVariant
        {
            get { return ThemeManager.Instance.GetLightVariantForTheme(this); }
        }

        /// <summary>
        /// Gets the dark variant of this theme.
        /// </summary>
        public Theme DarkVariant
        {
            get { return ThemeManager.Instance.GetDarkVariantForTheme(this); }
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
        /// Returns a <see cref="Theme"/> that is a copy of another <see cref="Theme"/>.
        /// </summary>
        /// <param name="type">The type of this theme.</param>
        /// <param name="identifier">A unique identifier for this theme.</param>
        /// <param name="name">The friendly name for this theme, or <c>null</c> if no friendly name is specified.</param>
        /// <param name="theme">A theme from which to copy colors.</param>
        /// <returns>A <see cref="Theme"/> that is a copy of another <see cref="Theme"/>.</returns>
        public static Theme FromTheme(ThemeType type, string identifier, string name, Theme theme)
        {
            return new Theme(type, identifier, name, theme);
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
        /// Returns the unique colors used in this theme.
        /// </summary>
        /// <returns>The unique colors used in this theme.</returns>
        public Color[] GetPalette()
        {
            Color[] allColors =
            {
                this.ProgressBarColor,
                this.ProgressBackgroundColor,
                this.BackgroundColor,
                this.ExpirationFlashColor,
                this.PrimaryTextColor,
                this.PrimaryHintColor,
                this.SecondaryTextColor,
                this.SecondaryHintColor,
                this.ButtonColor,
                this.ButtonHoverColor
            };

            return allColors.Distinct().ToArray();
        }

        /// <summary>
        /// Sets all of the properties, except for <see cref="Type"/> and <see cref="Identifier"/>, from another
        /// instance of the <see cref="Theme"/> class.
        /// </summary>
        /// <param name="theme">Another instance of the <see cref="Theme"/> class.</param>
        public void Set(Theme theme)
        {
            this.Name = theme.Name;
            this.BackgroundColor = theme.BackgroundColor;
            this.ProgressBarColor = theme.ProgressBarColor;
            this.ProgressBackgroundColor = theme.ProgressBackgroundColor;
            this.ExpirationFlashColor = theme.ExpirationFlashColor;
            this.PrimaryTextColor = theme.PrimaryTextColor;
            this.PrimaryHintColor = theme.PrimaryHintColor;
            this.SecondaryTextColor = theme.SecondaryTextColor;
            this.SecondaryHintColor = theme.SecondaryHintColor;
            this.ButtonColor = theme.ButtonColor;
            this.ButtonHoverColor = theme.ButtonHoverColor;
        }

        /// <summary>
        /// Returns the representation of the <see cref="Theme"/> used for XML serialization.
        /// </summary>
        /// <returns>The representation of the <see cref="Theme"/> used for XML serialization.</returns>
        public ThemeInfo ToThemeInfo()
        {
            return new ThemeInfo
            {
                Identifier = this.identifier,
                Name = this.name,
                BackgroundColor = this.backgroundColor,
                ProgressBarColor = this.progressBarColor,
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
