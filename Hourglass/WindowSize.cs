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
        /// <param name="position">The position of the window's top-left corner in relation to the desktop.</param>
        /// <param name="size">The size of the window.</param>
        /// <param name="windowState">A value that indicates whether the window is restored, minimized, or maximized.
        /// </param>
        /// <param name="restorePosition">The position of the window's top-left corner in relation to the desktop
        /// before the window was minimized or maximized.</param>
        /// <param name="restoreSize">The size of the window before the window was minimized or maximized.</param>
        /// <param name="restoreWindowState">The window's <see cref="Window.WindowState"/> before the window was
        /// minimized.</param>
        public WindowSize(Point? position, Size? size, WindowState? windowState, Point? restorePosition, Size? restoreSize, WindowState? restoreWindowState)
        {
            this.Position = position;
            this.Size = size;
            this.WindowState = windowState;
            this.RestorePosition = restorePosition;
            this.RestoreSize = restoreSize;
            this.RestoreWindowState = restoreWindowState;
        }

        /// <summary>
        /// Gets or sets the position of the window's top-left corner in relation to the desktop.
        /// </summary>
        public Point? Position { get; set; }

        /// <summary>
        /// Gets or sets the size of the window.
        /// </summary>
        public Size? Size { get; set; }

        /// <summary>
        /// Gets or sets a value that indicates whether the window is restored, minimized, or maximized.
        /// </summary>
        public WindowState? WindowState { get; set; }
        
        /// <summary>
        /// Gets or sets the position of the window's top-left corner in relation to the desktop before the window was
        /// minimized or maximized.
        /// </summary>
        public Point? RestorePosition { get; set; }

        /// <summary>
        /// Gets or sets the size of the window before the window was minimized or maximized.
        /// </summary>
        public Size? RestoreSize { get; set; }

        /// <summary>
        /// Gets or sets the window's <see cref="Window.WindowState"/> before the window was minimized.
        /// </summary>
        public WindowState? RestoreWindowState { get; set; }

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
                windowSize.Position,
                windowSize.Size,
                windowSize.WindowState,
                windowSize.RestorePosition,
                windowSize.RestoreSize,
                windowSize.RestoreWindowState);
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
                new Point(window.Left, window.Top),
                new Size(window.Width, window.Height),
                window.WindowState,
                window.RestoreBounds.TopLeft,
                window.RestoreBounds.Size,
                window.RestoreWindowState);
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

            if (windowSize == null)
            {
                return result;
            }

            result.Position = windowSize.Position ?? result.Position;
            result.Size = windowSize.Size ?? result.Size;
            result.WindowState = windowSize.WindowState ?? result.WindowState;
            result.RestorePosition = windowSize.RestorePosition ?? result.RestorePosition;
            result.RestoreSize = windowSize.RestoreSize ?? result.RestoreSize;
            result.RestoreWindowState = windowSize.RestoreWindowState ?? result.RestoreWindowState;

            return result;
        }
    }
}
