// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NativeMethods.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass
{
    using System;
    using System.Runtime.ConstrainedExecution;
    using System.Runtime.InteropServices;
    using System.Security;

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
        /// Sets the specified timer to the inactive state.
        /// </summary>
        /// <param name="hTimer">A handle to the timer object.</param>
        /// <returns><c>true</c> if the call succeeds, or <c>false</c> otherwise.</returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool CancelWaitableTimer(IntPtr hTimer);

        /// <summary>
        /// Closes an open object handle.
        /// </summary>
        /// <param name="hObject">A valid handle to an open object.</param>
        /// <returns><c>true</c> if the call succeeds, or <c>false</c> otherwise.</returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        [SuppressUnmanagedCodeSecurity]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool CloseHandle(IntPtr hObject);

        /// <summary>
        /// Creates or opens a timer object.
        /// </summary>
        /// <param name="lpTimerAttributes">A pointer to a structure that specifies a security descriptor for the new
        /// timer object and determines whether child processes can inherit the returned handle.</param>
        /// <param name="bManualReset">If this parameter is <c>true</c>, the timer is a manual-reset notification timer.
        /// Otherwise, the timer is a synchronization timer.</param>
        /// <param name="lpTimerName">The name of the timer object.</param>
        /// <returns>A handle to the timer object if the call succeeds, or <see cref="IntPtr.Zero"/> otherwise.</returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr CreateWaitableTimer(IntPtr lpTimerAttributes, bool bManualReset, string lpTimerName);

        /// <summary>
        /// Enables an application to inform the system that it is in use, thereby preventing the system from entering
        /// sleep or turning off the display while the application is running.
        /// </summary>
        /// <param name="esFlags">The thread's execution requirements.</param>
        /// <returns>If the function succeeds, the return value is the previous thread execution state. If the function
        /// fails, the return value is <see cref="ExecutionState.EsNull"/>.</returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern ExecutionState SetThreadExecutionState(ExecutionState esFlags);

        /// <summary>
        /// Activates the specified timer. When the due time arrives, the timer is signaled and the thread that set the
        /// timer calls the optional completion routine.
        /// </summary>
        /// <param name="hTimer">A handle to the timer object.</param>
        /// <param name="pDueTime">The time after which the state of the timer is to be set to signaled.</param>
        /// <param name="lPeriod">The period of the timer, in milliseconds. If <paramref name="lPeriod"/> is zero, the
        /// timer is signaled once. If <paramref name="lPeriod"/> is greater than zero, the timer is periodic.</param>
        /// <param name="pfnCompletionRoutine">A pointer to an optional completion routine.</param>
        /// <param name="lpArgToCompletionRoutine">A pointer to a structure that is passed to the completion routine.
        /// </param>
        /// <param name="fResume">If this parameter is <c>true</c>, restores a system in suspended power conservation
        /// mode when the timer state is set to signaled. Otherwise, the system is not restored.</param>
        /// <returns><c>true</c> if the call succeeds, or <c>false</c> otherwise.</returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool SetWaitableTimer(IntPtr hTimer, [In] ref long pDueTime, int lPeriod, IntPtr pfnCompletionRoutine, IntPtr lpArgToCompletionRoutine, bool fResume);
    }
}
