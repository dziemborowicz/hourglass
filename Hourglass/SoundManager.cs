// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SoundManager.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    using Hourglass.Properties;

    /// <summary>
    /// Manages notification sounds.
    /// </summary>
    public class SoundManager
    {
        /// <summary>
        /// Singleton instance of the <see cref="SoundManager"/> class.
        /// </summary>
        public static readonly SoundManager Instance = new SoundManager();

        /// <summary>
        /// A collection of sounds.
        /// </summary>
        private readonly List<Sound> sounds = new List<Sound>(); 

        /// <summary>
        /// Prevents a default instance of the <see cref="SoundManager"/> class from being created.
        /// </summary>
        private SoundManager()
        {
            this.sounds.AddRange(this.GetResourceSounds());
            this.sounds.AddRange(this.GetFileSounds());
        }

        /// <summary>
        /// Gets the default sound.
        /// </summary>
        public Sound DefaultSound
        {
            get { return this.GetSound("resource:Normal beep"); }
        }

        /// <summary>
        /// Gets a collection of sounds stored in the assembly.
        /// </summary>
        public IEnumerable<Sound> BuiltInSounds
        {
            get { return this.sounds.Where(s => s.IsBuiltIn); }
        }

        /// <summary>
        /// Gets a collection of sounds stored in the file system.
        /// </summary>
        public IEnumerable<Sound> UserProvidedSounds
        {
            get { return this.sounds.Where(s => !s.IsBuiltIn); }
        }

        /// <summary>
        /// Returns the sound for the specified identifier, or <c>null</c> if no such sound is loaded.
        /// </summary>
        /// <param name="identifier">The identifier for the sound.</param>
        /// <returns>The sound for the specified identifier, or <c>null</c> if no such sound is loaded.</returns>
        public Sound GetSound(string identifier)
        {
            if (string.IsNullOrEmpty(identifier))
            {
                return null;
            }

            return this.sounds.FirstOrDefault(s => s.Identifier == identifier);
        }

        /// <summary>
        /// Returns the sound for the specified identifier, or <see cref="DefaultSound"/> if no such sound is loaded.
        /// </summary>
        /// <param name="identifier">The identifier for the sound.</param>
        /// <returns>The sound for the specified identifier, or <see cref="DefaultSound"/> if no such sound is loaded.
        /// </returns>
        public Sound GetSoundOrDefault(string identifier)
        {
            if (string.IsNullOrEmpty(identifier))
            {
                return null;
            }

            return this.GetSound(identifier) ?? this.DefaultSound;
        }

        /// <summary>
        /// Loads the collection of sounds stored in the assembly.
        /// </summary>
        /// <returns>A collection of sounds stored in the assembly.</returns>
        private IList<Sound> GetResourceSounds()
        {
            List<Sound> list = new List<Sound>();
            list.Add(new Sound("Loud beep", () => Resources.BeepLoud, TimeSpan.FromMilliseconds(600)));
            list.Add(new Sound("Normal beep", () => Resources.BeepNormal, TimeSpan.FromMilliseconds(600)));
            list.Add(new Sound("Quiet beep", () => Resources.BeepQuiet, TimeSpan.FromMilliseconds(600)));
            return list;
        }

        /// <summary>
        /// Loads the collection of sounds stored in the file system.
        /// </summary>
        /// <returns>A collection of sounds stored in the file system.</returns>
        private IList<Sound> GetFileSounds()
        {
            try
            {
                List<Sound> list = new List<Sound>();

                string appDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? ".";
                if (Directory.Exists(appDirectory))
                {
                    IEnumerable<string> paths = Directory.GetFiles(appDirectory, "*.wav");
                    IEnumerable<Sound> fileSounds = paths.Select(path => new Sound(path));
                    list.AddRange(fileSounds);
                }

                string soundsDirectory = Path.Combine(appDirectory, "Sounds");
                if (Directory.Exists(soundsDirectory))
                {
                    IEnumerable<string> paths = Directory.GetFiles(soundsDirectory, "*.wav");
                    IEnumerable<Sound> fileSounds = paths.Select(path => new Sound(path));
                    list.AddRange(fileSounds);
                }

                list.Sort((a, b) => string.Compare(a.Name, b.Name, CultureInfo.CurrentCulture, CompareOptions.StringSort));

                return list;
            }
            catch
            {
                // Not worth raising an exception
                return new List<Sound>();
            }
        }
    }
}
