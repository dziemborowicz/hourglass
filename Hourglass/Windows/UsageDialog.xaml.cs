// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UsageDialog.xaml.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass.Windows
{
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Navigation;

    /// <summary>
    /// A window that displays command-line usage.
    /// </summary>
    public partial class UsageDialog
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UsageDialog"/> class.
        /// </summary>
        public UsageDialog()
        {
            this.InitializeComponent();
            this.InitializeMaxSize();
        }

        /// <summary>
        /// Gets or sets an optional error message to be displayed.
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Initializes the <see cref="Window.MaxWidth"/> and <see cref="Window.MaxHeight"/> properties.
        /// </summary>
        private void InitializeMaxSize()
        {
            this.MaxWidth = 0.75 * SystemParameters.WorkArea.Width;
            this.MaxHeight = 0.75 * SystemParameters.WorkArea.Height;
        }

        /// <summary>
        /// Invoked when the window is laid out, rendered, and ready for interaction.
        /// </summary>
        /// <param name="sender">The window.</param>
        /// <param name="e">The event data.</param>
        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(this.ErrorMessage))
            {
                this.MessageTextBlock.Background = new SolidColorBrush(Color.FromRgb(199, 80, 80));
                this.MessageTextBlock.Text = this.ErrorMessage;
            }
            else
            {
                this.MessageTextBlock.Background = Brushes.Gray;
                this.MessageTextBlock.Text = Properties.Resources.UsageDialogDefaultMessageText;
            }

            this.Activate();
        }

        /// <summary>
        /// Invoked when the "About Hourglass" hyperlink is clicked.
        /// </summary>
        /// <param name="sender">The "About Hourglass" hyperlink.</param>
        /// <param name="e">The event data.</param>
        private void AboutHourglassHyperlinkClick(object sender, RoutedEventArgs e)
        {
            AboutDialog.ShowOrActivate();
        }

        /// <summary>
        /// Invoked when the close button is clicked.
        /// </summary>
        /// <param name="sender">The close button.</param>
        /// <param name="e">The event data.</param>
        private void CloseButtonClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
