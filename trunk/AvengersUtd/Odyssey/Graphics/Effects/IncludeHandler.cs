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
        private readonly string effectFile;
        public IncludeHandler(string effectFile)
        {
            this.effectFile = effectFile;
        }
        public void Open(IncludeType type, string fileName, Stream parentStream, out Stream stream)
        {
            string fullPath = Global.FXPath + fileName;
            if (!File.Exists(fullPath))
                Error.MessageMissingFile(Properties.Resources.ERR_IO_EffectMissingIncludeFile, fullPath, effectFile);

            stream = new FileStream(fullPath, FileMode.Open, FileAccess.Read);
        }

        public void Close(Stream stream)
        {
            stream.Close();
        }
    }
}
