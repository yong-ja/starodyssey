using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using SlimDX.D3DCompiler;

namespace AvengersUtd.Odyssey.Graphics.Effects
{
    public class IncludeHandler : Include
    {
        public void Open(IncludeType type, string fileName, Stream parentStream, out Stream stream)
        {
            stream = new FileStream(Global.FXPath + fileName, FileMode.Open, FileAccess.Read);
        }

        public void Close(Stream stream)
        {
            stream.Close();
        }
    }
}
