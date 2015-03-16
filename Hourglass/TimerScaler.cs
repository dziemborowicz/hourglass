// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimerScaler.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass
{
    using System;
    using System.Globalization;
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
        /// The default height of a <see cref="timerWindow"/>.
        /// </summary>
        private const double BaseWindowHeight = 150;

        /// <summary>
        /// The size at and above which the <see cref="LargeFontFamily"/> will be used to render controls.
        /// </summary>
        /// <seealso cref="LargeFontFamily"/>
        private const double LargeFontSize = 14;

        /// <summary>
        /// The font family used for the controls.
        /// </summary>
        private static readonly FontFamily FontFamily = new FontFamily("Segoe UI");

        /// <summary>
        /// The font family used for the controls at larger font sizes.
        /// </summary>
        /// <seealso cref="LargeFontSize"/>
        private static readonly FontFamily LargeFontFamily = new FontFamily("Segoe UI Light, Segoe UI");

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
        /// The width of the three reference texts "88 minutes 88 seconds", "88 hours 88 minutes 88 seconds", and "888
        /// days 88 hours 88 minutes 88 seconds" respectively.
        /// </summary>
        private readonly double[] referenceTextWidths = new double[3];

        /// <summary>
        /// The most recent scale factor for controls other than the <see cref="timerTextBox"/>.
        /// </summary>
        private double lastScaleFactor = double.NaN;

        /// <summary>
        /// The most recent scale factor for the <see cref="timerTextBox"/>.
        /// </summary>
        private double lastTimerTextBoxScaleFactor = double.NaN;

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

            // Cache reference text widths
            this.referenceTextWidths[0] = new FormattedText("88 minutes 88 seconds", CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface(FontFamily, this.timerTextBox.FontStyle, this.timerTextBox.FontWeight, this.timerTextBox.FontStretch), BaseInputFontSize, this.timerTextBox.Foreground).Width;
            this.referenceTextWidths[1] = new FormattedText("88 hours 88 minutes 88 seconds", CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface(FontFamily, this.timerTextBox.FontStyle, this.timerTextBox.FontWeight, this.timerTextBox.FontStretch), BaseInputFontSize, this.timerTextBox.Foreground).Width;
            this.referenceTextWidths[2] = new FormattedText("888 days 88 hours 88 minutes 88 seconds", CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface(FontFamily, this.timerTextBox.FontStyle, this.timerTextBox.FontWeight, this.timerTextBox.FontStretch), BaseInputFontSize, this.timerTextBox.Foreground).Width;
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
        /// Scales the <see cref="TimerWindow"/> to ensure that controls shrink and grow with the window size.
        /// </summary>
        private void ScaleControls()
        {
            double inputScaleFactor = this.GetTimerTextBoxScaleFactor();
            double otherScaleFactor = this.GetScaleFactor(inputScaleFactor);

            if (inputScaleFactor > 0 && (!inputScaleFactor.Equals(this.lastTimerTextBoxScaleFactor) || !otherScaleFactor.Equals(this.lastScaleFactor)))
            {
                this.controlsGrid.Margin = new Thickness(otherScaleFactor * BaseControlsGridMargin);

                this.timerTextBox.FontSize = inputScaleFactor * BaseInputFontSize;
                this.timerTextBox.FontFamily = this.timerTextBox.FontSize < LargeFontSize ? FontFamily : LargeFontFamily;
                this.timerTextBox.Margin = new Thickness(0, inputScaleFactor * BaseInputTopMargin, 0, inputScaleFactor * BaseInputBottomMargin);

                this.titleTextBox.FontSize = otherScaleFactor * BaseFontSize;
                this.titleTextBox.FontFamily = this.titleTextBox.FontSize < LargeFontSize ? FontFamily : LargeFontFamily;

                foreach (Button button in this.buttons)
                {
                    button.FontSize = otherScaleFactor * BaseFontSize;
                    button.FontFamily = button.FontSize < LargeFontSize ? FontFamily : LargeFontFamily;
                    button.Margin = new Thickness(otherScaleFactor * otherScaleFactor * BaseButtonMargin, 0, otherScaleFactor * otherScaleFactor * BaseButtonMargin, 0);
                }

                this.lastTimerTextBoxScaleFactor = inputScaleFactor;
                this.lastScaleFactor = otherScaleFactor;
            }
        }

        /// <summary>
        /// Updates the <see cref="Timer"/> interval to ensure smooth animation of the progress bar.
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

        /// <summary>
        /// Returns the scale factor for controls other than the <see cref="timerTextBox"/>.
        /// </summary>
        /// <param name="timerTextBoxScaleFactor">The scale factor for the <see cref="timerTextBox"/>.</param>
        /// <returns>The scale factor.</returns>
        /// <seealso cref="GetTimerTextBoxScaleFactor"/>
        private double GetScaleFactor(double timerTextBoxScaleFactor)
        {
            return Math.Max(1, 1 + Math.Log(timerTextBoxScaleFactor));
        }

        /// <summary>
        /// Returns the scale factor for the <see cref="timerTextBox"/>.
        /// </summary>
        /// <returns>The scale factor for the <see cref="timerTextBox"/>.</returns>
        /// <seealso cref="GetScaleFactor"/>
        private double GetTimerTextBoxScaleFactor()
        {
            return Math.Min(this.timerWindow.ActualHeight / BaseWindowHeight, 0.8 * this.timerTextBox.ActualWidth / this.GetReferenceTextWidth());
        }

        /// <summary>
        /// Returns the width of the <see cref="timerTextBox"/> reference text.
        /// </summary>
        /// <remarks>
        /// Reference text is used rather than the actual text to avoid small variations in the scale factor as a
        /// result of small changes in the width of the actual text as the timer counts down.
        /// </remarks>
        /// <returns>The width of the <see cref="timerTextBox"/> reference text.</returns>
        private double GetReferenceTextWidth()
        {
            if (this.timerTextBox.Text.Contains("day"))
            {
                return this.referenceTextWidths[2];
            }

            if (this.timerTextBox.Text.Contains("hour"))
            {
                return this.referenceTextWidths[1];
            }

            return this.referenceTextWidths[0];
        }
    }
}
