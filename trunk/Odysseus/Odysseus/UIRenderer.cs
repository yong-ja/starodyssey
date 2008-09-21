using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AvengersUtd.Odyssey;
using AvengersUtd.Odyssey.UserInterface.Helpers;

namespace AvengersUtd.Odysseus
{
    public class UIRenderer : Renderer
    {
        public override void Init()
        {
            
        }

        public override void Render()
        {
            DebugManager.Instance.DisplayStats();
        }

        public override void ProcessInput()
        {
            
        }

        public override void Dispose()
        {
            DebugManager.Instance.DisplayStats();
        }
    }
}
