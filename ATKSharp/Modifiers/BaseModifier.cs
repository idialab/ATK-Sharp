//-----------------------------------------------------------------------
// <copyright file="BaseModifier.cs" company="Aaron Anderson">
//     Copyright (c) Aaron Anderson. All rights reserved.
// </copyright>
// <license type="MIT">
// See LICENSE.md in the project root for full license information.  
// </license>
// <summary>This is the BaseModifier class.</summary>
//-----------------------------------------------------------------------
namespace ATKSharp.Modifiers
{
    /// <summary>
    /// The Modifier Type.
    /// </summary>
    public enum ModifierType
    {
        /// <summary>
        /// The Low Pass modifier type.
        /// </summary>
        LowPass,

        /// <summary>
        /// The High Pass modifier type.
        /// </summary>
        HighPass,

        /// <summary>
        /// The Band Pass modifier type.
        /// </summary>
        BandPass,

        /// <summary>
        /// The Band Reject modifier type.
        /// </summary>
        BandReject,

        /// <summary>
        /// The Notch modifier type.
        /// </summary>
        Notch,

        /// <summary>
        /// The Peak modifier type.
        /// </summary>
        Peak,

        /// <summary>
        /// The Low Shelf modifier type.
        /// </summary>
        LowShelf,

        /// <summary>
        /// The High Shelf modifier type.
        /// </summary>
        HighShelf
    }

    /// <summary>
    /// The BaseModifier class.
    /// Contains the base functionality for modifiers.
    /// </summary>
    public abstract class BaseModifier : IModifier
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseModifier"/> class.
        /// </summary>
        protected BaseModifier()
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the modifier type.
        /// </summary>
        public virtual ModifierType ModifierType
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the modifier frequency.
        /// </summary>
        public virtual float Frequency
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the current sample.
        /// </summary>
        public virtual float CurrentSample
        {
            get; protected set;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Abstract class. BaseModifier should not be used in your code unless you are extending ATK
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>The modified input.</returns>
        public virtual float Modify(float input)
        {
            this.CurrentSample = input;
            return this.CurrentSample; // base filter doesn't do anything to the input
        }
        #endregion
    }
}
