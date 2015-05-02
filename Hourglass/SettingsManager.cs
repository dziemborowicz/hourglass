// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SettingsManager.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass
{
    using Hourglass.Properties;

    /// <summary>
    /// Manages default settings.
    /// </summary>
    public class SettingsManager
    {
        /// <summary>
        /// Singleton instance of the <see cref="SettingsManager"/> class.
        /// </summary>
        public static readonly SettingsManager Instance = new SettingsManager();

        /// <summary>
        /// Prevents a default instance of the <see cref="SettingsManager"/> class from being created.
        /// </summary>
        private SettingsManager()
        {
        }

        /// <summary>
        /// Loads state from the default settings.
        /// </summary>
        public void Load()
        {
            Settings.Default.Reload();

            TimerManager.Instance.Load();
            TimerInputManager.Instance.Load();
        }

        /// <summary>
        /// Saves state to the default settings.
        /// </summary>
        public void Save()
        {
            TimerManager.Instance.Save();
            TimerInputManager.Instance.Save();

            Settings.Default.Save();
        }
    }
}
