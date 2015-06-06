// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AppManager.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass
{
    /// <summary>
    /// Manages the app.
    /// </summary>
    public class AppManager : Manager
    {
        /// <summary>
        /// Singleton instance of the <see cref="AppManager"/> class.
        /// </summary>
        public static readonly AppManager Instance = new AppManager();

        /// <summary>
        /// The manager class singleton instances.
        /// </summary>
        private static readonly Manager[] Managers = 
        {
            ErrorManager.Instance,
            KeepAwakeManager.Instance,
            SoundManager.Instance,
            TimerColorManager.Instance,
            TimerStartManager.Instance,
            TimerOptionsManager.Instance,
            TimerNotifyIconManager.Instance,
            TimerManager.Instance,
            SettingsManager.Instance
        };

        /// <summary>
        /// Prevents a default instance of the <see cref="AppManager"/> class from being created.
        /// </summary>
        private AppManager()
        {
        }

        /// <summary>
        /// Initializes the class.
        /// </summary>
        public override void Initialize()
        {
            foreach (Manager manager in Managers)
            {
                manager.Initialize();
            }
        }

        /// <summary>
        /// Persists the state of the class.
        /// </summary>
        public override void Persist()
        {
            foreach (Manager manager in Managers)
            {
                manager.Persist();
            }
        }

        /// <summary>
        /// Disposes the manager.
        /// </summary>
        /// <param name="disposing">A value indicating whether this method was invoked by an explicit call to <see
        /// cref="Dispose"/>.</param>
        protected override void Dispose(bool disposing)
        {
            if (this.Disposed)
            {
                return;
            }

            if (disposing)
            {
                foreach (Manager manager in Managers)
                {
                    manager.Dispose();
                }
            }

            base.Dispose(disposing);
        }
    }
}
