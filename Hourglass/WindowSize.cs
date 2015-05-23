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
        /// Initializes a new instance of the <see cref="WindowSize"/> class.
        /// </summary>
        public WindowSize()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WindowSize"/> class.
        /// </summary>
        /// <param name="restoreBounds">The size and location of the window before being either minimized or maximized.
        /// </param>
        /// <param name="windowState">A value that indicates whether the window is restored, minimized, or maximized.
        /// </param>
        /// <param name="restoreWindowState">The window's <see cref="Window.WindowState"/> before the window was
        /// minimized.</param>
        /// <param name="isFullScreen">A value indicating whether the window is in full-screen mode.</param>
        public WindowSize(Rect? restoreBounds, WindowState? windowState, WindowState? restoreWindowState, bool? isFullScreen)
        {
            this.RestoreBounds = restoreBounds;
            this.WindowState = windowState;
            this.RestoreWindowState = restoreWindowState;
            this.IsFullScreen = isFullScreen;
        }

        /// <summary>
        /// Gets or sets the size and location of the window before being either minimized or maximized.
        /// </summary>
        public Rect? RestoreBounds { get; set; }

        /// <summary>
        /// Gets or sets a value that indicates whether the window is restored, minimized, or maximized.
        /// </summary>
        public WindowState? WindowState { get; set; }

        /// <summary>
        /// Gets or sets the window's <see cref="Window.WindowState"/> before the window was minimized.
        /// </summary>
        public WindowState? RestoreWindowState { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the window is in full-screen mode.
        /// </summary>
        public bool? IsFullScreen { get; set; }

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

            return new WindowSize(
                windowSize.RestoreBounds,
                windowSize.WindowState,
                windowSize.RestoreWindowState,
                windowSize.IsFullScreen);
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

            return new WindowSize(
                window.RestoreBounds,
                window.WindowState,
                window.RestoreWindowState,
                window.IsFullScreen);
        }

        /// <summary>
        /// Combines the properties of <paramref name="baseWindowSize"/> with <paramref name="windowSize"/>, where the
        /// properties of <paramref name="windowSize"/> take precedence.
        /// </summary>
        /// <param name="baseWindowSize">The base <see cref="WindowSize"/>.</param>
        /// <param name="windowSize">A <see cref="WindowSize"/>.</param>
        /// <returns>A <see cref="WindowSize"/> with the properties of <paramref name="baseWindowSize"/> and <paramref
        /// name="windowSize"/>.</returns>
        public static WindowSize Merge(WindowSize baseWindowSize, WindowSize windowSize)
        {
            WindowSize result = WindowSize.FromWindowSize(baseWindowSize ?? windowSize);

            if (baseWindowSize == null || windowSize == null)
            {
                return result;
            }

            result.RestoreBounds = windowSize.RestoreBounds ?? result.RestoreBounds;
            result.WindowState = windowSize.WindowState ?? result.WindowState;
            result.RestoreWindowState = windowSize.RestoreWindowState ?? result.RestoreWindowState;
            result.IsFullScreen = windowSize.IsFullScreen ?? result.IsFullScreen;

            return result;
        }
    }
}
