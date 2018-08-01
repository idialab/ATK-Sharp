//-----------------------------------------------------------------------
// <copyright file="PinkNoise.cs" company="Aaron Anderson">
//     Copyright (c) Aaron Anderson. All rights reserved.
// </copyright>
// <license type="MIT">
// See LICENSE.md in the project root for full license information.  
// </license>
// <summary>This is the PinkNoise class.</summary>
//-----------------------------------------------------------------------
namespace ATKSharp.Generators.Noise
{
    using System;

    /// <summary>
    /// The PinkNoise class.
    /// Contains the functionality to generate pink noise.
    /// </summary>
    public class PinkNoise : BaseNoise
    {
        // there are 31 table entries
        /*
         x|x|x|x|x|x|x|x|x|x|x|x|x|x|x|x|
          x | x | x | x | x | x | x | x
            x   |   x   |   x   |   x
                x       |       x
                        x

         This can be thought of in terms of a step sequencer. Each
         tick the sequencer increments one value. The value at that location 
         is replaced with a new random sample. The sum of all values is the result. 
         */
        #region Fields
        private float[][] tables = new float[5][];
        private int x; // TODO: ?
        private int tableIndex, index;
        private float oldValue, newValue, difference;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="PinkNoise"/> class.
        /// </summary>
        public PinkNoise() : base()
        {
            for (int i = this.tables.Length - 1; i >= 0; i--)
            {
                this.tables[i] = new float[(int)Math.Pow(2, i)];
                for (int j = 0; j < this.tables[i].Length; j++)
                {
                    this.tables[i][j] = ((float)this.Random.NextDouble() * 2) - 1;
                }
            }

            this.PreviousValue = 0;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the previous value.
        /// </summary>
        private float PreviousValue
        {
            get; set;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Generate the next pink noise sample. The current method is not incredibily 
        /// efficient and does not produce the desired result. This is in progress.
        /// </summary>
        /// <returns>The signal.</returns>
        public override float Generate()
        {
            this.tableIndex = this.FindTable(this.x);
            this.index = this.FindEntry(this.tableIndex, this.x);
            this.oldValue = this.tables[this.tableIndex][this.index];
            this.newValue = ((float)this.Random.NextDouble() * 2) - 1;
            this.tables[this.tableIndex][this.index] = this.newValue;
            this.difference = this.newValue - this.oldValue;
            this.CurrentSample = this.PreviousValue + this.difference;
            this.PreviousValue = this.CurrentSample;

            // currentSample += ofRandom(-1.0, 1.0);
            return this.CurrentSample;
        }

        private int FindTable(int index)
        {
            if (index % 2 == 0)
            {
                return 0;
            }
            else if ((index + 1) % 4 == 0)
            {
                return 1;
            }
            else if ((index + 3) % 8 == 0)
            {
                return 2;
            }
            else if ((index + 7) % 16 == 0)
            {
                return 3;
            }
            else if (index == 15)
            { // there is only one in this table
                return 4;
            }
            else
            {
                return 0;
            }
        }

        private int FindEntry(int table, int index)
        {
            return table == 4 ? 0 : (index - ((int)Math.Pow(2, table) - 1)) / (int)Math.Pow(2, table + 1);
            /*switch (table) {
                case 0:
                    return index / 2;
                case 1:
                    return (index - 1) / 4;
                case 2:
                    return (index - 3) / 8;
                case 3:
                    return (index - 7) / 16;
                case 4:
                    return 0;
                default:
                    return 0;
                    Trace.TraceWarning("[ATK] Issue with table in Pink Noise.");
            }*/
        }
        #endregion
    }
}