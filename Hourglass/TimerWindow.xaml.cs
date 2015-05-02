// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimerWindow.xaml.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass
{
    using System;
    using System.Windows;

    /// <summary>
    /// A timer window.
    /// </summary>
    public partial class TimerWindow
    {
        /// <summary>
        /// A <see cref="TimerMenu"/>.
        /// </summary>
        private TimerMenu timerMenu;

        /// <summary>
        /// A <see cref="TimerScaler"/>.
        /// </summary>
        private TimerScaler timerScaler;

        /// <summary>
        /// Initializes a new instance of the <see cref="TimerWindow"/> class.
        /// </summary>
        public TimerWindow()
        {
            this.InitializeComponent();

            this.timerMenu = new TimerMenu(this /* timerWindow */);
            this.timerScaler = new TimerScaler(this /* timerWindow */);
        }

        /// <summary>
        /// Gets or sets the timer.
        /// </summary>
        public ViewableTimer Timer
        {
            get { return this.DataContext as ViewableTimer; }
            set { this.DataContext = value; }
        }

        /// <summary>
        /// Shows the <see cref="TimerStatusControl"/>.
        /// </summary>
        private void ShowTimerStatusControl()
        {
            this.TimerStatusControl.Show();
            this.TimerExpiredControl.Hide();
            this.TimerInputControl.Hide();
        }

        /// <summary>
        /// Shows the <see cref="TimerExpiredControl"/>.
        /// </summary>
        /// <param name="sound">The sound to play.</param>
        private void ShowTimerExpiredControl(Sound sound)
        {
            this.TimerStatusControl.Hide();
            this.TimerExpiredControl.Show(sound);
            this.TimerInputControl.Hide();
        }

        /// <summary>
        /// Show the <see cref="TimerInputControl"/>.
        /// </summary>
        /// <param name="isCancelable">A value indicating whether the control is displaying validation errors.</param>
        private void ShowTimerInputControl(bool isCancelable)
        {
            this.TimerInputControl.IsCancelable = isCancelable;

            this.TimerStatusControl.Hide();
            this.TimerExpiredControl.Hide();
            this.TimerInputControl.Show();
        }

        /// <summary>
        /// Invoked when the data context for this window changes.
        /// </summary>
        /// <param name="sender">The window.</param>
        /// <param name="e">The event data.</param>
        private void WindowDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            ViewableTimer oldTimer = e.OldValue as ViewableTimer;
            if (oldTimer != null)
            {
                oldTimer.Stopped -= this.TimerStopped;
                oldTimer.Expired -= this.TimerExpired;
            }

            ViewableTimer newTimer = e.NewValue as ViewableTimer;
            if (newTimer != null)
            {
                newTimer.Stopped += this.TimerStopped;
                newTimer.Expired += this.TimerExpired;
            }
        }

        /// <summary>
        /// Invoked when this window is laid out, rendered, and ready for interaction.
        /// </summary>
        /// <param name="sender">This window.</param>
        /// <param name="e">The event data.</param>
        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            if (this.Timer.State == TimerState.Running || this.Timer.State == TimerState.Paused)
            {
                this.ShowTimerStatusControl();
            }
            else if (this.Timer.State == TimerState.Expired)
            {
                this.ShowTimerExpiredControl(this.Timer.Options.Sound);
            }
            else
            {
                this.ShowTimerInputControl(false /* isCancelable */);
            }
        }

        /// <summary>
        /// Invoked when the user has specified an input.
        /// </summary>
        /// <param name="sender">The <see cref="TimerInputControl"/>.</param>
        /// <param name="e">The event data.</param>
        private void TimerInputControlStarted(object sender, TimerInputEventArgs e)
        {
            this.Timer = ViewableTimer.GetTimerForInput(e.Input);
            this.Timer.StartCommand.Execute(e.Input);

            TimerManager.Instance.Add(this.Timer);
            TimerInputManager.Instance.Add(e.Input);

            this.ShowTimerStatusControl();
        }

        /// <summary>
        /// Invoked when the user has canceled.
        /// </summary>
        /// <param name="sender">The <see cref="TimerInputControl"/>.</param>
        /// <param name="e">The event data.</param>
        private void TimerInputControlCanceled(object sender, EventArgs e)
        {
            if (this.Timer == null || this.Timer.State == TimerState.Stopped)
            {
                this.Timer = null;
                this.ShowTimerInputControl(false /* isCancelable */);
            }
            else if (this.Timer.State == TimerState.Expired)
            {
                // Do not play a sound if we are going to the expired view sometime after the timer actually expires
                this.ShowTimerExpiredControl(null /* sound */);
            }
            else
            {
                this.ShowTimerStatusControl();
            }
        }

        /// <summary>
        /// Invoked when the user has reset.
        /// </summary>
        /// <param name="sender">The <see cref="TimerExpiredControl"/>.</param>
        /// <param name="e">The event data.</param>
        private void TimerExpiredControlReset(object sender, EventArgs e)
        {
            this.Timer = null;
            this.ShowTimerInputControl(false /* isCancelable */);
        }

        /// <summary>
        /// Invoked when the timer expires.
        /// </summary>
        /// <param name="sender">The timer.</param>
        /// <param name="e">The event data.</param>
        private void TimerExpired(object sender, EventArgs e)
        {
            if (this.Timer == null || this.Timer.State != TimerState.Expired)
            {
                return;
            }

            this.ShowTimerExpiredControl(this.Timer.Options.Sound);
        }

        /// <summary>
        /// Invoked when the timer is stopped.
        /// </summary>
        /// <param name="sender">The timer.</param>
        /// <param name="e">The event data.</param>
        private void TimerStopped(object sender, EventArgs e)
        {
            this.Timer = null;
            this.ShowTimerInputControl(false /* isCancelable */);
        }
    }
}
