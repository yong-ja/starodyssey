using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AvengersUtd.Odyssey.Graphics.Meshes;
using AvengersUtd.Odyssey.Graphics.Rendering;
using AvengersUtd.Odyssey.Graphics.Rendering.Management;
using AvengersUtd.Odyssey.UserInterface.Controls;
using SlimDX;
using SlimDX.Direct3D11;
using AvengersUtd.Odyssey;
using AvengersUtd.Odyssey.Utils.Logging;
using System.Windows;
using System.Windows.Input;
using AvengersUtd.Odyssey.UserInterface;
using System.Diagnostics.Contracts;
using AvengersUtd.Odyssey.Graphics.Materials;
using System.Drawing;
using System.Threading;
using AvengersUtd.Odyssey.Geometry;

namespace WpfTest
{

    public struct InputEvent
    {
        public DateTime TimeStamp { get; private set; }
        public float[] Progress { get; private set; }

        public InputEvent(DateTime timestamp, float[] progress) : this()
        {
            TimeStamp = timestamp;
            Progress = progress;
        }
    }
    public class TouchRayPanel : Panel
    {
        List<InputEvent> events = new List<InputEvent>();
        bool[] arrowConditions;

        bool lastX, lastY, lastZ;
       
        const string ControlTag = "TouchRayPanel";
        const int dwellInterval = 500;
        const float maxR = (ScalingWidget.ArrowIntersectionRadius * ScalingWidget.ArrowIntersectionRadius);
        readonly Window window;
        readonly Dictionary<TouchDevice, Vector2> points;
        readonly Dictionary<TouchDevice, IRenderable> arrows;
        float[] axis;
        Vector2 prevEyeLocation;
        private IRenderable box;
        private IRenderable eyeArrow;
        Vector3 startingSValues;
        static TrackerWrapper tracker;
        TexturedIcon crosshair;
        ScalingWidget sWidget;
        IBox frame;

        private IRenderable tempArrow;

        bool gazeOn;
        bool completed;
        bool gazeLock;
        bool xLock, yLock, zLock;
        
        EventWaitHandle dwellTime;
        DateTime dwellStart;
        
        static int count;
        Thread completetionCheck;

        public event EventHandler<BoxEventArgs> Completed;



        public TouchRayPanel() : base(ControlTag + ++count, "Empty")
        {
            completetionCheck = new Thread(Check);
            completetionCheck.SetApartmentState(ApartmentState.STA);
            completetionCheck.Start();
            points = new Dictionary<TouchDevice, Vector2>();
            arrows = new Dictionary<TouchDevice, IRenderable>();
            window = Global.Window;

            dwellTime = new EventWaitHandle(false, EventResetMode.ManualReset);

            crosshair = new TexturedIcon
            {
                CanRaiseEvents = false,
                Position = Vector2.Zero,
                Size = new System.Drawing.Size(64, 64),
                Texture = Texture2D.FromFile(Game.Context.Device, "Resources/Textures/crosshair.png")
            };
            
            Add(crosshair);
        }


        protected void OnCompleted(object sender, BoxEventArgs e)
        {
            DateTime end = DateTime.Now;
            Label label = new Label
            {
                Position = new Vector2(400, 300),
                TextDescriptionClass = "Large",
                Content = "Session complete\nTrial " + (Test.Count+1)
            };

            Test.Count++;

            if (Test.Count > 0 && Test.Count % 8 == 0) 
                label.Content += "\nPlease have a break.";


            if (Test.Count == BoxRenderer.ConditionsCount)
                label.Content = "Thanks, this task is now complete"; 

            completed = true;

            OdysseyUI.CurrentHud.BeginDesign();
            OdysseyUI.CurrentHud.Controls.Add(label);
            OdysseyUI.CurrentHud.EndDesign();

            BoxPerformance bp = new BoxPerformance();
            bp.SetData(BoxRenderer.startTime, end, events);

            bp.ShowDialog();

            if (Completed != null)
                Completed(sender, e);
        }

        public void SetFrame(IBox frame)
        {
            this.frame = frame;
        }

