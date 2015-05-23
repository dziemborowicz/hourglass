// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WindowSizeExtensions.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass
{
    using System;
    using System.Linq;
    using System.Windows;

    using Hourglass.Properties;

    /// <summary>
    /// A set of extension methods for manipulating the size, position, and state of <see cref="TimerWindow"/>s.
    /// </summary>
    public static class WindowSizeExtensions
    {
        /// <summary>
        /// Restores the size, position, and state of a <see cref="TimerWindow"/>.
        /// </summary>
        /// <param name="window">A <see cref="TimerWindow"/>.</param>
        /// <param name="windowSize">The size, position, and state to restore.</param>
        public static void Restore(this TimerWindow window, WindowSize windowSize)
        {
            if (window == null || windowSize == null)
            {
                return;
            }

            // Restore size and position
            window.RestoreBounds(windowSize);

            // Restore state
            window.RestoreState(windowSize);

            // If the window is restored to a size or position that does not fit on the screen, fallback to center
            if (!window.IsOnScreen())
            {
                window.CenterOnScreen();
            }

            // If the window still does not fit on the screen, fallback to its default size
            if (!window.IsOnScreen())
            {
                window.ResetSize();
                window.CenterOnScreen();
            }
        }

        /// <summary>
        /// Restores the size, position, and state of a <see cref="TimerWindow"/> from the app settings.
        /// </summary>
        /// <param name="window">A <see cref="TimerWindow"/>.</param>
        public static void RestoreFromSettings(this TimerWindow window)
        {
            window.Restore(Settings.Default.WindowSize);
        }

        /// <summary>
        /// Restores the size, position, and state of a <see cref="TimerWindow"/> from <see cref="TimerOptions"/>.
        /// </summary>
        /// <param name="window">A <see cref="TimerWindow"/>.</param>
        /// <param name="options">The <see cref="TimerOptions"/> containing the size, position, and state to restore.
        /// </param>
        public static void RestoreFromOptions(this TimerWindow window, TimerOptions options)
        {
            if (options != null && options.WindowSize != null)
            {
                window.Restore(options.WindowSize);
            }
            else
            {
                window.RestoreFromRecentWindow();
            }
        }

        /// <summary>
        /// Restores the size, position, and state of a <see cref="TimerWindow"/> from another <see
        /// cref="TimerWindow"/>.
        /// </summary>
        /// <param name="window">A <see cref="TimerWindow"/>.</param>
        /// <param name="otherWindow">The <see cref="TimerWindow"/> from which to copy the size, position, and state.
        /// </param>
        public static void RestoreFromWindow(this TimerWindow window, TimerWindow otherWindow)
        {
            WindowSize windowSize = WindowSize.FromWindow(otherWindow);
            window.Restore(windowSize);
            window.Offset();
        }

        /// <summary>
        /// Restores the size, position, and state of a <see cref="TimerWindow"/> from another visible <see
        /// cref="TimerWindow"/>, or from the app settings if there is no other open <see cref="TimerWindow"/>.
        /// </summary>
        /// <param name="window">A <see cref="TimerWindow"/>.</param>
        public static void RestoreFromRecentWindow(this TimerWindow window)
        {
            if (Application.Current == null)
            {
                window.RestoreFromSettings();
                return;
            }

            TimerWindow otherWindow = Application.Current.Windows.OfType<TimerWindow>().FirstOrDefault(w => !w.Equals(window));
            if (otherWindow != null && otherWindow.IsVisible)
            {
                window.RestoreFromWindow(otherWindow);
            }
            else
            {
                window.RestoreFromSettings();
            }
        }

        /// <summary>
        /// Returns a value indicating whether the window's size and position are such that the window is visible on
        /// the screen.
        /// </summary>
        /// <param name="window">A <see cref="TimerWindow"/>.</param>
        /// <returns>A value indicating whether the window's size and position are such that the window is visible on
        /// the screen.</returns>
        private static bool IsOnScreen(this TimerWindow window)
        {
            Rect virtualScreenRect = new Rect(
                SystemParameters.VirtualScreenLeft,
                SystemParameters.VirtualScreenTop,
                SystemParameters.VirtualScreenWidth,
                SystemParameters.VirtualScreenHeight);

            Rect windowRect;
            if ((window.WindowState == WindowState.Minimized || window.WindowState == WindowState.Maximized)
                && window.RestoreBounds != Rect.Empty)
            {
                windowRect = new Rect(
                    window.RestoreBounds.Left,
                    window.RestoreBounds.Top,
                    window.RestoreBounds.Width,
                    window.RestoreBounds.Height);
            }
            else
            {
                windowRect = new Rect(
                    window.Left,
                    window.Top,
                    window.Width,
                    window.Height);
            }

            return virtualScreenRect.Contains(windowRect);
        }

        /// <summary>
        /// Positions a <see cref="TimerWindow"/> in the center of the screen and sets its size to a default size.
        /// </summary>
        /// <param name="window">A <see cref="TimerWindow"/>.</param>
        private static void CenterOnScreen(this TimerWindow window)
        {
            window.Width = Math.Min(window.Width, SystemParameters.WorkArea.Width);
            window.Height = Math.Min(window.Height, SystemParameters.WorkArea.Height);
            window.Left = ((SystemParameters.WorkArea.Width - window.Width) / 2) + SystemParameters.WorkArea.Left;
            window.Top = ((SystemParameters.WorkArea.Height - window.Height) / 2) + SystemParameters.WorkArea.Top;
        }

        /// <summary>
        /// Offsets a <see cref="TimerWindow"/> slightly from its current position.
        /// </summary>
        /// <param name="window">A <see cref="TimerWindow"/>.</param>
        private static void Offset(this TimerWindow window)
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

        /// <summary>
        /// Resizes a <see cref="TimerWindow"/> to its default size, or the <see cref="SystemParameters.WorkArea"/> if
        /// it is smaller than the default size of a <see cref="TimerWindow"/>.
        /// </summary>
        /// <param name="window">A <see cref="TimerWindow"/>.</param>
        private static void ResetSize(this TimerWindow window)
        {
            window.Width = Math.Min(TimerWindow.DefaultSize.Width, SystemParameters.WorkArea.Width);
            window.Height = Math.Min(TimerWindow.DefaultSize.Height, SystemParameters.WorkArea.Height);
        }

        /// <summary>
        /// Restores the size and position of a <see cref="Window"/>.
        /// </summary>
        /// <param name="window">A <see cref="Window"/>.</param>
        /// <param name="windowSize">A <see cref="WindowSize"/> specifying the size and position to restore.</param>
        private static void RestoreBounds(this Window window, WindowSize windowSize)
        {
            if (windowSize.RestoreBounds.HasValue)
            {
                Rect restoreBounds = windowSize.RestoreBounds.Value;
                window.Left = restoreBounds.Left;
                window.Top = restoreBounds.Top;
                window.Width = restoreBounds.Width;
                window.Height = restoreBounds.Height;
            }
        }

        /// <summary>
        /// Restores the state of a <see cref="TimerWindow"/>.
        /// </summary>
        /// <param name="window">A <see cref="TimerWindow"/>.</param>
        /// <param name="windowSize">A <see cref="WindowSize"/> specifying the state to restore.</param>
        private static void RestoreState(this TimerWindow window, WindowSize windowSize)
        {
            if (windowSize.WindowState.HasValue || windowSize.IsFullScreen.HasValue)
            {
                WindowState windowState = windowSize.WindowState ?? window.WindowState;
                WindowState restoreWindowState = windowSize.RestoreWindowState ?? windowState;
                bool isFullScreen = windowSize.IsFullScreen ?? false;

                // The restore state should never be minimized
                if (restoreWindowState == WindowState.Minimized)
                {
                    restoreWindowState = WindowState.Normal;
                }

                // Setting the state to maximized or full-screen before the has loaded will maximize the window on the
                // primary display rather than the display where the window was originally maximized or full-screened
                if (!window.IsVisible)
                {
                    if (isFullScreen)
                    {
                        // Remove old handlers (if any)
                        window.Loaded -= FullScreenSenderWindow;
                        window.Loaded -= MaximizeSenderWindow;

                        // Set the correct state on load
                        window.Loaded += FullScreenSenderWindow;
                        window.RestoreWindowState = restoreWindowState;
                        return;
                    }
                    else if (windowState == WindowState.Maximized)
                    {
                        // Remove old handlers (if any)
                        window.Loaded -= FullScreenSenderWindow;
                        window.Loaded -= MaximizeSenderWindow;

                        // Set the correct state on load
                        window.Loaded += MaximizeSenderWindow;
                        window.RestoreWindowState = restoreWindowState;
                        return;
                    }
                }

                // Restore the state
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
        }

        /// <summary>
        /// Invoked when a <see cref="TimerWindow"/> that should be restored to a full-screen state is laid out,
        /// rendered, and ready for interaction to set the window to full-screen mode.
        /// </summary>
        /// <param name="sender">The <see cref="TimerWindow"/>.</param>
        /// <param name="e">The event data.</param>
        private static void FullScreenSenderWindow(object sender, RoutedEventArgs e)
        {
            TimerWindow window = (TimerWindow)sender;
            window.IsFullScreen = true;
            window.Loaded -= FullScreenSenderWindow;
        }

        /// <summary>
        /// Invoked when a <see cref="TimerWindow"/> that should be restored to a maximized state is laid out,
        /// rendered, and ready for interaction to maximize the window.
        /// </summary>
        /// <param name="sender">The <see cref="TimerWindow"/>.</param>
        /// <param name="e">The event data.</param>
        private static void MaximizeSenderWindow(object sender, RoutedEventArgs e)
        {
            TimerWindow window = (TimerWindow)sender;
            window.WindowState = WindowState.Maximized;
            window.Loaded -= MaximizeSenderWindow;
        }
    }
}
