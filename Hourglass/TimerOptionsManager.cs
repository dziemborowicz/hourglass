// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimerOptionsManager.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass
{
    using System.Linq;
    using System.Windows;

    using Hourglass.Properties;
    using Hourglass.Serialization;

    /// <summary>
    /// Manages <see cref="TimerOptions"/>s.
    /// </summary>
    public class TimerOptionsManager
    {
        /// <summary>
        /// Singleton instance of the <see cref="TimerOptionsManager"/> class.
        /// </summary>
        public static readonly TimerOptionsManager Instance = new TimerOptionsManager();

        /// <summary>
        /// The default <see cref="TimerOptions"/> for new timers.
        /// </summary>
        private TimerOptions defaultOptions = new TimerOptions();

        /// <summary>
        /// Prevents a default instance of the <see cref="TimerOptionsManager"/> class from being created.
        /// </summary>
        private TimerOptionsManager()
        {
        }

        /// <summary>
        /// Gets the default <see cref="TimerOptions"/> for new timers.
        /// </summary>
        public TimerOptions DefaultOptions
        {
            get
            {
                this.UpdateDefaultOptions();

                return this.defaultOptions;
            }
        }

        /// <summary>
        /// Loads state from the default settings.
        /// </summary>
        public void Load()
        {
            this.defaultOptions = TimerOptions.FromTimerOptionsInfo(Settings.Default.DefaultOptions) ?? new TimerOptions();
        }

        /// <summary>
        /// Saves state to the default settings.
        /// </summary>
        public void Save()
        {
            this.UpdateDefaultOptions();

            Settings.Default.DefaultOptions = TimerOptionsInfo.FromTimerOptions(this.defaultOptions);
        }

        /// <summary>
        /// Updates the <see cref="DefaultOptions"/> from the currently opened <see cref="TimerWindow"/>s.
        /// </summary>
        private void UpdateDefaultOptions()
        {
            if (Application.Current == null)
            {
                return;
            }

            // Set the default options to the options most recently shown to the user from a window that is still open
            var q = from window in Application.Current.Windows.OfType<TimerWindow>()
                    where window.IsVisible
                    orderby window.Menu.LastShowed descending
                    select window.Timer.Options;

            this.defaultOptions = q.FirstOrDefault() ?? this.defaultOptions;
        }
    }
}
