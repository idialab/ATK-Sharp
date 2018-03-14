//-----------------------------------------------------------------------
// <copyright file="ATKSettings.cs" company="Aaron Anderson">
//     Copyright (c) Aaron Anderson. All rights reserved.
// </copyright>
// <license type="MIT">
// See LICENSE.md in the project root for full license information.  
// </license>
// <summary>This is the Window class.</summary>
//-----------------------------------------------------------------------
namespace ATKSharp
{
    /// <summary>
    /// The ATK Settings class.
    /// Contains static properties that most ATK classes will need to access globally.
    /// </summary>
    public static class ATKSettings
    {
        #region Fields
        private static int sampleRate = 48000;
        private static int bufferSize = 512;

        /// <summary>
        /// Gets or sets the application audio sample rate.
        /// </summary>
        public static int SampleRate
        {
            get
            {
                return sampleRate;
            }

            set
            {
                sampleRate = value;
            }
        }

        /// <summary>
        /// Gets or sets the application audio buffer size.
        /// </summary>
        public static int BufferSize
        {
            get
            {
                return bufferSize;
            }

            set
            {
                bufferSize = value;
            }
        }

        /// <summary>
        /// Gets half the application audio sample rate.
        /// </summary>
        public static float HalfSampleRate
        {
            get
            {
                return SampleRate * .5f;
            }
        }
        #endregion
    }
}
