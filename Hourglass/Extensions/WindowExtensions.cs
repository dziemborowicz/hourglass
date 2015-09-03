// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WindowExtensions.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass.Extensions
{
    using System;
    using System.Windows;

    using Hourglass.Windows;

    /// <summary>
    /// Specifies options for restoring a <see cref="WindowSize"/> to a <see cref="Window"/>.
    /// </summary>
    [Flags]
    public enum RestoreOptions
    {
        /// <summary>
        /// Specifies that no options are set.
        /// </summary>
        None,

        /// <summary>
        /// Allows restoring a window to a minimized state.
        /// </summary>
        AllowMinimized
    }

    /// <summary>
    /// Provides extensions methods for the <see cref="Window"/> class, and helper methods for manipulating the size,
    /// position, and state of windows through the <see cref="WindowSize"/> class.
    /// </summary>
    public static class WindowExtensions
    {
        #region Public Methods

        /// <summary>
        /// Returns a new <see cref="Rect"/> with the merged properties of <paramref name="rect"/> and <paramref
        /// name="otherRect"/>. Each property in the returned <see cref="Rect"/> is set from <paramref name="otherRect"/>,
        /// except where that property is <see cref="double.PositiveInfinity"/>, <see cref="double.NegativeInfinity"/>,
        /// or <see cref="double.NaN"/>, in which case the property is set from <paramref name="rect"/>.
        /// </summary>
        /// <param name="rect">A <see cref="Rect"/>.</param>
        /// <param name="otherRect">Another <see cref="Rect"/>.</param>
        /// <returns>A new <see cref="Rect"/> with the merged properties of <paramref name="rect"/> and <paramref
        /// name="otherRect"/>.</returns>
        public static Rect Merge(this Rect rect, Rect otherRect)
        {
            return new Rect(
                IsValidDouble(otherRect.X) ? otherRect.X : rect.X,
                IsValidDouble(otherRect.Y) ? otherRect.Y : rect.Y,
                IsValidDouble(otherRect.Width) ? otherRect.Width : rect.Width,
                IsValidDouble(otherRect.Height) ? otherRect.Height : rect.Height);
        }

        /// <summary>
        /// Offsets a <see cref="WindowSize"/> slightly from its current position.
        /// </summary>
        /// <param name="windowSize">A <see cref="WindowSize"/>.</param>
        /// <returns>The offset <see cref="WindowSize"/>.</returns>
        public static WindowSize Offset(this WindowSize windowSize)
        {
            if (windowSize == null)
            {
                return null;
            }

            return new WindowSize(
                windowSize.RestoreBounds.Offset(),
                windowSize.WindowState,
                windowSize.RestoreWindowState,
                windowSize.IsFullScreen);
        }

        /// <summary>
        /// Restores the size, position, and state of a window from its persisted size, position, and state.
        /// </summary>
        /// <typeparam name="T">The type of the window.</typeparam>
        /// <param name="window">A window.</param>
        /// <param name="options">Options for performing the restore. (Optional.)</param>
        public static void RestoreFromSettings<T>(this T window, RestoreOptions options = RestoreOptions.None)
            where T : Window, IRestorableWindow
        {
            WindowSize windowSize = window.PersistedSize;
            window.Restore(windowSize, options);
        }

        /// <summary>
        /// Restores the size, position, and state of a window from another window of the same type.
        /// </summary>
        /// <remarks>
        /// This method offsets the window position slightly so that the two windows do not overlap.
        /// </remarks>
        /// <typeparam name="T">The type of the window.</typeparam>
        /// <param name="window">A window.</param>
        /// <param name="otherWindow">The window from which to copy the size, position, and state.</param>
        /// <param name="options">Options for performing the restore. (Optional.)</param>
        public static void RestoreFromWindow<T>(this T window, T otherWindow, RestoreOptions options = RestoreOptions.None)
            where T : Window, IRestorableWindow
        {
            WindowSize windowSize = WindowSize.FromWindow(otherWindow);
            WindowSize offsetWindowSize = windowSize.Offset();
            window.Restore(offsetWindowSize, options);
        }

        /// <summary>
        /// Restores the size, position, and state of a window from another visible window of the same type, or from
        /// the app settings if there is no other visible window of the same type.
        /// </summary>
        /// <remarks>
        /// This method offsets the window position slightly so that the two windows do not overlap.
        /// </remarks>
        /// <typeparam name="T">The type of the window.</typeparam>
        /// <param name="window">A window.</param>
        /// <param name="options">Options for performing the restore. (Optional.)</param>
        public static void RestoreFromSibling<T>(this T window, RestoreOptions options = RestoreOptions.None)
            where T : Window, IRestorableWindow
        {
            WindowSize windowSize = WindowSize.FromSiblingOfWindow(window);
            if (windowSize != null)
            {
                WindowSize offsetWindowSize = windowSize.Offset();
                window.Restore(offsetWindowSize, options);
            }
            else
            {
                window.RestoreFromSettings(options);
            }
        }

        /// <summary>
        /// Restores the size, position, and state of a window.
        /// </summary>
        /// <typeparam name="T">The type of the window.</typeparam>
        /// <param name="window">A window.</param>
        /// <param name="windowSize">The size, position, and state to restore.</param>
        /// <param name="options">Options for performing the restore. (Optional.)</param>
        public static void Restore<T>(this T window, WindowSize windowSize, RestoreOptions options = RestoreOptions.None)
            where T : Window, IRestorableWindow
        {
            if (window == null || windowSize == null)
            {
                return;
            }

            // Restore size and position
            window.RestoreBounds(windowSize.RestoreBounds);

            // Restore state
            if (windowSize.WindowState == WindowState.Minimized && !options.HasFlag(RestoreOptions.AllowMinimized))
            {
                window.RestoreState(
                    windowSize.RestoreWindowState,
                    windowSize.RestoreWindowState,
                    windowSize.IsFullScreen);
            }
            else
            {
                window.RestoreState(
                    windowSize.WindowState,
                    windowSize.RestoreWindowState,
                    windowSize.IsFullScreen);
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
        /// <param name="restoreBounds">The size and location of the window before being either minimized or maximized.
        /// </param>
        private static void RestoreBounds(this Window window, Rect restoreBounds)
        {
            if (restoreBounds.HasLocation())
            {
                window.Left = restoreBounds.Left;
                window.Top = restoreBounds.Top;
            }

            if (restoreBounds.HasSize())
            {
                window.Width = restoreBounds.Width;
                window.Height = restoreBounds.Height;
            }
        }

        /// <summary>
        /// Restores the state of a window.
        /// </summary>
        /// <typeparam name="T">The type of the window.</typeparam>
        /// <param name="window">A window.</param>
        /// <param name="windowState">A value that indicates whether the window is restored, minimized, or maximized.
        /// </param>
        /// <param name="restoreWindowState">The window's <see cref="Window.WindowState"/> before the window was
        /// minimized.</param>
        /// <param name="isFullScreen">A value indicating whether the window is in full-screen mode.</param>
        /// <seealso cref="RestoreStateToNotVisibleWindow{T}"/>
        private static void RestoreState<T>(this T window, WindowState windowState, WindowState restoreWindowState, bool isFullScreen)
            where T : Window, IRestorableWindow
        {
            if (window.IsVisible)
            {
                RestoreStateToVisibleWindow(
                    window,
                    windowState,
                    restoreWindowState,
                    isFullScreen);
            }
            else
            {
                RestoreStateToNotVisibleWindow(
                    window,
                    windowState,
                    restoreWindowState,
                    isFullScreen);
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
        /// Positions a <see cref="Rect"/> in the center of the screen. If the <see cref="Rect"/> is larger than the
        /// work area, the <see cref="Rect"/> is decreased to fit in the work area.
        /// </summary>
        /// <param name="rect">A <see cref="Rect"/>.</param>
        /// <returns>The centered <see cref="Rect"/>.</returns>
        private static Rect CenterOnScreen(this Rect rect)
        {
            Rect offsetRect = rect;

            if (offsetRect.HasSize())
            {
                offsetRect.Width = Math.Min(rect.Width, SystemParameters.WorkArea.Width);
                offsetRect.Height = Math.Min(rect.Height, SystemParameters.WorkArea.Height);
                offsetRect.X = ((SystemParameters.WorkArea.Width - offsetRect.Width) / 2) + SystemParameters.WorkArea.Left;
                offsetRect.Y = ((SystemParameters.WorkArea.Height - offsetRect.Height) / 2) + SystemParameters.WorkArea.Top;
            }

            return offsetRect;
        }

        /// <summary>
        /// Positions a window in the center of the screen. If the window is larger than the work area, the window size
        /// is decreased to fit in the work area.
        /// </summary>
        /// <param name="window">A window.</param>
        private static void CenterOnScreen(this Window window)
        {
            Rect windowRect = window.GetBoundsForNormalState();
            Rect centeredRect = windowRect.CenterOnScreen();

            window.RestoreBounds(centeredRect);
        }

        /// <summary>
        /// Returns the bounds of a window, or its restore bounds if it is minimized or maximized.
        /// </summary>
        /// <param name="window">A window.</param>
        /// <returns>The bounds of a window, or its restore bounds if it is minimized or maximized.</returns>
        private static Rect GetBoundsForNormalState(this Window window)
        {
            if (window.WindowState != WindowState.Normal && window.RestoreBounds.HasSizeAndLocation())
            {
                return window.RestoreBounds;
            }

            return new Rect(window.Left, window.Top, window.Width, window.Height);
        }

        /// <summary>
        /// Returns a value indicating whether the <see cref="Rect"/> has a valid location.
        /// </summary>
        /// <param name="rect">A <see cref="Rect"/>.</param>
        /// <returns>A value indicating whether the <see cref="Rect"/> has a valid location.</returns>
        private static bool HasLocation(this Rect rect)
        {
            return IsValidDouble(rect.X) && IsValidDouble(rect.Y);
        }

        /// <summary>
        /// Returns a value indicating whether the <see cref="Rect"/> has a valid size.
        /// </summary>
        /// <param name="rect">A <see cref="Rect"/>.</param>
        /// <returns>A value indicating whether the <see cref="Rect"/> has a valid size.</returns>
        private static bool HasSize(this Rect rect)
        {
            return IsValidDouble(rect.Width) && IsValidDouble(rect.Height);
        }

        /// <summary>
        /// Returns a value indicating whether the <see cref="Rect"/> has a valid size and location.
        /// </summary>
        /// <param name="rect">A <see cref="Rect"/>.</param>
        /// <returns>A value indicating whether the <see cref="Rect"/> has a valid size and location.</returns>
        private static bool HasSizeAndLocation(this Rect rect)
        {
            return rect.HasSize() && rect.HasLocation();
        }

        /// <summary>
        /// Returns a value indicating whether the size and position of the <see cref="Rect"/> are such that the <see
        /// cref="Rect"/> is entirely visible on the screen.
        /// </summary>
        /// <param name="rect">A <see cref="Rect"/>.</param>
        /// <returns>A value indicating whether the size and position of the <see cref="Rect"/> are such that the <see
        /// cref="Rect"/> is entirely visible on the screen.</returns>
        private static bool IsOnScreen(this Rect rect)
        {
            Rect virtualScreenRect = new Rect(
                SystemParameters.VirtualScreenLeft,
                SystemParameters.VirtualScreenTop,
                SystemParameters.VirtualScreenWidth,
                SystemParameters.VirtualScreenHeight);

            if (rect.HasSizeAndLocation())
            {
                return virtualScreenRect.Contains(rect);
            }
            else if (rect.HasSize())
            {
                return rect.Width <= virtualScreenRect.Width && rect.Height <= virtualScreenRect.Height;
            }
            else if (rect.HasLocation())
            {
                return virtualScreenRect.Contains(rect.Location);
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Returns a value indicating whether the size and position of the window are such that the window is entirely
        /// visible on the screen when in its normal state.
        /// </summary>
        /// <param name="window">A window.</param>
        /// <returns>A value indicating whether the size and position of the window are such that the window is entirely
        /// visible on the screen when in its normal state.</returns>
        private static bool IsOnScreen(this Window window)
        {
            Rect windowRect = window.GetBoundsForNormalState();
            return windowRect.IsOnScreen();
        }

        /// <summary>
        /// Returns a value indicating whether a <see cref="double"/> is a value other than <see
        /// cref="double.PositiveInfinity"/>, <see cref="double.NegativeInfinity"/>, and <see cref="double.NaN"/>.
        /// </summary>
        /// <param name="value">A <see cref="double"/>.</param>
        /// <returns>A value indicating whether a <see cref="double"/> is a value other than <see
        /// cref="double.PositiveInfinity"/>, <see cref="double.NegativeInfinity"/>, and <see cref="double.NaN"/>.</returns>
        private static bool IsValidDouble(double value)
        {
            return !double.IsInfinity(value) && !double.IsNaN(value);
        }

        /// <summary>
        /// Offsets a <see cref="Rect"/> slightly from its current position.
        /// </summary>
        /// <param name="rect">A <see cref="Rect"/>.</param>
        /// <returns>The offset <see cref="Rect"/>.</returns>
        private static Rect Offset(this Rect rect)
        {
            if (!rect.HasSizeAndLocation())
            {
                return rect;
            }

            const int OffsetAmount = 25;
            Rect offsetRect = rect;

            // Move the rect down and to the right
            offsetRect.X += OffsetAmount;
            offsetRect.Y += OffsetAmount;

            if (offsetRect.IsOnScreen())
            {
                return offsetRect;
            }

            // Move the rect to the top and to the right
            offsetRect.X += OffsetAmount - (Math.Floor((offsetRect.Y - SystemParameters.VirtualScreenTop) / OffsetAmount) * OffsetAmount);
            offsetRect.Y = SystemParameters.VirtualScreenTop;

            if (offsetRect.IsOnScreen())
            {
                return offsetRect;
            }

            // Move the rect to the far top-left
            offsetRect.X = SystemParameters.VirtualScreenLeft;
            offsetRect.Y = SystemParameters.VirtualScreenTop;

            if (offsetRect.IsOnScreen())
            {
                return offsetRect;
            }

            // Center the rect as a fallback
            return rect.CenterOnScreen();
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
