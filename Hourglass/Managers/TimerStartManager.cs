// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimerStartManager.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass.Managers
{
    using System.Collections.Generic;
    using System.Linq;

    using Hourglass.Properties;
    using Hourglass.Timing;

    /// <summary>
    /// Manages recent <see cref="TimerStart"/> objects.
    /// </summary>
    public class TimerStartManager : Manager
    {
        /// <summary>
        /// The maximum number of <see cref="TimerStart"/>s.
        /// </summary>
        public const int Capacity = 10;

        /// <summary>
        /// Singleton instance of the <see cref="TimerStartManager"/> class.
        /// </summary>
        public static readonly TimerStartManager Instance = new TimerStartManager();

        /// <summary>
        /// The most recent <see cref="TimerStart"/> objects in reverse chronological order.
        /// </summary>
        private readonly List<TimerStart> timerStarts = new List<TimerStart>(Capacity);

        /// <summary>
        /// Prevents a default instance of the <see cref="TimerStartManager"/> class from being created.
        /// </summary>
        private TimerStartManager()
        {
        }

        /// <summary>
        /// Gets a list of the most recent <see cref="TimerStart"/> objects in reverse chronological order.
        /// </summary>
        public IList<TimerStart> TimerStarts
        {
            get { return this.timerStarts.Where(e => e.IsCurrent).ToList(); }
        }

        /// <summary>
        /// Gets the most recent <see cref="TimerStart"/>, or the default <see cref="TimerStart"/> if there are no <see
        /// cref="TimerStart"/> objects in <see cref="TimerStarts"/>.
        /// </summary>
        public TimerStart LastTimerStart
        {
            get { return this.timerStarts.FirstOrDefault(e => e.IsCurrent) ?? TimerStart.Default; }
        }

        /// <summary>
        /// Initializes the class.
        /// </summary>
        public override void Initialize()
        {
            this.timerStarts.Clear();
            this.timerStarts.AddRange(Settings.Default.TimerStarts);
        }

        /// <summary>
        /// Persists the state of the class.
        /// </summary>
        public override void Persist()
        {
            Settings.Default.TimerStarts = this.timerStarts;
        }

        /// <summary>
        /// Adds a <see cref="TimerStart"/> to the list of recent <see cref="TimerStart"/> objects.
        /// </summary>
        /// <param name="timerStart">A <see cref="TimerStart"/>.</param>
        public void Add(TimerStart timerStart)
        {
            // Remove all equivalent objects
            this.timerStarts.RemoveAll(e => e.ToString() == timerStart.ToString());

            // Add the object to the top of the list
            this.timerStarts.Insert(0, timerStart);

            // Limit the number of objects in the list
            while (this.timerStarts.Count > Capacity)
            {
                this.timerStarts.RemoveAt(this.timerStarts.Count - 1);
            }
        }

        /// <summary>
        /// Clears the list of recent <see cref="TimerStart"/> objects.
        /// </summary>
        public void Clear()
        {
            this.timerStarts.Clear();
        }
    }
}
