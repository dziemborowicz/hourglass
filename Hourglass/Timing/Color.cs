// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Color.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass.Timing
{
    using System;
    using System.Windows.Media;

    using Hourglass.Extensions;
    using Hourglass.Managers;

    /// <summary>
    /// A color for the timer progress bar.
    /// </summary>
    public class Color
    {
        /// <summary>
        /// The friendly name for this color, or <c>null</c> if no friendly name is specified.
        /// </summary>
        private readonly string name;

        /// <summary>
        /// A unique identifier for this color.
        /// </summary>
        private readonly string identifier;

        /// <summary>
        /// A value indicating whether this color is defined in the assembly.
        /// </summary>
        private readonly bool isBuiltIn;

        /// <summary>
        /// The <see cref="System.Windows.Media.Color"/> representation of this color.
        /// </summary>
        private readonly System.Windows.Media.Color mediaColor;

        /// <summary>
        /// The <see cref="Brush"/> used to paint the this color.
        /// </summary>
        private Brush brush;

        /// <summary>
        /// Initializes a new instance of the <see cref="Color"/> class.
        /// </summary>
        /// <param name="mediaColor">The color.</param>
        /// <param name="invariantName">The culture-insensitive name of the color. (Optional.)</param>
        /// <param name="name">The localized friendly name of the color. (Optional.)</param>
        /// <param name="isBuiltIn">A value indicating whether this color is defined in the assembly. (Optional.)</param>
        public Color(System.Windows.Media.Color mediaColor, string invariantName = null, string name = null, bool isBuiltIn = false)
        {
            this.name = name;
            this.identifier = isBuiltIn ? ("resource:" + invariantName) : ("color:" + mediaColor);
            this.isBuiltIn = isBuiltIn;
            this.mediaColor = mediaColor;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Color"/> class.
        /// </summary>
        /// <param name="r">The sRGB red channel, R, of the color.</param>
        /// <param name="g">The sRGB green channel, G, of the color.</param>
        /// <param name="b">The sRGB blue channel, B, of the color.</param>
        /// <param name="invariantName">The culture-insensitive name of the color. (Optional.)</param>
        /// <param name="name">The localized friendly name of the color. (Optional.)</param>
        /// <param name="isBuiltIn">A value indicating whether this color is defined in the assembly. (Optional.)</param>
        public Color(byte r, byte g, byte b, string invariantName = null, string name = null, bool isBuiltIn = false)
            : this(System.Windows.Media.Color.FromRgb(r, g, b), invariantName, name, isBuiltIn)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Color"/> class.
        /// </summary>
        /// <param name="colorString">A string representation of a color.</param>
        /// <param name="invariantName">The culture-insensitive name of the color. (Optional.)</param>
        /// <param name="name">The localized friendly name of the color. (Optional.)</param>
        /// <param name="isBuiltIn">A value indicating whether this color is defined in the assembly. (Optional.)</param>
        public Color(string colorString, string invariantName = null, string name = null, bool isBuiltIn = false)
            : this(ColorExtensions.ConvertFromString(colorString), invariantName, name, isBuiltIn)
        {
        }

        /// <summary>
        /// Gets the default color.
        /// </summary>
        public static Color DefaultColor
        {
            get { return ColorManager.Instance.DefaultColor; }
        }

        /// <summary>
        /// Gets the friendly name of this color, or <c>null</c> if no friendly name is specified.
        /// </summary>
        public string Name
        {
            get { return this.name; }
        }

        /// <summary>
        /// Gets the unique identifier for this color.
        /// </summary>
        public string Identifier
        {
            get { return this.identifier; }
        }

        /// <summary>
        /// Gets a value indicating whether this color is defined in the assembly.
        /// </summary>
        public bool IsBuiltIn
        {
            get { return this.isBuiltIn; }
        }

        /// <summary>
        /// Gets the <see cref="System.Windows.Media.Color"/> representation of this color.
        /// </summary>
        public System.Windows.Media.Color MediaColor
        {
            get { return this.mediaColor; }
        }

        /// <summary>
        /// Gets the <see cref="Brush"/> used to paint the this color.
        /// </summary>
        public Brush Brush
        {
            get
            {
                if (this.brush == null)
                {
                    this.brush = new SolidColorBrush(this.mediaColor);
                }

                return this.brush;
            }
        }
        
        /// <summary>
        /// Returns a <see cref="Color"/> for the specified identifier, or <c>null</c> if the identifier is <c>null</c>
        /// or empty.
        /// </summary>
        /// <param name="identifier">The identifier for the color.</param>
        /// <returns>A <see cref="Color"/> for the specified identifier, or <c>null</c> if the identifier is
        /// <c>null</c> or empty.</returns>
        public static Color FromIdentifier(string identifier)
        {
            if (identifier.StartsWith("color:", StringComparison.Ordinal))
            {
                Color color = ColorManager.Instance.GetColorByIdentifier(identifier);

                // If the color is not registered in the ColorManager, register it
                if (color == null)
                {
                    string colorString = identifier.Substring("color:".Length);
                    color = new Color(colorString);
                    ColorManager.Instance.Add(color);
                }

                return color;
            }
            else
            {
                return ColorManager.Instance.GetColorOrDefaultByIdentifier(identifier);
            }
        }

        /// <summary>
        /// Returns an <see cref="int"/> for this color.
        /// </summary>
        /// <returns>An <see cref="int"/> for this color.</returns>
        public int ToInt()
        {
            return ColorExtensions.ConverToInt(this.mediaColor);
        }
    }
}
