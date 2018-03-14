//-----------------------------------------------------------------------
// <copyright file="TapOut.cs" company="Aaron Anderson">
//     Copyright (c) Aaron Anderson. All rights reserved.
// </copyright>
// <license type="MIT">
// See LICENSE.md in the project root for full license information.  
// </license>
// <summary>This is the TapOut class.</summary>
//-----------------------------------------------------------------------
namespace ATKSharp.Utilities
{
    using ATKSharp.Generators;

    /// <summary>
    /// The TapOut class.
    /// </summary>
    public class TapOut : IGenerator
    {
        #region Fields
        private int readIndex;
        private float delayMilliseconds;
        private TapIn tapIn; // TODO: Property to swich references. This would be tricky, as we would have to decide where to put the reader
        #endregion

        #region Constructors 
        /// <summary>
        /// Initializes a new instance of the <see cref="TapOut"/> class.
        /// </summary>
        /// <param name="inRef">The initial TapIn reference.</param>
        /// <param name="milliseconds">The delay in milliseconds.</param>
        public TapOut(TapIn inRef, float milliseconds)
        {
            TapIn = inRef;
            this.DelayMilliseconds = milliseconds;
            this.ReadIndex = (int)(TapIn.BufferSize - Utilities.MillisecondsToSamples(milliseconds) - 1);
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the read index.
        /// </summary>
        public int ReadIndex
        {
            get
            {
                return this.readIndex;
            }

            set
            {
                if (value < TapIn.BufferSize && value > 0)
                {
                    this.readIndex = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the delay in milliseconds.
        /// </summary>
        public float DelayMilliseconds
        {
            get
            {
                return this.delayMilliseconds;
            }

            set
            {
                if (value > 0)
                { // This version of ATK can't see into the future.
                    this.delayMilliseconds = value;
                    this.ReadIndex = TapIn.WriteIndex - Utilities.MillisecondsToSamples(this.delayMilliseconds);
                    while (this.ReadIndex < 0)
                    {
                        this.ReadIndex += TapIn.BufferSize;
                    }
                }
            }
        }

        /// <summary>
        /// Gets the current sample.
        /// </summary>
        public float CurrentSample
        {
            get; private set;
        }

        /// <summary>
        /// Gets or sets the TapIn.
        /// </summary>
        public TapIn TapIn
        {
            get
            {
                return this.tapIn;
            }

            set
            {
                this.tapIn = value;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// produce the next sample of the buffer
        /// </summary>
        /// <returns>delay sample</returns>
        public float Generate()
        {
            this.CurrentSample = TapIn.Buffer[this.ReadIndex];
            this.ReadIndex++;
            if (this.readIndex >= TapIn.BufferSize)
            {
                this.ReadIndex = 0;
            }

            return this.CurrentSample;
        }
        #endregion 
    }
}
