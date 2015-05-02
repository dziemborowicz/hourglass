// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimerInputControl.xaml.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass
{
    using System;
    using System.ComponentModel;
    using System.Text.RegularExpressions;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    /// <summary>
    /// A control that allows a user to specify a <see cref="TimerInput"/>.
    /// </summary>
    public partial class TimerInputControl : INotifyPropertyChanged
    {
        /// <summary>
        /// A <see cref="DependencyProperty"/> that specifies whether the user can cancel specifying a <see
        /// cref="TimerInput"/>.
        /// </summary>
        public static readonly DependencyProperty IsCancelableProperty = DependencyProperty.Register(
                                   "IsCancelable",
                                   typeof(bool),
                                   typeof(TimerInputControl),
                                   new PropertyMetadata(true /* defaultValue */, IsCancelablePropertyChanged));

        /// <summary>
        /// A value indicating whether the control is displaying validation errors.
        /// </summary>
        private bool hasValidationErrors;

        /// <summary>
        /// The last valid input specified by the user.
        /// </summary>
        private TimerInput lastInput;

        /// <summary>
        /// Initializes a new instance of the <see cref="TimerInputControl"/> class.
        /// </summary>
        public TimerInputControl()
        {
            this.InitializeComponent();

            this.CancelButton.IsEnabled = this.IsCancelable;
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
        /// Raised when the user has specified an input.
        /// </summary>
        public event EventHandler<TimerInputEventArgs> Started;

        /// <summary>
        /// Raised when the user has canceled.
        /// </summary>
        public event EventHandler Canceled;

        /// <summary>
        /// Raised when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets a value indicating whether the user can cancel specifying a <see cref="TimerInput"/>.
        /// </summary>
        public bool IsCancelable
        {
            get { return (bool)this.GetValue(IsCancelableProperty); }
            set { this.SetValue(IsCancelableProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the control is displaying validation errors.
        /// </summary>
        public bool HasValidationErrors
        {
            get
            {
                return this.hasValidationErrors; 
            }

            protected set
            {
                if (this.hasValidationErrors == value)
                {
                    return;
                }

                this.hasValidationErrors = value;
                this.OnPropertyChanged("HasValidationErrors");
            }
        }

        /// <summary>
        /// Shows the control.
        /// </summary>
        public void Show()
        {
            this.Visibility = Visibility.Visible;
            this.TimerTextBox.Text = this.lastInput == null ? string.Empty : this.lastInput.ToString();
            this.TimerTextBox.SelectAll();
            this.TimerTextBox.Focus();

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
        /// Raises the <see cref="Started"/> event.
        /// </summary>
        /// <param name="input">A <see cref="TimerInput"/>.</param>
        protected void OnStarted(TimerInput input)
        {
            EventHandler<TimerInputEventArgs> eventHandler = this.Started;
            TimerInputEventArgs eventArgs = new TimerInputEventArgs(input);

            if (eventHandler != null)
            {
                eventHandler(this, eventArgs);
            }
        }

        /// <summary>
        /// Raises the <see cref="Canceled"/> event.
        /// </summary>
        protected void OnCanceled()
        {
            EventHandler eventHandler = this.Canceled;

            if (eventHandler != null)
            {
                eventHandler(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <param name="propertyNames">One or more property names.</param>
        protected void OnPropertyChanged(params string[] propertyNames)
        {
            PropertyChangedEventHandler eventHandler = this.PropertyChanged;

            if (eventHandler != null)
            {
                foreach (string propertyName in propertyNames)
                {
                    eventHandler(this, new PropertyChangedEventArgs(propertyName));
                }
            }
        }

        /// <summary>
        /// Invoked when the effective value of the <see cref="IsCancelableProperty"/> changes.
        /// </summary>
        /// <param name="sender">The <see cref="DependencyObject"/> on which the <see cref="IsCancelableProperty"/>
        /// has changed value.</param>
        /// <param name="e">Event data that is issued by any event that tracks changes to the effective value of this
        /// property.</param>
        private static void IsCancelablePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            TimerInputControl control = sender as TimerInputControl;
            if (control == null)
            {
                return;
            }

            control.CancelButton.IsEnabled = control.IsCancelable;
            control.OnPropertyChanged("IsCancelable");
        }

        /// <summary>
        /// Returns a <see cref="TimerInput"/> representing the user's input, or <c>null</c> if the user has not
        /// specified a valid input.
        /// </summary>
        /// <returns>A <see cref="TimerInput"/> representing the user's input, or <c>null</c> if the user has not
        /// specified a valid input.</returns>
        private TimerInput GetInput()
        {
            string input = this.TimerTextBox.Text;

            if (string.IsNullOrWhiteSpace(input))
            {
                return null;
            }

            if (Regex.IsMatch(input, @"^\s*(un)?till?\s*|^20\d\d$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant))
            {
                input = Regex.Replace(input, @"^\s*(un)?till?\s*", string.Empty, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
                return TimerInput.FromDateTimeOrTimeSpan(input);
            }

            return TimerInput.FromTimeSpanOrDateTime(input);
        }

        /// <summary>
        /// Invoked when the <see cref="StartButton"/> is clicked.
        /// </summary>
        /// <param name="sender">The <see cref="StartButton"/>.</param>
        /// <param name="e">The event data.</param>
        private void StartButtonClick(object sender, RoutedEventArgs e)
        {
            this.HasValidationErrors = false;

            TimerInput input = this.GetInput();
            if (input == null)
            {
                this.HasValidationErrors = true;
                return;
            }
            this.lastInput = input;

            this.Hide();
            this.OnStarted(input);
        }

        /// <summary>
        /// Invoked when the <see cref="CancelButton"/> is clicked.
        /// </summary>
        /// <param name="sender">The <see cref="CancelButton"/>.</param>
        /// <param name="e">The event data.</param>
        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            this.HasValidationErrors = false;

            if (this.IsCancelable)
            {
                this.Hide();
                this.OnCanceled();
            }
        }

        /// <summary>
        /// Invoked when the background area of the control is clicked.
        /// </summary>
        /// <param name="sender">The background <see cref="Grid"/> control.</param>
        /// <param name="e">The event data.</param>
        private void BackgroundMouseDown(object sender, MouseButtonEventArgs e)
        {
            this.HasValidationErrors = false;

            if (this.IsCancelable)
            {
                this.Hide();
                this.OnCanceled();
            }
        }
    }
}
