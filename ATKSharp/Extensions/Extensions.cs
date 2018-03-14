//-----------------------------------------------------------------------
// <copyright file="Extensions.cs" company="Aaron Anderson">
//     Copyright (c) Aaron Anderson. All rights reserved.
// </copyright>
// <license type="MIT">
// See LICENSE.md in the project root for full license information.  
// </license>
// <summary>This is the Extensions class.</summary>
//-----------------------------------------------------------------------
namespace ATKSharp.Extensions
{
    using System;

    /// <summary>
    /// The ATK Extensions class.
    /// Contains extension methods to add functionality. 
    /// </summary>
    public static class Extensions
    {
        #region Methods
        /// <summary>
        /// Clamps an IComparable <typeparamref name="T"/> value to the range defined by minimum and maximum.
        /// </summary>
        /// <typeparam name="T">A type that inherits from the IComparable interface.</typeparam>
        /// <param name="value">The value to clamp.</param>
        /// <param name="minimum">The minimum value.</param>
        /// <param name="maximum">The maximum value.</param>
        /// <returns>The clamped value.</returns>
        public static T Clamp<T>(this T value, T minimum, T maximum) where T : IComparable<T>
        {
            if (value.CompareTo(minimum) < 0)
            {
                return minimum;
            }
            else if (value.CompareTo(maximum) > 0)
            {
                return maximum;
            }
            else
            {
                return value;
            }
        }

        /// <summary>
        /// Maps the value to an output range.
        /// </summary>
        /// <param name="value">The value to map.</param>
        /// <param name="inputMin">The input range minimum.</param>
        /// <param name="inputMax">The input range maximum.</param>
        /// <param name="outputMin">The output range minimum.</param>
        /// <param name="outputMax">The output range maximum.</param>
        /// <param name="clamp">Should the value be clamped to the output range?</param>
        /// <returns>Mapped value.</returns>
        public static float Map(this float value, float inputMin, float inputMax, float outputMin, float outputMax, bool clamp = false)
        {
            float unclampedValue = ((value - inputMin) / (inputMax - inputMin) * (outputMax - outputMin)) + outputMin;
            return clamp ? unclampedValue.Clamp(outputMin, outputMax) : unclampedValue;
        }
        #endregion
    }
}
