// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TextControlWrapper.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass
{
    using System;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    /// <summary>
    /// A wrapper around <see cref="TextBox"/> and <see cref="TextBlock"/> controls.
    /// </summary>
    public class TextControlWrapper
    {
        /// <summary>
        /// The wrapped <see cref="TextBox"/>, or <c>null</c> if the wrapped control is not a <see cref="TextBox"/>.
        /// </summary>
        private readonly TextBox textBox;

        /// <summary>
        /// The wrapped <see cref="TextBlock"/>, or <c>null</c> if the wrapped control is not a <see cref="TextBlock"/>.
        /// </summary>
        private readonly TextBlock textBlock;

        /// <summary>
        /// Initializes a new instance of the <see cref="TextControlWrapper"/> class that wraps a <see
        /// cref="TextBox"/>.
        /// </summary>
        /// <param name="textBox">A <see cref="TextBox"/>.</param>
        public TextControlWrapper(TextBox textBox)
        {
            if (textBox == null)
            {
                throw new ArgumentNullException("textBox");
            }

            this.textBox = textBox;

            this.textBox.TextChanged += (s, e) => this.OnTextChanged();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextControlWrapper"/> class that wraps a <see
        /// cref="TextBlock"/>.
        /// </summary>
        /// <param name="textBlock">A <see cref="TextBlock"/>.</param>
        public TextControlWrapper(TextBlock textBlock)
        {
            if (textBlock == null)
            {
                throw new ArgumentNullException("textBlock");
            }

            this.textBlock = textBlock;

            DependencyPropertyDescriptor
                .FromProperty(TextBlock.TextProperty, typeof(TextBlock))
                .AddValueChanged(this.textBlock, (s, e) => this.OnTextChanged());
        }

        /// <summary>
        /// Raised when content changes in the text control.
        /// </summary>
        public event EventHandler TextChanged;

        /// <summary>
        /// Gets or sets the direction that text and other user interface elements flow within any parent element that
        /// controls their layout.
        /// </summary>
        public FlowDirection FlowDirection
        {
            get
            {
                return this.textBox != null ? this.textBox.FlowDirection : this.textBlock.FlowDirection;
            }

            set
            {
                if (this.textBox != null)
                {
                    this.textBox.FlowDirection = value;
                }
                else
                {
                    this.textBlock.FlowDirection = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the font family of the control.
        /// </summary>
        public FontFamily FontFamily
        {
            get
            {
                return this.textBox != null ? this.textBox.FontFamily : this.textBlock.FontFamily;
            }

            set
            {
                if (this.textBox != null)
                {
                    this.textBox.FontFamily = value;
                }
                else
                {
                    this.textBlock.FontFamily = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the font size.
        /// </summary>
        public double FontSize
        {
            get
            {
                return this.textBox != null ? this.textBox.FontSize : this.textBlock.FontSize;
            }

            set
            {
                if (this.textBox != null)
                {
                    this.textBox.FontSize = value;
                }
                else
                {
                    this.textBlock.FontSize = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the degree to which a font is condensed or expanded on the screen.
        /// </summary>
        public FontStretch FontStretch
        {
            get
            {
                return this.textBox != null ? this.textBox.FontStretch : this.textBlock.FontStretch;
            }

            set
            {
                if (this.textBox != null)
                {
                    this.textBox.FontStretch = value;
                }
                else
                {
                    this.textBlock.FontStretch = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the font style.
        /// </summary>
        public FontStyle FontStyle
        {
            get
            {
                return this.textBox != null ? this.textBox.FontStyle : this.textBlock.FontStyle;
            }

            set
            {
                if (this.textBox != null)
                {
                    this.textBox.FontStyle = value;
                }
                else
                {
                    this.textBlock.FontStyle = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the weight or thickness of the specified font.
        /// </summary>
        public FontWeight FontWeight
        {
            get
            {
                return this.textBox != null ? this.textBox.FontWeight : this.textBlock.FontWeight;
            }

            set
            {
                if (this.textBox != null)
                {
                    this.textBox.FontWeight = value;
                }
                else
                {
                    this.textBlock.FontWeight = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets a brush that describes the foreground color.
        /// </summary>
        public Brush Foreground
        {
            get
            {
                return this.textBox != null ? this.textBox.Foreground : this.textBlock.Foreground;
            }

            set
            {
                if (this.textBox != null)
                {
                    this.textBox.Foreground = value;
                }
                else
                {
                    this.textBlock.Foreground = value;
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether this element is visible in the user interface.
        /// </summary>
        public bool IsVisible
        {
            get
            {
                return this.textBox != null ? this.textBox.IsVisible : this.textBlock.IsVisible;
            }
        }

        /// <summary>
        /// Gets or sets the outer margin of an element.
        /// </summary>
        public Thickness Margin
        {
            get
            {
                return this.textBox != null ? this.textBox.Margin : this.textBlock.Margin;
            }

            set
            {
                if (this.textBox != null)
                {
                    this.textBox.Margin = value;
                }
                else
                {
                    this.textBlock.Margin = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the text contents of the control.
        /// </summary>
        public string Text
        {
            get
            {
                return this.textBox != null ? this.textBox.Text : this.textBlock.Text;
            }

            set
            {
                if (this.textBox != null)
                {
                    this.textBox.Text = value;
                }
                else
                {
                    this.textBlock.Text = value;
                }
            }
        }

        /// <summary>
        /// Raises the <see cref="TextChanged"/> event.
        /// </summary>
        protected void OnTextChanged()
        {
            EventHandler eventHandler = this.TextChanged;

            if (eventHandler != null)
            {
                eventHandler(this, EventArgs.Empty);
            }
        }
    }
}
