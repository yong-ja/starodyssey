using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AvengersUtd.Odyssey;
using AvengersUtd.Odyssey.UserInterface.Helpers;
using AvengersUtd.Odyssey.UserInterface.RenderableControls;
using AvengersUtd.StarOdyssey;
using SlimDX.Direct3D9;

namespace GUITest
{
    public class GuiRenderer : Renderer
    {
        Hud hud;

        public override void Init()
        {
            hud = GuiDesigner.MainMenuTest;
            Game.Input.IsInputEnabled = true;
        }

        public override void Render()
        {
 
            hud.Render();
            DebugManager.Instance.DisplayStats();
        }

        public override void ProcessInput()
        {
            return;
        }

        public override void Dispose()
        {
            return;
        }
    }
}
