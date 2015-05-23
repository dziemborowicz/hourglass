// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UsageWindow.xaml.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass
{
    using System.Windows;

    /// <summary>
    /// A window that displays command-line usage.
    /// </summary>
    public partial class UsageWindow
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UsageWindow"/> class.
        /// </summary>
        public UsageWindow()
        {
            this.InitializeComponent();
            this.InitializeMaxSize();
        }

        /// <summary>
        /// Initializes the <see cref="Window.MaxWidth"/> and <see cref="Window.MaxHeight"/> properties.
        /// </summary>
        private void InitializeMaxSize()
        {
            this.MaxWidth = 0.75 * SystemParameters.WorkArea.Width;
            this.MaxHeight = 0.75 * SystemParameters.WorkArea.Height;
        }
    }
}
