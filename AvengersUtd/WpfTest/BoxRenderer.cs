using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AvengersUtd.Odyssey.Graphics.Rendering;
using SlimDX;
using AvengersUtd.Odyssey.Graphics.Meshes;
using AvengersUtd.Odyssey.Graphics.Rendering.Management;
using SlimDX.Direct3D11;
using AvengersUtd.Odyssey;
using AvengersUtd.Odyssey.UserInterface.Controls;
using AvengersUtd.Odyssey.UserInterface;

namespace WpfTest
{
    public class BoxRenderer :Renderer
    {

        public BoxRenderer(IDeviceContext deviceContext)
            : base(deviceContext)
        { }

        public override void Init()
        {

            Camera.LookAt(Vector3.Zero, new Vector3(0, 5, -10f));

            AvengersUtd.Odyssey.Graphics.Meshes.Grid grid = new AvengersUtd.Odyssey.Graphics.Meshes.Grid(50, 50, 8, 8);
            //grid.PositionV3 = new Vector3(-5, 5, 0);
            //Box plane = new Box(Vector3.Zero, 50, 1, 50);
            //Box box = new Box(1, 1, 1);
            Pyramid box = new Pyramid(1, 2, 1);

            RenderableNode rNodeGrid = new RenderableNode(grid);
            RenderableNode rNodeBox = new RenderableNode(box) { Label = "RBox" };
            FixedNode fNodeGrid = new FixedNode { Label = "fGrid", Position = grid.PositionV3 };

            CameraAnchorNode coNode = new CameraAnchorNode();
            Scene.Tree.RootNode.AppendChild(fNodeGrid);
            Scene.Tree.RootNode.AppendChild(coNode);
            fNodeGrid.AppendChild(rNodeGrid);
            fNodeGrid.AppendChild(rNodeBox);

            DeviceContext.Immediate.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleList;

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
            Game.Logger.Init();
            Hud.BeginDesign();


            Game.Logger.Log("Prova1");
            Game.Logger.Log("Prova2");
            Game.Logger.Log("Prova3");
            Game.Logger.Log("Prova4");
            Game.Logger.Log("Prova5");

            Button bConnect = new Button()
            {
                Size = new System.Drawing.Size(120, 30),
                //Content = "Start",
                Position = new Vector2(1800, 0)
            };

            Button bTracking = new Button()
            {
                Size = new System.Drawing.Size(120, 30),
                //Content = "Start",
                Position = new Vector2(1800, 40)
            };


            //bConnect.MouseClick += (sender, e) => { tracker.Connect(); };
            //bTracking.MouseClick += (sender, e) => { tracker.StartTracking(); };


            TouchRayPanel rp = new TouchRayPanel { Size = Hud.Size, Camera = this.Camera };
            Hud.Add(rp);

            rp.Add(bConnect);
            rp.Add(bTracking);
            //rp.SetTracker(tracker);

            Game.Logger.Activate();
            Hud.Init();
            Hud.EndDesign();

            Scene.BuildRenderScene();
            Hud.AddToScene(this, Scene);
            IsInited = true;
            //tracker.StartBrowsing();
        }

        public override void Render()
        {
            Game.Logger.Update();
            Scene.Display();
        }

        public override void ProcessInput()
        {
            //Camera.UpdateStates();
            Hud.ProcessKeyEvents();
        }

        protected override void OnDisposing(object sender, System.EventArgs e)
        {
            base.OnDisposing(sender, e);
            //tracker.DisconnectTracker();
        }
    }
}
