using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Timers;
using System.Windows.Documents;
using AvengersUtd.Odyssey.Graphics.Rendering;
using SlimDX;
using AvengersUtd.Odyssey.Graphics.Meshes;
using AvengersUtd.Odyssey.Graphics.Rendering.Management;
using SlimDX.Direct3D11;
using AvengersUtd.Odyssey;
using AvengersUtd.Odyssey.UserInterface.Controls;
using AvengersUtd.Odyssey.UserInterface;
using BoundingBox = AvengersUtd.Odyssey.Graphics.Meshes.BoundingBox;
using AvengersUtd.Odyssey.Geometry;

namespace WpfTest
{
    public class BoxRenderer : Renderer
    {
        static TrackerWrapper tracker;
        FixedNode fNodeBox;
        BoundingBox bbox;
        ScalingWidget sWidget;
        private Label lCountDown;
        private Timer clock;
        private Stopwatch stopwatch;

        TouchRayPanel rp;
        private static int countdown = 3;
        static int index;
        private Button bConnect, bNew, bTracking;


        private List<float[]> conditions = new List<float[]>
                                               {
                                                   new[] {2f, 2f, 2f},
                                                   new[] {2f, 2f, 3.5f},
                                                   new[] {2f, 2f, 5f},
                                                   new[] {2f, 3.5f, 2f},
                                                   new[] {2f, 3.5f, 3.5f},
                                                   new[] {2f, 3.5f, 5f},
                                                   new[] {2f, 5f, 2f},
                                                   new[] {2f, 5f, 3.5f,},
                                                   new[] {2f, 5f, 5f},

                                                   new[] {3.5f, 2f, 2f},
                                                   new[] {3.5f, 2f, 3.5f},
                                                   new[] {3.5f, 2f, 5f},
                                                   new[] {3.5f, 3.5f, 2f},
                                                   new[] {3.5f, 3.5f, 3.5f},
                                                   new[] {3.5f, 3.5f, 5f},
                                                   new[] {3.5f, 5f, 2f},
                                                   new[] {3.5f, 5f, 3.5f},
                                                   new[] {3.5f, 5f, 5f},

                                                   new[] {5f, 2f, 2f},
                                                   new[] {5f, 2f, 3.5f},
                                                   new[] {5f, 2f, 5f},
                                                   new[] {5f, 3.5f, 2f},
                                                   new[] {5f, 3.5f, 3.5f},
                                                   new[] {5f, 3.5f, 5f},
                                                   new[] {5f, 5f, 2f},
                                                   new[] {5f, 5f, 3.5f},
                                                   new[] {5f, 5f, 5f},

                                               };
        static BoxRenderer()
        {
#if TRACKER
            tracker = new TrackerWrapper();
            tracker.StartBrowsing();
            tracker.SetWindow(Global.Window);
#endif
        }


        public BoxRenderer(IDeviceContext deviceContext)
            : base(deviceContext)
        { }

        void NewSession()
        {

            float[] sizes = conditions[index];
            float width = sizes[0];
            float height = sizes[1];
            float depth = sizes[2];
            bbox = new BoundingBox(width, height, depth);
            //bbox = new BoundingBox(2.5f);
            bbox.PositionV3 = new Vector3(0, bbox.Height / 2 - BoundingBox.DefaultThickness,  0);
            sWidget.SetFrame(bbox);
            fNodeBox.Position = sWidget.GetBoxOffset();
            index++;
        }




        void StartNew()
        {
            Hud.BeginDesign();
            //Hud.Controls.Remove(lCountDown);
            bConnect.IsVisible = false;
            bTracking.IsVisible = false;
            bNew.IsVisible = false;
            Hud.EndDesign();
            rp.SetTracker(tracker);
            tracker.Connect();
            tracker.StartTracking();
            stopwatch.Start();
        }

        private void Stop()
        {
            stopwatch.Stop();
            bConnect.IsVisible = true;
            bTracking.IsVisible = true;
            bNew.IsVisible = true;
            countdown = 3;
        }


        public override void Init()
        {
            clock = new Timer() { Interval = 1000 };
            stopwatch = new Stopwatch();
            clock.Elapsed += delegate
                             {

                                 lCountDown.Content = (--countdown).ToString();
                                 if (countdown == 0)
                                 {
                                     BoxRenderer boxR = new BoxRenderer(Game.Context);
                                     Global.Window.Dispatcher.BeginInvoke(new Action(delegate { Game.ChangeRenderer(boxR);boxR.StartNew(); }));
                                     
                                     clock.Stop();
                                 }
                             };
            Camera.LookAt(new Vector3(3,0f, 3), new Vector3(-6.5f, 5.55f, -6.5f));

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
            
            Hud.BeginDesign();

            //Game.Logger.Init();
            //Game.Logger.Log("Prova1");
            //Game.Logger.Log("Prova2");
            //Game.Logger.Log("Prova3");
            //Game.Logger.Log("Prova4");
            //Game.Logger.Log("Prova5");

            bConnect = new Button()
            {
                Size = new System.Drawing.Size(120, 30),
                Content = "Connect",
                Position = new Vector2(1800, 0)
            };


            bTracking = new Button()
            {
                Size = new System.Drawing.Size(120, 30),
                Content = "Start",
                Position = new Vector2(1800, 40)
            };

            bNew = new Button()
            {
                Size = new System.Drawing.Size(120, 30),
                Content = "New Session",
                Position = new Vector2(1800, 80)
            };

            Button bStop = new Button()
                           {
                               Size = new System.Drawing.Size(256, 128),
                               Content = "Stop",
                               Position = new Vector2(0, 952),
                               TextDescriptionClass="Large"
                           };

            lCountDown = new Label()
                         {
                             Content = "3",
                             Position = new Vector2(800, 300),
                             IsVisible = false,
                             TextDescriptionClass = "Huge"
                         };

            rp = new TouchRayPanel { Size = Hud.Size, };//Camera = this.Camera };
            rp.SetScalingWidget(sWidget);
            rp.SetBox(box);
            rp.SetFrame((IBox)bbox);
            Hud.Add(rp);

            rp.Add(bConnect);
            rp.Add(bTracking);
            rp.Add(bNew);
            rp.Add(bStop);

            //bConnect.TouchUp += (sender, e) =>
            //{
            //    rp.SetTracker(tracker);
            //    tracker.Connect();
            //};
            //bTracking.MouseClick += (sender, e) =>
            bStop.TouchUp += delegate
                                {
                                    if (stopwatch.IsRunning) 
                                        Stop();
                                };
            bNew.TouchUp += delegate
                               {
                                   Hud.BeginDesign();
                                   lCountDown.IsVisible = true;
                                   Hud.Controls.Add(lCountDown);
                                   Hud.EndDesign();
                                   clock.Start();
                               };


            //Game.Logger.Activate();
            Hud.Init();
            Hud.EndDesign();

            Scene.BuildRenderScene();
            Hud.AddToScene(this, Scene);
            IsInited = true;
            
        }

        public override void Render()
        {
            //Game.Logger.Update();
            Scene.Display();
        }

        public override void ProcessInput()
        {
            ////Camera.UpdateStates();
            //Hud.ProcessKeyEvents();
        }

        protected override void OnDisposing(object sender, System.EventArgs e)
        {
            base.OnDisposing(sender, e);
#if TRACKER
            tracker.DisconnectTracker();
#endif
        }
    }
}
