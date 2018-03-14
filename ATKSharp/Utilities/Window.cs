//-----------------------------------------------------------------------
// <copyright file="Window.cs" company="Aaron Anderson">
//     Copyright (c) Aaron Anderson. All rights reserved.
// </copyright>
// <license type="MIT">
// See LICENSE.md in the project root for full license information.  
// </license>
// <summary>This is the Window class.</summary>
//-----------------------------------------------------------------------
namespace ATKSharp.Generators.Oscillators
{
    using System;
    using ATKSharp.Utilities;

    /// <summary>
    /// The WindowType enum.
    /// </summary>
    public enum WindowType
    {
        /// <summary>
        /// The None window type.
        /// </summary>
        None,

        /// <summary>
        /// The Hann window type.
        /// </summary>
        Hann,

        /// <summary>
        /// The Cosine window type.
        /// </summary>
        Cosine,

        /// <summary>
        /// The Rectangular window type.
        /// </summary>
        Rectangular,

        /// <summary>
        /// The Triangular window type.
        /// </summary>
        Triangular,

        /// <summary>
        /// The Blackman-Harris window type.
        /// </summary>
        BlackmanHarris,

        /// <summary>
        /// The Blackman-Nutall window type.
        /// </summary>
        BlackmanNutall,

        /// <summary>
        /// The Gauss window type.
        /// </summary>
        Gauss
    }

    /// <summary>
    /// The Window class.
    /// </summary>
    public class Window : BaseGenerator
    {
        #region Fields
        private WindowMethod windowMethod;
        private float increment;
        private float duration;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Window"/> class.
        /// </summary>
        /// <param name="windowType">The initial window type.</param>
        /// <param name="duration">The intial duration.</param>
        public Window(WindowType windowType = WindowType.None, float duration = 100)
        {
            this.WindowType = windowType;
            this.Duration = duration;
            this.Phase = 0;
        }
        #endregion

        #region Delegates
        private delegate double WindowMethod(double phase);
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the window type.
        /// </summary>
        public WindowType WindowType
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the duration.
        /// </summary>
        public float Duration
        {
            get
            {
                return Utilities.SamplesToMilliseconds((int)this.duration);
            }

            set
            {
                this.duration = Utilities.MillisecondsToSamples(value);
                this.increment = 1 / this.duration;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Modifies the phase with a Hann window.
        /// </summary>
        /// <param name="phase">The phase.</param>
        /// <returns>The modified phase.</returns>
        public static double Hann(double phase)
        {
            return 0.5 * (1.0 - Math.Cos((2.0f * Math.PI * phase) / 1.0f));
        }

        /// <summary>
        /// Modifies the phase with a Cosine window.
        /// </summary>
        /// <param name="phase">The phase.</param>
        /// <returns>The modified phase.</returns>
        public static double Cosine(double phase)
        {
            return Math.Sin((Math.PI * phase) / 1.0f);
        }

        /// <summary>
        /// Modifies the phase with a Rectangular window.
        /// </summary>
        /// <param name="phase">The phase.</param>
        /// <returns>The modified phase.</returns>
        public static double Rectangular(double phase)
        {
            return 1;
        }

        /// <summary>
        /// Modifies the phase with a Triangular window.
        /// </summary>
        /// <param name="phase">The phase.</param>
        /// <returns>The modified phase.</returns>
        public static double Triangular(double phase)
        {
            return 2.0f * (0.5 - Math.Abs(phase - 0.5));
        }

        /// <summary>
        /// Modifies the phase with a Blackman-Harris window.
        /// </summary>
        /// <param name="phase">The phase.</param>
        /// <returns>The modified phase.</returns>
        public static double BlackmanHarris(double phase)
        {
            return 0.35875 -
            (0.48829 * Math.Cos((2 * Math.PI * phase) / 1.0f)) +
            (0.14128 * Math.Cos((4 * Math.PI * phase) / 1.0f)) +
            (0.01168 * Math.Cos((6 * Math.PI * phase) / 1.0f));
        }

        /// <summary>
        /// Modifies the phase with a Blackman-Nutall window.
        /// </summary>
        /// <param name="phase">The phase.</param>
        /// <returns>The modified phase.</returns>
        public static double BlackmanNutall(double phase)
        {
            return 0.3635819 -
            (0.4891775 * Math.Cos((2 * Math.PI * phase) / 1.0f)) +
            (0.1365995 * Math.Cos((4 * Math.PI * phase) / 1.0f)) +
            (0.0106411 * Math.Cos((6 * Math.PI * phase) / 1.0f));
        }

        /// <summary>
        /// Modifies the phase with a Gauss window.
        /// </summary>
        /// <param name="phase">The phase.</param>
        /// <returns>The modified phase.</returns>
        public static double Gauss(double phase)
        {
            double p = 1.0 / (2.0 * 0.02);
            double k = phase - 0.5;
            return Math.Exp(-k * k * p);
        }

        /// <summary>
        /// Generates a signal.
        /// </summary>
        /// <returns>The signal.</returns>
        public override float Generate()
        {
            switch (WindowType)
            {
                case WindowType.None:
                    this.windowMethod = null;
                    break;
                case WindowType.Hann:
                    this.windowMethod = Hann;
                    break;
                case WindowType.Cosine:
                    this.windowMethod = Cosine;
                    break;
                case WindowType.Rectangular:
                    this.windowMethod = Rectangular;
                    break;
                case WindowType.Triangular:
                    this.windowMethod = Triangular;
                    break;
                case WindowType.BlackmanHarris:
                    this.windowMethod = BlackmanHarris;
                    break;
                case WindowType.BlackmanNutall:
                    this.windowMethod = BlackmanNutall;
                    break;
                case WindowType.Gauss:
                    this.windowMethod = Gauss;
                    break;
                default:
                    this.windowMethod = null;
                    break;
            }

            this.CurrentSample = this.windowMethod != null ? (float)this.windowMethod(this.Phase) : 0f;
            this.Phase += this.increment;
            if (this.Phase >= 1.0)
            {
                this.Phase -= 1.0;
            }

            return this.CurrentSample;
        }
        #endregion
    }
}
