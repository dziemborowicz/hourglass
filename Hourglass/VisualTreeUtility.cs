// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VisualTreeUtility.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass
{
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Media;

    /// <summary>
    /// A utility class for traversing the visual tree in Windows Presentation Framework applications.
    /// </summary>
    public class VisualTreeUtility
    {
        /// <summary>
        /// Returns the children of type <typeparamref name="T"/> of a <see cref="DependencyObject"/> in the visual
        /// tree.
        /// </summary>
        /// <typeparam name="T">The type of children to return.</typeparam>
        /// <param name="depObj">A <see cref="DependencyObject"/>.</param>
        /// <returns>The children of type <typeparamref name="T"/> of a <see cref="DependencyObject"/> in the visual
        /// tree.</returns>
        public static IEnumerable<T> GetVisualChildren<T>(DependencyObject depObj)
            where T : DependencyObject
        {
            if (depObj == null)
            {
                yield break;
            }

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(depObj, i);

                if (child is T)
                {
                    yield return (T)child;
                }

                foreach (T childOfChild in GetVisualChildren<T>(child))
                {
                    yield return childOfChild;
                }
            }
        }
    }
}
