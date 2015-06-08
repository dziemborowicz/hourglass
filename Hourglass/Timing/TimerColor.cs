// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimerColor.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass.Timing
{
    using System;
    using System.Windows.Media;

    using Hourglass.Managers;
    using Hourglass.Serialization;

    /// <summary>
    /// A color for the progress bar in a <see cref="TimerWindow"/>.
    /// </summary>
    public class TimerColor
    {
        /// <summary>
        /// The <see cref="Color"/> representation of this color.
        /// </summary>
        private readonly Color color;

        /// <summary>
        /// The friendly name of the color, or <c>null</c> if no friendly name is specified.
        /// </summary>
        private readonly string name;

        /// <summary>
        /// A value indicating whether this color is defined in the assembly.
        /// </summary>
        private readonly bool isBuiltIn;

        /// <summary>
        /// The <see cref="Brush"/> used to paint the this color.
        /// </summary>
        private Brush brush;

        /// <summary>
        /// Initializes a new instance of the <see cref="TimerColor"/> class.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <param name="name">The friendly name of the color. (Optional.)</param>
        /// <param name="isBuiltIn">A value indicating whether this color is defined in the assembly.</param>
        public TimerColor(Color color, string name = null, bool isBuiltIn = false)
        {
            this.color = color;
            this.name = name;
            this.isBuiltIn = isBuiltIn;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TimerColor"/> class.
        /// </summary>
        /// <param name="r">The sRGB red channel, R, of the color.</param>
        /// <param name="g">The sRGB green channel, G, of the color.</param>
        /// <param name="b">The sRGB blue channel, B, of the color.</param>
        /// <param name="name">The friendly name of the color. (Optional.)</param>
        /// <param name="isBuiltIn">A value indicating whether this color is defined in the assembly.</param>
        public TimerColor(byte r, byte g, byte b, string name = null, bool isBuiltIn = false)
            : this(Color.FromRgb(r, g, b), name, isBuiltIn)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TimerColor"/> class.
        /// </summary>
        /// <param name="colorString">A string representation of a color.</param>
        /// <param name="name">The friendly name of the color. (Optional.)</param>
        /// <param name="isBuiltIn">A value indicating whether this color is defined in the assembly.</param>
        public TimerColor(string colorString, string name = null, bool isBuiltIn = false)
            : this(GetColorFromString(colorString), name, isBuiltIn)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TimerColor"/> class from a <see cref="TimerColorInfo"/>.
        /// </summary>
        /// <param name="info">A <see cref="TimerColorInfo"/>.</param>
        private TimerColor(TimerColorInfo info)
            : this(info.Color, info.Name, info.IsBuiltIn)
        {
        }

        /// <summary>
        /// Gets the default color.
        /// </summary>
        public static TimerColor DefaultColor
        {
            get { return TimerColorManager.Instance.DefaultColor; }
        }

        /// <summary>
        /// Gets the <see cref="Color"/> representation of this color.
        /// </summary>
        public Color Color
        {
            get { return this.color; }
        }

        /// <summary>
        /// Gets the friendly name of the color, or <c>null</c> if no friendly name is specified.
        /// </summary>
        public string Name
        {
            get { return this.name; }
        }

        /// <summary>
        /// Gets a value indicating whether this color is defined in the assembly.
        /// </summary>
        public bool IsBuiltIn
        {
            get { return this.isBuiltIn; }
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
                    this.brush = new SolidColorBrush(this.color);
                }

                return this.brush;
            }
        }

        /// <summary>
        /// Returns a <see cref="TimerColor"/> for the specified <see cref="TimerColorInfo"/>, or <c>null</c> if the
        /// specified <see cref="TimerColorInfo"/> is <c>null</c>.
        /// </summary>
        /// <param name="info">A <see cref="TimerColorInfo"/>.</param>
        /// <returns>A <see cref="TimerColor"/> for the specified <see cref="TimerColorInfo"/>, or <c>null</c> if the
        /// specified <see cref="TimerColorInfo"/> is <c>null</c>.</returns>
        public static TimerColor FromTimerColorInfo(TimerColorInfo info)
        {
            if (info == null)
            {
                return null;
            }

            if (info.IsBuiltIn)
            {
                TimerColor color = TimerColorManager.Instance.TryGetColorByName(info.Name, info.IsBuiltIn);
                return color ?? new TimerColor(info.Color);
            }
            else
            {
                TimerColor color = TimerColorManager.Instance.TryGetColorByColor(info.Color, info.IsBuiltIn);
                return color ?? new TimerColor(info);
            }
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns><c>true</c> if the specified object is equal to the current object, or <c>false</c> otherwise.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (object.ReferenceEquals(this, obj))
            {
                return true;
            }

            if (object.ReferenceEquals(obj, null))
            {
                return false;
            }

            if (this.GetType() != obj.GetType())
            {
                return false;
            }

            TimerColor timerColor = (TimerColor)obj;
            return object.Equals(this.color, timerColor.color)
                && object.Equals(this.name, timerColor.name)
                && object.Equals(this.isBuiltIn, timerColor.isBuiltIn);
        }

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            int hash = 17;
            hash = (hash * 31) + this.color.GetHashCode();
            hash = (hash * 31) + (this.name != null ? this.name.GetHashCode() : 0);
            hash = (hash * 31) + this.isBuiltIn.GetHashCode();
            return hash;
        }

        /// <summary>
        /// Returns an <see cref="int"/> for this color.
        /// </summary>
        /// <returns>An <see cref="int"/> for this color.</returns>
        public int ToInt()
        {
            return (this.color.R << 0) | (this.color.G << 8) | (this.color.B << 16);
        }

        /// <summary>
        /// Returns the representation of the <see cref="TimerColor"/> used for XML serialization.
        /// </summary>
        /// <returns>The representation of the <see cref="TimerColor"/> used for XML serialization.</returns>
        public TimerColorInfo ToTimerColorInfo()
        {
            TimerColorInfo info = new TimerColorInfo();
            info.Color = this.color;
            info.Name = this.name;
            info.IsBuiltIn = this.isBuiltIn;
            return info;
        }

        /// <summary>
        /// Converts a <see cref="string"/> representation of a <see cref="Color"/> into a <see cref="Color"/>.
        /// </summary>
        /// <param name="colorString">A <see cref="string"/> representation of a <see cref="Color"/>.</param>
        /// <returns>A <see cref="Color"/>.</returns>
        private static Color GetColorFromString(string colorString)
        {
            object color = ColorConverter.ConvertFromString(colorString);

            if (color == null)
            {
                throw new ArgumentException("colorString");
            }

            return (Color)color;
        }
    }
}
