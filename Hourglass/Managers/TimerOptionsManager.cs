// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimerOptionsManager.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass.Managers
{
    using System.Linq;
    using System.Windows;

    using Hourglass.Properties;
    using Hourglass.Timing;
    using Hourglass.Windows;

    /// <summary>
    /// Manages <see cref="TimerOptions"/>s.
    /// </summary>
    public class TimerOptionsManager : Manager
    {
        /// <summary>
        /// Singleton instance of the <see cref="TimerOptionsManager"/> class.
        /// </summary>
        public static readonly TimerOptionsManager Instance = new TimerOptionsManager();

        /// <summary>
        /// The most recent <see cref="TimerOptions"/>.
        /// </summary>
        private TimerOptions mostRecentOptions = new TimerOptions();

        /// <summary>
        /// Prevents a default instance of the <see cref="TimerOptionsManager"/> class from being created.
        /// </summary>
        private TimerOptionsManager()
        {
        }

        /// <summary>
        /// Gets the most recent <see cref="TimerOptions"/>.
        /// </summary>
        public TimerOptions MostRecentOptions
        {
            get
            {
                this.UpdateMostRecentOptions();
                return TimerOptions.FromTimerOptions(this.mostRecentOptions);
            }
        }

        /// <summary>
        /// Initializes the class.
        /// </summary>
        public override void Initialize()
        {
            this.mostRecentOptions = Settings.Default.MostRecentOptions ?? new TimerOptions();
        }

        /// <summary>
        /// Persists the state of the class.
        /// </summary>
        public override void Persist()
        {
            this.UpdateMostRecentOptions();
            Settings.Default.MostRecentOptions = this.mostRecentOptions;
        }

        /// <summary>
        /// Updates the <see cref="MostRecentOptions"/> from the currently opened <see cref="TimerWindow"/>s.
        /// </summary>
        private void UpdateMostRecentOptions()
        {
            if (Application.Current == null)
            {
                return;
            }

            // Get the options most recently shown to the user from a window that is still open
            var q = from window in Application.Current.Windows.OfType<TimerWindow>()
                    where window.IsVisible
                    orderby window.Menu.LastShowed descending
                    select window.Options;

            this.mostRecentOptions = TimerOptions.FromTimerOptions(q.FirstOrDefault()) ?? this.mostRecentOptions;

            // Never save a title
            this.mostRecentOptions.Title = string.Empty;

            // Never save shutting down when expired or lock interface options
            this.mostRecentOptions.ShutDownWhenExpired = false;
            this.mostRecentOptions.LockInterface = false;
        }
    }
}
