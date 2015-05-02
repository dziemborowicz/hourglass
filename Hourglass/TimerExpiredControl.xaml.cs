// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimerExpiredControl.xaml.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass
{
    using System;
    using System.Windows;
    using System.Windows.Media.Animation;

    /// <summary>
    /// A control that displays the timer expired message.
    /// </summary>
    public partial class TimerExpiredControl
    {
        /// <summary>
        /// An flash animation for the notification border.
        /// </summary>
        private readonly DoubleAnimation flashAnimation;

        /// <summary>
        /// An glow animation for the notification border.
        /// </summary>
        private readonly DoubleAnimation glowAnimation;

        /// <summary>
        /// The current animation for the notification border.
        /// </summary>
        private AnimationTimeline currentAnimation;

        /// <summary>
        /// Initializes a new instance of the <see cref="TimerExpiredControl"/> class.
        /// </summary>
        public TimerExpiredControl()
        {
            this.InitializeComponent();

            this.flashAnimation = new DoubleAnimation(1.0, 0.0, new Duration(TimeSpan.FromSeconds(1)));
            this.flashAnimation.EasingFunction = new ExponentialEase { EasingMode = EasingMode.EaseOut };

            this.glowAnimation = new DoubleAnimation(1.0, 0.0, new Duration(TimeSpan.FromSeconds(2)));
            this.glowAnimation.AutoReverse = true;
            this.glowAnimation.EasingFunction = new ExponentialEase { EasingMode = EasingMode.EaseInOut };
            this.glowAnimation.RepeatBehavior = RepeatBehavior.Forever;

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
        /// Raised when the user has reset.
        /// </summary>
        public event EventHandler Reset;

        /// <summary>
        /// Shows the control.
        /// </summary>
        /// <param name="sound">The sound to play.</param>
        public void Show(Sound sound)
        {
            this.Visibility = Visibility.Visible;
            this.flashAnimation.Completed += this.FlashAnimationCompleted;
            this.currentAnimation = this.flashAnimation;
            this.NotificationBorder.BeginAnimation(UIElement.OpacityProperty, this.currentAnimation);

            if (sound != null)
            {
                sound.PlayAsync();
            }

            this.OnShowed();
        }

        /// <summary>
        /// Hides the control.
        /// </summary>
        public void Hide()
        {
            this.Visibility = Visibility.Hidden;
            this.flashAnimation.Completed -= this.FlashAnimationCompleted;
            this.currentAnimation = null;
            this.NotificationBorder.BeginAnimation(UIElement.OpacityProperty, this.currentAnimation);

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
        /// Raises the <see cref="Reset"/> event.
        /// </summary>
        protected void OnReset()
        {
            EventHandler eventHandler = this.Reset;

            if (eventHandler != null)
            {
                eventHandler(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Invoked when the <see cref="ResetButton"/> is clicked.
        /// </summary>
        /// <param name="sender">The <see cref="ResetButton"/>.</param>
        /// <param name="e">The event data.</param>
        private void ResetButtonClick(object sender, RoutedEventArgs e)
        {
            this.Hide();
            this.OnReset();
        }

        /// <summary>
        /// Invoked when the flash animation has completely finished playing.
        /// </summary>
        /// <param name="sender">The flash animation.</param>
        /// <param name="e">The event data.</param>
        private void FlashAnimationCompleted(object sender, EventArgs e)
        {
            this.NotificationBorder.BeginAnimation(UIElement.OpacityProperty, null);
            this.NotificationBorder.BeginAnimation(UIElement.OpacityProperty, this.currentAnimation);
        }
    }
}
