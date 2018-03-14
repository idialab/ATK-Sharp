//-----------------------------------------------------------------------
// <copyright file="SquareTable.cs" company="Aaron Anderson">
//     Copyright (c) Aaron Anderson. All rights reserved.
// </copyright>
// <license type="MIT">
// See LICENSE.md in the project root for full license information.  
// </license>
// <summary>This is the SquareTable class.</summary>
//----------------------------------------------------------------------- 
namespace ATKSharp.Generators
{
    using System;

    /// <summary>
    /// The SquareTable class.
    /// Singleton that contains the functionality to create a square wave table.
    /// </summary>
    public class SquareTable : WaveTable
    {
        #region Fields
        private static SquareTable instance;
        #endregion

        #region Constructors
        /// <summary>
        /// Prevents a default instance of the <see cref="SquareTable"/> class from being created.
        /// </summary>
        private SquareTable() : base()
        {
            // keep tabs on the lowest frequency for each octave
            for (int i = 0; i < this.NumTables; i++)
            { // set for each octave
                this.LowestFreqList[i] = this.CurrentLow;
                this.CurrentLow *= 2.0f; // jump to the next octave
            }

            /* generate the harmonics for each sample of each table
            This breaks down to an additive process. Sinusoids are summed as long as
            the resulting frequency will be under the nyquist frequency */
            for (int i = 0; i < this.NumTables; i++)
            {
                for (int j = 0; j < this.TableSize; j++)
                {
                    float numHarmonics = 1;
                    this.Table[i, j] = 0;
                    while (this.LowestFreqList[i] * numHarmonics < this.Nyquist)
                    {
                        float theta = (float)(j * Math.PI * 2 * numHarmonics) / (this.TableSize - 1);
                        this.Table[i, j] += (float)Math.Sin(theta) * (1 / numHarmonics);
                        numHarmonics += 2;
                    }
                }
            }

            // normalize the tables-----------------------------------------
            for (int i = 0; i < this.NumTables; i++)
            {
                // find the largest ignoring polarity
                float largestValue = 0;
                for (int j = 0; j < this.TableSize; j++)
                {
                    if (Math.Abs(this.Table[i, j]) > largestValue)
                    {
                        largestValue = (float)Math.Abs(Table[i, j]);
                    }
                }

                float scalarValue = 1 / largestValue; // calculate the adjustment value
                                                      // multiply the adjustment
                for (int j = 0; j < this.TableSize; j++)
                {
                    this.Table[i, j] *= scalarValue;
                }
            }
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the static SquareTable instance.
        /// </summary>
        public static SquareTable Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SquareTable();
                }

                return instance;
            }

            private set
            {
                instance = value;
            }
        }
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
