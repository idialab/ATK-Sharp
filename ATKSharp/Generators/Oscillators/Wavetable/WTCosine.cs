//-----------------------------------------------------------------------
// <copyright file="WTCosine.cs" company="Aaron Anderson">
//     Copyright (c) Aaron Anderson. All rights reserved.
// </copyright>
// <license type="MIT">
// See LICENSE.md in the project root for full license information.  
// </license>
// <summary>This is the WTCosine class.</summary>
//-----------------------------------------------------------------------
namespace ATKSharp.Generators.Oscillators.Wavetable
{
    using ATKSharp.Utilities;

    /// <summary>
    /// The WTCosine class.
    /// Identical to the wavetable sine oscillator but shifted by PI/2
    /// </summary>
    public class WTCosine : BaseGenerator
    {
        #region Fields
        private float quarterTableSize, adjustedPhase;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="WTCosine"/> class.
        /// </summary>
        /// <param name="initialFrequency">The initial frequency.</param>
        /// <param name="initialAmplitude">The initial amplitude.</param>
        /// <param name="initialPhase">The initial phase.</param>
        public WTCosine(float initialFrequency = 440f, float initialAmplitude = 1f, double initialPhase = 0) : base(initialFrequency, initialAmplitude, initialPhase)
        {
            this.quarterTableSize = SineTable.Instance.TableSize * 0.25f;
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
                return this.Frequency / (double)SineTable.Instance.TableFundamentalFreq;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Generates a signal.
        /// </summary>
        /// <returns>The signal.</returns>
        public override float Generate()
        {
            this.adjustedPhase = ((float)this.Phase + this.quarterTableSize) % (SineTable.Instance.TableSize - 1);
            this.CurrentSample = Interpolation.Linear(SineTable.Instance.Table[(int)this.adjustedPhase], SineTable.Instance.Table[(int)(this.adjustedPhase + 1)], (float)this.Phase);
            this.CurrentSample *= this.Amplitude;
            this.Phase += this.Increment;
            if (this.Phase >= SineTable.Instance.TableSize - 1)
            {
                this.Phase -= SineTable.Instance.TableSize;
            }

            if (this.Phase <= 0)
            {
                this.Phase += SineTable.Instance.TableSize;
            }

            return this.CurrentSample;
        }
        #endregion
    }
}