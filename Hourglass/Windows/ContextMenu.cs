// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContextMenu.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass.Windows
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using System.Windows.Threading;

    using Hourglass.Extensions;
    using Hourglass.Managers;
    using Hourglass.Properties;
    using Hourglass.Timing;

    /// <summary>
    /// A <see cref="System.Windows.Controls.ContextMenu"/> for the <see cref="TimerWindow"/>.
    /// </summary>
    public class ContextMenu : System.Windows.Controls.ContextMenu
    {
        #region Private Members

        /// <summary>
        /// The <see cref="TimerWindow"/> that uses this context menu.
        /// </summary>
        private TimerWindow timerWindow;

        /// <summary>
        /// A <see cref="DispatcherTimer"/> used to raise events.
        /// </summary>
        private DispatcherTimer dispatcherTimer;

        /// <summary>
        /// The "New timer" <see cref="MenuItem"/>.
        /// </summary>
        private MenuItem newTimerMenuItem;

        /// <summary>
        /// The "Always on top" <see cref="MenuItem"/>.
        /// </summary>
        private MenuItem alwaysOnTopMenuItem;

        /// <summary>
        /// The "Full screen" <see cref="MenuItem"/>.
        /// </summary>
        private MenuItem fullScreenMenuItem;

        /// <summary>
        /// The "Prompt on exit" <see cref="MenuItem"/>.
        /// </summary>
        private MenuItem promptOnExitMenuItem;

        /// <summary>
        /// The "Show in notification area" <see cref="MenuItem"/>.
        /// </summary>
        private MenuItem showInNotificationAreaMenuItem;

        /// <summary>
        /// The "Loop timer" <see cref="MenuItem"/>.
        /// </summary>
        private MenuItem loopTimerMenuItem;

        /// <summary>
        /// The "Pop up when expired" <see cref="MenuItem"/>.
        /// </summary>
        private MenuItem popUpWhenExpiredMenuItem;

        /// <summary>
        /// The "Close when expired" <see cref="MenuItem"/>.
        /// </summary>
        private MenuItem closeWhenExpiredMenuItem;

        /// <summary>
        /// The "Recent inputs" <see cref="MenuItem"/>.
        /// </summary>
        private MenuItem recentInputsMenuItem;

        /// <summary>
        /// The "Clear recent inputs" <see cref="MenuItem"/>.
        /// </summary>
        private MenuItem clearRecentInputsMenuItem;

        /// <summary>
        /// The "Saved timers" <see cref="MenuItem"/>.
        /// </summary>
        private MenuItem savedTimersMenuItem;

        /// <summary>
        /// The "Open all saved timers" <see cref="MenuItem"/>.
        /// </summary>
        private MenuItem openAllSavedTimersMenuItem;

        /// <summary>
        /// The "Clear saved timers" <see cref="MenuItem"/>.
        /// </summary>
        private MenuItem clearSavedTimersMenuItem;

        /// <summary>
        /// The "Theme" <see cref="MenuItem"/>.
        /// </summary>
        private MenuItem themeMenuItem;

        /// <summary>
        /// The "Light theme" <see cref="MenuItem"/>.
        /// </summary>
        private MenuItem lightThemeMenuItem;

        /// <summary>
        /// The "Dark theme" <see cref="MenuItem"/>.
        /// </summary>
        private MenuItem darkThemeMenuItem;

        /// <summary>
        /// The "Manage themes" <see cref="MenuItem"/>.
        /// </summary>
        private MenuItem manageThemesMenuItem;

        /// <summary>
        /// The "Theme" <see cref="MenuItem"/>s associated with <see cref="Theme"/>s.
        /// </summary>
        private IList<MenuItem> selectableThemeMenuItems;

        /// <summary>
        /// The "Sound" <see cref="MenuItem"/>.
        /// </summary>
        private MenuItem soundMenuItem;

        /// <summary>
        /// The "Sound" <see cref="MenuItem"/>s associated with <see cref="Sound"/>s.
        /// </summary>
        private IList<MenuItem> selectableSoundMenuItems;

        /// <summary>
        /// The "Loop sound" <see cref="MenuItem"/>.
        /// </summary>
        private MenuItem loopSoundMenuItem;

        /// <summary>
        /// The "Advanced options" <see cref="MenuItem"/>.
        /// </summary>
        private MenuItem advancedOptionsMenuItem;

        /// <summary>
        /// The "Do not keep computer awake" <see cref="MenuItem"/>.
        /// </summary>
        private MenuItem doNotKeepComputerAwakeMenuItem;

        /// <summary>
        /// The "Open saved timers on startup" <see cref="MenuItem"/>.
        /// </summary>
        private MenuItem openSavedTimersOnStartupMenuItem;

        /// <summary>
        /// The "Show time elapsed" <see cref="MenuItem"/>.
        /// </summary>
        private MenuItem showTimeElapsedMenuItem;

        /// <summary>
        /// The "Shut down when expired" <see cref="MenuItem"/>.
        /// </summary>
        private MenuItem shutDownWhenExpiredMenuItem;

        /// <summary>
        /// The "Window title" <see cref="MenuItem"/>.
        /// </summary>
        private MenuItem windowTitleMenuItem;

        /// <summary>
        /// The "Application name" window title <see cref="MenuItem"/>.
        /// </summary>
        private MenuItem applicationNameWindowTitleMenuItem;

        /// <summary>
        /// The "Time left" window title <see cref="MenuItem"/>.
        /// </summary>
        private MenuItem timeLeftWindowTitleMenuItem;

        /// <summary>
        /// The "Time elapsed" window title <see cref="MenuItem"/>.
        /// </summary>
        private MenuItem timeElapsedWindowTitleMenuItem;

        /// <summary>
        /// The "Timer title" window title <see cref="MenuItem"/>.
        /// </summary>
        private MenuItem timerTitleWindowTitleMenuItem;

        /// <summary>
        /// The "Timer title + time left" window title <see cref="MenuItem"/>.
        /// </summary>
        private MenuItem timerTitlePlusTimeLeftWindowTitleMenuItem;

        /// <summary>
        /// The "Timer title + time elapsed" window title <see cref="MenuItem"/>.
        /// </summary>
        private MenuItem timerTitlePlusTimeElapsedWindowTitleMenuItem;

        /// <summary>
        /// The "Time left + timer title" window title <see cref="MenuItem"/>.
        /// </summary>
        private MenuItem timeLeftPlusTimerTitleWindowTitleMenuItem;

        /// <summary>
        /// The "Time elapsed + timer title" window title <see cref="MenuItem"/>.
        /// </summary>
        private MenuItem timeElapsedPlusTimerTitleWindowTitleMenuItem;

        /// <summary>
        /// The "Window title" <see cref="MenuItem"/>s associated with <see cref="WindowTitleMode"/>s.
        /// </summary>
        private IList<MenuItem> selectableWindowTitleMenuItems;

        /// <summary>
        /// The "Close" <see cref="MenuItem"/>.
        /// </summary>
        private MenuItem closeMenuItem;

        /// <summary>
        /// The date and time the menu was last visible.
        /// </summary>
        private DateTime lastShowed = DateTime.MinValue;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the date and time the menu was last visible.
        /// </summary>
        public DateTime LastShowed
        {
            get { return this.lastShowed; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Binds the <see cref="ContextMenu"/> to a <see cref="TimerWindow"/>.
        /// </summary>
        /// <param name="window">A <see cref="TimerWindow"/>.</param>
        public void Bind(TimerWindow window)
        {
            // Validate parameters
            if (window == null)
            {
                throw new ArgumentNullException("window");
            }

            // Validate state
            if (this.timerWindow != null)
            {
                throw new InvalidOperationException();
            }

            // Initialize members
            this.timerWindow = window;

            this.timerWindow.ContextMenuOpening += this.WindowContextMenuOpening;
            this.timerWindow.ContextMenuClosing += this.WindowContextMenuClosing;
            this.timerWindow.ContextMenu = this;

            this.dispatcherTimer = new DispatcherTimer(DispatcherPriority.Normal, this.Dispatcher);
            this.dispatcherTimer.Interval = TimeSpan.FromSeconds(1);
            this.dispatcherTimer.Tick += this.DispatcherTimerTick;

            this.selectableThemeMenuItems = new List<MenuItem>();
            this.selectableSoundMenuItems = new List<MenuItem>();
            this.selectableWindowTitleMenuItems = new List<MenuItem>();

            // Build the menu
            this.BuildMenu();
        }

        #endregion

        #region Private Methods (Lifecycle)

        /// <summary>
        /// Invoked when the context menu is opened.
        /// </summary>
        /// <param name="sender">The bound <see cref="TimerWindow"/>.</param>
        /// <param name="e">The event data.</param>
        private void WindowContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            // Update dynamic items
            this.UpdateRecentInputsMenuItem();
            this.UpdateSavedTimersMenuItem();
            this.UpdateThemeMenuItem();
            this.UpdateSoundMenuItem();

            // Update binding
            this.UpdateMenuFromOptions();

            this.lastShowed = DateTime.Now;
            this.dispatcherTimer.Start();
        }

        /// <summary>
        /// Invoked when the <see cref="dispatcherTimer"/> interval has elapsed.
        /// </summary>
        /// <param name="sender">The <see cref="DispatcherTimer"/>.</param>
        /// <param name="e">The event data.</param>
        private void DispatcherTimerTick(object sender, EventArgs e)
        {
            this.lastShowed = DateTime.Now;
            this.UpdateSavedTimersHeaders();
        }

        /// <summary>
        /// Invoked just before the context menu is closed.
        /// </summary>
        /// <param name="sender">The bound <see cref="TimerWindow"/>.</param>
        /// <param name="e">The event data.</param>
        private void WindowContextMenuClosing(object sender, ContextMenuEventArgs e)
        {
            this.UpdateOptionsFromMenu();

            this.lastShowed = DateTime.Now;
            this.dispatcherTimer.Stop();

            AppManager.Instance.Persist();
        }

        #endregion

        #region Private Methods (Binding)

        /// <summary>
        /// Reads the options from the <see cref="TimerOptions"/> and applies them to this menu.
        /// </summary>
        private void UpdateMenuFromOptions()
        {
            // Always on top
            this.alwaysOnTopMenuItem.IsChecked = this.timerWindow.Options.AlwaysOnTop;

            // Full screen
            this.fullScreenMenuItem.IsChecked = this.timerWindow.IsFullScreen;

            // Prompt on exit
            this.promptOnExitMenuItem.IsChecked = this.timerWindow.Options.PromptOnExit;

            // Show in notification area
            this.showInNotificationAreaMenuItem.IsChecked = Settings.Default.ShowInNotificationArea;

            // Loop timer
            if (this.timerWindow.Timer.SupportsLooping)
            {
                this.loopTimerMenuItem.IsEnabled = true;
                this.loopTimerMenuItem.IsChecked = this.timerWindow.Options.LoopTimer;
            }
            else
            {
                this.loopTimerMenuItem.IsEnabled = false;
                this.loopTimerMenuItem.IsChecked = false;
            }

            // Pop up when expired
            this.popUpWhenExpiredMenuItem.IsChecked = this.timerWindow.Options.PopUpWhenExpired;

            // Close when expired
            if ((!this.timerWindow.Options.LoopTimer || !this.timerWindow.Timer.SupportsLooping) && !this.timerWindow.Options.LoopSound)
            {
                this.closeWhenExpiredMenuItem.IsChecked = this.timerWindow.Options.CloseWhenExpired;
                this.closeWhenExpiredMenuItem.IsEnabled = true;
            }
            else
            {
                this.closeWhenExpiredMenuItem.IsChecked = false;
                this.closeWhenExpiredMenuItem.IsEnabled = false;
            }

            // Theme
            foreach (MenuItem menuItem in this.selectableThemeMenuItems)
            {
                Theme menuItemTheme = (Theme)menuItem.Tag;
                menuItem.IsChecked = menuItemTheme == this.timerWindow.Options.Theme;
                if (this.timerWindow.Options.Theme.Type == ThemeType.UserProvided)
                {
                    menuItem.Visibility = menuItemTheme.Type == ThemeType.BuiltInLight || menuItemTheme.Type == ThemeType.UserProvided
                        ? Visibility.Visible
                        : Visibility.Collapsed;
                }
                else
                {
                    menuItem.Visibility = menuItemTheme.Type == this.timerWindow.Options.Theme.Type || menuItemTheme.Type == ThemeType.UserProvided
                        ? Visibility.Visible
                        : Visibility.Collapsed;
                }
            }

            this.lightThemeMenuItem.IsChecked = this.timerWindow.Options.Theme.Type == ThemeType.BuiltInLight;
            this.darkThemeMenuItem.IsChecked = this.timerWindow.Options.Theme.Type == ThemeType.BuiltInDark;

            // Sound
            foreach (MenuItem menuItem in this.selectableSoundMenuItems)
            {
                menuItem.IsChecked = menuItem.Tag == this.timerWindow.Options.Sound;
            }

            // Loop sound
            this.loopSoundMenuItem.IsChecked = this.timerWindow.Options.LoopSound;

            // Do not keep computer awake
            this.doNotKeepComputerAwakeMenuItem.IsChecked = this.timerWindow.Options.DoNotKeepComputerAwake;

            // Open saved timers on startup
            this.openSavedTimersOnStartupMenuItem.IsChecked = Settings.Default.OpenSavedTimersOnStartup;

            // Show time elapsed
            this.showTimeElapsedMenuItem.IsChecked = this.timerWindow.Options.ShowTimeElapsed;

            // Shut down when expired
            if ((!this.timerWindow.Options.LoopTimer || !this.timerWindow.Timer.SupportsLooping) && !this.timerWindow.Options.LoopSound)
            {
                this.shutDownWhenExpiredMenuItem.IsChecked = this.timerWindow.Options.ShutDownWhenExpired;
                this.shutDownWhenExpiredMenuItem.IsEnabled = true;
            }
            else
            {
                this.shutDownWhenExpiredMenuItem.IsChecked = false;
                this.shutDownWhenExpiredMenuItem.IsEnabled = false;
            }

            // Window title
            foreach (MenuItem menuItem in this.selectableWindowTitleMenuItems)
            {
                WindowTitleMode windowTitleMode = (WindowTitleMode)menuItem.Tag;
                menuItem.IsChecked = windowTitleMode == this.timerWindow.Options.WindowTitleMode;
            }
        }

        /// <summary>
        /// Reads the options from this menu and applies them to the <see cref="TimerOptions"/>.
        /// </summary>
        private void UpdateOptionsFromMenu()
        {
            // Always on top
            this.timerWindow.Options.AlwaysOnTop = this.alwaysOnTopMenuItem.IsChecked;

            // Full screen
            this.timerWindow.IsFullScreen = this.fullScreenMenuItem.IsChecked;

            // Prompt on exit
            this.timerWindow.Options.PromptOnExit = this.promptOnExitMenuItem.IsChecked;

            // Show in notification area
            Settings.Default.ShowInNotificationArea = this.showInNotificationAreaMenuItem.IsChecked;

            // Loop timer
            if (this.loopTimerMenuItem.IsEnabled)
            {
                this.timerWindow.Options.LoopTimer = this.loopTimerMenuItem.IsChecked;
            }

            // Pop up when expired
            this.timerWindow.Options.PopUpWhenExpired = this.popUpWhenExpiredMenuItem.IsChecked;

            // Close when expired
            if (this.closeWhenExpiredMenuItem.IsEnabled)
            {
                this.timerWindow.Options.CloseWhenExpired = this.closeWhenExpiredMenuItem.IsChecked;
            }

            // Sound
            MenuItem selectedSoundMenuItem = this.selectableSoundMenuItems.FirstOrDefault(mi => mi.IsChecked);
            this.timerWindow.Options.Sound = selectedSoundMenuItem != null ? selectedSoundMenuItem.Tag as Sound : null;

            // Loop sound
            this.timerWindow.Options.LoopSound = this.loopSoundMenuItem.IsChecked;

            // Do not keep computer awake
            this.timerWindow.Options.DoNotKeepComputerAwake = this.doNotKeepComputerAwakeMenuItem.IsChecked;

            // Open saved timers on startup
            Settings.Default.OpenSavedTimersOnStartup = this.openSavedTimersOnStartupMenuItem.IsChecked;

            // Show time elapsed
            this.timerWindow.Options.ShowTimeElapsed = this.showTimeElapsedMenuItem.IsChecked;

            // Shut down when expired
            if (this.shutDownWhenExpiredMenuItem.IsEnabled)
            {
                this.timerWindow.Options.ShutDownWhenExpired = this.shutDownWhenExpiredMenuItem.IsChecked;
            }

            // Window title
            MenuItem selectedWindowTitleMenuItem = this.selectableWindowTitleMenuItems.FirstOrDefault(mi => mi.IsChecked);
            this.timerWindow.Options.WindowTitleMode = selectedWindowTitleMenuItem != null
                ? (WindowTitleMode)selectedWindowTitleMenuItem.Tag
                : WindowTitleMode.ApplicationName;
        }

        /// <summary>
        /// Invoked when a checkable <see cref="MenuItem"/> is clicked.
        /// </summary>
        /// <param name="sender">The <see cref="MenuItem"/> where the event handler is attached.</param>
        /// <param name="e">The event data.</param>
        private void CheckableMenuItemClick(object sender, RoutedEventArgs e)
        {
            this.UpdateOptionsFromMenu();
            this.UpdateMenuFromOptions();
        }

        #endregion

        #region Private Methods (Building)

        /// <summary>
        /// Builds or rebuilds the context menu.
        /// </summary>
        private void BuildMenu()
        {
            this.Items.Clear();

            // New timer
            this.newTimerMenuItem = new MenuItem();
            this.newTimerMenuItem.Header = Properties.Resources.ContextMenuNewTimerMenuItem;
            this.newTimerMenuItem.Click += this.NewTimerMenuItemClick;
            this.Items.Add(this.newTimerMenuItem);

            this.Items.Add(new Separator());

            // Always on top
            this.alwaysOnTopMenuItem = new MenuItem();
            this.alwaysOnTopMenuItem.Header = Properties.Resources.ContextMenuAlwaysOnTopMenuItem;
            this.alwaysOnTopMenuItem.IsCheckable = true;
            this.alwaysOnTopMenuItem.Click += this.CheckableMenuItemClick;
            this.Items.Add(this.alwaysOnTopMenuItem);

            // Full screen
            this.fullScreenMenuItem = new MenuItem();
            this.fullScreenMenuItem.Header = Properties.Resources.ContextMenuFullScreenMenuItem;
            this.fullScreenMenuItem.IsCheckable = true;
            this.fullScreenMenuItem.Click += this.CheckableMenuItemClick;
            this.Items.Add(this.fullScreenMenuItem);

            // Prompt on exit
            this.promptOnExitMenuItem = new MenuItem();
            this.promptOnExitMenuItem.Header = Properties.Resources.ContextMenuPromptOnExitMenuItem;
            this.promptOnExitMenuItem.IsCheckable = true;
            this.promptOnExitMenuItem.Click += this.CheckableMenuItemClick;
            this.Items.Add(this.promptOnExitMenuItem);

            // Show in notification area
            this.showInNotificationAreaMenuItem = new MenuItem();
            this.showInNotificationAreaMenuItem.Header = Properties.Resources.ContextMenuShowInNotificationAreaMenuItem;
            this.showInNotificationAreaMenuItem.IsCheckable = true;
            this.showInNotificationAreaMenuItem.Click += this.CheckableMenuItemClick;
            this.Items.Add(this.showInNotificationAreaMenuItem);

            this.Items.Add(new Separator());

            // Loop timer
            this.loopTimerMenuItem = new MenuItem();
            this.loopTimerMenuItem.Header = Properties.Resources.ContextMenuLoopTimerMenuItem;
            this.loopTimerMenuItem.IsCheckable = true;
            this.loopTimerMenuItem.Click += this.CheckableMenuItemClick;
            this.Items.Add(this.loopTimerMenuItem);

            // Pop up when expired
            this.popUpWhenExpiredMenuItem = new MenuItem();
            this.popUpWhenExpiredMenuItem.Header = Properties.Resources.ContextMenuPopUpWhenExpiredMenuItem;
            this.popUpWhenExpiredMenuItem.IsCheckable = true;
            this.popUpWhenExpiredMenuItem.Click += this.CheckableMenuItemClick;
            this.Items.Add(this.popUpWhenExpiredMenuItem);

            // Close when expired
            this.closeWhenExpiredMenuItem = new MenuItem();
            this.closeWhenExpiredMenuItem.Header = Properties.Resources.ContextMenuCloseWhenExpiredMenuItem;
            this.closeWhenExpiredMenuItem.IsCheckable = true;
            this.closeWhenExpiredMenuItem.Click += this.CheckableMenuItemClick;
            this.Items.Add(this.closeWhenExpiredMenuItem);

            this.Items.Add(new Separator());

            // Recent inputs
            this.recentInputsMenuItem = new MenuItem();
            this.recentInputsMenuItem.Header = Properties.Resources.ContextMenuRecentInputsMenuItem;
            this.Items.Add(this.recentInputsMenuItem);

            // Saved timers
            this.savedTimersMenuItem = new MenuItem();
            this.savedTimersMenuItem.Header = Properties.Resources.ContextMenuSavedTimersMenuItem;
            this.Items.Add(this.savedTimersMenuItem);

            this.Items.Add(new Separator());

            // Theme
            this.themeMenuItem = new MenuItem();
            this.themeMenuItem.Header = Properties.Resources.ContextMenuThemeMenuItem;
            this.Items.Add(this.themeMenuItem);

            // Sound
            this.soundMenuItem = new MenuItem();
            this.soundMenuItem.Header = Properties.Resources.ContextMenuSoundMenuItem;
            this.Items.Add(this.soundMenuItem);

            Separator separator = new Separator();
            this.Items.Add(separator);

            // Advanced options
            this.advancedOptionsMenuItem = new MenuItem();
            this.advancedOptionsMenuItem.Header = Properties.Resources.ContextMenuAdvancedOptionsMenuItem;
            this.Items.Add(this.advancedOptionsMenuItem);

            // Do not keep computer awake
            this.doNotKeepComputerAwakeMenuItem = new MenuItem();
            this.doNotKeepComputerAwakeMenuItem.Header = Properties.Resources.ContextMenuDoNotKeepComputerAwakeMenuItem;
            this.doNotKeepComputerAwakeMenuItem.IsCheckable = true;
            this.doNotKeepComputerAwakeMenuItem.Click += this.CheckableMenuItemClick;
            this.advancedOptionsMenuItem.Items.Add(this.doNotKeepComputerAwakeMenuItem);

            // Open saved timers on startup
            this.openSavedTimersOnStartupMenuItem = new MenuItem();
            this.openSavedTimersOnStartupMenuItem.Header = Properties.Resources.ContextMenuOpenSavedTimersOnStartupMenuItem;
            this.openSavedTimersOnStartupMenuItem.IsCheckable = true;
            this.openSavedTimersOnStartupMenuItem.Click += this.CheckableMenuItemClick;
            this.advancedOptionsMenuItem.Items.Add(this.openSavedTimersOnStartupMenuItem);

            // Show time elapsed
            this.showTimeElapsedMenuItem = new MenuItem();
            this.showTimeElapsedMenuItem.Header = Properties.Resources.ContextMenuShowTimeElapsedMenuItem;
            this.showTimeElapsedMenuItem.IsCheckable = true;
            this.showTimeElapsedMenuItem.Click += this.CheckableMenuItemClick;
            this.advancedOptionsMenuItem.Items.Add(this.showTimeElapsedMenuItem);

            // Shut down when expired
            this.shutDownWhenExpiredMenuItem = new MenuItem();
            this.shutDownWhenExpiredMenuItem.Header = Properties.Resources.ContextMenuShutDownWhenExpiredMenuItem;
            this.shutDownWhenExpiredMenuItem.IsCheckable = true;
            this.shutDownWhenExpiredMenuItem.Click += this.CheckableMenuItemClick;
            this.advancedOptionsMenuItem.Items.Add(this.shutDownWhenExpiredMenuItem);

            // Window title
            this.windowTitleMenuItem = new MenuItem();
            this.windowTitleMenuItem.Header = Properties.Resources.ContextMenuWindowTitleMenuItem;
            this.advancedOptionsMenuItem.Items.Add(this.windowTitleMenuItem);

            // Application name (window title)
            this.applicationNameWindowTitleMenuItem = new MenuItem();
            this.applicationNameWindowTitleMenuItem.Header = Properties.Resources.ContextMenuApplicationNameWindowTitleMenuItem;
            this.applicationNameWindowTitleMenuItem.IsCheckable = true;
            this.applicationNameWindowTitleMenuItem.Tag = WindowTitleMode.ApplicationName;
            this.applicationNameWindowTitleMenuItem.Click += this.WindowTitleMenuItemClick;
            this.applicationNameWindowTitleMenuItem.Click += this.CheckableMenuItemClick;
            this.windowTitleMenuItem.Items.Add(this.applicationNameWindowTitleMenuItem);
            this.selectableWindowTitleMenuItems.Add(this.applicationNameWindowTitleMenuItem);

            this.windowTitleMenuItem.Items.Add(new Separator());

            // Time left (window title)
            this.timeLeftWindowTitleMenuItem = new MenuItem();
            this.timeLeftWindowTitleMenuItem.Header = Properties.Resources.ContextMenuTimeLeftWindowTitleMenuItem;
            this.timeLeftWindowTitleMenuItem.IsCheckable = true;
            this.timeLeftWindowTitleMenuItem.Tag = WindowTitleMode.TimeLeft;
            this.timeLeftWindowTitleMenuItem.Click += this.WindowTitleMenuItemClick;
            this.timeLeftWindowTitleMenuItem.Click += this.CheckableMenuItemClick;
            this.windowTitleMenuItem.Items.Add(this.timeLeftWindowTitleMenuItem);
            this.selectableWindowTitleMenuItems.Add(this.timeLeftWindowTitleMenuItem);

            // Time elapsed (window title)
            this.timeElapsedWindowTitleMenuItem = new MenuItem();
            this.timeElapsedWindowTitleMenuItem.Header = Properties.Resources.ContextMenuTimeElapsedWindowTitleMenuItem;
            this.timeElapsedWindowTitleMenuItem.IsCheckable = true;
            this.timeElapsedWindowTitleMenuItem.Tag = WindowTitleMode.TimeElapsed;
            this.timeElapsedWindowTitleMenuItem.Click += this.WindowTitleMenuItemClick;
            this.timeElapsedWindowTitleMenuItem.Click += this.CheckableMenuItemClick;
            this.windowTitleMenuItem.Items.Add(this.timeElapsedWindowTitleMenuItem);
            this.selectableWindowTitleMenuItems.Add(this.timeElapsedWindowTitleMenuItem);

            // Timer title (window title)
            this.timerTitleWindowTitleMenuItem = new MenuItem();
            this.timerTitleWindowTitleMenuItem.Header = Properties.Resources.ContextMenuTimerTitleWindowTitleMenuItem;
            this.timerTitleWindowTitleMenuItem.IsCheckable = true;
            this.timerTitleWindowTitleMenuItem.Tag = WindowTitleMode.TimerTitle;
            this.timerTitleWindowTitleMenuItem.Click += this.WindowTitleMenuItemClick;
            this.timerTitleWindowTitleMenuItem.Click += this.CheckableMenuItemClick;
            this.windowTitleMenuItem.Items.Add(this.timerTitleWindowTitleMenuItem);
            this.selectableWindowTitleMenuItems.Add(this.timerTitleWindowTitleMenuItem);

            this.windowTitleMenuItem.Items.Add(new Separator());

            // Time left + timer title (window title)
            this.timeLeftPlusTimerTitleWindowTitleMenuItem = new MenuItem();
            this.timeLeftPlusTimerTitleWindowTitleMenuItem.Header = Properties.Resources.ContextMenuTimeLeftPlusTimerTitleWindowTitleMenuItem;
            this.timeLeftPlusTimerTitleWindowTitleMenuItem.IsCheckable = true;
            this.timeLeftPlusTimerTitleWindowTitleMenuItem.Tag = WindowTitleMode.TimeLeftPlusTimerTitle;
            this.timeLeftPlusTimerTitleWindowTitleMenuItem.Click += this.WindowTitleMenuItemClick;
            this.timeLeftPlusTimerTitleWindowTitleMenuItem.Click += this.CheckableMenuItemClick;
            this.windowTitleMenuItem.Items.Add(this.timeLeftPlusTimerTitleWindowTitleMenuItem);
            this.selectableWindowTitleMenuItems.Add(this.timeLeftPlusTimerTitleWindowTitleMenuItem);

            // Time elapsed + timer title (window title)
            this.timeElapsedPlusTimerTitleWindowTitleMenuItem = new MenuItem();
            this.timeElapsedPlusTimerTitleWindowTitleMenuItem.Header = Properties.Resources.ContextMenuTimeElapsedPlusTimerTitleWindowTitleMenuItem;
            this.timeElapsedPlusTimerTitleWindowTitleMenuItem.IsCheckable = true;
            this.timeElapsedPlusTimerTitleWindowTitleMenuItem.Tag = WindowTitleMode.TimeElapsedPlusTimerTitle;
            this.timeElapsedPlusTimerTitleWindowTitleMenuItem.Click += this.WindowTitleMenuItemClick;
            this.timeElapsedPlusTimerTitleWindowTitleMenuItem.Click += this.CheckableMenuItemClick;
            this.windowTitleMenuItem.Items.Add(this.timeElapsedPlusTimerTitleWindowTitleMenuItem);
            this.selectableWindowTitleMenuItems.Add(this.timeElapsedPlusTimerTitleWindowTitleMenuItem);

            this.windowTitleMenuItem.Items.Add(new Separator());

            // Timer title + time left (window title)
            this.timerTitlePlusTimeLeftWindowTitleMenuItem = new MenuItem();
            this.timerTitlePlusTimeLeftWindowTitleMenuItem.Header = Properties.Resources.ContextMenuTimerTitlePlusTimeLeftWindowTitleMenuItem;
            this.timerTitlePlusTimeLeftWindowTitleMenuItem.IsCheckable = true;
            this.timerTitlePlusTimeLeftWindowTitleMenuItem.Tag = WindowTitleMode.TimerTitlePlusTimeLeft;
            this.timerTitlePlusTimeLeftWindowTitleMenuItem.Click += this.WindowTitleMenuItemClick;
            this.timerTitlePlusTimeLeftWindowTitleMenuItem.Click += this.CheckableMenuItemClick;
            this.windowTitleMenuItem.Items.Add(this.timerTitlePlusTimeLeftWindowTitleMenuItem);
            this.selectableWindowTitleMenuItems.Add(this.timerTitlePlusTimeLeftWindowTitleMenuItem);

            // Timer title + time elapsed (window title)
            this.timerTitlePlusTimeElapsedWindowTitleMenuItem = new MenuItem();
            this.timerTitlePlusTimeElapsedWindowTitleMenuItem.Header = Properties.Resources.ContextMenuTimerTitlePlusTimeElapsedWindowTitleMenuItem;
            this.timerTitlePlusTimeElapsedWindowTitleMenuItem.IsCheckable = true;
            this.timerTitlePlusTimeElapsedWindowTitleMenuItem.Tag = WindowTitleMode.TimerTitlePlusTimeElapsed;
            this.timerTitlePlusTimeElapsedWindowTitleMenuItem.Click += this.WindowTitleMenuItemClick;
            this.timerTitlePlusTimeElapsedWindowTitleMenuItem.Click += this.CheckableMenuItemClick;
            this.windowTitleMenuItem.Items.Add(this.timerTitlePlusTimeElapsedWindowTitleMenuItem);
            this.selectableWindowTitleMenuItems.Add(this.timerTitlePlusTimeElapsedWindowTitleMenuItem);

            this.Items.Add(new Separator());

            // Close
            this.closeMenuItem = new MenuItem();
            this.closeMenuItem.Header = Properties.Resources.ContextMenuCloseMenuItem;
            this.closeMenuItem.Click += this.CloseMenuItemClick;
            this.Items.Add(this.closeMenuItem);
        }

        #endregion

        #region Private Methods (New Timer)

        /// <summary>
        /// Invoked when the "New timer" <see cref="MenuItem"/> is clicked.
        /// </summary>
        /// <param name="sender">The <see cref="MenuItem"/> where the event handler is attached.</param>
        /// <param name="e">The event data.</param>
        private void NewTimerMenuItemClick(object sender, RoutedEventArgs e)
        {
            TimerWindow window = new TimerWindow();
            window.RestoreFromWindow(this.timerWindow);
            window.Show();
        }

        #endregion

        #region Private Methods (Recent Inputs)

        /// <summary>
        /// Updates the <see cref="recentInputsMenuItem"/>.
        /// </summary>
        private void UpdateRecentInputsMenuItem()
        {
            this.recentInputsMenuItem.Items.Clear();

            if (TimerStartManager.Instance.TimerStarts.Count == 0)
            {
                MenuItem noRecentInputsMenuItem = new MenuItem();
                noRecentInputsMenuItem.Header = Properties.Resources.ContextMenuNoRecentInputsMenuItem;
                noRecentInputsMenuItem.Foreground = Brushes.DarkGray;

                this.recentInputsMenuItem.Items.Add(noRecentInputsMenuItem);
            }
            else
            {
                foreach (TimerStart timerStart in TimerStartManager.Instance.TimerStarts)
                {
                    MenuItem timerMenuItem = new MenuItem();
                    timerMenuItem.Header = timerStart.ToString();
                    timerMenuItem.Tag = timerStart;
                    timerMenuItem.Click += this.RecentInputMenuItemClick;

                    this.recentInputsMenuItem.Items.Add(timerMenuItem);
                }
            }

            this.recentInputsMenuItem.Items.Add(new Separator());

            if (this.clearRecentInputsMenuItem == null)
            {
                this.clearRecentInputsMenuItem = new MenuItem();
                this.clearRecentInputsMenuItem.Header = Properties.Resources.ContextMenuClearRecentInputsMenuItem;
                this.clearRecentInputsMenuItem.Click += this.ClearRecentInputsMenuItemClick;
            }

            this.recentInputsMenuItem.Items.Add(this.clearRecentInputsMenuItem);
        }

        /// <summary>
        /// Invoked when a recent <see cref="TimerStart"/> <see cref="MenuItem"/> is clicked.
        /// </summary>
        /// <param name="sender">The <see cref="MenuItem"/> where the event handler is attached.</param>
        /// <param name="e">The event data.</param>
        private void RecentInputMenuItemClick(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = (MenuItem)sender;
            TimerStart timerStart = (TimerStart)menuItem.Tag;

            TimerWindow window;
            if (this.timerWindow.Timer.State == TimerState.Stopped || this.timerWindow.Timer.State == TimerState.Expired)
            {
                window = this.timerWindow;
            }
            else
            {
                window = new TimerWindow();
                window.Options.Set(this.timerWindow.Options);
                window.RestoreFromWindow(this.timerWindow);
            }

            window.Show(timerStart);
        }

        /// <summary>
        /// Invoked when the "Clear recent inputs" <see cref="MenuItem"/> is clicked.
        /// </summary>
        /// <param name="sender">The <see cref="MenuItem"/> where the event handler is attached.</param>
        /// <param name="e">The event data.</param>
        private void ClearRecentInputsMenuItemClick(object sender, RoutedEventArgs e)
        {
            TimerStartManager.Instance.Clear();
        }

        #endregion

        #region Private Methods (Saved Timers)

        /// <summary>
        /// Updates the <see cref="savedTimersMenuItem"/>.
        /// </summary>
        private void UpdateSavedTimersMenuItem()
        {
            this.savedTimersMenuItem.Items.Clear();

            IList<Timer> savedTimers = TimerManager.Instance.ResumableTimers;

            if (savedTimers.Count == 0)
            {
                MenuItem noRunningTimersMenuItem = new MenuItem();
                noRunningTimersMenuItem.Header = Properties.Resources.ContextMenuNoSavedTimersMenuItem;
                noRunningTimersMenuItem.Foreground = Brushes.DarkGray;

                this.savedTimersMenuItem.Items.Add(noRunningTimersMenuItem);
            }
            else
            {
                foreach (Timer savedTimer in savedTimers)
                {
                    savedTimer.Update();

                    MenuItem timerMenuItem = new MenuItem();
                    timerMenuItem.Header = this.GetHeaderForTimer(savedTimer);
                    timerMenuItem.Icon = this.GetIconForTimer(savedTimer);
                    timerMenuItem.Tag = savedTimer;
                    timerMenuItem.Click += this.SavedTimerMenuItemClick;

                    this.savedTimersMenuItem.Items.Add(timerMenuItem);
                }
            }

            this.savedTimersMenuItem.Items.Add(new Separator());

            if (this.openAllSavedTimersMenuItem == null)
            {
                this.openAllSavedTimersMenuItem = new MenuItem();
                this.openAllSavedTimersMenuItem.Header = Properties.Resources.ContextMenuOpenAllSavedTimersMenuItem;
                this.openAllSavedTimersMenuItem.Click += this.OpenAllSavedTimersMenuItemClick;
            }

            this.savedTimersMenuItem.Items.Add(this.openAllSavedTimersMenuItem);

            if (this.clearSavedTimersMenuItem == null)
            {
                this.clearSavedTimersMenuItem = new MenuItem();
                this.clearSavedTimersMenuItem.Header = Properties.Resources.ContextMenuClearSavedTimersMenuItem;
                this.clearSavedTimersMenuItem.Click += this.ClearSavedTimersMenuItemClick;
            }

            this.savedTimersMenuItem.Items.Add(this.clearSavedTimersMenuItem);
        }

        /// <summary>
        /// Updates the <see cref="MenuItem.Header"/> of the items in the <see cref="savedTimersMenuItem"/>.
        /// </summary>
        private void UpdateSavedTimersHeaders()
        {
            foreach (MenuItem menuItem in this.savedTimersMenuItem.Items.OfType<MenuItem>())
            {
                Timer timer = menuItem.Tag as Timer;
                if (timer != null)
                {
                    menuItem.Header = this.GetHeaderForTimer(timer);
                    menuItem.Icon = this.GetIconForTimer(timer);
                }
            }
        }

        /// <summary>
        /// Returns an object that can be set for the <see cref="MenuItem.Header"/> of a <see cref="MenuItem"/> that
        /// displays a <see cref="Timer"/>.
        /// </summary>
        /// <param name="timer">A <see cref="Timer"/>.</param>
        /// <returns>An object that can be set for the <see cref="MenuItem.Header"/>.</returns>
        private object GetHeaderForTimer(Timer timer)
        {
            return timer.ToString();
        }

        /// <summary>
        /// Returns an object that can be set for the <see cref="MenuItem.Icon"/> of a <see cref="MenuItem"/> that
        /// displays a <see cref="Timer"/>.
        /// </summary>
        /// <param name="timer">A <see cref="Timer"/>.</param>
        /// <returns>An object that can be set for the <see cref="MenuItem.Icon"/>.</returns>
        private object GetIconForTimer(Timer timer)
        {
            Border outerBorder = new Border();
            outerBorder.BorderBrush = new SolidColorBrush(Colors.LightGray);
            outerBorder.BorderThickness = new Thickness(1);
            outerBorder.CornerRadius = new CornerRadius(2);
            outerBorder.Width = 16;
            outerBorder.Height = 6;

            if (timer.State == TimerState.Expired)
            {
                Border progress = new Border();
                progress.Background = new SolidColorBrush(Color.FromRgb(199, 80, 80));
                progress.Width = 16;
                progress.Height = 6;

                outerBorder.Child = progress;
            }
            else if (timer.TimeLeftAsPercentage.HasValue)
            {
                Border progress = new Border();
                progress.Background = timer.Options.Theme.ProgressBarBrush;
                progress.HorizontalAlignment = HorizontalAlignment.Left;
                progress.Width = timer.TimeLeftAsPercentage.Value / 100.0 * 16.0;
                progress.Height = 6;

                outerBorder.Child = progress;
            }

            return outerBorder;
        }

        /// <summary>
        /// Invoked when a saved timer <see cref="MenuItem"/> is clicked.
        /// </summary>
        /// <param name="sender">The <see cref="MenuItem"/> where the event handler is attached.</param>
        /// <param name="e">The event data.</param>
        private void SavedTimerMenuItemClick(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = (MenuItem)sender;
            Timer savedTimer = (Timer)menuItem.Tag;
            this.ShowSavedTimer(savedTimer);
        }

        /// <summary>
        /// Invoked when the "Open all saved timers" <see cref="MenuItem"/> is clicked.
        /// </summary>
        /// <param name="sender">The <see cref="MenuItem"/> where the event handler is attached.</param>
        /// <param name="e">The event data.</param>
        private void OpenAllSavedTimersMenuItemClick(object sender, RoutedEventArgs e)
        {
            foreach (Timer savedTimer in TimerManager.Instance.ResumableTimers)
            {
                this.ShowSavedTimer(savedTimer);
            }
        }

        /// <summary>
        /// Shows an existing <see cref="Timer"/>.
        /// </summary>
        /// <param name="savedTimer">An existing <see cref="Timer"/>.</param>
        private void ShowSavedTimer(Timer savedTimer)
        {
            if (this.timerWindow.Timer.State == TimerState.Stopped || this.timerWindow.Timer.State == TimerState.Expired)
            {
                this.ShowSavedTimerInCurrentWindow(savedTimer);
            }
            else
            {
                this.ShowSavedTimerInNewWindow(savedTimer);
            }
        }

        /// <summary>
        /// Shows an existing <see cref="Timer"/> in the current <see cref="TimerWindow"/>.
        /// </summary>
        /// <param name="savedTimer">An existing <see cref="Timer"/>.</param>
        private void ShowSavedTimerInCurrentWindow(Timer savedTimer)
        {
            if (savedTimer.Options.WindowSize != null)
            {
                this.timerWindow.Restore(savedTimer.Options.WindowSize);
            }

            this.timerWindow.Show(savedTimer);
            this.UpdateMenuFromOptions();
        }

        /// <summary>
        /// Shows an existing <see cref="Timer"/> in a new <see cref="TimerWindow"/>.
        /// </summary>
        /// <param name="savedTimer">An existing <see cref="Timer"/>.</param>
        private void ShowSavedTimerInNewWindow(Timer savedTimer)
        {
            TimerWindow newTimerWindow = new TimerWindow();

            if (savedTimer.Options.WindowSize != null)
            {
                newTimerWindow.Restore(savedTimer.Options.WindowSize);
            }
            else
            {
                newTimerWindow.RestoreFromWindow(this.timerWindow);
            }

            newTimerWindow.Show(savedTimer);
        }

        /// <summary>
        /// Invoked when the "Clear saved timers" <see cref="MenuItem"/> is clicked.
        /// </summary>
        /// <param name="sender">The <see cref="MenuItem"/> where the event handler is attached.</param>
        /// <param name="e">The event data.</param>
        private void ClearSavedTimersMenuItemClick(object sender, RoutedEventArgs e)
        {
            TimerManager.Instance.ClearResumableTimers();
        }

        #endregion

        #region Private Methods (Theme)

        /// <summary>
        /// Updates the <see cref="themeMenuItem"/>.
        /// </summary>
        private void UpdateThemeMenuItem()
        {
            this.themeMenuItem.Items.Clear();
            this.selectableThemeMenuItems.Clear();

            // Switch between light and dark themes
            if (this.lightThemeMenuItem == null)
            {
                this.lightThemeMenuItem = new MenuItem();
                this.lightThemeMenuItem.Header = Properties.Resources.ContextMenuLightThemeMenuItem;
                this.lightThemeMenuItem.Tag = ThemeType.BuiltInLight;
                this.lightThemeMenuItem.Click += this.ThemeTypeMenuItemClick;
            }

            this.themeMenuItem.Items.Add(this.lightThemeMenuItem);

            if (this.darkThemeMenuItem == null)
            {
                this.darkThemeMenuItem = new MenuItem();
                this.darkThemeMenuItem.Header = Properties.Resources.ContextMenuDarkThemeMenuItem;
                this.darkThemeMenuItem.Tag = ThemeType.BuiltInDark;
                this.darkThemeMenuItem.Click += this.ThemeTypeMenuItemClick;
            }

            this.themeMenuItem.Items.Add(this.darkThemeMenuItem);

            // Built-in themes
            this.CreateThemeMenuItemsFromList(ThemeManager.Instance.BuiltInThemes);

            // User-provided themes
            if (ThemeManager.Instance.UserProvidedThemes.Count > 0)
            {
                this.CreateThemeMenuItemsFromList(ThemeManager.Instance.UserProvidedThemes);
            }

            // Manage themes
            this.themeMenuItem.Items.Add(new Separator());

            if (this.manageThemesMenuItem == null)
            {
                this.manageThemesMenuItem = new MenuItem();
                this.manageThemesMenuItem.Header = Properties.Resources.ContextMenuManageThemesMenuItem;
                this.manageThemesMenuItem.Click += this.ManageThemesMenuItemClick;
            }

            this.themeMenuItem.Items.Add(this.manageThemesMenuItem);
        }

        /// <summary>
        /// Creates a <see cref="MenuItem"/> for each <see cref="Theme"/> in the collection.
        /// </summary>
        /// <param name="themes">A collection of <see cref="Theme"/>s.</param>
        private void CreateThemeMenuItemsFromList(IList<Theme> themes)
        {
            this.themeMenuItem.Items.Add(new Separator());

            foreach (Theme theme in themes)
            {
                this.CreateThemeMenuItem(theme);
            }
        }

        /// <summary>
        /// Creates a <see cref="MenuItem"/> for a <see cref="Theme"/>.
        /// </summary>
        /// <param name="theme">A <see cref="Theme"/>.</param>
        private void CreateThemeMenuItem(Theme theme)
        {
            MenuItem menuItem = new MenuItem();
            menuItem.Header = this.GetHeaderForTheme(theme);
            menuItem.Tag = theme;
            menuItem.IsCheckable = true;
            menuItem.Click += this.ThemeMenuItemClick;
            menuItem.Click += this.CheckableMenuItemClick;

            this.themeMenuItem.Items.Add(menuItem);
            this.selectableThemeMenuItems.Add(menuItem);
        }

        /// <summary>
        /// Returns an object that can be set for the <see cref="MenuItem.Header"/> of a <see cref="MenuItem"/> that
        /// displays a <see cref="Theme"/>.
        /// </summary>
        /// <param name="theme">A <see cref="Theme"/>.</param>
        /// <returns>An object that can be set for the <see cref="MenuItem.Header"/>.</returns>
        private object GetHeaderForTheme(Theme theme)
        {
            Border border = new Border();
            border.Background = theme.ProgressBarBrush;
            border.CornerRadius = new CornerRadius(2);
            border.Width = 8;
            border.Height = 8;

            TextBlock textBlock = new TextBlock();
            textBlock.Text = theme.Name ?? Properties.Resources.ContextMenuUnnamedTheme;
            textBlock.Margin = new Thickness(5, 0, 0, 0);

            StackPanel stackPanel = new StackPanel();
            stackPanel.Orientation = Orientation.Horizontal;
            stackPanel.Children.Add(border);
            stackPanel.Children.Add(textBlock);
            return stackPanel;
        }

        /// <summary>
        /// Invoked when a theme type <see cref="MenuItem"/> is clicked.
        /// </summary>
        /// <param name="sender">The <see cref="MenuItem"/> where the event handler is attached.</param>
        /// <param name="e">The event data.</param>
        private void ThemeTypeMenuItemClick(object sender, RoutedEventArgs e)
        {
            MenuItem clickedMenuItem = (MenuItem)sender;
            ThemeType type = (ThemeType)clickedMenuItem.Tag;

            if (type == ThemeType.BuiltInDark)
            {
                this.timerWindow.Options.Theme = this.timerWindow.Options.Theme.DarkVariant;
            }
            else
            {
                this.timerWindow.Options.Theme = this.timerWindow.Options.Theme.LightVariant;
            }
        }

        /// <summary>
        /// Invoked when a theme <see cref="MenuItem"/> is clicked.
        /// </summary>
        /// <param name="sender">The <see cref="MenuItem"/> where the event handler is attached.</param>
        /// <param name="e">The event data.</param>
        private void ThemeMenuItemClick(object sender, RoutedEventArgs e)
        {
            foreach (MenuItem menuItem in this.selectableThemeMenuItems)
            {
                menuItem.IsChecked = object.ReferenceEquals(menuItem, sender);
            }

            MenuItem selectedMenuItem = (MenuItem)sender;
            this.timerWindow.Options.Theme = (Theme)selectedMenuItem.Tag;
        }

        /// <summary>
        /// Invoked when the "Manage themes" <see cref="MenuItem"/> is clicked.
        /// </summary>
        /// <param name="sender">The <see cref="MenuItem"/> where the event handler is attached.</param>
        /// <param name="e">The event data.</param>
        private void ManageThemesMenuItemClick(object sender, RoutedEventArgs e)
        {
            ThemeManagerWindow window = Application.Current.Windows.OfType<ThemeManagerWindow>().FirstOrDefault();
            if (window != null)
            {
                window.SetTimerWindow(this.timerWindow);
                window.BringToFrontAndActivate();
            }
            else
            {
                window = new ThemeManagerWindow(this.timerWindow);
                window.Show();
            }
        }

        #endregion

        #region Private Methods (Sound)

        /// <summary>
        /// Updates the <see cref="soundMenuItem"/>.
        /// </summary>
        private void UpdateSoundMenuItem()
        {
            this.soundMenuItem.Items.Clear();
            this.selectableSoundMenuItems.Clear();

            // Sounds
            this.CreateSoundMenuItem(Sound.NoSound);
            this.CreateSoundMenuItemsFromList(SoundManager.Instance.BuiltInSounds);
            this.CreateSoundMenuItemsFromList(SoundManager.Instance.UserProvidedSounds);

            // Options
            this.soundMenuItem.Items.Add(new Separator());

            if (this.loopSoundMenuItem == null)
            {
                this.loopSoundMenuItem = new MenuItem();
                this.loopSoundMenuItem.Header = Properties.Resources.ContextMenuLoopSoundMenuItem;
                this.loopSoundMenuItem.IsCheckable = true;
                this.loopSoundMenuItem.Click += this.CheckableMenuItemClick;
            }

            this.soundMenuItem.Items.Add(this.loopSoundMenuItem);
        }

        /// <summary>
        /// Creates a <see cref="MenuItem"/> for a <see cref="Sound"/>.
        /// </summary>
        /// <param name="sound">A <see cref="Sound"/>.</param>
        private void CreateSoundMenuItem(Sound sound)
        {
            MenuItem menuItem = new MenuItem();
            menuItem.Header = sound != null ? sound.Name : Properties.Resources.ContextMenuNoSoundMenuItem;
            menuItem.Tag = sound;
            menuItem.IsCheckable = true;
            menuItem.Click += this.SoundMenuItemClick;
            menuItem.Click += this.CheckableMenuItemClick;

            this.soundMenuItem.Items.Add(menuItem);
            this.selectableSoundMenuItems.Add(menuItem);
        }

        /// <summary>
        /// Creates a <see cref="MenuItem"/> for each <see cref="Sound"/> in the collection.
        /// </summary>
        /// <param name="sounds">A collection of <see cref="Sound"/>s.</param>
        private void CreateSoundMenuItemsFromList(IList<Sound> sounds)
        {
            if (sounds.Count > 0)
            {
                this.soundMenuItem.Items.Add(new Separator());
                foreach (Sound sound in sounds)
                {
                    this.CreateSoundMenuItem(sound);
                }
            }
        }

        /// <summary>
        /// Invoked when a sound <see cref="MenuItem"/> is clicked.
        /// </summary>
        /// <param name="sender">The <see cref="MenuItem"/> where the event handler is attached.</param>
        /// <param name="e">The event data.</param>
        private void SoundMenuItemClick(object sender, RoutedEventArgs e)
        {
            foreach (MenuItem menuItem in this.selectableSoundMenuItems)
            {
                menuItem.IsChecked = object.ReferenceEquals(menuItem, sender);
            }
        }

        #endregion

        #region Private Methods (Window Title)

        /// <summary>
        /// Invoked when a window title <see cref="MenuItem"/> is clicked.
        /// </summary>
        /// <param name="sender">The <see cref="MenuItem"/> where the event handler is attached.</param>
        /// <param name="e">The event data.</param>
        private void WindowTitleMenuItemClick(object sender, RoutedEventArgs e)
        {
            foreach (MenuItem menuItem in this.selectableWindowTitleMenuItems)
            {
                menuItem.IsChecked = object.ReferenceEquals(menuItem, sender);
            }
        }

        #endregion

        #region Private Methods (Close)

        /// <summary>
        /// Invoked when the "Close" <see cref="MenuItem"/> is clicked.
        /// </summary>
        /// <param name="sender">The <see cref="MenuItem"/> where the event handler is attached.</param>
        /// <param name="e">The event data.</param>
        private void CloseMenuItemClick(object sender, RoutedEventArgs e)
        {
            this.timerWindow.Close();
        }

        #endregion
    }
}
