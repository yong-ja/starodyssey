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
using BoundingBox = AvengersUtd.Odyssey.Graphics.Meshes.BoundingBox;

namespace WpfTest
{
    public class BoxRenderer : Renderer
    {
        TrackerWrapper tracker;
        FixedNode fNodeBox;
        BoundingBox bbox;
        ScalingWidget sWidget;
        Random rand;

        public BoxRenderer(IDeviceContext deviceContext)
            : base(deviceContext)
        { }

        void NewSession()
        {
            float width = (float)rand.NextDouble()*3 + 1.5f;
            float height = (float)rand.NextDouble() + 1.5f;
            float depth = (float)rand.NextDouble()*3 + 1.5f;
            bbox = new BoundingBox(width, height, depth);
            //bbox = new BoundingBox(2.5f);
            bbox.PositionV3 = new Vector3(0, bbox.Height / 2 - BoundingBox.DefaultThickness,  0);
            sWidget.SetFrame(bbox);
            fNodeBox.Position = sWidget.GetBoxOffset();
        }

        public override void Init()
        {
            rand = new Random();
            Camera.LookAt(new Vector3(1,0, 1), new Vector3(-4.5f, 3f, -4.5f));

            Box box = new Box(1, 1, 1);
             sWidget = new ScalingWidget(box);
           
            RenderableNode rNodeBox = new RenderableNode(box) { Label = "RBox" };
            FixedNode fNodeFrame = new FixedNode { Label = "fGrid", Position = Vector3.Zero };

            fNodeBox = new FixedNode
            {
                Label = "fBox",
            };

            NewSession();

            CameraAnchorNode coNode = new CameraAnchorNode();
            Scene.Tree.RootNode.AppendChild(fNodeFrame);
            Scene.Tree.RootNode.AppendChild(coNode);
            fNodeBox.AppendChild(rNodeBox);
            fNodeFrame.AppendChild(bbox.ToBranch());
            fNodeFrame.AppendChild(fNodeBox);


            FixedNode nWidget = sWidget.ToBranch();
            fNodeBox.AppendChild(nWidget);

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

            Button bNew = new Button()
            {
                Size = new System.Drawing.Size(120, 30),
                Content = "New Session",
                Position = new Vector2(1800, 80)
            };


            TouchRayPanel rp = new TouchRayPanel { Size = Hud.Size, };//Camera = this.Camera };
            rp.SetScalingWidget(sWidget);
            rp.SetBox(box);
            Hud.Add(rp);

            rp.Add(bConnect);
            rp.Add(bTracking);
            rp.Add(bNew);
            tracker = new TrackerWrapper();
            tracker.StartBrowsing();
            tracker.SetWindow(Global.Window);
            bConnect.TouchUp += (sender, e) =>
            {
                rp.SetTracker(tracker);
                tracker.Connect();
            };
            bTracking.TouchUp += (sender, e) => { tracker.StartTracking(); };
            bNew.TouchUp += (sender, e) => { Game.ChangeRenderer(new BoxRenderer(Game.Context)); };


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
