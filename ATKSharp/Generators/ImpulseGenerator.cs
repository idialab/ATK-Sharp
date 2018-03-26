//-----------------------------------------------------------------------
// <copyright file="ImpulseGenerator.cs" company="Aaron Anderson">
//     Copyright (c) Aaron Anderson. All rights reserved.
// </copyright>
// <license type="MIT">
// See LICENSE.md in the project root for full license information.  
// </license>
// <summary>This is the ImpulseGenerator class.</summary>
//-----------------------------------------------------------------------
namespace ATKSharp.Generators
{
    using System;

    /// <summary>
    /// The ImpulseGenerator class.
    /// Contains the functionality to generate an impulse.
    /// </summary>
    public class ImpulseGenerator : BaseGenerator
    {
        #region Fields
        private bool burstMasked;
        private float nextTarget; // 1 + pulseDeviation;
        private Random random;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ImpulseGenerator"/> class.
        /// </summary>
        /// <param name="initialFrequency">The initial frequency.</param>
        /// <param name="initialAmplitude">The initial amplitude.</param>
        /// <param name="initialPhase">The initial phase.</param>
        public ImpulseGenerator(float initialFrequency = 1f, float initialAmplitude = 1f, double initialPhase = 0) : base(initialFrequency, initialAmplitude, initialPhase)
        {
            this.random = new Random();
            this.nextTarget = (float)(1.0 + ((this.random.NextDouble() * this.PulseDeviation * 2) - this.PulseDeviation));
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the pulse deviation.
        /// </summary>
        public float PulseDeviation
        {
            get; set;
        } // 0.0 to 1.0

        /// <summary>
        /// Gets or sets the burst masking.
        /// </summary>
        public float BurstMasking
        {
            get; set;
        } // 0.0 to 1.0 inspired by Curtis Roads
        #endregion

        #region Methods
        /// <summary>
        /// Decide if the next sample to output will be 1 or 0
        /// </summary>
        /// <returns>The signal.</returns>
        public override float Generate()
        {
            this.Phase += this.Increment;
            if (this.Phase >= this.nextTarget)
            {
                this.CurrentSample = this.burstMasked ? 0 : 1;
                this.Phase -= this.nextTarget;
                this.nextTarget = (float)(1.0 + ((this.random.NextDouble() * this.PulseDeviation * 2) - this.PulseDeviation));
                this.burstMasked = (this.random.NextDouble() < this.BurstMasking) ? true : false;
            }
            else if (this.Phase >= 0.0)
            {
                this.CurrentSample = 0;
            }
            else if (this.Phase <= 0)
            { // better plan for negative frequencies
                this.CurrentSample = 1;
                this.Phase += this.nextTarget;
                this.nextTarget = (float)(1.0 + ((this.random.NextDouble() * this.PulseDeviation * 2) - this.PulseDeviation));
                this.burstMasked = (this.random.NextDouble() < this.BurstMasking) ? true : false;
            }

            return this.CurrentSample;
        }
        #endregion
    }

    /// <remarks>
    /// this is an impulse generator;
    /// it reports a "1" n times per second;
    /// otherwise, it returns a zero;
    /// Impulse generators are commonly used in granluar/particle
    /// synthesis as triggers for grains.
    /// This impulse generator features
    /// "Burst Masking" - a term coined by Curtis Roads
    /// in "Microsound". 
    /// The periodicity can be smeared by adjust the 
    /// "Pulse Deviation" parameter.If pulse deviation is set
    /// to 0, the impulses are emmitted periodically. if set to 1.0,
    /// the impulses are emmitted aperiodically while mainting the same
    /// average frequency
    /// </remarks>
}
