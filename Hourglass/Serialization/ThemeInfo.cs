// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ThemeInfo.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass.Serialization
{
    using Hourglass.Timing;

    /// <summary>
    /// The representation of a <see cref="Theme"/> used for XML serialization.
    /// </summary>
    public class ThemeInfo
    {
        /// <summary>
        /// Gets or sets the friendly name for this theme, or <c>null</c> if no friendly name is specified.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the background color of the window.
        /// </summary>
        public System.Windows.Media.Color BackgroundColor { get; set; }

        /// <summary>
        /// Gets or sets the background color of the progress bar. See <see cref="TimerOptions.Color"/> for the
        /// foreground color of the progress bar.
        /// </summary>
        public System.Windows.Media.Color ProgressBackgroundColor { get; set; }

        /// <summary>
        /// Gets or sets the color that is flashed on expiration.
        /// </summary>
        public System.Windows.Media.Color ExpirationFlashColor { get; set; }

        /// <summary>
        /// Gets or sets the color of the primary text.
        /// </summary>
        public System.Windows.Media.Color PrimaryTextColor { get; set; }

        /// <summary>
        /// Gets or sets the color of the watermark in the primary text box.
        /// </summary>
        public System.Windows.Media.Color PrimaryHintColor { get; set; }

        /// <summary>
        /// Gets or sets the color of any secondary text.
        /// </summary>
        public System.Windows.Media.Color SecondaryTextColor { get; set; }

        /// <summary>
        /// Gets or sets the color of the watermark in any secondary text box.
        /// </summary>
        public System.Windows.Media.Color SecondaryHintColor { get; set; }

        /// <summary>
        /// Gets or sets the color of the button text.
        /// </summary>
        public System.Windows.Media.Color ButtonColor { get; set; }

        /// <summary>
        /// Gets or sets the color of the button text when the user hovers over the button.
        /// </summary>
        public System.Windows.Media.Color ButtonHoverColor { get; set; }

        /// <summary>
        /// Returns a <see cref="ThemeInfo"/> for the specified <see cref="Theme"/>.
        /// </summary>
        /// <param name="options">A <see cref="Theme"/>.</param>
        /// <returns>A <see cref="ThemeInfo"/> for the specified <see cref="Theme"/>.</returns>
        public static ThemeInfo FromTheme(Theme options)
        {
            if (options == null)
            {
                return null;
            }

            return options.ToThemeInfo();
        }
    }
}
