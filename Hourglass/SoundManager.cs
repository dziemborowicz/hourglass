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
    using System.Media;
    using System.Reflection;

    using Hourglass.Properties;

    /// <summary>
    /// Manages and plays notification sounds.
    /// </summary>
    public class SoundManager
    {
        /// <summary>
        /// Singleton instance of the <see cref="SoundManager"/> class.
        /// </summary>
        public static readonly SoundManager Instance = new SoundManager();

        /// <summary>
        /// A <see cref="SoundPlayer"/>.
        /// </summary>
        private readonly SoundPlayer soundPlayer = new SoundPlayer();

        /// <summary>
        /// A collection of sounds in built-in resources.
        /// </summary>
        private readonly ReadOnlyCollection<SoundInfo> builtInSounds;

        /// <summary>
        /// A collection of sounds in provided by the user and stored in the application directory or in the "Sounds"
        /// folder under the application directory.
        /// </summary>
        private readonly ReadOnlyCollection<SoundInfo> customSounds;

        /// <summary>
        /// Prevents a default instance of the <see cref="SoundManager"/> class from being created.
        /// </summary>
        private SoundManager()
        {
            this.builtInSounds = new ReadOnlyCollection<SoundInfo>(this.LoadBuiltInSounds());
            this.customSounds = new ReadOnlyCollection<SoundInfo>(this.LoadCustomSounds());
        }

        /// <summary>
        /// Gets the default sound.
        /// </summary>
        public SoundInfo DefaultSound
        {
            get { return this.builtInSounds[1]; }
        }

        /// <summary>
        /// Gets a collection of sounds in built-in resources.
        /// </summary>
        public IEnumerable<SoundInfo> BuiltInSounds
        {
            get { return this.builtInSounds; }
        }

        /// <summary>
        /// Gets a collection of sounds in provided by the user and stored in the application directory or in the
        /// "Sounds" folder under the application directory.
        /// </summary>
        public IEnumerable<SoundInfo> CustomSounds
        {
            get { return this.customSounds; }
        }

        /// <summary>
        /// Returns the sound specified by the provided path, or <c>null</c> if no such sound is loaded.
        /// </summary>
        /// <param name="path">A path specifying a sound.</param>
        /// <returns>the sound specified by the provided path, or <c>null</c> if no such sound is loaded.</returns>
        public SoundInfo GetSound(string path)
        {
            return this.builtInSounds.FirstOrDefault(s => s.Path == path) ?? this.customSounds.FirstOrDefault(s => s.Path == path);
        }

        /// <summary>
        /// Returns the sound specified by the provided path, or <see cref="DefaultSound"/> if no such sound is loaded.
        /// </summary>
        /// <param name="path">A path specifying a sound.</param>
        /// <returns>the sound specified by the provided path, or <see cref="DefaultSound"/> if no such sound is
        /// loaded.</returns>
        public SoundInfo GetSoundOrDefault(string path)
        {
            return this.GetSound(path) ?? this.DefaultSound;
        }

        /// <summary>
        /// Plays a sound.
        /// </summary>
        /// <param name="sound">The sound to play.</param>
        /// <returns><c>true</c> if the sound plays successfully, or <c>false</c> otherwise.</returns>
        public bool Play(SoundInfo sound)
        {
            try
            {
                // TODO Implement this properly
                this.soundPlayer.Stream = Resources.BeepNormal;
                this.soundPlayer.Play();

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Loads the collection of sounds in built-in resources.
        /// </summary>
        /// <returns>A collection of sounds in built-in resources.</returns>
        private IList<SoundInfo> LoadBuiltInSounds()
        {
            List<SoundInfo> list = new List<SoundInfo>();
            list.Add(new SoundInfo("Loud beep", true /* isBuiltIn */, "builtin:BeepLoud.wav"));
            list.Add(new SoundInfo("Normal beep", true /* isBuiltIn */, "builtin:BeepNormal.wav"));
            list.Add(new SoundInfo("Quiet beep", true /* isBuiltIn */, "builtin:BeepQuiet.wav"));
            return list;
        }

        /// <summary>
        /// Loads the sounds in provided by the user and stored in the application directory or in the "Sounds"
        /// folder under the application directory.
        /// </summary>
        /// <returns>A collection of the sounds in provided by the user and stored in the application directory or in
        /// the "Sounds" folder under the application directory.</returns>
        private IList<SoundInfo> LoadCustomSounds()
        {
            try
            {
                List<SoundInfo> list = new List<SoundInfo>();

                string appPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? ".";
                list.AddRange(this.LoadCustomSounds(appPath, appPath));

                string soundsPath = Path.Combine(appPath, "Sounds");
                list.AddRange(this.LoadCustomSounds(appPath, soundsPath));

                list.Sort((a, b) => string.Compare(a.Name, b.Name, CultureInfo.CurrentCulture, CompareOptions.StringSort));

                return list;
            }
            catch
            {
                // Not worth reporting this
                return new List<SoundInfo>();
            }
        }

        /// <summary>
        /// Loads the sounds in the specified directory.
        /// </summary>
        /// <param name="rootDirectory">The root application directory.</param>
        /// <param name="directory">The directory from which to load the sounds.</param>
        /// <returns>A collection of the sounds in the specified directory.</returns>
        private IList<SoundInfo> LoadCustomSounds(string rootDirectory, string directory)
        {
            List<SoundInfo> list = new List<SoundInfo>();
            
            if (!Directory.Exists(directory))
            {
                return list;
            }

            foreach (string file in Directory.GetFiles(directory, "*.wav"))
            {
                string name = Path.GetFileNameWithoutExtension(file);
                string path = this.MakeCustomSoundPath(rootDirectory, file);

                list.Add(new SoundInfo(name, true /* isBuiltIn */, "custom:" + path));
            }

            return list;
        }

        /// <summary>
        /// Returns a path to the specified <paramref name="file"/> that is relative to <paramref name="directory"/>.
        /// </summary>
        /// <param name="directory">A directory path.</param>
        /// <param name="file">A file path.</param>
        /// <returns>A path to the specified <paramref name="file"/> that is relative to <paramref name="directory"/>.
        /// </returns>
        /// <exception cref="ArgumentException">If the file is not under the directory.</exception>
        private string MakeCustomSoundPath(string directory, string file)
        {
            string directoryPath = Path.GetFullPath(directory);
            string filePath = Path.GetFullPath(file);

            if (!filePath.StartsWith(directoryPath, StringComparison.Ordinal))
            {
                throw new ArgumentException("The file must be under the directory.");
            }

            return "custom:" + filePath.Substring(directoryPath.Length + 1);
        }
    }
}
