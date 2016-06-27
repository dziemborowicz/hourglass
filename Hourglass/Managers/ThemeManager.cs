// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ThemeManager.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass.Managers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Hourglass.Properties;
    using Hourglass.Timing;

    /// <summary>
    /// Manages themes.
    /// </summary>
    public class ThemeManager : Manager
    {
        /// <summary>
        /// Singleton instance of the <see cref="ThemeManager"/> class.
        /// </summary>
        public static readonly ThemeManager Instance = new ThemeManager();

        /// <summary>
        /// A collection of themes.
        /// </summary>
        private readonly List<Theme> themes = new List<Theme>();

        /// <summary>
        /// Prevents a default instance of the <see cref="ThemeManager"/> class from being created.
        /// </summary>
        private ThemeManager()
        {
        }

        /// <summary>
        /// Gets the default theme.
        /// </summary>
        public Theme DefaultTheme
        {
            get { return this.GetThemeByIdentifier("resource:Light theme"); }
        }

        /// <summary>
        /// Gets a collection of all themes.
        /// </summary>
        public IList<Theme> AllThemes
        {
            get { return this.themes.ToList(); }
        }

        /// <summary>
        /// Gets a collection of the themes stored in the assembly.
        /// </summary>
        public IList<Theme> BuiltInThemes
        {
            get { return this.themes.Where(c => c.IsBuiltIn).ToList(); }
        }

        /// <summary>
        /// Gets a collection of the themes defined by the user.
        /// </summary>
        public IList<Theme> UserProvidedThemes
        {
            get { return this.themes.Where(c => !c.IsBuiltIn).ToList(); }
        }

        /// <summary>
        /// Initializes the class.
        /// </summary>
        public override void Initialize()
        {
            this.themes.Clear();
            this.AddRange(this.GetBuiltInThemes());
            this.AddRange(this.GetUserProvidedThemes());
        }

        /// <summary>
        /// Persists the state of the class.
        /// </summary>
        public override void Persist()
        {
            Settings.Default.UserProvidedThemes = this.UserProvidedThemes;
        }

        /// <summary>
        /// Adds a theme.
        /// </summary>
        /// <param name="theme">A <see cref="Theme"/>.</param>
        public void Add(Theme theme)
        {
            if (this.GetThemeByIdentifier(theme.Identifier) == null)
            {
                this.themes.Add(theme);
            }
        }

        /// <summary>
        /// Adds the themes of the specified collection.
        /// </summary>
        /// <param name="collection">A collection of <see cref="Theme"/>s.</param>
        public void AddRange(IEnumerable<Theme> collection)
        {
            foreach (Theme theme in collection)
            {
                this.Add(theme);
            }
        }

        /// <summary>
        /// Clears all <see cref="UserProvidedThemes"/>.
        /// </summary>
        public void ClearUserProvidedThemes()
        {
            this.themes.RemoveAll(c => !c.IsBuiltIn);
        }

        /// <summary>
        /// Returns the theme for the specified identifier, or <c>null</c> if no such theme is loaded.
        /// </summary>
        /// <param name="identifier">The identifier for the theme.</param>
        /// <returns>The theme for the specified identifier, or <c>null</c> if no such theme is loaded.</returns>
        public Theme GetThemeByIdentifier(string identifier)
        {
            if (string.IsNullOrEmpty(identifier))
            {
                return null;
            }

            return this.themes.FirstOrDefault(c => c.Identifier == identifier);
        }

        /// <summary>
        /// Returns the theme for the specified identifier, or <see cref="DefaultTheme"/> if no such theme is loaded.
        /// </summary>
        /// <param name="identifier">The identifier for the theme.</param>
        /// <returns>The theme for the specified identifier, or <see cref="DefaultTheme"/> if no such theme is loaded.
        /// </returns>
        public Theme GetThemeOrDefaultByIdentifier(string identifier)
        {
            return this.GetThemeByIdentifier(identifier) ?? this.DefaultTheme;
        }

        /// <summary>
        /// Returns the first theme for the specified name, or <c>null</c> if no such theme is loaded.
        /// </summary>
        /// <param name="name">The name for the theme.</param>
        /// <param name="stringComparison">One of the enumeration values that specifies how the strings will be
        /// compared.</param>
        /// <returns>The first theme for the specified name, or <c>null</c> if no such theme is loaded.</returns>
        public Theme GetThemeByName(string name, StringComparison stringComparison = StringComparison.Ordinal)
        {
            if (string.IsNullOrEmpty(name))
            {
                return null;
            }

            return this.themes.FirstOrDefault(c => string.Equals(c.Name, name, stringComparison));
        }

        /// <summary>
        /// Returns the first theme for the specified name, or <see cref="DefaultTheme"/> if no such theme is loaded.
        /// </summary>
        /// <param name="name">The name for the theme.</param>
        /// <param name="stringComparison">One of the enumeration values that specifies how the strings will be
        /// compared.</param>
        /// <returns>The first theme for the specified name, or <see cref="DefaultTheme"/> if no such theme is loaded.
        /// </returns>
        public Theme GetThemeOrDefaultByName(string name, StringComparison stringComparison = StringComparison.Ordinal)
        {
            return this.GetThemeByName(name, stringComparison) ?? this.DefaultTheme;
        }

        /// <summary>
        /// Loads the collection of themes defined in the assembly.
        /// </summary>
        /// <returns>A collection of themes defined in the assembly.</returns>
        private IList<Theme> GetBuiltInThemes()
        {
            return new List<Theme>
            {
                new Theme(
                    "#FFFFFF" /* backgroundColor */,
                    "#EEEEEE" /* progressBackgroundColor */,
                    "#C75050" /* expirationFlashColor */,
                    "#000000" /* mainTextColor */,
                    "#808080" /* mainHintColor */,
                    "#808080" /* secondaryTextColor */,
                    "#808080" /* secondaryHintColor */,
                    "#0066CC" /* buttonColor */,
                    "#FF0000" /* buttonHoverColor */,
                    Resources.ThemeManagerLightTheme /* name */,
                    "Light theme" /* invariantName */,
                    true /* isBuiltIn */),

                new Theme(
                    "#1E1E1E" /* backgroundColor */,
                    "#2D2D30" /* progressBackgroundColor */,
                    "#C75050" /* expirationFlashColor */,
                    "#808080" /* mainTextColor */,
                    "#505050" /* mainHintColor */,
                    "#505050" /* secondaryTextColor */,
                    "#505050" /* secondaryHintColor */,
                    "#0066CC" /* buttonColor */,
                    "#FF0000" /* buttonHoverColor */,
                    Resources.ThemeManagerDarkTheme /* name */,
                    "Dark theme" /* invariantName */,
                    true /* isBuiltIn */)
            };
        }

        /// <summary>
        /// Loads the collection of themes defined by the user.
        /// </summary>
        /// <returns>A collection of sounds defined by the user.</returns>
        private IList<Theme> GetUserProvidedThemes()
        {
            return Settings.Default.UserProvidedThemes;
        }
    }
}
