// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ColorManager.cs" company="Chris Dziemborowicz">
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
    /// Manages colors.
    /// </summary>
    public class ColorManager : Manager
    {
        /// <summary>
        /// Singleton instance of the <see cref="ColorManager"/> class.
        /// </summary>
        public static readonly ColorManager Instance = new ColorManager();

        /// <summary>
        /// A collection of colors.
        /// </summary>
        private readonly List<Color> colors = new List<Color>(); 

        /// <summary>
        /// Prevents a default instance of the <see cref="ColorManager"/> class from being created.
        /// </summary>
        private ColorManager()
        {
        }

        /// <summary>
        /// Gets the default color.
        /// </summary>
        public Color DefaultColor
        {
            get { return this.GetColorByIdentifier("resource:Default color"); }
        }

        /// <summary>
        /// Gets a collection of all colors.
        /// </summary>
        public IList<Color> AllColors
        {
            get { return this.colors.ToList(); }
        }

        /// <summary>
        /// Gets a collection of the colors stored in the assembly.
        /// </summary>
        public IList<Color> BuiltInColors
        {
            get { return this.colors.Where(c => c.IsBuiltIn).ToList(); }
        }

        /// <summary>
        /// Gets a collection of the colors defined by the user.
        /// </summary>
        public IList<Color> UserProvidedColors
        {
            get { return this.colors.Where(c => !c.IsBuiltIn).ToList(); }
        }

        /// <summary>
        /// Initializes the class.
        /// </summary>
        public override void Initialize()
        {
            this.colors.Clear();
            this.colors.AddRange(this.GetBuiltInColors());
            this.colors.AddRange(this.GetUserProvidedColors());
        }

        /// <summary>
        /// Persists the state of the class.
        /// </summary>
        public override void Persist()
        {
            Settings.Default.UserProvidedColors = this.UserProvidedColors;
        }

        /// <summary>
        /// Adds a color.
        /// </summary>
        /// <param name="color">A <see cref="Color"/>.</param>
        public void Add(Color color)
        {
            if (this.GetColorByIdentifier(color.Identifier) == null)
            {
                this.colors.Add(color);
            }
        }

        /// <summary>
        /// Clears all <see cref="UserProvidedColors"/>.
        /// </summary>
        public void ClearUserProvidedColors()
        {
            this.colors.RemoveAll(c => !c.IsBuiltIn);
        }

        /// <summary>
        /// Returns the color for the specified identifier, or <c>null</c> if no such color is loaded.
        /// </summary>
        /// <param name="identifier">The identifier for the color.</param>
        /// <returns>The color for the specified identifier, or <c>null</c> if no such color is loaded.</returns>
        public Color GetColorByIdentifier(string identifier)
        {
            if (string.IsNullOrEmpty(identifier))
            {
                return null;
            }

            return this.colors.FirstOrDefault(c => c.Identifier == identifier);
        }

        /// <summary>
        /// Returns the color for the specified identifier, or <see cref="DefaultColor"/> if no such color is loaded.
        /// </summary>
        /// <param name="identifier">The identifier for the color.</param>
        /// <returns>The color for the specified identifier, or <see cref="DefaultColor"/> if no such color is loaded.
        /// </returns>
        public Color GetColorOrDefaultByIdentifier(string identifier)
        {
            if (string.IsNullOrEmpty(identifier))
            {
                return null;
            }

            return this.GetColorByIdentifier(identifier) ?? this.DefaultColor;
        }

        /// <summary>
        /// Returns the first color for the specified name, or <c>null</c> if no such color is loaded.
        /// </summary>
        /// <param name="name">The name for the color.</param>
        /// <param name="stringComparison">One of the enumeration values that specifies how the strings will be
        /// compared.</param>
        /// <returns>The first color for the specified name, or <c>null</c> if no such color is loaded.</returns>
        public Color GetColorByName(string name, StringComparison stringComparison = StringComparison.Ordinal)
        {
            if (string.IsNullOrEmpty(name))
            {
                return null;
            }

            return this.colors.FirstOrDefault(c => string.Equals(c.Name, name, stringComparison));
        }

        /// <summary>
        /// Returns the first color for the specified name, or <see cref="DefaultColor"/> if no such color is loaded.
        /// </summary>
        /// <param name="name">The name for the color.</param>
        /// <param name="stringComparison">One of the enumeration values that specifies how the strings will be
        /// compared.</param>
        /// <returns>The first color for the specified name, or <see cref="DefaultColor"/> if no such color is loaded.
        /// </returns>
        public Color GetColorOrDefaultByName(string name, StringComparison stringComparison = StringComparison.Ordinal)
        {
            if (string.IsNullOrEmpty(name))
            {
                return null;
            }

            return this.GetColorByName(name, stringComparison) ?? this.DefaultColor;
        }

        /// <summary>
        /// Returns the first color for the specified <see cref="System.Windows.Media.Color"/>, or <c>null</c> if no
        /// such color is loaded.
        /// </summary>
        /// <param name="mediaColor">The <see cref="System.Windows.Media.Color"/> for the color.</param>
        /// <returns>The first color for the specified <see cref="System.Windows.Media.Color"/>, or <c>null</c> if no
        /// such color is loaded.</returns>
        public Color GetColorByMediaColor(System.Windows.Media.Color mediaColor)
        {
            return this.colors.FirstOrDefault(c => object.Equals(c.MediaColor, mediaColor));
        }

        /// <summary>
        /// Returns the first color for the specified <see cref="System.Windows.Media.Color"/>, or <see
        /// cref="DefaultColor"/> if no such color is loaded.
        /// </summary>
        /// <param name="mediaColor">The <see cref="System.Windows.Media.Color"/> for the color.</param>
        /// <returns>The first color for the specified <see cref="System.Windows.Media.Color"/>, or <see
        /// cref="DefaultColor"/> if no such color is loaded.</returns>
        public Color GetColorOrDefaultByMediaColor(System.Windows.Media.Color mediaColor)
        {
            return this.GetColorByMediaColor(mediaColor) ?? this.DefaultColor;
        }

        /// <summary>
        /// Loads the collection of colors defined in the assembly.
        /// </summary>
        /// <returns>A collection of colors defined in the assembly.</returns>
        private IList<Color> GetBuiltInColors()
        {
            return new List<Color>
            {
                new Color("#3665B3", "Default color" /* name */, true /* isBuiltIn */),
                new Color("#C75050", "Red" /* name */, true /* isBuiltIn */),
                new Color("#FF7F50", "Orange" /* name */, true /* isBuiltIn */),
                new Color("#FFC800", "Yellow" /* name */, true /* isBuiltIn */),
                new Color("#57A64A", "Green" /* name */, true /* isBuiltIn */),
                new Color("#3665B3", "Blue" /* name */, true /* isBuiltIn */),
                new Color("#843179", "Purple" /* name */, true /* isBuiltIn */),
                new Color("#666666", "Gray" /* name */, true /* isBuiltIn */),
                new Color("#000000", "Black" /* name */, true /* isBuiltIn */)
            };
        }

        /// <summary>
        /// Loads the collection of colors defined by the user.
        /// </summary>
        /// <returns>A collection of sounds defined by the user.</returns>
        private IList<Color> GetUserProvidedColors()
        {
            return Settings.Default.UserProvidedColors;
        }
    }
}
