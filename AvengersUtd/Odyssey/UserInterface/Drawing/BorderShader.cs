using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AvengersUtd.Odyssey.UserInterface.Drawing
{
    public class BorderShader : LinearShader, IBorderShader
    {
        public Borders Borders { get; set; }
    }
}
