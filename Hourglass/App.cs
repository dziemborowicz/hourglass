// --------------------------------------------------------------------------------------------------------------------
// <copyright file="App.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass
{
    using System.Windows;

    using Hourglass.Properties;

    /// <summary>
    /// The application.
    /// </summary>
    public class App : Application
    {
        /// <summary>
        /// Invoked just before the application shuts down, and cannot be canceled.
        /// </summary>
        /// <param name="e">The event data.</param>
        protected override void OnExit(ExitEventArgs e)
        {
            SettingsManager.Instance.Save();

            base.OnExit(e);
        }
    }
}
