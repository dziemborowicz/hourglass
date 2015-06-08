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
            this.InitializeMaxSize();
        }

        /// <summary>
        /// Opens the window and returns only when the window is closed.
        /// </summary>
        /// <param name="message">An error message.</param>
        /// <param name="details">Details of the error. (Optional.)</param>
        public void ShowDialog(string message, string details = null)
        {
            this.MessageTextBlock.Text = message;
            this.DetailsTextBox.Text = details ?? string.Empty;
            this.DetailsButton.IsEnabled = !string.IsNullOrEmpty(details);

            this.ShowDialog();
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
        /// Invoked when the <see cref="DetailsButton"/> is clicked.
        /// </summary>
        /// <param name="sender">The <see cref="DetailsButton"/>.</param>
        /// <param name="e">The event data.</param>
        private void DetailsButtonClick(object sender, RoutedEventArgs e)
        {
            if (this.DetailsBorder.Visibility != Visibility.Visible)
            {
                this.DetailsBorder.Visibility = Visibility.Visible;
                this.DetailsButton.IsEnabled = false;
            }
        }
    }
}
