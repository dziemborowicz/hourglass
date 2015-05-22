// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IManager.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass
{
    using System;

    /// <summary>
    /// An interface for singleton manager classes.
    /// </summary>
    public interface IManager : IDisposable
    {
        /// <summary>
        /// Initializes the class.
        /// </summary>
        void Initialize();

        /// <summary>
        /// Persists the state of the class.
        /// </summary>
        void Persist();
    }
}
