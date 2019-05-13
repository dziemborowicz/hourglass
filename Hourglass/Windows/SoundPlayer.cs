// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SoundPlayer.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass.Windows
{
    using System;
    using System.Windows.Media;
    using System.Windows.Threading;
    using Hourglass.Managers;
    using Hourglass.Timing;

    /// <summary>
    /// Plays <see cref="Sound"/>s stored in the file system.
    /// </summary>
    public class SoundPlayer : IDisposable
    {
        #region Private Members

        /// <summary>
        /// A <see cref="System.Media.SoundPlayer"/> that can play *.wav files.
        /// </summary>
        private readonly System.Media.SoundPlayer soundPlayer;

        /// <summary>
        /// A <see cref="DispatcherTimer"/> used to raise events.
        /// </summary>
        private readonly DispatcherTimer dispatcherTimer;

        /// <summary>
        /// A <see cref="MediaPlayer"/> that can play most sound files.
        /// </summary>
        private readonly MediaPlayer mediaPlayer;

        /// <summary>
        /// Indicates whether this object has been disposed.
        /// </summary>
        private bool disposed;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SoundPlayer"/> class.
        /// </summary>
        public SoundPlayer()
        {
            // Resource sound player
            this.soundPlayer = new System.Media.SoundPlayer();

            this.dispatcherTimer = new DispatcherTimer();
            this.dispatcherTimer.Interval = TimeSpan.FromSeconds(1);
            this.dispatcherTimer.Tick += this.DispatcherTimerTick;

            // File sound player
            this.mediaPlayer = new MediaPlayer();
            this.mediaPlayer.MediaEnded += this.MediaPlayerOnMediaEnded;
            this.mediaPlayer.MediaFailed += this.MediaPlayerOnMediaFailed;
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

        #region Properties

        /// <summary>
        /// Gets a value indicating whether the player is playing a sound.
        /// </summary>
        public bool IsPlaying { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the player is looping the sound playback indefinitely.
        /// </summary>
        public bool IsLooping { get; private set; }

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

            try
            {
                this.IsPlaying = true;
                this.IsLooping = loop;

                if (sound.IsBuiltIn)
                {
                    // Use the sound player
                    this.soundPlayer.Stream = sound.GetStream();

                    if (loop)
                    {
                        // Asynchronously play looping sound
                        this.soundPlayer.PlayLooping();
                    }
                    else
                    {
                        // Asynchronously play sound once
                        this.soundPlayer.Play();

                        // Start a timer to notify the completion of playback if we know the duration
                        if (sound.Duration.HasValue)
                        {
                            this.dispatcherTimer.Interval = sound.Duration.Value;
                            this.dispatcherTimer.Start();
                        }
                    }
                }
                else
                {
                    // Use the media player
                    this.mediaPlayer.Open(new Uri(sound.Path));
                    this.mediaPlayer.Play();
                }
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
                this.IsPlaying = false;
                this.IsLooping = false;

                // Stop the sound player
                this.soundPlayer.Stop();
                this.dispatcherTimer.Stop();

                if (this.soundPlayer.Stream != null)
                {
                    this.soundPlayer.Stream.Dispose();
                    this.soundPlayer.Stream = null;
                }

                // Stop the media player
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
        protected virtual void Dispose(bool disposing)
        {
            if (this.disposed)
            {
                return;
            }

            this.disposed = true;

            if (disposing)
            {
                this.IsPlaying = false;
                this.IsLooping = false;

                // Dispose the sound player
                this.soundPlayer.Stop();
                this.soundPlayer.Dispose();

                if (this.soundPlayer.Stream != null)
                {
                    this.soundPlayer.Stream.Dispose();
                }

                this.dispatcherTimer.Stop();

                // Dispose the media player
                this.mediaPlayer.Stop();
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
        /// Invoked when the <see cref="DispatcherTimer"/> interval has elapsed.
        /// </summary>
        /// <param name="sender">The <see cref="DispatcherTimer"/>.</param>
        /// <param name="e">The event data.</param>
        private void DispatcherTimerTick(object sender, EventArgs e)
        {
            this.dispatcherTimer.Stop();
            this.OnPlaybackCompleted();
        }

        /// <summary>
        /// Invoked when when the media has finished playback in the <see cref="MediaPlayer"/>.
        /// </summary>
        /// <param name="sender">The <see cref="MediaPlayer"/>.</param>
        /// <param name="e">The event data.</param>
        private void MediaPlayerOnMediaEnded(object sender, EventArgs e)
        {
            if (this.IsLooping)
            {
                this.mediaPlayer.Position = TimeSpan.Zero;
                this.mediaPlayer.Play();
            }
            else
            {
                this.OnPlaybackCompleted();
            }
        }

        /// <summary>
        /// Invoked when an error is encountered in the <see cref="MediaPlayer"/>.
        /// </summary>
        /// <param name="sender">The <see cref="MediaPlayer"/>.</param>
        /// <param name="e">The event data.</param>
        private void MediaPlayerOnMediaFailed(object sender, ExceptionEventArgs e)
        {
            ErrorManager.Instance.ReportError(e.ErrorException.ToString());
        }

        #endregion
    }
}
