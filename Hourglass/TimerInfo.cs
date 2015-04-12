// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimerInfo.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass
{
    using System;
    using System.Xml.Serialization;

    /// <summary>
    /// A representation of the state of a <see cref="Timer"/>.
    /// </summary>
    public class TimerInfo
    {
        /// <summary>
        /// Gets or sets the title or description of the timer.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="TimerState"/> of the timer.
        /// </summary>
        public TimerState State { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="DateTime"/> that the timer was started if the <see cref="State"/> is not <see
        /// cref="TimerState.Stopped"/> and the timer is counting down a <see cref="TimeSpan"/>, or <c>null</c>
        /// otherwise.
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="DateTime"/> that the timer will expire if the <see cref="State"/> is not <see
        /// cref="TimerState.Stopped"/>, or <c>null</c> otherwise.
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// Gets or sets a <see cref="TimeSpan"/> representing the time left until the timer expires if the
        /// <see cref="State"/> is not <see cref="TimerState.Stopped"/>, or <c>null</c> otherwise.
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
        /// Gets or sets a <see cref="TimeSpan"/> representing the total time that the timer will run if the
        /// <see cref="State"/> is not <see cref="TimerState.Stopped"/> and the timer is counting down a
        /// <see cref="TimeSpan"/>, or <c>null</c> otherwise.
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
        /// Gets or sets a value indicating whether to loop the timer continuously.
        /// </summary>
        public bool LoopTimer { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="TimerWindow"/> should always be displayed on top of
        /// other windows.
        /// </summary>
        public bool AlwaysOnTop { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether an icon for the <see cref="TimerWindow"/> should be shown in the
        /// notification area (system tray).
        /// </summary>
        public bool ShowInNotificationArea { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="TimerWindow"/> should be brought to the top of other
        /// windows when the timer expires.
        /// </summary>
        public bool PopUpWhenExpired { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="TimerWindow"/> should be closed when the timer
        /// expires.
        /// </summary>
        public bool CloseWhenExpired { get; set; }

        /// <summary>
        /// Gets or sets the path of the sound to play when the timer expires. Set to <c>null</c> if no sound is to be
        /// played.
        /// </summary>
        public string SoundPath { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the sound that plays when the timer expires should be looped.
        /// </summary>
        public bool LoopSound { get; set; }
    }
}
