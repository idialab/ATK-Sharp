//-----------------------------------------------------------------------
// <copyright file="TapIn.cs" company="Aaron Anderson">
//     Copyright (c) Aaron Anderson. All rights reserved.
// </copyright>
// <license type="MIT">
// See LICENSE.md in the project root for full license information.  
// </license>
// <summary>This is the TapIn class.</summary>
//-----------------------------------------------------------------------
namespace ATKSharp.Utilities
{
    /// <summary>
    /// The TapIn class.
    /// </summary>
    public class TapIn
    {
        #region Fields
        private float[] buffer; // pointer to beginning of buffer
        private int bufferSize; // in samples
        private int writeIndex; // keep track of needle head
        #endregion

        #region Constructors 
        /// <summary>
        /// Initializes a new instance of the <see cref="TapIn"/> class.
        /// </summary>
        /// <param name="milliseconds">The length of the buffer in milliseconds.</param>
        public TapIn(float milliseconds)
        {
            this.BufferSize = Utilities.MillisecondsToSamples(milliseconds);
            this.Buffer = new float[this.BufferSize]; // allocate enough space for the buffer
            this.WriteIndex = 0; // initialize the write Index
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the buffer.
        /// </summary>
        public float[] Buffer
        {
            get
            {
                return this.buffer;
            }

            private set
            {
                this.buffer = value;
            }
        }

        /// <summary>
        /// Gets the buffer size.
        /// </summary>
        public int BufferSize
        {
            get
            {
                return this.bufferSize;
            }

            private set
            {
                this.bufferSize = value;
            }
        }

        /// <summary>
        /// Gets the write index.
        /// </summary>
        public int WriteIndex
        {
            get
            {
                return this.writeIndex;
            }

            private set
            {
                this.writeIndex = value;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Feed a sample to the buffer.
        /// </summary>
        /// <param name="sample">The sample.</param>
        public void Feed(float sample)
        { // give the tapIn audio
            this.Buffer[this.WriteIndex] = sample; // store that sample
            this.WriteIndex++; // progress the index
            if (this.WriteIndex >= this.BufferSize)
            {
                this.WriteIndex = 0;
            } // border check
        }
        #endregion
    }
}