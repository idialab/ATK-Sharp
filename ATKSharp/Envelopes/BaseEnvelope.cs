//-----------------------------------------------------------------------
// <copyright file="BaseEnvelope.cs" company="Aaron Anderson">
//     Copyright (c) Aaron Anderson. All rights reserved.
// </copyright>
// <license type="MIT">
// See LICENSE.md in the project root for full license information.  
// </license>
// <summary>This is the BaseEnvelope class.</summary>
//-----------------------------------------------------------------------
namespace ATKSharp.Envelopes
{
    using ATKSharp.Generators;
    using ATKSharp.Modifiers;

    #region Enums
    /// <summary>
    /// The Mode Type.
    /// Defines the mode type of the envelope.
    /// </summary>
    public enum ModeType
    {
        /// <summary>
        /// The Analog mode type.
        /// </summary>
        ANALOG,

        /// <summary>
        /// The Digital mode type.
        /// </summary>
        DIGITAL,

        /// <summary>
        /// The Linear mode type.
        /// </summary>
        LINEAR
    }

    /// <summary>
    /// The Envelope Type.
    /// Defines the type of the envelope.
    /// </summary>
    public enum EnvelopeType
    {
        /// <summary>
        /// The Attack-Release type.
        /// </summary>
        AR,

        /// <summary>
        /// The Attack-Decay type.
        /// </summary>
        AD,

        /// <summary>
        /// The Attack-Sustain-Release type.
        /// </summary>
        ASR,

        /// <summary>
        /// The Attack-Decay-Sustain-Release type.
        /// </summary>
        ADSR,

        /// <summary>
        /// The Attack-Hold-Decay-Sustain-Release type.
        /// </summary>
        AHDSR
    }

    /// <summary>
    /// The Envelope State.
    /// Defines the state of the envelope.
    /// </summary>
    public enum EnvelopeState
    {
        /// <summary>
        /// The Off state.
        /// </summary>
        OFF,

        /// <summary>
        /// The Attack state.
        /// </summary>
        ATTACK,

        /// <summary>
        /// The Decay state.
        /// </summary>
        DECAY,

        /// <summary>
        /// The Sustain state.
        /// </summary>
        SUSTAIN,

        /// <summary>
        /// The Release state.
        /// </summary>
        RELEASE,

        /// <summary>
        /// The Shutdown state.
        /// </summary>
        SHUTDOWN // for non-legato mode
    }
    #endregion

    /// <summary>
    /// The BaseEnvelope class.
    /// Contains the shared base functionality for envelopes.
    /// </summary>
    public abstract class BaseEnvelope : IGenerator, IModifier
    {
        #region Fields
        private int gate;
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseEnvelope"/> class.
        /// </summary>
        /// <param name="envelopeType">The envelope type.</param>
        /// <param name="attackTime">The envelope attack time (ms).</param>
        /// <param name="decayTime">The envelope decay time (ms).</param>
        /// <param name="sustainLevel">The envelope sustain level (0-1).</param>
        /// <param name="releaseTime">The envelope release time (ms).</param>
        /// <param name="mode">The envelope mode.</param>
        protected BaseEnvelope(EnvelopeType envelopeType = EnvelopeType.ADSR, double attackTime = 200, double decayTime = 50, float sustainLevel = .01f, double releaseTime = 400, ModeType mode = ModeType.ANALOG)
        {
            this.Type = envelopeType;
            this.AttackTime = attackTime;
            this.DecayTime = decayTime;
            this.SustainLevel = sustainLevel;
            this.ReleaseTime = releaseTime;
            this.Mode = mode;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the envelope type.
        /// (AR, ADSR, etc)
        /// </summary>
        public virtual EnvelopeType Type { get; set; }

        /// <summary>
        /// Gets or sets the envelope attack time (ms).
        /// </summary>
        public virtual double AttackTime { get; set; }

        /// <summary>
        /// Gets or sets the envelope decay time (ms).
        /// </summary>
        public virtual double DecayTime { get; set; }

        /// <summary>
        /// Gets or sets the envelope sustain level (0 to 1).
        /// </summary>
        public virtual float SustainLevel { get; set; }

        /// <summary>
        /// Gets or sets the envelope release time (ms).
        /// </summary>
        public virtual double ReleaseTime { get; set; }

        /// <summary>
        /// Gets or sets the envelope mode.
        /// </summary>
        public virtual ModeType Mode { get; protected set; }

        /// <summary>
        /// Gets or sets a value indicating whether this envelope is legato.
        /// Envelopes are in Legato mode by default, meaning that the level does not
        /// have to reach zero before the attack phase is reentered. When legato
        /// mode is false, the envelope will take a moment to reach zero before the
        /// next attack phase (In progress)
        /// </summary>
        public virtual bool Legato { get; set; }

        /// <summary>
        /// Gets or sets the envelope gate.
        /// </summary>
        public virtual int Gate
        {
            protected get
            {
                return this.gate;
            }

            set
            {
                int prev = this.gate;
                this.gate = value;
                if (this.gate == 1 && prev == 0)
                {
                    this.State = EnvelopeState.ATTACK;
                }

                if (this.gate == 0 && prev == 1)
                {
                    this.State = EnvelopeState.RELEASE;
                }
            }
        }

        /// <summary>
        /// Gets or sets the envelope state.
        /// </summary>
        public virtual EnvelopeState State { get; protected set; }

        /// <summary>
        /// Gets or sets the current sample of the envelope.
        /// </summary>
        public virtual float CurrentSample { get; protected set; }
        #endregion

        #region Methods

        /// <summary>
        /// Calculates and returns the next the envelope value.
        /// </summary>
        /// <returns>The envelope value.</returns>
        public virtual float Generate()
        {
            this.CurrentSample = 0f;
            return this.CurrentSample;
        }

        /// <summary>
        /// Multiplies the input by the envelope value.
        /// This may help simplify your code
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>The input signal multiplied by the envelope value.</returns>
        public virtual float Modify(float input)
        {
            this.CurrentSample = input * this.Generate();
            return this.CurrentSample;
        }
        #endregion
    }
}