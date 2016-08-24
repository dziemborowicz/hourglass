// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KeepAwakeManager.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass.Managers
{
    using System.Collections.Generic;

    /// <summary>
    /// Manages the thread-state of the main user interface thread to keep the computer from sleeping while a timer is
    /// running in any window.
    /// </summary>
    public class KeepAwakeManager : Manager
    {
        /// <summary>
        /// Singleton instance of the <see cref="KeepAwakeManager"/> class.
        /// </summary>
        public static readonly KeepAwakeManager Instance = new KeepAwakeManager();

        /// <summary>
        /// The set of objects that require that the system be kept awake.
        /// </summary>
        private readonly HashSet<object> objectsToKeepAwakeFor = new HashSet<object>();

        /// <summary>
        /// The <see cref="ExecutionState"/> before the manager started keeping the system awake.
        /// </summary>
        private ExecutionState previousExecutionState = ExecutionState.EsNull;

        /// <summary>
        /// Prevents a default instance of the <see cref="KeepAwakeManager"/> class from being created.
        /// </summary>
        private KeepAwakeManager()
        {
        }

        /// <summary>
        /// Gets a value indicating whether the manager is currently keeping the system awake.
        /// </summary>
        public bool IsKeepingSystemAwake { get; private set; }

        /// <summary>
        /// Adds the specified object to the set of objects that require that the system be kept awake and starts
        /// keeping the system awake if it was not already being kept awake.
        /// </summary>
        /// <param name="obj">An <see cref="object"/>.</param>
        public void StartKeepAwakeFor(object obj)
        {
            this.objectsToKeepAwakeFor.Add(obj);
            this.UpdateKeepAwake();
        }

        /// <summary>
        /// Removes the specified object from the set of objects that require that the system be kept awake and stops
        /// keeping the system awake if it was being kept awake.
        /// </summary>
        /// <param name="obj">An <see cref="object"/>.</param>
        public void StopKeepAwakeFor(object obj)
        {
            this.objectsToKeepAwakeFor.Remove(obj);
            this.UpdateKeepAwake();
        }

        /// <summary>
        /// Disposes the manager.
        /// </summary>
        /// <param name="disposing">A value indicating whether this method was invoked by an explicit call to <see
        /// cref="Dispose"/>.</param>
        protected override void Dispose(bool disposing)
        {
            if (this.Disposed)
            {
                return;
            }

            this.StopKeepAwake();
            base.Dispose(disposing);
        }

        /// <summary>
        /// Starts or stops keeping the system awake as required.
        /// </summary>
        private void UpdateKeepAwake()
        {
            if (this.objectsToKeepAwakeFor.Count > 0)
            {
                this.StartKeepAwake();
            }
            else
            {
                this.StopKeepAwake();
            }
        }

        /// <summary>
        /// Start keeping the system awake. If the system is already being kept awake, this method does nothing.
        /// </summary>
        private void StartKeepAwake()
        {
            if (!this.IsKeepingSystemAwake)
            {
                ExecutionState executionState = ExecutionState.EsContinuous | ExecutionState.EsDisplayRequired | ExecutionState.EsSystemRequired;
                this.previousExecutionState = NativeMethods.SetThreadExecutionState(executionState);

                this.IsKeepingSystemAwake = true;
            }
        }

        /// <summary>
        /// Stops keeping the system awake. If the system is not being kept awake, this method does nothing.
        /// </summary>
        private void StopKeepAwake()
        {
            if (this.IsKeepingSystemAwake)
            {
                if (this.previousExecutionState != ExecutionState.EsNull)
                {
                    NativeMethods.SetThreadExecutionState(this.previousExecutionState);
                }

                this.IsKeepingSystemAwake = false;
            }
        }
    }
}
