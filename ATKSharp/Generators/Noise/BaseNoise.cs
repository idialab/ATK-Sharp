//-----------------------------------------------------------------------
// <copyright file="BaseNoise.cs" company="Aaron Anderson">
//     Copyright (c) Aaron Anderson. All rights reserved.
// </copyright>
// <license type="MIT">
// See LICENSE.md in the project root for full license information.  
// </license>
// <summary>This is the BaseNoise class.</summary>
//-----------------------------------------------------------------------
namespace ATKSharp.Generators.Noise
{
    using System;

    /// <summary>
    /// The Noise class.
    /// Contains the base functionality for noise. 
    /// </summary>
    public abstract class BaseNoise : IGenerator
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseNoise"/> class.
        /// </summary>
        protected BaseNoise()
        {
            this.Random = new Random();
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the current sample of this generator.
        /// </summary>
        public virtual float CurrentSample
        {
            get; protected set;
        }

        /// <summary>
        /// Gets or sets a random number generator.
        /// </summary>
        protected Random Random
        {
            get; set;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Generates nothing. This class should not find it's way in your code; use the child classes.
        /// </summary>
        /// <returns>returns zero</returns>
        public virtual float Generate()
        {
            this.CurrentSample = 0f;
            return this.CurrentSample;
        }
        #endregion
    }
}
