//-----------------------------------------------------------------------
// <copyright file="TSine.cs" company="Aaron Anderson">
//     Copyright (c) Aaron Anderson. All rights reserved.
// </copyright>
// <license type="MIT">
// See LICENSE.md in the project root for full license information.  
// </license>
// <summary>This is the TSine class.</summary>
//-----------------------------------------------------------------------
namespace ATKSharp.Generators.Oscillators.Trivial
{
    using System;

    /// <summary>
    /// The TSine class.
    /// Contains the functionality to generate a trivial sine oscillator.
    /// </summary>
    public class TSine : BaseGenerator
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="TSine"/> class.
        /// </summary>
        /// <param name="initialFrequency">The initial frequency.</param>
        /// <param name="initialAmplitude">The initial amplitude.</param>
        /// <param name="initialPhase">The initial phase.</param>
        public TSine(float initialFrequency = 440f, float initialAmplitude = 1f, double initialPhase = 0) : base(initialFrequency, initialAmplitude, initialPhase)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the increment.
        /// </summary>
        public override double Increment
        {
            get
            {
                return (Math.PI * 2 * this.Frequency) / ATKSettings.SampleRate;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Generate the next sinusoid sample. 
        /// This is the most precise option. However, the
        /// wavetable variety is not audibly unclean and is
        /// significantly more CPU efficient 
        /// </summary>
        /// <returns>The signal.</returns>
        public override float Generate()
        {
            this.CurrentSample = (float)(Math.Sin(this.Phase) * this.Amplitude);
            this.Phase += this.Increment; // TODO: is this supposed to come after the sample is calculated?
            return this.CurrentSample;
        }
        #endregion
    }

    /// <remarks> 
    /// The trivial oscillators generate the shape of the waveform very well. 
    /// However, due to the strong pressence over overtones the undesired aliasing 
    /// on these oscillators is strong. These are best suited for control signals 
    /// such as LFOs.
    /// </remarks>
}