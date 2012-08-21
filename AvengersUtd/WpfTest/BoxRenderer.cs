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
using AvengersUtd.Odyssey.Graphics;

namespace WpfTest
{
    public class BoxRenderer : Renderer
    {
        public static DateTime startTime, endTime;
        public static int Session { get { return index; } }
        static TrackerWrapper tracker;
        FixedNode fNodeBox;
        BoundingBox bbox;
        Box box;
        ScalingWidget sWidget;
        private Label lCountDown;
        private Timer clock;
        private Stopwatch stopwatch;

        TouchRayPanel rp;
        private static int countdown = 3;
        static int index=0;
        private Button bNew;
        static bool started = false;
        bool startingNewSession;

        float[] frameSize;
        float[] boxSize;


        //private List<float[]> conditions = new List<float[]>
        //                                       {
        //                                           new[] {5f, 5f, 5f},
        //                                           new[] {5f, 5f, 1f},
        //                                           new[] {5f, 1f, 5f},
        //                                           //...
        //                                           new[] {1f, 5f, 5f},

        //                                       };

        //private List<float[]> boxSizes = new List<float[]>
        //{
        //                                           new[] {1f, 1f, 1f},
        //                                           new[] {1f, 1f, 5f},
        //                                           new[] {1f, 5f, 1f},
        //                                           //...
        //                                           new[] {5f, 1f, 1f}
        //};

        float[] offsets = new[] { 
            2.5f,
            2.5f,
            3.5f,
            2.5f,
            1.5f,
            2.5f,
            2.5f,
            2.5f,
            2.5f, //9
            2.5f,
            1.5f,
            3.5f,
            2.5f,
            2.5f,
            1.5f,
            2.5f,
            1.5f,
            1.5f, //18
            2.5f,
            2.5f,
            1.5f,
            2.5f,
            1.5f,
            1.5f,//24

        };

        private List<bool[]> arrowConditions = new List<bool[]>
        {
            new [] { true, true, true},
            new [] { true, true, false},
            new [] { true, false, true},
            new [] { true, false, false},
            new [] { false, true, true},
            new [] { false, true, false},
            new [] { false, false, true},
            new [] { false, false, false}
        };

        private List<int[]> conditions = new List<int[]>();

