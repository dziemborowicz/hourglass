// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimerMenu.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using System.Windows.Threading;

    /// <summary>
    /// A <see cref="ContextMenu"/> for the <see cref="TimerWindow"/>.
    /// </summary>
    public class TimerMenu : ContextMenu
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
        /// The "Loop timer" <see cref="MenuItem"/>.
        /// </summary>
        private MenuItem loopTimerMenuItem;

        /// <summary>
        /// The "Always on top" <see cref="MenuItem"/>.
        /// </summary>
        private MenuItem alwaysOnTopMenuItem;

        /// <summary>
        /// The "Show in notification area" <see cref="MenuItem"/>.
        /// </summary>
        private MenuItem showInNotificationAreaMenuItem;

        /// <summary>
        /// The "Pop up when expired" <see cref="MenuItem"/>.
        /// </summary>
        private MenuItem popUpWhenExpiredMenuItem;

        /// <summary>
        /// The "Close when expired" <see cref="MenuItem"/>.
        /// </summary>
        private MenuItem closeWhenExpiredMenuItem;

        /// <summary>
        /// The "Recent timers" <see cref="MenuItem"/>.
        /// </summary>
        private MenuItem recentTimersMenuItem;

        /// <summary>
        /// The "Clear recent timers" <see cref="MenuItem"/>.
        /// </summary>
        private MenuItem clearRecentTimersMenuItem;

        /// <summary>
        /// The "Saved timers" <see cref="MenuItem"/>.
        /// </summary>
        private MenuItem savedTimersMenuItem;

        /// <summary>
        /// The "Clear saved timers" <see cref="MenuItem"/>.
        /// </summary>
        private MenuItem clearSavedTimersMenuItem;

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
        /// Binds the <see cref="TimerMenu"/> to a <see cref="TimerWindow"/>.
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

            this.selectableSoundMenuItems = new List<MenuItem>();

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
            this.UpdateRecentTimersMenuItem();
            this.UpdateSavedTimersMenuItem();
            this.UpdateSoundMenuItem();

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

            SettingsManager.Instance.Save();
        }

        #endregion

        #region Private Methods (Binding)

        /// <summary>
        /// Reads the options from the <see cref="TimerOptions"/> and applies them to this menu.
        /// </summary>
        private void UpdateMenuFromOptions()
        {
            if (!(this.timerWindow.Timer is DateTimeTimer))
            {
                this.loopTimerMenuItem.IsEnabled = true;
                this.loopTimerMenuItem.IsChecked = this.timerWindow.Timer.Options.LoopTimer;
            }
            else
            {
                this.loopTimerMenuItem.IsEnabled = false;
                this.loopTimerMenuItem.IsChecked = false;
            }

            this.alwaysOnTopMenuItem.IsChecked = this.timerWindow.Timer.Options.AlwaysOnTop;
            this.showInNotificationAreaMenuItem.IsChecked = this.timerWindow.Timer.Options.ShowInNotificationArea;
            this.popUpWhenExpiredMenuItem.IsChecked = this.timerWindow.Timer.Options.PopUpWhenExpired;
            this.closeWhenExpiredMenuItem.IsChecked = this.timerWindow.Timer.Options.CloseWhenExpired;
            this.loopSoundMenuItem.IsChecked = this.timerWindow.Timer.Options.LoopSound;

            foreach (MenuItem menuItem in this.selectableSoundMenuItems)
            {
                menuItem.IsChecked = menuItem.Tag == this.timerWindow.Timer.Options.Sound;
            }
        }

        /// <summary>
        /// Reads the options from this menu and applies them to the <see cref="TimerOptions"/>.
        /// </summary>
        private void UpdateOptionsFromMenu()
        {
            if (!(this.timerWindow.Timer is DateTimeTimer))
            {
                this.timerWindow.Timer.Options.LoopTimer = this.loopTimerMenuItem.IsChecked;
            }

            this.timerWindow.Timer.Options.AlwaysOnTop = this.alwaysOnTopMenuItem.IsChecked;
            this.timerWindow.Timer.Options.ShowInNotificationArea = this.showInNotificationAreaMenuItem.IsChecked;
            this.timerWindow.Timer.Options.PopUpWhenExpired = this.popUpWhenExpiredMenuItem.IsChecked;
            this.timerWindow.Timer.Options.CloseWhenExpired = this.closeWhenExpiredMenuItem.IsChecked;
            this.timerWindow.Timer.Options.LoopSound = this.loopSoundMenuItem.IsChecked;

            MenuItem selectedSoundMenuItem = this.selectableSoundMenuItems.FirstOrDefault(mi => mi.IsChecked);
            this.timerWindow.Timer.Options.Sound = selectedSoundMenuItem != null ? selectedSoundMenuItem.Tag as Sound : null;
        }

        #endregion

        #region Private Methods (Building)

        /// <summary>
        /// Builds or rebuilds the context menu.
        /// </summary>
        private void BuildMenu()
        {
            this.Items.Clear();

            this.loopTimerMenuItem = new MenuItem();
            this.loopTimerMenuItem.Header = "Loop timer";
            this.loopTimerMenuItem.IsCheckable = true;
            this.Items.Add(this.loopTimerMenuItem);

            this.Items.Add(new Separator());

            this.alwaysOnTopMenuItem = new MenuItem();
            this.alwaysOnTopMenuItem.Header = "Always on top";
            this.alwaysOnTopMenuItem.IsCheckable = true;
            this.Items.Add(this.alwaysOnTopMenuItem);

            this.showInNotificationAreaMenuItem = new MenuItem();
            this.showInNotificationAreaMenuItem.Header = "Show in notification area";
            this.showInNotificationAreaMenuItem.IsCheckable = true;
            this.Items.Add(this.showInNotificationAreaMenuItem);

            this.Items.Add(new Separator());

            this.popUpWhenExpiredMenuItem = new MenuItem();
            this.popUpWhenExpiredMenuItem.Header = "Pop up when expired";
            this.popUpWhenExpiredMenuItem.IsCheckable = true;
            this.Items.Add(this.popUpWhenExpiredMenuItem);

            this.closeWhenExpiredMenuItem = new MenuItem();
            this.closeWhenExpiredMenuItem.Header = "Close when expired";
            this.closeWhenExpiredMenuItem.IsCheckable = true;
            this.Items.Add(this.closeWhenExpiredMenuItem);

            this.Items.Add(new Separator());

            this.recentTimersMenuItem = new MenuItem();
            this.recentTimersMenuItem.Header = "Recent timers";
            this.Items.Add(this.recentTimersMenuItem);

            this.savedTimersMenuItem = new MenuItem();
            this.savedTimersMenuItem.Header = "Saved timers";
            this.Items.Add(this.savedTimersMenuItem);

            this.Items.Add(new Separator());

            this.soundMenuItem = new MenuItem();
            this.soundMenuItem.Header = "Sound";
            this.Items.Add(this.soundMenuItem);

            this.Items.Add(new Separator());

            this.closeMenuItem = new MenuItem();
            this.closeMenuItem.Header = "Close";
            this.closeMenuItem.Click += this.CloseMenuItemClick;
            this.Items.Add(this.closeMenuItem);
        }

        #endregion
        
        #region Private Methods (Recent Timers)

        /// <summary>
        /// Updates the <see cref="recentTimersMenuItem"/>.
        /// </summary>
        private void UpdateRecentTimersMenuItem()
        {
            this.recentTimersMenuItem.Items.Clear();

            if (TimerInputManager.Instance.Inputs.Count == 0)
            {
                MenuItem noRecentTimersMenuItem = new MenuItem();
                noRecentTimersMenuItem.Header = "No recent timers";
                noRecentTimersMenuItem.Foreground = Brushes.DarkGray;

                this.recentTimersMenuItem.Items.Add(noRecentTimersMenuItem);
            }
            else
            {
                foreach (TimerInput input in TimerInputManager.Instance.Inputs)
                {
                    MenuItem timerMenuItem = new MenuItem();
                    timerMenuItem.Header = input.ToString();
                    timerMenuItem.Tag = input;
                    timerMenuItem.Click += this.RecentTimerMenuItemClick;

                    this.recentTimersMenuItem.Items.Add(timerMenuItem);
                }
            }

            this.recentTimersMenuItem.Items.Add(new Separator());

            if (this.clearRecentTimersMenuItem == null)
            {
                this.clearRecentTimersMenuItem = new MenuItem();
                this.clearRecentTimersMenuItem.Header = "Clear recent timers";
                this.clearRecentTimersMenuItem.Click += this.ClearRecentTimersMenuItemClick;
            }

            this.recentTimersMenuItem.Items.Add(this.clearRecentTimersMenuItem);
        }

        /// <summary>
        /// Invoked when a recent <see cref="TimerInput"/> <see cref="MenuItem"/> is clicked.
        /// </summary>
        /// <param name="sender">The <see cref="MenuItem"/> where the event handler is attached.</param>
        /// <param name="e">The event data.</param>
        private void RecentTimerMenuItemClick(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = (MenuItem)sender;
            TimerInput input = (TimerInput)menuItem.Tag;
            input.Options.SetFromTimerOptions(this.timerWindow.Timer.Options);

            TimerWindow window = new TimerWindow();
            window.Show(input);
        }

        /// <summary>
        /// Invoked when the "Clear recent timers" <see cref="MenuItem"/> is clicked.
        /// </summary>
        /// <param name="sender">The <see cref="MenuItem"/> where the event handler is attached.</param>
        /// <param name="e">The event data.</param>
        private void ClearRecentTimersMenuItemClick(object sender, RoutedEventArgs e)
        {
            TimerInputManager.Instance.Clear();
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
                noRunningTimersMenuItem.Header = "No saved timers";
                noRunningTimersMenuItem.Foreground = Brushes.DarkGray;

                this.savedTimersMenuItem.Items.Add(noRunningTimersMenuItem);
            }
            else
            {
                foreach (Timer savedTimer in savedTimers)
                {
                    MenuItem timerMenuItem = new MenuItem();
                    timerMenuItem.Header = savedTimer.ToString();
                    timerMenuItem.Tag = savedTimer;
                    timerMenuItem.Click += this.SavedTimerMenuItemClick;

                    this.savedTimersMenuItem.Items.Add(timerMenuItem);
                }
            }

            this.savedTimersMenuItem.Items.Add(new Separator());

            if (this.clearSavedTimersMenuItem == null)
            {
                this.clearSavedTimersMenuItem = new MenuItem();
                this.clearSavedTimersMenuItem.Header = "Clear saved timers";
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
                    menuItem.Header = timer.ToString();
                }
            }
        }

        /// <summary>
        /// Invoked when a saved <see cref="Timer"/> <see cref="MenuItem"/> is clicked.
        /// </summary>
        /// <param name="sender">The <see cref="MenuItem"/> where the event handler is attached.</param>
        /// <param name="e">The event data.</param>
        private void SavedTimerMenuItemClick(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = (MenuItem)sender;
            HourglassTimer savedTimer = (HourglassTimer)menuItem.Tag;

            TimerWindow window = new TimerWindow();
            window.Show(savedTimer);
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

        #region Private Methods (Sound)

        /// <summary>
        /// Updates the <see cref="soundMenuItem"/>.
        /// </summary>
        private void UpdateSoundMenuItem()
        {
            this.soundMenuItem.Items.Clear();
            this.selectableSoundMenuItems.Clear();

            // No sound
            MenuItem noSoundMenuItem = new MenuItem();
            noSoundMenuItem.Header = "No sound";
            noSoundMenuItem.IsCheckable = true;
            noSoundMenuItem.Click += this.SoundMenuItemClick;

            this.soundMenuItem.Items.Add(noSoundMenuItem);
            this.selectableSoundMenuItems.Add(noSoundMenuItem);

            // Built-in sounds
            if (SoundManager.Instance.BuiltInSounds.Any())
            {
                this.soundMenuItem.Items.Add(new Separator());

                foreach (Sound sound in SoundManager.Instance.BuiltInSounds)
                {
                    MenuItem menuItem = new MenuItem();
                    menuItem.Header = sound.Name;
                    menuItem.Tag = sound;
                    menuItem.IsCheckable = true;
                    menuItem.Click += this.SoundMenuItemClick;

                    this.soundMenuItem.Items.Add(menuItem);
                    this.selectableSoundMenuItems.Add(menuItem);
                }
            }

            // User-provided sounds
            if (SoundManager.Instance.UserProvidedSounds.Any())
            {
                this.soundMenuItem.Items.Add(new Separator());

                foreach (Sound sound in SoundManager.Instance.UserProvidedSounds)
                {
                    MenuItem menuItem = new MenuItem();
                    menuItem.Header = sound.Name;
                    menuItem.Tag = sound;
                    menuItem.IsCheckable = true;
                    menuItem.Click += this.SoundMenuItemClick;

                    this.soundMenuItem.Items.Add(menuItem);
                    this.selectableSoundMenuItems.Add(menuItem);
                }
            }

            // Options
            this.soundMenuItem.Items.Add(new Separator());

            if (this.loopSoundMenuItem == null)
            {
                this.loopSoundMenuItem = new MenuItem();
                this.loopSoundMenuItem.Header = "Loop sound";
                this.loopSoundMenuItem.IsCheckable = true;
            }

            this.soundMenuItem.Items.Add(this.loopSoundMenuItem);
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
