//-----------------------------------------------------------------------
// <copyright file="IGenerator.cs" company="Aaron Anderson">
//     Copyright (c) Aaron Anderson. All rights reserved.
// </copyright>
// <license type="MIT">
// See LICENSE.md in the project root for full license information.  
// </license>
// <summary>This is the IGenerator class.</summary>
//-----------------------------------------------------------------------
namespace ATKSharp.Generators
{
    /// <summary>
    /// The IGenerator interface.
    /// </summary>
    public interface IGenerator
    {
        /// <summary>
        /// Gets the current sample of this generator.
        /// </summary>
        float CurrentSample
        {
            get;
        }

        /// <summary>
        /// Generates the current sample.
        /// </summary>
        /// <returns>The current sample.</returns>
        float Generate();
    }
}