        private List<float[]> axes = new List<float[]> 
        {
            new [] { 0f, 0f, 0f},
            new [] { -15f, 30f, 0f},
            new [] {15f, 0f, -30}
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
        {
            InitConditions();
        }

        public void InitConditions()
        {

            for (int i = 0; i < 4; i++)
                for (int j = 0; j < arrowConditions.Count; j++)
                    for (int k = 0; k < axes.Count; k++)
                        conditions.Add(new int[] { i, j, k });
        }

        void NewSession()
        {
            int[] condition = conditions[index];
            bool[] arrowCondition = arrowConditions[condition[1]];

            frameSize = new float[]
            {
                arrowCondition[0] ? 5.0f : 1f,
                arrowCondition[1] ? 5.0f : 1f,
                arrowCondition[2] ? 5.0f : 1f
            };

            bbox = new BoundingBox(frameSize[0], frameSize[1], frameSize[2]);
            //bbox = new BoundingBox(2.5f);
            bbox.PositionV3 = new Vector3(0, bbox.Height / 2 - BoundingBox.DefaultThickness,  0);
            sWidget.SetFrame(bbox);
            bool[] arrowDirections = arrowConditions[condition[1]];
            sWidget.ChooseArrangement(arrowDirections[0], arrowDirections[1], arrowDirections[2]);
            fNodeBox.Position = sWidget.GetBoxOffset();

            startTime = DateTime.Now;
            TrackerEvent.BoxSessionStart.Log(Session, frameSize[0], frameSize[1], frameSize[2], condition[2], condition[1]);
            Camera.LookAt(new Vector3(0.5f, 0.5f, 0.5f) , new Vector3(-5.5f, 5.5f, -5.5f));
            Camera.PositionV3 += new Vector3(-bbox.Width / 2 + 0.5f, offsets[index%24], -bbox.Depth / 2 +0.5f);

            //Camera.PositionV3 += new Vector3(0, 1.5f + , 0);
        }

        void StartNew()
        {
            started = true;
            Hud.BeginDesign();

            bNew.Position = new Vector2(1930, 1090);
            Hud.EndDesign();
            rp.Reset();
#if TRACKER
            rp.SetTracker(tracker);
            tracker.Connect();
            tracker.StartTracking();
#endif
            
            stopwatch.Start();

        }

        private void Stop()
        {
            endTime = DateTime.Now;
            stopwatch.Stop();
            //bConnect.IsVisible = true;
            //bTracking.IsVisible = true;
            countdown = 3;
            bNew.Position=new Vector2(1760, 0);
#if TRACKER
            tracker.StopTracking();
#endif
            TrackerEvent.BoxSessionEnd.Log(Session, stopwatch.ElapsedMilliseconds/1000d);
            started = false;
            index++;
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
            //Camera.LookAt(new Vector3(3,0f, 3), new Vector3(-6.5f, 5.55f, -6.5f));
            int[] condition = conditions[index];
            bool[] arrowCondition = arrowConditions[condition[1]];
            boxSize = new float[]
            {
                arrowCondition[0] ? 1.0f : 5f,
                arrowCondition[1] ? 1.0f : 5f,
                arrowCondition[2] ? 1.0f : 5f
            };
            box = new Box(1, 1, 1);
            box.ScalingValues = new Vector3(boxSize[0], boxSize[1], boxSize[2]);
            sWidget = new ScalingWidget(box);

            RenderableNode rNodeBox = new RenderableNode(box) { Label = "RBox" };
            float[] axis = axes[condition[2]];
            
            FixedNode fNodeFrame = new FixedNode { 
                Label = "fGrid",
                Position = Vector3.Zero,
                Rotation = Quaternion.RotationYawPitchRoll(MathHelper.DegreesToRadians(axis[0]),
                MathHelper.DegreesToRadians(axis[1]),
                MathHelper.DegreesToRadians(axis[2]))
            };

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

           
            bNew = new Button()
            {
                Size = new System.Drawing.Size(120, 60),
                Content = "New Session",
                Position = new Vector2(1760, 0)
            };

            Button bSession = new Button()
            {
                Size = new System.Drawing.Size(120, 30),
                Content = "Next",
                Position = new Vector2(1760, 40)
            };
            bSession.MouseClick += (sender, e) =>
            {
                index++;
                BoxRenderer boxR = new BoxRenderer(Game.Context);
                Global.Window.Dispatcher.BeginInvoke(new Action(delegate { Game.ChangeRenderer(boxR); boxR.StartNew(); }));
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
            rp.SetAxis(axis);
            Hud.Add(rp);
            //rp.Add(bSession);
            //rp.Add(bConnect);
            //rp.Add(bTracking);
            //
            rp.Add(bNew);
            rp.Completed += (sender, e) => ((BoxRenderer)Game.CurrentRenderer).Stop();

            //bConnect.TouchUp += (sender, e) =>
            //{
            //    rp.SetTracker(tracker);
            //    tracker.Connect();
            //};
            //bTracking.MouseClick += (sender, e) =>

            bNew.TouchUp += delegate
                               {
                                   if (startingNewSession)
                                       return;
                                   startingNewSession = true;
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
            if (!started)
                Scene.Display(CommandType.Render);
            else
                Scene.Display();
        }

        public override void ProcessInput()
        {
            ////Camera.UpdateStates();
            Hud.ProcessKeyEvents();
        }

        protected override void OnDisposing(object sender, System.EventArgs e)
        {
            base.OnDisposing(sender, e);
            foreach (TraceListener tl in Trace.Listeners)
            {
                tl.Flush();
                tl.Dispose();
            }
#if TRACKER
            tracker.DisconnectTracker();
#endif
        }
    }
}
