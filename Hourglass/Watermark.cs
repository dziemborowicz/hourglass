using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace Hourglass
{
    public static class Watermark
    {
        public static readonly DependencyProperty HintTextProperty = DependencyProperty.RegisterAttached(
                                   "HintText",
                                   typeof(object), typeof(Watermark),
                                   new FrameworkPropertyMetadata(null, HintTextPropertyChanged));

        public static object GetHintText(Control control)
        {
            return control.GetValue(HintTextProperty);
        }

        public static void SetHintText(Control control, object value)
        {
            control.SetValue(HintTextProperty, value);
        }

        private static void HintTextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is TextBox) && !(d is ComboBox))
                return;

            var control = (Control)d;

            control.Loaded += (sender, eventArgs) =>
            {
                if (!HasActualValue(control))
                    AddWatermark(control);
            };

            control.GotKeyboardFocus += (sender, eventArgs) =>
            {
                if (!HasActualValue(control))
                    RemoveWatermark(control);
            };

            control.LostKeyboardFocus += (sender, eventArgs) =>
            {
                if (!HasActualValue(control))
                    AddWatermark(control);
            };
        }

        private static void AddWatermark(Control control)
        {
            var layer = AdornerLayer.GetAdornerLayer(control);
            if (layer == null)
                return;

            layer.Add(new WatermarkAdorner(control, GetHintText(control)));
        }

        private static void RemoveWatermark(Control control)
        {
            var layer = AdornerLayer.GetAdornerLayer(control);
            if (layer == null)
                return;

            var adorners = layer.GetAdorners(control);
            if (adorners == null)
                return;

            foreach (var adorner in adorners.OfType<WatermarkAdorner>())
            {
                adorner.Visibility = Visibility.Hidden;
                layer.Remove(adorner);
            }
        }

        private static bool HasActualValue(Control control)
        {
            var textBox = control as TextBox;
            if (textBox != null)
                return !string.IsNullOrEmpty(textBox.Text);

            var comboBox = control as ComboBox;
            if (comboBox != null)
                return !string.IsNullOrEmpty(comboBox.Text);

            return true;
        }
    }
}
