// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AppEntry.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass
{
    using System;
    using System.Collections.Generic;
    using System.Windows;

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
            CommandLineParseResult result;
            if (!TryGetTimerWindowForArgs(e.CommandLine, out window, out result))
            {
                CommandLineArguments.ShowUsage(result.ErrorMessage);
                AppManager.Instance.Dispose();
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
            CommandLineParseResult result;
            if (!TryGetTimerWindowForArgs(e.CommandLine, out window, out result))
            {
                CommandLineArguments.ShowUsage(result.ErrorMessage);
                return;
            }

            window.Show();

            if (window.WindowState != WindowState.Minimized)
            {
                window.BringToFrontAndActivate();
            }
        }

        /// <summary>
        /// Parses command-line arguments and instantiates a new instance of the <see cref="TimerWindow"/> class.
        /// </summary>
        /// <param name="args">The command-line arguments.</param>
        /// <param name="window">The new instance of the <see cref="TimerWindow"/> class.</param>
        /// <param name="result">A <see cref="CommandLineParseResult"/>.</param>
        /// <returns><c>true</c> if a <see cref="TimerWindow"/> was successfully instantiated, or <c>false</c> if the
        /// command-line arguments were invalid.</returns>
        private static bool TryGetTimerWindowForArgs(IList<string> args, out TimerWindow window, out CommandLineParseResult result)
        {
            result = CommandLineArguments.Parse(args);
            if (result.Type != CommandLineParseResultType.Success)
            {
                window = null;
                return false;
            }

            window = GetTimerWindowForArguments(result.Arguments);
            SetGlobalOptionsFromArguments(result.Arguments);
            return true;
        }

        /// <summary>
        /// Returns a new <see cref="TimerWindow"/> from parsed command-line arguments.
        /// </summary>
        /// <param name="arguments">Parsed command-line arguments.</param>
        /// <returns>A new <see cref="TimerWindow"/> from parsed command-line arguments.</returns>
        private static TimerWindow GetTimerWindowForArguments(CommandLineArguments arguments)
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
        private static void SetWindowOptionsFromArguments(TimerWindow window, CommandLineArguments arguments)
        {
            window.Options.Set(arguments.ToTimerOptions());
        }

        /// <summary>
        /// Restores the size, position, and state of a window from parsed command-line arguments.
        /// </summary>
        /// <param name="window">A <see cref="TimerWindow"/>.</param>
        /// <param name="arguments">Parsed command-line arguments.</param>
        private static void RestoreWindowFromArguments(TimerWindow window, CommandLineArguments arguments)
        {
            // Work out window size
            WindowSize windowSizeFromSettings = Settings.Default.WindowSize;
            WindowSize windowSizeFromSibling = WindowSize.FromSiblingOfWindow(window);
            WindowSize windowSizeFromArguments = arguments.ToWindowSize();
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
        private static void SetGlobalOptionsFromArguments(CommandLineArguments arguments)
        {
            Settings.Default.ShowInNotificationArea = arguments.ShowInNotificationArea;
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
