// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NativeMethods.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass
{
    using System;
    using System.Runtime.InteropServices;

    /// <summary>
    /// The thread's execution requirements.
    /// </summary>
    [Flags]
    internal enum ExecutionState : uint
    {
        /// <summary>
        /// <para>
        /// Enables away mode. This value must be specified with <see cref="EsContinuous"/>.
        /// </para><para>
        /// Away mode should be used only by media-recording and media-distribution applications that must perform
        /// critical background processing on desktop computers while the computer appears to be sleeping.
        /// </para><para>
        /// Windows Server 2003 and Windows XP: <see cref="EsAwaymodeRequired"/> is not supported.
        /// </para>
        /// </summary>
        EsAwaymodeRequired = 0x00000040,

        /// <summary>
        /// Informs the system that the state being set should remain in effect until the next call that uses 
        /// <see cref="EsContinuous"/> and one of the other state flags is cleared.
        /// </summary>
        EsContinuous = 0x80000000,

        /// <summary>
        /// <para>
        /// Forces the display to be on by resetting the display idle timer.
        /// </para><para>
        /// Windows 8: This flag can only keep a display turned on, it can't turn on a display that's currently
        /// off.
        /// </para>
        /// </summary>
        EsDisplayRequired = 0x00000002,

        /// <summary>
        /// Forces the system to be in the working state by resetting the system idle timer.
        /// </summary>
        EsSystemRequired = 0x00000001,

        /// <summary>
        /// <para>
        /// This value is not supported. If <see cref="EsUserPresent"/> is combined with other <see
        /// cref="ExecutionState"/> values, the call will fail and none of the specified states will be set.
        /// </para><para>
        /// Windows Server 2003 and Windows XP: Informs the system that a user is present and resets the display
        /// and system idle timers. <see cref="EsUserPresent"/> must be called with <see cref="EsContinuous"/>.
        /// </para>
        /// </summary>
        EsUserPresent = 0x00000004,

        /// <summary>
        /// Indicates that a call to <see cref="NativeMethods.SetThreadExecutionState"/> failed.
        /// </summary>
        EsNull = 0x00000000
    }

    /// <summary>
    /// Wrapper class for calling Win32 and other unmanaged APIs.
    /// </summary>
    internal static class NativeMethods
    {
        /// <summary>
        /// Enables an application to inform the system that it is in use, thereby preventing the system from entering
        /// sleep or turning off the display while the application is running.
        /// </summary>
        /// <param name="esFlags">The thread's execution requirements.</param>
        /// <returns>If the function succeeds, the return value is the previous thread execution state. If the function
        /// fails, the return value is <see cref="ExecutionState.EsNull"/>.</returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern ExecutionState SetThreadExecutionState(ExecutionState esFlags);
    }
}
