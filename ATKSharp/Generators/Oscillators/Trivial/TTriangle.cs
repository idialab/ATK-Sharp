//-----------------------------------------------------------------------
// <copyright file="TTriangle.cs" company="Aaron Anderson">
//     Copyright (c) Aaron Anderson. All rights reserved.
// </copyright>
// <license type="MIT">
// See LICENSE.md in the project root for full license information.  
// </license>
// <summary>This is the TTriangle class.</summary>
//-----------------------------------------------------------------------
namespace ATKSharp.Generators.Oscillators.Trivial
{
    using System;

    /// <summary>
    /// The TTriangle class.
    /// Contains the functionality to generate a trivial triangle oscillator.
    /// </summary>
    public class TTriangle : TSawtooth
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="TTriangle"/> class.
        /// </summary>
        /// <param name="initialFrequency">The initial frequency.</param>
        /// <param name="initialAmplitude">The initial amplitude.</param>
        public TTriangle(float initialFrequency = 440f, float initialAmplitude = 1f) : base(initialFrequency, initialAmplitude)
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Generate the next triangle sample.
        /// </summary>
        /// <returns>Triangle sample</returns>
        public float GetSample()
        { // TODO: Figure out what this is and document it
            float tri = (2 * Math.Abs(CurrentSample)) - 1; // manipulate sawtooth to triangle
            return tri * this.Amplitude;
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