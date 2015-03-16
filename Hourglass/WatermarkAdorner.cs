// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WatermarkAdorner.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Documents;
    using System.Windows.Media;

    /// <summary>
    /// An <see cref="Adorner"/> that displays a watermark on <see cref="TextBox"/> and <see cref="ComboBox"/> controls
    /// when the control has no actual value.
    /// </summary>
    /// <seealso cref="Watermark"/>
    public class WatermarkAdorner : Adorner
    {
        /// <summary>
        /// A <see cref="ContentPresenter"/> that contains the watermark text.
        /// </summary>
        private readonly ContentPresenter contentPresenter;

        /// <summary>
        /// Initializes a new instance of the <see cref="WatermarkAdorner"/> class.
        /// </summary>
        /// <param name="adornedElement">The <see cref="UIElement"/> to apply the watermark to.</param>
        /// <param name="watermark">The content of the watermark, typically a <see cref="string"/>.</param>
        public WatermarkAdorner(UIElement adornedElement, object watermark)
            : base(adornedElement)
        {
            this.contentPresenter = new ContentPresenter();
            this.contentPresenter.Content = watermark;
            this.contentPresenter.HorizontalAlignment = HorizontalAlignment.Center;

            this.IsHitTestVisible = false;

            Binding opacityBinding = new Binding();
            opacityBinding.Source = adornedElement;
            opacityBinding.Path = new PropertyPath("Opacity");
            opacityBinding.Converter = new MultiplierConverter(0.5);
            BindingOperations.SetBinding(this, UIElement.OpacityProperty, opacityBinding);

            Binding visibilityBinding = new Binding();
            visibilityBinding.Source = adornedElement;
            visibilityBinding.Path = new PropertyPath("Visibility");
            BindingOperations.SetBinding(this, UIElement.VisibilityProperty, visibilityBinding);
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

            TextBox textBox = AdornedElement as TextBox;

            if (textBox != null)
            {
                TextElement.SetFontFamily(this.contentPresenter, textBox.FontFamily);
                TextElement.SetFontSize(this.contentPresenter, textBox.FontSize);
            }

            ComboBox comboBox = AdornedElement as ComboBox;

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
            this.contentPresenter.Measure(AdornedElement.RenderSize);
            return AdornedElement.RenderSize;
        }
    }
}
