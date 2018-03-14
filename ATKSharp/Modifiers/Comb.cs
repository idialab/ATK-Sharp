//-----------------------------------------------------------------------
// <copyright file="Comb.cs" company="Aaron Anderson">
//     Copyright (c) Aaron Anderson. All rights reserved.
// </copyright>
// <license type="MIT">
// See LICENSE.md in the project root for full license information.  
// </license>
// <summary>This is the Comb class.</summary>
//-----------------------------------------------------------------------
namespace ATKSharp.Modifiers
{
    using ATKSharp.Extensions;
    using ATKSharp.Utilities;

    /// <summary>
    /// The Comb class. Standard comb filter.
    /// </summary>
    public abstract class Comb : BaseModifier
    {
        #region Fields
        private float delayMilliseconds;
        private float feedback;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Comb"/> class.
        /// </summary>
        /// <param name="initMaxDelay">The initial maximum delay.</param>
        /// <param name="initDelay">The initial delay.</param>
        /// <param name="initFeedBack">The initial feedback.</param>
        public Comb(float initMaxDelay = 1000, float initDelay = 1.0f, float initFeedBack = 0.9f)
        {
            this.DelayMax = initMaxDelay;
            this.DelayMilliseconds = initDelay;
            this.DelayLine = new TapIn(this.DelayMax);
            this.DelayLineAccess = new TapOut(this.DelayLine, this.DelayMilliseconds);
            this.Feedback = initFeedBack;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the maximum delay.
        /// </summary>
        public virtual float DelayMax
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the delay in milliseconds.
        /// </summary>
        public virtual float DelayMilliseconds
        {
            get
            {
                return this.delayMilliseconds;
            }

            set
            {
                this.delayMilliseconds = value.Clamp(0f, this.DelayMax); // I changed 1 to delayMax here - Aaron (2-19-18 5:53)
                if (this.DelayLineAccess != null)
                {
                    this.DelayLineAccess.DelayMilliseconds = this.delayMilliseconds;
                }
            }
        }

        /// <summary>
        /// Gets or sets the feedback.
        /// </summary>
        public virtual float Feedback
        {
            get
            {
                return this.feedback;
            }

            set
            {
                this.feedback = value.Clamp(0f, 1f);
            }
        }

        /// <summary>
        /// Gets or sets the delay line reference.
        /// </summary>
        protected virtual TapIn DelayLine
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the delay line access reference. 
        /// </summary>
        protected virtual TapOut DelayLineAccess
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
            this.DelayLine.Feed(input + (delay * this.Feedback));
            this.CurrentSample = input + delay;
            return this.CurrentSample;
        }
        #endregion
    }
}