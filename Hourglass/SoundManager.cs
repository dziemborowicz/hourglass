// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SoundManager.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    using Hourglass.Properties;

    /// <summary>
    /// Manages notification sounds.
    /// </summary>
    public class SoundManager : Manager
    {
        /// <summary>
        /// The extensions of the supported sound files.
        /// </summary>
        public static readonly ReadOnlyCollection<string> SupportedTypes = new ReadOnlyCollection<string>(new[] { "*.aac", "*.m4a", "*.mid", "*.midi", "*.mp3", "*.wav", "*.wma" });

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
        public IList<Sound> BuiltInSounds
        {
            get { return this.sounds.Where(s => s.IsBuiltIn).ToList(); }
        }

        /// <summary>
        /// Gets a collection of sounds stored in the file system.
        /// </summary>
        public IList<Sound> UserProvidedSounds
        {
            get { return this.sounds.Where(s => !s.IsBuiltIn).ToList(); }
        }

        /// <summary>
        /// Initializes the class.
        /// </summary>
        public override void Initialize()
        {
            this.sounds.AddRange(this.GetResourceSounds());
            this.sounds.AddRange(this.GetFileSounds());
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
        /// Returns the first sound for the specified name, or <c>null</c> if no such sound is loaded.
        /// </summary>
        /// <param name="name">The name for the sound.</param>
        /// <returns>The first sound for the specified name, or <c>null</c> if no such sound is loaded.</returns>
        public Sound GetSoundByName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return null;
            }

            return this.sounds.FirstOrDefault(s => s.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase));
        }

        /// <summary>
        /// Returns the first sound for the specified name, or <see cref="DefaultSound"/> if no such sound is loaded.
        /// </summary>
        /// <param name="name">The name for the sound.</param>
        /// <returns>The first sound for the specified name, or <see cref="DefaultSound"/> if no such sound is loaded.
        /// </returns>
        public Sound GetSoundOrDefaultByName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return null;
            }

            return this.GetSoundByName(name) ?? this.DefaultSound;
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
                string appDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? ".";
                string soundsDirectory = Path.Combine(appDirectory, "Sounds");

                List<Sound> list = new List<Sound>();
                list.AddRange(this.GetFileSounds(appDirectory));
                list.AddRange(this.GetFileSounds(soundsDirectory));
                list.Sort((a, b) => string.Compare(a.Name, b.Name, CultureInfo.CurrentCulture, CompareOptions.StringSort));
                return list;
            }
            catch
            {
                // Not worth raising an exception
                return new List<Sound>();
            }
        }

        /// <summary>
        /// Loads the collection of sounds stored in the file system at the specified path.
        /// </summary>
        /// <param name="path">A path to a directory.</param>
        /// <returns>A collection of sounds stored in the file system at the specified path.</returns>
        private IList<Sound> GetFileSounds(string path)
        {
            try
            {
                List<Sound> list = new List<Sound>();

                if (Directory.Exists(path))
                {
                    foreach (string supportedType in SupportedTypes)
                    {
                        IEnumerable<string> filePaths = Directory.GetFiles(path, supportedType);
                        IEnumerable<Sound> fileSounds = filePaths.Select(p => new Sound(p));
                        list.AddRange(fileSounds);
                    }
                }

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
