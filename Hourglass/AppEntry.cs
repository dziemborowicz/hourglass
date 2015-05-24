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
            AppManager.Instance.Initialize();

            TimerWindow window;
            if (!TryGetTimerWindowForArgs(e.CommandLine.ToArray(), out window))
            {
                CommandLine.ShowUsage();
                return false;
            }

            this.app = new App();
            this.app.Exit += AppExit;
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
            if (!TryGetTimerWindowForArgs(e.CommandLine.ToArray(), out window))
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
        private static bool TryGetTimerWindowForArgs(string[] args, out TimerWindow window)
        {
            CommandLine arguments;
            if (!CommandLine.TryParse(args, out arguments))
            {
                window = null;
                return false;
            }
            
            window = GetTimerWindowForArguments(arguments);
            SetGlobalOptionsFromArguments(arguments);
            return true;
        }

        /// <summary>
        /// Returns a new <see cref="TimerWindow"/> from parsed command-line arguments.
        /// </summary>
        /// <param name="arguments">Parsed command-line arguments.</param>
        /// <returns>A new <see cref="TimerWindow"/> from parsed command-line arguments.</returns>
        private static TimerWindow GetTimerWindowForArguments(CommandLine arguments)
        {
            TimerWindow window = new TimerWindow(arguments.Input);
            SetWindowOptionsFromArguments(window, arguments);
            RestoreWindowFromArguments(window, arguments);
            return window;
        }

        /// <summary>
        /// Sets the <see cref="TimerOptions"/> for a <see cref="TimerWindow"/> from parsed command-line arguments.
        /// </summary>
        /// <param name="window">A <see cref="TimerWindow"/>.</param>
        /// <param name="arguments">Parsed command-line arguments.</param>
        private static void SetWindowOptionsFromArguments(TimerWindow window, CommandLine arguments)
        {
            if (arguments.Options != null)
            {
                window.Options.Set(arguments.Options);
            }
        }

        /// <summary>
        /// Restores the size, position, and state of a window from parsed command-line arguments.
        /// </summary>
        /// <param name="window">A <see cref="TimerWindow"/>.</param>
        /// <param name="arguments">Parsed command-line arguments.</param>
        private static void RestoreWindowFromArguments(TimerWindow window, CommandLine arguments)
        {
            // If no window size is specified in the options, restore from a sibling
            if (arguments.Options == null || arguments.Options.WindowSize == null)
            {
                window.RestoreFromSibling();
                return;
            }

            // Work out window size
            WindowSize windowSizeFromSettings = Settings.Default.WindowSize;
            WindowSize windowSizeFromSibling = WindowSize.FromSiblingOfWindow(window);
            WindowSize windowSizeFromArguments = arguments.Options.WindowSize;
            WindowSize windowSize = WindowSize.Merge(
                windowSizeFromSettings,
                windowSizeFromSibling,
                windowSizeFromArguments);

            // Work out restore options
            WindowRestoreOptions restoreOptions = WindowRestoreOptions.None;

            if (windowSizeFromArguments.WindowState.HasValue)
            {
                restoreOptions |= WindowRestoreOptions.AllowMinimizedState;
            }

            if (!windowSizeFromArguments.RestoreBounds.HasValue && windowSizeFromSibling != null && windowSizeFromSibling.RestoreBounds.HasValue)
            {
                restoreOptions |= WindowRestoreOptions.Offset;
            }

            // Restore
            window.Restore(windowSize, restoreOptions);
        }

        /// <summary>
        /// Sets global options from parsed command-line arguments.
        /// </summary>
        /// <param name="arguments">Parsed command-line arguments.</param>
        private static void SetGlobalOptionsFromArguments(CommandLine arguments)
        {
            if (arguments.ShowInNotificationArea.HasValue)
            {
                Settings.Default.ShowInNotificationArea = arguments.ShowInNotificationArea.Value;
            }
        }

        /// <summary>
        /// Invoked just before the application shuts down, and cannot be canceled.
        /// </summary>
        /// <param name="sender">The application.</param>
        /// <param name="e">The event data.</param>
        private static void AppExit(object sender, ExitEventArgs e)
        {
            AppManager.Instance.Persist();
            AppManager.Instance.Dispose();
        }
    }
}
