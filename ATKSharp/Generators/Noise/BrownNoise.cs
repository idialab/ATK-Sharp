//-----------------------------------------------------------------------
// <copyright file="BrownNoise.cs" company="Aaron Anderson">
//     Copyright (c) Aaron Anderson. All rights reserved.
// </copyright>
// <license type="MIT">
// See LICENSE.md in the project root for full license information.  
// </license>
// <summary>This is the BrownNoise class.</summary>
//-----------------------------------------------------------------------
namespace ATKSharp.Generators.Noise
{
    /// <summary>
    /// The BrownNoise class.
    /// Contains the functionality to generate brown noise.
    /// </summary>
    public class BrownNoise : BaseNoise
    {
        #region Fields
        private float largeVal; // used to interact more freely with -1, 1 borders;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="BrownNoise"/> class.
        /// </summary>
        /// <param name="jumpMax">The max jump.</param>
        public BrownNoise(float jumpMax = 1.0f) : base()
        {
            this.JumpMax = jumpMax;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the max jump.
        /// </summary>
        public float JumpMax
        {
            private get; set;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Generates a signal.
        /// </summary>
        /// <returns>The signal.</returns>
        public override float Generate()
        {
            while (true)
            {
                float r = (float)((Random.NextDouble() * 2 * this.JumpMax) - this.JumpMax);
                this.largeVal += r;
                if (this.largeVal < -16.0f || this.largeVal >= 16.0f)
                {
                    this.largeVal -= r;
                }
                else
                {
                    this.CurrentSample = this.largeVal * 0.0625f;
                    break;
                }
            }

            return this.CurrentSample;
        }
        #endregion
    }
}