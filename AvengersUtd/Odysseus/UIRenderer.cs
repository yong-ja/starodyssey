using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AvengersUtd.Odyssey;
using AvengersUtd.Odyssey.Graphics.Rendering;
using AvengersUtd.Odyssey.UserInterface.Controls;
using AvengersUtd.Odyssey.UserInterface.Style;
using AvengersUtd.Odyssey.UserInterface.Xml;
using SlimDX.Direct3D11;
using System.Reflection;
using System.Threading;

namespace AvengersUtd.Odysseus
{
    public class UIRenderer : Renderer
    {
        private bool designMode;

        public UIRenderer(DeviceContext11 deviceContext11) : base(deviceContext11)
        {}

        public bool DesignMode
        {
            get { return designMode; }
            set
            {
                if (designMode != value)
                {
                    designMode = value;

                    if (designMode)
                    {
                        Hud.MouseDown += SelectionRectangle.StartSelection;
                        Hud.MouseMove += SelectionRectangle.UpdateSelection;
                        Hud.MouseUp += SelectionRectangle.FinalizeSelection;
                    }
                    else
                    {
                        Hud.MouseDown -= SelectionRectangle.StartSelection;
                        Hud.MouseMove -= SelectionRectangle.UpdateSelection;
                        Hud.MouseUp -= SelectionRectangle.FinalizeSelection;
                        Hud.MouseClick += (HudMouseClick);
                    }
                }
            }
        }

        internal delegate void ConvertControlCallback();
        internal static Toolbox Toolbox { get; set; }
        internal static PropertyBox PropertyBox { get; set; }
        internal ControlSelector ControlSelector { get; private set; }
        internal SelectionRectangle SelectionRectangle { get; private set; }
        internal static Main Form { get; set; }

        static object controlLock = new object();
        void HudMouseClick(object sender, MouseEventArgs e)
        {
            BaseControl control = Hud.Find(e.Location);
            if (control == null)
            {
                ControlSelector.TargetControl = null;
                ControlSelector.IsVisible = false;
            }
            else if (ControlSelector.TargetControl == control)
            {
                return;
            }
            else
            {

                    ControlSelector.TargetControl = control;
                    ControlSelector.IsVisible = true;

                ConvertControl();
            }

        }

        void ConvertControl()
        {
            if (PropertyBox.InvokeRequired)
            {
                ConvertControlCallback c = ConvertControl;
                Form.Invoke(c);
            }
            else
            {
                Type xmlControlType = UIParser.GetWrapperTypeForControl(ControlSelector.TargetControl.GetType());

                XmlBaseControl xmlControl = (XmlBaseControl) Activator.CreateInstance(xmlControlType, ControlSelector.TargetControl);
                PropertyBox.SelectedControl = xmlControl;
            }
        }

        void SelectionRectangleSelectionFinalized(object sender, SelectionEventArgs e)
        {
            if (Toolbox.SelectedButton == null)
                return;

            Type type = (Type)Toolbox.SelectedButton.Tag;
            BaseControl newControl = (BaseControl)Activator.CreateInstance(type);
            newControl.Position = e.Position;
            newControl.Size = e.Size;
            newControl.CanRaiseEvents = false;
            ControlSelector.TargetControl = newControl;

            Hud.BeginDesign();
            Hud.Controls.Add(newControl);
            SelectionRectangle.IsVisible = false;
            ControlSelector.IsVisible = true;
            SelectionRectangle.BringToFront();
            Hud.EndDesign();

            //OdysseusForm.RenderPanel.Cursor = Cursors.Arrow;
            Toolbox.DeselectToolStripButton();
        }


        public override void Init()
        {
            StyleManager.LoadControlDescription("Odyssey ControlDescriptions.ocd");
            StyleManager.LoadTextDescription("Odyssey TextDescriptions.otd");
            StyleManager.LoadControlDescription("Odysseus ControlDescriptions.ocd");

            Hud = Hud.FromDescription(Game.Context.Device,
                new HudDescription(
                    cameraEnabled: false,
                    width: Game.Context.Settings.ScreenWidth,
                    height: Game.Context.Settings.ScreenHeight,
                    zFar: Game.CurrentRenderer.Camera.FarClip,
                    zNear: Game.CurrentRenderer.Camera.NearClip,
                    multithreaded: true
                    ));
            Hud.BeginDesign();
            Grid grid = new Grid {Size = Hud.Size};
            SelectionRectangle = new SelectionRectangle{IsVisible = false};
            ControlSelector = new ControlSelector { IsVisible = false };

            SelectionRectangle.OwnerGrid = ControlSelector.OwnerGrid = grid;
            SelectionRectangle.SelectionFinalized += SelectionRectangleSelectionFinalized;
            Scene.BuildRenderScene();

            Hud.Add(grid);
            Hud.Add(SelectionRectangle);
            Hud.Add(ControlSelector);
            grid.SendToBack();
            Hud.Init();
            Hud.EndDesign();
            
            Hud.AddToScene(Scene);

            ClearColor = Color.CornflowerBlue;
            
            DeviceContext.Immediate.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleList;
        }

        public override void Render()
        {
            Scene.Display();
        }

        public override void ProcessInput()
        {
            return;
        }


    }
}
