// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SoundPlayer.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass
{
    using System;

    /// <summary>
    /// Plays <see cref="Sound"/>s.
    /// </summary>
    public class SoundPlayer : IDisposable
    {
        /// <summary>
        /// A <see cref="System.Media.SoundPlayer"/> that can be used to play *.wav files.
        /// </summary>
        private readonly System.Media.SoundPlayer soundPlayer = new System.Media.SoundPlayer();

        /// <summary>
        /// Indicates whether this object has been disposed.
        /// </summary>
        private bool disposed;

        /// <summary>
        /// Plays a <see cref="Sound"/> asynchronously.
        /// </summary>
        /// <param name="sound">A <see cref="Sound"/>.</param>
        /// <param name="loop">A value indicating whether playback should be looped.</param>
        /// <returns><c>true</c> if the <see cref="Sound"/> plays successfully, or <c>false</c> otherwise.</returns>
        public bool Play(Sound sound, bool loop)
        {
            this.ThrowIfDisposed();

            try
            {
                if (!this.Stop())
                {
                    return false;
                }

                if (sound == null)
                {
                    return true;
                }

                this.soundPlayer.Stream = sound.GetStream();

                if (loop)
                {
                    this.soundPlayer.PlayLooping();
                }
                else
                {
                    this.soundPlayer.Play();
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Stops playback of a <see cref="Sound"/> if playback is occurring.
        /// </summary>
        /// <returns><c>true</c> if playback is stopped successfully or no playback was occurring, or <c>false</c>
        /// otherwise.</returns>
        public bool Stop()
        {
            this.ThrowIfDisposed();

            try
            {
                this.soundPlayer.Stop();

                if (this.soundPlayer.Stream != null)
                {
                    this.soundPlayer.Stream.Dispose();
                    this.soundPlayer.Stream = null;
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Disposes the timer.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true /* disposing */);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes the timer.
        /// </summary>
        /// <param name="disposing">A value indicating whether this method was invoked by an explicit call to <see
        /// cref="Dispose"/>.</param>
        protected void Dispose(bool disposing)
        {
            if (this.disposed)
            {
                return;
            }

            this.disposed = true;

            if (disposing)
            {
                this.soundPlayer.Dispose();

                if (this.soundPlayer.Stream != null)
                {
                    this.soundPlayer.Stream.Dispose();
                }
            }
        }

        /// <summary>
        /// Throws a <see cref="ObjectDisposedException"/> if the object has been disposed.
        /// </summary>
        protected void ThrowIfDisposed()
        {
            if (this.disposed)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }
        }
    }
}
