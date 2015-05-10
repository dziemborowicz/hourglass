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

            // Restore size
            window.Left = windowSize.Left;
            window.Top = windowSize.Top;
            window.Width = windowSize.Width;
            window.Height = windowSize.Height;

            // Restore window state (if the window is not loaded yet, setting the state to maximized will maximize the
            // window on the primary display rather than the display where the window was originally maximized)
            WindowState windowState = windowSize.WindowState != WindowState.Minimized ? windowSize.WindowState : windowSize.RestoreWindowState;
            if (windowState != WindowState.Maximized || window.IsVisible)
            {
                window.WindowState = windowState;
            }
            else
            {
                // Do not raise the event twice even if we have previously attached this handler but the window was
                // never laoded
                window.Loaded -= WindowLoaded;

                // Maximize the window when it loads
                window.Loaded += WindowLoaded;
            }

            // If the window is restored to a size or position that does not fit on the scree, fallback to center
            if (!window.IsOnScreen())
            {
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
            window.Restore(options.WindowSize);
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

            Rect windowRect = new Rect(
                window.Left,
                window.Top,
                window.Width,
                window.Height);

            return virtualScreenRect.Contains(windowRect);
        }

        /// <summary>
        /// Positions a <see cref="TimerWindow"/> in the center of the screen and sets its size to a default size.
        /// </summary>
        /// <param name="window">A <see cref="TimerWindow"/>.</param>
        private static void CenterOnScreen(this TimerWindow window)
        {
            window.Width = Math.Min(350, SystemParameters.WorkArea.Width);
            window.Height = Math.Min(150, SystemParameters.WorkArea.Height);
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
            window.Width = Math.Min(window.Width, SystemParameters.WorkArea.Width);
            window.Height = Math.Min(window.Height, SystemParameters.WorkArea.Height);
            window.Left = ((SystemParameters.WorkArea.Width - window.Width) / 2) + SystemParameters.WorkArea.Left;
            window.Top = ((SystemParameters.WorkArea.Height - window.Height) / 2) + SystemParameters.WorkArea.Top;
        }

        /// <summary>
        /// Invoked when a <see cref="TimerWindow"/> that should be restored to a maximized state is laid out,
        /// rendered, and ready for interaction.
        /// </summary>
        /// <param name="sender">The <see cref="TimerWindow"/>.</param>
        /// <param name="e">The event data.</param>
        private static void WindowLoaded(object sender, RoutedEventArgs e)
        {
            TimerWindow window = (TimerWindow)sender;
            window.WindowState = WindowState.Maximized;
            window.Loaded -= WindowLoaded;
        }
    }
}
