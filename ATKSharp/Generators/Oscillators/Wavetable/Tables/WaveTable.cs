//-----------------------------------------------------------------------
// <copyright file="WaveTable.cs" company="Aaron Anderson">
//     Copyright (c) Aaron Anderson. All rights reserved.
// </copyright>
// <license type="MIT">
// See LICENSE.md in the project root for full license information.  
// </license>
// <summary>This is the WaveTable class.</summary>
//-----------------------------------------------------------------------
namespace ATKSharp.Generators
{
    /// <summary>
    /// The WaveTable class.
    /// Contains the functionality for creating a general wave table.
    /// </summary>
    public abstract class WaveTable
    {
        #region Fields
        private const int NUMTABLES = 10;
        private const int TABLESIZE = 2048;
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="WaveTable"/> class.
        /// </summary>
        protected WaveTable()
        {
            this.LowestFreqList = new float[NUMTABLES];
            this.TableFundamentalFreq = ATKSettings.SampleRate / TABLESIZE;
            this.Table = new float[NUMTABLES, TABLESIZE];
            this.CurrentLow = 20;
            this.Nyquist = 20000;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the lowest frequency list.
        /// </summary>
        public virtual float[] LowestFreqList
        {
            get; protected set;
        }

        /// <summary>
        /// Gets or sets the table fundamental frequency.
        /// </summary>
        public virtual float TableFundamentalFreq
        {
            get; protected set;
        }

        /// <summary>
        /// Gets or sets the table.
        /// </summary>
        public virtual float[,] Table
        {
            get; protected set;
        }

        /// <summary>
        /// Gets the number of tables.
        /// </summary>
        public virtual int NumTables
        {
            get
            {
                return NUMTABLES;
            }
        }

        /// <summary>
        /// Gets the table size.
        /// </summary>
        public virtual int TableSize
        {
            get
            {
                return TABLESIZE;
            }
        }

        /// <summary>
        /// Gets or sets the current low.
        /// </summary>
        protected virtual float CurrentLow
        {
            get; set;
        } // lowest octave's lowest frequency

        /// <summary>
        /// Gets or sets the nyquist.
        /// </summary>
        protected virtual float Nyquist
        {
            get; set;
        }
        #endregion
    }
}
