//-----------------------------------------------------------------------
// <copyright file="SineTable.cs" company="Aaron Anderson">
//     Copyright (c) Aaron Anderson. All rights reserved.
// </copyright>
// <license type="MIT">
// See LICENSE.md in the project root for full license information.  
// </license>
// <summary>This is the SineTable class.</summary>
//-----------------------------------------------------------------------
namespace ATKSharp.Generators
{
    using System;
    using ATKSharp.Extensions;

    /// <summary>
    /// The SineTable class.
    /// Singleton that contains the functionality to create a sine wave table.
    /// </summary>
    public class SineTable : WaveTable
    {
        #region Fields
        private static SineTable instance;
        #endregion

        #region Constructors
        /// <summary>
        /// Prevents a default instance of the <see cref="SineTable"/> class from being created.
        /// </summary>
        private SineTable() : base()
        {
            this.Table = new float[TableSize];
            for (int j = 0; j < this.TableSize; j++)
            {
                float theta = ((float)j).Map(0, this.TableSize - 1, 0, (float)Math.PI * 2);

                // float theta = (float)((j / 2048f) * Math.PI * 2);
                this.Table[j] = (float)Math.Sin(theta);
            }
        }
        #endregion
        
        #region Properties
        /// <summary>
        /// Gets the static SineTable instance.
        /// </summary>
        public static SineTable Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SineTable();
                }

                return instance;
            }

            private set
            {
                instance = value;
            }
        }

        /// <summary>
        /// Gets or sets the table.
        /// </summary>
        public new float[] Table
        {
            get; protected set;
        } // hide the base class version, because SineTable is just a float array 
        #endregion
    }

    /// <remarks>
    /// The wavetable oscillators are the most efficient option for generating 
    /// synthetic audio signal. Use these if you need to listen to the waveform
    /// without aliasing but do not need control of the duty cycle. RAM impact is fairly 
    /// high with these (10x2048x32 bits per shape!) but the waveforms are only generated 
    /// once per shape.This is negligable on a modern PC but might be a factor on a 
    /// mobile device. 
    /// </remarks>
}
