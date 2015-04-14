// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SoundInfo.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass
{
    using System;

    /// <summary>
    /// A representation of a sound used to notify the user that a <see cref="Timer"/> has expired.
    /// </summary>
    public class SoundInfo
    {
        /// <summary>
        /// The friendly name for the sound.
        /// </summary>
        private readonly string name;

        /// <summary>
        /// A value indicating whether the sound is a built-in resource.
        /// </summary>
        private readonly bool isBuiltIn;

        /// <summary>
        /// The path to the sound.
        /// </summary>
        private readonly string path;

        /// <summary>
        /// Initializes a new instance of the <see cref="SoundInfo"/> class.
        /// </summary>
        /// <param name="name">The friendly name for the sound.</param>
        /// <param name="isBuiltIn">A value indicating whether the sound is a built-in resource.</param>
        /// <param name="path">The path to the sound.</param>
        public SoundInfo(string name, bool isBuiltIn, string path)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }

            this.name = name;
            this.path = path;
            this.isBuiltIn = isBuiltIn;
        }

        /// <summary>
        /// Gets the friendly name for the sound.
        /// </summary>
        public string Name
        {
            get { return this.name; }
        }

        /// <summary>
        /// Gets a value indicating whether the sound is a built-in resource.
        /// </summary>
        public bool IsBuiltIn
        {
            get { return this.isBuiltIn; }
        }

        /// <summary>
        /// Gets the path to the sound.
        /// </summary>
        public string Path
        {
            get { return this.path; }
        }
    }
}
