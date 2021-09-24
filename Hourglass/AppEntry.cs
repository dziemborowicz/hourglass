// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AppEntry.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass
{
    using System;
    using System.Linq;
    using System.Windows;

    using Hourglass.Extensions;
    using Hourglass.Managers;
    using Hourglass.Properties;
    using Hourglass.Timing;
    using Hourglass.Windows;

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

            CommandLineArguments arguments = CommandLineArguments.Parse(e.CommandLine);
            if (arguments.ShouldShowUsage || arguments.HasParseError)
            {
                CommandLineArguments.ShowUsage(arguments.ParseErrorMessage);
                AppManager.Instance.Dispose();
                return false;
            }

            SetGlobalSettingsFromArguments(arguments);

            this.app = new App();
            this.app.Startup += (sender, args) =>
            {
                ShowTimerWindowsForArguments(arguments);
            };
            this.app.Exit += AppExit;
            this.app.Run();

            return false;
        }

        /// <summary>
        /// Invoked when a subsequent instance of this application starts.
        /// </summary>
        /// <param name="e">Contains the command-line arguments of the subsequent application instance and indicates
        /// whether the first application instance should be brought to the foreground.</param>
        protected override void OnStartupNextInstance(StartupNextInstanceEventArgs e)
        {
            CommandLineArguments arguments = CommandLineArguments.Parse(e.CommandLine);
            if (arguments.ShouldShowUsage || arguments.HasParseError)
            {
                CommandLineArguments.ShowUsage(arguments.ParseErrorMessage);
                return;
            }

            SetGlobalSettingsFromArguments(arguments);

            ShowTimerWindowsForArguments(arguments);
        }

        /// <summary>
        /// Shows a new timer window or windows for all saved timers, depending on whether the <see
        /// cref="CommandLineArguments"/> specify to open saved timers.
        /// </summary>
        /// <param name="arguments">Parsed command-line arguments.</param>
        private static void ShowTimerWindowsForArguments(CommandLineArguments arguments)
        {
            if (arguments.OpenSavedTimers && TimerManager.Instance.ResumableTimers.Any())
            {
                ShowSavedTimerWindows(arguments);

                if (arguments.TimerStart != null)
                {
                    ShowNewTimerWindow(arguments);
                }
            }
            else
            {
                ShowNewTimerWindow(arguments);
            }
        }

        /// <summary>
        /// Shows a new timer window. The window will run the <see cref="TimerStart"/> specified in the <see
        /// cref="CommandLineArguments"/>, or it will display in input mode if there is no <see cref="TimerStart"/>.
        /// </summary>
        /// <param name="arguments">Parsed command-line arguments.</param>
        private static void ShowNewTimerWindow(CommandLineArguments arguments)
        {
            TimerWindow window = new TimerWindow(arguments.TimerStart);
            window.Options.Set(arguments.GetTimerOptions());
            window.Restore(arguments.GetWindowSize(), RestoreOptions.AllowMinimized);
            window.Show();

            if (window.WindowState != WindowState.Minimized)
            {
                window.BringToFrontAndActivate();
            }
        }

        /// <summary>
        /// Shows windows for all saved timers.
        /// </summary>
        /// <param name="arguments">Parsed command-line arguments.</param>
        private static void ShowSavedTimerWindows(CommandLineArguments arguments)
        {
            foreach (Timer savedTimer in TimerManager.Instance.ResumableTimers)
            {
                TimerWindow window = new TimerWindow();

                if (savedTimer.Options.WindowSize != null)
                {
                    window.Restore(savedTimer.Options.WindowSize, RestoreOptions.AllowMinimized);
                }
                else
                {
                    window.Restore(arguments.GetWindowSize(), RestoreOptions.AllowMinimized);
                }

                window.Show(savedTimer);
            }
        }

        /// <summary>
        /// Sets global options from parsed command-line arguments.
        /// </summary>
        /// <param name="arguments">Parsed command-line arguments.</param>
        private static void SetGlobalSettingsFromArguments(CommandLineArguments arguments)
        {
            Settings.Default.ShowInNotificationArea = arguments.ShowInNotificationArea;
            Settings.Default.OpenSavedTimersOnStartup = arguments.OpenSavedTimers;
            Settings.Default.Prefer24HourTime = arguments.Prefer24HourTime;
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
