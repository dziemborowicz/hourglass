// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FocusUtility.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass
{
    using System.Windows;
    using System.Windows.Input;

    /// <summary>
    /// A utility class for handling focus in Windows Presentation Foundation applications.
    /// </summary>
    public static class FocusUtility
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
