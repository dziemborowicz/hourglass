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
        /// <summary>
        /// The <see cref="TimerWindow"/> that uses this context menu.
        /// </summary>
        private TimerWindow timerWindow;

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
        /// The "No sound" <see cref="MenuItem"/>.
        /// </summary>
        private MenuItem noSoundMenuItem;

        /// <summary>
        /// The "Loop sound" <see cref="MenuItem"/>.
        /// </summary>
        private MenuItem loopSoundMenuItem;

        /// <summary>
        /// The "Close" <see cref="MenuItem"/>.
        /// </summary>
        private MenuItem closeMenuItem;

        /// <summary>
        /// A <see cref="DispatcherTimer"/> used to raise events.
        /// </summary>
        private DispatcherTimer ticker;

        /// <summary>
        /// A value indicating whether the <see cref="Menu"/> is currently updating.
        /// </summary>
        private bool updating;

        /// <summary>
        /// Initializes a new instance of the <see cref="TimerMenu"/> class.
        /// </summary>
        public TimerMenu()
        {
            this.BuildMenu();
        }

        #region Private Methods (Binding)

        /// <summary>
        /// Binds the context menu to a <see cref="TimerWindow"/>.
        /// </summary>
        /// <param name="window">A <see cref="TimerWindow"/>.</param>
        /// <exception cref="InvalidOperationException">If this method is invoked more than once.</exception>
        public void Bind(TimerWindow window)
        {
            if (this.timerWindow != null)
            {
                throw new InvalidOperationException("Bind can be called only once for each TimerMenu object.");
            }

            // Bind the timer and window
            this.timerWindow = window;
            this.timerWindow.ContextMenu = this;
            this.timerWindow.ContextMenuOpening += this.WindowContextMenuOpening;
            this.timerWindow.ContextMenuClosing += this.WindowContextMenuClosing;

            this.ticker = new DispatcherTimer(DispatcherPriority.Normal, this.Dispatcher);
            this.ticker.Interval = new TimeSpan(0, 0, 0, 0, 500 /* ms */);
            this.ticker.Tick += this.DispatcherTimerTick;
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
            this.UpdateMenu();

            this.ticker.Start();
        }

        /// <summary>
        /// Invoked when the <see cref="ticker"/> interval has elapsed.
        /// </summary>
        /// <param name="sender">The <see cref="DispatcherTimer"/>.</param>
        /// <param name="e">The event data.</param>
        private void DispatcherTimerTick(object sender, EventArgs e)
        {
            this.UpdateSavedTimersHeaders();
        }

        /// <summary>
        /// Invoked just before the context menu is closed.
        /// </summary>
        /// <param name="sender">The bound <see cref="TimerWindow"/>.</param>
        /// <param name="e">The event data.</param>
        private void WindowContextMenuClosing(object sender, ContextMenuEventArgs e)
        {
            this.ticker.Stop();
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
            this.loopTimerMenuItem.Checked += this.OptionMenuItemChanged;
            this.loopTimerMenuItem.Unchecked += this.OptionMenuItemChanged;
            this.Items.Add(this.loopTimerMenuItem);

            this.Items.Add(new Separator());

            this.alwaysOnTopMenuItem = new MenuItem();
            this.alwaysOnTopMenuItem.Header = "Always on top";
            this.alwaysOnTopMenuItem.IsCheckable = true;
            this.alwaysOnTopMenuItem.Checked += this.OptionMenuItemChanged;
            this.alwaysOnTopMenuItem.Unchecked += this.OptionMenuItemChanged;
            this.Items.Add(this.alwaysOnTopMenuItem);

            this.showInNotificationAreaMenuItem = new MenuItem();
            this.showInNotificationAreaMenuItem.Header = "Show in notification area";
            this.showInNotificationAreaMenuItem.IsCheckable = true;
            this.showInNotificationAreaMenuItem.Checked += this.OptionMenuItemChanged;
            this.showInNotificationAreaMenuItem.Unchecked += this.OptionMenuItemChanged;
            this.Items.Add(this.showInNotificationAreaMenuItem);

            this.Items.Add(new Separator());

            this.popUpWhenExpiredMenuItem = new MenuItem();
            this.popUpWhenExpiredMenuItem.Header = "Pop up when expired";
            this.popUpWhenExpiredMenuItem.IsCheckable = true;
            this.popUpWhenExpiredMenuItem.Checked += this.OptionMenuItemChanged;
            this.popUpWhenExpiredMenuItem.Unchecked += this.OptionMenuItemChanged;
            this.Items.Add(this.popUpWhenExpiredMenuItem);

            this.closeWhenExpiredMenuItem = new MenuItem();
            this.closeWhenExpiredMenuItem.Header = "Close when expired";
            this.closeWhenExpiredMenuItem.IsCheckable = true;
            this.closeWhenExpiredMenuItem.Checked += this.OptionMenuItemChanged;
            this.closeWhenExpiredMenuItem.Unchecked += this.OptionMenuItemChanged;
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

        /// <summary>
        /// Updates the context menu before display.
        /// </summary>
        private void UpdateMenu()
        {
            this.updating = true;

            this.UpdateOptions();
            this.UpdateRecentTimersMenuItem();
            this.UpdateSavedTimersMenuItem();
            this.UpdateSoundMenuItem();

            this.updating = false;
        }

        #endregion

        #region Private Methods (Options)

        /// <summary>
        /// Updates the checkable option <see cref="MenuItem"/>s.
        /// </summary>
        private void UpdateOptions()
        {
            this.loopTimerMenuItem.IsChecked = this.timerWindow.Timer.LoopTimer;
            this.alwaysOnTopMenuItem.IsChecked = this.timerWindow.Timer.AlwaysOnTop;
            this.showInNotificationAreaMenuItem.IsChecked = this.timerWindow.Timer.ShowInNotificationArea;
            this.popUpWhenExpiredMenuItem.IsChecked = this.timerWindow.Timer.PopUpWhenExpired;
            this.closeWhenExpiredMenuItem.IsChecked = this.timerWindow.Timer.CloseWhenExpired;
        }

        /// <summary>
        /// Invoked when a checkable option <see cref="MenuItem"/> is checked or unchecked.
        /// </summary>
        /// <param name="sender">The <see cref="MenuItem"/> where the event handler is attached.</param>
        /// <param name="e">The event data.</param>
        private void OptionMenuItemChanged(object sender, RoutedEventArgs e)
        {
            if (this.updating)
            {
                return;
            }

            this.timerWindow.Timer.LoopTimer = this.loopTimerMenuItem.IsChecked;
            this.timerWindow.Timer.AlwaysOnTop = this.alwaysOnTopMenuItem.IsChecked;
            this.timerWindow.Timer.ShowInNotificationArea = this.showInNotificationAreaMenuItem.IsChecked;
            this.timerWindow.Timer.PopUpWhenExpired = this.popUpWhenExpiredMenuItem.IsChecked;
            this.timerWindow.Timer.CloseWhenExpired = this.closeWhenExpiredMenuItem.IsChecked;
        }

        #endregion

        #region Private Methods (Recent Timers)

        /// <summary>
        /// Updates the <see cref="recentTimersMenuItem"/>.
        /// </summary>
        private void UpdateRecentTimersMenuItem()
        {
            this.recentTimersMenuItem.Items.Clear();

            if (TimerManager.Instance.Inputs.Count == 0)
            {
                MenuItem noRecentTimersMenuItem = new MenuItem();
                noRecentTimersMenuItem.Header = "No recent timers";
                noRecentTimersMenuItem.Foreground = Brushes.DarkGray;
                this.recentTimersMenuItem.Items.Add(noRecentTimersMenuItem);
            }
            else
            {
                foreach (TimerInput input in TimerManager.Instance.Inputs)
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

            Timer recentTimer = new Timer(input);
            TimerWindow window = new TimerWindow(recentTimer);
            window.Show();
        }

        /// <summary>
        /// Invoked when the "Clear recent timers" <see cref="MenuItem"/> is clicked.
        /// </summary>
        /// <param name="sender">The <see cref="MenuItem"/> where the event handler is attached.</param>
        /// <param name="e">The event data.</param>
        private void ClearRecentTimersMenuItemClick(object sender, RoutedEventArgs e)
        {
            TimerManager.Instance.ClearInputs();
        }

        #endregion

        #region Private Methods (Saved Timers)

        /// <summary>
        /// Updates the <see cref="savedTimersMenuItem"/>.
        /// </summary>
        private void UpdateSavedTimersMenuItem()
        {
            this.savedTimersMenuItem.Items.Clear();

            IList<Timer> savedTimers = this.GetSavedTimers();

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
                    timerMenuItem.Header = this.GetMenuHeaderForTimer(savedTimer);
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
                    menuItem.Header = this.GetMenuHeaderForTimer(timer);
                }
            }
        }

        /// <summary>
        /// Returns a list of saved <see cref="Timer"/> objects.
        /// </summary>
        /// <returns>A list of saved <see cref="Timer"/> objects.</returns>
        private IList<Timer> GetSavedTimers()
        {
            return TimerManager.Instance.Timers.Where(t => t.TimerWindow == null && t.State != TimerState.Stopped).Take(TimerManager.MaxSavedTimers).ToList();
        }

        /// <summary>
        /// Returns the header string for a specified <see cref="Timer"/>.
        /// </summary>
        /// <param name="savedTimer">A <see cref="Timer"/>.</param>
        /// <returns>The header string for a specified <see cref="Timer"/>.</returns>
        private string GetMenuHeaderForTimer(Timer savedTimer)
        {
            savedTimer.Update();

            string format = string.Empty;
            switch (savedTimer.State)
            {
                case TimerState.Running:
                    format = "{0} \u2794 {1}";
                    break;

                case TimerState.Paused:
                    format = "{0} \u2794 {1} (Paused)";
                    break;

                case TimerState.Expired:
                    format = "{1} (Expired)";
                    break;
            }

            string timeLeft = TimeSpanUtility.ToNaturalString(savedTimer.TimeLeft);

            string target = savedTimer.TotalTime.HasValue
                ? TimeSpanUtility.ToShortNaturalString(savedTimer.TotalTime)
                : DateTimeUtility.ToNaturalString(savedTimer.EndTime);

            return string.Format(format, timeLeft, target);
        }

        /// <summary>
        /// Invoked when a saved <see cref="Timer"/> <see cref="MenuItem"/> is clicked.
        /// </summary>
        /// <param name="sender">The <see cref="MenuItem"/> where the event handler is attached.</param>
        /// <param name="e">The event data.</param>
        private void SavedTimerMenuItemClick(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = (MenuItem)sender;
            Timer savedTimer = (Timer)menuItem.Tag;
            savedTimer.Update();

            if (savedTimer.TimerWindow != null || savedTimer.State == TimerState.Stopped || savedTimer.State == TimerState.Expired)
            {
                return;
            }

            TimerWindow window = new TimerWindow(savedTimer);
            window.Show();
        }

        /// <summary>
        /// Invoked when the "Clear saved timers" <see cref="MenuItem"/> is clicked.
        /// </summary>
        /// <param name="sender">The <see cref="MenuItem"/> where the event handler is attached.</param>
        /// <param name="e">The event data.</param>
        private void ClearSavedTimersMenuItemClick(object sender, RoutedEventArgs e)
        {
            IList<Timer> savedTimers = this.GetSavedTimers();
            TimerManager.Instance.Remove(savedTimers);
        }

        #endregion

        #region Private Methods (Sound)

        /// <summary>
        /// Updates the <see cref="soundMenuItem"/>.
        /// </summary>
        private void UpdateSoundMenuItem()
        {
            this.soundMenuItem.Items.Clear();

            if (this.noSoundMenuItem == null)
            {
                this.noSoundMenuItem = new MenuItem();
                this.noSoundMenuItem.Header = "No sound";
                this.noSoundMenuItem.IsCheckable = true;
            }

            this.soundMenuItem.Items.Add(this.noSoundMenuItem);

            if (SoundManager.Instance.ResourceSounds.Any())
            {
                this.soundMenuItem.Items.Add(new Separator());

                foreach (Sound sound in SoundManager.Instance.ResourceSounds)
                {
                    MenuItem menuItem = new MenuItem();
                    menuItem.Header = sound.Name;
                    menuItem.IsCheckable = true;
                    this.soundMenuItem.Items.Add(menuItem);
                }
            }

            if (SoundManager.Instance.FileSounds.Any())
            {
                this.soundMenuItem.Items.Add(new Separator());

                foreach (Sound sound in SoundManager.Instance.FileSounds)
                {
                    MenuItem menuItem = new MenuItem();
                    menuItem.Header = sound.Name;
                    menuItem.IsCheckable = true;
                    this.soundMenuItem.Items.Add(menuItem);
                }
            }

            this.soundMenuItem.Items.Add(new Separator());

            if (this.loopSoundMenuItem == null)
            {
                this.loopSoundMenuItem = new MenuItem();
                this.loopSoundMenuItem.Header = "Loop sound";
                this.loopSoundMenuItem.IsCheckable = true;
            }

            this.soundMenuItem.Items.Add(this.loopSoundMenuItem);
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
