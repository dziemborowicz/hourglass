// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimerInput.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass
{
    using System;
    using System.Globalization;
    using System.Xml;
    using System.Xml.Schema;
    using System.Xml.Serialization;

    /// <summary>
    /// A representation of the input used to start a <see cref="Timer"/>.
    /// </summary>
    public class TimerInput : IXmlSerializable
    {
        /// <summary>
        /// The <see cref="TimeSpan"/> representing the input, if the input was a <see cref="TimeSpan"/>.
        /// </summary>
        private TimeSpan? timeSpan;

        /// <summary>
        /// The <see cref="DateTime"/> representing the input, if the input was a <see cref="DateTime"/>.
        /// </summary>
        private DateTime? dateTime;

        /// <summary>
        /// Initializes a new instance of the <see cref="TimerInput"/> class.
        /// </summary>
        public TimerInput()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TimerInput"/> class with an underlying <see cref="TimeSpan"/>.
        /// </summary>
        /// <param name="timeSpan">A <see cref="TimeSpan"/>.</param>
        public TimerInput(TimeSpan timeSpan)
        {
            this.timeSpan = timeSpan;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TimerInput"/> class with an underlying <see cref="DateTime"/>.
        /// </summary>
        /// <param name="dateTime">A <see cref="DateTime"/>.</param>
        public TimerInput(DateTime dateTime)
        {
            this.dateTime = dateTime;
        }

        /// <summary>
        /// Gets a value indicating whether this input is a <see cref="TimeSpan"/>.
        /// </summary>
        public bool IsTimeSpan
        {
            get { return this.timeSpan.HasValue; }
        }

        /// <summary>
        /// Gets the <see cref="TimeSpan"/> representing the input, if the input was a <see cref="TimeSpan"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException">If the input was a <see cref="TimeSpan"/>.</exception>
        public TimeSpan TimeSpan
        {
            get
            {
                if (!this.timeSpan.HasValue)
                {
                    throw new InvalidOperationException("Input is not a TimeSpan.");
                }

                return this.timeSpan.Value;
            }
        }

        /// <summary>
        /// Gets the <see cref="DateTime"/> representing the input, if the input was a <see cref="DateTime"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException">If the input was a <see cref="DateTime"/>.</exception>
        public DateTime DateTime
        {
            get
            {
                if (!this.dateTime.HasValue)
                {
                    throw new InvalidOperationException("Input is not a DateTime.");
                }

                return this.dateTime.Value;
            }
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">An <see cref="object"/>.</param>
        /// <returns><c>true</c> if the specified object is equal to the current object, or <c>false</c> otherwise.
        /// </returns>
        public override bool Equals(object obj)
        {
            TimerInput timerInput = obj as TimerInput;
            return timerInput != null && timerInput.timeSpan == this.timeSpan && timerInput.dateTime == this.dateTime;
        }

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            return this.IsTimeSpan ? this.TimeSpan.GetHashCode() : this.DateTime.GetHashCode();
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return this.IsTimeSpan
                ? TimeSpanUtility.ToShortNaturalString(this.TimeSpan)
                : DateTimeUtility.ToNaturalString(this.DateTime);
        }

        /// <summary>
        /// This method is an implementation of a reserved method and should not be used. It always returns
        /// <c>null</c>.
        /// </summary>
        /// <returns><c>null</c>.</returns>
        /// <seealso cref="IXmlSerializable.GetSchema"/>
        XmlSchema IXmlSerializable.GetSchema()
        {
            return null;
        }

        /// <summary>
        /// Generates a <see cref="TimerInput"/> from its XML representation.
        /// </summary>
        /// <param name="reader">The <see cref="XmlReader"/> stream from which the <see cref="TimerInput"/> is
        /// deserialized.</param>
        /// <seealso cref="IXmlSerializable.ReadXml"/>
        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            if (reader.Name != "TimerInput" || reader.IsEmptyElement)
            {
                throw new InvalidOperationException("Could not deserialize TimerInput from reader.");
            }

            // Read to the first child element
            while (reader.Read() && reader.NodeType != XmlNodeType.Element)
            {
            }

            // Read timeSpan or dateTime element
            if (reader.Name == "timeSpan")
            {
                long ticks = long.Parse(reader.ReadElementContentAsString(), CultureInfo.InvariantCulture);
                this.timeSpan = new TimeSpan(ticks);
                this.dateTime = null;
            }
            else
            {
                XmlSerializer dateTimeSerializer = new XmlSerializer(typeof(DateTime));
                this.timeSpan = null;
                this.dateTime = (DateTime)dateTimeSerializer.Deserialize(reader);
            }

            // Read to end of element
            while (reader.Read() && reader.Name != "TimerInput" && reader.NodeType != XmlNodeType.EndElement)
            {
            }
        }

        /// <summary>
        /// Converts a <see cref="TimerInput"/> into its XML representation.
        /// </summary>
        /// <param name="writer">The <see cref="XmlWriter"/> stream to which the <see cref="TimerInput"/> is
        /// serialized.</param>
        /// <seealso cref="IXmlSerializable.WriteXml"/>
        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            if (this.IsTimeSpan)
            {
                writer.WriteStartElement("timeSpan");
                writer.WriteValue(this.TimeSpan.Ticks.ToString(CultureInfo.InvariantCulture));
                writer.WriteEndElement();
            }
            else
            {
                XmlSerializer dateTimeSerializer = new XmlSerializer(typeof(DateTime));
                dateTimeSerializer.Serialize(writer, this.DateTime);
            }
        }
    }
}
