//-----------------------------------------------------------------------
// <copyright file="WTTriangle.cs" company="Aaron Anderson">
//     Copyright (c) Aaron Anderson. All rights reserved.
// </copyright>
// <license type="MIT">
// See LICENSE.md in the project root for full license information.  
// </license>
// <summary>This is the WTTriangle class.</summary>
//-----------------------------------------------------------------------
namespace ATKSharp.Generators.Oscillators.Wavetable
{
    using System;
    using ATKSharp.Utilities;

    /// <summary>
    /// The WTTriangle class.
    /// Contains the functionality to generate a wavetable triangle oscillator.
    /// </summary>
    public class WTTriangle : BaseGenerator
    {
        #region Fields
        private float lowOct, highOct;
        private int index;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="WTTriangle"/> class.
        /// </summary>
        /// <param name="initialFrequency">The initial frequency.</param>
        /// <param name="initialAmplitude">The initial amplitude.</param>
        /// <param name="initialPhase">The initial phase.</param>
        public WTTriangle(float initialFrequency = 440f, float initialAmplitude = 1f, double initialPhase = 0) : base(initialFrequency, initialAmplitude, initialPhase)
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
                return this.Frequency / TriangleTable.Instance.TableFundamentalFreq;
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
            this.CurrentOctave = this.WhichOctave(TriangleTable.Instance.LowestFreqList, this.Frequency);
            this.lowOct = Interpolation.Linear(TriangleTable.Instance.Table[(int)this.CurrentOctave, (int)this.Phase], TriangleTable.Instance.Table[(int)this.CurrentOctave, (int)(this.Phase + 1)], (float)this.Phase);
            this.highOct = Interpolation.Linear(TriangleTable.Instance.Table[(int)Math.Min(this.CurrentOctave + 1, 9), (int)this.Phase], TriangleTable.Instance.Table[(int)Math.Min(this.CurrentOctave + 1, 9), (int)(this.Phase + 1)], (float)this.Phase);
            this.CurrentSample = Interpolation.Linear(this.lowOct, this.highOct, this.CurrentOctave);
            this.CurrentSample *= this.Amplitude;
            this.Phase += this.Increment;
            if (this.Phase >= TriangleTable.Instance.TableSize - 1)
            {
                this.Phase -= TriangleTable.Instance.TableSize - 1;
            }

            if (this.Phase <= 0)
            {
                this.Phase = TriangleTable.Instance.TableSize - 1;
            }

            return this.CurrentSample;
        }
        #endregion
    }
}