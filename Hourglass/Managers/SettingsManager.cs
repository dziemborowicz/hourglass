// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SettingsManager.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass.Managers
{
    using Hourglass.Properties;

    /// <summary>
    /// Manages default settings.
    /// </summary>
    public class SettingsManager : Manager
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
        /// Initializes the class.
        /// </summary>
        public override void Initialize()
        {
            if (Settings.Default.UpgradeRequired)
            {
                Settings.Default.Upgrade();
                Settings.Default.UpgradeRequired = false;
                Settings.Default.Save();
            }
        }

        /// <summary>
        /// Persists the state of the class.
        /// </summary>
        public override void Persist()
        {
            Settings.Default.Save();
        }
    }
}
