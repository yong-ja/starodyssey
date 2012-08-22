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
        static int index;
        static int participant;
        
        
        static int count;
        static int totalConditions;


        public static int Count { get { return count; } }
        public static int ConditionsCount { get { return totalConditions;}}
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

        private Button bNew;
        static bool started = false;
        bool startingNewSession;

        float[] frameSize;
        float[] boxSize;
   

        float[] offsets = new[] { 
            2.5f,
            3.5f,
            3.5f,

            2.5f,
            2.0f,
            3.5f,

            2.5f,
            3.5f,
            3.5f, //8

            2.5f,
            2.5f,
            4.0f,

            2.5f,
            3.5f,
            2.5f,

            2.5f,
            2.5f,
            2.5f, //17

            2.5f,
            3.5f,
            2.5f,

            2.5f,
            2.5f,
            2.5f,//23

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
            new [] { 15f, 0f, -30f},
            //new [] { -22.5f, 45f, 0f},
            //new [] {22.5f, 0f, -45}
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

            for (int i = 0; i < 2; i++)
                for (int j = 0; j < arrowConditions.Count; j++)
                    for (int k = 0; k < axes.Count; k++)
                        conditions.Add(new int[] { i, j, k });

            totalConditions = conditions.Count;
            //Participant, Rep, Axis, Arrow1, Arrow2, Arrow3, Time
            TrackerEvent.BoxSessionEnd.Log("Participant, Rep, Axis, Arrow1, Arrow2, Arrow3, Time\n");
        }

        void NewSession()
        {
            int[] condition = conditions[index % conditions.Count];
            bool[] arrowCondition = arrowConditions[condition[1]];

            frameSize = new float[]
            {
                arrowCondition[0] ? 5.0f : 1f,
                arrowCondition[1] ? 5.0f : 1f,
                arrowCondition[2] ? 5.0f : 1f
            };

            bbox = new BoundingBox(frameSize[0], frameSize[1], frameSize[2]);
            bbox.PositionV3 = new Vector3(0, bbox.Height / 2 - BoundingBox.DefaultThickness,  0);
            sWidget.SetFrame(bbox);
            bool[] arrowDirections = arrowConditions[condition[1]];
            sWidget.ChooseArrangement(arrowDirections[0], arrowDirections[1], arrowDirections[2]);
            fNodeBox.Position = sWidget.GetBoxOffset();

            startTime = DateTime.Now;
            TrackerEvent.BoxSessionStart.Log(Count, frameSize[0], frameSize[1], frameSize[2]);
            Camera.LookAt(new Vector3(0.5f, 0.5f, 0.5f) , new Vector3(-5.5f, 5.5f, -5.5f));
            Camera.PositionV3 += new Vector3(-bbox.Width / 2 + 0.5f, offsets[index%24], -bbox.Depth / 2 +0.5f);
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

        private void Stop(BoxEventArgs e)
        {
            endTime = DateTime.Now;
            stopwatch.Stop();
            countdown = 3;
            bNew.Position=new Vector2(1760, 0);
#if TRACKER
            tracker.StopTracking();
#endif
            //Participant, Rep, Axis, Arrow1, Arrow2, Arrow3, Time

            int[] condition = conditions[index % conditions.Count];
            bool[] arrowCondition = arrowConditions[condition[1]];
            TrackerEvent.BoxSessionEnd.Log(participant,
                condition[0], condition[2],
                arrowCondition[0] ? "Increasing" : "Decreasing",
                arrowCondition[1] ? "Increasing" : "Decreasing",
                arrowCondition[2] ? "Increasing" : "Decreasing",
                e.Duration);
            started = false;
            count++;
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
            int[] condition = conditions[index % conditions.Count];
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

            Button bStop = new Button
            {
                Size = new System.Drawing.Size(120, 60),
                Content = "Stop",
                Position = new Vector2(0, 1020)
            };
            bStop.TouchUp += (sender, e) =>
                {
                    stopwatch.Stop();
                    Stop(new BoxEventArgs(startTime, DateTime.Now));
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
            rp.Add(bStop);
            rp.Add(bNew);
            rp.Completed += (sender, e) => ((BoxRenderer)Game.CurrentRenderer).Stop(e);

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
