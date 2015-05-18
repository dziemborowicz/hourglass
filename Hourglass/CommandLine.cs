// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CommandLine.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Windows;

    using Hourglass.Properties;

    /// <summary>
    /// Parsed command-line arguments.
    /// </summary>
    public class CommandLine
    {
        /// <summary>
        /// Gets the command-line usage for this application.
        /// </summary>
        public static string Usage
        {
            get
            {
                string assemblyName = Assembly.GetExecutingAssembly().GetName().Name.ToLowerInvariant();
                return string.Format(Resources.Usage, assemblyName);
            }
        }

        /// <summary>
        /// Gets or sets a <see cref="TimerInput"/>, or <c>null</c> if no <see cref="TimerInput"/> was specified on the
        /// command line.
        /// </summary>
        public TimerInput Input { get; set; }

        /// <summary>
        /// Gets or sets a <see cref="TimerOptions"/>, or <c>null</c> if no <see cref="TimerOptions"/> was specified on
        /// the command line.
        /// </summary>
        public TimerOptions Options { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether an icon for the app should be visible in the notification area of
        /// the taskbar.
        /// </summary>
        public bool? ShowInNotificationArea { get; set; }

        /// <summary>
        /// Shows the command-line usage of this application in the console or in a window.
        /// </summary>
        public static void ShowUsage()
        {
            CommandLineUsageWindow window = new CommandLineUsageWindow();
            window.ShowDialog();
        }

        /// <summary>
        /// Tries to create a new instance of the <see cref="CommandLine"/> class from command-line arguments.
        /// </summary>
        /// <param name="args">The command-line arguments.</param>
        /// <param name="arguments">The new instance of the <see cref="CommandLine"/> class.</param>
        /// <returns><c>true</c> if the <see cref="CommandLine"/> was successfully created, or <c>false</c>
        /// otherwise.</returns>
        public static bool TryParse(string[] args, out CommandLine arguments)
        {
            ParseResult result;
            TimerInput input;
            TimerOptions options;

            if (!TryParse(args, out result) ||
                !TryParseResultToTimerOptions(result, out options) ||
                !TryParseResultToTimerInput(result, options, out input))
            {
                arguments = null;
                return false;
            }

            arguments = new CommandLine();
            arguments.Input = input;
            arguments.Options = options;
            arguments.ShowInNotificationArea = result.ShowInNotificationArea;
            return true;
        }

        /// <summary>
        /// Tries to create a new instance of the <see cref="ParseResult"/> class from command-line arguments.
        /// </summary>
        /// <param name="args">The command-line arguments.</param>
        /// <param name="result">The new instance of the <see cref="ParseResult"/> class.</param>
        /// <returns><c>true</c> if the <see cref="ParseResult"/> was successfully created, or <c>false</c> otherwise.
        /// </returns>
        private static bool TryParse(string[] args, out ParseResult result)
        {
            result = new ParseResult();

            for (int i = 0; i < args.Length; i++)
            {
                switch (args[i])
                {
                    case "+d":
                    case "-d":
                    case "+default-options":
                    case "-default-options":
                        if (result.DefaultOptions.HasValue)
                        {
                            result = null;
                            return false;
                        }

                        result.DefaultOptions = IsPositiveSwitch(args[i]);
                        break;

                    case "+t":
                    case "+title":
                        if (result.Title != null)
                        {
                            result = null;
                            return false;
                        }

                        result.Title = ParseNextValue(args, ref i);

                        if (result.Title == null)
                        {
                            result = null;
                            return false;
                        }

                        break;

                    case "+a":
                    case "-a":
                    case "+always-on-top":
                    case "-always-on-top":
                        if (result.AlwaysOnTop.HasValue)
                        {
                            result = null;
                            return false;
                        }

                        result.AlwaysOnTop = IsPositiveSwitch(args[i]);
                        break;

                    case "+n":
                    case "-n":
                    case "+show-in-notification-area":
                    case "-show-in-notification-area":
                        if (result.ShowInNotificationArea.HasValue)
                        {
                            result = null;
                            return false;
                        }

                        result.ShowInNotificationArea = IsPositiveSwitch(args[i]);
                        break;

                    case "+l":
                    case "-l":
                    case "+loop-timer":
                    case "-loop-timer":
                        if (result.LoopTimer.HasValue)
                        {
                            result = null;
                            return false;
                        }

                        result.LoopTimer = IsPositiveSwitch(args[i]);
                        break;

                    case "+p":
                    case "-p":
                    case "+pop-up-when-expired":
                    case "-pop-up-when-expired":
                        if (result.PopUpWhenExpired.HasValue)
                        {
                            result = null;
                            return false;
                        }

                        result.PopUpWhenExpired = IsPositiveSwitch(args[i]);
                        break;

                    case "+c":
                    case "-c":
                    case "+close-when-expired":
                    case "-close-when-expired":
                        if (result.CloseWhenExpired.HasValue)
                        {
                            result = null;
                            return false;
                        }

                        result.CloseWhenExpired = IsPositiveSwitch(args[i]);
                        break;

                    case "+s":
                    case "-s":
                    case "+sound":
                    case "-sound":
                        if (result.NoSound.HasValue || result.SoundName != null)
                        {
                            result = null;
                            return false;
                        }

                        if (IsPositiveSwitch(args[i]))
                        {
                            result.NoSound = false;
                            result.SoundName = ParseNextValue(args, ref i);
                        }
                        else
                        {
                            result.NoSound = true;
                            result.SoundName = null;
                        }

                        break;

                    case "+r":
                    case "-r":
                    case "+loop-sound":
                    case "-loop-sound":
                        if (result.LoopSound.HasValue)
                        {
                            result = null;
                            return false;
                        }

                        result.LoopSound = IsPositiveSwitch(args[i]);
                        break;

                    case "+w":
                    case "+window-state":
                        if (result.WindowState.HasValue)
                        {
                            result = null;
                            return false;
                        }

                        WindowState windowState;
                        WindowState restoreWindowState;
                        if (!TryParseWindowState(ParseNextValue(args, ref i), out windowState, out restoreWindowState))
                        {
                            result = null;
                            return false;
                        }

                        result.WindowState = windowState;
                        result.RestoreWindowState = restoreWindowState;
                        break;

                    case "+y":
                    case "+window-position":
                        if (result.WindowPosition.HasValue)
                        {
                            result = null;
                            return false;
                        }

                        Point position;
                        if (!TryParseWindowPosition(ParseNextValue(args, ref i), out position))
                        {
                            result = null;
                            return false;
                        }

                        result.WindowPosition = position;
                        break;

                    case "+z":
                    case "+window-size":
                        if (result.WindowSize.HasValue)
                        {
                            result = null;
                            return false;
                        }

                        Size size;
                        if (!TryParseWindowSize(ParseNextValue(args, ref i), out size))
                        {
                            result = null;
                            return false;
                        }

                        result.WindowSize = size;
                        break;

                    default:
                        if (IsPositiveSwitch(args[i]) || IsNegativeSwitch(args[i]) || result.Input != null)
                        {
                            result = null;
                            return false;
                        }

                        result.Input = args[i];
                        break;
                }
            }

            return true;
        }

        /// <summary>
        /// Tries to create a new instance of the <see cref="TimerOptions"/> class from an instance of the <see
        /// cref="ParseResult"/> class.
        /// </summary>
        /// <param name="result">A <see cref="ParseResult"/>.</param>
        /// <param name="options">The new instance of the <see cref="TimerOptions"/> class, or <c>null</c> if the <see
        /// cref="TimerOptions"/> could not be created.</param>
        /// <returns><c>true</c> if the <see cref="TimerOptions"/> was successfully created, or <c>false</c> otherwise.
        /// </returns>
        private static bool TryParseResultToTimerOptions(ParseResult result, out TimerOptions options)
        {
            options = result.DefaultOptions == true
                ? new TimerOptions()
                : TimerOptionsManager.Instance.MostRecentOptions;

            options.AlwaysOnTop = result.AlwaysOnTop ?? options.AlwaysOnTop;
            options.LoopTimer = result.LoopTimer ?? options.LoopTimer;
            options.PopUpWhenExpired = result.PopUpWhenExpired ?? options.PopUpWhenExpired;
            options.CloseWhenExpired = result.CloseWhenExpired ?? options.CloseWhenExpired;

            if (result.NoSound.HasValue)
            {
                options.Sound = !result.NoSound.Value ? SoundManager.Instance.GetSoundOrDefaultByName(result.SoundName) : null;
            }

            options.LoopSound = result.LoopSound ?? options.LoopSound;

            if (result.WindowPosition.HasValue || result.WindowSize.HasValue || result.WindowState.HasValue)
            {
                WindowSize windowSize = new WindowSize(
                    result.WindowPosition,
                    result.WindowSize,
                    result.WindowState,
                    result.RestoreWindowState);

                options.WindowSize = WindowSize.Merge(options.WindowSize, windowSize);
            }

            return true;
        }

        /// <summary>
        /// Tries to create a new instance of the <see cref="TimerInput"/> class from an instance of the <see
        /// cref="ParseResult"/> class and an instance of the <see cref="TimerOptions"/> class.
        /// </summary>
        /// <param name="result">A <see cref="ParseResult"/>.</param>
        /// <param name="options">A <see cref="TimerOptions"/>.</param>
        /// <param name="input">The new instance of the <see cref="TimerInput"/> class, or <c>null</c> if the <see
        /// cref="TimerInput"/> could not be created.</param>
        /// <returns><c>true</c> if the <see cref="TimerInput"/> was successfully created, or <c>false</c> otherwise.
        /// </returns>
        private static bool TryParseResultToTimerInput(ParseResult result, TimerOptions options, out TimerInput input)
        {
            if (result.Input == null)
            {
                input = null;
                return true;
            }

            if ((input = TimerInput.FromString(result.Input)) == null)
            {
                return false;
            }

            input.Options.SetFromTimerOptions(options);
            return true;
        }

        /// <summary>
        /// Tries to parse a window position from a string.
        /// </summary>
        /// <param name="arg">A <see cref="string"/>.</param>
        /// <param name="position">The position of the window's top-left corner in relation to the desktop.</param>
        /// <returns><c>true</c> if the window size was successfully parsed, or <c>false</c> otherwise.</returns>
        private static bool TryParseWindowPosition(string arg, out Point position)
        {
            try
            {
                double[] values = arg.Split(',').Select(double.Parse).ToArray();

                if (values.Length != 2)
                {
                    position = new Point();
                    return false;
                }

                position = new Point(values[0], values[1]);
                return true;
            }
            catch (Exception)
            {
                position = new Point();
                return false;
            }
        }

        /// <summary>
        /// Tries to parse a window size from a string.
        /// </summary>
        /// <param name="arg">A <see cref="string"/>.</param>
        /// <param name="size">The size of the window.</param>
        /// <returns><c>true</c> if the window size was successfully parsed, or <c>false</c> otherwise.</returns>
        private static bool TryParseWindowSize(string arg, out Size size)
        {
            try
            {
                double[] values = arg.Split(',').Select(double.Parse).ToArray();

                if (values.Length != 2)
                {
                    size = new Size();
                    return false;
                }

                size = new Size(values[0], values[1]);
                return true;
            }
            catch (Exception)
            {
                size = new Size();
                return false;
            }
        }

        /// <summary>
        /// Tries to parse a window state from a string.
        /// </summary>
        /// <param name="arg">A <see cref="string"/>.</param>
        /// <param name="windowState">A value that indicates whether the window is restored, minimized, or maximized.
        /// </param>
        /// <param name="restoreWindowState">The window's <see cref="Window.WindowState"/> before the window was
        /// minimized.</param>
        /// <returns><c>true</c> if the window state was successfully parsed, or <c>false</c> otherwise.</returns>
        private static bool TryParseWindowState(string arg, out WindowState windowState, out WindowState restoreWindowState)
        {
            string[] values = arg.Split(',');
            if (values.Length != 1 && values.Length != 2)
            {
                windowState = restoreWindowState = WindowState.Normal;
                return false;
            }

            if (!Enum.TryParse(values[0], true /* ignoreCase */, out windowState) || !Enum.IsDefined(typeof(WindowState), windowState))
            {
                windowState = restoreWindowState = WindowState.Normal;
                return false;
            }

            int i = values.Length == 2 ? 1 : 0;
            if (!Enum.TryParse(values[i], true /* ignoreCase */, out restoreWindowState) || !Enum.IsDefined(typeof(WindowState), restoreWindowState))
            {
                windowState = restoreWindowState = WindowState.Normal;
                return false;
            }

            if (windowState != WindowState.Minimized)
            {
                restoreWindowState = windowState;
            }
            else if (restoreWindowState == WindowState.Minimized)
            {
                restoreWindowState = WindowState.Normal;
            }

            return true;
        }

        /// <summary>
        /// Returns the value argument at the index immediately following <paramref name="i"/> and advances <paramref
        /// name="i"/> by 1, or returns <c>null</c> and does not advance <paramref name="i"/> if there is no value
        /// argument at the index immediately following <paramref name="i"/>.
        /// </summary>
        /// <param name="args">The command-line arguments.</param>
        /// <param name="i">The current index in the command-line arguments.</param>
        /// <returns>The value argument at the index immediately following <paramref name="i"/>, or <c>null</c> if
        /// there is no value argument at the index immediately following <paramref name="i"/>.</returns>
        /// <seealso cref="IsValue"/>
        private static string ParseNextValue(string[] args, ref int i)
        {
            if ((i + 1) < args.Length && IsValue(args[i + 1]))
            {
                return args[++i];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Returns a value indicating whether <paramref name="arg"/> is a value argument (i.e., an argument other than
        /// a positive switch or a negative switch).
        /// </summary>
        /// <param name="arg">A command-line argument.</param>
        /// <returns>A value indicating whether <paramref name="arg"/> is a value argument.</returns>
        /// <seealso cref="IsPositiveSwitch"/>
        /// <seealso cref="IsNegativeSwitch"/>
        private static bool IsValue(string arg)
        {
            return !string.IsNullOrEmpty(arg) && !IsPositiveSwitch(arg) && !IsNegativeSwitch(arg);
        }

        /// <summary>
        /// Returns a value indicating whether <paramref name="arg"/> is a a positive switch (e.g., "+a", "++alpha").
        /// </summary>
        /// <param name="arg">A command-line argument.</param>
        /// <returns>A value indicating whether <paramref name="arg"/> is a a positive switch.</returns>
        /// <seealso cref="IsValue"/>
        /// <seealso cref="IsNegativeSwitch"/>
        private static bool IsPositiveSwitch(string arg)
        {
            return arg[0] == '+';
        }

        /// <summary>
        /// Returns a value indicating whether <paramref name="arg"/> is a a negative switch (e.g., "-a", "--alpha").
        /// </summary>
        /// <param name="arg">A command-line argument.</param>
        /// <returns>A value indicating whether <paramref name="arg"/> is a a negative switch.</returns>
        /// <seealso cref="IsValue"/>
        /// <seealso cref="IsPositiveSwitch"/>
        private static bool IsNegativeSwitch(string arg)
        {
            return arg[0] == '-';
        }

        /// <summary>
        /// A partially-processed representation of the command-line arguments.
        /// </summary>
        private class ParseResult
        {
            /// <summary>
            /// Gets or sets the input string used to start a timer.
            /// </summary>
            public string Input { get; set; }

            /// <summary>
            /// Gets or sets a value indicating whether the timer should use the default options, rather than the most
            /// recently used options.
            /// </summary>
            public bool? DefaultOptions { get; set; }

            /// <summary>
            /// Gets or sets a user-specified title for the timer.
            /// </summary>
            public string Title { get; set; }

            /// <summary>
            /// Gets or sets a value indicating whether the timer window should always be displayed on top of other
            /// windows.
            /// </summary>
            public bool? AlwaysOnTop { get; set; }

            /// <summary>
            /// Gets or sets a value indicating whether an icon for the app should be visible in the notification area
            /// of the taskbar.
            /// </summary>
            public bool? ShowInNotificationArea { get; set; }

            /// <summary>
            /// Gets or sets a value indicating whether to loop the timer continuously.
            /// </summary>
            public bool? LoopTimer { get; set; }

            /// <summary>
            /// Gets or sets a value indicating whether the timer window should be brought to the top of other windows
            /// when the timer expires.
            /// </summary>
            public bool? PopUpWhenExpired { get; set; }

            /// <summary>
            /// Gets or sets a value indicating whether the timer window should be closed when the timer expires.
            /// </summary>
            public bool? CloseWhenExpired { get; set; }

            /// <summary>
            /// Gets or sets a value indicating whether no sound should be played when the timer expires.
            /// </summary>
            public bool? NoSound { get; set; }

            /// <summary>
            /// Gets or sets the identifier of the sound to play when the timer expires.
            /// </summary>
            public string SoundName { get; set; }

            /// <summary>
            /// Gets or sets a value indicating whether the sound that plays when the timer expires should be looped
            /// until stopped by the user.
            /// </summary>
            public bool? LoopSound { get; set; }

            /// <summary>
            /// Gets or sets the position of the timer window's top-left corner in relation to the desktop.
            /// </summary>
            public Point? WindowPosition { get; set; }

            /// <summary>
            /// Gets or sets the size of the timer window.
            /// </summary>
            public Size? WindowSize { get; set; }

            /// <summary>
            /// Gets or sets a value that indicates whether the timer window is restored, minimized, or maximized.
            /// </summary>
            public WindowState? WindowState { get; set; }

            /// <summary>
            /// Gets or sets the timer window's <see cref="Window.WindowState"/> before the window was minimized.
            /// </summary>
            public WindowState? RestoreWindowState { get; set; }
        }
    }
}
