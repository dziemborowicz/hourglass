// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimerColorManager.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass.Managers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Media;

    using Hourglass.Properties;
    using Hourglass.Serialization;
    using Hourglass.Timing;

    /// <summary>
    /// Manages colors.
    /// </summary>
    public class TimerColorManager : Manager
    {
        /// <summary>
        /// Singleton instance of the <see cref="TimerColorManager"/> class.
        /// </summary>
        public static readonly TimerColorManager Instance = new TimerColorManager();

        /// <summary>
        /// The default color.
        /// </summary>
        private readonly TimerColor defaultColor = new TimerColor("#3665B3", "Default color" /* name */, true /* isBuiltIn */);

        /// <summary>
        /// A collection of colors.
        /// </summary>
        private readonly List<TimerColor> colors = new List<TimerColor>(); 

        /// <summary>
        /// Prevents a default instance of the <see cref="TimerColorManager"/> class from being created.
        /// </summary>
        private TimerColorManager()
        {
            IList<TimerColor> builtInColors = this.GetBuiltInColors();
            this.colors.AddRange(builtInColors);
        }

        /// <summary>
        /// Gets the default color.
        /// </summary>
        public TimerColor DefaultColor
        {
            get { return this.defaultColor; }
        }

        /// <summary>
        /// Gets a collection of colors.
        /// </summary>
        /// <remarks>
        /// This does not include <see cref="DefaultColor"/>.
        /// </remarks>
        public IList<TimerColor> Colors
        {
            get { return this.colors.ToList(); }
        }

        /// <summary>
        /// Gets a collection of colors defined in the assembly.
        /// </summary>
        /// <remarks>
        /// This does not include <see cref="DefaultColor"/>.
        /// </remarks>
        public IList<TimerColor> BuiltInColors
        {
            get { return this.colors.Where(c => c.IsBuiltIn).ToList(); }
        }

        /// <summary>
        /// Gets a collection of colors specified by the user.
        /// </summary>
        public IList<TimerColor> UserProvidedColors
        {
            get { return this.colors.Where(c => !c.IsBuiltIn).ToList(); }
        }

        /// <summary>
        /// Initializes the class.
        /// </summary>
        public override void Initialize()
        {
            this.RemoveUserProvidedColors();

            IEnumerable<TimerColorInfo> colorInfos = Settings.Default.Colors;
            if (colorInfos != null)
            {
                this.colors.AddRange(colorInfos.Select(TimerColor.FromTimerColorInfo));
            }
        }

        /// <summary>
        /// Persists the state of the class.
        /// </summary>
        public override void Persist()
        {
            IEnumerable<TimerColorInfo> colorInfos = this.colors
                .Where(c => !c.IsBuiltIn)
                .Select(TimerColorInfo.FromTimerColor);

            Settings.Default.Colors = new TimerColorInfoList(colorInfos);
        }

        /// <summary>
        /// Add a <see cref="TimerColor"/>.
        /// </summary>
        /// <param name="color">The <see cref="TimerColor"/> to add.</param>
        public void Add(TimerColor color)
        {
            if (!this.colors.Contains(color))
            {
                this.colors.Add(color);
            }
        }

        /// <summary>
        /// Removes a <see cref="TimerColor"/>.
        /// </summary>
        /// <param name="color">The <see cref="TimerColor"/> to remove.</param>
        public void Remove(TimerColor color)
        {
            this.colors.Remove(color);
        }

        /// <summary>
        /// Removes all of the <see cref="UserProvidedColors"/>.
        /// </summary>
        public void RemoveUserProvidedColors()
        {
            this.colors.RemoveAll(c => !c.IsBuiltIn);
        }

        /// <summary>
        /// Returns a <see cref="TimerColor"/> with the specified name.
        /// </summary>
        /// <param name="name">The friendly name for the <see cref="TimerColor"/>.</param>
        /// <param name="isBuiltIn">A value indicating whether this color is defined in the assembly.</param>
        /// <returns>A <see cref="TimerColor"/> with the specified name.</returns>
        public TimerColor TryGetColorByName(string name, bool isBuiltIn)
        {
            if (string.IsNullOrEmpty(name))
            {
                return null;
            }

            if (string.Equals(name, this.defaultColor.Name, StringComparison.CurrentCultureIgnoreCase) && object.Equals(isBuiltIn, this.defaultColor.IsBuiltIn))
            {
                return this.defaultColor;
            }

            return this.colors.FirstOrDefault(c => string.Equals(c.Name, name, StringComparison.OrdinalIgnoreCase) && object.Equals(c.IsBuiltIn, isBuiltIn));
        }

        /// <summary>
        /// Returns a <see cref="TimerColor"/> with the specified name.
        /// </summary>
        /// <param name="color">The underlying <see cref="Color"/> of the <see cref="TimerColor"/>.</param>
        /// <param name="isBuiltIn">A value indicating whether this color is defined in the assembly.</param>
        /// <returns>A <see cref="TimerColor"/> with the specified name.</returns>
        public TimerColor TryGetColorByColor(Color color, bool isBuiltIn)
        {
            return this.colors.FirstOrDefault(c => object.Equals(c.Color, color) && object.Equals(c.IsBuiltIn, isBuiltIn));
        }

        /// <summary>
        /// Loads the collection of sounds defined in the assembly.
        /// </summary>
        /// <returns>A collection of sounds defined in the assembly.</returns>
        private IList<TimerColor> GetBuiltInColors()
        {
            List<TimerColor> list = new List<TimerColor>();
            list.Add(new TimerColor("#C75050", "Red" /* name */, true /* isBuiltIn */));
            list.Add(new TimerColor("#FF7F50", "Orange" /* name */, true /* isBuiltIn */));
            list.Add(new TimerColor("#FFC800", "Yellow" /* name */, true /* isBuiltIn */));
            list.Add(new TimerColor("#57A64A", "Green" /* name */, true /* isBuiltIn */));
            list.Add(new TimerColor("#3665B3", "Blue" /* name */, true /* isBuiltIn */));
            list.Add(new TimerColor("#843179", "Purple" /* name */, true /* isBuiltIn */));
            list.Add(new TimerColor("#666666", "Gray" /* name */, true /* isBuiltIn */));
            list.Add(new TimerColor("#000000", "Black" /* name */, true /* isBuiltIn */));
            return list;
        }
    }
}
