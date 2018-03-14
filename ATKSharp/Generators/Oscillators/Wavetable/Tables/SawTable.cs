//-----------------------------------------------------------------------
// <copyright file="SawTable.cs" company="Aaron Anderson">
//     Copyright (c) Aaron Anderson. All rights reserved.
// </copyright>
// <license type="MIT">
// See LICENSE.md in the project root for full license information.  
// </license>
// <summary>This is the SawTable class.</summary>
//-----------------------------------------------------------------------
namespace ATKSharp.Generators
{
    using System;

    /// <summary>
    /// The SawTable class.
    /// Singleton that contains the functionality to create a saw wave table.
    /// </summary>
    public class SawTable : WaveTable
    {
        #region Fields
        private static SawTable instance;
        #endregion

        #region Constructors
        private SawTable() : base()
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
                    float numHarmonics = 1; // This needs to be initiated before the for loop
                    this.Table[i, j] = 0;
                    while (this.LowestFreqList[i] * numHarmonics < this.Nyquist)
                    { // Add harmonics until nyquist is reached.
                        // consider making sure that the frequency stays under the local table nyquist as well.
                        float theta = (float)(j * Math.PI * 2 * numHarmonics) / (this.TableSize - 1);
                        this.Table[i, j] += (float)Math.Sin(theta) * (-1 / numHarmonics);
                        numHarmonics += 1;
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
                        largestValue = Math.Abs(this.Table[i, j]);
                    }
                }

                float scalarValue = 1 / (float)largestValue; // calculate the adjustment value

                for (int j = 0; j < this.TableSize; j++)
                {
                    this.Table[i, j] *= scalarValue; // multiply the adjustment
                }
            }
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the static SawTable instance.
        /// </summary>
        public static SawTable Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SawTable();
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
