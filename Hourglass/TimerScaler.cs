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
        /// The default margin for the <see cref="Grid"/> controls containing the controls of the <see
        /// cref="timerWindow"/>.
        /// </summary>
        private const double BaseControlsGridMargin = 10;

        /// <summary>
        /// The default margin for the <see cref="StackPanel"/> controls containing the controls of the <see
        /// cref="timerWindow"/>.
        /// </summary>
        private const double BaseControlsPanelMargin = 20;

        /// <summary>
        /// The default font size used to render controls in the <see cref="timerWindow"/>.
        /// </summary>
        private const double BaseFontSize = 12;

        /// <summary>
        /// The default font size of the larger text controls on the <see cref="timerWindow"/>.
        /// </summary>
        private const double BasePrimaryTextControlFontSize = 18;

        /// <summary>
        /// The default top margin of the larger text controls on the <see cref="timerWindow"/>.
        /// </summary>
        private const double BasePrimaryTextControlTopMargin = 1;

        /// <summary>
        /// The default bottom margin of the larger text controls on the <see cref="timerWindow"/>.
        /// </summary>
        private const double BasePrimaryTextControlBottomMargin = 4;

        /// <summary>
        /// The default border thickness for the <see cref="Border"/> controls that visualize validation errors and
        /// expired timer state.
        /// </summary>
        private const double BaseBorderThickness = 1;

        /// <summary>
        /// The default margin for the <see cref="Border"/> controls that visualize validation errors and expired timer
        /// state.
        /// </summary>
        private const double BaseBorderMargin = 15;

        /// <summary>
        /// The default width of a <see cref="timerWindow"/>.
        /// </summary>
        private const double BaseWindowWidth = 350;

        /// <summary>
        /// The default height of a <see cref="timerWindow"/>.
        /// </summary>
        private const double BaseWindowHeight = 150;

        /// <summary>
        /// The change in the base scale factor which is ignored to prevent constant changes in the base scale factor
        /// resulting from small changes in the content of the <see cref="timerTextBox"/>.
        /// </summary>
        private const double BaseScaleFactorSmoothingThreshold = 0.05;

        /// <summary>
        /// The reduction factor that relates the base scale factor with the reduced scale factor.
        /// </summary>
        private const double ReductionFactor = 0.5;

        /// <summary>
        /// A <see cref="TimerWindow"/>.
        /// </summary>
        private TimerWindow timerWindow;

        /// <summary>
        /// The <see cref="Grid"/> control that is the first child of the <see cref="timerWindow"/>.
        /// </summary>
        private Grid outerGrid;

        /// <summary>
        /// The <see cref="Grid"/> control that contains the <see cref="controlsPanel"/>.
        /// </summary>
        private Grid innerGrid;

        /// <summary>
        /// The <see cref="StackPanel"/> control that contains the controls of the <see cref="timerWindow"/>.
        /// </summary>
        private StackPanel controlsPanel;

        /// <summary>
        /// The larger <see cref="TextBox"/> on the <see cref="timerWindow"/>.
        /// </summary>
        private TextBox timerTextBox;

        /// <summary>
        /// The smaller <see cref="TextBox"/> on the <see cref="timerWindow"/>.
        /// </summary>
        private TextBox titleTextBox;

        /// <summary>
        /// The <see cref="Border"/> that animates to notify the user that the timer has expired or that the input was
        /// invalid.
        /// </summary>
        private Border innerNotificationBorder;

        /// <summary>
        /// An array of the <see cref="Button"/> elements on the <see cref="timerWindow"/>.
        /// </summary>
        private Button[] buttons;

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
        private double lastBaseScaleFactor = 1.0;
        
        /// <summary>
        /// Binds the <see cref="TimerScaler"/> to a <see cref="TimerWindow"/>.
        /// </summary>
        /// <param name="window">A <see cref="TimerWindow"/>.</param>
        public void Bind(TimerWindow window)
        {
            // Validate parameters
            if (window == null)
            {
                throw new ArgumentNullException("window");
            }

            // Validate state
            if (this.timerWindow != null)
            {
                throw new InvalidOperationException();
            }

            // Initialize members
            this.timerWindow = window;

            this.outerGrid = this.timerWindow.OuterGrid;
            this.innerGrid = this.timerWindow.InnerGrid;
            this.controlsPanel = this.timerWindow.ControlsPanel;
            this.timerTextBox = this.timerWindow.TimerTextBox;
            this.titleTextBox = this.timerWindow.TitleTextBox;
            this.innerNotificationBorder = this.timerWindow.InnerNotificationBorder;
            this.buttons = new[]
            {
                this.timerWindow.StartButton,
                this.timerWindow.PauseButton,
                this.timerWindow.ResumeButton,
                this.timerWindow.StopButton,
                this.timerWindow.ResetButton,
                this.timerWindow.CancelButton
            };

            // Hook up events
            this.timerWindow.Loaded += (s, e) => this.Scale();
            this.timerWindow.SizeChanged += (s, e) => this.Scale();
            this.timerTextBox.TextChanged += (s, e) => this.Scale();
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
            double reducedScaleFactor = this.GetReducedScaleFactor(baseScaleFactor, ReductionFactor);

            if (!this.ShouldScaleControls(baseScaleFactor))
            {
                return;
            }

            this.ScaleControls(baseScaleFactor, reducedScaleFactor);

            this.lastWindowWidth = this.timerWindow.ActualWidth;
            this.lastWindowHeight = this.timerWindow.ActualHeight;
            this.lastBaseScaleFactor = baseScaleFactor;
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
            this.innerGrid.Margin = new Thickness(reducedScaleFactor * BaseControlsGridMargin);

            this.controlsPanel.Margin = new Thickness(reducedScaleFactor * BaseControlsPanelMargin, 0, reducedScaleFactor * BaseControlsPanelMargin, 0);

            this.timerTextBox.FontSize = baseScaleFactor * BasePrimaryTextControlFontSize;
            this.timerTextBox.Margin = new Thickness(0, baseScaleFactor * BasePrimaryTextControlTopMargin, 0, baseScaleFactor * BasePrimaryTextControlBottomMargin);

            this.titleTextBox.FontSize = reducedScaleFactor * BaseFontSize;

            this.innerNotificationBorder.BorderThickness = new Thickness(reducedScaleFactor * BaseBorderThickness);
            this.innerNotificationBorder.Margin = new Thickness(reducedScaleFactor * BaseBorderMargin);

            foreach (Button button in this.buttons)
            {
                button.FontSize = reducedScaleFactor * BaseFontSize;
                button.Margin = new Thickness(baseScaleFactor * BaseButtonMargin, 0, baseScaleFactor * BaseButtonMargin, 0);
            }
        }

        /// <summary>
        /// <para>
        /// Returns a value indicating whether the controls should be scaled using the specified base scale factor.
        /// </para><para>
        /// The controls should be scaled each time the window size changes to ensure smooth window scaling.
        /// </para><para>
        /// The controls should also be scaled when it is necessary to do so to ensure that all of the text in the
        /// <see cref="timerTextBox"/> is visible. However, the controls should not be scaled when the change in
        /// the base scale factor is small and entirely attributable to text changes. This prevents small text changes
        /// from causing constant rescaling of the interface.
        /// </para>
        /// </summary>
        /// <param name="baseScaleFactor">The base scale factor.</param>
        /// <returns>A value indicating whether the controls should be scaled.</returns>
        private bool ShouldScaleControls(double baseScaleFactor)
        {
            // Do not scale the window if it is not loaded or visible
            if (!this.timerWindow.IsVisible)
            {
                return false;
            }

            // Scale each time the window size changes to ensure smooth window scaling
            if (!this.timerWindow.ActualWidth.Equals(this.lastWindowWidth) ||
                !this.timerWindow.ActualHeight.Equals(this.lastWindowHeight))
            {
                return true;
            }

            // The base scale factor changing when the window size has not changed indicates that the text has changed,
            // but we scale only when the base scale factor change attributable to a text change exceeds a threshold
            double baseScaleFactorChagne = Math.Abs((baseScaleFactor / this.lastBaseScaleFactor) - 1);
            if (baseScaleFactorChagne > BaseScaleFactorSmoothingThreshold)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Returns the base scale factor for the user interface based on the width and height of the <see
        /// cref="timerWindow"/> and the width of the text in the visible <see cref="timerTextBox"/>.
        /// </summary>
        /// <returns>The base scale factor.</returns>
        private double GetBaseScaleFactor()
        {
            double widthFactor = Math.Max(this.timerWindow.ActualWidth / BaseWindowWidth, 2.0 / 3.0);
            double heightFactor = Math.Max(this.timerWindow.ActualHeight / BaseWindowHeight, 2.0 / 3.0);
            double sizeFactor = Math.Min(widthFactor, heightFactor);
            double reducedSizeFactor = this.GetReducedScaleFactor(sizeFactor, ReductionFactor);

            double textWidth = this.GetTextWidth();
            double textWidthWithBuffer = textWidth * (1 + BaseScaleFactorSmoothingThreshold);
            double textControlWidth = this.outerGrid.ActualWidth - (2.0 * BaseControlsGridMargin * reducedSizeFactor) - (2.0 * BaseControlsPanelMargin * reducedSizeFactor);
            double textFactor = !textWidth.Equals(0.0) ? textControlWidth / textWidthWithBuffer : double.PositiveInfinity;

            return Math.Min(sizeFactor, textFactor);
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
        /// Gets the width of the text in the main text box that is currently visible.
        /// </summary>
        /// <returns>The width of the text in the main text box that is currently visible.</returns>
        private double GetTextWidth()
        {
            Typeface typeface = new Typeface(this.timerTextBox.FontFamily, this.timerTextBox.FontStyle, this.timerTextBox.FontWeight, this.timerTextBox.FontStretch);
            FormattedText formattedText = new FormattedText(this.timerTextBox.Text, CultureInfo.CurrentCulture, this.timerTextBox.FlowDirection, typeface, BasePrimaryTextControlFontSize, this.timerTextBox.Foreground);
            return formattedText.Width;
        }

        /// <summary>
        /// Updates the timer interval to ensure smooth animation of the progress bar.
        /// </summary>
        private void ScaleTimerInterval()
        {
            if (!this.ShouldScaleTimerInterval())
            {
                return;
            }

            Timer timer = this.timerWindow.Timer;

            if (timer.TotalTime.HasValue)
            {
                double interval = timer.TotalTime.Value.TotalMilliseconds / this.timerWindow.ActualWidth / 2;
                interval = MathUtility.LimitToRange(interval, 10, Timer.DefaultInterval.TotalMilliseconds);
                timer.Interval = TimeSpan.FromMilliseconds(interval);
            }
            else
            {
                timer.Interval = Timer.DefaultInterval;
            }
        }

        /// <summary>
        /// Returns a value indicating whether the timer interval should be scaled.
        /// </summary>
        /// <returns>A value indicating whether the timer interval should be scaled.</returns>
        private bool ShouldScaleTimerInterval()
        {
            // Do not scale the window if it is not loaded or visible
            if (!this.timerWindow.IsVisible)
            {
                return false;
            }

            return true;
        }
    }
}
