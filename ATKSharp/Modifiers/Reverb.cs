//-----------------------------------------------------------------------
// <copyright file="Reverb.cs" company="Aaron Anderson">
//     Copyright (c) Aaron Anderson. All rights reserved.
// </copyright>
// <license type="MIT">
// See LICENSE.md in the project root for full license information.  
// </license>
// <summary>This is the Reverb class.</summary>
//-----------------------------------------------------------------------
namespace ATKSharp.Modifiers
{
    using ATKSharp.Extensions;
    using ATKSharp.Utilities;

    /// <summary>
    /// The Reverb class.
    /// Contains the functionality to add reverb to an audio signal.
    /// </summary>
    public class Reverb : BaseModifier
    {
        #region Fields
        private const int NumEarlyReflections = 16;
        private float feedBack;
        private float frequency;
        private float dryWetMix;
        private float preDelay;
        private TapOut[] earlyReflections = new TapOut[16];
        private float[] reflectionTimeList = { 4.3f, 21.5f, 22.5f, 26.8f, 27.0f, 29.8f, 45.8f, 48.8f, 57.2f, 58.7f, 59.5f, 61.2f, 70.7f, 70.8f, 72.6f, 74.1f };
        private float[] reflectionAmpList = { 0.841f, 0.504f, 0.491f, 0.379f, 0.38f, 0.346f, 0.289f, 0.272f, 0.192f, 0.193f, 0.217f, 0.181f, 0.18f, 0.181f, 0.176f, 0.142f };
        private LowPassComb[] combFilters = new LowPassComb[4]; // should be four of these
        private float[] combDelayTimes = { 29.7f, 37.1f, 41.1f, 43.7f };
        private AllPass[] allPassFilters = new AllPass[2];
        private float[] allPassDelayTimes = { 5.0f, 1.7f };
        private TapIn delayLine;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Reverb"/> class.
        /// </summary>
        /// <param name="absorption">The initial absorption.</param>
        /// <param name="absorptionFrequency">The initial absorption frequency.</param>
        /// <param name="dryWetMix">The initial dry/wet mix.</param>
        /// <param name="preDelay">The initial pre-delay.</param>
        public Reverb(float absorption = 0.03f, float absorptionFrequency = 5000f, float dryWetMix = 0.7f, float preDelay = 40f)
        {
            this.delayLine = new TapIn(4000f);
            this.FeedBack = 1.0f - absorption.Clamp(0.0f, 1.0f);
            this.Frequency = absorptionFrequency;
            this.DryWetMix = 1.0f - dryWetMix.Clamp(0.0f, 1.0f);
            this.PreDelay = preDelay;
            for (int i = 0; i < this.earlyReflections.Length; i++)
            {
                this.earlyReflections[i] = new TapOut(this.delayLine, this.reflectionTimeList[i] + this.PreDelay); // Early reflections is simply a multi-tap delay
            }

            for (int i = 0; i < this.combFilters.Length; i++)
            {
                this.combFilters[i] = new LowPassComb(1000f, this.combDelayTimes[i] + this.PreDelay, this.FeedBack, this.Frequency); // this helps transition from early reflection to fused reverb
            }

            for (int i = 0; i < this.allPassFilters.Length; i++)
            {
                this.allPassFilters[i] = new AllPass(1000f, this.allPassDelayTimes[i] + this.PreDelay, 0.7f); // This is essentially for transient smearing
            }
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the feedback.
        /// </summary>
        public float FeedBack
        {
            get
            {
                return this.feedBack;
            }

            set
            {
                this.feedBack = value.Clamp(0.0f, 1.0f);
            }
        }

        /// <summary>
        /// Gets or sets the frequency.
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
                for (int i = 0; i < this.combFilters.Length; i++)
                {
                    if (this.combFilters[i] != null)
                    {
                        this.combFilters[i].Frequency = this.frequency;
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the dry/wet mix (0-1).
        /// </summary>
        public float DryWetMix
        {
            get
            {
                return this.dryWetMix;
            }

            set
            {
                this.dryWetMix = value;
            }
        }

        /// <summary>
        /// Gets or sets the pre-delay (ms).
        /// </summary>
        public float PreDelay
        {
            get
            {
                return this.preDelay;
            }

            set
            {
                this.preDelay = value;
                for (int i = 0; i < NumEarlyReflections; i++)
                {
                    if (this.earlyReflections[i] != null)
                    {
                        this.earlyReflections[i].DelayMilliseconds = this.reflectionTimeList[i] + value;
                    }
                }

                for (int i = 0; i < 4; i++)
                {
                    if (this.combFilters[i] != null)
                    {
                        this.combFilters[i].DelayMilliseconds = this.combDelayTimes[i] + value;
                    }
                }

                for (int i = 0; i < 2; i++)
                {
                    if (this.allPassFilters[i] != null)
                    {
                        this.allPassFilters[i].DelayMilliseconds = this.allPassDelayTimes[i] + value;
                    }
                }
            }
        }        
        #endregion

        #region Methods
        /// <summary>
        /// Add reverberation to an input signal.
        /// </summary>
        /// <param name="input">The input signal.</param>
        /// <returns>Reverberated sample</returns>
        public override float Modify(float input)
        {
            this.delayLine.Feed(input);
            float reflectionAccumulation = 0; // add the early reflections
            for (int i = 0; i < this.earlyReflections.Length; i++)
            {
                this.earlyReflections[i].Generate();
                reflectionAccumulation += this.earlyReflections[i].CurrentSample * this.reflectionAmpList[i];
            }

            float combAccumulation = 0; // use the 4 comb filters in paralell
            for (int i = 0; i < this.combFilters.Length; i++)
            {
                combAccumulation += this.combFilters[i].Modify(input);
            }

            for (int i = 0; i < this.allPassFilters.Length; i++) 
            {
                combAccumulation = this.allPassFilters[i].Modify(combAccumulation); // use the 2 allpass filters in series
            }
            
            float wet = reflectionAccumulation + combAccumulation; // add the parts together
            this.CurrentSample = (wet * this.DryWetMix) + (input * (1 - this.DryWetMix));
            return this.CurrentSample; // note that combAccumulation also includes the allpass Generate(
        }
        #endregion 
    } // end of class

    /// <remarks>
    /// a very simple shroeder-style digital reverb.
    /// I may add more sophisticated reverbs in the future
    /// in addition to improving this to the Moore style
    /// This reverb only has 3 ingredients: comb filters, allpass filters, and a delay line.
    /// </remarks>
} // end of namespace
