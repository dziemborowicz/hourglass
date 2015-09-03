// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UpdateInfo.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass.Serialization
{
    /// <summary>
    /// The representation of the latest version of the app used for XML serialization.
    /// </summary>
    public class UpdateInfo
    {
        /// <summary>
        /// Gets or sets the latest version of the app.
        /// </summary>
        public string LatestVersion { get; set; }

        /// <summary>
        /// Gets or sets the URL to download the update to the latest version of the app.
        /// </summary>
        public string UpdateUrl { get; set; }
    }
}
