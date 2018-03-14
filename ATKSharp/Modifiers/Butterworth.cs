//-----------------------------------------------------------------------
// <copyright file="Butterworth.cs" company="Aaron Anderson">
//     Copyright (c) Aaron Anderson. All rights reserved.
// </copyright>
// <license type="MIT">
// See LICENSE.md in the project root for full license information.  
// </license>
// <summary>This is the Butterworth class.</summary>
//-----------------------------------------------------------------------
namespace ATKSharp.Modifiers
{
    using System;

    /// <summary>
    /// The Butterworth class. This filter has a nice frequency response.
    /// It is a bit more tempermental than other filters; use with caution. 
    /// </summary>
    public class Butterworth : BaseModifier
    {
        #region Fields
        private float frequency;
        private float bandwidth;
        private float prevPrevIn, prevIn, prevPrevOut, prevOut;
        private float lambda, phi, lambdaSquared;
        private float a0, a1, a2, b1, b2; // coefficients 
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Butterworth"/> class.
        /// </summary>
        /// <param name="initFreq">The initial frequency (Hz).</param>
        public Butterworth(float initFreq = 1000f)
        {
            this.ModifierType = ModifierType.HighPass;
            this.prevIn = this.prevPrevIn = 0.0f;
            this.prevOut = this.prevPrevOut = 0.0f;
            this.Bandwidth = 120f;
            this.Frequency = initFreq;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the frequency.
        /// </summary>
        public override float Frequency
        {
            get
            {
                return this.frequency;
            }

            set
            {
                this.frequency = value;
                this.CalculateCoeff();
            }
        }

        /// <summary>
        /// Gets or sets the bandwidth.
        /// </summary>
        public float Bandwidth
        {
            get
            {
                return this.bandwidth;
            }

            set
            {
                this.bandwidth = value;
                if (this.ModifierType == ModifierType.BandPass || this.ModifierType == ModifierType.BandReject)
                {
                    this.CalculateCoeff();
                }
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
            this.CurrentSample = this.CalculateOutput(input);
            this.TransferValues(input);
            return this.CurrentSample;
        }

        // have to do this a lot, so let's just make it a function
        private void TransferValues(float input)
        {
            this.prevPrevOut = this.prevOut;
            this.prevOut = this.CurrentSample;
            this.prevPrevIn = this.prevIn;
            this.prevIn = input;
        }

        private float CalculateOutput(float input)
        {
            return (this.a0 * input) + (this.a1 * this.prevIn) + (this.a2 * this.prevPrevIn) -
            (this.b1 * this.prevOut) - (this.b2 * this.prevPrevOut);
        }

        private void CalculateCoeff()
        {
            switch (this.ModifierType)
            {
                case ModifierType.LowPass:
                    this.lambda = (float)(1 / Math.Tan((Math.PI * this.Frequency) / ATKSettings.SampleRate));
                    this.lambdaSquared = this.lambda * this.lambda;
                    
                    // phi not needed in this mode
                    this.a0 = 1 / (1 + (2 * this.lambda) + this.lambdaSquared);
                    this.a1 = this.a0 * 2;
                    this.a2 = this.a0;
                    this.b1 = (2 * this.a0) * (1 - this.lambdaSquared);
                    this.b2 = this.a0 * (1 - (2 * this.lambda) + this.lambdaSquared); // oop?
                    break;
                case ModifierType.HighPass:
                    this.lambda = (float)Math.Tan((Math.PI * this.Frequency) / ATKSettings.SampleRate);
                    this.lambdaSquared = this.lambda * this.lambda;
                    
                    // phi not needed in this mode
                    this.a0 = 1 / (1 + (2 * this.lambda) + this.lambdaSquared);
                    this.a1 = this.a0 * 2;
                    this.a2 = this.a0;
                    this.b1 = (2 * this.a0) * (this.lambdaSquared - 1);
                    this.b2 = this.a0 * (1 - (2 * this.lambda) + this.lambdaSquared); // oop?
                    break;
                case ModifierType.BandPass:
                    this.lambda = (float)(1 / Math.Tan((Math.PI * this.Bandwidth) / ATKSettings.SampleRate));
                    this.lambdaSquared = this.lambda * this.lambda;
                    this.phi = (float)(2 * Math.Cos((Math.PI * 2.0f * this.Frequency) / ATKSettings.SampleRate));
                    this.a0 = 1 / (1.0f + this.lambda);
                    this.a1 = 0;
                    this.a2 = this.a1 * -1.0f;
                    this.b1 = -1.0f * this.lambda * this.phi * this.a0;
                    this.b2 = this.a0 * (this.lambda - 1);
                    break;
                case ModifierType.BandReject:
                    this.lambda = (float)Math.Tan((Math.PI * this.Bandwidth) / ATKSettings.SampleRate);
                    this.lambdaSquared = this.lambda * this.lambda;
                    this.phi = (float)(2 * Math.Cos((Math.PI * 2.0f * this.Frequency) / ATKSettings.SampleRate));
                    this.a0 = 1 / (1.0f + this.lambda);
                    this.a1 = -1.0f * this.phi * this.a0;
                    this.a2 = this.a0;
                    this.b1 = -1.0f * this.lambda * this.phi * this.a0;
                    this.b2 = this.a0 * (this.lambda - 1.0f);
                    break;
                default:
                    break;
            }
        }
        #endregion
    }
}