#region Disclaimer

/* 
 * Timer
 *
 * Created on 21 August 2007
 * Author: Adalberto L. Simeone (Taranto, Italy)
 * Website: http://www.avengersutd.com
 *
 * Part of the Odyssey User Interface Library
 *
 * This source code is Intellectual property of the Author
 * and is released under the Creative Commons Attribution 
 * NonCommercial License, available at:
 * http://creativecommons.org/licenses/by-nc/3.0/ 
 * You can alter and use this source code as you wish, 
 * provided that you do not use the results in commercial
 * projects, without the express and written consent of
 * the Author.
 *
 */

#endregion

using System;

namespace AvengersUtd.Odyssey.UserInterface.Helpers
{
    /// <summary>
    /// High Precision Timer Class
    /// </summary>
    public class Timer
    {
        long baseTime;
        long elapsedTime;
        long ticksPerSecond;

        /// <summary>
        /// High Precision Timer Class
        /// </summary>
        public Timer()
        {
            // Use QueryPerformanceFrequency to get frequency of the timer
            if (!SafeNativeMethods.QueryPerformanceFrequency(ref ticksPerSecond))
                throw new ApplicationException("Timer: Performance Frequency Unavailable");
            Reset();
        }

        /// <summary>
        /// Resets the Timer, updates the base time
        /// </summary>
        public void Reset()
        {
            long time = 0;
            SafeNativeMethods.QueryPerformanceCounter(ref time);
            baseTime = time;
            elapsedTime = 0;
        }

        /// <summary>
        /// Get the time since last reset
        /// </summary>
        /// <returns>The time since last reset.</returns>
        public double GetTime()
        {
            long time = 0;
            SafeNativeMethods.QueryPerformanceCounter(ref time);
            return (double) (time - baseTime)/(double) ticksPerSecond;
        }

        /// <summary>
        /// Get the current time of the system
        /// </summary>
        /// <returns>The current time in seconds.</returns>
        public double GetAbsoluteTime()
        {
            long time = 0;
            SafeNativeMethods.QueryPerformanceCounter(ref time);
            return (double) time/(double) ticksPerSecond;
        }

        /// <summary>
        /// Get the time since last call of this method, This is a Rendering Timer
        /// </summary>
        /// <returns>The number of seconds since last call of this function.</returns>
        public double GetElapsedTime()
        {
            long time = 0;
            SafeNativeMethods.QueryPerformanceCounter(ref time);
            double absoluteTime = (double) (time - elapsedTime)/(double) ticksPerSecond;
            elapsedTime = time;
            return absoluteTime;
        }
    }
}