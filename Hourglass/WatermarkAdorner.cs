using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;

namespace Hourglass
{
    public class WatermarkAdorner : Adorner
    {
        private readonly ContentPresenter _contentPresenter;

        public WatermarkAdorner(UIElement adornedElement, object watermark)
            : base(adornedElement)
        {
            _contentPresenter = new ContentPresenter
            {
                Content = watermark,
                HorizontalAlignment = HorizontalAlignment.Center
            };
            IsHitTestVisible = false;

            BindingOperations.SetBinding(this, OpacityProperty, new Binding
            {
                Source = adornedElement,
                Path = new PropertyPath("Opacity"),
                Converter = new MultiplierConverter(0.5)
            });

            BindingOperations.SetBinding(this, VisibilityProperty, new Binding
            {
                Source = adornedElement,
                Path = new PropertyPath("Visibility")
            });
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            _contentPresenter.Arrange(new Rect(finalSize));

            var textBox = AdornedElement as TextBox;
            if (textBox != null)
            {
                TextElement.SetFontFamily(_contentPresenter, textBox.FontFamily);
                TextElement.SetFontSize(_contentPresenter, textBox.FontSize);
            }

            var comboBox = AdornedElement as ComboBox;
            if (comboBox != null)
            {
                TextElement.SetFontFamily(_contentPresenter, comboBox.FontFamily);
                TextElement.SetFontSize(_contentPresenter, comboBox.FontSize);
            }

            return finalSize;
        }

        protected override Visual GetVisualChild(int index)
        {
            return _contentPresenter;
        }

        protected override Size MeasureOverride(Size constraint)
        {
            _contentPresenter.Measure(AdornedElement.RenderSize);
            return AdornedElement.RenderSize;
        }

        protected override int VisualChildrenCount
        {
            get { return 1; }
        }
    }
}
