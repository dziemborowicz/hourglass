// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ThemeInfoList.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass.Serialization
{
    using System.Collections.Generic;

    /// <summary>
    /// A list of <see cref="ThemeInfo"/> objects used for XML serialization.
    /// </summary>
    public class ThemeInfoList : List<ThemeInfo>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ThemeInfoList"/> class that is empty and has the default
        /// initial capacity.
        /// </summary>
        public ThemeInfoList()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ThemeInfoList"/> class that contains elements copied from the
        /// specified collection and has sufficient capacity to accommodate the number of elements copied.
        /// </summary>
        /// <param name="collection">The collection whose elements are copied to the new list.</param>
        public ThemeInfoList(IEnumerable<ThemeInfo> collection)
            : base(collection)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ThemeInfoList"/> class that is empty and has the specified
        /// initial capacity.
        /// </summary>
        /// <param name="capacity">The number of elements that the new list can initially store.</param>
        public ThemeInfoList(int capacity)
            : base(capacity)
        {
        }
    }
}
