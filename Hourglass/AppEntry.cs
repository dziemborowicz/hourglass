// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AppEntry.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass
{
    using System;
    using System.Linq;

    using Microsoft.VisualBasic.ApplicationServices;

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
        /// <param name="eventArgs">Contains the command-line arguments of the application and indicates whether the
        /// application startup should be canceled.</param>
        /// <returns>A value indicating whether the application should continue starting up.</returns>
        protected override bool OnStartup(StartupEventArgs eventArgs)
        {
            this.app = new App();
            this.app.Run(new TimerWindow(eventArgs.CommandLine.ToArray()));
            return false;
        }

        /// <summary>
        /// Invoked when a subsequent instance of this application starts.
        /// </summary>
        /// <param name="eventArgs">Contains the command-line arguments of the subsequent application instance and
        /// indicates whether the first application instance should be brought to the foreground.</param>
        protected override void OnStartupNextInstance(StartupNextInstanceEventArgs eventArgs)
        {
            TimerWindow timerWindow = new TimerWindow(eventArgs.CommandLine.ToArray());
            timerWindow.Show();
        }
    }
}
