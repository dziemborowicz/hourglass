// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimerInputManager.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    using Hourglass.Properties;

    /// <summary>
    /// Manages <see cref="TimerInput"/>s.
    /// </summary>
    public class TimerInputManager
    {
        /// <summary>
        /// The maximum number of <see cref="TimerInput"/>s.
        /// </summary>
        public const int Capacity = 5;

        /// <summary>
        /// Singleton instance of the <see cref="TimerManager"/> class.
        /// </summary>
        public static readonly TimerInputManager Instance = new TimerInputManager();

        /// <summary>
        /// The most recent <see cref="TimerInput"/>s in reverse chronological order.
        /// </summary>
        private readonly List<TimerInput> timerInputs = new List<TimerInput>(); 

        /// <summary>
        /// Prevents a default instance of the <see cref="TimerInputManager"/> class from being created.
        /// </summary>
        private TimerInputManager()
        {
        }

        /// <summary>
        /// Gets a list of the most recent <see cref="TimerInput"/>s in reverse chronological order.
        /// </summary>
        public IList<TimerInput> Inputs
        {
            get { return new ReadOnlyCollection<TimerInput>(this.timerInputs); }
        }

        /// <summary>
        /// Adds a <see cref="TimerInput"/> to the list of recent inputs.
        /// </summary>
        /// <param name="input">A <see cref="TimerInput"/>.</param>
        public void Add(TimerInput input)
        {
            // Insert or move to the top of the list
            this.timerInputs.Remove(input);
            this.timerInputs.Insert(0, input);

            // Limit the number of inputs in the list
            while (this.timerInputs.Count > Capacity)
            {
                this.timerInputs.RemoveAt(this.timerInputs.Count - 1);
            }
        }

        /// <summary>
        /// Clears the list of recent <see cref="TimerInput"/>s.
        /// </summary>
        public void Clear()
        {
            this.timerInputs.Clear();
        }

        /// <summary>
        /// Saves state to the default settings.
        /// </summary>
        public void Save()
        {
            IEnumerable<TimerInputInfo> timerInputInfos = this.timerInputs.Select(TimerInputInfo.FromTimerInput);
            Settings.Default.Inputs = new TimerInputInfoList(timerInputInfos);
        }

        /// <summary>
        /// Loads state from the default settings.
        /// </summary>
        public void Load()
        {
            this.timerInputs.Clear();

            IEnumerable<TimerInputInfo> timerInputInfos = Settings.Default.Inputs;
            if (timerInputInfos != null)
            {
                this.timerInputs.AddRange(timerInputInfos.Select(TimerInput.FromTimerInputInfo));
            }
        }
    }
}
