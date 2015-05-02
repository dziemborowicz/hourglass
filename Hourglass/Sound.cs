// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Sound.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass
{
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// A sound that can be used to notify the user that a <see cref="Timer"/> has expired.
    /// </summary>
    public abstract class Sound
    {
        /// <summary>
        /// The friendly name for this sound.
        /// </summary>
        private readonly string name;

        /// <summary>
        /// A unique identifier for this sound.
        /// </summary>
        private readonly string identifier;

        /// <summary>
        /// Initializes a new instance of the <see cref="Sound"/> class.
        /// </summary>
        /// <param name="name">The friendly name for the sound.</param>
        /// <param name="identifier">A unique identifier for the sound.</param>
        protected Sound(string name, string identifier)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }

            if (identifier == null)
            {
                throw new ArgumentNullException("identifier");
            }

            this.name = name;
            this.identifier = identifier;
        }

        /// <summary>
        /// Gets the friendly name for this sound.
        /// </summary>
        public string Name
        {
            get { return this.name; }
        }

        /// <summary>
        /// Gets the unique identifier for this sound.
        /// </summary>
        public string Identifier
        {
            get { return this.identifier; }
        }

        /// <summary>
        /// Plays the sound.
        /// </summary>
        /// <returns><c>true</c> if the sound plays successfully, or <c>false</c> otherwise.</returns>
        public abstract bool Play();

        /// <summary>
        /// Plays the sound asynchronously.
        /// </summary>
        /// <returns><c>true</c> if the sound plays successfully, or <c>false</c> otherwise.</returns>
        public Task<bool> PlayAsync()
        {
            Task<bool> task = new Task<bool>(this.Play);
            task.Start();
            return task;
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">An <see cref="object"/>.</param>
        /// <returns><c>true</c> if the specified object is equal to the current object, or <c>false</c> otherwise.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (object.ReferenceEquals(obj, null))
            {
                return false;
            }

            if (object.ReferenceEquals(this, obj))
            {
                return true;
            }

            if (this.GetType() != obj.GetType())
            {
                return false;
            }

            Sound sound = (Sound)obj;
            return object.Equals(this.identifier, sound.identifier);
        }

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            int hashCode = 17;
            hashCode = (31 * hashCode) + this.identifier.GetHashCode();
            return hashCode;
        }
    }
}
