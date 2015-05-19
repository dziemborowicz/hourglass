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
    using Hourglass.Serialization;

    /// <summary>
    /// Manages recent <see cref="TimerInput"/>s.
    /// </summary>
    public class TimerInputManager
    {
        /// <summary>
        /// The maximum number of <see cref="TimerInput"/>s.
        /// </summary>
        public const int Capacity = 10;

        /// <summary>
        /// Singleton instance of the <see cref="TimerInputManager"/> class.
        /// </summary>
        public static readonly TimerInputManager Instance = new TimerInputManager();

        /// <summary>
        /// The most recent <see cref="TimerInput"/>s in reverse chronological order.
        /// </summary>
        private readonly List<TimerInput> timerInputs = new List<TimerInput>(Capacity);

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
        /// Gets the most recent <see cref="TimerInput"/>, or <c>null</c> if there are no <see cref="TimerInput"/>s.
        /// </summary>
        public TimerInput LastInput
        {
            get { return this.timerInputs.FirstOrDefault(); }
        }

        /// <summary>
        /// Adds a <see cref="TimerInput"/> to the list of recent inputs.
        /// </summary>
        /// <param name="input">A <see cref="TimerInput"/>.</param>
        public void Add(TimerInput input)
        {
            // Remove all equivalent inputs
            this.timerInputs.Remove(input);

            // Add the input to the top of the list
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

        /// <summary>
        /// Saves state to the default settings.
        /// </summary>
        public void Save()
        {
            IEnumerable<TimerInputInfo> timerInputInfos = this.timerInputs.Select(TimerInputInfo.FromTimerInput);
            Settings.Default.Inputs = new TimerInputInfoList(timerInputInfos);
        }
    }
}
