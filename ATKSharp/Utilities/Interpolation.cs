//-----------------------------------------------------------------------
// <copyright file="Interpolation.cs" company="Aaron Anderson">
//     Copyright (c) Aaron Anderson. All rights reserved.
// </copyright>
// <license type="MIT">
// See LICENSE.md in the project root for full license information.  
// </license>
// <summary>This is the Interpolation class.</summary>
//-----------------------------------------------------------------------
namespace ATKSharp.Utilities
{
    using System;

    /// <summary>
    /// The static Interpolation class
    /// </summary>
    public static class Interpolation
    {
        #region Methods
        /// <summary>
        /// Interpolates from the origin to the destination by the interpolation value.
        /// </summary>
        /// <param name="originX">The origin x coordinate.</param>
        /// <param name="originY">The origin y coordinate.</param>
        /// <param name="destinationX">The destination x coordinate.</param>
        /// <param name="destinationY">The destination y coordinate.</param>
        /// <param name="interpolationValue">The interpolation value.</param>
        /// <returns>The interpolated value.</returns>
        public static float Linear(float originX, float originY, float destinationX, float destinationY, float interpolationValue)
        {
            if (destinationX == originX)
            {
                return 0;
            } // avoid divideBy0
            float scalar = (interpolationValue - originX) / (destinationX - originX); // gather normalized position between two x values
            return (originY * scalar) + (destinationY * (1 - scalar));
        }

        // a small form version that assumes x locations are integers and neighbors

        /// <summary>
        /// Interpolates from the origin to the destination by the interpolation value.
        /// </summary>
        /// <param name="origin">The origin.</param>
        /// <param name="destination">The destination.</param>
        /// <param name="interpolationValue">The interpolation value.</param>
        /// <returns>The interpolated value.</returns>
        public static float Linear(float origin, float destination, float interpolationValue)
        { // this version assumes integer x values
            float scalar = interpolationValue - (int)interpolationValue; // how far away are we from y0?
            return (origin * (1 - scalar)) + (destination * scalar);
        }

        // this is considerably more expensive than linear

        /// <summary>
        /// Interpolates from the origin to the destination by the interpolation value.
        /// </summary>
        /// <param name="preOrigin">The pre-origin.</param>
        /// <param name="origin">The origin.</param>
        /// <param name="destination">The destination.</param>
        /// <param name="postDestination">The post destination.</param>
        /// <param name="interpolationValue">The interpolation value.</param>
        /// <returns>The interpolated value.</returns>
        public static float Cubic(float preOrigin, float origin, float destination, float postDestination, float interpolationValue)
        {
            float x = interpolationValue - (int)interpolationValue; // how far between points x1 and x2?
            float a = (-0.5f * preOrigin) + (1.5f * origin) - (1.5f * destination) + (0.5f * postDestination);
            float b = preOrigin - (2.5f * origin) + (2 * destination) - (0.5f * postDestination);
            float c = (-0.5f * preOrigin) + (0.5f * destination);
            return (a * (float)Math.Pow(x, 3f)) + (b * (float)Math.Pow(x, 2f)) + (c * x) + origin;
        }
        #endregion
    }

    /// <remarks>
    /// I find a noticeable quality difference between cubic and linear interploation in most use cases. However, 
    /// I have yet to hear a quality difference that warrented the extra CPU cost. If you are not concerned with 
    /// maxing out your CPU, cubic is for you! 
    /// </remarks>
}
