// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WindowSize.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass
{
    using System.Windows;

    /// <summary>
    /// The size, position, and state of a <see cref="TimerWindow"/>.
    /// </summary>
    public class WindowSize
    {
        /// <summary>
        /// Gets or sets the position of the window's left edge, in relation to the desktop.
        /// </summary>
        public double Left { get; set; }

        /// <summary>
        /// Gets or sets the position of the window's top edge, in relation to the desktop.
        /// </summary>
        public double Top { get; set; }

        /// <summary>
        /// Gets or sets the width of the window.
        /// </summary>
        public double Width { get; set; }

        /// <summary>
        /// Gets or sets the height of the window.
        /// </summary>
        public double Height { get; set; }

        /// <summary>
        /// Gets or sets a value that indicates whether the window is restored, minimized, or maximized.
        /// </summary>
        public WindowState WindowState { get; set; }

        /// <summary>
        /// Gets or sets the window's <see cref="Window.WindowState"/> before the window was minimized.
        /// </summary>
        public WindowState RestoreWindowState { get; set; }

        /// <summary>
        /// Returns a <see cref="WindowSize"/> for the specified <see cref="WindowSize"/>, or <c>null</c> if the
        /// specified <see cref="WindowSize"/> is <c>null</c>.
        /// </summary>
        /// <param name="windowSize">A <see cref="WindowSize"/>.</param>
        /// <returns>A <see cref="WindowSize"/> for the specified <see cref="WindowSize"/>, or <c>null</c> if the
        /// specified <see cref="WindowSize"/> is <c>null</c>.</returns>
        public static WindowSize FromWindowSize(WindowSize windowSize)
        {
            if (windowSize == null)
            {
                return null;
            }

            return new WindowSize
            {
                Left = windowSize.Left,
                Top = windowSize.Top,
                Width = windowSize.Width,
                Height = windowSize.Height,
                WindowState = windowSize.WindowState,
                RestoreWindowState = windowSize.RestoreWindowState
            };
        }

        /// <summary>
        /// Returns a <see cref="WindowSize"/> for the specified <see cref="TimerWindow"/>, or <c>null</c> if the
        /// specified <see cref="TimerWindow"/> is <c>null</c>.
        /// </summary>
        /// <param name="window">A <see cref="TimerWindow"/>.</param>
        /// <returns>A <see cref="WindowSize"/> for the specified <see cref="TimerWindow"/>, or <c>null</c> if the
        /// specified <see cref="TimerWindow"/> is <c>null</c>.</returns>
        public static WindowSize FromWindow(TimerWindow window)
        {
            if (window == null)
            {
                return null;
            }

            return new WindowSize
            {
                Left = window.Left,
                Top = window.Top,
                Width = window.Width,
                Height = window.Height,
                WindowState = window.WindowState,
                RestoreWindowState = window.RestoreWindowState
            };
        }
    }
}
