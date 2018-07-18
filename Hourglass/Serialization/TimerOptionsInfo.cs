// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimerOptionsInfo.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass.Serialization
{
    using Hourglass.Timing;

    /// <summary>
    /// The representation of a <see cref="TimerOptions"/> used for XML serialization.
    /// </summary>
    public class TimerOptionsInfo
    {
        /// <summary>
        /// Gets or sets a user-specified title for the timer.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the timer window should always be displayed on top of other windows.
        /// </summary>
        public bool AlwaysOnTop { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to prompt the user before closing the timer window if the timer is
        /// running.
        /// </summary>
        public bool PromptOnExit { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show progress in the taskbar.
        /// </summary>
        public bool ShowProgressInTaskbar { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to keep the computer awake while the timer is running.
        /// </summary>
        public bool DoNotKeepComputerAwake { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show the time elapsed rather than the time left.
        /// </summary>
        public bool ShowTimeElapsed { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to loop the timer continuously.
        /// </summary>
        public bool LoopTimer { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the timer window should be brought to the top of other windows when
        /// the timer expires.
        /// </summary>
        public bool PopUpWhenExpired { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the timer window should be closed when the timer expires.
        /// </summary>
        public bool CloseWhenExpired { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether Windows should be shut down when the timer expires.
        /// </summary>
        public bool ShutDownWhenExpired { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the theme of the timer window.
        /// </summary>
        public string ThemeIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the sound to play when the timer expires, or <c>null</c> if no sound is to
        /// be played.
        /// </summary>
        public string SoundIdentifier { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the sound that plays when the timer expires should be looped until
        /// stopped by the user.
        /// </summary>
        public bool LoopSound { get; set; }

        /// <summary>
        /// Gets or sets a value indicating what information to display in the timer window title.
        /// </summary>
        public WindowTitleMode WindowTitleMode { get; set; }

        /// <summary>
        /// Gets or sets the size, position, and state of the timer window.
        /// </summary>
        public WindowSizeInfo WindowSize { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user interface should be locked, preventing the user from taking
        /// any action until the timer expires.
        /// </summary>
        public bool LockInterface { get; set; }

        /// <summary>
        /// Returns a <see cref="TimerOptionsInfo"/> for the specified <see cref="TimerOptions"/>.
        /// </summary>
        /// <param name="options">A <see cref="TimerOptions"/>.</param>
        /// <returns>A <see cref="TimerOptionsInfo"/> for the specified <see cref="TimerOptions"/>.</returns>
        public static TimerOptionsInfo FromTimerOptions(TimerOptions options)
        {
            if (options == null)
            {
                return null;
            }

            return options.ToTimerOptionsInfo();
        }
    }
}
