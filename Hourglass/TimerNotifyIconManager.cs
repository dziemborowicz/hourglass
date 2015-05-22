// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimerNotifyIconManager.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass
{
    /// <summary>
    /// Manages the <see cref="TimerNotifyIcon"/>.
    /// </summary>
    public class TimerNotifyIconManager : Manager
    {
        /// <summary>
        /// Singleton instance of the <see cref="TimerNotifyIconManager"/> class.
        /// </summary>
        public static readonly TimerNotifyIconManager Instance = new TimerNotifyIconManager();

        /// <summary>
        /// The icon for the app in the notification area of the taskbar.
        /// </summary>
        private TimerNotifyIcon notifyIcon;

        /// <summary>
        /// Prevents a default instance of the <see cref="TimerNotifyIconManager"/> class from being created.
        /// </summary>
        private TimerNotifyIconManager()
        {
        }

        /// <summary>
        /// Gets the icon for the app in the notification area of the taskbar.
        /// </summary>
        public TimerNotifyIcon NotifyIcon
        {
            get { return this.notifyIcon; }
        }

        /// <summary>
        /// Initializes the class.
        /// </summary>
        public override void Initialize()
        {
            this.notifyIcon = new TimerNotifyIcon();
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
                this.notifyIcon.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
