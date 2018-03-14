//-----------------------------------------------------------------------
// <copyright file="WTSquare.cs" company="Aaron Anderson">
//     Copyright (c) Aaron Anderson. All rights reserved.
// </copyright>
// <license type="MIT">
// See LICENSE.md in the project root for full license information.  
// </license>
// <summary>This is the WTSquare class.</summary>
//-----------------------------------------------------------------------
namespace ATKSharp.Generators.Oscillators.Wavetable
{
    using System;
    using ATKSharp.Utilities;

    /// <summary>
    /// The WTSquare class.
    /// Contains the functionality to generate a wavetable square oscillator.
    /// </summary>
    public class WTSquare : BaseGenerator
    {
        #region Fields
        private float lowOct, highOct;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="WTSquare"/> class.
        /// </summary>
        /// <param name="initialFrequency">The initial frequency.</param>
        /// <param name="initialAmplitude">The initial amplitude.</param>
        /// <param name="initialPhase">The initial phase.</param>
        public WTSquare(float initialFrequency = 440f, float initialAmplitude = 1f, double initialPhase = 0) : base(initialFrequency, initialAmplitude, initialPhase)
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
                return this.Frequency / SquareTable.Instance.TableFundamentalFreq;
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
            this.CurrentOctave = this.WhichOctave(SquareTable.Instance.LowestFreqList, this.Frequency);
            this.lowOct = Interpolation.Linear(SquareTable.Instance.Table[(int)this.CurrentOctave, (int)this.Phase], SquareTable.Instance.Table[(int)this.CurrentOctave, (int)(this.Phase + 1)], (float)this.Phase);
            this.highOct = Interpolation.Linear(SquareTable.Instance.Table[(int)Math.Min(this.CurrentOctave + 1, 9), (int)this.Phase], SquareTable.Instance.Table[(int)Math.Min(this.CurrentOctave + 1, 9), (int)(this.Phase + 1)], (float)this.Phase);
            this.CurrentSample = Interpolation.Linear(this.lowOct, this.highOct, this.CurrentOctave);
            this.CurrentSample *= this.Amplitude;
            this.Phase += this.Increment;
            if (this.Phase >= SquareTable.Instance.TableSize - 1)
            {
                this.Phase -= SquareTable.Instance.TableSize - 1;
            }

            if (this.Phase <= 0)
            {
                this.Phase = SquareTable.Instance.TableSize - 1;
            }

            return this.CurrentSample;
        }
        #endregion
    }
}