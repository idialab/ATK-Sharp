//-----------------------------------------------------------------------
// <copyright file="AllPass.cs" company="Aaron Anderson">
//     Copyright (c) Aaron Anderson. All rights reserved.
// </copyright>
// <license type="MIT">
// See LICENSE.md in the project root for full license information.  
// </license>
// <summary>This is the AllPass class.</summary>
//-----------------------------------------------------------------------
namespace ATKSharp.Modifiers
{
    using ATKSharp.Utilities;

    /// <summary>
    /// The AllPass class.
    /// </summary>
    public class AllPass : BaseModifier
    {
        #region Fields
        private float delayMilliseconds;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="AllPass"/> class.
        /// </summary>
        /// <param name="initMaxDelay">The initial max delay.</param>
        /// <param name="initDelay">The initial delay.</param>
        /// <param name="initGainCoef">The initial gain coefficient.</param>
        public AllPass(float initMaxDelay = 1000f, float initDelay = 1f, float initGainCoef = 1.0f)
        {
            this.DelayLine = new TapIn(initMaxDelay);
            this.DelayMilliseconds = initDelay;
            this.DelayLineAccess = new TapOut(this.DelayLine, this.DelayMilliseconds);
            this.GainCoef = initGainCoef;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the delay in milliseconds.
        /// </summary>
        public float DelayMilliseconds
        {
            get
            {
                return this.delayMilliseconds;
            }

            set
            {
                this.delayMilliseconds = value;
                if (this.DelayLineAccess != null)
                {
                    this.DelayLineAccess.DelayMilliseconds = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the gain coefficient.
        /// </summary>
        public float GainCoef
        {
            get; protected set;
        }

        /// <summary>
        /// Gets or sets the delay line reference.
        /// </summary>
        protected TapIn DelayLine
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the delay line access reference.
        /// </summary>
        protected TapOut DelayLineAccess
        {
            get; set;
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

            // algorithm from EarLevel.com
            // note I did not have the tools to properly test this at the time
            // the effect is hard to hear on its own, but I believe this to be correct
            this.CurrentSample = ((delay + (input * -this.GainCoef)) * this.GainCoef) + input;
            this.DelayLine.Feed(this.CurrentSample);
            return this.CurrentSample;
        }
        #endregion
    }
}