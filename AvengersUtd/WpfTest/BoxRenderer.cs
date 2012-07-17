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
    public class BoxRenderer : Renderer
    {
        TrackerWrapper tracker;  

        public BoxRenderer(IDeviceContext deviceContext)
            : base(deviceContext)
        { }

        public override void Init()
        {

                           

            Camera.LookAt(new Vector3(1,0, 1), new Vector3(-2.5f, 2.5f, -2.5f));

            Box box = new Box(1, 1, 1);
            Sphere sphere = new Sphere(1, 16);
            ScalingWidget sWidget = new ScalingWidget(box);
            AvengersUtd.Odyssey.Graphics.Meshes.BoundingBox bbox = new AvengersUtd.Odyssey.Graphics.Meshes.BoundingBox(2.5f);
            bbox.PositionV3 = new Vector3(bbox.Width / 2 - box.Width/2, bbox.Height/2 - box.Height/2, bbox.Depth/2 - box.Depth/2);
            sphere.PositionV3 = new Vector3(0f, 3f, 0);

            RenderableNode rNodeBox = new RenderableNode(box) { Label = "RBox" };
            FixedNode fNodeGrid = new FixedNode { Label = "fGrid", Position = Vector3.Zero };

            CameraAnchorNode coNode = new CameraAnchorNode();
            Scene.Tree.RootNode.AppendChild(fNodeGrid);
            Scene.Tree.RootNode.AppendChild(coNode);
            fNodeGrid.AppendChild(rNodeBox);
            fNodeGrid.AppendChild(bbox.ToBranch());


            FixedNode nWidget = sWidget.ToBranch();
            fNodeGrid.AppendChild(nWidget);

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
                Content = "Connect",
                Position = new Vector2(1800, 0)
            };


            Button bTracking = new Button()
            {
                Size = new System.Drawing.Size(120, 30),
                Content = "Start",
                Position = new Vector2(1800, 40)
            };


            

            TouchRayPanel rp = new TouchRayPanel { Size = Hud.Size, };//Camera = this.Camera };
            rp.SetScalingWidget(sWidget);
            rp.SetBox(box);
            Hud.Add(rp);

            rp.Add(bConnect);
            rp.Add(bTracking);

            bConnect.TouchUp += (sender, e) =>
            {
                tracker = new TrackerWrapper();
                rp.SetTracker(tracker);
                tracker.StartBrowsing();
                tracker.Connect();
            };
            bTracking.TouchUp += (sender, e) => { tracker.StartTracking(); };


            Game.Logger.Activate();
            Hud.Init();
            Hud.EndDesign();

            Scene.BuildRenderScene();
            Hud.AddToScene(this, Scene);
            IsInited = true;
            
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
            tracker.DisconnectTracker();
        }
    }
}
