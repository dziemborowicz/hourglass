// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnvironmentExtensions.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass.Extensions
{
    using System;

    /// <summary>
    /// Provides information about the current environment and platform.
    /// </summary>
    public static class EnvironmentExtensions
    {
        /// <summary>
        /// Gets a value indicating whether the current environment and platform is Windows 10 or newer.
        /// </summary>
        public static bool IsWindows10OrNewer
        {
            get
            {
                return Environment.OSVersion.Platform == PlatformID.Win32NT
                    && Environment.OSVersion.Version.Major >= 10;
            }
        }
    }
}
