// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimerNotifyIcon.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass
{
    using System;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Forms;
    using System.Windows.Threading;

    using Hourglass.Properties;

    using Application = System.Windows.Application;

    /// <summary>
    /// Displays an icon for the app in the notification area of the taskbar.
    /// </summary>
    public class TimerNotifyIcon : IDisposable
    {
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
        /// Initializes a new instance of the <see cref="TimerNotifyIcon"/> class.
        /// </summary>
        public TimerNotifyIcon()
        {
            this.notifyIcon = new NotifyIcon();
            this.notifyIcon.Icon = Resources.AppIcon;
            this.notifyIcon.MouseDown += this.NotifyIconMouseDown;

            this.notifyIcon.ContextMenu = new ContextMenu();
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
        /// Disposes the <see cref="TimerNotifyIcon"/>.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true /* disposing */);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes the <see cref="TimerNotifyIcon"/>.
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
        /// Invoked after the value of an application settings property is changed.
        /// </summary>
        /// <param name="sender">The settings object.</param>
        /// <param name="e">The event data.</param>
        private void SettingsPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.IsVisible = Settings.Default.ShowInNotificationArea;
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
                foreach (TimerWindow window in Application.Current.Windows.OfType<TimerWindow>())
                {
                    window.BringToFrontAndActivate();
                }
            }
        }

        /// <summary>
        /// Invoked before the notify icon context menu is displayed.
        /// </summary>
        /// <param name="sender">The notify icon context menu.</param>
        /// <param name="e">The event data.</param>
        private void ContextMenuPopup(object sender, EventArgs e)
        {
            this.notifyIcon.ContextMenu.MenuItems.Clear();

            MenuItem newTimerMenuItem = new MenuItem("New timer");
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

            MenuItem exitMenuItem = new MenuItem("Exit");
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
                    window.Timer.Update();
                    menuItem.Text = window.ToString();
                }
            }
        }

        /// <summary>
        /// Invoked when the shortcut menu collapses.
        /// </summary>
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
            window.RestoreFromRecentWindow();
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
