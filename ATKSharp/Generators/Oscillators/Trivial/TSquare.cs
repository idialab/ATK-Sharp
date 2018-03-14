//-----------------------------------------------------------------------
// <copyright file="TSquare.cs" company="Aaron Anderson">
//     Copyright (c) Aaron Anderson. All rights reserved.
// </copyright>
// <license type="MIT">
// See LICENSE.md in the project root for full license information.  
// </license>
// <summary>This is the TSquare class.</summary>
//-----------------------------------------------------------------------
namespace ATKSharp.Generators.Oscillators.Trivial
{
    /// <summary>
    /// The TSquare class.
    /// Contains the functionality to generate a trivial square oscillator.
    /// </summary>
    public class TSquare : BaseGenerator
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="TSquare"/> class.
        /// </summary>
        /// <param name="initialFrequency">The initial frequency.</param>
        /// <param name="initialAmplitude">The initial amplitude.</param>
        /// <param name="initialPhase">The initial phase.</param>
        public TSquare(float initialFrequency = 440f, float initialAmplitude = 1f, double initialPhase = 0) : base(initialFrequency, initialAmplitude, initialPhase)
        {
            this.DutyCycle = 0.5f;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the duty cycle.
        /// </summary>
        public float DutyCycle
        {
            get; private set;
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
            {
                this.Phase -= 1;
            }

            while (this.Phase < 0)
            {
                this.Phase += 1.0;
            }

            if (this.Phase > this.DutyCycle)
            {
                this.CurrentSample = 1.0f * this.Amplitude;
            }
            else
            {
                this.CurrentSample = -1.0f * this.Amplitude;
            }

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