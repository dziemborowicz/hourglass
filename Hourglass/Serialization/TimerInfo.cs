// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimerInfo.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass.Serialization
{
    using System;
    using System.Xml.Serialization;

    /// <summary>
    /// A representation of the state of a <see cref="Timer"/>.
    /// </summary>
    [XmlInclude(typeof(HourglassTimerInfo))]
    [XmlInclude(typeof(DateTimeTimerInfo))]
    [XmlInclude(typeof(TimeSpanTimerInfo))]
    public abstract class TimerInfo
    {
        /// <summary>
        /// Gets or sets the <see cref="TimerState"/> of this timer.
        /// </summary>
        public TimerState State { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="DateTime"/> that this timer was started if the <see cref="State"/> is <see
        /// cref="TimerState.Running"/> or <see cref="TimerState.Expired"/>, or <c>null</c> otherwise.
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="DateTime"/> that this timer will expire or has expired if the <see
        /// cref="State"/> is <see cref="TimerState.Running"/> or <see cref="TimerState.Expired"/>, or <c>null</c>
        /// otherwise.
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// Gets or sets a <see cref="TimeSpan"/> representing the time elapsed since this timer started if the <see
        /// cref="State"/> is <see cref="TimerState.Running"/>, <see cref="TimerState.Paused"/>, or <see
        /// cref="TimerState.Expired"/>, or <c>null</c> otherwise.
        /// </summary>
        [XmlIgnore]
        public TimeSpan? TimeElapsed { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="TimeSpan.Ticks"/> property of <see cref="TimeElapsed"/>.
        /// </summary>
        /// <remarks>
        /// This property is exposed because <see cref="TimeSpan"/> does not serialize to XML.
        /// </remarks>
        [XmlElement("TimeElapsed")]
        public long? TimeElapsedTicks
        {
            get { return this.TimeElapsed.HasValue ? this.TimeElapsed.Value.Ticks : (long?)null; }
            set { this.TimeElapsed = value.HasValue ? new TimeSpan(value.Value) : (TimeSpan?)null; }
        }

        /// <summary>
        /// Gets or sets a <see cref="TimeSpan"/> representing the time left until this timer expires if the <see
        /// cref="State"/> is <see cref="TimerState.Running"/> or <see cref="TimerState.Paused"/>, <see
        /// cref="TimeSpan.Zero"/> if the <see cref="State"/> is <see cref="TimerState.Expired"/>, or <c>null</c>
        /// otherwise.
        /// </summary>
        [XmlIgnore]
        public TimeSpan? TimeLeft { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="TimeSpan.Ticks"/> property of <see cref="TimeLeft"/>.
        /// </summary>
        /// <remarks>
        /// This property is exposed because <see cref="TimeSpan"/> does not serialize to XML.
        /// </remarks>
        [XmlElement("TimeLeft")]
        public long? TimeLeftTicks
        {
            get { return this.TimeLeft.HasValue ? this.TimeLeft.Value.Ticks : (long?)null; }
            set { this.TimeLeft = value.HasValue ? new TimeSpan(value.Value) : (TimeSpan?)null; }
        }

        /// <summary>
        /// Gets or sets a <see cref="TimeSpan"/> representing the total time that this timer will run for or has run
        /// for if the <see cref="State"/> is <see cref="TimerState.Running"/> or <see cref="TimerState.Expired"/>, or
        /// <c>null</c> otherwise.
        /// </summary>
        [XmlIgnore]
        public TimeSpan? TotalTime { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="TimeSpan.Ticks"/> property of <see cref="TotalTime"/>.
        /// </summary>
        /// <remarks>
        /// This property is exposed because <see cref="TimeSpan"/> does not serialize to XML.
        /// </remarks>
        [XmlElement("TotalTime")]
        public long? TotalTimeTicks
        {
            get { return this.TotalTime.HasValue ? this.TotalTime.Value.Ticks : (long?)null; }
            set { this.TotalTime = value.HasValue ? new TimeSpan(value.Value) : (TimeSpan?)null; }
        }

        /// <summary>
        /// Returns a <see cref="TimerInfo"/> for the specified <see cref="Timer"/>.
        /// </summary>
        /// <param name="timer">A <see cref="Timer"/>.</param>
        /// <returns>A <see cref="TimerInfo"/> for the specified <see cref="Timer"/>.</returns>
        public static TimerInfo FromTimer(Timer timer)
        {
            if (timer == null)
            {
                return null;
            }

            return timer.ToTimerInfo();
        }
    }
}
