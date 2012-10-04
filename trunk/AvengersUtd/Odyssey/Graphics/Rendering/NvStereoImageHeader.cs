using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace AvengersUtd.Odyssey.Graphics.Rendering
{
    [StructLayout(LayoutKind.Sequential)]
    public struct NvStereoImageHeader
    {
        private readonly uint dwSignature;
        private readonly uint dwWidth;
        private readonly uint dwHeight;
        private readonly uint dwBPP;
        private readonly uint dwFlags;

        public NvStereoImageHeader(uint dwSignature, uint dwWidth, uint dwHeight, uint dwBpp, uint dwFlags)
        {
            this.dwSignature = dwSignature;
            this.dwWidth = dwWidth;
            this.dwHeight = dwHeight;
            dwBPP = dwBpp;
            this.dwFlags = dwFlags;
        }

        public NvStereoImageHeader(uint width, uint height)
            : this(0x4433564e,
            2* width,
            height,
            32, 0x00000002)
        {
        }

        public byte[] ToByteArray()
        {
            return StructureToByteArray(this);
        }

        static byte[] StructureToByteArray(object obj)
        {
            int len = Marshal.SizeOf(obj);
            byte[] arr = new byte[len];
            IntPtr ptr = Marshal.AllocHGlobal(len);
            Marshal.StructureToPtr(obj, ptr, true);
            Marshal.Copy(ptr, arr, 0, len);
            Marshal.FreeHGlobal(ptr);
            return arr;
        }

    }
}
