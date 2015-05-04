// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CommandTimer.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass
{
    using System.Windows.Input;

    /// <summary>
    /// A <see cref="Timer"/> that can be controlled using <see cref="ICommand"/>s.
    /// </summary>
    public abstract class CommandTimer : Timer
    {
        #region Private Members

        /// <summary>
        /// Starts the timer.
        /// </summary>
        private ICommand startCommand;

        /// <summary>
        /// Pauses the timer.
        /// </summary>
        private ICommand pauseCommand;

        /// <summary>
        /// Resumes the timer.
        /// </summary>
        private ICommand resumeCommand;

        /// <summary>
        /// Stops the timer.
        /// </summary>
        private ICommand stopCommand;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandTimer"/> class.
        /// </summary>
        protected CommandTimer()
        {
            this.InitializeCommands();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandTimer"/> class.
        /// </summary>
        /// <param name="timerInfo">A <see cref="CommandTimerInfo"/> representing the state of the <see
        /// cref="CommandTimer"/>.</param>
        protected CommandTimer(CommandTimerInfo timerInfo)
            : base(timerInfo)
        {
            this.InitializeCommands();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a command that starts the timer.
        /// </summary>
        public ICommand StartCommand
        {
            get { return this.startCommand; }
        }

        /// <summary>
        /// Gets a command that pauses the timer.
        /// </summary>
        public ICommand PauseCommand
        {
            get { return this.pauseCommand; }
        }

        /// <summary>
        /// Gets a command that resumes the timer.
        /// </summary>
        public ICommand ResumeCommand
        {
            get { return this.resumeCommand; }
        }

        /// <summary>
        /// Gets a command that stops the timer.
        /// </summary>
        public ICommand StopCommand
        {
            get { return this.stopCommand; }
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Starts the timer.
        /// </summary>
        /// <param name="parameter">A <see cref="TimerInput"/> used to start the timer.</param>
        protected abstract void ExecuteStart(object parameter);

        /// <summary>
        /// Returns a value indicating whether the timer can be started.
        /// </summary>
        /// <returns>A value indicating whether the timer can be started.</returns>
        /// <param name="parameter">A <see cref="TimerInput"/> used to start the timer.</param>
        protected abstract bool CanExecuteStart(object parameter);

        /// <summary>
        /// Pauses the timer.
        /// </summary>
        protected abstract void ExecutePause();

        /// <summary>
        /// Returns a value indicating whether the timer can be paused.
        /// </summary>
        /// <returns>A value indicating whether the timer can be paused.</returns>
        protected abstract bool CanExecutePause();

        /// <summary>
        /// Resumes the timer.
        /// </summary>
        protected abstract void ExecuteResume();

        /// <summary>
        /// Returns a value indicating whether the timer can be resumed.
        /// </summary>
        /// <returns>A value indicating whether the timer can be resumed.</returns>
        protected abstract bool CanExecuteResume();

        /// <summary>
        /// Stops the timer.
        /// </summary>
        protected abstract void ExecuteStop();

        /// <summary>
        /// Returns a value indicating whether the timer can be stopped.
        /// </summary>
        /// <returns>A value indicating whether the timer can be stopped.</returns>
        protected abstract bool CanExecuteStop();

        #endregion

        #region Private Methods

        /// <summary>
        /// Initializes the <see cref="ICommand"/>s.
        /// </summary>
        private void InitializeCommands()
        {
            this.startCommand = new RelayCommand(this.ExecuteStart, this.CanExecuteStart);
            this.pauseCommand = new RelayCommand(this.ExecutePause, this.CanExecutePause);
            this.resumeCommand = new RelayCommand(this.ExecuteResume, this.CanExecuteResume);
            this.stopCommand = new RelayCommand(this.ExecuteStop, this.CanExecuteStop);
        }

        #endregion
    }
}
