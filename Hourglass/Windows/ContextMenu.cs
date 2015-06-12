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

    using Color = Hourglass.Timing.Color;
    using ColorDialog = System.Windows.Forms.ColorDialog;
    using DialogResult = System.Windows.Forms.DialogResult;

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
        /// The "Clear saved timers" <see cref="MenuItem"/>.
        /// </summary>
        private MenuItem clearSavedTimersMenuItem;

        /// <summary>
        /// The "Color" <see cref="MenuItem"/>.
        /// </summary>
        private MenuItem colorMenuItem;

        /// <summary>
        /// The "Color" <see cref="MenuItem"/>s associated with <see cref="Color"/>s.
        /// </summary>
        private IList<MenuItem> selectableColorMenuItems;

        /// <summary>
        /// The "Add custom color..." <see cref="MenuItem"/>.
        /// </summary>
        private MenuItem addCustomColorMenuItem;

        /// <summary>
        /// The "Clear custom colors" <see cref="MenuItem"/>.
        /// </summary>
        private MenuItem clearCustomColorsMenuItem;

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

            this.selectableColorMenuItems = new List<MenuItem>();
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
            this.UpdateRecentInputsMenuItem();
            this.UpdateSavedTimersMenuItem();
            this.UpdateColorMenuItem();
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

            // Color
            foreach (MenuItem menuItem in this.selectableColorMenuItems)
            {
                menuItem.IsChecked = object.Equals(menuItem.Tag, this.timerWindow.Options.Color);
            }

            // Sound
            foreach (MenuItem menuItem in this.selectableSoundMenuItems)
            {
                menuItem.IsChecked = menuItem.Tag == this.timerWindow.Options.Sound;
            }

            // Loop sound
            this.loopSoundMenuItem.IsChecked = this.timerWindow.Options.LoopSound;
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

            // Color
            MenuItem selectedColorMenuItem = this.selectableColorMenuItems.FirstOrDefault(mi => mi.IsChecked);
            this.timerWindow.Options.Color = selectedColorMenuItem != null ? selectedColorMenuItem.Tag as Color : Color.DefaultColor;

            // Sound
            MenuItem selectedSoundMenuItem = this.selectableSoundMenuItems.FirstOrDefault(mi => mi.IsChecked);
            this.timerWindow.Options.Sound = selectedSoundMenuItem != null ? selectedSoundMenuItem.Tag as Sound : null;

            // Loop sound
            this.timerWindow.Options.LoopSound = this.loopSoundMenuItem.IsChecked;
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

            this.newTimerMenuItem = new MenuItem();
            this.newTimerMenuItem.Header = "New timer";
            this.newTimerMenuItem.Click += this.NewTimerMenuItemClick;
            this.Items.Add(this.newTimerMenuItem);

            this.Items.Add(new Separator());

            this.alwaysOnTopMenuItem = new MenuItem();
            this.alwaysOnTopMenuItem.Header = "Always on top";
            this.alwaysOnTopMenuItem.IsCheckable = true;
            this.alwaysOnTopMenuItem.Click += this.CheckableMenuItemClick;
            this.Items.Add(this.alwaysOnTopMenuItem);

            this.fullScreenMenuItem = new MenuItem();
            this.fullScreenMenuItem.Header = "Full screen";
            this.fullScreenMenuItem.IsCheckable = true;
            this.fullScreenMenuItem.Click += this.CheckableMenuItemClick;
            this.Items.Add(this.fullScreenMenuItem);

            this.showInNotificationAreaMenuItem = new MenuItem();
            this.showInNotificationAreaMenuItem.Header = "Show in notification area";
            this.showInNotificationAreaMenuItem.IsCheckable = true;
            this.showInNotificationAreaMenuItem.Click += this.CheckableMenuItemClick;
            this.Items.Add(this.showInNotificationAreaMenuItem);

            this.Items.Add(new Separator());

            this.loopTimerMenuItem = new MenuItem();
            this.loopTimerMenuItem.Header = "Loop timer";
            this.loopTimerMenuItem.IsCheckable = true;
            this.loopTimerMenuItem.Click += this.CheckableMenuItemClick;
            this.Items.Add(this.loopTimerMenuItem);

            this.popUpWhenExpiredMenuItem = new MenuItem();
            this.popUpWhenExpiredMenuItem.Header = "Pop up when expired";
            this.popUpWhenExpiredMenuItem.IsCheckable = true;
            this.popUpWhenExpiredMenuItem.Click += this.CheckableMenuItemClick;
            this.Items.Add(this.popUpWhenExpiredMenuItem);

            this.closeWhenExpiredMenuItem = new MenuItem();
            this.closeWhenExpiredMenuItem.Header = "Close when expired";
            this.closeWhenExpiredMenuItem.IsCheckable = true;
            this.closeWhenExpiredMenuItem.Click += this.CheckableMenuItemClick;
            this.Items.Add(this.closeWhenExpiredMenuItem);

            this.Items.Add(new Separator());

            this.recentInputsMenuItem = new MenuItem();
            this.recentInputsMenuItem.Header = "Recent inputs";
            this.Items.Add(this.recentInputsMenuItem);

            this.savedTimersMenuItem = new MenuItem();
            this.savedTimersMenuItem.Header = "Saved timers";
            this.Items.Add(this.savedTimersMenuItem);

            this.Items.Add(new Separator());

            this.colorMenuItem = new MenuItem();
            this.colorMenuItem.Header = "Color";
            this.Items.Add(this.colorMenuItem);

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
                noRecentInputsMenuItem.Header = "No recent inputs";
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
                this.clearRecentInputsMenuItem.Header = "Clear recent inputs";
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
                noRunningTimersMenuItem.Header = "No saved timers";
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
                progress.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(199, 80, 80));
                progress.Width = 16;
                progress.Height = 6;

                outerBorder.Child = progress;
            }
            else if (timer.TimeLeftAsPercentage.HasValue)
            {
                Border progress = new Border();
                progress.Background = timer.Options.Color.Brush;
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

        #region Private Methods (Color)

        /// <summary>
        /// Updates the <see cref="colorMenuItem"/>.
        /// </summary>
        private void UpdateColorMenuItem()
        {
            this.colorMenuItem.Items.Clear();
            this.selectableColorMenuItems.Clear();

            // Ensure the current timer color is registered
            if (!this.timerWindow.Options.Color.IsBuiltIn)
            {
                ColorManager.Instance.Add(this.timerWindow.Options.Color);
            }

            // Colors
            this.CreateColorMenuItem(Color.DefaultColor);
            this.CreateColorMenuItemsFromList(ColorManager.Instance.BuiltInColors.Where(c => c != Color.DefaultColor).ToList());
            this.CreateColorMenuItemsFromList(ColorManager.Instance.UserProvidedColors);

            // Custom color actions
            this.colorMenuItem.Items.Add(new Separator());

            if (this.addCustomColorMenuItem == null)
            {
                this.addCustomColorMenuItem = new MenuItem();
                this.addCustomColorMenuItem.Header = "Add custom color...";
                this.addCustomColorMenuItem.Click += this.AddCustomColorMenuItemClick;
            }

            this.colorMenuItem.Items.Add(this.addCustomColorMenuItem);

            if (this.clearCustomColorsMenuItem == null)
            {
                this.clearCustomColorsMenuItem = new MenuItem();
                this.clearCustomColorsMenuItem.Header = "Clear custom colors";
                this.clearCustomColorsMenuItem.Click += this.ClearCustomColorsMenuItemClick;
            }

            this.colorMenuItem.Items.Add(this.clearCustomColorsMenuItem);
        }

        /// <summary>
        /// Creates a <see cref="MenuItem"/> for a <see cref="Color"/>.
        /// </summary>
        /// <param name="color">A <see cref="Color"/>.</param>
        private void CreateColorMenuItem(Color color)
        {
            MenuItem menuItem = new MenuItem();
            menuItem.Header = this.GetHeaderForColor(color);
            menuItem.Tag = color;
            menuItem.IsCheckable = true;
            menuItem.Click += this.ColorMenuItemClick;
            menuItem.Click += this.CheckableMenuItemClick;

            this.colorMenuItem.Items.Add(menuItem);
            this.selectableColorMenuItems.Add(menuItem);
        }

        /// <summary>
        /// Creates a <see cref="MenuItem"/> for each <see cref="Color"/> in the collection.
        /// </summary>
        /// <param name="colors">A collection of <see cref="Color"/>s.</param>
        private void CreateColorMenuItemsFromList(IList<Color> colors)
        {
            if (colors.Count > 0)
            {
                this.colorMenuItem.Items.Add(new Separator());
                foreach (Color color in colors)
                {
                    this.CreateColorMenuItem(color);
                }
            }
        }

        /// <summary>
        /// Returns an object that can be set for the <see cref="MenuItem.Header"/> of a <see cref="MenuItem"/> that
        /// displays a <see cref="Color"/>.
        /// </summary>
        /// <param name="color">A <see cref="Color"/>.</param>
        /// <returns>An object that can be set for the <see cref="MenuItem.Header"/>.</returns>
        private object GetHeaderForColor(Color color)
        {
            Border border = new Border();
            border.Background = color.Brush;
            border.CornerRadius = new CornerRadius(2);
            border.Width = 8;
            border.Height = 8;

            TextBlock textBlock = new TextBlock();
            textBlock.Text = color.Name ?? "Custom color";
            textBlock.Margin = new Thickness(5, 0, 0, 0);

            StackPanel stackPanel = new StackPanel();
            stackPanel.Orientation = Orientation.Horizontal;
            stackPanel.Children.Add(border);
            stackPanel.Children.Add(textBlock);
            return stackPanel;
        }

        /// <summary>
        /// Invoked when a color <see cref="MenuItem"/> is clicked.
        /// </summary>
        /// <param name="sender">The <see cref="MenuItem"/> where the event handler is attached.</param>
        /// <param name="e">The event data.</param>
        private void ColorMenuItemClick(object sender, RoutedEventArgs e)
        {
            foreach (MenuItem menuItem in this.selectableColorMenuItems)
            {
                menuItem.IsChecked = object.ReferenceEquals(menuItem, sender);
            }
        }

        /// <summary>
        /// Invoked when the <see cref="addCustomColorMenuItem"/> is clicked.
        /// </summary>
        /// <param name="sender">The <see cref="MenuItem"/> where the event handler is attached.</param>
        /// <param name="e">The event data.</param>
        private void AddCustomColorMenuItemClick(object sender, RoutedEventArgs e)
        {
            ColorDialog dialog = new ColorDialog();
            dialog.AnyColor = true;
            dialog.FullOpen = true;
            dialog.CustomColors = ColorManager.Instance.AllColors
                .Where(c => c != Color.DefaultColor)
                .Select(c => c.ToInt())
                .ToArray();

            DialogResult result = dialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                Color color = new Color(dialog.Color.R, dialog.Color.G, dialog.Color.B);
                ColorManager.Instance.Add(color);
                this.timerWindow.Options.Color = color;
            }
        }

        /// <summary>
        /// Invoked when the <see cref="clearCustomColorsMenuItem"/> is clicked.
        /// </summary>
        /// <param name="sender">The <see cref="MenuItem"/> where the event handler is attached.</param>
        /// <param name="e">The event data.</param>
        private void ClearCustomColorsMenuItemClick(object sender, RoutedEventArgs e)
        {
            ColorManager.Instance.ClearUserProvidedColors();
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
                this.loopSoundMenuItem.Header = "Loop sound";
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
            menuItem.Header = sound != null ? sound.Name : "No sound";
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
