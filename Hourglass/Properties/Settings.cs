// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Settings.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass.Properties
{
    /// <summary>
    /// Application settings.
    /// </summary>
#if PORTABLE
    [System.Configuration.SettingsProvider(typeof(PortableSettingsProvider))]
#endif
    internal sealed partial class Settings 
    {
    }
}