        public void SetGazeTracking(bool value)
        {
            gazeOn = value;
        }

        public void SetTracker(TrackerWrapper newTracker)
        {
#if TRACKER
            tracker = newTracker;
            tracker.GazeDataReceived += new EventHandler<GazeEventArgs>(tracker_GazeDataReceived);
#endif
        }

        void tracker_GazeDataReceived(object sender, GazeEventArgs e)
        {
            crosshair.Position = e.GazePoint;

            string gazeEvent;

            if (!gazeOn || yLock || completed)
                return;

            if (arrows.Count < 2)
                return;

            if (eyeArrow == null)
            {
                IRenderable tempArrow= sWidget.FindIntersection2D(e.GazePoint);
                if (tempArrow == null)
                {
                    // Session Id, gpX, gpY, valL, valR, GazeOn
                    gazeEvent = "GazeScreen";
                }
                else
                {
                    Arrow gazeArrow = (Arrow)tempArrow;

                    if (gazeArrow.Snapped)
                    {
                        return;
                    }
                    if (!gazeArrow.IsTouched)
                    {
                        gazeEvent = "GazeDwelling" + gazeArrow.Name;
                        TrackerEvent.ArrowDwell.Log(gazeArrow.Name);
                        sWidget.Select(gazeArrow.Name, Color.Orange);
                        gazeArrow.IsDwelling = true;
                        dwellStart = DateTime.Now;
                        eyeArrow = gazeArrow;

                    }
                    else
                        gazeEvent = "GazeDwellSelected" + gazeArrow.Name;
                }
            }
            else {
                TimeSpan delta = DateTime.Now.Subtract(dwellStart);
                IRenderable dwellCheckArrow = sWidget.FindIntersection2D(e.GazePoint);
                Arrow tempArrow = ((Arrow)dwellCheckArrow);
                if (delta.TotalMilliseconds < dwellInterval)
                {
                    gazeEvent = "GazeDwelling" + ((dwellCheckArrow == null) ? "Screen" : dwellCheckArrow.Name);
                }
                else if (tempArrow == eyeArrow && !tempArrow.IsTouched)
                {
                    gazeEvent = "GazeMove" + eyeArrow.Name;

                    if (!tempArrow.Snapped)
                    {
                        sWidget.SetColor(tempArrow, Color.Red);
                        EyeMoveArrow(e.GazePoint);
                        
                        if (!tempArrow.GazeLock)
                        {
                            TrackerEvent.ArrowMoveStart.Log(tempArrow.Name);
                            tempArrow.GazeLock = true;
                            gazeLock = true;
                        }
                    }
                    else sWidget.SetColor(tempArrow, Color.Green);
                }
                else
                {
                    //sWidget.ResetColors();
                    gazeEvent = "GazeLost";
                    DisableArrow(eyeArrow);
                    eyeArrow = null;
                }
            }

            prevEyeLocation = e.GazePoint;

            TrackerEvent.Gaze.Log(Test.BoxIndex % BoxRenderer.ConditionsCount, e.GazePoint.X, e.GazePoint.Y, e.LeftValid, e.RightValid, gazeEvent);


        }

        void DisableArrow(IRenderable arrow)
        {
            Arrow tempArrow = (Arrow)arrow;
            if (tempArrow != null)
            {
                tempArrow.IsDwelling = false;
                tempArrow.GazeLock = false;
                sWidget.SetColor(tempArrow, Color.Yellow);
                gazeLock = false;
            }
            else 
                LogEvent.Engine.Write("OH NO!");
        }

        void EyeMoveArrow(Vector2 gazePoint)
        {
            float delta = Vector2.Subtract(gazePoint, prevEyeLocation).LengthSquared();
            if (delta < maxR)
            {
                MoveArrow(gazePoint, eyeArrow, true);
            }
            else
            {
                DisableArrow(eyeArrow);
                eyeArrow = null;
            }
        }

