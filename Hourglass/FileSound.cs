// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileSound.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass
{
    using System;
    using System.IO;
    using System.Media;
    using System.Reflection;

    /// <summary>
    /// A <see cref="Sound"/> stored in the file system.
    /// </summary>
    public class FileSound : Sound
    {
        /// <summary>
        /// The path to the sound file.
        /// </summary>
        private string path;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileSound"/> class.
        /// </summary>
        /// <param name="path">The path to the sound file.</param>
        public FileSound(string path)
            : base(GetName(path), GetIdentifier(path))
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException("path");
            }

            this.path = path;
        }

        /// <summary>
        /// Plays the sound.
        /// </summary>
        /// <returns><c>true</c> if the sound plays successfully, or <c>false</c> otherwise.</returns>
        public override bool Play()
        {
            try
            {
                using (SoundPlayer player = new SoundPlayer(this.path))
                {
                    player.PlaySync();
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Returns the friendly name for a sound file.
        /// </summary>
        /// <param name="path">The path to the sound file.</param>
        /// <returns>The friendly name for a sound file.</returns>
        protected static string GetName(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException("path");
            }

            return Path.GetFileNameWithoutExtension(path);
        }

        /// <summary>
        /// Returns the unique identifier for a sound file.
        /// </summary>
        /// <param name="path">The path to the sound file.</param>
        /// <returns>The unique identifier for a sound file.</returns>
        protected static string GetIdentifier(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException("path");
            }

            string appDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? ".";
            string fullPath = Path.GetFullPath(path);

            // Return a relative path if the sound is in or under the app directory, or otherwise return the full path
            return fullPath.StartsWith(appDirectory, StringComparison.OrdinalIgnoreCase)
                ? "file:///." + fullPath.Substring(appDirectory.Length)
                : "file:///" + path;
        }
    }
}
