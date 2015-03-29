// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SoundManager.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass
{
    using System.Media;

    using Hourglass.Properties;

    /// <summary>
    /// Manages and plays notification sounds.
    /// </summary>
    public class SoundManager
    {
        /// <summary>
        /// A <see cref="SoundPlayer"/>.
        /// </summary>
        private readonly SoundPlayer soundPlayer = new SoundPlayer();

        /// <summary>
        /// Plays the notification sound.
        /// </summary>
        /// <returns><c>true</c> if the sound plays successfully, or <c>false</c> otherwise.</returns>
        public bool PlayNotificationSound()
        {
            try
            {
                this.soundPlayer.Stream = Resources.Beep;
                this.soundPlayer.Play();

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