        protected override void OnTouchDown(AvengersUtd.Odyssey.UserInterface.Input.TouchEventArgs e)
        {
            base.OnTouchDown(e);
            window.CaptureTouch(e.TouchDevice);

            IRenderable result;
            bool test = IntersectsArrow(sWidget, GetRay(e.Location), out result);
            if (test)
                TrackerEvent.ArrowIntersection.Log(result.Name, e.TouchDevice.Id);
            // Session Id, tpX, tpY, tdId, eventType
            TrackerEvent.Touch.Log(Test.BoxIndex % BoxRenderer.ConditionsCount, e.Location.X, e.Location.Y, e.TouchDevice.Id, "TouchDown");

            

            if (test)
            {
                arrows.Add(e.TouchDevice, result);
                if (!points.ContainsKey(e.TouchDevice))
                    points.Add(e.TouchDevice, e.Location);
                sWidget.Select(result.Name, Color.Cyan);
                Arrow arrow = (Arrow)result;
                arrow.IsTouched = true;
                if (arrows.Count == 2)
                    gazeOn = true;
            }

        }

        static Vector2 GetPosition(Vector3 position)
        {
            Vector3 screenSpace = Vector3.Project(position, 0, 0, Game.Context.Settings.ScreenWidth, Game.Context.Settings.ScreenHeight,
                Game.CurrentRenderer.Camera.NearClip, Game.CurrentRenderer.Camera.FarClip, Game.CurrentRenderer.Camera.WorldViewProjection);

            return new Vector2(screenSpace.X, screenSpace.Y);
        }

        static Ray GetRay(Vector2 location)
        {
            QuaternionCam camera = Game.CurrentRenderer.Camera;


            Viewport viewport = camera.Viewport;

            Vector3 pNear = Vector3.Unproject(new Vector3(location.X, location.Y, 0), viewport.X, viewport.Y, viewport.Width, viewport.Height, viewport.MinZ, viewport.MaxZ,
                              camera.WorldViewProjection);
            Vector3 pFar = Vector3.Unproject(new Vector3(location.X, location.Y, 1), viewport.X, viewport.Y, viewport.Width, viewport.Height, viewport.MinZ, viewport.MaxZ,
                              camera.WorldViewProjection);
            Ray r = new Ray(pNear, pFar - pNear);
            return r;
        }

        Vector3 GetIntersection(Vector2 location, Vector3 axis, IRenderable reference, out bool result)
        {
            QuaternionCam camera = Game.CurrentRenderer.Camera;
           

            Viewport viewport = camera.Viewport;

            Vector3 pNear = Vector3.Unproject(new Vector3(location.X, location.Y, 0), viewport.X, viewport.Y, viewport.Width, viewport.Height, viewport.MinZ, viewport.MaxZ,
                              camera.WorldViewProjection);
            Vector3 pFar = Vector3.Unproject(new Vector3(location.X, location.Y, 1), viewport.X, viewport.Y, viewport.Width, viewport.Height, viewport.MinZ, viewport.MaxZ,
                              camera.WorldViewProjection);
            Ray r = new Ray(pNear, pFar - pNear);
            Plane p = new Plane(reference.AbsolutePosition, axis);

            float distance;
            result = Ray.Intersects(r, p, out distance);
            Vector3 pIntersection = r.Position + distance * Vector3.Normalize(r.Direction);

            return pIntersection;
        }

        bool IntersectsArrow(ScalingWidget sWidget, Ray r, out IRenderable result)
        {
            result = sWidget.FindIntersection(r);
            if (result == null)
                return false;
            else 
                return true;

        }

