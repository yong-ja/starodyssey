#region Disclaimer

/* 
 * SafeNativeMethods
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

using System.Runtime.InteropServices;
using System.Security;

namespace AvengersUtd.Odyssey.UserInterface.Helpers
{
    [SuppressUnmanagedCodeSecurity]
    internal static class SafeNativeMethods
    {
        [DllImport("kernel32")]
        [return : MarshalAs(UnmanagedType.Bool)]
        internal static extern bool QueryPerformanceFrequency(ref long PerformanceFrequency);

        [DllImport("kernel32")]
        [return : MarshalAs(UnmanagedType.Bool)]
        internal static extern bool QueryPerformanceCounter(ref long PerformanceCount);
    }
}