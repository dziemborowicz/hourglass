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
    using System.Windows.Controls.Primitives;
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
        /// A <see cref="DependencyProperty"/> that specifies the content of the watermark to be displayed when the
        /// control has no actual value and no keyboard focus, typically a <see cref="string"/>.
        /// </summary>
        public static readonly DependencyProperty HintProperty = DependencyProperty.RegisterAttached(
                                   "Hint",
                                   typeof(object),
                                   typeof(Watermark),
                                   new FrameworkPropertyMetadata(HintPropertyChanged));

        /// <summary>
        /// Returns the value of the <see cref="HintProperty"/>.
        /// </summary>
        /// <param name="control">A <see cref="Control"/>.</param>
        /// <returns>The value of the <see cref="HintProperty"/>.</returns>
        public static object GetHint(Control control)
        {
            return control.GetValue(HintProperty);
        }

        /// <summary>
        /// Sets the value of the <see cref="HintProperty"/>.
        /// </summary>
        /// <param name="control">A <see cref="Control"/>.</param>
        /// <param name="value">The value to set.</param>
        public static void SetHint(Control control, object value)
        {
            control.SetValue(HintProperty, value);
        }

        /// <summary>
        /// Invoked when the effective value of the <see cref="HintProperty"/> changes.
        /// </summary>
        /// <param name="sender">The <see cref="DependencyObject"/> on which the <see cref="HintProperty"/> has changed
        /// value.</param>
        /// <param name="e">Event data that is issued by any event that tracks changes to the effective value of this
        /// property.</param>
        private static void HintPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (!(sender is TextBox) && !(sender is ComboBox))
            {
                return;
            }

            Control control = (Control)sender;

            control.Loaded -= ControlLoaded;
            control.Loaded += ControlLoaded;

            control.GotKeyboardFocus -= ControlGotKeyboardFocus;
            control.GotKeyboardFocus += ControlGotKeyboardFocus;

            control.LostKeyboardFocus -= ControlLostKeyboardFocus;
            control.LostKeyboardFocus += ControlLostKeyboardFocus;

            TextBox textBox = control as TextBox;
            if (textBox != null)
            {
                textBox.TextChanged -= TextBoxTextChanged;
                textBox.TextChanged += TextBoxTextChanged;
            }

            ComboBox comboBox = control as ComboBox;
            if (comboBox != null)
            {
                comboBox.SelectionChanged -= ComboBoxSelectionChanged;
                comboBox.SelectionChanged += ComboBoxSelectionChanged;

                comboBox.RemoveHandler(TextBoxBase.TextChangedEvent, new RoutedEventHandler(ComboBoxTextChanged));
                comboBox.AddHandler(TextBoxBase.TextChangedEvent, new RoutedEventHandler(ComboBoxTextChanged));
            }

            UpdateWatermark(control);
        }

        /// <summary>
        /// Invoked when the control is laid out, rendered, and ready for interaction.
        /// </summary>
        /// <param name="sender">The control.</param>
        /// <param name="e">The event data.</param>
        private static void ControlLoaded(object sender, RoutedEventArgs e)
        {
            Control control = (Control)sender;
            UpdateWatermark(control);
        }

        /// <summary>
        /// Invoked when the keyboard is focused on a control.
        /// </summary>
        /// <param name="sender">The control.</param>
        /// <param name="e">The event data.</param>
        private static void ControlGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            Control control = (Control)sender;
            UpdateWatermark(control);
        }

        /// <summary>
        /// Invoked when the keyboard is no longer focused on a control.
        /// </summary>
        /// <param name="sender">The control.</param>
        /// <param name="e">The event data.</param>
        private static void ControlLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            Control control = (Control)sender;
            UpdateWatermark(control);
        }

        /// <summary>
        /// Invoked when content changes in a text box control.
        /// </summary>
        /// <param name="sender">The control.</param>
        /// <param name="e">The event data.</param>
        private static void TextBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            Control control = (Control)sender;
            UpdateWatermark(control);
        }

        /// <summary>
        /// Invoked when the selection of a combo box control changes.
        /// </summary>
        /// <param name="sender">The control.</param>
        /// <param name="e">The event data.</param>
        private static void ComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Control control = (Control)sender;
            UpdateWatermark(control);
        }

        /// <summary>
        /// Invoked when content changes in a combo box control changes.
        /// </summary>
        /// <param name="sender">The control.</param>
        /// <param name="e">The event data.</param>
        private static void ComboBoxTextChanged(object sender, RoutedEventArgs e)
        {
            Control control = (Control)sender;
            UpdateWatermark(control);
        }

        /// <summary>
        /// Adds a <see cref="WatermarkAdorner"/> to a <see cref="Control"/>.
        /// </summary>
        /// <param name="control">A <see cref="Control"/>.</param>
        /// <param name="hint">The content of the watermark, typically a <see cref="string"/>.</param>
        private static void AddWatermarkAdorner(Control control, object hint)
        {
            AdornerLayer layer = AdornerLayer.GetAdornerLayer(control);
            if (layer == null)
            {
                return;
            }

            layer.Add(new WatermarkAdorner(control, hint));
        }

        /// <summary>
        /// Removes the <see cref="WatermarkAdorner"/> from a <see cref="Control"/>.
        /// </summary>
        /// <param name="control">A <see cref="Control"/>.</param>
        private static void RemoveWatermarkAdorner(Control control)
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
                layer.Remove(adorner);
            }
        }

        /// <summary>
        /// Updates the <see cref="WatermarkAdorner"/> on the <see cref="Control"/>.
        /// </summary>
        /// <param name="control">A <see cref="Control"/>.</param>
        private static void UpdateWatermark(Control control)
        {
            if (!control.IsKeyboardFocused && !HasActualValue(control))
            {
                WatermarkAdorner watermarkAdorner = GetWatermarkAdorner(control);
                object hint = GetHint(control);

                if (watermarkAdorner == null)
                {
                    AddWatermarkAdorner(control, hint);
                }
                else if (!object.Equals(watermarkAdorner.Hint, hint))
                {
                    watermarkAdorner.Hint = hint;
                }
            }
            else
            {
                RemoveWatermarkAdorner(control);
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

        /// <summary>
        /// Returns a <see cref="WatermarkAdorner"/> for a <see cref="Control"/>, or <c>null</c> if none exists.
        /// </summary>
        /// <param name="control">A <see cref="Control"/>.</param>
        /// <returns>A <see cref="WatermarkAdorner"/> for the <see cref="Control"/>, or <c>null</c> if none exists.
        /// </returns>
        private static WatermarkAdorner GetWatermarkAdorner(Control control)
        {
            AdornerLayer layer = AdornerLayer.GetAdornerLayer(control);
            if (layer == null)
            {
                return null;
            }

            Adorner[] adorners = layer.GetAdorners(control);
            if (adorners == null)
            {
                return null;
            }

            return adorners.OfType<WatermarkAdorner>().FirstOrDefault();
        }
    }
}
