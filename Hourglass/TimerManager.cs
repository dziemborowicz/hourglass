// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimerManager.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    using Hourglass.Properties;

    /// <summary>
    /// Tracks currently loaded <see cref="Timer"/> objects.
    /// </summary>
    public class TimerManager
    {
        /// <summary>
        /// The maximum number of <see cref="Timer"/> objects to persist in settings.
        /// </summary>
        public const int MaxSavedTimers = 25;

        /// <summary>
        /// The maximum number of <see cref="TimerInput"/> objects to track and persist in settings.
        /// </summary>
        public const int MaxInputs = 5;

        /// <summary>
        /// Singleton instance of the <see cref="TimerManager"/> class.
        /// </summary>
        public static readonly TimerManager Instance = new TimerManager();

        /// <summary>
        /// The currently loaded <see cref="Timer"/> objects in reverse chronological order.
        /// </summary>
        private readonly List<Timer> timers = new List<Timer>();

        /// <summary>
        /// The most recent <see cref="TimerInput"/> objects in reverse chronological order.
        /// </summary>
        private readonly List<TimerInput> inputs = new List<TimerInput>(); 

        /// <summary>
        /// Prevents a default instance of the <see cref="TimerManager"/> class from being created.
        /// </summary>
        private TimerManager()
        {
        }

        /// <summary>
        /// Gets a list of the currently loaded <see cref="Timer"/> objects.
        /// </summary>
        public IList<Timer> Timers
        {
            get { return new ReadOnlyCollection<Timer>(this.timers); }
        }

        /// <summary>
        /// Gets a list of the most recent <see cref="TimerInput"/> objects.
        /// </summary>
        public IList<TimerInput> Inputs
        {
            get { return new ReadOnlyCollection<TimerInput>(this.inputs); }
        }

        /// <summary>
        /// Add a new <see cref="Timer"/>.
        /// </summary>
        /// <param name="timer">A <see cref="Timer"/>.</param>
        /// <exception cref="InvalidOperationException">If the <see cref="Timer"/> has already been added.</exception>
        public void Add(Timer timer)
        {
            if (this.timers.Contains(timer))
            {
                throw new InvalidOperationException("The Timer was already added.");
            }

            this.timers.Insert(0, timer);
        }

        /// <summary>
        /// Remove an existing <see cref="Timer"/>.
        /// </summary>
        /// <param name="timer">A <see cref="Timer"/>.</param>
        /// <exception cref="InvalidOperationException">If the timer had not been added previously or has already been
        /// removed.</exception>
        public void Remove(Timer timer)
        {
            if (!this.timers.Contains(timer))
            {
                throw new InvalidOperationException("The Timer was not found.");
            }

            this.timers.Remove(timer);
        }

        /// <summary>
        /// Removes the <see cref="Timer"/> elements of the specified collection.
        /// </summary>
        /// <param name="collection">A collection of <see cref="Timer"/> objects to remove.</param>
        public void Remove(IEnumerable<Timer> collection)
        {
            foreach (Timer timer in collection)
            {
                this.Remove(timer);
            }
        }

        /// <summary>
        /// Adds a <see cref="TimerInput"/> to the list of recent inputs.
        /// </summary>
        /// <param name="input">A <see cref="TimerInput"/>.</param>
        public void AddInput(TimerInput input)
        {
            this.inputs.Remove(input);
            this.inputs.Insert(0, input);

            while (this.inputs.Count > MaxInputs)
            {
                this.inputs.RemoveAt(this.inputs.Count - 1);
            }
        }

        /// <summary>
        /// Clears the list of recent <see cref="TimerInput"/> objects.
        /// </summary>
        public void ClearInputs()
        {
            this.inputs.Clear();
        }

        /// <summary>
        /// Saves state to the default settings.
        /// </summary>
        public void SaveToSettings()
        {
            IEnumerable<TimerInfo> timerInfos = this.timers
                .Where(t => t.State != TimerState.Stopped && t.State != TimerState.Expired)
                .Take(MaxSavedTimers)
                .Select(t => t.ToTimerInfo());

            Settings.Default.Timers = new TimerInfoList(timerInfos);
            Settings.Default.Inputs = new TimerInputList(this.inputs);
            Settings.Default.Save();
        }

        /// <summary>
        /// Loads state from the default settings.
        /// </summary>
        public void LoadFromSettings()
        {
            if (this.timers.Count != 0)
            {
                throw new InvalidOperationException("Cannot load timers if timers are already loaded.");
            }

            IEnumerable<TimerInfo> timerInfos = Settings.Default.Timers;
            if (timerInfos != null)
            {
                this.timers.AddRange(timerInfos.Select(ti => new Timer(ti)));
            }

            IEnumerable<TimerInput> savedInputs = Settings.Default.Inputs;
            if (savedInputs != null)
            {
                this.inputs.AddRange(savedInputs);
            }
        }
    }
}
