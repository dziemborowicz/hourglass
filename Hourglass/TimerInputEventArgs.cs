// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimerInputEventArgs.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass
{
    using System;

    /// <summary>
    /// Event data that specifies a <see cref="TimerInput"/>.
    /// </summary>
    public class TimerInputEventArgs : EventArgs
    {
        /// <summary>
        /// The <see cref="TimerInput"/>.
        /// </summary>
        private TimerInput input;

        /// <summary>
        /// Initializes a new instance of the <see cref="TimerInputEventArgs"/> class.
        /// </summary>
        /// <param name="input">A <see cref="TimerInput"/>.</param>
        public TimerInputEventArgs(TimerInput input)
        {
            this.input = input;
        }

        /// <summary>
        /// Gets the <see cref="TimerInput"/>.
        /// </summary>
        public TimerInput Input
        {
            get { return this.input; }
        }
    }
}
