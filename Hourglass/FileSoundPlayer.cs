// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileSoundPlayer.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass
{
    using System;
    using System.Windows.Media;

    /// <summary>
    /// Plays <see cref="Sound"/>s stored in the file system.
    /// </summary>
    public class FileSoundPlayer : IDisposable
    {
        #region Private Members

        /// <summary>
        /// A <see cref="MediaPlayer"/>.
        /// </summary>
        private readonly MediaPlayer mediaPlayer;

        /// <summary>
        /// A value indicating whether the player is looping the sound playback indefinitely.
        /// </summary>
        private bool isLooping;

        /// <summary>
        /// Indicates whether this object has been disposed.
        /// </summary>
        private bool disposed;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FileSoundPlayer"/> class.
        /// </summary>
        public FileSoundPlayer()
        {
            this.mediaPlayer = new MediaPlayer();
            this.mediaPlayer.MediaEnded += this.MediaPlayerOnMediaEnded;
        }

        #endregion

        #region Events

        /// <summary>
        /// Raised when sound playback has started.
        /// </summary>
        public event EventHandler PlaybackStarted;

        /// <summary>
        /// Raised when sound playback has stopped.
        /// </summary>
        public event EventHandler PlaybackStopped;

        /// <summary>
        /// Raised when sound playback has completed.
        /// </summary>
        public event EventHandler PlaybackCompleted;

        #endregion

        #region Public Methods

        /// <summary>
        /// Plays a <see cref="Sound"/> asynchronously.
        /// </summary>
        /// <param name="sound">A <see cref="Sound"/>.</param>
        /// <param name="loop">A value indicating whether playback should be looped.</param>
        /// <returns><c>true</c> if the <see cref="Sound"/> plays successfully, or <c>false</c> otherwise.</returns>
        public bool Play(Sound sound, bool loop)
        {
            this.ThrowIfDisposed();

            // Stop all playback
            if (!this.Stop())
            {
                return false;
            }

            // Do not play nothing
            if (sound == null)
            {
                return true;
            }

            // Try to play the sound
            try
            {
                this.isLooping = loop;
                this.mediaPlayer.Open(new Uri(sound.Path));
                this.mediaPlayer.Play();
            }
            catch
            {
                return false;
            }

            // Raise an event
            this.OnPlaybackStarted();
            return true;
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
                this.isLooping = false;
                this.mediaPlayer.Stop();
                this.mediaPlayer.Close();
            }
            catch
            {
                return false;
            }

            // Raise an event
            this.OnPlaybackStopped();
            return true;
        }

        /// <summary>
        /// Disposes the timer.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true /* disposing */);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Protected Methods

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
                this.mediaPlayer.Close();
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

        /// <summary>
        /// Raises the <see cref="PlaybackStarted"/> event.
        /// </summary>
        protected virtual void OnPlaybackStarted()
        {
            EventHandler eventHandler = this.PlaybackStarted;

            if (eventHandler != null)
            {
                eventHandler(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Raises the <see cref="PlaybackStopped"/> event.
        /// </summary>
        protected virtual void OnPlaybackStopped()
        {
            EventHandler eventHandler = this.PlaybackStopped;

            if (eventHandler != null)
            {
                eventHandler(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Raises the <see cref="PlaybackCompleted"/> event.
        /// </summary>
        protected virtual void OnPlaybackCompleted()
        {
            EventHandler eventHandler = this.PlaybackCompleted;

            if (eventHandler != null)
            {
                eventHandler(this, EventArgs.Empty);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Invoked when when the media has finished playback in the <see cref="MediaPlayer"/>.
        /// </summary>
        /// <param name="sender">The <see cref="MediaPlayer"/>.</param>
        /// <param name="e">The event data.</param>
        private void MediaPlayerOnMediaEnded(object sender, EventArgs e)
        {
            if (this.isLooping)
            {
                this.mediaPlayer.Position = TimeSpan.Zero;
                this.mediaPlayer.Play();
            }
            else
            {
                this.OnPlaybackCompleted();
            }
        }

        #endregion
    }
}
