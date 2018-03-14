//-----------------------------------------------------------------------
// <copyright file="Biquad.cs" company="Aaron Anderson">
//     Copyright (c) Aaron Anderson. All rights reserved.
// </copyright>
// <license type="MIT">
// See LICENSE.md in the project root for full license information.  
// </license>
// <summary>This is the Biquad class.</summary>
//-----------------------------------------------------------------------
namespace ATKSharp.Modifiers
{
    using System;

    /// <summary>
    /// The Biquad class. This behaves similarily to the biquad~ in MaxMSP
    /// </summary>
    public class Biquad : BaseModifier
    {
        #region Fields
        private ModifierType filterType;
        private float q;
        private float frequency;
        private float peakGain;
        private float a0, a1, a2, b1, b2, z1, z2;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Biquad"/> class.
        /// </summary>
        /// <param name="type">The modifier type.</param>
        /// <param name="frequency">The frequency (Hz).</param>
        /// <param name="q">The quality.</param>
        /// <param name="peakGain">The peak gain.</param>
        public Biquad(ModifierType type = ModifierType.LowPass, float frequency = 500f, float q = .707f, float peakGain = 0f)
        {
            this.ModifierType = type;
            this.Q = q;
            this.Frequency = frequency / ATKSettings.SampleRate;
            this.PeakGain = peakGain;
            this.a0 = 1.0f;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the modifier type.
        /// </summary>
        public override ModifierType ModifierType
        {
            get
            {
                return this.filterType;
            }

            set
            {
                this.filterType = value;
                this.CalcBiquad();
            }
        }

        /// <summary>
        /// Gets or sets the frequency.
        /// </summary>
        public override float Frequency
        {
            get
            {
                return this.frequency * ATKSettings.SampleRate;
            }

            set
            {
                this.frequency = value / ATKSettings.SampleRate;
                this.CalcBiquad();
            }
        }

        /// <summary>
        /// Gets or sets the Q aka quality factor aka bandwidth.
        /// </summary>
        public float Q
        {
            get
            {
                return this.q;
            }

            set
            {
                this.q = value;
                this.CalcBiquad();
            }
        }

        /// <summary>
        /// Gets or sets the peak gain.
        /// </summary>
        public float PeakGain
        {
            get
            {
                return this.peakGain;
            }

            set
            {
                this.peakGain = value;
                this.CalcBiquad();
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Modifies the input.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>The modified input.</returns>
        public override float Modify(float input)
        {
            float output = (input * this.a0) + this.z1;
            this.z1 = (input * this.a1) + this.z2 - (this.b1 * output);
            this.z2 = (input * this.a2) - (this.b2 * output);
            return output;
        }

        private void CalcBiquad()
        {
            float norm;
            float v = (float)Math.Pow(10, Math.Abs(this.PeakGain) / 20.0f);
            float k = (float)Math.Tan(Math.PI * this.Frequency);
            switch (this.ModifierType)
            {
                case ModifierType.LowPass:
                    norm = 1 / (1 + (k / this.Q) + (k * k));
                    this.a0 = k * k * norm;
                    this.a1 = 2 * this.a0;
                    this.a2 = this.a0;
                    this.b1 = 2 * ((k * k) - 1) * norm;
                    this.b2 = (1 - (k / this.Q) + (k * k)) * norm;
                    break;

                case ModifierType.HighPass:
                    norm = 1 / (1 + (k / this.Q) + (k * k));
                    this.a0 = 1 * norm;
                    this.a1 = -2 * this.a0;
                    this.a2 = this.a0;
                    this.b1 = 2 * ((k * k) - 1) * norm;
                    this.b2 = (1 - (k / this.Q) + (k * k)) * norm;
                    break;

                case ModifierType.BandPass:
                    norm = 1 / (1 + (k / this.Q) + (k * k));
                    this.a0 = k / this.Q * norm;
                    this.a1 = 0;
                    this.a2 = -this.a0;
                    this.b1 = 2 * ((k * k) - 1) * norm;
                    this.b2 = (1 - (k / this.Q) + (k * k)) * norm;
                    break;

                case ModifierType.Notch:
                    norm = 1 / (1 + (k / this.Q) + (k * k));
                    this.a0 = (1 + (k * k)) * norm;
                    this.a1 = 2 * ((k * k) - 1) * norm;
                    this.a2 = this.a0;
                    this.b1 = this.a1;
                    this.b2 = (1 - (k / this.Q) + (k * k)) * norm;
                    break;

                case ModifierType.Peak:
                    if (this.PeakGain >= 0)
                    {    // boost
                        norm = 1 / (1 + ((1 / this.Q) * k) + (k * k));
                        this.a0 = (1 + ((v / this.Q) * k) + (k * k)) * norm;
                        this.a1 = 2 * ((k * k) - 1) * norm;
                        this.a2 = (1 - ((v / this.Q) * k) + (k * k)) * norm;
                        this.b1 = this.a1;
                        this.b2 = (1 - ((1 / this.Q) * k) + (k * k)) * norm;
                    }
                    else
                    {    // cut
                        norm = 1 / (1 + ((v / this.Q) * k) + (k * k));
                        this.a0 = (1 + ((1 / this.Q) * k) + (k * k)) * norm;
                        this.a1 = 2 * ((k * k) - 1) * norm;
                        this.a2 = (1 - 1 / this.Q * k + k * k) * norm;
                        this.b1 = this.a1;
                        this.b2 = (1 - v / this.Q * k + k * k) * norm;
                    }

                    break;
                case ModifierType.LowShelf:
                    if (this.PeakGain >= 0)
                    {    // boost
                        norm = 1 / (float)(1 + (Math.Sqrt(2) * k) + (k * k));
                        this.a0 = (float)(1 + (Math.Sqrt(2 * v) * k) + ((v * k) * k)) * norm;
                        this.a1 = 2 * (((v * k) * k) - 1) * norm;
                        this.a2 = (float)(1 - (Math.Sqrt(2 * v) * k) + ((v * k) * k)) * norm;
                        this.b1 = 2 * ((k * k) - 1) * norm;
                        this.b2 = (float)(1 - (Math.Sqrt(2) * k) + (k * k)) * norm;
                    }
                    else
                    {    // cut
                        norm = 1 / (float)(1 + Math.Sqrt(2 * v) * k + v * k * k);
                        this.a0 = (float)(1 + Math.Sqrt(2) * k + k * k) * norm;
                        this.a1 = 2 * (k * k - 1) * norm;
                        this.a2 = (float)(1 - Math.Sqrt(2) * k + k * k) * norm;
                        this.b1 = 2 * (v * k * k - 1) * norm;
                        this.b2 = (float)(1 - Math.Sqrt(2 * v) * k + v * k * k) * norm;
                    }

                    break;
                case ModifierType.HighShelf:
                    if (this.PeakGain >= 0)
                    {    // boost
                        norm = 1 / (float)(1 + (Math.Sqrt(2) * k) + (k * k));
                        this.a0 = (float)(v + (Math.Sqrt(2 * v) * k) + (k * k)) * norm;
                        this.a1 = 2 * ((k * k) - v) * norm;
                        this.a2 = (float)(v - (Math.Sqrt(2 * v) * k) + (k * k)) * norm;
                        this.b1 = 2 * ((k * k) - 1) * norm;
                        this.b2 = (float)(1 - (Math.Sqrt(2) * k) + (k * k)) * norm;
                    }
                    else
                    {    // cut
                        norm = 1 / (float)(v + Math.Sqrt(2 * v) * k + k * k);
                        this.a0 = (float)(1 + Math.Sqrt(2) * k + k * k) * norm;
                        this.a1 = 2 * (k * k - 1) * norm;
                        this.a2 = (float)(1 - Math.Sqrt(2) * k + k * k) * norm;
                        this.b1 = 2 * (k * k - v) * norm;
                        this.b2 = (float)(v - Math.Sqrt(2 * v) * k + k * k) * norm;
                    }

                    break;
                default:
                    break;
            }

            return;
        }
        #endregion
    }

    /// <remarks>
    /// This filter is fairly efficient. However, it has to recalculate the 
    /// coefficients any time a parameter is changed. This is costly. Use this filter 
    /// if you do not need to change the parameters at the audio rate
    /// </remarks>
}