        protected override void OnTouchUp(AvengersUtd.Odyssey.UserInterface.Input.TouchEventArgs e)
        {
            base.OnTouchUp(e);
            if (arrows.ContainsKey(e.TouchDevice))
                ((Arrow)arrows[e.TouchDevice]).IsTouched = false;
            arrows.Remove(e.TouchDevice);
            window.ReleaseTouchCapture(e.TouchDevice);
           
            IRenderable result;
            TrackerEvent.Touch.Log(Test.BoxIndex % BoxRenderer.ConditionsCount, e.Location.X, e.Location.Y, e.TouchDevice.Id, "TouchUp");
            bool test = IntersectsArrow(sWidget, GetRay(e.Location), out result);

            if (test)
            {
                Arrow tempArrow = (Arrow)result;
                TrackerEvent.ArrowIntersection.Log(result.Name, e.TouchDevice.Id);
                if (points.ContainsKey(e.TouchDevice))
                    points.Remove(e.TouchDevice);
                if (!tempArrow.Snapped)
                    sWidget.SetColor(tempArrow, Color.Yellow);
            }
        }

        void Check()
        {
            const float snapRange = 0.25f;
            while (!completed)
            {
                if (box == null)
                {
                    Thread.Sleep(100);
                    continue;
                }

                float[] progress = GetArrowProgress();
                if (progress[0]!=0 || progress[1] !=0 || progress[2] !=0)
                    events.Add(new InputEvent(DateTime.Now, progress));

                float yOffset = Math.Abs(frame.Height - box.ScalingValues.Y);
                float xOffset = Math.Abs(frame.Width - box.ScalingValues.X);
                float zOffset = Math.Abs(frame.Depth - box.ScalingValues.Z);

                float totalOffset = xOffset + yOffset + zOffset;
                if (totalOffset <= snapRange)
                    completed = true;
                else
                    Thread.Sleep(25);
            }

            OnCompleted(this, new BoxEventArgs(BoxRenderer.startTime, DateTime.Now,false));
        }


