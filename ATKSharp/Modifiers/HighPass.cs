//-----------------------------------------------------------------------
// <copyright file="HighPass.cs" company="Aaron Anderson">
//     Copyright (c) Aaron Anderson. All rights reserved.
// </copyright>
// <license type="MIT">
// See LICENSE.md in the project root for full license information.  
// </license>
// <summary>This is the HighPass class.</summary>
//-----------------------------------------------------------------------
namespace ATKSharp.Modifiers
{
    using System;

    /// <summary>
    /// The HighPass class.
    /// </summary>
    public class HighPass : BaseModifier
    {
        #region Fields
        private float frequency;
        private double coef, cosTheta;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="HighPass"/> class.
        /// </summary>
        /// <param name="initFreq">The initial frequency.</param>
        public HighPass(float initFreq = 1000f)
        {
            this.Frequency = initFreq;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Get or set the cut-off frequency
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
                this.cosTheta = 2.0 - Math.Cos((Math.PI * 2.0f * value) / ATKSettings.SampleRate);
                this.coef = this.cosTheta - Math.Sqrt((this.cosTheta * this.cosTheta) - 1.0);
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
            this.CurrentSample = (float)((input * (1.0f - this.coef)) - (this.CurrentSample * this.coef));
            return this.CurrentSample;
        }
        #endregion
    }
}