using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace Hourglass
{
    public class WatermarkAdorner : Adorner
    {
        private readonly ContentPresenter contentPresenter;

        public WatermarkAdorner(UIElement adornedElement, object watermark)
            : base(adornedElement)
        {
            this.contentPresenter = new ContentPresenter();
            this.contentPresenter.Content = watermark;
            this.contentPresenter.HorizontalAlignment = HorizontalAlignment.Center;
            this.contentPresenter.Opacity = 0.25;
            this.IsHitTestVisible = false;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            contentPresenter.Arrange(new Rect(finalSize));

            TextBox textBox = AdornedElement as TextBox;
            if (textBox != null)
            {
                TextElement.SetFontFamily(contentPresenter, textBox.FontFamily);
                TextElement.SetFontSize(contentPresenter, textBox.FontSize);
            }

            ComboBox comboBox = AdornedElement as ComboBox;
            if (comboBox != null)
            {
                TextElement.SetFontFamily(contentPresenter, comboBox.FontFamily);
                TextElement.SetFontSize(contentPresenter, comboBox.FontSize);
            }

            return finalSize;
        }

        protected override Visual GetVisualChild(int index)
        {
            return contentPresenter;
        }

        protected override Size MeasureOverride(Size constraint)
        {
            contentPresenter.Measure(((Control)AdornedElement).RenderSize);
            return ((Control)AdornedElement).RenderSize;
        }

        protected override int VisualChildrenCount
        {
            get { return 1; }
        }
    }
}
