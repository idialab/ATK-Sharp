//-----------------------------------------------------------------------
// <copyright file="Utilities.cs" company="Aaron Anderson">
//     Copyright (c) Aaron Anderson. All rights reserved.
// </copyright>
// <license type="MIT">
// See LICENSE.md in the project root for full license information.  
// </license>
// <summary>This is the Utilities class.</summary>
//-----------------------------------------------------------------------
namespace ATKSharp.Utilities
{
    using System;

    /// <summary>
    /// The Utilities class.
    /// </summary>
    public static class Utilities
    {
        /// <summary>
        /// Converts a MIDI note to its corresponding frequency.
        /// </summary>
        /// <param name="midiNote">The MIDI note.</param>
        /// <returns>The frequency.</returns>
        public static float MidiToFrequency(float midiNote)
        {
            return (float)(440 * Math.Pow(2, (midiNote - 69) / 12.0f));
        }

        /// <summary>
        /// Converts a MIDI note to its corresponding speed.
        /// </summary>
        /// <param name="midiNote">The MIDI note.</param>
        /// <returns>The speed.</returns>
        public static float MidiToSpeed(float midiNote)
        { // assumes middle C as normal speed (1.0);
            return (float)Math.Pow(2, (midiNote - 60) / 12.0f);
        }

        /// <summary>
        /// Converts milliseconds to a number of samples.
        /// </summary>
        /// <param name="milliseconds">The number of milliseconds.</param>
        /// <returns>The number of samples.</returns>
        public static int MillisecondsToSamples(float milliseconds)
        {
            return (int)(milliseconds * 0.001 * ATKSettings.SampleRate);
        }

        /// <summary>
        /// Converts samples to milliseconds.
        /// </summary>
        /// <param name="samples">The number of samples.</param>
        /// <returns>time (ms)</returns>
        public static float SamplesToMilliseconds(int samples)
        {
            return samples / (ATKSettings.SampleRate * 1000f);
        }

        /// <summary>
        /// Coverts decibels to a 0-1 amplitude value.
        /// </summary>
        /// <param name="decibels">The decibels.</param>
        /// <returns>The amplitude.</returns>
        public static float DecibelsToAmplitude(float decibels)
        {
            return (float)Math.Pow(10, decibels / 20);
        }
    }
}