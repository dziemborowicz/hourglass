// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WatermarkAdorner.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass.Windows
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Documents;
    using System.Windows.Media;

    /// <summary>
    /// An <see cref="Adorner"/> that displays a watermark on a <see cref="TextBox"/> or <see cref="ComboBox"/> control.
    /// </summary>
    /// <seealso cref="Hint"/>
    public class WatermarkAdorner : Adorner
    {
        /// <summary>
        /// A <see cref="ContentPresenter"/> that contains the watermark.
        /// </summary>
        private readonly ContentPresenter contentPresenter;

        /// <summary>
        /// Initializes a new instance of the <see cref="WatermarkAdorner"/> class.
        /// </summary>
        /// <param name="adornedElement">The <see cref="UIElement"/> to apply the watermark to.</param>
        /// <param name="hint">The content of the watermark, typically a <see cref="string"/>.</param>
        /// <param name="brush">The foreground of the watermark.</param>
        public WatermarkAdorner(UIElement adornedElement, object hint, Brush brush)
            : base(adornedElement)
        {
            this.contentPresenter = new ContentPresenter();
            this.contentPresenter.Content = hint;
            this.contentPresenter.HorizontalAlignment = HorizontalAlignment.Center;
            TextBlock.SetForeground(this.contentPresenter, brush);

            this.IsHitTestVisible = false;

            this.AdornedElement.IsVisibleChanged += this.AdornedElementIsVisibleChanged;
            this.Visibility = this.AdornedElement.IsVisible ? Visibility.Visible : Visibility.Collapsed;

            Binding opacityBinding = new Binding();
            opacityBinding.Source = this.AdornedElement;
            opacityBinding.Path = new PropertyPath("Opacity");
            BindingOperations.SetBinding(this, UIElement.OpacityProperty, opacityBinding);
        }

        /// <summary>
        /// Gets or sets the content of the watermark.
        /// </summary>
        public object Hint
        {
            get { return this.contentPresenter.Content; }
            set { this.contentPresenter.Content = value; }
        }

        /// <summary>
        /// Gets or sets the foreground of the watermark.
        /// </summary>
        public Brush Brush
        {
            get { return TextBlock.GetForeground(this.contentPresenter); }
            set { TextBlock.SetForeground(this.contentPresenter, value); }
        }

        /// <summary>
        /// Gets the number of visual child elements within this element.
        /// </summary>
        protected override int VisualChildrenCount
        {
            get { return 1; }
        }

        /// <summary>
        /// Positions child elements and determines a size for the <see cref="WatermarkAdorner"/>.
        /// </summary>
        /// <param name="finalSize">The final area within the parent that this element should use to arrange itself and
        /// its children.</param>
        /// <returns>The actual size used.</returns>
        protected override Size ArrangeOverride(Size finalSize)
        {
            this.contentPresenter.Arrange(new Rect(finalSize));

            TextBox textBox = this.AdornedElement as TextBox;

            if (textBox != null)
            {
                TextElement.SetFontFamily(this.contentPresenter, textBox.FontFamily);
                TextElement.SetFontSize(this.contentPresenter, textBox.FontSize);
            }

            ComboBox comboBox = this.AdornedElement as ComboBox;

            if (comboBox != null)
            {
                TextElement.SetFontFamily(this.contentPresenter, comboBox.FontFamily);
                TextElement.SetFontSize(this.contentPresenter, comboBox.FontSize);
            }

            return finalSize;
        }

        /// <summary>
        /// Returns the child at the specified index from the child elements within the element.
        /// </summary>
        /// <param name="index">The zero-based index of the requested child element within the element.</param>
        /// <returns>The requested child element.</returns>
        /// <exception cref="ArgumentOutOfRangeException">If the provided index is out of range.</exception>
        protected override Visual GetVisualChild(int index)
        {
            if (index != 0)
            {
                throw new ArgumentOutOfRangeException("index");
            }

            return this.contentPresenter;
        }

        /// <summary>
        /// Measures the size in layout required for child elements and determines a size for the
        /// <see cref="WatermarkAdorner"/>.
        /// </summary>
        /// <param name="availableSize">The available size that this element can give to child elements. Infinity can
        /// be specified as a value to indicate that the element will size to whatever content is available.</param>
        /// <returns>The size that this element determines it needs during layout, based on its calculations of child
        /// element sizes.</returns>
        protected override Size MeasureOverride(Size availableSize)
        {
            this.contentPresenter.Measure(this.AdornedElement.RenderSize);
            return this.AdornedElement.RenderSize;
        }

        /// <summary>
        /// Invoked when the value of the <see cref="UIElement.IsVisible"/> property changes on the <see
        /// cref="Adorner.AdornedElement"/>.
        /// </summary>
        /// <param name="sender">The <see cref="Adorner.AdornedElement"/>.</param>
        /// <param name="e">The event data.</param>
        private void AdornedElementIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            this.Visibility = this.AdornedElement.IsVisible ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}
