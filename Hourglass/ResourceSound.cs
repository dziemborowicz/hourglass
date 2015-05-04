// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ResourceSound.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass
{
    using System;
    using System.IO;
    using System.Media;

    /// <summary>
    /// A <see cref="Sound"/> stored in the assembly.
    /// </summary>
    public class ResourceSound : Sound
    {
        /// <summary>
        /// A method that returns a stream to the sound data.
        /// </summary>
        private readonly Func<UnmanagedMemoryStream> streamProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceSound"/> class.
        /// </summary>
        /// <param name="name">The friendly name for the sound.</param>
        /// <param name="streamProvider">A method that returns a stream to the sound data.</param>
        public ResourceSound(string name, Func<UnmanagedMemoryStream> streamProvider)
            : base(name, "resx://" + name)
        {
            if (streamProvider == null)
            {
                throw new ArgumentNullException("streamProvider");
            }

            this.streamProvider = streamProvider;
        }

        /// <summary>
        /// Plays the sound.
        /// </summary>
        /// <returns><c>true</c> if the sound plays successfully, or <c>false</c> otherwise.</returns>
        public override bool Play()
        {
            try
            {
                using (UnmanagedMemoryStream stream = this.streamProvider())
                using (SoundPlayer player = new SoundPlayer(stream))
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
    }
}
