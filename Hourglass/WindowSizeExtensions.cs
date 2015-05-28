// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WindowSizeExtensions.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass
{
    using System;
    using System.Windows;

    /// <summary>
    /// Represents the options that can be specified when restoring a window.
    /// </summary>
    [Flags]
    public enum WindowRestoreOptions
    {
        /// <summary>
        /// Indicates that no additional options should be used when restoring a window.
        /// </summary>
        None,

        /// <summary>
        /// Indicates that the window may be restored to a minimized state.
        /// </summary>
        AllowMinimizedState,

        /// <summary>
        /// Indicates that the window should be offset after it is restored.
        /// </summary>
        Offset
    }

    /// <summary>
    /// A set of extension methods for manipulating the size, position, and state of windows.
    /// </summary>
    public static class WindowSizeExtensions
    {
        #region Public Methods

        /// <summary>
        /// Restores the size, position, and state of a window from its persisted size, position, and state.
        /// </summary>
        /// <typeparam name="T">The type of the window.</typeparam>
        /// <param name="window">A window.</param>
        public static void RestoreFromSettings<T>(this T window)
            where T : Window, IRestorableWindow
        {
            WindowSize windowSize = window.PersistedSize;
            window.Restore(windowSize);
        }

        /// <summary>
        /// Restores the size, position, and state of a window from another window of the same type.
        /// </summary>
        /// <typeparam name="T">The type of the window.</typeparam>
        /// <param name="window">A window.</param>
        /// <param name="otherWindow">The window from which to copy the size, position, and state.</param>
        public static void RestoreFromWindow<T>(this T window, T otherWindow)
            where T : Window, IRestorableWindow
        {
            WindowSize windowSize = WindowSize.FromWindow(otherWindow);
            window.Restore(windowSize, WindowRestoreOptions.Offset);
        }

        /// <summary>
        /// Restores the size, position, and state of a window from another visible window of the same type, or from
        /// the app settings if there is no other visible window of the same type.
        /// </summary>
        /// <typeparam name="T">The type of the window.</typeparam>
        /// <param name="window">A window.</param>
        public static void RestoreFromSibling<T>(this T window)
            where T : Window, IRestorableWindow
        {
            WindowSize windowSize = WindowSize.FromSiblingOfWindow(window);
            if (windowSize != null)
            {
                window.Restore(windowSize, WindowRestoreOptions.Offset);
            }
            else
            {
                window.RestoreFromSettings();
            }
        }

        /// <summary>
        /// Restores the size, position, and state of a window.
        /// </summary>
        /// <typeparam name="T">The type of the window.</typeparam>
        /// <param name="window">A window.</param>
        /// <param name="windowSize">The size, position, and state to restore.</param>
        /// <param name="options">Specifies the options to use when restoring the window. (Optional.)</param>
        public static void Restore<T>(this T window, WindowSize windowSize, WindowRestoreOptions options = WindowRestoreOptions.None)
            where T : Window, IRestorableWindow
        {
            if (window == null || windowSize == null)
            {
                return;
            }

            // Restore size and position
            window.RestoreBounds(windowSize);

            // Restore state
            window.RestoreState(windowSize, options);

            // Offset if required
            if (options.HasFlag(WindowRestoreOptions.Offset))
            {
                window.Offset();
            }

            // If the window is restored to a size or position that does not fit on the screen, fallback to center
            if (!window.IsOnScreen())
            {
                window.CenterOnScreen();
            }

            // If the window still does not fit on the screen, fallback to its default size and state
            if (!window.IsOnScreen())
            {
                window.ResetSizeAndState();
            }
        }

        #endregion

        #region Private Methods (Restore)

        /// <summary>
        /// Restores the size and position of a window.
        /// </summary>
        /// <param name="window">A window.</param>
        /// <param name="windowSize">A <see cref="WindowSize"/> specifying the size and position to restore.</param>
        private static void RestoreBounds(this Window window, WindowSize windowSize)
        {
            if (windowSize.RestoreBounds.HasValue)
            {
                Rect restoreBounds = windowSize.RestoreBounds.Value;

                if (!double.IsInfinity(restoreBounds.Left) && !double.IsInfinity(restoreBounds.Top))
                {
                    window.Left = restoreBounds.Left;
                    window.Top = restoreBounds.Top;
                }

                if (!double.IsInfinity(restoreBounds.Width) && !double.IsInfinity(restoreBounds.Height))
                {
                    window.Width = restoreBounds.Width;
                    window.Height = restoreBounds.Height;
                }
            }
        }

        /// <summary>
        /// Restores the state of a window.
        /// </summary>
        /// <typeparam name="T">The type of the window.</typeparam>
        /// <param name="window">A window.</param>
        /// <param name="windowSize">A <see cref="WindowSize"/> specifying the state to restore.</param>
        /// <param name="options">Specifies the options to use when restoring the window.</param>
        private static void RestoreState<T>(this T window, WindowSize windowSize, WindowRestoreOptions options)
            where T : Window, IRestorableWindow
        {
            if (windowSize.WindowState.HasValue || windowSize.IsFullScreen.HasValue)
            {
                WindowState windowState;
                WindowState restoreWindowState;
                bool isFullScreen;
                GetStateForRestore(window, windowSize, options, out windowState, out restoreWindowState, out isFullScreen);

                if (window.IsVisible)
                {
                    RestoreStateToVisibleWindow(window, windowState, restoreWindowState, isFullScreen);
                }
                else
                {
                    RestoreStateToNotVisibleWindow(window, windowState, restoreWindowState, isFullScreen);
                }
            }
        }

        /// <summary>
        /// Gets the window state to set based on a <see cref="WindowSize"/> and <see cref="WindowRestoreOptions"/>.
        /// </summary>
        /// <typeparam name="T">The type of the window.</typeparam>
        /// <param name="window">A window.</param>
        /// <param name="windowSize">A <see cref="WindowSize"/> specifying the state to restore.</param>
        /// <param name="options">Specifies the options to use when restoring the window.</param>
        /// <param name="windowState">A value that indicates whether the window is restored, minimized, or maximized.
        /// </param>
        /// <param name="restoreWindowState">The window's <see cref="Window.WindowState"/> before the window was
        /// minimized.</param>
        /// <param name="isFullScreen">A value indicating whether the window is in full-screen mode.</param>
        private static void GetStateForRestore<T>(this T window, WindowSize windowSize, WindowRestoreOptions options, out WindowState windowState, out WindowState restoreWindowState, out bool isFullScreen)
            where T : Window, IRestorableWindow
        {
            // Get values from WindowSize
            windowState = windowSize.WindowState ?? window.WindowState;
            restoreWindowState = windowSize.RestoreWindowState ?? windowState;
            isFullScreen = windowSize.IsFullScreen ?? false;

            // The restore state should never be minimized
            if (restoreWindowState == WindowState.Minimized)
            {
                restoreWindowState = WindowState.Normal;
            }

            // Do not restore to a minimized state unless it is explicitly allowed
            if (windowState == WindowState.Minimized && !options.HasFlag(WindowRestoreOptions.AllowMinimizedState))
            {
                windowState = restoreWindowState;
            }
        }

        /// <summary>
        /// Restores the state of a window that is visible.
        /// </summary>
        /// <remarks>
        /// Setting the state to maximized or full-screen before the has loaded will maximize or full-screen the window
        /// on the primary display rather than the display where the window was originally maximized or full-screened,
        /// so we need to handle those cases differently.
        /// </remarks>
        /// <typeparam name="T">The type of the window.</typeparam>
        /// <param name="window">A window.</param>
        /// <param name="windowState">A value that indicates whether the window is restored, minimized, or maximized.
        /// </param>
        /// <param name="restoreWindowState">The window's <see cref="Window.WindowState"/> before the window was
        /// minimized.</param>
        /// <param name="isFullScreen">A value indicating whether the window is in full-screen mode.</param>
        /// <seealso cref="RestoreStateToNotVisibleWindow{T}"/>
        private static void RestoreStateToVisibleWindow<T>(T window, WindowState windowState, WindowState restoreWindowState, bool isFullScreen)
            where T : Window, IRestorableWindow
        {
            if (isFullScreen)
            {
                window.IsFullScreen = true;
                window.RestoreWindowState = restoreWindowState;
            }
            else
            {
                window.IsFullScreen = false;
                window.WindowState = windowState;
                window.RestoreWindowState = restoreWindowState;
            }
        }

        /// <summary>
        /// Restores the state of a window that is not visible.
        /// </summary>
        /// <remarks>
        /// Setting the state to maximized or full-screen before the has loaded will maximize or full-screen the window
        /// on the primary display rather than the display where the window was originally maximized or full-screened,
        /// so we need to handle those cases differently.
        /// </remarks>
        /// <typeparam name="T">The type of the window.</typeparam>
        /// <param name="window">A window.</param>
        /// <param name="windowState">A value that indicates whether the window is restored, minimized, or maximized.
        /// </param>
        /// <param name="restoreWindowState">The window's <see cref="Window.WindowState"/> before the window was
        /// minimized.</param>
        /// <param name="isFullScreen">A value indicating whether the window is in full-screen mode.</param>
        /// <seealso cref="RestoreStateToVisibleWindow{T}"/>
        private static void RestoreStateToNotVisibleWindow<T>(T window, WindowState windowState, WindowState restoreWindowState, bool isFullScreen)
            where T : Window, IRestorableWindow
        {
            // Remove old handlers (if any)
            window.Loaded -= FullScreenSenderWindow;
            window.Loaded -= MaximizeSenderWindow;

            if (isFullScreen)
            {
                // Handle this on window load
                window.Loaded += FullScreenSenderWindow;
                window.RestoreWindowState = restoreWindowState;
            }
            else if (windowState == WindowState.Maximized)
            {
                // Handle this on window load
                window.Loaded += MaximizeSenderWindow;
                window.RestoreWindowState = restoreWindowState;
            }
            else
            {
                // Handle this now
                window.IsFullScreen = false;
                window.WindowState = windowState;
                window.RestoreWindowState = restoreWindowState;
            }
        }

        /// <summary>
        /// Invoked when a window is laid out, rendered, and ready for interaction in order to restore it to a
        /// full-screen state.
        /// </summary>
        /// <param name="sender">A window.</param>
        /// <param name="e">The event data.</param>
        private static void FullScreenSenderWindow(object sender, RoutedEventArgs e)
        {
            Window window = (Window)sender;
            IRestorableWindow restorableWindow = (IRestorableWindow)sender;

            restorableWindow.IsFullScreen = true;
            window.Loaded -= FullScreenSenderWindow;
        }

        /// <summary>
        /// Invoked when a window is laid out, rendered, and ready for interaction in order to restore it to a
        /// maximized state.
        /// </summary>
        /// <param name="sender">A window.</param>
        /// <param name="e">The event data.</param>
        private static void MaximizeSenderWindow(object sender, RoutedEventArgs e)
        {
            Window window = (Window)sender;
            IRestorableWindow restorableWindow = (IRestorableWindow)sender;

            restorableWindow.IsFullScreen = false;
            window.WindowState = WindowState.Maximized;
            window.Loaded -= MaximizeSenderWindow;
        }

        #endregion

        #region Private Methods (Size Manipulation)

        /// <summary>
        /// Positions a window in the center of the screen. If the window is larger than the work area, the window size
        /// is decreased to fit in the work area.
        /// </summary>
        /// <param name="window">A window.</param>
        private static void CenterOnScreen(this Window window)
        {
            if (window.WindowState == WindowState.Normal)
            {
                window.Width = Math.Min(window.Width, SystemParameters.WorkArea.Width);
                window.Height = Math.Min(window.Height, SystemParameters.WorkArea.Height);
                window.Left = ((SystemParameters.WorkArea.Width - window.Width) / 2) + SystemParameters.WorkArea.Left;
                window.Top = ((SystemParameters.WorkArea.Height - window.Height) / 2) + SystemParameters.WorkArea.Top;
            }
        }

        /// <summary>
        /// Returns the bounds of a window, or its restore bounds if it is minimized or maximized.
        /// </summary>
        /// <param name="window">A window.</param>
        /// <returns>The bounds of a window, or its restore bounds if it is minimized or maximized.</returns>
        private static Rect GetBoundsForNormalState(this Window window)
        {
            if (window.WindowState != WindowState.Normal && window.RestoreBounds != Rect.Empty)
            {
                return window.RestoreBounds;
            }

            return new Rect(window.Left, window.Top, window.Width, window.Height);
        }

        /// <summary>
        /// Returns a value indicating whether the window's size and position are such that the window is entirely
        /// visible on the screen when in its normal state.
        /// </summary>
        /// <param name="window">A window.</param>
        /// <returns>A value indicating whether the window's size and position are such that the window is entirely
        /// visible on the screen when in its normal state.</returns>
        private static bool IsOnScreen(this Window window)
        {
            Rect virtualScreenRect = new Rect(
                SystemParameters.VirtualScreenLeft,
                SystemParameters.VirtualScreenTop,
                SystemParameters.VirtualScreenWidth,
                SystemParameters.VirtualScreenHeight);

            Rect windowRect = window.GetBoundsForNormalState();

            return virtualScreenRect.Contains(windowRect);
        }

        /// <summary>
        /// Offsets a window slightly from its current position.
        /// </summary>
        /// <param name="window">A window.</param>
        private static void Offset(this Window window)
        {
            if (window.WindowState == WindowState.Normal)
            {
                // Move the window down and to the right
                window.Left += 25;
                window.Top += 25;
                if (window.IsOnScreen())
                {
                    return;
                }

                // Move the window to the top and to the right
                window.Left += 25 - (Math.Floor((window.Top - SystemParameters.VirtualScreenTop) / 25) * 25);
                window.Top = SystemParameters.VirtualScreenTop;
                if (window.IsOnScreen())
                {
                    return;
                }

                // Move the window to the far top-left
                window.Left = SystemParameters.VirtualScreenLeft;
                window.Top = SystemParameters.VirtualScreenTop;
                if (window.IsOnScreen())
                {
                    return;
                }

                // Center the window as a fallback
                window.CenterOnScreen();
            }
        }

        /// <summary>
        /// Resizes a window to its default size (or the <see cref="SystemParameters.WorkArea"/> if it is smaller than
        /// its default size) and state.
        /// </summary>
        /// <typeparam name="T">The type of the window.</typeparam>
        /// <param name="window">A window.</param>
        private static void ResetSizeAndState<T>(this T window)
            where T : Window, IRestorableWindow
        {
            // Reset state
            window.IsFullScreen = false;
            window.WindowState = WindowState.Normal;
            window.RestoreWindowState = WindowState.Normal;

            // Reset size
            window.Width = Math.Min(window.DefaultSize.Width, SystemParameters.WorkArea.Width);
            window.Height = Math.Min(window.DefaultSize.Height, SystemParameters.WorkArea.Height);

            // Center
            window.CenterOnScreen();
        }

        #endregion
    }
}
