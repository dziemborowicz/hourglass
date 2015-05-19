// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AppEntry.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass
{
    using System;
    using System.Linq;

    using Hourglass.Properties;

    using Microsoft.VisualBasic.ApplicationServices;

    using ExitEventArgs = System.Windows.ExitEventArgs;
    using StartupEventArgs = Microsoft.VisualBasic.ApplicationServices.StartupEventArgs;

    /// <summary>
    /// Handles application start up, command-line arguments, and ensures that only one instance of the application is
    /// running at any time.
    /// </summary>
    public class AppEntry : WindowsFormsApplicationBase
    {
        /// <summary>
        /// An instance of the <see cref="App"/> class.
        /// </summary>
        private App app;

        /// <summary>
        /// The icon for the app in the notification area of the taskbar.
        /// </summary>
        private TimerNotifyIcon notifyIcon;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppEntry"/> class.
        /// </summary>
        public AppEntry()
        {
            this.IsSingleInstance = true;
        }

        /// <summary>
        /// The entry point for the application.
        /// </summary>
        /// <param name="args">Command-line arguments.</param>
        [STAThread]
        public static void Main(string[] args)
        {
            AppEntry appEntry = new AppEntry();
            appEntry.Run(args);
        }

        /// <summary>
        /// Invoked when the application starts.
        /// </summary>
        /// <param name="e">Contains the command-line arguments of the application and indicates whether the
        /// application startup should be canceled.</param>
        /// <returns>A value indicating whether the application should continue starting up.</returns>
        protected override bool OnStartup(StartupEventArgs e)
        {
            SettingsManager.Instance.Load();

            this.notifyIcon = new TimerNotifyIcon();

            TimerWindow window;
            if (!this.TryGetTimerWindowForArgs(e.CommandLine.ToArray(), out window))
            {
                CommandLine.ShowUsage();
                return false;
            }

            this.app = new App();
            this.app.Exit += this.AppExit;
            this.app.Run(window);

            return false;
        }

        /// <summary>
        /// Invoked when a subsequent instance of this application starts.
        /// </summary>
        /// <param name="e">Contains the command-line arguments of the subsequent application instance and indicates
        /// whether the first application instance should be brought to the foreground.</param>
        protected override void OnStartupNextInstance(StartupNextInstanceEventArgs e)
        {
            TimerWindow window;
            if (!this.TryGetTimerWindowForArgs(e.CommandLine.ToArray(), out window))
            {
                CommandLine.ShowUsage();
                return;
            }

            window.Show();
            window.BringToFrontAndActivate();
        }

        /// <summary>
        /// Parses command-line arguments and instantiates a new instance of the <see cref="TimerWindow"/> class.
        /// </summary>
        /// <param name="args">The command-line arguments.</param>
        /// <param name="window">The new instance of the <see cref="TimerWindow"/> class.</param>
        /// <returns><c>true</c> if a <see cref="TimerWindow"/> was successfully instantiated, or <c>false</c> if the
        /// command-line arguments were invalid.</returns>
        private bool TryGetTimerWindowForArgs(string[] args, out TimerWindow window)
        {
            // Parse command-line arguments
            CommandLine arguments;
            if (!CommandLine.TryParse(args, out arguments))
            {
                window = null;
                return false;
            }

            // Instantiate window
            window = new TimerWindow(arguments.Input);

            // Set timer-specific options
            if (arguments.Options != null)
            {
                window.Options.Set(arguments.Options);
            }

            // Set global options
            Settings.Default.ShowInNotificationArea = arguments.ShowInNotificationArea ?? Settings.Default.ShowInNotificationArea;

            // Restore window
            window.RestoreFromOptions(arguments.Options);
            return true;
        }

        /// <summary>
        /// Invoked just before the application shuts down, and cannot be canceled.
        /// </summary>
        /// <param name="sender">The application.</param>
        /// <param name="e">The event data.</param>
        private void AppExit(object sender, ExitEventArgs e)
        {
            this.notifyIcon.Dispose();
            SettingsManager.Instance.Save();
        }
    }
}
