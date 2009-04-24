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
using SlimDX;
using ContainerControl=AvengersUtd.Odyssey.UserInterface.RenderableControls.ContainerControl;


namespace AvengersUtd.Odysseus
{
    public class UIRenderer : Renderer
    {
        Hud hud;
        bool designMode;
        ControlSelector controlSelector;

        public static OdysseusForm OdysseusForm
        {
            get;
            set;
        }

        public BaseControl SelectedControl
        {
            get { return controlSelector.TargetControl; }
        }

        public SelectionRectangle SelectionRectangle { get; private set; }


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
                        hud.MouseDown += SelectionRectangle.StartSelection;
                        hud.MouseMove += SelectionRectangle.UpdateSelection;
                        hud.MouseUp += SelectionRectangle.FinalizeSelection;
                    }
                    else
                    {
                        hud.MouseDown -= SelectionRectangle.StartSelection;
                        hud.MouseMove -= SelectionRectangle.UpdateSelection;
                        hud.MouseUp -= SelectionRectangle.FinalizeSelection;
                        hud.MouseClick += (hud_MouseClick);
                    }
                }
            }
        }

        void hud_MouseClick(object sender, MouseEventArgs e)
        {
            BaseControl control = hud.Find(e.Location);
            if (control == null || controlSelector.TargetControl == control)
                return;

            controlSelector.TargetControl = control;
            controlSelector.IsVisible = true;
            
        }

        public UIRenderer()
        {
            StyleManager.LoadControlStyles("Odyssey ControlStyles.ocs");
            StyleManager.LoadControlStyles("Odysseus ControlStyles.ocs");
            StyleManager.LoadTextStyles("Odyssey TextStyles.ots");
            VideoSettings currentVideoSettings = EngineSettings.Video;
            hud = new Hud
                      {
                          Id = "CurrentForm",
                          Size = OdysseusForm.RenderPanel.Size
            };


            OdysseyUI.CurrentHud = hud;
            Grid grid = new Grid();
            hud.Add(grid);
        }

        public override void Init()
        {
            hud.BeginDesign();
            SelectionRectangle = new SelectionRectangle
                                     {
                                         Id = "Selector",
                                         Offset = new Vector2(OdysseusForm.RenderPanel.Location.X, 0)
                                     };
            SelectionRectangle.SelectionFinalized += selectionRectangle_SelectionFinalized;
            controlSelector = new ControlSelector
                                  {
                                      Id = "Handler",
                                      TargetControl = hud,
                                      IsVisible = false
                                  };

            hud.Add(SelectionRectangle);
            hud.Add(controlSelector);
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
            controlSelector.TargetControl = newControl;

            hud.BeginDesign();
            hud.Controls.Add(newControl);
            SelectionRectangle.IsVisible = false;
            controlSelector.IsVisible = true;
            hud.EndDesign();

            //OdysseusForm.RenderPanel.Cursor = Cursors.Arrow;
            OdysseusForm.DeselectToolStripButton();
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
