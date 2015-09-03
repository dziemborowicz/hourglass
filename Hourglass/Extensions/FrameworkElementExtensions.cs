// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FrameworkElementExtensions.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass.Extensions
{
    using System.Windows;
    using System.Windows.Input;

    /// <summary>
    /// Provides extensions methods for the <see cref="FrameworkElementExtensions"/> class.
    /// </summary>
    public static class FrameworkElementExtensions
    {
        /// <summary>
        /// Removes focus from the <see cref="FrameworkElement"/>.
        /// </summary>
        /// <param name="element">A <see cref="FrameworkElement"/>.</param>
        /// <returns>A value indicating whether the focus was removed from the element.</returns>
        public static bool Unfocus(this FrameworkElement element)
        {
            if (element.IsFocused)
            {
                FrameworkElement parent = (FrameworkElement)element.Parent;
                while (parent != null && !((IInputElement)parent).Focusable)
                {
                    parent = (FrameworkElement)parent.Parent;
                }

                DependencyObject scope = FocusManager.GetFocusScope(element);
                FocusManager.SetFocusedElement(scope, parent);
                return true;
            }

            return false;
        }
    }
}
