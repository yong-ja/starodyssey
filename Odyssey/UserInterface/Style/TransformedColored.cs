#if (SlimDX)

using SlimDX;
using SlimDX.Direct3D9;
using System.Runtime.InteropServices;

namespace AvengersUtd.Odyssey.UserInterface.Style
{
    [StructLayout(LayoutKind.Sequential)]
    public struct TransformedColored
    {
        Vector4 positionRhw;
        public int color;
        static readonly VertexFormat format;

        static TransformedColored()
        {
            format = VertexFormat.PositionRhw | VertexFormat.Diffuse;
        }

        public TransformedColored(float xValue, float yValue, float zValue, float rhwValue, int color)
        {
            positionRhw = new Vector4(xValue, yValue, zValue, rhwValue);
            this.color = color;
        }

        public static VertexFormat Format
        {
            get { return format; }
        }

        public static int SizeInBytes
        {
            get { return Marshal.SizeOf(typeof(TransformedColored)); }
        }
    }
}
#endif
