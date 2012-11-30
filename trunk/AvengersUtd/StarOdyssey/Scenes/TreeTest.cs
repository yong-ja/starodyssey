using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AvengersUtd.Odyssey.Graphics.Rendering;
using SlimDX;
using AvengersUtd.Odyssey.Graphics.Meshes;
using AvengersUtd.Odyssey.UserInterface.Drawing;
using AvengersUtd.Odyssey.UserInterface;
using AvengersUtd.Odyssey.Graphics.Rendering.Management;
using AvengersUtd.Odyssey.Graphics.Materials;
using AvengersUtd.Odyssey;
using AvengersUtd.Odyssey.UserInterface.Controls;
using SlimDX.Direct3D11;
using AvengersUtd.Odyssey.Geometry;

namespace AvengersUtd.StarOdyssey.Scenes
{
    public class TreeTest:Renderer
    {

        public TreeTest(AvengersUtd.Odyssey.IDeviceContext deviceContext) : base(deviceContext)
        {
        }

        Torus torus;
        protected override void OnInit(object sender, EventArgs e)
        {

            FixedNode fNode = new FixedNode();
            

            Camera.LookAt(Vector3.Zero, new Vector3(0, 0, -10f));
            DeviceContext.Immediate.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleList;

            torus = new Torus(5f, 0.25f, 256, 64);
            RenderableNode rNode = new RenderableNode(torus);
            fNode.AppendChild(rNode);


            Box box = new Box(1f);
            RenderableNode rNodeBox = new RenderableNode(box);
            //fNode.AppendChild(rNodeBox);

            //for (int i = 0; i < 10; i++)
            //{
            //    Quad quad = new Quad();
            //    quad.PositionV3 = new Vector3(0 + 1f * i, 0, -1f*i);

            //    RenderableNode rNode = new RenderableNode(quad);

            //    fNode.AppendChild(rNode);
            //}

            //for (int i = 0; i < 10; i++)
            //{
            //    Quad quad = new Quad();
            //    quad.PositionV3 = new Vector3(0 + 1f * i, 0.5f, -1.5f * i);
            //    ((IColorMaterial)quad.Material).DiffuseColor = new Color4(1f, 0, 0);

            //    RenderableNode rNode = new RenderableNode(quad);

            //    fNode.AppendChild(rNode);
            //}



            Scene.Tree.RootNode.AppendChild(fNode);
            Scene.BuildRenderScene();
            

            Hud = Hud.FromDescription(Game.Context.Device,
                new HudDescription(
                    width: Game.Context.Settings.ScreenWidth,
                    height: Game.Context.Settings.ScreenHeight,
                    zNear: Game.CurrentRenderer.Camera.NearClip,
                    zFar: Game.CurrentRenderer.Camera.FarClip,
                    cameraEnabled: true,
                    multithreaded: true
                    ));
            OdysseyUI.CurrentHud = Hud; 
            Game.Logger.Activate();
            Hud.BeginDesign();
            Hud.Init();
            Hud.EndDesign();
            
        }

        float delta;
        public override void Render()
        {
            delta += (float)MathHelper.TwoPi * 0.005f;
            torus.CurrentRotation = Quaternion.RotationAxis(Vector3.UnitX, delta);
            Game.Logger.Update();
            Scene.Display();
        }

        public override void ProcessInput()
        {
            Hud.ProcessKeyEvents();
        }
    }
}
