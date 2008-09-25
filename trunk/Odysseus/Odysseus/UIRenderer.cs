using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AvengersUtd.Odyssey;
using AvengersUtd.Odyssey.UserInterface.Helpers;
using AvengersUtd.Odyssey.UserInterface.RenderableControls;
using AvengersUtd.Odyssey.Settings;
using AvengersUtd.Odyssey.UserInterface;
using AvengersUtd.Odyssey.UserInterface.Style;


namespace AvengersUtd.Odysseus
{
    public class UIRenderer : Renderer
    {
        Hud hud;
        SelectionRectangle selectionRectangle;

        public UIRenderer()
        {
            StyleManager.LoadControlStyles("Odyssey ControlStyles.ocs");
            StyleManager.LoadTextStyles("Odyssey TextStyles.ots");
            VideoSettings currentVideoSettings = EngineSettings.Video;
            hud = new Hud
            {
                Id = "CurrentForm",
                Size = currentVideoSettings.ScreenSize
            };

            OdysseyUI.CurrentHud = hud;
        }
        public override void Init()
        {
            hud.BeginDesign();
            selectionRectangle = new SelectionRectangle
                                     {
                                         Id = "Selector"
                                     };
            hud.MouseDown += selectionRectangle.StartSelection;
            hud.MouseMove += selectionRectangle.UpdateSelection;
            hud.MouseUp += selectionRectangle.FinalizeSelection;
            
            hud.Add(selectionRectangle);
            hud.EndDesign();
        }

        public override void Render()
        {
            DebugManager.Instance.DisplayStats();
            hud.Render();
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
