//-----------------------------------------------------------------------
// <copyright file="WhiteNoise.cs" company="Aaron Anderson">
//     Copyright (c) Aaron Anderson. All rights reserved.
// </copyright>
// <license type="MIT">
// See LICENSE.md in the project root for full license information.  
// </license>
// <summary>This is the WhiteNoise class.</summary>
//-----------------------------------------------------------------------
namespace ATKSharp.Generators.Noise
{
    /// <summary>
    /// The WhiteNoise class.
    /// Contains the functionality to generate white noise.
    /// </summary>
    public class WhiteNoise : BaseNoise
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="WhiteNoise"/> class.
        /// </summary>
        public WhiteNoise() : base()
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the current sample.
        /// </summary>
        public override float CurrentSample
        {
            get
            {
                return (float)this.Random.NextDouble();
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
            return this.CurrentSample;
        }
        #endregion
    }
}