        void MoveArrow(Vector2 location, IRenderable arrow, bool eyeMove = false)
        {
            if (completed) 
                return;
            if (arrows.Count < 2)
                return;
            if (!gazeLock)
                return;

            IRenderable arrowHead = ((MeshGroup)arrow).Objects[0];
            const float minSize = 1f;
            const float maxSizeY = 5f;
            const float maxSizeZ = 5f;
            const float maxSizeX = 5f;
            const float touchSnapRange = 0.25f;
            const float eyeSnapRange = 0.25f;
            Matrix rotation = Matrix.RotationYawPitchRoll(MathHelper.DegreesToRadians(axis[0]),
                MathHelper.DegreesToRadians(axis[1]),
                MathHelper.DegreesToRadians(axis[2]));
            Matrix rotationInv = Matrix.Invert(rotation);

            Vector3 pIntersection;
            Vector3 tAxis;
            Vector3 pTransform;
            Vector3 vPosRot = Vector3.Transform(box.AbsolutePosition, rotationInv).ToVector3();
            FixedNode fNode = (FixedNode)arrowHead.ParentNode.Parent;
            float delta;
            bool test;

            float snapRange = eyeMove ? eyeSnapRange : touchSnapRange;
            switch (arrow.Name)
            {
                case "YArrow":
                    tAxis = Vector3.Transform(-Vector3.UnitZ, rotation).ToVector3();
                    pIntersection = GetIntersection(location, -Vector3.UnitZ, arrowHead, out test);
                    pTransform = Vector3.Transform(pIntersection, rotationInv).ToVector3();

                    if (test)
                    {
                        delta = pTransform.Y - (vPosRot.Y + box.ScalingValues.Y / 2);

                        if (sWidget.YInverted)
                            delta = Math.Min(maxSizeY, delta);
                        else
                            delta = Math.Min(minSize, delta);

                        box.ScalingValues += new Vector3(0, delta, 0);
                        float axisOffset = sWidget.GetBoxOffset().Y;
                        if (box.ScalingValues.Y < minSize)
                        {
                            box.ScalingValues = new Vector3(box.ScalingValues.X, minSize, box.ScalingValues.Z);
                            fNode.Position = new Vector3(fNode.Position.X, (box.PositionV3.Y + box.ScalingValues.Y / 2), fNode.Position.Z);
                        }
                        else if (box.ScalingValues.Y > maxSizeY)
                        {
                            box.ScalingValues = new Vector3(box.ScalingValues.X, maxSizeY, box.ScalingValues.Z);
                            fNode.Position = new Vector3(fNode.Position.X, (box.PositionV3.Y + box.ScalingValues.Y / 2), fNode.Position.Z);
                        }
                        else if (Math.Abs(frame.Height - box.ScalingValues.Y) <= snapRange)
                        {
                            box.ScalingValues = new Vector3(box.ScalingValues.X, frame.Height, box.ScalingValues.Z);
                            fNode.Position = new Vector3(fNode.Position.X, (box.PositionV3.Y + box.ScalingValues.Y / 2), fNode.Position.Z);
                            yLock = true;
                            gazeLock = true;
                            Arrow yArrow = sWidget.SelectByName("YArrow");
                            sWidget.SetColor(yArrow, Color.Green);
                            yArrow.Snapped = true;
                            if (eyeMove)
                            {
                                TrackerEvent.ArrowLock.Log(yArrow.Name, "Gaze");
                                eyeArrow = null;
                            }
                            else
                                TrackerEvent.ArrowLock.Log(yArrow.Name, "Touch");
                        }
                        else
                            fNode.Position = new Vector3(fNode.Position.X, pTransform.Y - axisOffset, fNode.Position.Z);

                        box.PositionV3 = new Vector3(box.PositionV3.X,box.ScalingValues.Y / 2 - startingSValues.Y/2, box.PositionV3.Z);
                    }
                    break;

                case "XArrow":
                    tAxis = Vector3.Transform(-Vector3.UnitZ, rotation).ToVector3();
                    pIntersection = GetIntersection(location, -Vector3.UnitZ, arrowHead, out test);
                    pTransform = Vector3.Transform(pIntersection, rotationInv).ToVector3();

                    if (test)
                    {
                        delta = //sWidget.XInverted ? - pIntersection.X + (box.AbsolutePosition.X - box.ScalingValues.X/2) :
                            pTransform.X - (vPosRot.X + box.ScalingValues.X / 2);

                        if (sWidget.XInverted)
                            delta = Math.Min(maxSizeX, delta);
                        else
                            delta = Math.Min(minSize, delta);

                        box.ScalingValues += new Vector3(delta, 0, 0);
                        float axisOffset = sWidget.GetBoxOffset().X;
                        if (box.ScalingValues.X < minSize)
                        {
                            box.ScalingValues = new Vector3(minSize, box.ScalingValues.Y, box.ScalingValues.Z);
                            fNode.Position = new Vector3((box.PositionV3.X + box.ScalingValues.X / 2), fNode.Position.Y, fNode.Position.Z);
                        }
                        else if (box.ScalingValues.X > maxSizeX)
                        {
                            box.ScalingValues = new Vector3(maxSizeX, box.ScalingValues.Y, box.ScalingValues.Z);
                            fNode.Position = new Vector3((box.PositionV3.X + box.ScalingValues.X / 2), fNode.Position.Y, fNode.Position.Z);
                        }
                        else if (Math.Abs(frame.Width - box.ScalingValues.X) <= snapRange)
                        {
                            box.ScalingValues = new Vector3(frame.Width, box.ScalingValues.Y, box.ScalingValues.Z);
                            fNode.Position = new Vector3((box.PositionV3.X + box.ScalingValues.X / 2), fNode.Position.Y, fNode.Position.Z);
                            xLock = true;
                            Arrow xArrow = sWidget.SelectByName("XArrow");
                            sWidget.SetColor(xArrow, Color.Green);
                            xArrow.Snapped = true;
                            if (eyeMove)
                            {
                                TrackerEvent.ArrowLock.Log(xArrow.Name, "Gaze");
                                eyeArrow = null;
                            }
                            else
                                TrackerEvent.ArrowLock.Log(xArrow.Name, "Touch");
                        }
                        else
                        {
                            fNode.Position = new Vector3(pTransform.X - axisOffset, fNode.Position.Y, fNode.Position.Z);
                            //if (sWidget.XInverted)
                            //{
                            //    FixedNode fNodeZ = (FixedNode)sWidget.SelectByName("ZArrow").Objects[0].ParentNode.Parent;
                            //    FixedNode fNodeY = (FixedNode)sWidget.SelectByName("YArrow").Objects[0].ParentNode.Parent;
                            //    fNodeZ.Position = new Vector3(fNode.Position.X, fNodeZ.Position.Y, fNodeZ.Position.Z);
                            //    fNodeY.Position = new Vector3(fNode.Position.X, fNodeY.Position.Y, fNodeY.Position.Z);
                            //}
                        }
                        box.PositionV3 = 
                            //sWidget.XInverted ?
                        //    new Vector3(-(box.ScalingValues.X / 2 - 0.5f), box.PositionV3.Y, box.PositionV3.Z) :
                            new Vector3(box.ScalingValues.X / 2 - startingSValues.X/2, box.PositionV3.Y, box.PositionV3.Z);
                    }
                    break;

                case "ZArrow":

                    tAxis = Vector3.Transform(Vector3.UnitY, rotation).ToVector3();
                    pIntersection = GetIntersection(location, tAxis, arrowHead, out test);
                    pTransform = Vector3.Transform(pIntersection, Matrix.Invert(rotation)).ToVector3();
       
                    if (test)
                    {
                        delta = pTransform.Z - (vPosRot.Z + box.ScalingValues.Z/2);// * box.ScalingValues.Z) ;
                        if (sWidget.ZInverted)
                            delta = Math.Min(maxSizeZ, delta);
                        else
                            delta = Math.Min(minSize, delta);

                        //if (zLock)
                        //    return;

                        //zLock = false;

                        box.ScalingValues += new Vector3(0, 0, delta);
                        float axisOffset = sWidget.GetBoxOffset().Z;
                        if (box.ScalingValues.Z < minSize)
                        {
                            box.ScalingValues = new Vector3(box.ScalingValues.X, box.ScalingValues.Y, minSize);
                            fNode.Position = new Vector3(fNode.Position.X, fNode.Position.Y, (box.PositionV3.Z + box.ScalingValues.Z / 2));
                        }
                        else if (box.ScalingValues.Z > maxSizeZ)
                        {
                            box.ScalingValues = new Vector3(box.ScalingValues.X, box.ScalingValues.Y, maxSizeZ);
                            fNode.Position = new Vector3(fNode.Position.X, fNode.Position.Y, (box.PositionV3.Z + box.ScalingValues.Z / 2));
                        }
                        else if (Math.Abs(frame.Depth - box.ScalingValues.Z) <= snapRange)
                        {
                            box.ScalingValues = new Vector3(box.ScalingValues.X, box.ScalingValues.Y, frame.Depth);
                            fNode.Position = new Vector3(fNode.Position.X, fNode.Position.Y, (box.PositionV3.Z + box.ScalingValues.Z / 2));
                            zLock = true;
                            Arrow zArrow = sWidget.SelectByName("ZArrow");
                            sWidget.SetColor(zArrow, Color.Green);
                            zArrow.Snapped = true;
                            if (eyeMove)
                            {
                                TrackerEvent.ArrowLock.Log(zArrow.Name, "Gaze");
                                eyeArrow = null;
                            }
                            else
                                TrackerEvent.ArrowLock.Log(zArrow.Name, "Touch");
                        }
                        else
                            fNode.Position = new Vector3(fNode.Position.X, fNode.Position.Y, pTransform.Z - axisOffset);
                        box.PositionV3 = new Vector3(box.PositionV3.X, box.PositionV3.Y, box.ScalingValues.Z / 2 - startingSValues.Z/2);
                    }
                    break;

            }
        }

