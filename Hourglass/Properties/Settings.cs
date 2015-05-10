// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Settings.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass.Properties
{
    using System.Configuration;

    /// <summary>
    /// Application settings.
    /// </summary>
#if PORTABLE
    [SettingsProvider(typeof(PortableSettingsProvider))]
#endif
    internal sealed partial class Settings 
    {
    }
}
