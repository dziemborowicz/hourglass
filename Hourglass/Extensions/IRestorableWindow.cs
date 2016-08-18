// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IRestorableWindow.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass.Extensions
{
    using System.Windows;

    using Hourglass.Windows;

    /// <summary>
    /// An interface for windows whose size, position, and state of windows can be saved and restored.
    /// </summary>
    public interface IRestorableWindow
    {
        /// <summary>
        /// Gets the <see cref="WindowSize"/> for the window persisted in the settings.
        /// </summary>
        WindowSize PersistedSize { get; }

        /// <summary>
        /// Gets or sets the <see cref="Window.WindowState"/> before the window was minimized.
        /// </summary>
        WindowState RestoreWindowState { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the window is in full-screen mode.
        /// </summary>
        bool IsFullScreen { get; set; }
    }
}
