// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ThemeManagerWindow.xaml.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass.Windows
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;

    using Hourglass.Extensions;
    using Hourglass.Managers;
    using Hourglass.Timing;

    /// <summary>
    /// The state of the window used to manage themes.
    /// </summary>
    public enum ThemeManagerWindowState
    {
        /// <summary>
        /// The window is displaying a built-in theme that cannot be edited.
        /// </summary>
        BuiltInTheme,

        /// <summary>
        /// The window is displaying a user-provided theme that has not yet been edited.
        /// </summary>
        UserThemeUnedited,

        /// <summary>
        /// The window is displaying a user-provided theme that has been edited.
        /// </summary>
        UserThemeEdited
    }

    /// <summary>
    /// The window used to manage themes.
    /// </summary>
    public partial class ThemeManagerWindow
    {
        /// <summary>
        /// The state of the window.
        /// </summary>
        private ThemeManagerWindowState state;

        /// <summary>
        /// The <see cref="TimerWindow"/> that will be updated when a theme is selected in this window.
        /// </summary>
        private TimerWindow timerWindow;

        /// <summary>
        /// The currently selected theme.
        /// </summary>
        private Theme selectedTheme;

        /// <summary>
        /// A copy of the currently selected theme. The changes to this copy are applied to the selected theme when the
        /// user saves their changes.
        /// </summary>
        private Theme editedTheme;

        /// <summary>
        /// Initializes a new instance of the <see cref="ThemeManagerWindow"/> class.
        /// </summary>
        /// <param name="timerWindow">The <see cref="TimerWindow"/> to edit the theme for.</param>
        public ThemeManagerWindow(TimerWindow timerWindow)
        {
            this.InitializeComponent();
            this.BindThemesComboBox();

            this.TimerWindow = timerWindow;
        }

        /// <summary>
        /// Gets the state of the window.
        /// </summary>
        public ThemeManagerWindowState State
        {
            get
            {
                return this.state;
            }

            private set
            {
                this.state = value;
                this.BindState();
            }
        }

        /// <summary>
        /// Gets the <see cref="TimerWindow"/> that will be updated when a theme is selected in this window.
        /// </summary>
        public TimerWindow TimerWindow
        {
            get
            {
                return this.timerWindow;
            }

            private set
            {
                this.timerWindow = value;
                this.BindTimerWindow();
            }
        }

        /// <summary>
        /// Gets the currently selected theme.
        /// </summary>
        public Theme SelectedTheme
        {
            get
            {
                return this.selectedTheme;
            }

            private set
            {
                this.selectedTheme = value;
                this.BindSelectedTheme();
            }
        }

        /// <summary>
        /// Gets or sets a copy of the currently selected theme. The changes to this copy are applied to the selected
        /// theme when the user saves their changes.
        /// </summary>
        private Theme EditedTheme
        {
            get
            {
                return this.editedTheme;
            }

            set
            {
                this.editedTheme = value;
                this.DataContext = this.editedTheme;
            }
        }

        /// <summary>
        /// Brings the window to the front.
        /// </summary>
        /// <returns><c>true</c> if the window is brought to the foreground, or <c>false</c> if the window cannot be
        /// brought to the foreground for any reason.</returns>
        public bool BringToFront()
        {
            try
            {
                this.Show();
                this.Topmost = true;
                this.Topmost = false;
                return true;
            }
            catch (InvalidOperationException)
            {
                // This happens if the window is closing when this method is called
                return false;
            }
        }

        /// <summary>
        /// Brings the window to the front, activates it, and focusses it.
        /// </summary>
        public void BringToFrontAndActivate()
        {
            this.BringToFront();
            this.Activate();
        }

        /// <summary>
        /// Sets the <see cref="TimerWindow"/> that will be updated when a theme is selected in this window.
        /// </summary>
        /// <param name="newTimerWindow">The <see cref="TimerWindow"/> to set.</param>
        /// <returns><c>true</c> if the <see cref="TimerWindow"/> was set, or <c>false</c> if the user canceled the
        /// change because there were unsaved changes to the selected theme.</returns>
        public bool SetTimerWindow(TimerWindow newTimerWindow)
        {
            if (!this.PromptToSaveIfRequired())
            {
                return false;
            }

            this.TimerWindow = newTimerWindow;
            return true;
        }

        /// <summary>
        /// Removes focus from all controls.
        /// </summary>
        /// <returns>A value indicating whether the focus was removed from any element.</returns>
        private bool UnfocusAll()
        {
            bool unfocused = this.ThemesComboBox.Unfocus()
                || this.NewButton.Unfocus()
                || this.NameTextBox.Unfocus()
                || this.DeleteButton.Unfocus()
                || this.ColorsGrid.Unfocus()
                || this.SaveButton.Unfocus()
                || this.CancelButton.Unfocus()
                || this.CloseButton.Unfocus();

            foreach (ColorControl control in this.ColorsGrid.GetAllVisualChildren().OfType<ColorControl>())
            {
                unfocused |= control.Unfocus();
            }

            return unfocused;
        }

        /// <summary>
        /// Binds the <see cref="TimerWindow"/> to the controls.
        /// </summary>
        private void BindTimerWindow()
        {
            if (this.timerWindow.Theme.Type == ThemeType.UserProvided)
            {
                this.EditedTheme = this.CloneThemeForEditing(this.timerWindow.Theme);
                this.SelectedTheme = this.timerWindow.Theme;
                this.State = ThemeManagerWindowState.UserThemeUnedited;
            }
            else
            {
                this.EditedTheme = this.timerWindow.Theme;
                this.SelectedTheme = this.timerWindow.Theme;
                this.State = ThemeManagerWindowState.BuiltInTheme;
            }
        }

        /// <summary>
        /// Binds the selected theme to the <see cref="ThemesComboBox"/>.
        /// </summary>
        private void BindSelectedTheme()
        {
            for (int i = 0; i < this.ThemesComboBox.Items.Count; i++)
            {
                ComboBoxItem item = (ComboBoxItem)this.ThemesComboBox.Items[i];
                Theme theme = (Theme)item.Tag;
                if (theme != null && this.SelectedTheme != null && theme.Identifier == this.SelectedTheme.Identifier)
                {
                    this.ThemesComboBox.SelectedIndex = i;
                    break;
                }
            }
        }

        /// <summary>
        /// Binds the <see cref="ThemeManagerWindowState"/> to the controls.
        /// </summary>
        private void BindState()
        {
            switch (this.state)
            {
                case ThemeManagerWindowState.BuiltInTheme:
                    this.NameTextBox.IsEnabled = false;
                    this.DeleteButton.IsEnabled = false;

                    foreach (ColorControl control in this.ColorsGrid.GetAllVisualChildren().OfType<ColorControl>())
                    {
                        control.IsEnabled = false;
                    }

                    this.SaveButton.Visibility = Visibility.Collapsed;
                    this.CancelButton.Visibility = Visibility.Collapsed;
                    this.CloseButton.Visibility = Visibility.Visible;
                    break;

                case ThemeManagerWindowState.UserThemeUnedited:
                    this.NameTextBox.IsEnabled = true;
                    this.DeleteButton.IsEnabled = true;

                    foreach (ColorControl control in this.ColorsGrid.GetAllVisualChildren().OfType<ColorControl>())
                    {
                        control.IsEnabled = true;
                    }

                    this.SaveButton.Visibility = Visibility.Collapsed;
                    this.CancelButton.Visibility = Visibility.Collapsed;
                    this.CloseButton.Visibility = Visibility.Visible;
                    break;

                case ThemeManagerWindowState.UserThemeEdited:
                    this.NameTextBox.IsEnabled = true;
                    this.DeleteButton.IsEnabled = true;

                    foreach (ColorControl control in this.ColorsGrid.GetAllVisualChildren().OfType<ColorControl>())
                    {
                        control.IsEnabled = true;
                    }

                    this.SaveButton.Visibility = Visibility.Visible;
                    this.CancelButton.Visibility = Visibility.Visible;
                    this.CloseButton.Visibility = Visibility.Collapsed;
                    break;
            }
        }

        /// <summary>
        /// Binds the themes from the <see cref="ThemeManager"/> to the <see cref="ThemesComboBox"/>.
        /// </summary>
        private void BindThemesComboBox()
        {
            this.ThemesComboBox.Items.Clear();

            this.AddThemesToComboBox(
                Properties.Resources.ThemeManagerWindowLightThemesSectionHeader,
                ThemeManager.Instance.BuiltInLightThemes);

            this.AddThemesToComboBox(
                Properties.Resources.ThemeManagerWindowDarkThemesSectionHeader,
                ThemeManager.Instance.BuiltInDarkThemes);

            this.AddThemesToComboBox(
                Properties.Resources.ThemeManagerWindowUserProvidedThemesSectionHeader,
                ThemeManager.Instance.UserProvidedThemes);

            this.BindSelectedTheme();
        }

        /// <summary>
        /// Adds the specified themes to the <see cref="ThemesComboBox"/>.
        /// </summary>
        /// <param name="title">The section title.</param>
        /// <param name="themes">The themes to add to the <see cref="ThemesComboBox"/>.</param>
        private void AddThemesToComboBox(string title, IList<Theme> themes)
        {
            if (themes.Count == 0)
            {
                return;
            }

            // Spacing between sections
            if (this.ThemesComboBox.Items.Count > 0)
            {
                this.ThemesComboBox.Items.Add(new ComboBoxItem { IsEnabled = false });
            }

            // Section header
            this.ThemesComboBox.Items.Add(new ComboBoxItem
            {
                Content = title,
                IsEnabled = false,
                FontStyle = FontStyles.Italic,
                FontWeight = FontWeights.Bold
            });

            // Themes in section
            foreach (Theme theme in themes)
            {
                ComboBoxItem item = new ComboBoxItem();
                item.Content = theme.Name;
                item.Tag = theme;
                this.ThemesComboBox.Items.Add(item);
            }
        }

        /// <summary>
        /// Prompts the user to save unsaved changes to the selected theme, if there are any.
        /// </summary>
        /// <returns><c>true</c> if the theme has been saved or the user has elected to discard unsaved changes, or
        /// <c>false</c> if the user has elected to cancel the operation.</returns>
        private bool PromptToSaveIfRequired()
        {
            if (this.State == ThemeManagerWindowState.UserThemeEdited)
            {
                MessageBoxResult result = MessageBox.Show(
                    this /* owner */,
                    Properties.Resources.ThemeManagerWindowSavePrompt,
                    Properties.Resources.MessageBoxTitle,
                    MessageBoxButton.YesNoCancel,
                    MessageBoxImage.Question);

                switch (result)
                {
                    case MessageBoxResult.Yes:
                        this.SaveChanges();
                        return true;

                    case MessageBoxResult.No:
                        return true;

                    case MessageBoxResult.Cancel:
                        return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Saves changes to the currently selected theme.
        /// </summary>
        private void SaveChanges()
        {
            if (this.State == ThemeManagerWindowState.UserThemeEdited)
            {
                this.SelectedTheme.Set(this.EditedTheme);
                this.State = ThemeManagerWindowState.UserThemeUnedited;
                this.BindThemesComboBox();
                this.UnfocusAll();
            }
        }

        /// <summary>
        /// Reverts changes to the currently selected theme.
        /// </summary>
        private void RevertChanges()
        {
            if (this.State == ThemeManagerWindowState.UserThemeEdited)
            {
                this.EditedTheme = this.CloneThemeForEditing(this.SelectedTheme);
                this.State = ThemeManagerWindowState.UserThemeUnedited;
                this.UnfocusAll();
            }
        }

        /// <summary>
        /// Clones a theme. This creates a clone of an existing <see cref="Theme"/>, but with a new identifier and a
        /// <see cref="ThemeType.UserProvided"/> type.
        /// </summary>
        /// <param name="theme">A <see cref="Theme"/>.</param>
        /// <returns>The cloned theme.</returns>
        private Theme CloneThemeForEditing(Theme theme)
        {
            string identifier = Guid.NewGuid().ToString();
            return Theme.FromTheme(ThemeType.UserProvided, identifier, theme.Name, theme);
        }

        /// <summary>
        /// Invoked when the selection in the <see cref="ThemesComboBox"/> changes.
        /// </summary>
        /// <param name="sender">The <see cref="ThemesComboBox"/>.</param>
        /// <param name="e">The event data.</param>
        private void ThemesComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem selectedItem = (ComboBoxItem)this.ThemesComboBox.SelectedItem;
            if (selectedItem != null)
            {
                Theme newSelectedTheme = (Theme)selectedItem.Tag;
                if (newSelectedTheme.Identifier != this.selectedTheme.Identifier)
                {
                    if (this.PromptToSaveIfRequired())
                    {
                        this.timerWindow.Options.Theme = newSelectedTheme;
                        this.BindTimerWindow();
                    }
                    else
                    {
                        // Revert the selection
                        this.BindSelectedTheme();
                    }
                }
            }
        }

        /// <summary>
        /// Invoked when the <see cref="NewThemeButton"/> is clicked.
        /// </summary>
        /// <param name="sender">The <see cref="NewThemeButton"/>.</param>
        /// <param name="e">The event data.</param>
        private void NewButtonClick(object sender, RoutedEventArgs e)
        {
            if (!this.PromptToSaveIfRequired())
            {
                return;
            }

            this.timerWindow.Options.Theme = ThemeManager.Instance.AddThemeBasedOnTheme(this.SelectedTheme);
            this.BindThemesComboBox();
            this.BindTimerWindow();
        }

        /// <summary>
        /// Invoked when the <see cref="TextBox.Text"/> property value changes in the <see cref="NameTextBox"/>.
        /// </summary>
        /// <param name="sender">The <see cref="NameTextBox"/>.</param>
        /// <param name="e">The event data.</param>
        private void NameTextBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.State == ThemeManagerWindowState.UserThemeUnedited && this.EditedTheme.Name != this.SelectedTheme.Name)
            {
                this.State = ThemeManagerWindowState.UserThemeEdited;
            }
        }

        /// <summary>
        /// Invoked when the <see cref="DeleteThemeButton"/> is clicked.
        /// </summary>
        /// <param name="sender">The <see cref="DeleteThemeButton"/>.</param>
        /// <param name="e">The event data.</param>
        private void DeleteButtonClick(object sender, RoutedEventArgs e)
        {
            if (this.SelectedTheme.Type == ThemeType.UserProvided)
            {
                MessageBoxResult result = MessageBox.Show(
                    this /* owner */,
                    Properties.Resources.ThemeManagerWindowDeletePrompt,
                    Properties.Resources.MessageBoxTitle,
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    ThemeManager.Instance.Remove(this.SelectedTheme);
                    this.BindThemesComboBox();
                    this.BindTimerWindow();
                }
            }
        }

        /// <summary>
        /// Invoked when the <see cref="ColorControl.Color"/> property changes in a <see cref="ColorControl"/>.
        /// </summary>
        /// <param name="sender">The <see cref="ColorControl"/>.</param>
        /// <param name="e">The event data.</param>
        private void ColorControlColorChanged(object sender, EventArgs e)
        {
            if (this.State == ThemeManagerWindowState.UserThemeUnedited)
            {
                this.State = ThemeManagerWindowState.UserThemeEdited;
            }
        }

        /// <summary>
        /// Invoked when the <see cref="SaveButton"/> is clicked.
        /// </summary>
        /// <param name="sender">The <see cref="SaveButton"/>.</param>
        /// <param name="e">The event data.</param>
        private void SaveButtonClick(object sender, RoutedEventArgs e)
        {
            this.SaveChanges();
        }

        /// <summary>
        /// Invoked when the <see cref="CancelButton"/> is clicked.
        /// </summary>
        /// <param name="sender">The <see cref="CancelButton"/>.</param>
        /// <param name="e">The event data.</param>
        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            this.RevertChanges();
        }

        /// <summary>
        /// Invoked when the <see cref="CloseButton"/> is clicked.
        /// </summary>
        /// <param name="sender">The <see cref="CloseButton"/>.</param>
        /// <param name="e">The event data.</param>
        private void CloseButtonClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Invoked directly after <see cref="Window.Close"/> is called, and can be handled to cancel window closure.
        /// </summary>
        /// <param name="sender">The <see cref="ThemeManagerWindow"/>.</param>
        /// <param name="e">The event data.</param>
        private void WindowClosing(object sender, CancelEventArgs e)
        {
            e.Cancel = !this.PromptToSaveIfRequired();
        }
    }
}
