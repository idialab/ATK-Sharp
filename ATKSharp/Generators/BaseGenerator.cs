//-----------------------------------------------------------------------
// <copyright file="BaseGenerator.cs" company="Aaron Anderson">
//     Copyright (c) Aaron Anderson. All rights reserved.
// </copyright>
// <license type="MIT">
// See LICENSE.md in the project root for full license information.  
// </license>
// <summary>This is the BaseGenerator class.</summary>
//-----------------------------------------------------------------------
namespace ATKSharp.Generators
{
    using ATKSharp.Extensions;

    /// <summary>
    /// The BaseOscillator class.
    /// Contains the base functionality for oscillators.
    /// </summary>
    public abstract class BaseGenerator : IGenerator
    {
        #region Fields
        private float frequency;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseGenerator"/> class.
        /// </summary>
        /// <param name="initialFrequency">The initial frequency (Hz).</param>
        /// <param name="initialAmplitude">The initial amplitude (0-1).</param>
        /// <param name="initialPhase">The initial phase (radians).</param>
        protected BaseGenerator(float initialFrequency = 440f, float initialAmplitude = 1f, double initialPhase = 0)
        {
            this.Frequency = initialFrequency;
            this.Amplitude = initialAmplitude;
            this.Phase = initialPhase;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the oscillator frequency. 
        /// </summary>
        public virtual float Frequency
        {
            get
            {
                return this.frequency;
            }

            set
            {
                this.frequency = value.Clamp(-ATKSettings.HalfSampleRate, ATKSettings.HalfSampleRate);
            }
        }

        /// <summary>
        /// Gets or sets the oscillator frequency.
        /// </summary>
        public virtual float Amplitude
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the oscillator phase.
        /// </summary>
        public virtual double Phase
        {
            get; set;
        }

        /// <summary>
        /// Gets the oscillator increment.
        /// </summary>
        public virtual double Increment
        {
            get
            {
                return this.Frequency / ATKSettings.SampleRate;
            }
        }

        /// <summary>
        /// Gets or sets the oscillator current sample.
        /// </summary>
        public virtual float CurrentSample
        {
            get; protected set;
        }

        /// <summary>
        /// Gets or sets the oscillator current octave.
        /// </summary>
        public virtual float CurrentOctave
        {
            get; protected set;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Returns 0. BaseGenerator should only ever be used if extending ATK
        /// </summary>
        /// <returns>The signal.</returns>
        public virtual float Generate()
        {
            return 0f; // base oscillator doesn't generate anything
        }

        /// <summary>
        /// Selects which ocatve.
        /// </summary>
        /// <param name="lowestFreqList">The lowest frequency list.</param>
        /// <param name="freq">The frequency.</param>
        /// <returns>Which octave.</returns>
        protected float WhichOctave(float[] lowestFreqList, float freq)
        { // what tables should be drawn from
            if (freq > lowestFreqList[9])
            {
                this.CurrentOctave = 9f;
                return 9.0f;
            }
            else if (freq <= lowestFreqList[0])
            {
                this.CurrentOctave = 0f;
                return 0.0f;
            }

            int index = 0;
            while (freq > lowestFreqList[index])
            {
                if (freq < lowestFreqList[index + 1])
                {
                    return index + ((freq / lowestFreqList[index]) - 1.0f); // return the position between tables.
                }
                else
                {
                    index++;
                }
            }

            return 0f;
        }
        #endregion
    }
}
