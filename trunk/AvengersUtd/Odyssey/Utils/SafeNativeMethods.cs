using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace AvengersUtd.Odyssey
{
    [System.Security.SuppressUnmanagedCodeSecurity] // We won't use this maliciously, no seriously!
    public static class SafeNativeMethods
    {
        [DllImport("kernel32")]
        [return : MarshalAs(UnmanagedType.Bool)]
        internal static extern bool QueryPerformanceFrequency(ref long PerformanceFrequency);

        [DllImport("kernel32")]
        [return : MarshalAs(UnmanagedType.Bool)]
        internal static extern bool QueryPerformanceCounter(ref long PerformanceCount);
    }
}