// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SoundManager.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass.Managers
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    using Hourglass.Properties;
    using Hourglass.Timing;

    /// <summary>
    /// Manages notification sounds.
    /// </summary>
    public class SoundManager : Manager
    {
        /// <summary>
        /// Singleton instance of the <see cref="SoundManager"/> class.
        /// </summary>
        public static readonly SoundManager Instance = new SoundManager();

        /// <summary>
        /// The extensions of the supported sound files.
        /// </summary>
        private static readonly string[] SupportedTypes =
        {
            "*.aac",
            "*.m4a",
            "*.mid",
            "*.midi",
            "*.mp3",
            "*.wav",
            "*.wma"
        };

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
            get { return this.GetSoundByIdentifier("resource:Normal beep"); }
        }

        /// <summary>
        /// Gets a collection of all sounds.
        /// </summary>
        public IList<Sound> AllSounds
        {
            get { return this.sounds.ToList(); }
        }

        /// <summary>
        /// Gets a collection of the sounds stored in the assembly.
        /// </summary>
        public IList<Sound> BuiltInSounds
        {
            get { return this.sounds.Where(s => s.IsBuiltIn).ToList(); }
        }

        /// <summary>
        /// Gets a collection of the sounds stored in the file system.
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
            this.sounds.Clear();
            this.sounds.AddRange(this.GetBuiltInSounds());
            this.sounds.AddRange(this.GetUserProvidedSounds());
        }

        /// <summary>
        /// Returns the sound for the specified identifier, or <c>null</c> if no such sound is loaded.
        /// </summary>
        /// <param name="identifier">The identifier for the sound.</param>
        /// <returns>The sound for the specified identifier, or <c>null</c> if no such sound is loaded.</returns>
        public Sound GetSoundByIdentifier(string identifier)
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
        public Sound GetSoundOrDefaultByIdentifier(string identifier)
        {
            if (string.IsNullOrEmpty(identifier))
            {
                return null;
            }

            return this.GetSoundByIdentifier(identifier) ?? this.DefaultSound;
        }

        /// <summary>
        /// Returns the first sound for the specified name, or <c>null</c> if no such sound is loaded.
        /// </summary>
        /// <param name="name">The name for the sound.</param>
        /// <param name="stringComparison">One of the enumeration values that specifies how the strings will be
        /// compared.</param>
        /// <returns>The first sound for the specified name, or <c>null</c> if no such sound is loaded.</returns>
        public Sound GetSoundByName(string name, StringComparison stringComparison = StringComparison.Ordinal)
        {
            if (string.IsNullOrEmpty(name))
            {
                return null;
            }

            return this.sounds.FirstOrDefault(s => string.Equals(s.Name, name, stringComparison));
        }

        /// <summary>
        /// Returns the first sound for the specified name, or <see cref="DefaultSound"/> if no such sound is loaded.
        /// </summary>
        /// <param name="name">The name for the sound.</param>
        /// <param name="stringComparison">One of the enumeration values that specifies how the strings will be
        /// compared.</param>
        /// <returns>The first sound for the specified name, or <see cref="DefaultSound"/> if no such sound is loaded.
        /// </returns>
        public Sound GetSoundOrDefaultByName(string name, StringComparison stringComparison = StringComparison.Ordinal)
        {
            if (string.IsNullOrEmpty(name))
            {
                return null;
            }

            return this.GetSoundByName(name, stringComparison) ?? this.DefaultSound;
        }

        /// <summary>
        /// Loads the collection of sounds stored in the assembly.
        /// </summary>
        /// <returns>A collection of sounds stored in the assembly.</returns>
        private IList<Sound> GetBuiltInSounds()
        {
            return new List<Sound>
            {
                new Sound(
                    "Loud beep",
                    Resources.SoundManagerLoudBeep,
                    () => Resources.BeepLoud,
                    TimeSpan.FromMilliseconds(600)),

                new Sound(
                    "Normal beep",
                    Resources.SoundManagerNormalBeep,
                    () => Resources.BeepNormal,
                    TimeSpan.FromMilliseconds(600)),

                new Sound(
                    "Quiet beep",
                    Resources.SoundManagerQuietBeep,
                    () => Resources.BeepQuiet,
                    TimeSpan.FromMilliseconds(600))
            };
        }

        /// <summary>
        /// Loads the collection of sounds stored in the file system.
        /// </summary>
        /// <returns>A collection of sounds stored in the file system.</returns>
        private IList<Sound> GetUserProvidedSounds()
        {
            try
            {
                string appDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? ".";
                string soundsDirectory = Path.Combine(appDirectory, "Sounds");

                List<Sound> list = new List<Sound>();
                list.AddRange(this.GetUserProvidedSounds(appDirectory));
                list.AddRange(this.GetUserProvidedSounds(soundsDirectory));
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
        private IList<Sound> GetUserProvidedSounds(string path)
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