        float[] GetArrowProgress()
        {

            float currentProgress;

            float[] progress = new float[3];

            bool arrowX = arrowConditions[0];
            bool arrowY = arrowConditions[1];
            bool arrowZ = arrowConditions[2];

            if (arrowX)
                currentProgress = MathHelper.Clamp(box.ScalingValues.X, startingSValues.X, frame.Width);
            else
                currentProgress = MathHelper.Clamp(box.ScalingValues.X, frame.Width, startingSValues.X);

            if (!lastX)
            {
                progress[0] = 
                    Math.Abs(currentProgress - startingSValues.X) / Math.Abs(frame.Width - startingSValues.X);
                if (progress[0] == 1)
                    lastX = true;
            }
            else progress[0] = 0;

            if (arrowY)
                currentProgress = MathHelper.Clamp(box.ScalingValues.Y, startingSValues.Y, frame.Height);
            else
                currentProgress = MathHelper.Clamp(box.ScalingValues.Y, frame.Height, startingSValues.Y);
            

            if (!lastY)
            {
                progress[1] = Math.Abs(currentProgress - startingSValues.Y) / Math.Abs(frame.Height - startingSValues.Y);
                if (progress[1] == 1)
                    lastY = true;
            }
            else progress[1] = 0;

            if (arrowZ)
                currentProgress = MathHelper.Clamp(box.ScalingValues.Z, startingSValues.Z, frame.Depth);
            else
                currentProgress = MathHelper.Clamp(box.ScalingValues.Z, frame.Depth, startingSValues.Z);

            if (!lastZ)
            {
                progress[2] = Math.Abs(currentProgress - startingSValues.Z) / Math.Abs(frame.Depth - startingSValues.Z);
                if (progress[2] == 1)
                    lastZ = true;
            }
            else progress[2] = 0;
            return progress;
        }

