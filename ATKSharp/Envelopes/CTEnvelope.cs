//-----------------------------------------------------------------------
// <copyright file="CTEnvelope.cs" company="Aaron Anderson">
//     Copyright (c) Aaron Anderson. All rights reserved.
// </copyright>
// <license type="MIT">
// See LICENSE.md in the project root for full license information.  
// </license>
// <summary>This is the CTEnvelope class.</summary>
//-----------------------------------------------------------------------
namespace ATKSharp.Envelopes
{
    using ATKSharp.Extensions;

    /// <summary>
    /// The Constant Time envelope class. This is the simplist and most efficient envelope. 
    /// This comes with the tradeoff of not sounding as natural as the CREnvelope. Use this
    /// envelope if any of the envelope parameters need to be modified at the sample rate
    /// </summary>
    public class CTEnvelope : BaseEnvelope
    {
        #region Fields
        private double attackTime;
        private double decayTime;
        private float sustainLevel;
        private double releaseTime;
        private float attackInc, decayInc, releaseInc; // Increments and decrements for linear progression.
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="CTEnvelope" /> class.
        /// </summary>
        /// <param name="attackTime">The envelope attack time (ms).</param>
        /// <param name="decayTime">The envelope decay time (ms).</param>
        /// <param name="sustainLevel">The envelope sustain level (0-1).</param>
        /// <param name="releaseTime">The envelope release time (ms).</param>
        public CTEnvelope(double attackTime = 100, double decayTime = 20, float sustainLevel = .7f, double releaseTime = 150) : base(EnvelopeType.ADSR, attackTime, decayTime, sustainLevel, releaseTime)
        {
            this.Legato = false;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the attack time.
        /// </summary>
        public override double AttackTime
        {
            get
            {
                return this.attackTime;
            }

            set
            {
                this.attackTime = value;
                this.attackInc = (float)(1 / (ATKSettings.SampleRate * (this.attackTime * 0.001)));
            }
        }

        /// <summary>
        /// Gets or sets the decay time.
        /// </summary>
        public override double DecayTime
        {
            get
            {
                return this.decayTime;
            }

            set
            {
                this.decayTime = value;
                this.decayInc = (float)((1f - this.SustainLevel) / (ATKSettings.SampleRate * (this.decayTime * 0.001)));
            }
        }

        /// <summary>
        /// Gets or sets the sustain level.
        /// </summary>
        public override float SustainLevel
        {
            get
            {
                return this.SustainLevel;
            }

            set
            {
                this.SustainLevel = value.Clamp(0, 1);
                this.decayInc = (float)((1f - this.SustainLevel) / (ATKSettings.SampleRate * (this.decayTime * 0.001)));
                this.releaseInc = (float)(this.SustainLevel / (ATKSettings.SampleRate * (this.releaseTime * 0.001)));
            }
        }

        /// <summary>
        /// Gets or sets the release time.
        /// </summary>
        public override double ReleaseTime
        {
            get
            {
                return this.releaseTime;
            }

            set
            {
                this.releaseTime = value;
                this.releaseInc = (float)(this.SustainLevel / (ATKSettings.SampleRate * (this.releaseTime * 0.001)));
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
            switch (this.Type)
            {
                case EnvelopeType.ADSR: // Default type.
                    switch (this.State)
                    {
                        case EnvelopeState.ATTACK:
                            this.CurrentSample += this.attackInc;
                            if (this.CurrentSample >= 1.0f)
                            {
                                this.CurrentSample = 1.0f;
                                this.State = EnvelopeState.DECAY;
                            }

                            break;
                        case EnvelopeState.DECAY:
                            this.CurrentSample -= this.decayInc;
                            if (this.CurrentSample <= this.SustainLevel)
                            {
                                this.CurrentSample = this.SustainLevel;
                                this.State = EnvelopeState.SUSTAIN;
                            }

                            break;
                        case EnvelopeState.SUSTAIN:
                            // No need to do anything here.
                            break;
                        case EnvelopeState.RELEASE:
                            this.CurrentSample -= this.releaseInc;
                            if (this.CurrentSample <= 0.0)
                            {
                                this.CurrentSample = 0;
                                this.State = EnvelopeState.OFF;
                            }

                            break;
                        case EnvelopeState.OFF:
                            break;
                        default: break;
                    }

                    break;
                case EnvelopeType.AR:
                    switch (this.State)
                    {
                        case EnvelopeState.ATTACK:
                            this.CurrentSample += this.attackInc;
                            if (this.CurrentSample >= 1.0f)
                            {
                                this.CurrentSample = 1.0f;
                                this.State = EnvelopeState.DECAY;
                            }

                            break;
                        case EnvelopeState.DECAY:
                            this.CurrentSample -= this.decayInc;
                            if (this.CurrentSample <= 0.0f)
                            {
                                this.CurrentSample = 0.0f;
                                this.State = EnvelopeState.OFF;
                            }

                            break;
                        default: break;
                    }

                    break;
                default: break;
            }

            return this.CurrentSample;
        }
        #endregion
    }

    // TODO modify is an option but doesn't do anything yet.
}
