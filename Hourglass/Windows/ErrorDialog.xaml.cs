// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ErrorDialog.xaml.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass.Windows
{
    using System.Windows;

    /// <summary>
    /// A window that displays an error.
    /// </summary>
    public partial class ErrorDialog
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorDialog"/> class.
        /// </summary>
        public ErrorDialog()
        {
            this.InitializeComponent();
            this.InitializeResources();
            this.InitializeMaxSize();
        }

        /// <summary>
        /// Opens the window and returns only when the window is closed.
        /// </summary>
        /// <param name="title">The title for the error dialog..</param>
        /// <param name="message">The error message to show. (Optional.)</param>
        /// <param name="details">Details of the error, such as a call stack. (Optional.)</param>
        public void ShowDialog(string title, string message = null, string details = null)
        {
            this.TitleTextBlock.Text = title;

            this.MessageTextBox.Text = message ?? string.Empty;
            this.MessageBorder.Visibility = string.IsNullOrEmpty(message) ? Visibility.Collapsed : Visibility.Visible;

            this.DetailsTextBox.Text = details ?? string.Empty;
            this.ShowDetailsButton.IsEnabled = !string.IsNullOrEmpty(details);

            this.ShowDialog();
        }

        /// <summary>
        /// Initializes localized resources.
        /// </summary>
        private void InitializeResources()
        {
            this.Title = Properties.Resources.ErrorDialogTitle;
            this.TitleTextBlock.Text = Properties.Resources.ErrorDialogDefaultMessageText;
            this.ShowDetailsButton.Content = Properties.Resources.ErrorDialogShowDetailsButtonContent;
            this.CloseButton.Content = Properties.Resources.ErrorDialogCloseButtonContent;
        }

        /// <summary>
        /// Initializes the <see cref="Window.MaxWidth"/> and <see cref="Window.MaxHeight"/> properties.
        /// </summary>
        private void InitializeMaxSize()
        {
            this.MaxWidth = 0.75 * SystemParameters.WorkArea.Width;
            this.MaxHeight = 0.75 * SystemParameters.WorkArea.Height;
        }

        /// <summary>
        /// Invoked when the <see cref="ShowDetailsButton"/> is clicked.
        /// </summary>
        /// <param name="sender">The <see cref="ShowDetailsButton"/>.</param>
        /// <param name="e">The event data.</param>
        private void ShowDetailsButtonClick(object sender, RoutedEventArgs e)
        {
            if (this.DetailsBorder.Visibility != Visibility.Visible)
            {
                this.DetailsBorder.Visibility = Visibility.Visible;
                this.ShowDetailsButton.IsEnabled = false;
            }
        }
    }
}
