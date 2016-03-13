// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NotificationAreaIcon.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass.Windows
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Windows;
    using System.Windows.Forms;
    using System.Windows.Threading;

    using Hourglass.Extensions;
    using Hourglass.Properties;
    using Hourglass.Timing;

    using Application = System.Windows.Application;

    /// <summary>
    /// Displays an icon for the app in the notification area of the taskbar.
    /// </summary>
    public class NotificationAreaIcon : IDisposable
    {
        /// <summary>
        /// The timeout in milliseconds for the balloon tip that is showed when a timer has expired.
        /// </summary>
        private const int TimerExpiredBalloonTipTimeout = 10000;

        /// <summary>
        /// A <see cref="NotifyIcon"/>.
        /// </summary>
        private readonly NotifyIcon notifyIcon;

        /// <summary>
        /// A <see cref="DispatcherTimer"/> used to raise events.
        /// </summary>
        private readonly DispatcherTimer dispatcherTimer;

        /// <summary>
        /// Indicates whether this object has been disposed.
        /// </summary>
        private bool disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationAreaIcon"/> class.
        /// </summary>
        public NotificationAreaIcon()
        {
            this.notifyIcon = new NotifyIcon();
            this.notifyIcon.Icon = new Icon(Resources.TrayIcon, SystemInformation.SmallIconSize);
            this.notifyIcon.MouseDown += this.NotifyIconMouseDown;
            this.notifyIcon.MouseMove += this.NotifyIconMouseMove;

            this.notifyIcon.BalloonTipClicked += this.BalloonTipClicked;

            this.notifyIcon.ContextMenu = new System.Windows.Forms.ContextMenu();
            this.notifyIcon.ContextMenu.Popup += this.ContextMenuPopup;
            this.notifyIcon.ContextMenu.Collapse += this.ContextMenuCollapse;

            this.dispatcherTimer = new DispatcherTimer(DispatcherPriority.Normal);
            this.dispatcherTimer.Interval = TimeSpan.FromSeconds(1);
            this.dispatcherTimer.Tick += this.DispatcherTimerTick;

            Settings.Default.PropertyChanged += this.SettingsPropertyChanged;
            this.IsVisible = Settings.Default.ShowInNotificationArea;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the icon is visible in the notification area of the taskbar.
        /// </summary>
        public bool IsVisible
        {
            get { return this.notifyIcon.Visible; }
            set { this.notifyIcon.Visible = value; }
        }

        /// <summary>
        /// Displays a balloon tip notifying that a timer has expired.
        /// </summary>
        public void ShowBalloonTipForExpiredTimer()
        {
            this.notifyIcon.ShowBalloonTip(
                TimerExpiredBalloonTipTimeout,
                Resources.NotificationAreaIconTimerExpired,
                Resources.NotificationAreaIconYourTimerHasExpired,
                ToolTipIcon.Info);
        }

        /// <summary>
        /// Disposes the <see cref="NotificationAreaIcon"/>.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true /* disposing */);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes the <see cref="NotificationAreaIcon"/>.
        /// </summary>
        /// <param name="disposing">A value indicating whether this method was invoked by an explicit call to <see
        /// cref="Dispose"/>.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (this.disposed)
            {
                return;
            }

            this.disposed = true;

            if (disposing)
            {
                this.dispatcherTimer.Stop();
                this.notifyIcon.Dispose();

                Settings.Default.PropertyChanged -= this.SettingsPropertyChanged;
            }
        }

        /// <summary>
        /// Throws a <see cref="ObjectDisposedException"/> if the object has been disposed.
        /// </summary>
        protected void ThrowIfDisposed()
        {
            if (this.disposed)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }
        }

        /// <summary>
        /// Restores all <see cref="TimerWindow"/>s.
        /// </summary>
        private void RestoreAllTimerWindows()
        {
            if (Application.Current != null)
            {
                foreach (TimerWindow window in Application.Current.Windows.OfType<TimerWindow>())
                {
                    window.BringToFrontAndActivate();
                }
            }
        }

        /// <summary>
        /// Restores all <see cref="TimerWindow"/>s that show expired timers.
        /// </summary>
        private void RestoreAllExpiredTimerWindows()
        {
            if (Application.Current != null)
            {
                foreach (TimerWindow window in Application.Current.Windows.OfType<TimerWindow>().Where(w => w.Timer.State == TimerState.Expired))
                {
                    window.BringToFrontAndActivate();
                }
            }
        }

        /// <summary>
        /// Invoked after the value of an application settings property is changed.
        /// </summary>
        /// <param name="sender">The settings object.</param>
        /// <param name="e">The event data.</param>
        private void SettingsPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (this.IsVisible != Settings.Default.ShowInNotificationArea)
            {
                if (Settings.Default.ShowInNotificationArea)
                {
                    this.IsVisible = true;
                }
                else
                {
                    this.IsVisible = false;
                    this.RestoreAllTimerWindows();
                }
            }
        }

        /// <summary>
        /// Invoked when the user presses the mouse button while the pointer is over the icon in the notification area
        /// of the taskbar.
        /// </summary>
        /// <param name="sender">The <see cref="NotifyIcon"/>.</param>
        /// <param name="e">The event data.</param>
        private void NotifyIconMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.RestoreAllTimerWindows();
            }
        }

        /// <summary>
        /// Invoked when the user moves the mouse while the pointer is over the icon in the notification area of the
        /// taskbar.
        /// </summary>
        /// <param name="sender">The <see cref="NotifyIcon"/>.</param>
        /// <param name="e">The event data.</param>
        private void NotifyIconMouseMove(object sender, MouseEventArgs e)
        {
            if (Application.Current != null)
            {
                StringBuilder builder = new StringBuilder();

                foreach (TimerWindow window in Application.Current.Windows.OfType<TimerWindow>().Where(window => window.Timer.State != TimerState.Stopped))
                {
                    string windowString = builder.Length == 0
                        ? window.ToString()
                        : Environment.NewLine + window.ToString();

                    if (builder.Length + windowString.Length < 64)
                    {
                        builder.Append(windowString);
                    }
                }

                this.notifyIcon.Text = builder.ToString();
            }
        }

        /// <summary>
        /// Invoked when the balloon tip is clicked.
        /// </summary>
        /// <param name="sender">The <see cref="NotifyIcon"/>.</param>
        /// <param name="e">The event data.</param>
        private void BalloonTipClicked(object sender, EventArgs e)
        {
            this.RestoreAllExpiredTimerWindows();
        }

        /// <summary>
        /// Invoked before the notify icon context menu is displayed.
        /// </summary>
        /// <param name="sender">The notify icon context menu.</param>
        /// <param name="e">The event data.</param>
        private void ContextMenuPopup(object sender, EventArgs e)
        {
            this.notifyIcon.ContextMenu.MenuItems.Clear();

            MenuItem newTimerMenuItem = new MenuItem(Resources.NotificationAreaIconNewTimerMenuItem);
            newTimerMenuItem.Click += this.NewTimerMenuItemClick;
            this.notifyIcon.ContextMenu.MenuItems.Add(newTimerMenuItem);

            this.notifyIcon.ContextMenu.MenuItems.Add("-" /* separator */);

            foreach (TimerWindow window in Application.Current.Windows.OfType<TimerWindow>())
            {
                MenuItem windowMenuItem = new MenuItem(window.ToString());
                windowMenuItem.Tag = window;
                windowMenuItem.Click += this.WindowMenuItemClick;
                this.notifyIcon.ContextMenu.MenuItems.Add(windowMenuItem);
            }

            this.notifyIcon.ContextMenu.MenuItems.Add("-" /* separator */);

            MenuItem exitMenuItem = new MenuItem(Resources.NotificationAreaIconExitMenuItem);
            exitMenuItem.Click += this.ExitMenuItemClick;
            this.notifyIcon.ContextMenu.MenuItems.Add(exitMenuItem);

            this.dispatcherTimer.Start();
        }

        /// <summary>
        /// Invoked when the <see cref="dispatcherTimer"/> interval has elapsed.
        /// </summary>
        /// <param name="sender">The <see cref="DispatcherTimer"/>.</param>
        /// <param name="e">The event data.</param>
        private void DispatcherTimerTick(object sender, EventArgs e)
        {
            foreach (MenuItem menuItem in this.notifyIcon.ContextMenu.MenuItems)
            {
                TimerWindow window = menuItem.Tag as TimerWindow;
                if (window != null)
                {
                    if (!window.Timer.Disposed)
                    {
                        window.Timer.Update();
                    }

                    menuItem.Text = window.ToString();
                }
            }
        }

        /// <summary>
        /// Invoked when the shortcut menu collapses.
        /// </summary>
        /// <remarks>
        /// The Microsoft .NET Framework does not call this method consistently.
        /// </remarks>
        /// <param name="sender">The notify icon context menu.</param>
        /// <param name="e">The event data.</param>
        private void ContextMenuCollapse(object sender, EventArgs e)
        {
            this.dispatcherTimer.Stop();
        }

        /// <summary>
        /// Invoked when the "New timer" <see cref="MenuItem"/> is clicked.
        /// </summary>
        /// <param name="sender">The <see cref="MenuItem"/> where the event handler is attached.</param>
        /// <param name="e">The event data.</param>
        private void NewTimerMenuItemClick(object sender, EventArgs e)
        {
            TimerWindow window = new TimerWindow();
            window.RestoreFromSibling();
            window.Show();
        }

        /// <summary>
        /// Invoked when a <see cref="MenuItem"/> for a <see cref="TimerWindow"/> is clicked.
        /// </summary>
        /// <param name="sender">The <see cref="MenuItem"/> where the event handler is attached.</param>
        /// <param name="e">The event data.</param>
        private void WindowMenuItemClick(object sender, EventArgs e)
        {
            MenuItem windowMenuItem = (MenuItem)sender;
            TimerWindow window = (TimerWindow)windowMenuItem.Tag;
            window.BringToFrontAndActivate();
        }

        /// <summary>
        /// Invoked when the "Exit" <see cref="MenuItem"/> is clicked.
        /// </summary>
        /// <param name="sender">The <see cref="MenuItem"/> where the event handler is attached.</param>
        /// <param name="e">The event data.</param>
        private void ExitMenuItemClick(object sender, EventArgs e)
        {
            foreach (Window window in Application.Current.Windows)
            {
                window.Close();
            }
        }
    }
}
