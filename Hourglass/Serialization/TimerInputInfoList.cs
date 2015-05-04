// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimerInputInfoList.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass.Serialization
{
    using System.Collections.Generic;

    /// <summary>
    /// A list of <see cref="TimerInputInfo"/> objects used for XML serialization.
    /// </summary>
    public class TimerInputInfoList : List<TimerInputInfo>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TimerInputInfoList"/> class that is empty and has the default
        /// initial capacity.
        /// </summary>
        public TimerInputInfoList()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TimerInputInfoList"/> class that contains elements copied from
        /// the specified collection and has sufficient capacity to accommodate the number of elements copied.
        /// </summary>
        /// <param name="collection">The collection whose elements are copied to the new list.</param>
        public TimerInputInfoList(IEnumerable<TimerInputInfo> collection)
            : base(collection)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TimerInputInfoList"/> class that is empty and has the
        /// specified initial capacity.
        /// </summary>
        /// <param name="capacity">The number of elements that the new list can initially store.</param>
        public TimerInputInfoList(int capacity)
            : base(capacity)
        {
        }
    }
}
