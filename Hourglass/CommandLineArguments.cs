// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CommandLineArguments.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text.RegularExpressions;
    using System.Windows;

    using Hourglass.Extensions;
    using Hourglass.Managers;
    using Hourglass.Properties;
    using Hourglass.Timing;
    using Hourglass.Windows;

    /// <summary>
    /// Parsed command-line arguments.
    /// </summary>
    public class CommandLineArguments
    {
        #region Properties

        /// <summary>
        /// Gets the command-line usage for this application.
        /// </summary>
        public static string Usage
        {
            get
            {
                string assemblyLocation = Assembly.GetEntryAssembly().CodeBase;
                string assemblyFileName = Path.GetFileName(assemblyLocation);
                return string.Format(Resources.Usage, assemblyFileName);
            }
        }

        /// <summary>
        /// Gets a value indicating whether the command-line arguments were not successfully parsed.
        /// </summary>
        public bool HasParseError { get; private set; }

        /// <summary>
        /// Gets an error message if the command-line arguments were not successfully parsed, or <c>null</c> otherwise.
        /// </summary>
        public string ParseErrorMessage { get; private set; }

        /// <summary>
        /// Gets a value indicating whether command-line usage information should be showed to the user.
        /// </summary>
        public bool ShouldShowUsage { get; private set; }

        /// <summary>
        /// Gets a <see cref="TimerStart"/>, or <c>null</c> if no <see cref="TimerStart"/> was specified on the command
        /// line.
        /// </summary>
        public TimerStart TimerStart { get; private set; }

        /// <summary>
        /// Gets a user-specified title for the timer.
        /// </summary>
        public string Title { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the timer window should always be displayed on top of other windows.
        /// </summary>
        public bool AlwaysOnTop { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the timer window should be in full-screen mode.
        /// </summary>
        public bool IsFullScreen { get; private set; }

        /// <summary>
        /// Gets a value indicating whether to prompt the user before closing the timer window if the timer is running.
        /// </summary>
        public bool PromptOnExit { get; private set; }

        /// <summary>
        /// Gets a value indicating whether to keep the computer awake while the timer is running.
        /// </summary>
        public bool DoNotKeepComputerAwake { get; private set; }

        /// <summary>
        /// Gets a value indicating whether an icon for the app should be visible in the notification area of the
        /// taskbar.
        /// </summary>
        public bool ShowInNotificationArea { get; private set; }

        /// <summary>
        /// Gets a value indicating whether to show the time elapsed rather than the time left.
        /// </summary>
        public bool ShowTimeElapsed { get; private set; }

        /// <summary>
        /// Gets a value indicating whether to loop the timer continuously.
        /// </summary>
        public bool LoopTimer { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the timer window should be brought to the top of other windows when the
        /// timer expires.
        /// </summary>
        public bool PopUpWhenExpired { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the timer window should be closed when the timer expires.
        /// </summary>
        public bool CloseWhenExpired { get; private set; }

        /// <summary>
        /// Gets a value indicating whether Windows should be shut down when the timer expires.
        /// </summary>
        public bool ShutDownWhenExpired { get; private set; }

        /// <summary>
        /// Gets the theme of the timer window.
        /// </summary>
        public Theme Theme { get; private set; }

        /// <summary>
        /// Gets the sound to play when the timer expires, or <c>null</c> if no sound is to be played.
        /// </summary>
        public Sound Sound { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the sound that plays when the timer expires should be looped until stopped
        /// by the user.
        /// </summary>
        public bool LoopSound { get; private set; }

        /// <summary>
        /// Gets a value indicating whether all saved timers should be opened when the application starts.
        /// </summary>
        public bool OpenSavedTimers { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating what information to display in the timer window title.
        /// </summary>
        public WindowTitleMode WindowTitleMode { get; set; }

        /// <summary>
        /// Gets a value that indicates whether the timer window is restored, minimized, or maximized.
        /// </summary>
        public WindowState WindowState { get; private set; }

        /// <summary>
        /// Gets the window's <see cref="Window.WindowState"/> before the window was minimized.
        /// </summary>
        public WindowState RestoreWindowState { get; private set; }

        /// <summary>
        /// Gets the size and location of the window.
        /// </summary>
        public Rect WindowBounds { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the user interface should be locked, preventing the user from taking any
        /// action until the timer expires.
        /// </summary>
        public bool LockInterface { get; private set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Parses command-line arguments.
        /// </summary>
        /// <param name="args">The command-line arguments.</param>
        /// <returns>A <see cref="CommandLineArguments"/> object.</returns>
        public static CommandLineArguments Parse(IList<string> args)
        {
            try
            {
                return GetCommandLineArguments(args);
            }
            catch (ParseException e)
            {
                return new CommandLineArguments
                {
                    HasParseError = true,
                    ParseErrorMessage = e.Message
                };
            }
        }

        /// <summary>
        /// Shows the command-line usage of this application in a window.
        /// </summary>
        /// <param name="errorMessage">An error message to display. (Optional.)</param>
        public static void ShowUsage(string errorMessage = null)
        {
            UsageDialog usageDialog = new UsageDialog();
            usageDialog.ErrorMessage = errorMessage;

            if (Application.Current != null && Application.Current.Dispatcher != null)
            {
                usageDialog.Show();
            }
            else
            {
                usageDialog.ShowDialog();
            }
        }

        /// <summary>
        /// Returns the <see cref="TimerOptions"/> specified in the parsed command-line arguments.
        /// </summary>
        /// <returns>The <see cref="TimerOptions"/> specified in the parsed command-line arguments.</returns>
        public TimerOptions GetTimerOptions()
        {
            return new TimerOptions
            {
                Title = this.Title,
                AlwaysOnTop = this.AlwaysOnTop,
                PromptOnExit = this.PromptOnExit,
                DoNotKeepComputerAwake = this.DoNotKeepComputerAwake,
                ShowTimeElapsed = this.ShowTimeElapsed,
                LoopTimer = this.LoopTimer,
                PopUpWhenExpired = this.PopUpWhenExpired,
                CloseWhenExpired = this.CloseWhenExpired,
                ShutDownWhenExpired = this.ShutDownWhenExpired,
                Theme = this.Theme,
                Sound = this.Sound,
                LoopSound = this.LoopSound,
                WindowTitleMode = this.WindowTitleMode,
                WindowSize = this.GetWindowSize(),
                LockInterface = this.LockInterface
            };
        }

        /// <summary>
        /// Returns the <see cref="WindowSize"/> specified in the parsed command-line arguments.
        /// </summary>
        /// <returns>The <see cref="WindowSize"/> specified in the parsed command-line arguments.</returns>
        public WindowSize GetWindowSize()
        {
            return new WindowSize(
                this.WindowBounds,
                this.WindowState,
                this.RestoreWindowState,
                this.IsFullScreen);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Returns an <see cref="CommandLineArguments"/> instance based on the most recent options.
        /// </summary>
        /// <returns>An <see cref="CommandLineArguments"/> instance based on the most recent options.</returns>
        private static CommandLineArguments GetArgumentsFromMostRecentOptions()
        {
            TimerOptions options = TimerOptionsManager.Instance.MostRecentOptions;
            WindowSize windowSize = GetMostRecentWindowSize();

            return new CommandLineArguments
            {
                Title = null,
                AlwaysOnTop = options.AlwaysOnTop,
                IsFullScreen = windowSize.IsFullScreen,
                PromptOnExit = options.PromptOnExit,
                DoNotKeepComputerAwake = options.DoNotKeepComputerAwake,
                ShowTimeElapsed = options.ShowTimeElapsed,
                ShowInNotificationArea = Settings.Default.ShowInNotificationArea,
                LoopTimer = options.LoopTimer,
                PopUpWhenExpired = options.PopUpWhenExpired,
                CloseWhenExpired = options.CloseWhenExpired,
                ShutDownWhenExpired = options.ShutDownWhenExpired,
                Theme = options.Theme,
                Sound = options.Sound,
                LoopSound = options.LoopSound,
                OpenSavedTimers = Settings.Default.OpenSavedTimersOnStartup,
                WindowTitleMode = options.WindowTitleMode,
                WindowState = windowSize.WindowState != WindowState.Minimized ? windowSize.WindowState : windowSize.RestoreWindowState,
                RestoreWindowState = windowSize.RestoreWindowState,
                WindowBounds = windowSize.RestoreBounds,
                LockInterface = options.LockInterface
            };
        }

        /// <summary>
        /// Returns an <see cref="CommandLineArguments"/> instance based on the factory default settings.
        /// </summary>
        /// <returns>An <see cref="CommandLineArguments"/> instance based on the factory default settings.</returns>
        private static CommandLineArguments GetArgumentsFromFactoryDefaults()
        {
            TimerOptions defaultOptions = new TimerOptions();

            WindowSize mostRecentWindowSize = GetMostRecentWindowSize();
            Rect defaultWindowBounds = defaultOptions.WindowSize.RestoreBounds;
            Rect defaultWindowBoundsWithLocation = mostRecentWindowSize.RestoreBounds.Merge(defaultWindowBounds);

            return new CommandLineArguments
            {
                Title = defaultOptions.Title,
                AlwaysOnTop = defaultOptions.AlwaysOnTop,
                IsFullScreen = defaultOptions.WindowSize.IsFullScreen,
                PromptOnExit = defaultOptions.PromptOnExit,
                DoNotKeepComputerAwake = defaultOptions.DoNotKeepComputerAwake,
                ShowTimeElapsed = defaultOptions.ShowTimeElapsed,
                ShowInNotificationArea = false,
                LoopTimer = defaultOptions.LoopTimer,
                PopUpWhenExpired = defaultOptions.PopUpWhenExpired,
                CloseWhenExpired = defaultOptions.CloseWhenExpired,
                ShutDownWhenExpired = defaultOptions.ShutDownWhenExpired,
                Theme = defaultOptions.Theme,
                Sound = defaultOptions.Sound,
                LoopSound = defaultOptions.LoopSound,
                OpenSavedTimers = false,
                WindowTitleMode = WindowTitleMode.ApplicationName,
                WindowState = defaultOptions.WindowSize.WindowState,
                RestoreWindowState = defaultOptions.WindowSize.RestoreWindowState,
                WindowBounds = defaultWindowBoundsWithLocation,
                LockInterface = defaultOptions.LockInterface
            };
        }

        /// <summary>
        /// Returns the most recent <see cref="WindowSize"/>.
        /// </summary>
        /// <returns>The most recent <see cref="WindowSize"/>.</returns>
        private static WindowSize GetMostRecentWindowSize()
        {
            WindowSize windowSizeFromSettings = Settings.Default.WindowSize;
            WindowSize windowSizeFromSibling = WindowSize.FromWindowOfType<TimerWindow>().Offset();
            return windowSizeFromSibling ?? windowSizeFromSettings ?? new WindowSize();
        }

        /// <summary>
        /// Parses command-line arguments.
        /// </summary>
        /// <param name="args">The command-line arguments.</param>
        /// <returns>The parsed command-line arguments.</returns>
        /// <exception cref="ParseException">If the command-line arguments could not be parsed.</exception>
        private static CommandLineArguments GetCommandLineArguments(IEnumerable<string> args)
        {
            CommandLineArguments argumentsBasedOnMostRecentOptions = GetArgumentsFromMostRecentOptions();
            CommandLineArguments argumentsBasedOnFactoryDefaults = GetArgumentsFromFactoryDefaults();
            bool useFactoryDefaults = false;

            HashSet<string> specifiedSwitches = new HashSet<string>();
            Queue<string> remainingArgs = new Queue<string>(args);
            while (remainingArgs.Count > 0)
            {
                string arg = remainingArgs.Dequeue();

                switch (arg)
                {
                    case "--title":
                    case "-t":
                        ThrowIfDuplicateSwitch(specifiedSwitches, "--title");

                        string title = GetRequiredValue(arg, remainingArgs);

                        argumentsBasedOnMostRecentOptions.Title = title;
                        argumentsBasedOnFactoryDefaults.Title = title;
                        break;

                    case "--always-on-top":
                    case "-a":
                        ThrowIfDuplicateSwitch(specifiedSwitches, "--always-on-top");

                        bool alwaysOnTop = GetBoolValue(
                            arg,
                            remainingArgs,
                            argumentsBasedOnMostRecentOptions.AlwaysOnTop);

                        argumentsBasedOnMostRecentOptions.AlwaysOnTop = alwaysOnTop;
                        argumentsBasedOnFactoryDefaults.AlwaysOnTop = alwaysOnTop;
                        break;

                    case "--full-screen":
                    case "-f":
                        ThrowIfDuplicateSwitch(specifiedSwitches, "--full-screen");

                        bool isFullScreen = GetBoolValue(
                            arg,
                            remainingArgs,
                            argumentsBasedOnMostRecentOptions.IsFullScreen);

                        argumentsBasedOnMostRecentOptions.IsFullScreen = isFullScreen;
                        argumentsBasedOnFactoryDefaults.IsFullScreen = isFullScreen;
                        break;

                    case "--prompt-on-exit":
                    case "-o":
                        ThrowIfDuplicateSwitch(specifiedSwitches, "--prompt-on-exit");

                        bool promptOnExit = GetBoolValue(
                            arg,
                            remainingArgs,
                            argumentsBasedOnMostRecentOptions.PromptOnExit);

                        argumentsBasedOnMostRecentOptions.PromptOnExit = promptOnExit;
                        argumentsBasedOnFactoryDefaults.PromptOnExit = promptOnExit;
                        break;

                    case "--do-not-keep-awake":
                    case "-k":
                        ThrowIfDuplicateSwitch(specifiedSwitches, "--do-not-keep-awake");

                        bool doNotKeepComputerAwake = GetBoolValue(
                            arg,
                            remainingArgs,
                            argumentsBasedOnMostRecentOptions.DoNotKeepComputerAwake);

                        argumentsBasedOnMostRecentOptions.DoNotKeepComputerAwake = doNotKeepComputerAwake;
                        argumentsBasedOnFactoryDefaults.DoNotKeepComputerAwake = doNotKeepComputerAwake;
                        break;

                    case "--show-time-elapsed":
                    case "-u":
                        ThrowIfDuplicateSwitch(specifiedSwitches, "--show-time-elapsed");

                        bool showTimeElapsed = GetBoolValue(
                            arg,
                            remainingArgs,
                            argumentsBasedOnMostRecentOptions.ShowTimeElapsed);

                        argumentsBasedOnMostRecentOptions.ShowTimeElapsed = showTimeElapsed;
                        argumentsBasedOnFactoryDefaults.ShowTimeElapsed = showTimeElapsed;
                        break;

                    case "--show-in-notification-area":
                    case "-n":
                        ThrowIfDuplicateSwitch(specifiedSwitches, "--show-in-notification-area");

                        bool showInNotificationArea = GetBoolValue(
                            arg,
                            remainingArgs,
                            argumentsBasedOnMostRecentOptions.ShowInNotificationArea);

                        argumentsBasedOnMostRecentOptions.ShowInNotificationArea = showInNotificationArea;
                        argumentsBasedOnFactoryDefaults.ShowInNotificationArea = showInNotificationArea;
                        break;

                    case "--loop-timer":
                    case "-l":
                        ThrowIfDuplicateSwitch(specifiedSwitches, "--loop-timer");

                        bool loopTimer = GetBoolValue(
                            arg,
                            remainingArgs,
                            argumentsBasedOnMostRecentOptions.LoopTimer);

                        argumentsBasedOnMostRecentOptions.LoopTimer = loopTimer;
                        argumentsBasedOnFactoryDefaults.LoopTimer = loopTimer;
                        break;

                    case "--pop-up-when-expired":
                    case "-p":
                        ThrowIfDuplicateSwitch(specifiedSwitches, "--pop-up-when-expired");

                        bool popUpWhenExpired = GetBoolValue(
                            arg,
                            remainingArgs,
                            argumentsBasedOnMostRecentOptions.PopUpWhenExpired);

                        argumentsBasedOnMostRecentOptions.PopUpWhenExpired = popUpWhenExpired;
                        argumentsBasedOnFactoryDefaults.PopUpWhenExpired = popUpWhenExpired;
                        break;

                    case "--close-when-expired":
                    case "-e":
                        ThrowIfDuplicateSwitch(specifiedSwitches, "--close-when-expired");

                        bool closeWhenExpired = GetBoolValue(
                            arg,
                            remainingArgs,
                            argumentsBasedOnMostRecentOptions.CloseWhenExpired);

                        argumentsBasedOnMostRecentOptions.CloseWhenExpired = closeWhenExpired;
                        argumentsBasedOnFactoryDefaults.CloseWhenExpired = closeWhenExpired;
                        break;

                    case "--shut-down-when-expired":
                    case "-x":
                        ThrowIfDuplicateSwitch(specifiedSwitches, "--shut-down-when-expired");

                        bool shutDownWhenExpired = GetBoolValue(
                            arg,
                            remainingArgs);

                        argumentsBasedOnMostRecentOptions.ShutDownWhenExpired = shutDownWhenExpired;
                        argumentsBasedOnFactoryDefaults.ShutDownWhenExpired = shutDownWhenExpired;
                        break;

                    case "--theme":
                    case "-m":
                        ThrowIfDuplicateSwitch(specifiedSwitches, "--theme");

                        Theme theme = GetThemeValue(
                            arg,
                            remainingArgs,
                            argumentsBasedOnMostRecentOptions.Theme);

                        argumentsBasedOnMostRecentOptions.Theme = theme;
                        argumentsBasedOnFactoryDefaults.Theme = theme;
                        break;

                    case "--sound":
                    case "-s":
                        ThrowIfDuplicateSwitch(specifiedSwitches, "--sound");

                        Sound sound = GetSoundValue(
                            arg,
                            remainingArgs,
                            argumentsBasedOnMostRecentOptions.Sound);

                        argumentsBasedOnMostRecentOptions.Sound = sound;
                        argumentsBasedOnFactoryDefaults.Sound = sound;
                        break;

                    case "--loop-sound":
                    case "-r":
                        ThrowIfDuplicateSwitch(specifiedSwitches, "--loop-sound");

                        bool loopSound = GetBoolValue(
                            arg,
                            remainingArgs,
                            argumentsBasedOnMostRecentOptions.LoopSound);

                        argumentsBasedOnMostRecentOptions.LoopSound = loopSound;
                        argumentsBasedOnFactoryDefaults.LoopSound = loopSound;
                        break;

                    case "--open-saved-timers":
                    case "-v":
                        ThrowIfDuplicateSwitch(specifiedSwitches, "--open-saved-timers");

                        bool openSavedTimers = GetBoolValue(
                            arg,
                            remainingArgs,
                            argumentsBasedOnMostRecentOptions.OpenSavedTimers);

                        argumentsBasedOnMostRecentOptions.OpenSavedTimers = openSavedTimers;
                        argumentsBasedOnFactoryDefaults.OpenSavedTimers = openSavedTimers;
                        break;

                    case "--window-title":
                    case "-i":
                        ThrowIfDuplicateSwitch(specifiedSwitches, "--window-title");

                        WindowTitleMode windowTitleMode = GetWindowTitleModeValue(
                            arg,
                            remainingArgs,
                            argumentsBasedOnMostRecentOptions.WindowTitleMode);

                        argumentsBasedOnMostRecentOptions.WindowTitleMode = windowTitleMode;
                        argumentsBasedOnFactoryDefaults.WindowTitleMode = windowTitleMode;
                        break;

                    case "--window-state":
                    case "-w":
                        ThrowIfDuplicateSwitch(specifiedSwitches, "--window-state");

                        WindowState windowState = GetWindowStateValue(
                            arg,
                            remainingArgs,
                            argumentsBasedOnMostRecentOptions.WindowState);

                        argumentsBasedOnMostRecentOptions.WindowState = windowState;
                        argumentsBasedOnFactoryDefaults.WindowState = windowState;
                        break;

                    case "--window-bounds":
                    case "-b":
                        ThrowIfDuplicateSwitch(specifiedSwitches, "--window-bounds");

                        Rect windowBounds = GetRectValue(
                            arg,
                            remainingArgs,
                            argumentsBasedOnMostRecentOptions.WindowBounds);

                        argumentsBasedOnMostRecentOptions.WindowBounds = argumentsBasedOnMostRecentOptions.WindowBounds.Merge(windowBounds);
                        argumentsBasedOnFactoryDefaults.WindowBounds = argumentsBasedOnFactoryDefaults.WindowBounds.Merge(windowBounds);
                        break;

                    case "--lock-interface":
                    case "-z":
                        ThrowIfDuplicateSwitch(specifiedSwitches, "--lock-interface");

                        bool lockInterface = GetBoolValue(
                            arg,
                            remainingArgs);

                        argumentsBasedOnMostRecentOptions.LockInterface = lockInterface;
                        argumentsBasedOnFactoryDefaults.LockInterface = lockInterface;
                        break;

                    case "--use-factory-defaults":
                    case "-d":
                        ThrowIfDuplicateSwitch(specifiedSwitches, "--use-factory-defaults");

                        useFactoryDefaults = true;
                        break;

                    case "--help":
                    case "-h":
                    case "-?":
                        ThrowIfDuplicateSwitch(specifiedSwitches, "--help");

                        argumentsBasedOnMostRecentOptions.ShouldShowUsage = true;
                        argumentsBasedOnFactoryDefaults.ShouldShowUsage = true;
                        break;

                    default:
                        if (IsSwitch(arg))
                        {
                            string message = string.Format(
                                Resources.ResourceManager.GetEffectiveProvider(),
                                Resources.CommandLineArgumentsParseExceptionUnrecognizedSwitchFormatString,
                                arg);

                            throw new ParseException(message);
                        }

                        List<string> inputArgs = remainingArgs.ToList();
                        inputArgs.Insert(0, arg);
                        remainingArgs.Clear();

                        TimerStart timerStart = GetTimerStartValue(inputArgs);

                        argumentsBasedOnMostRecentOptions.TimerStart = timerStart;
                        argumentsBasedOnFactoryDefaults.TimerStart = timerStart;
                        break;
                }
            }

            return useFactoryDefaults ? argumentsBasedOnFactoryDefaults : argumentsBasedOnMostRecentOptions;
        }

        /// <summary>
        /// Returns the next value in <paramref name="remainingArgs"/>.
        /// </summary>
        /// <param name="remainingArgs">The unparsed arguments.</param>
        /// <returns>The next value in <paramref name="remainingArgs"/>.</returns>
        private static string GetValue(Queue<string> remainingArgs)
        {
            if (remainingArgs.Count > 0 && !IsSwitch(remainingArgs.Peek()))
            {
                string value = remainingArgs.Dequeue();
                return UnescapeValue(value);
            }

            return null;
        }

        /// <summary>
        /// Returns the next value in <paramref name="remainingArgs"/>, or throws an exception if <paramref
        /// name="remainingArgs"/> is empty or the next argument in <paramref name="remainingArgs"/> is a switch.
        /// </summary>
        /// <param name="arg">The name of the argument for which the value is to be returned.</param>
        /// <param name="remainingArgs">The unparsed arguments.</param>
        /// <returns>The next value in <paramref name="remainingArgs"/>.</returns>
        /// <exception cref="ParseException">If <paramref name="remainingArgs"/> is empty or the next argument in
        /// <paramref name="remainingArgs"/> is a switch.</exception>
        private static string GetRequiredValue(string arg, Queue<string> remainingArgs)
        {
            string value = GetValue(remainingArgs);

            if (value == null)
            {
                string message = string.Format(
                    Resources.ResourceManager.GetEffectiveProvider(),
                    Resources.CommandLineArgumentsParseExceptionMissingValueForSwitchFormatString,
                    arg);

                throw new ParseException(message);
            }

            return value;
        }

        /// <summary>
        /// Returns the next <see cref="bool"/> value in <paramref name="remainingArgs"/>, or throws an exception if
        /// <paramref name="remainingArgs"/> is empty or the next argument is not "on" or "off".
        /// </summary>
        /// <param name="arg">The name of the argument for which the value is to be returned.</param>
        /// <param name="remainingArgs">The unparsed arguments.</param>
        /// <returns>The next <see cref="bool"/> value in <paramref name="remainingArgs"/>.</returns>
        /// <exception cref="ParseException">If <paramref name="remainingArgs"/> is empty or the next argument is not
        /// "on" or "off".</exception>
        private static bool GetBoolValue(string arg, Queue<string> remainingArgs)
        {
            string value = GetRequiredValue(arg, remainingArgs);

            switch (value.ToLowerInvariant())
            {
                case "on":
                    return true;

                case "off":
                    return false;

                default:
                    string message = string.Format(
                        Resources.ResourceManager.GetEffectiveProvider(),
                        Resources.CommandLineArgumentsParseExceptionInvalidValueForSwitchFormatString,
                        arg,
                        value);

                    throw new ParseException(message);
            }
        }

        /// <summary>
        /// Returns the next <see cref="bool"/> value in <paramref name="remainingArgs"/>, or throws an exception if
        /// <paramref name="remainingArgs"/> is empty or the next argument is not "on", "off", or "last".
        /// </summary>
        /// <param name="arg">The name of the argument for which the value is to be returned.</param>
        /// <param name="remainingArgs">The unparsed arguments.</param>
        /// <param name="last">The value of the argument returned when the user specifies "last".</param>
        /// <returns>The next <see cref="bool"/> value in <paramref name="remainingArgs"/>.</returns>
        /// <exception cref="ParseException">If <paramref name="remainingArgs"/> is empty or the next argument is not
        /// "on", "off", or "last".</exception>
        private static bool GetBoolValue(string arg, Queue<string> remainingArgs, bool last)
        {
            string value = GetRequiredValue(arg, remainingArgs);

            switch (value.ToLowerInvariant())
            {
                case "on":
                    return true;

                case "off":
                    return false;

                case "last":
                    return last;

                default:
                    string message = string.Format(
                        Resources.ResourceManager.GetEffectiveProvider(),
                        Resources.CommandLineArgumentsParseExceptionInvalidValueForSwitchFormatString,
                        arg,
                        value);

                    throw new ParseException(message);
            }
        }

        /// <summary>
        /// Returns the next <see cref="Theme"/> value in <paramref name="remainingArgs"/>, or throws an exception if
        /// <paramref name="remainingArgs"/> is empty or the next argument is not "last" or a valid representation of a
        /// <see cref="Theme"/>.
        /// </summary>
        /// <param name="arg">The name of the argument for which the value is to be returned.</param>
        /// <param name="remainingArgs">The unparsed arguments.</param>
        /// <param name="last">The value of the argument returned when the user specifies "last".</param>
        /// <returns>The next <see cref="Theme"/> value in <paramref name="remainingArgs"/></returns>
        /// <exception cref="ParseException">If <paramref name="remainingArgs"/> is empty or the next argument is not
        /// "last" or a valid representation of a <see cref="Theme"/>.</exception>
        private static Theme GetThemeValue(string arg, Queue<string> remainingArgs, Theme last)
        {
            string value = GetRequiredValue(arg, remainingArgs);

            switch (value.ToLowerInvariant())
            {
                case "last":
                    return last;

                default:
                    Theme theme = ThemeManager.Instance.GetThemeByIdentifier(value.ToLowerInvariant()) ??
                        ThemeManager.Instance.GetThemeByName(value, StringComparison.CurrentCultureIgnoreCase);

                    if (theme == null)
                    {
                        string message = string.Format(
                            Resources.ResourceManager.GetEffectiveProvider(),
                            Resources.CommandLineArgumentsParseExceptionInvalidValueForSwitchFormatString,
                            arg,
                            value);

                        throw new ParseException(message);
                    }

                    return theme;
            }
        }

        /// <summary>
        /// Returns the next <see cref="Sound"/> value in <paramref name="remainingArgs"/>, or throws an exception if
        /// <paramref name="remainingArgs"/> is empty or the next argument is not "none", "last", or a valid
        /// representation of a <see cref="Sound"/>.
        /// </summary>
        /// <param name="arg">The name of the argument for which the value is to be returned.</param>
        /// <param name="remainingArgs">The unparsed arguments.</param>
        /// <param name="last">The value of the argument returned when the user specifies "last".</param>
        /// <returns>The next <see cref="Sound"/> value in <paramref name="remainingArgs"/>.</returns>
        /// <exception cref="ParseException">if <paramref name="remainingArgs"/> is empty or the next argument is not
        /// "none", "last", or a valid representation of a <see cref="Sound"/>.</exception>
        private static Sound GetSoundValue(string arg, Queue<string> remainingArgs, Sound last)
        {
            string value = GetRequiredValue(arg, remainingArgs);

            switch (value.ToLowerInvariant())
            {
                case "none":
                    return null;

                case "last":
                    return last;

                default:
                    Sound sound = SoundManager.Instance.GetSoundByName(value, StringComparison.CurrentCultureIgnoreCase);

                    if (sound == null)
                    {
                        string message = string.Format(
                            Resources.ResourceManager.GetEffectiveProvider(),
                            Resources.CommandLineArgumentsParseExceptionInvalidValueForSwitchFormatString,
                            arg,
                            value);

                        throw new ParseException(message);
                    }

                    return sound;
            }
        }

        /// <summary>
        /// Returns the next <see cref="WindowTitleMode"/> value in <paramref name="remainingArgs"/>, or throws an
        /// exception if <paramref name="remainingArgs"/> is empty or the next argument is not "app", "left", "elapsed",
        /// "title", or "last".
        /// </summary>
        /// <param name="arg">The name of the argument for which the value is to be returned.</param>
        /// <param name="remainingArgs">The unparsed arguments.</param>
        /// <param name="last">The value of the argument returned when the user specifies "last".</param>
        /// <returns>The next <see cref="WindowTitleMode"/> value in <paramref name="remainingArgs"/>.</returns>
        /// <exception cref="ParseException">If <paramref name="remainingArgs"/> is empty or the next argument is not
        /// "app", "left", "elapsed", "title", or "last".</exception>
        private static WindowTitleMode GetWindowTitleModeValue(string arg, Queue<string> remainingArgs, WindowTitleMode last)
        {
            string value = GetRequiredValue(arg, remainingArgs);

            switch (value.ToLowerInvariant())
            {
                case "app":
                    return WindowTitleMode.ApplicationName;

                case "left":
                    return WindowTitleMode.TimeLeft;

                case "elapsed":
                    return WindowTitleMode.TimeElapsed;

                case "title":
                    return WindowTitleMode.TimerTitle;

                case "left+title":
                    return WindowTitleMode.TimeLeftPlusTimerTitle;

                case "elapsed+title":
                    return WindowTitleMode.TimeElapsedPlusTimerTitle;

                case "title+left":
                    return WindowTitleMode.TimerTitlePlusTimeLeft;

                case "title+elapsed":
                    return WindowTitleMode.TimerTitlePlusTimeElapsed;

                case "last":
                    return last;

                default:
                    string message = string.Format(
                        Resources.ResourceManager.GetEffectiveProvider(),
                        Resources.CommandLineArgumentsParseExceptionInvalidValueForSwitchFormatString,
                        arg,
                        value);

                    throw new ParseException(message);
            }
        }

        /// <summary>
        /// Returns the next <see cref="WindowState"/> value in <paramref name="remainingArgs"/>, or throws an
        /// exception if <paramref name="remainingArgs"/> is empty or the next argument is not "normal", "maximized",
        /// "minimized", or "last".
        /// </summary>
        /// <param name="arg">The name of the argument for which the value is to be returned.</param>
        /// <param name="remainingArgs">The unparsed arguments.</param>
        /// <param name="last">The value of the argument returned when the user specifies "last".</param>
        /// <returns>The next <see cref="WindowState"/> value in <paramref name="remainingArgs"/>.</returns>
        /// <exception cref="ParseException">If <paramref name="remainingArgs"/> is empty or the next argument is not
        /// "normal", "maximized", "minimized", or "last".</exception>
        private static WindowState GetWindowStateValue(string arg, Queue<string> remainingArgs, WindowState last)
        {
            string value = GetRequiredValue(arg, remainingArgs);

            switch (value.ToLowerInvariant())
            {
                case "normal":
                    return WindowState.Normal;

                case "maximized":
                    return WindowState.Maximized;

                case "minimized":
                    return WindowState.Minimized;

                case "last":
                    return last;

                default:
                    string message = string.Format(
                        Resources.ResourceManager.GetEffectiveProvider(),
                        Resources.CommandLineArgumentsParseExceptionInvalidValueForSwitchFormatString,
                        arg,
                        value);

                    throw new ParseException(message);
            }
        }

        /// <summary>
        /// Returns the next <see cref="Rect"/> value in <paramref name="remainingArgs"/>, or throws an exception if
        /// <paramref name="remainingArgs"/> is empty or the next argument is not a valid representation of a <see
        /// cref="Rect"/>.
        /// </summary>
        /// <param name="arg">The name of the argument for which the value is to be returned.</param>
        /// <param name="remainingArgs">The unparsed arguments.</param>
        /// <param name="last">The value of the argument returned when the user specifies "last".</param>
        /// <returns>The next <see cref="Rect"/> value in <paramref name="remainingArgs"/>.</returns>
        /// <exception cref="ParseException">If <paramref name="remainingArgs"/> is empty or the next argument is not a
        /// valid representation of a <see cref="Rect"/>.</exception>
        private static Rect GetRectValue(string arg, Queue<string> remainingArgs, Rect last)
        {
            string value = GetRequiredValue(arg, remainingArgs);

            try
            {
                if (value == "last")
                {
                    return last;
                }

                string adjustedValue = Regex.Replace(value, @"\bauto\b", @"Infinity");
                return Rect.Parse(adjustedValue);
            }
            catch
            {
                string message = string.Format(
                    Resources.ResourceManager.GetEffectiveProvider(),
                    Resources.CommandLineArgumentsParseExceptionInvalidValueForSwitchFormatString,
                    arg,
                    value);

                throw new ParseException(message);
            }
        }

        /// <summary>
        /// Returns the <see cref="TimerStart"/> value corresponding to the concatenation of all <paramref
        /// name="remainingArgs"/>, or throws an exception if the concatenation of all <paramref name="remainingArgs"/>
        /// is not a valid representation of a <see cref="TimerStart"/>.
        /// </summary>
        /// <param name="remainingArgs">The unparsed arguments.</param>
        /// <returns>The <see cref="TimerStart"/> value corresponding to the concatenation of all <paramref
        /// name="remainingArgs"/></returns>
        /// <exception cref="ParseException">If the concatenation of all <paramref name="remainingArgs"/> is not a
        /// valid representation of a <see cref="TimerStart"/>.</exception>
        private static TimerStart GetTimerStartValue(IEnumerable<string> remainingArgs)
        {
            string value = string.Join(" ", remainingArgs);
            TimerStart timerStart = TimerStart.FromString(value);

            if (timerStart == null)
            {
                string message = string.Format(
                    Resources.ResourceManager.GetEffectiveProvider(),
                    Resources.CommandLineArgumentsParseExceptionInvalidTimerInputFormatString,
                    value);

                throw new ParseException(message);
            }

            return timerStart;
        }

        /// <summary>
        /// Returns a value indicating whether a string is a command-line switch.
        /// </summary>
        /// <param name="arg">A string.</param>
        /// <returns>A value indicating whether <paramref name="arg"/> is a command-line switch.</returns>
        private static bool IsSwitch(string arg)
        {
            return arg.StartsWith("-");
        }

        /// <summary>
        /// Unescapes a command-line value.
        /// </summary>
        /// <remarks>
        /// A value is any command-line argument not beginning with '-'. If the user must specify a command-line value
        /// that begins with '-', the user must escape the '-' with '''.
        /// </remarks>
        /// <param name="value">An escaped value string.</param>
        /// <returns>The unescaped value.</returns>
        private static string UnescapeValue(string value)
        {
            return !value.StartsWith("'") ? value : value.Substring(1);
        }

        /// <summary>
        /// Throws an exception if <paramref name="canonicalSwitch"/> is already in <paramref name="specifiedSwitches"/>,
        /// or otherwise adds <paramref name="canonicalSwitch"/> to <paramref name="specifiedSwitches"/>.
        /// </summary>
        /// <param name="specifiedSwitches">The switch arguments that are already specified.</param>
        /// <param name="canonicalSwitch">The canonical representation of a switch argument.</param>
        private static void ThrowIfDuplicateSwitch(HashSet<string> specifiedSwitches, string canonicalSwitch)
        {
            if (!specifiedSwitches.Add(canonicalSwitch))
            {
                string message = string.Format(
                    Resources.ResourceManager.GetEffectiveProvider(),
                    Resources.CommandLineArgumentsParseExceptionDuplicateSwitchFormatString,
                    canonicalSwitch);

                throw new ParseException(message);
            }
        }

        #endregion

        #region Classes

        /// <summary>
        /// Represents an error during <see cref="CommandLineArguments.GetCommandLineArguments"/>.
        /// </summary>
        [Serializable]
        private class ParseException : Exception
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="ParseException"/> class.
            /// </summary>
            /// <param name="message">The message that describes the error.</param>
            public ParseException(string message)
                : base(message)
            {
            }
        }

        #endregion
    }
}
