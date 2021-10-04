// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AboutDialog.xaml.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass.Windows
{
    using System;
    using System.Diagnostics;
    using System.Windows;
    using System.Windows.Navigation;

    using Hourglass.Managers;

    /// <summary>
    /// A window that displays information about the app.
    /// </summary>
    public partial class AboutDialog
    {
        /// <summary>
        /// The instance of the <see cref="AboutDialog"/> that is showing, or null if there is no instance showing.
        /// </summary>
        private static AboutDialog instance;

        /// <summary>
        /// Initializes a new instance of the <see cref="AboutDialog"/> class.
        /// </summary>
        public AboutDialog()
        {
            this.InitializeComponent();
            this.InitializeMaxSize();
        }

        /// <summary>
        /// A string describing the app's copyright.
        /// </summary>
        public static string Copyright
        {
            get
            {
                // Finds the copyright line in the license string. Perhaps overkill, but doing it this way means there's
                // one less place to update the copyright notice.
                string[] lines = License.Split('\r', '\n');
                foreach (string line in lines)
                {
                    if (line.StartsWith("Copyright"))
                    {
                        return line;
                    }
                }

                throw new Exception("Could not find copyright line in license.");
            }
        }

        /// <summary>
        /// A string containing the app's license.
        /// </summary>
        public static string License => Properties.Resources.License;

        /// <summary>
        /// A string describing the app's version.
        /// </summary>
        public static string Version
        {
            get
            {
                Version version = UpdateManager.Instance.CurrentVersion;

                if (version.Revision != 0)
                {
                    return version.ToString();
                }
                else if (version.Build != 0)
                {
                    return version.ToString(3 /* fieldCount */);
                }
                else
                {
                    return version.ToString(2 /* fieldCount */);
                }
            }
        }

        /// <summary>
        /// Shows or activates the <see cref="AboutDialog"/>. Call this method instead of the constructor to prevent
        /// multiple instances of the dialog.
        /// </summary>
        public static void ShowOrActivate()
        {
            if (AboutDialog.instance == null)
            {
                AboutDialog.instance = new AboutDialog();
                AboutDialog.instance.Show();
            }
            else
            {
                AboutDialog.instance.Activate();
            }
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
        /// Invoked when the about dialog is closed.
        /// </summary>
        /// <param name="sender">The about dialog.</param>
        /// <param name="e">The event data.</param>
        private void AboutDialogClosed(object sender, EventArgs e)
        {
            AboutDialog.instance = null;
        }

        /// <summary>
        /// Invoked when navigation events are requested.
        /// </summary>
        /// <param name="sender">The hyperlink requesting navigation.</param>
        /// <param name="e">The event data.</param>
        private void HyperlinkRequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            if (e.Uri.Scheme != "https")
            {
                throw new ArgumentException();
            }

            Process.Start(e.Uri.ToString());
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
