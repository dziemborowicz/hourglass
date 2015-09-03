// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NotificationAreaIconManager.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass.Managers
{
    using Hourglass.Windows;

    /// <summary>
    /// Manages the <see cref="NotificationAreaIcon"/>.
    /// </summary>
    public class NotificationAreaIconManager : Manager
    {
        /// <summary>
        /// Singleton instance of the <see cref="NotificationAreaIconManager"/> class.
        /// </summary>
        public static readonly NotificationAreaIconManager Instance = new NotificationAreaIconManager();

        /// <summary>
        /// The icon for the app in the notification area of the taskbar.
        /// </summary>
        private NotificationAreaIcon notifyIcon;

        /// <summary>
        /// Prevents a default instance of the <see cref="NotificationAreaIconManager"/> class from being created.
        /// </summary>
        private NotificationAreaIconManager()
        {
        }

        /// <summary>
        /// Gets the icon for the app in the notification area of the taskbar.
        /// </summary>
        public NotificationAreaIcon NotifyIcon
        {
            get { return this.notifyIcon; }
        }

        /// <summary>
        /// Initializes the class.
        /// </summary>
        public override void Initialize()
        {
            this.notifyIcon = new NotificationAreaIcon();
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