        protected override void OnTouchMove(AvengersUtd.Odyssey.UserInterface.Input.TouchEventArgs e)
        {
            base.OnTouchMove(e);

            if (!arrows.ContainsKey(e.TouchDevice) || completed)
                return;

            IRenderable arrow = arrows[e.TouchDevice];
            TrackerEvent.Touch.Log(Test.BoxIndex % BoxRenderer.ConditionsCount, e.Location.X, e.Location.Y, e.TouchDevice.Id, "TouchMove");
            MoveArrow(e.Location, arrow);
            if (points.ContainsKey(e.TouchDevice))
            {
                Vector2 oldLocation = points[e.TouchDevice];
                Vector2 delta = Vector2.Subtract(e.Location, oldLocation);
                points[e.TouchDevice] = e.Location;
            }
        }



        protected override void OnMouseDown(AvengersUtd.Odyssey.UserInterface.Input.MouseEventArgs e)
        {
            base.OnMouseDown(e);
            IRenderable result;
            bool test = IntersectsArrow(sWidget, GetRay(e.Location), out result);

            if (test)
            {
                tempArrow = result;
                IRenderable arrowHead = ((MeshGroup)result).Objects[0];
                Vector2 p = GetPosition(arrowHead.AbsolutePosition);
                LogEvent.UserInterface.Write(string.Format("Projected P({0:f2},{1:f2})", p.X, p.Y));
                LogEvent.UserInterface.Write("MouseDown [" + result.Name + "]");
            }
            else
                LogEvent.UserInterface.Write("MouseDown [No Intersection]");
        }

        protected override void OnMouseMove(AvengersUtd.Odyssey.UserInterface.Input.MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (tempArrow == null)
                return;
            IRenderable arrowHead = ((MeshGroup) tempArrow).Objects[0];
            MoveArrow(e.Location, tempArrow);
        }

        protected override void OnMouseUp(AvengersUtd.Odyssey.UserInterface.Input.MouseEventArgs e)
        {
            base.OnMouseUp(e);
            tempArrow = null;
        }

        public void SetScalingWidget(ScalingWidget sWidget)
        {
            this.sWidget = sWidget;
        }

        public void SetBox(IRenderable box)
        {
            this.box = box;
            startingSValues = box.ScalingValues;
        }

        public void Reset()
        {
            eyeArrow = null;
            xLock = yLock = zLock = false;
            gazeOn = false;
            gazeLock = false;
            lastX = lastY = lastZ;
        }


        internal void SetAxis(float[] axis)
        {
            this.axis = axis;
        }

        internal void SetArrowConditions(bool[] arrowConditions)
        {
            this.arrowConditions = arrowConditions;
        }
    }
}
