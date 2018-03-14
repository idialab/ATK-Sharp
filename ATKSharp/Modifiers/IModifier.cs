//-----------------------------------------------------------------------
// <copyright file="IModifier.cs" company="Aaron Anderson">
//     Copyright (c) Aaron Anderson. All rights reserved.
// </copyright>
// <license type="MIT">
// See LICENSE.md in the project root for full license information.  
// </license>
// <summary>This is the IModifier interface.</summary>
//-----------------------------------------------------------------------
namespace ATKSharp.Modifiers
{
    /// <summary>
    /// The IModifier interface.
    /// </summary>
    public interface IModifier
    {
        #region Properties
        /// <summary>
        /// Gets the current sample.
        /// </summary>
        float CurrentSample
        {
            get;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Modifies the input.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>The modified input.</returns>
        float Modify(float input);
        #endregion
    }
}
