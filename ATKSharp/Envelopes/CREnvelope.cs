//-----------------------------------------------------------------------
// <copyright file="CREnvelope.cs" company="Aaron Anderson">
//     Copyright (c) Aaron Anderson. All rights reserved.
// </copyright>
// <license type="MIT">
// See LICENSE.md in the project root for full license information.  
// </license>
// <summary>This is the CREnvelope class.</summary>
//-----------------------------------------------------------------------
namespace ATKSharp.Envelopes
{
    using System;
    using System.Diagnostics;

    /// <summary>
    /// The CREnvelope class.
    /// </summary>
    public class CREnvelope : BaseEnvelope
    {
        #region Fields
        private double attackTime; // In ms. 
        private double decayTime;
        private double releaseTime;
        private ModeType mode;
        private double attackOffset, attackCoef, attackTCO;
        private double decayOffset, decayCoef, decayTCO;
        private double releaseOffset, releaseCoef, releaseTCO;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="CREnvelope" /> class.
        /// </summary>
        /// <param name="envelopeType">The envelope type.</param>
        public CREnvelope(EnvelopeType envelopeType = EnvelopeType.ADSR) : base(envelopeType)
        {
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
                this.CalcCoef(EnvelopeState.ATTACK, (float)this.AttackTime);
            }
        }

        /// <summary>
        /// Gets ot sets the decay time.
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
                this.CalcCoef(EnvelopeState.DECAY, (float)this.DecayTime);
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
                this.CalcCoef(EnvelopeState.RELEASE, (float)this.ReleaseTime);
            }
        }

        /// <summary>
        /// Gets or sets the mode type.
        /// </summary>
        /// <remarks>
        /// 'ANALOG' mode mimics the charging and discharging of a capacitor
        /// 'DIGITAL' mode is linear in decibels 
        /// </remarks>
        public override ModeType Mode
        {
            get
            {
                return this.mode;
            }

            protected set
            {
                this.mode = value;
                switch (value)
                {
                    case ModeType.ANALOG:
                        this.attackTCO = Math.Exp(-1.5);
                        this.decayTCO = Math.Exp(-4.95);
                        this.releaseTCO = this.decayTCO;
                        break;
                    case ModeType.DIGITAL:
                        this.attackTCO = 0.99999;
                        this.decayTCO = Math.Exp(-11.05); // Page 297
                        this.releaseTCO = this.decayTCO;
                        break;
                    default:
                        break;
                }
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Generate a signal.
        /// </summary>
        /// <returns>The signal.</returns>
        public override float Generate()
        {
            switch (this.Type)
            {
                case EnvelopeType.ADSR:

                    break;
                case EnvelopeType.AR:

                    break;
                case EnvelopeType.AD:

                    break;
                case EnvelopeType.ASR:

                    break;
                case EnvelopeType.AHDSR:

                    break;
                default:
                    Trace.TraceWarning("[ATK] No such envelope type: " + this.Type);
                    break;
            }

            switch (this.State)
            {
                case EnvelopeState.OFF:
                    this.CurrentSample = 0;
                    break;
                case EnvelopeState.ATTACK:
                    this.CurrentSample = (float)this.attackOffset + (this.CurrentSample * (float)this.attackCoef);

                    // Check if ready for next state
                    if (this.CurrentSample >= 1.0)
                    {
                        this.CurrentSample = 1.0f;
                        this.State = EnvelopeState.DECAY;
                    }

                    break;
                case EnvelopeState.DECAY:
                    this.CurrentSample = (float)this.decayOffset + (this.CurrentSample * (float)this.decayCoef);
                    if (this.CurrentSample <= this.SustainLevel)
                    {
                        this.CurrentSample = this.SustainLevel;
                        this.State = EnvelopeState.SUSTAIN;
                    }

                    break;
                case EnvelopeState.SUSTAIN:
                    this.CurrentSample = this.SustainLevel;
                    break;

                case EnvelopeState.RELEASE:
                    this.CurrentSample = (float)this.releaseOffset + (this.CurrentSample * (float)this.releaseCoef);
                    if (this.CurrentSample <= 0.0)
                    {
                        this.CurrentSample = 0;
                        this.State = EnvelopeState.OFF;
                    }

                    break;
            }

            return this.CurrentSample;
        }

        /// <summary>
        /// Sets the envelope attack time, decay time, sustain level, and release time.
        /// </summary>
        /// <param name="a">The attack time.</param>
        /// <param name="d">The decay time.</param>
        /// <param name="s">The sustain level.</param>
        /// <param name="r">The release time.</param>
        public void SetADSR(float a, float d, float s, float r)
        {
            this.AttackTime = a;
            this.DecayTime = d;
            this.SustainLevel = s;
            this.ReleaseTime = r;
        }
        
        /// <summary>
        /// Calculate the coefficient of a particular segment.
        /// </summary>
        /// <param name="state">The envelope state.</param>
        /// <param name="time">The envelope time.</param>
        private void CalcCoef(EnvelopeState state, float time)
        {
            double numSamples = ATKSettings.SampleRate * (time * 0.001); // It would have to be casted as double anyway
            switch (state)
            {
                case EnvelopeState.ATTACK:
                    this.attackCoef = Math.Exp(-Math.Log((1.0 + this.attackTCO) / this.attackTCO) / numSamples);
                    this.attackOffset = (1.0 + this.attackTCO) * (1.0 - this.attackCoef);
                    break;
                case EnvelopeState.DECAY:
                    this.decayCoef = Math.Exp(-Math.Log((1.0 + this.attackTCO) / this.attackTCO) / numSamples);
                    this.decayOffset = (this.SustainLevel - this.decayTCO) * (1.0 - this.decayCoef);
                    break;
                case EnvelopeState.RELEASE:
                    this.releaseCoef = Math.Exp(-Math.Log((1.0 + this.attackTCO) / this.attackTCO) / numSamples);
                    this.releaseOffset = -this.releaseTCO * (1.0 - this.releaseCoef);
                    break;
                default:
                    Trace.TraceWarning("[ATK] Coefficient not calculated. " + state + " is not a proper envelope state.");
                    break;
            }
        }
        #endregion
    }
}
