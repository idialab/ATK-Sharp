//-----------------------------------------------------------------------
// <copyright file="WTSine.cs" company="Aaron Anderson">
//     Copyright (c) Aaron Anderson. All rights reserved.
// </copyright>
// <license type="MIT">
// See LICENSE.md in the project root for full license information.  
// </license>
// <summary>This is the WTSine class.</summary>
//-----------------------------------------------------------------------
namespace ATKSharp.Generators.Oscillators.Wavetable
{
    using ATKSharp.Utilities;

    /// <summary>
    /// The WTSine class.
    /// Contains the functionality to generate a wavetable sine oscillator.
    /// </summary>
    public class WTSine : BaseGenerator
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="WTSine"/> class.
        /// </summary>
        /// <param name="initialFrequency">The initial frequency.</param>
        /// <param name="initialAmplitude">The initial amplitude.</param>
        /// <param name="initialPhase">The initial phase.</param>
        public WTSine(float initialFrequency = 440f, float initialAmplitude = 1f, double initialPhase = 0) : base(initialFrequency, initialAmplitude, initialPhase)
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
            this.CurrentSample = Interpolation.Linear(SineTable.Instance.Table[(int)this.Phase], SineTable.Instance.Table[(int)(this.Phase + 1) % SineTable.Instance.TableSize], (float)this.Phase);
            this.CurrentSample *= this.Amplitude;
            this.Phase += this.Increment;
            if (this.Phase >= SineTable.Instance.TableSize - 1)
            {
                this.Phase -= SineTable.Instance.TableSize - 1;
            }

            if (this.Phase <= 0)
            {
                this.Phase += SineTable.Instance.TableSize - 1;
            }

            return this.CurrentSample;
        }
        #endregion
    }
}