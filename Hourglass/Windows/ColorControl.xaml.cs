// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ColorControl.xaml.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass.Windows
{
    using System;
    using System.Linq;
    using System.Windows;
    using System.Windows.Forms;
    using System.Windows.Media;

    using Hourglass.Extensions;
    using Hourglass.Timing;

    /// <summary>
    /// A control for displaying and selecting a <see cref="Color"/>.
    /// </summary>
    public partial class ColorControl
    {
        /// <summary>
        /// A <see cref="DependencyProperty"/> that specifies the text label.
        /// </summary>
        public static readonly DependencyProperty TextProperty
            = DependencyProperty.Register("Text", typeof(string), typeof(ColorControl));

        /// <summary>
        /// A <see cref="DependencyProperty"/> that specifies the color.
        /// </summary>
        public static readonly DependencyProperty ColorProperty
            = DependencyProperty.Register("Color", typeof(Color), typeof(ColorControl));

        /// <summary>
        /// A <see cref="DependencyProperty"/> that specifies the theme to which the color belongs.
        /// </summary>
        public static readonly DependencyProperty ThemeProperty
            = DependencyProperty.Register("Theme", typeof(Theme), typeof(ColorControl));

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorControl"/> class.
        /// </summary>
        public ColorControl()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Occurs when the <see cref="Color"/> property changes.
        /// </summary>
        public event EventHandler ColorChanged;

        /// <summary>
        /// Gets or sets the text label.
        /// </summary>
        public string Text
        {
            get { return (string)this.GetValue(TextProperty); }
            set { this.SetValue(TextProperty, value); }
        }

        /// <summary>
        /// Gets or sets the color.
        /// </summary>
        public Color Color
        {
            get { return (Color)this.GetValue(ColorProperty); }
            set { this.SetValue(ColorProperty, value); }
        }

        /// <summary>
        /// Gets or sets the theme to which the color belongs.
        /// </summary>
        public Theme Theme
        {
            get { return (Theme)this.GetValue(ThemeProperty); }
            set { this.SetValue(ThemeProperty, value); }
        }

        /// <summary>
        /// Invoked when the <see cref="Button"/> is clicked.
        /// </summary>
        /// <param name="sender">The <see cref="Button"/>.</param>
        /// <param name="e">The event data.</param>
        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            ColorDialog dialog = new ColorDialog { AnyColor = true, FullOpen = true };

            if (this.Theme != null)
            {
                dialog.CustomColors = this.Theme.GetPalette().Select(c => c.ToInt()).ToArray();
            }

            DialogResult result = dialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                this.Color = Color.FromRgb(dialog.Color.R, dialog.Color.G, dialog.Color.B);
                this.ColorChanged?.Invoke(this /* sender */, EventArgs.Empty);
            }
        }
    }
}
