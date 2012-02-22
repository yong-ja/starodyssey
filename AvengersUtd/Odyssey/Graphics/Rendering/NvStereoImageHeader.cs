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
    }
}
