//-----------------------------------------------------------------------
// <copyright file="TPhasor.cs" company="Aaron Anderson">
//     Copyright (c) Aaron Anderson. All rights reserved.
// </copyright>
// <license type="MIT">
// See LICENSE.md in the project root for full license information.  
// </license>
// <summary>This is the TPhasor class.</summary>
//-----------------------------------------------------------------------
namespace ATKSharp.Generators.Oscillators.Trivial
{
    /// <summary>
    /// The TPhasor class.
    /// Contains the functionality to generate a trivial phasor oscillator.
    /// </summary>
    public class TPhasor : TSawtooth
    {
        #region Constrcutors
        /// <summary>
        /// Initializes a new instance of the <see cref="TPhasor"/> class.
        /// </summary>
        /// <param name="initialFrequency">The initial frequency.</param>
        /// <param name="initialAmplitude">The initial amplitude.</param>
        public TPhasor(float initialFrequency = 440f, float initialAmplitude = 1f) : base(initialFrequency, initialAmplitude)
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Generates a signal.
        /// </summary>
        /// <returns>The signal.</returns>
        public override float Generate()
        {
            this.Phase += this.Increment;
            while (this.Phase > 1.0)
            { // use while in case frequency is insanely high (above SR)
                this.Phase -= 1.0;
            }

            while (this.Phase < 0.0)
            { // to ensure that negative frequencies will work
                this.Phase += 1.0;
            }

            this.CurrentSample = (float)(this.Phase * this.Amplitude);
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