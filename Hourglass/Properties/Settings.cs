// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Settings.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass.Properties
{
    using System.Collections.Generic;
    using System.Linq;

    using Hourglass.Serialization;
    using Hourglass.Timing;
    using Hourglass.Windows;

    /// <summary>
    /// Application settings.
    /// </summary>
#if PORTABLE
    [System.Configuration.SettingsProvider(typeof(PortableSettingsProvider))]
#endif
    internal sealed partial class Settings 
    {
        /// <summary>
        /// Gets or sets the most recent <see cref="TimerOptions"/>.
        /// </summary>
        public TimerOptions MostRecentOptions
        {
            get { return TimerOptions.FromTimerOptionsInfo(this.MostRecentOptionsInfo); }
            set { this.MostRecentOptionsInfo = TimerOptionsInfo.FromTimerOptions(value); }
        }

        /// <summary>
        /// Gets or sets the <see cref="Timer"/>s.
        /// </summary>
        public IList<Timer> Timers
        {
            get
            {
                IEnumerable<TimerInfo> timerInfos = this.TimerInfos ?? new TimerInfoList();
                return timerInfos.Select(Timer.FromTimerInfo).ToList();
            }

            set
            {
                IEnumerable<TimerInfo> timerInfos = value.Select(TimerInfo.FromTimer);
                this.TimerInfos = new TimerInfoList(timerInfos);
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="TimerStart"/>s.
        /// </summary>
        public IList<TimerStart> TimerStarts
        {
            get
            {
                IEnumerable<TimerStartInfo> timerStartInfos = this.TimerStartInfos ?? new TimerStartInfoList();
                return timerStartInfos.Select(TimerStart.FromTimerStartInfo).ToList();
            }

            set
            {
                IEnumerable<TimerStartInfo> timerStartInfos = value.Select(TimerStartInfo.FromTimerStart);
                this.TimerStartInfos = new TimerStartInfoList(timerStartInfos);
            }
        }

        /// <summary>
        /// Gets or sets the collection of the themes defined by the user.
        /// </summary>
        public IList<Theme> UserProvidedThemes
        {
            get
            {
                IEnumerable<ThemeInfo> userProvidedThemeInfos = this.UserProvidedThemeInfos ?? new ThemeInfoList();
                return userProvidedThemeInfos.Select(Theme.FromThemeInfo).ToList();
            }

            set
            {
                IEnumerable<ThemeInfo> userProvidedThemeInfos = value.Select(ThemeInfo.FromTheme);
                this.UserProvidedThemeInfos = new ThemeInfoList(userProvidedThemeInfos);
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="WindowSize"/>.
        /// </summary>
        public WindowSize WindowSize
        {
            get { return WindowSize.FromWindowSizeInfo(this.WindowSizeInfo); }
            set { this.WindowSizeInfo = Serialization.WindowSizeInfo.FromWindowSize(value); }
        }
    }
}
