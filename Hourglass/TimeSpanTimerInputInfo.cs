// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimeSpanTimerInputInfo.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass
{
    using System;
    using System.Xml.Serialization;

    /// <summary>
    /// The representation of a <see cref="TimeSpanTimerInput"/> used for XML serialization.
    /// </summary>
    public class TimeSpanTimerInputInfo : TimerInputInfo
    {
        /// <summary>
        /// Gets or sets the <see cref="TimeSpanTimerInput.TimeSpan"/> for a <see cref="TimeSpanTimerInput"/>.
        /// </summary>
        [XmlIgnore]
        public TimeSpan TimeSpan { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="System.TimeSpan.Ticks"/> property of the <see cref="TimeSpan"/>.
        /// </summary>
        /// <remarks>
        /// This property is exposed because <see cref="System.TimeSpan"/> does not serialize to XML.
        /// </remarks>
        [XmlElement("TimeSpan")]
        public long TimeSpanTicks
        {
            get { return this.TimeSpan.Ticks; }
            set { this.TimeSpan = new TimeSpan(value); }
        }
    }
}
