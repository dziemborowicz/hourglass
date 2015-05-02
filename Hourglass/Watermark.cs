// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Watermark.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass
{
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;
    using System.Windows.Input;

    /// <summary>
    /// Provides a <see cref="DependencyProperty"/> that allows a watermark to be applied to <see cref="TextBox"/> and
    /// <see cref="ComboBox"/> controls when the control has no actual value.
    /// </summary>
    /// <seealso cref="WatermarkAdorner"/>
    public static class Watermark
    {
        /// <summary>
        /// A <see cref="DependencyProperty"/> that specifies the watermark text to be displayed when the control has
        /// no actual value.
        /// </summary>
        public static readonly DependencyProperty HintTextProperty = DependencyProperty.RegisterAttached(
                                   "HintText",
                                   typeof(object),
                                   typeof(Watermark),
                                   new FrameworkPropertyMetadata(HintTextPropertyChanged));

        /// <summary>
        /// Returns the value of the <see cref="HintTextProperty"/>.
        /// </summary>
        /// <param name="control">A <see cref="Control"/>.</param>
        /// <returns>The value of the <see cref="HintTextProperty"/>.</returns>
        public static object GetHintText(Control control)
        {
            return control.GetValue(HintTextProperty);
        }

        /// <summary>
        /// Sets the value of the <see cref="HintTextProperty"/>.
        /// </summary>
        /// <param name="control">A <see cref="Control"/>.</param>
        /// <param name="value">The value to set.</param>
        public static void SetHintText(Control control, object value)
        {
            control.SetValue(HintTextProperty, value);
        }

        /// <summary>
        /// Invoked when the effective value of the <see cref="HintTextProperty"/> changes.
        /// </summary>
        /// <param name="sender">The <see cref="DependencyObject"/> on which the <see cref="HintTextProperty"/> has
        /// changed value.</param>
        /// <param name="e">Event data that is issued by any event that tracks changes to the effective value of this
        /// property.</param>
        private static void HintTextPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (!(sender is TextBox) && !(sender is ComboBox))
            {
                return;
            }

            Control control = (Control)sender;

            control.Loaded += ControlLoaded;
            control.GotKeyboardFocus += ControlGotKeyboardFocus;
            control.LostKeyboardFocus += ControlLostKeyboardFocus;
        }

        /// <summary>
        /// Invoked when the control is laid out, rendered, and ready for interaction.
        /// </summary>
        /// <param name="sender">The control.</param>
        /// <param name="e">The event data.</param>
        private static void ControlLoaded(object sender, RoutedEventArgs e)
        {
            Control control = (Control)sender;

            if (!HasActualValue(control) && !control.IsKeyboardFocused)
            {
                AddWatermark(control);
            }
        }

        /// <summary>
        /// Invoked when the keyboard is focused on a control.
        /// </summary>
        /// <param name="sender">The control.</param>
        /// <param name="e">The event data.</param>
        private static void ControlGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            Control control = (Control)sender;

            if (!HasActualValue(control))
            {
                RemoveWatermark(control);
            }
        }

        /// <summary>
        /// Invoked when the keyboard is no longer focused on a control.
        /// </summary>
        /// <param name="sender">The control.</param>
        /// <param name="e">The event data.</param>
        private static void ControlLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            Control control = (Control)sender;

            if (!HasActualValue(control))
            {
                AddWatermark(control);
            }
        }

        /// <summary>
        /// Adds a watermark to a <see cref="Control"/>.
        /// </summary>
        /// <param name="control">A <see cref="Control"/>.</param>
        private static void AddWatermark(Control control)
        {
            AdornerLayer layer = AdornerLayer.GetAdornerLayer(control);

            if (layer == null)
            {
                return;
            }

            layer.Add(new WatermarkAdorner(control, GetHintText(control)));
        }

        /// <summary>
        /// Removes the watermark from a <see cref="Control"/>.
        /// </summary>
        /// <param name="control">A <see cref="Control"/>.</param>
        private static void RemoveWatermark(Control control)
        {
            AdornerLayer layer = AdornerLayer.GetAdornerLayer(control);

            if (layer == null)
            {
                return;
            }

            Adorner[] adorners = layer.GetAdorners(control);

            if (adorners == null)
            {
                return;
            }

            foreach (WatermarkAdorner adorner in adorners.OfType<WatermarkAdorner>())
            {
                adorner.Visibility = Visibility.Hidden;
                layer.Remove(adorner);
            }
        }

        /// <summary>
        /// Returns a value indicating whether a <see cref="Control"/> has an actual value (where the text property is
        /// not <c>null</c> or empty).
        /// </summary>
        /// <param name="control">A <see cref="Control"/>.</param>
        /// <returns>A value indicating whether the <see cref="Control"/> has an actual value (where the text property
        /// is not <c>null</c> or empty).</returns>
        private static bool HasActualValue(Control control)
        {
            TextBox textBox = control as TextBox;

            if (textBox != null)
            {
                return !string.IsNullOrEmpty(textBox.Text);
            }

            ComboBox comboBox = control as ComboBox;

            if (comboBox != null)
            {
                return !string.IsNullOrEmpty(comboBox.Text);
            }

            return true;
        }
    }
}
