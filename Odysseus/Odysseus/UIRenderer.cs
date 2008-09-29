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
using System.Windows.Forms;


namespace AvengersUtd.Odysseus
{
    public class UIRenderer : Renderer
    {
        Hud hud;
        bool designMode;
        SelectionRectangle selectionRectangle;
        ControlSelector controlSelector;

        public OdysseusForm OdysseusForm
        {
            get;
            set;
        }


        public bool DesignMode
        {
            get { return designMode;}
            set
            {
                if (designMode != value)
                {
                    designMode = value;

                    if (designMode)
                    {
                        hud.MouseDown += selectionRectangle.StartSelection;
                        hud.MouseMove += selectionRectangle.UpdateSelection;
                        hud.MouseUp += selectionRectangle.FinalizeSelection;
                    }
                    else
                    {
                        hud.MouseDown -= selectionRectangle.StartSelection;
                        hud.MouseMove -= selectionRectangle.UpdateSelection;
                        hud.MouseUp -= selectionRectangle.FinalizeSelection;
                        hud.MouseClick += (hud_MouseClick);
                    }
                }
            }
        }

        void hud_MouseClick(object sender, MouseEventArgs e)
        {
            BaseControl control = OdysseyUI.FindControl(e.Location);
            if (control == null || controlSelector.TargetControl == control)
                return;

            controlSelector.TargetControl = control;

            hud.BeginDesign();
            hud.Add(controlSelector);
            hud.EndDesign();
        }

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
            selectionRectangle.SelectionFinalized += selectionRectangle_SelectionFinalized;
            controlSelector = new ControlSelector
                                  {
                                      Id = "Handler"
                                  };

            hud.Add(selectionRectangle);
            hud.EndDesign();
        }

        void selectionRectangle_SelectionFinalized(object sender, SelectionEventArgs e)
        {
            if (OdysseusForm.SelectedButton == null)
                return;

            Type type = (Type)OdysseusForm.SelectedButton.Tag;
            BaseControl newControl = (BaseControl)Activator.CreateInstance(type);
            newControl.Position = e.Position;
            newControl.Size = e.Size;
            newControl.CanRaiseEvents = false;
            hud.BeginDesign();
            hud.Controls.Add(newControl);
            hud.EndDesign();
            selectionRectangle.IsVisible = false;
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
