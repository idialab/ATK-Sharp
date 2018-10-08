//-----------------------------------------------------------------------
// <copyright file="Smoother.cs" company="Aaron Anderson">
//     Copyright (c) Aaron Anderson. All rights reserved.
// </copyright>
// <license type="MIT">
// See LICENSE.md in the project root for full license information.  
// </license>
// <summary>This is the Smoother class.</summary>
//-----------------------------------------------------------------------
namespace ATKSharp.Utilities
{
    using System;

    /// <summary>
    /// The Smoother class. Use this to smooth input data from a gui or 3D world at the audio rate to avoid clicks.
    /// Also works well for portamento 
    /// </summary>
    public class Smoother 
    {
        #region Fields
        private float a;
        private float b;
        private float smoothTime;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Smoother"/> class.
        /// </summary>
        /// <param name="smoothTime">The initial smooth time.</param>
        /// <param name="startValue">The initial value.</param>
        public Smoother(float smoothTime = 20f, float startValue = 0f)
        {
            this.SmoothTime = smoothTime;
            this.CurrentSample = startValue;
        }
        #endregion

#region Properties
        /// <summary>
        /// Gets the current sample.
        /// </summary>
        public float CurrentSample
        {
            get; private set;
        }

        /// <summary>
        /// Gets or sets the smooth time.
        /// </summary>
        public float SmoothTime
        {
            get
            {
                return this.smoothTime;
            }

            set
            {
                if (this.smoothTime != value)
                { // don't recalculate if this is the same smooth time
                    this.smoothTime = value;
                    this.a = (float)Math.Exp((-Math.PI * 2.0f) / Utilities.MillisecondsToSamples(this.smoothTime));
                    this.b = 1.0f - this.a;
                }
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Smooths the input.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>Returns the smoothed value.Utlities</returns>
        public float Smooth(float input)
        {
            if (input != this.CurrentSample)
            {
                this.CurrentSample = (input * this.b) + (this.CurrentSample * this.a);
            }

            return this.CurrentSample;
        }
        #endregion
    }
}
