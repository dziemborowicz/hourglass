// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WindowSize.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass
{
    using System.Linq;
    using System.Windows;

    /// <summary>
    /// The size, position, and state of a window.
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
        /// Returns a <see cref="WindowSize"/> for the specified window, or <c>null</c> if the specified window is
        /// <c>null</c>.
        /// </summary>
        /// <typeparam name="T">The type of the window.</typeparam>
        /// <param name="window">A window.</param>
        /// <returns>A <see cref="WindowSize"/> for the specified window, or <c>null</c> if the specified window is
        /// <c>null</c>.</returns>
        public static WindowSize FromWindow<T>(T window)
            where T : Window, IRestorableWindow
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
        /// Returns a <see cref="WindowSize"/> for another visible window of the same type, or <c>null</c> if there is
        /// no other visible window of the same type.
        /// </summary>
        /// <typeparam name="T">The type of the window.</typeparam>
        /// <param name="window">A window.</param>
        /// <returns>A <see cref="WindowSize"/> for another visible window of the same type, or <c>null</c> if there is
        /// no other visible window of the same type.</returns>
        public static WindowSize FromSiblingOfWindow<T>(T window)
            where T : Window, IRestorableWindow
        {
            if (Application.Current != null)
            {
                T lastWindow = Application.Current.Windows
                    .OfType<T>()
                    .LastOrDefault(w => !w.Equals(window) && w.IsVisible);

                return WindowSize.FromWindow(lastWindow);
            }

            return null;
        }

        /// <summary>
        /// Returns a <see cref="WindowSize"/> with the merged properties of <paramref name="windowSizes"/>, with the
        /// last <see cref="WindowSize"/>s taking precedence.
        /// </summary>
        /// <remarks>
        /// This method never returns <c>null</c>. If no <paramref name="windowSizes"/> are specified, or all specified
        /// <paramref name="windowSizes"/> are <c>null</c>, this method will return a <see cref="WindowSize"/> with no
        /// properties set.
        /// </remarks>
        /// <param name="windowSizes">An collection of <see cref="WindowSize"/>s.</param>
        /// <returns>A <see cref="WindowSize"/> with the merged properties of <paramref name="windowSizes"/>.</returns>
        public static WindowSize Merge(params WindowSize[] windowSizes)
        {
            WindowSize result = new WindowSize();

            foreach (WindowSize windowSize in windowSizes)
            {
                if (windowSize != null)
                {
                    result.RestoreBounds = windowSize.RestoreBounds ?? result.RestoreBounds;
                    result.WindowState = windowSize.WindowState ?? result.WindowState;
                    result.RestoreWindowState = windowSize.RestoreWindowState ?? result.RestoreWindowState;
                    result.IsFullScreen = windowSize.IsFullScreen ?? result.IsFullScreen;
                }
            }

            return result;
        }
    }
}
