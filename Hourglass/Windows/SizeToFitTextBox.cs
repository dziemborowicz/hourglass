// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SizeToFitTextBox.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass.Windows
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    using Hourglass.Extensions;

    /// <summary>
    /// A <see cref="TextBox"/> that automatically adjusts the font size to ensure that the text is entirely visible.
    /// </summary>
    public class SizeToFitTextBox : TextBox
    {
        /// <summary>
        /// Identifies the minimum font size <see cref="DependencyProperty"/>.
        /// </summary>
        public static readonly DependencyProperty MinFontSizeProperty = DependencyProperty.Register(
            "MinFontSize",
            typeof(double),
            typeof(SizeToFitTextBox),
            new PropertyMetadata(double.NaN /* defaultValue */, MinFontSizePropertyChanged));

        /// <summary>
        /// Identifies the maximum font size <see cref="DependencyProperty"/>.
        /// </summary>
        public static readonly DependencyProperty MaxFontSizeProperty = DependencyProperty.Register(
            "MaxFontSize",
            typeof(double),
            typeof(SizeToFitTextBox),
            new PropertyMetadata(double.NaN /* defaultValue */, MaxFontSizePropertyChanged));

        /// <summary>
        /// Initializes a new instance of the <see cref="SizeToFitTextBox"/> class.
        /// </summary>
        public SizeToFitTextBox()
        {
            this.Loaded += (s, e) => this.UpdateFontSize();
            this.SizeChanged += (s, e) => this.UpdateFontSize();
            this.TextChanged += (s, e) => this.UpdateFontSize();
        }

        /// <summary>
        /// Gets or sets the minimum font size.
        /// </summary>
        public double MinFontSize
        {
            get { return (double)this.GetValue(MinFontSizeProperty); }
            set { this.SetValue(MinFontSizeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the maximum font size.
        /// </summary>
        public double MaxFontSize
        {
            get { return (double)this.GetValue(MaxFontSizeProperty); }
            set { this.SetValue(MaxFontSizeProperty, value); }
        }

        /// <summary>
        /// Invoked when the effective value of the <see cref="MinFontSizeProperty"/> changes.
        /// </summary>
        /// <param name="sender">The <see cref="DependencyObject"/> on which the <see cref="MinFontSizeProperty"/> has
        /// changed value.</param>
        /// <param name="e">Event data that is issued by any event that tracks changes to the effective value of this
        /// property.</param>
        private static void MinFontSizePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ((SizeToFitTextBox)sender).UpdateFontSize();
        }

        /// <summary>
        /// Invoked when the effective value of the <see cref="MaxFontSizeProperty"/> changes.
        /// </summary>
        /// <param name="sender">The <see cref="DependencyObject"/> on which the <see cref="MaxFontSizeProperty"/> has
        /// changed value.</param>
        /// <param name="e">Event data that is issued by any event that tracks changes to the effective value of this
        /// property.</param>
        private static void MaxFontSizePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ((SizeToFitTextBox)sender).UpdateFontSize();
        }

        /// <summary>
        /// Updates the <see cref="TextBox.FontSize"/> property to ensure that the text is entirely visible.
        /// </summary>
        private void UpdateFontSize()
        {
            if (double.IsNaN(this.MinFontSize) || double.IsNaN(this.MaxFontSize))
            {
                return;
            }

            double desiredFontSize = MathExtensions.LimitToRange(
                this.GetViewWidth() / this.GetTextWidth() * this.FontSize,
                this.MinFontSize,
                this.MaxFontSize);

            if (!double.IsInfinity(desiredFontSize) && !double.IsNaN(desiredFontSize) && desiredFontSize > 0.0)
            {
                this.FontSize = desiredFontSize;
            }
        }

        /// <summary>
        /// Returns the width of the text in the text box.
        /// </summary>
        /// <returns>The width of the text in the text box.</returns>
        private double GetTextWidth()
        {
            Typeface typeface = new Typeface(
                this.FontFamily,
                this.FontStyle,
                this.FontWeight,
                this.FontStretch);

            double pixelsPerDip = VisualTreeHelper.GetDpi(this).PixelsPerDip;

            FormattedText formattedText = new FormattedText(
                this.Text,
                CultureInfo.CurrentCulture,
                this.FlowDirection,
                typeface,
                this.FontSize,
                this.Foreground,
                pixelsPerDip);

            return formattedText.WidthIncludingTrailingWhitespace;
        }

        /// <summary>
        /// Returns the width of the control that contains the text.
        /// </summary>
        /// <returns>The width of the control that contains the text.</returns>
        private double GetViewWidth()
        {
            // This is the control closest to the text and gives the most accurate width
            DependencyObject textBoxView = this.FindVisualChild(
                o => o.GetType().ToString().Equals("System.Windows.Controls.TextBoxView", StringComparison.Ordinal));

            // Since TextBoxView is internal, fall back to this if it is not found
            FrameworkElement view = (textBoxView as FrameworkElement) ?? this;

            return view.ActualWidth;
        }
    }
}
