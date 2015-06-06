// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimerManager.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;

    using Hourglass.Properties;
    using Hourglass.Serialization;

    /// <summary>
    /// Manages timers.
    /// </summary>
    public class TimerManager : Manager
    {
        /// <summary>
        /// The maximum number of timers to persist in settings.
        /// </summary>
        public const int MaxSavedTimers = 10;

        /// <summary>
        /// Singleton instance of the <see cref="TimerManager"/> class.
        /// </summary>
        public static readonly TimerManager Instance = new TimerManager();

        /// <summary>
        /// The currently loaded timers in reverse chronological order.
        /// </summary>
        private readonly List<Timer> timers = new List<Timer>();

        /// <summary>
        /// Prevents a default instance of the <see cref="TimerManager"/> class from being created.
        /// </summary>
        private TimerManager()
        {
        }

        /// <summary>
        /// Gets a list of the currently loaded timers.
        /// </summary>
        public IList<Timer> Timers
        {
            get { return this.timers.AsReadOnly(); }
        }

        /// <summary>
        /// Gets a list of the currently loaded timers that are not bound to any <see cref="TimerWindow"/> and are not
        /// <see cref="TimerState.Stopped"/>.
        /// </summary>
        public IList<Timer> ResumableTimers
        {
            get { return this.timers.Where(t => t.State != TimerState.Stopped && !IsBoundToWindow(t)).ToList(); }
        }

        /// <summary>
        /// Initializes the class.
        /// </summary>
        public override void Initialize()
        {
            this.timers.Clear();

            IEnumerable<TimerInfo> timerInfos = Settings.Default.Timers;
            if (timerInfos != null)
            {
                this.timers.AddRange(timerInfos.Select(Timer.FromTimerInfo));
            }
        }

        /// <summary>
        /// Persists the state of the class.
        /// </summary>
        public override void Persist()
        {
            IEnumerable<TimerInfo> timerInfos = this.timers
                .Where(t => t.State != TimerState.Stopped && t.State != TimerState.Expired)
                .Take(MaxSavedTimers)
                .Select(TimerInfo.FromTimer);

            Settings.Default.Timers = new TimerInfoList(timerInfos);
        }

        /// <summary>
        /// Add a new timer.
        /// </summary>
        /// <param name="timer">A <see cref="Timer"/>.</param>
        /// <exception cref="InvalidOperationException">If the <see cref="Timer"/> has already been added.
        /// </exception>
        public void Add(Timer timer)
        {
            if (this.timers.Contains(timer))
            {
                throw new InvalidOperationException("The timer was already added.");
            }

            this.timers.Insert(0, timer);
        }

        /// <summary>
        /// Remove an existing timer.
        /// </summary>
        /// <param name="timer">A <see cref="Timer"/>.</param>
        /// <exception cref="InvalidOperationException">If the timer had not been added previously or has already been
        /// removed.</exception>
        public void Remove(Timer timer)
        {
            if (!this.timers.Contains(timer))
            {
                throw new InvalidOperationException("The timer was not found.");
            }

            this.timers.Remove(timer);
        }

        /// <summary>
        /// Removes the timer elements of the specified collection.
        /// </summary>
        /// <param name="collection">A collection of timers to remove.</param>
        public void Remove(IEnumerable<Timer> collection)
        {
            foreach (Timer timer in collection)
            {
                this.Remove(timer);
            }
        }

        /// <summary>
        /// Clears the <see cref="ResumableTimers"/>.
        /// </summary>
        public void ClearResumableTimers()
        {
            this.Remove(this.ResumableTimers);
        }

        /// <summary>
        /// Returns a value indicating whether a timer is bound to any <see cref="TimerWindow"/>.</summary>
        /// <param name="timer">A <see cref="Timer"/>.</param>
        /// <returns>A value indicating whether the timer is bound to any <see cref="TimerWindow"/>. </returns>
        private static bool IsBoundToWindow(Timer timer)
        {
            return Application.Current.Windows.OfType<TimerWindow>().Any(w => w.Timer == timer);
        }
    }
}
