// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ResourceManagerExtensions.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass.Extensions
{
    using System;
    using System.Globalization;
    using System.Resources;

    /// <summary>
    /// Provides extensions methods for the <see cref="ResourceManager"/> class.
    /// </summary>
    public static class ResourceManagerExtensions
    {
        /// <summary>
        /// Returns an <see cref="IFormatProvider"/> for the culture that is actually loaded when retrieving resources.
        /// </summary>
        /// <param name="resourceManager">A <see cref="ResourceManager"/>.</param>
        /// <returns>An <see cref="IFormatProvider"/> for the culture that is actually loaded when retrieving
        /// resources.</returns>
        public static IFormatProvider GetEffectiveProvider(this ResourceManager resourceManager)
        {
            string cultureName = resourceManager.GetString("CultureName");
            return !string.IsNullOrEmpty(cultureName) ? CultureInfo.GetCultureInfo(cultureName) : CultureInfo.InvariantCulture;
        }

        /// <summary>
        /// Returns an <see cref="IFormatProvider"/> for the culture that is actually loaded when retrieving resources
        /// for the culture specified by <paramref name="provider"/>.
        /// </summary>
        /// <param name="resourceManager">A <see cref="ResourceManager"/>.</param>
        /// <param name="provider">An <see cref="IFormatProvider"/> that is a <see cref="CultureInfo"/>.</param>
        /// <returns>An <see cref="IFormatProvider"/> for the culture that is actually loaded when retrieving resources
        /// for the culture specified by <paramref name="provider"/>.</returns>
        public static IFormatProvider GetEffectiveProvider(this ResourceManager resourceManager, IFormatProvider provider)
        {
            string cultureName = resourceManager.GetString("CultureName", (CultureInfo)provider);
            return !string.IsNullOrEmpty(cultureName) ? CultureInfo.GetCultureInfo(cultureName) : CultureInfo.InvariantCulture;
        }

        /// <summary>
        /// Returns the value of the string resource localized for a culture specified by an <see
        /// cref="IFormatProvider"/>.
        /// </summary>
        /// <param name="resourceManager">A <see cref="ResourceManager"/>.</param>
        /// <param name="name">The name of the resource to retrieve.</param>
        /// <param name="provider">An <see cref="IFormatProvider"/> that is a <see cref="CultureInfo"/>.</param>
        /// <returns>The value of the resource localized for the specified culture, or <c>null</c> if <paramref
        /// name="name"/> cannot be found in a resource set.</returns>
        public static string GetString(this ResourceManager resourceManager, string name, IFormatProvider provider)
        {
            return resourceManager.GetString(name, (CultureInfo)provider);
        }
    }
}
