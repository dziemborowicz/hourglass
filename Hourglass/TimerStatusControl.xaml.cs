// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimerStatusControl.xaml.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Input;

    /// <summary>
    /// A control that displays the status of a timer.
    /// </summary>
    public partial class TimerStatusControl
    {
        /// <summary>
        /// A <see cref="DependencyProperty"/> that is a command that pauses the timer.
        /// </summary>
        public static readonly DependencyProperty PauseCommandProperty = DependencyProperty.Register(
                                   "PauseCommand",
                                   typeof(ICommand),
                                   typeof(TimerStatusControl),
                                   new PropertyMetadata(null /* defaultValue */, CommandChanged));

        /// <summary>
        /// A <see cref="DependencyProperty"/> that is a command that resumes the timer.
        /// </summary>
        public static readonly DependencyProperty ResumeCommandProperty = DependencyProperty.Register(
                                   "ResumeCommand",
                                   typeof(ICommand),
                                   typeof(TimerStatusControl),
                                   new PropertyMetadata(null /* defaultValue */, CommandChanged));

        /// <summary>
        /// A <see cref="DependencyProperty"/> that is a command that stops the timer.
        /// </summary>
        public static readonly DependencyProperty StopCommandProperty = DependencyProperty.Register(
                                   "StopCommand",
                                   typeof(ICommand),
                                   typeof(TimerStatusControl),
                                   new PropertyMetadata(null /* defaultValue */, CommandChanged));

        /// <summary>
        /// Initializes a new instance of the <see cref="TimerStatusControl"/> class.
        /// </summary>
        public TimerStatusControl()
        {
            this.InitializeComponent();

            Binding pauseCommandBinding = new Binding();
            pauseCommandBinding.Path = new PropertyPath("PauseCommand");
            BindingOperations.SetBinding(this, TimerStatusControl.PauseCommandProperty, pauseCommandBinding);

            Binding resumeCommandBinding = new Binding();
            resumeCommandBinding.Path = new PropertyPath("ResumeCommand");
            BindingOperations.SetBinding(this, TimerStatusControl.ResumeCommandProperty, resumeCommandBinding);

            Binding stopCommandBinding = new Binding();
            stopCommandBinding.Path = new PropertyPath("StopCommand");
            BindingOperations.SetBinding(this, TimerStatusControl.StopCommandProperty, stopCommandBinding);

            this.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// Raised when the control is showed.
        /// </summary>
        public event EventHandler Showed;

        /// <summary>
        /// Raised when the control is hidden.
        /// </summary>
        public event EventHandler Hidden;

        /// <summary>
        /// Gets or sets a command that pauses the timer.
        /// </summary>
        public ICommand PauseCommand
        {
            get { return (ICommand)this.GetValue(PauseCommandProperty); }
            set { this.SetValue(PauseCommandProperty, value); }
        }

        /// <summary>
        /// Gets or sets a command that resumes the timer.
        /// </summary>
        public ICommand ResumeCommand
        {
            get { return (ICommand)this.GetValue(ResumeCommandProperty); }
            set { this.SetValue(ResumeCommandProperty, value); }
        }

        /// <summary>
        /// Gets or sets a command that stops the timer.
        /// </summary>
        public ICommand StopCommand
        {
            get { return (ICommand)this.GetValue(StopCommandProperty); }
            set { this.SetValue(StopCommandProperty, value); }
        }

        /// <summary>
        /// Shows the control.
        /// </summary>
        public void Show()
        {
            this.Visibility = Visibility.Visible;

            this.OnShowed();
        }

        /// <summary>
        /// Hides the control.
        /// </summary>
        public void Hide()
        {
            this.Visibility = Visibility.Hidden;

            this.OnHidden();
        }

        /// <summary>
        /// Raises the <see cref="Showed"/> event.
        /// </summary>
        protected void OnShowed()
        {
            EventHandler eventHandler = this.Showed;

            if (eventHandler != null)
            {
                eventHandler(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Raises the <see cref="Hidden"/> event.
        /// </summary>
        protected void OnHidden()
        {
            EventHandler eventHandler = this.Hidden;

            if (eventHandler != null)
            {
                eventHandler(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Invoked when the effective value of any of the command properties changes.
        /// </summary>
        /// <param name="sender">The <see cref="DependencyObject"/> on which the command property has changed value.
        /// </param>
        /// <param name="e">Event data that is issued by any event that tracks changes to the effective value of this
        /// property.</param>
        private static void CommandChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            TimerStatusControl control = sender as TimerStatusControl;
            if (control == null)
            {
                return;
            }

            ICommand oldCommand = e.OldValue as ICommand;
            if (oldCommand != null)
            {
                oldCommand.CanExecuteChanged -= control.CommandCanExecuteChanged;
            }

            ICommand newCommand = e.NewValue as ICommand;
            if (newCommand != null)
            {
                newCommand.CanExecuteChanged += control.CommandCanExecuteChanged;
            }

            control.UpdateCommands();
        }

        /// <summary>
        /// Invoked when the control is laid out, rendered, and ready for interaction.
        /// </summary>
        /// <param name="sender">The control.</param>
        /// <param name="e">The event data.</param>
        private void ControlLoaded(object sender, RoutedEventArgs e)
        {
            this.UpdateCommands();
        }

        /// <summary>
        /// Invoked when a changes occur that affect whether or not a command should execute.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void CommandCanExecuteChanged(object sender, EventArgs e)
        {
            this.UpdateCommands();
        }

        /// <summary>
        /// Invoked when the pause button is clicked.
        /// </summary>
        /// <param name="sender">The pause button.</param>
        /// <param name="e">The event data.</param>
        private void PauseButtonClick(object sender, RoutedEventArgs e)
        {
            if (this.PauseCommand != null)
            {
                this.PauseCommand.Execute(null /* parameter */);
            }
        }

        /// <summary>
        /// Invoked when the resume button is clicked.
        /// </summary>
        /// <param name="sender">The resume button.</param>
        /// <param name="e">The event data.</param>
        private void ResumeButtonClick(object sender, RoutedEventArgs e)
        {
            if (this.ResumeCommand != null)
            {
                this.ResumeCommand.Execute(null /* parameter */);
            }
        }

        /// <summary>
        /// Invoked when the stop button is clicked.
        /// </summary>
        /// <param name="sender">The stop button.</param>
        /// <param name="e">The event data.</param>
        private void StopButtonClick(object sender, RoutedEventArgs e)
        {
            if (this.StopCommand != null)
            {
                this.StopCommand.Execute(null /* parameter */);
            }
        }

        /// <summary>
        /// Updates the <see cref="UIElement.IsEnabled"/> property on the buttons bound to the commands.
        /// </summary>
        /// <remarks>
        /// This method is necessary to avoid flicker that occurs when relying on the default behavior when binding a
        /// <see cref="ICommand"/> to the <see cref="Button.Command"/> property.
        /// </remarks>
        private void UpdateCommands()
        {
            this.PauseButton.IsEnabled = this.PauseCommand != null && this.PauseCommand.CanExecute(null /* parameter */);
            this.ResumeButton.IsEnabled = this.ResumeCommand != null && this.ResumeCommand.CanExecute(null /* parameter */);
            this.StopButton.IsEnabled = true;
        }
    }
}
