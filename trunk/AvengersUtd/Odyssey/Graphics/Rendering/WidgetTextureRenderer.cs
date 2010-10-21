using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using AvengersUtd.Odyssey.UserInterface.Controls;
using AvengersUtd.Odyssey.UserInterface.Drawing;
using AvengersUtd.Odyssey.UserInterface.Style;
using SlimDX;
using SlimDX.Direct3D11;

namespace AvengersUtd.Odyssey.Graphics.Rendering
{
    public class WidgetTextureRenderer : Renderer
    {
        private readonly RenderToTextureCommand command;
        private float width;
        private float height;
        private BaseControl control;

        public Texture2D OutputTexture
        {
            get { return command.Texture; }
        }

        public WidgetTextureRenderer(int width, int height, DeviceContext11 deviceContext11) : base(deviceContext11)
        {
            this.width = width;
            this.height = height;
            Hud = Hud.FromDescription(Game.Context.Device,
                new HudDescription(
                    cameraEnabled: false,
                    width: width,
                    height: height,
                    zFar: Camera.FarClip,
                    zNear: Camera.NearClip,
                    multithreaded: false
                    ));
            Camera.ChangeScreenSize(width, height);
            command = new RenderToTextureCommand(width, height, Scene);

        }

        public GradientStop[] Gradient
        {
            get { return control.Description.FillShader.Gradient; }
            set {
                control.Description.FillShader.Gradient = value; 
                Hud.EndDesign();
            }
        }

        public override void Init()
        {
            
            Hud.BeginDesign();
            Button button = new Button
            {
                Size = new Size(128, 64)
            };
            button.BringToFront();
            button.Position = Layout.CenterControl(button, Hud);
            Hud.Controls.Add(button);
            Hud.EndDesign();
            Scene.BuildRenderScene();
            Hud.AddToScene(this,Scene);
            DeviceContext.Immediate.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleList;
            //Camera.Update();
            Scene.Update();

            control = button;
        }

        public void FreeResources()
        {
            command.Dispose();
        }

        public override void Render()
        {
            Scene.CommandManager.RunCommand(command);
        }

        public override void ProcessInput()
        {
            return;
        }

    }
}
