//-----------------------------------------------------------------------
// <copyright file="LowPass.cs" company="Aaron Anderson">
//     Copyright (c) Aaron Anderson. All rights reserved.
// </copyright>
// <license type="MIT">
// See LICENSE.md in the project root for full license information.  
// </license>
// <summary>This is the LowPass class.</summary>
//-----------------------------------------------------------------------
namespace ATKSharp.Modifiers
{
    using System;

    /// <summary>
    /// The LowPass class.
    /// </summary>
    /// <remarks>
    /// Simplest low pass filter possible. 1 pole IIR.
    /// </remarks>
    public class LowPass : BaseModifier
    {
        #region Fields
        private float frequency;
        private double coef, cosTheta;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="LowPass"/> class.
        /// </summary>
        /// <param name="initFreq">The initial frequency (Hz).</param>
        public LowPass(float initFreq = 1000f)
        {
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
                this.cosTheta = 2.0f - Math.Cos((Math.PI * 2.0f * value) / ATKSettings.SampleRate);
                this.coef = Math.Sqrt((this.cosTheta * this.cosTheta) - 1.0f) - this.cosTheta;
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
            this.CurrentSample = (float)((input * (1.0f + this.coef)) - (this.CurrentSample * this.coef));
            return this.CurrentSample;
        }
        #endregion
    }
}
