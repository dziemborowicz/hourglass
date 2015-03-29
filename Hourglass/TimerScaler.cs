// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimerScaler.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    /// <summary>
    /// Scales a <see cref="TimerWindow"/> to ensure that controls shrink and grow with the window size, and updates a
    /// <see cref="Timer"/> interval to ensure smooth animation of the progress bar.
    /// </summary>
    public class TimerScaler
    {
        /// <summary>
        /// The default margin for <see cref="Button"/> elements in the <see cref="timerWindow"/>.
        /// </summary>
        private const double BaseButtonMargin = 7;

        /// <summary>
        /// The default margin for the <see cref="controlsGrid"/>.
        /// </summary>
        private const double BaseControlsGridMargin = 10;

        /// <summary>
        /// The default font size used to render controls in the <see cref="timerWindow"/>.
        /// </summary>
        private const double BaseFontSize = 12;

        /// <summary>
        /// The default font size of the <see cref="timerTextBox"/>.
        /// </summary>
        private const double BaseInputFontSize = 18;

        /// <summary>
        /// The default top margin of the <see cref="timerTextBox"/>.
        /// </summary>
        private const double BaseInputTopMargin = 1;

        /// <summary>
        /// The default bottom margin of the <see cref="timerTextBox"/>.
        /// </summary>
        private const double BaseInputBottomMargin = 4;

        /// <summary>
        /// The default width of a <see cref="timerWindow"/>.
        /// </summary>
        private const double BaseWindowWidth = 350;

        /// <summary>
        /// The default height of a <see cref="timerWindow"/>.
        /// </summary>
        private const double BaseWindowHeight = 150;

        /// <summary>
        /// A <see cref="Timer"/>.
        /// </summary>
        private readonly Timer timer;

        /// <summary>
        /// A <see cref="TimerWindow"/>.
        /// </summary>
        private readonly TimerWindow timerWindow;

        /// <summary>
        /// A <see cref="Grid"/> containing the controls of the <see cref="timerWindow"/>.
        /// </summary>
        private readonly Grid controlsGrid;

        /// <summary>
        /// A <see cref="TextBox"/> for accepting user input of a time interval or a date and time on the <see
        /// cref="timerWindow"/>.
        /// </summary>
        private readonly TextBox timerTextBox;

        /// <summary>
        /// A <see cref="TextBox"/> for accepting user input of a title or description on the <see cref="timerWindow"/>.
        /// </summary>
        private readonly TextBox titleTextBox;

        /// <summary>
        /// An array of the <see cref="Button"/> elements on the <see cref="timerWindow"/>.
        /// </summary>
        private readonly Button[] buttons;

        /// <summary>
        /// The <see cref="Window.ActualWidth"/> of the <see cref="timerWindow"/> during the last scale operation.
        /// </summary>
        private double lastWindowWidth;

        /// <summary>
        /// The <see cref="Window.ActualHeight"/> of the <see cref="timerWindow"/> during the last scale operation.
        /// </summary>
        private double lastWindowHeight;

        /// <summary>
        /// The base scale factor during the last scale operation.
        /// </summary>
        /// <seealso cref="GetBaseScaleFactor"/>
        private double lastBaseScaleFactor;

        /// <summary>
        /// Initializes a new instance of the <see cref="TimerScaler"/> class.
        /// </summary>
        /// <param name="timer">A <see cref="Timer"/>.</param>
        /// <param name="timerWindow">A <see cref="TimerWindow"/>.</param>
        /// <param name="controlsGrid">A <see cref="Grid"/> containing the controls of the <see cref="TimerWindow"/>.
        /// </param>
        /// <param name="timerTextBox">A <see cref="TextBox"/> for accepting user input of a time interval or a date
        /// and time on the <see cref="TimerWindow"/>.</param>
        /// <param name="titleTextBox">A <see cref="TextBox"/> for accepting user input of a title or description on
        /// the <see cref="TimerWindow"/>.</param>
        /// <param name="buttons">An array of the <see cref="Button"/> elements on the <see cref="TimerWindow"/>.
        /// </param>
        /// <exception cref="ArgumentNullException">If any of the parameters are <c>null</c>.</exception>
        public TimerScaler(Timer timer, TimerWindow timerWindow, Grid controlsGrid, TextBox timerTextBox, TextBox titleTextBox, Button[] buttons)
        {
            // Check parameters
            if (timer == null)
            {
                throw new ArgumentNullException("timer");
            }

            if (timerWindow == null)
            {
                throw new ArgumentNullException("timerWindow");
            }

            if (controlsGrid == null)
            {
                throw new ArgumentNullException("controlsGrid");
            }

            if (timerTextBox == null)
            {
                throw new ArgumentNullException("timerTextBox");
            }

            if (titleTextBox == null)
            {
                throw new ArgumentNullException("titleTextBox");
            }

            if (buttons == null)
            {
                throw new ArgumentNullException("buttons");
            }

            // Initialize class
            this.timer = timer;
            this.timerWindow = timerWindow;
            this.controlsGrid = controlsGrid;
            this.timerTextBox = timerTextBox;
            this.titleTextBox = titleTextBox;
            this.buttons = (Button[])buttons.Clone();
        }

        /// <summary>
        /// Scales the <see cref="TimerWindow"/> to ensure that controls shrink and grow with the window size, and
        /// updates the <see cref="Timer"/> interval to ensure smooth animation of the progress bar.
        /// </summary>
        public void Scale()
        {
            this.ScaleControls();
            this.ScaleTimerInterval();
        }

        /// <summary>
        /// Scales the <see cref="timerWindow"/> and its controls.
        /// </summary>
        private void ScaleControls()
        {
            double baseScaleFactor = this.GetBaseScaleFactor();
            double reducedScaleFactor = this.GetReducedScaleFactor(baseScaleFactor, 0.5 /* reductionFactor */);

            if (!this.ShouldScaleControls(baseScaleFactor))
            {
                return;
            }

            this.ScaleControls(baseScaleFactor, reducedScaleFactor);
        }

        /// <summary>
        /// Scales the <see cref="timerWindow"/> and its controls using the specified factors.
        /// </summary>
        /// <param name="baseScaleFactor">The base scale factor.</param>
        /// <param name="reducedScaleFactor">The reduced scale factor.</param>
        /// <seealso cref="GetBaseScaleFactor"/>
        /// <seealso cref="GetReducedScaleFactor"/>
        private void ScaleControls(double baseScaleFactor, double reducedScaleFactor)
        {
            this.controlsGrid.Margin = new Thickness(reducedScaleFactor * BaseControlsGridMargin);

            this.titleTextBox.FontSize = reducedScaleFactor * BaseFontSize;
            this.timerTextBox.FontSize = baseScaleFactor * BaseInputFontSize;
            this.timerTextBox.Margin = new Thickness(0, baseScaleFactor * BaseInputTopMargin, 0, baseScaleFactor * BaseInputBottomMargin);

            foreach (Button button in this.buttons)
            {
                button.FontSize = reducedScaleFactor * BaseFontSize;
                button.Margin = new Thickness(baseScaleFactor * BaseButtonMargin, 0, baseScaleFactor * BaseButtonMargin, 0);
            }

            this.lastWindowWidth = this.timerWindow.ActualWidth;
            this.lastWindowHeight = this.timerWindow.ActualHeight;
            this.lastBaseScaleFactor = baseScaleFactor;
        }

        /// <summary>
        /// <para>
        /// Returns a value indicating whether the controls should be scaled using the specified base scale factor.
        /// </para><para>
        /// The controls should be scaled each time the window size changes to ensure smooth window scaling.
        /// </para><para>
        /// The controls should also be scaled when it is necessary to do so to ensure that all of the text in the
        /// <see cref="timerTextBox"/> is visible. However, the controls should not be scaled when the change in the
        /// base scale factor is small and entirely attributable to text changes. This prevents small text changes from
        /// causing constant rescaling of the interface.
        /// </para>
        /// </summary>
        /// <param name="baseScaleFactor">The base scale factor.</param>
        /// <returns>A value indicating whether the controls should be scaled.</returns>
        private bool ShouldScaleControls(double baseScaleFactor)
        {
            // Scale each time the window size changes to ensure smooth window scaling
            if (!this.timerWindow.ActualWidth.Equals(this.lastWindowWidth) ||
                !this.timerWindow.ActualHeight.Equals(this.lastWindowHeight))
            {
                return true;
            }

            // The base scale factor changing when the window size has not changed indicates that the text has changed,
            // but we scale only when the base scale factor change attributable to a text change exceeds a threshold
            if (Math.Abs(baseScaleFactor - this.lastBaseScaleFactor) > 0.05)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Returns the base scale factor for the user interface based on the width and height of the <see
        /// cref="timerWindow"/> and the width of the text in the <see cref="timerTextBox"/>.
        /// </summary>
        /// <returns>The base scale factor.</returns>
        private double GetBaseScaleFactor()
        {
            Typeface typeface = new Typeface(this.timerTextBox.FontFamily, this.timerTextBox.FontStyle, this.timerTextBox.FontWeight, this.timerTextBox.FontStretch);
            FormattedText formattedText = new FormattedText(this.timerTextBox.Text, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, typeface, BaseInputFontSize, this.timerTextBox.Foreground);
            double textWidth = 1.05 * formattedText.Width;

            double widthFactor = Math.Max(this.timerWindow.ActualWidth / BaseWindowWidth, 2.0 / 3.0);
            double heightFactor = Math.Max(this.timerWindow.ActualHeight / BaseWindowHeight, 2.0 / 3.0);
            double textFactor = this.timerTextBox.ActualWidth / textWidth;

            double[] factors = { widthFactor, heightFactor, textFactor };
            return factors.Min();
        }

        /// <summary>
        /// Returns the reduced scale factor. The reduced scale factor is computed by multiplying the amount by which
        /// the base scale factor exceeds 1.0 by the reduction factor. If the base scale factor is less than 1.0, the
        /// reduced scale factor is 1.0.
        /// </summary>
        /// <param name="baseScaleFactor">The base scale factor.</param>
        /// <param name="reductionFactor">The reduction factor.</param>
        /// <returns>The reduced scale factor.</returns>
        private double GetReducedScaleFactor(double baseScaleFactor, double reductionFactor)
        {
            if (baseScaleFactor < 1.0)
            {
                return 1.0;
            }

            double difference = baseScaleFactor - 1.0;
            return 1.0 + (difference * reductionFactor);
        }

        /// <summary>
        /// Updates the <see cref="timer"/> interval to ensure smooth animation of the progress bar.
        /// </summary>
        private void ScaleTimerInterval()
        {
            int intervalInMilliseconds = 100;

            if (this.timer.TotalTime.HasValue)
            {
                intervalInMilliseconds = (int)(1000 * this.timer.TotalTime.Value.TotalSeconds / this.timerWindow.ActualWidth / 2);

                if (intervalInMilliseconds < 10)
                {
                    intervalInMilliseconds = 10;
                }
                else if (intervalInMilliseconds > 100)
                {
                    intervalInMilliseconds = 100;
                }
            }

            this.timer.Interval = new TimeSpan(0, 0, 0, 0, intervalInMilliseconds);
        }
    }
}
