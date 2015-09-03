// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimerStartInfoList.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass.Serialization
{
    using System.Collections.Generic;

    /// <summary>
    /// A list of <see cref="TimerStartInfo"/> objects used for XML serialization.
    /// </summary>
    public class TimerStartInfoList : List<TimerStartInfo>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TimerStartInfoList"/> class that is empty and has the default
        /// initial capacity.
        /// </summary>
        public TimerStartInfoList()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TimerStartInfoList"/> class that contains elements copied from
        /// the specified collection and has sufficient capacity to accommodate the number of elements copied.
        /// </summary>
        /// <param name="collection">The collection whose elements are copied to the new list.</param>
        public TimerStartInfoList(IEnumerable<TimerStartInfo> collection)
            : base(collection)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TimerStartInfoList"/> class that is empty and has the
        /// specified initial capacity.
        /// </summary>
        /// <param name="capacity">The number of elements that the new list can initially store.</param>
        public TimerStartInfoList(int capacity)
            : base(capacity)
        {
        }
    }
}
