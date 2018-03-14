//-----------------------------------------------------------------------
// <copyright file="LowPassComb.cs" company="Aaron Anderson">
//     Copyright (c) Aaron Anderson. All rights reserved.
// </copyright>
// <license type="MIT">
// See LICENSE.md in the project root for full license information.  
// </license>
// <summary>This is the LowPassComb class.</summary>
//-----------------------------------------------------------------------
namespace ATKSharp.Modifiers
{
    // We will need a modified version of the standard recursive comb filter to include a low pass filter for reverb

    /// <summary>
    /// The LowPassComb class.
    /// </summary>
    public class LowPassComb : Comb
    {
        #region Fields
        private LowPass lpf;
        private float frequency;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="LowPassComb"/> class.
        /// </summary>
        /// <param name="initMaxDelay">The initial max delay.</param>
        /// <param name="initDelay">The initial delay.</param>
        /// <param name="initFeedBack">The initial feedback.</param>
        /// <param name="cutOffFreq">The initial cutoff frequency.</param>
        public LowPassComb(float initMaxDelay = 1000, float initDelay = 1, float initFeedBack = 0.9f, float cutOffFreq = 1000) : base(initMaxDelay, initDelay, initFeedBack)
        {
            this.Frequency = cutOffFreq;
            this.lpf = new LowPass(this.Frequency);
        }
        #endregion

        #region Properties
        /// <summary>
        /// get or set the cutoff frequency of the internal Lowpass filter
        /// </summary>
        public override float Frequency
        {
            get
            {
                return this.frequency;
            }

            set
            {
                this.frequency = value;
                if (this.lpf != null)
                {
                    this.lpf.Frequency = value;
                }
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Modifies the input.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>The modified input.</returns>
        public override float Modify(float input)
        {
            this.DelayLineAccess.Generate();
            float delay = this.DelayLineAccess.CurrentSample;
            float filteredDelay = this.lpf.Modify(delay);
            this.DelayLine.Feed(input + (filteredDelay * this.Feedback));
            this.CurrentSample = input + delay;
            return this.CurrentSample;
        }
        #endregion
    }

    /// <remarks>
    /// This variation of the comb filter was added to use in reverb algorithms.
    /// The difference here is a Lowpass filter in the feedback loop. This is a 
    /// very efficient (perhaps not compelling) simulation of sound reflecting around
    /// a room.
    /// </remarks>
}