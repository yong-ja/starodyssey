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
    public class TouchRayPanel : Panel
    {
        const string ControlTag = "TouchRayPanel";
        const int dwellInterval = 500;
        const float maxR = (ScalingWidget.ArrowIntersectionRadius * ScalingWidget.ArrowIntersectionRadius)/2;
        readonly Window window;
        readonly Dictionary<TouchDevice, Vector3> points;
        readonly Dictionary<TouchDevice, IRenderable> arrows;
        Vector2 prevEyeLocation;
        private IRenderable box;
        private IRenderable eyeArrow;
        static TrackerWrapper tracker;
        TexturedIcon crosshair;
        ScalingWidget sWidget;
        IBox frame;

        bool gazeOn;
        bool xLock, yLock, zLock;
        
        EventWaitHandle dwellTime;
        Thread dwellThread;

        DateTime dwellStart;
        
        static int count;

        public event EventHandler<EventArgs> Completed;



        public TouchRayPanel() : base(ControlTag + ++count, "Empty")
        {
            points = new Dictionary<TouchDevice, Vector3>();
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

        protected void OnCompleted(object sender, EventArgs e)
        {
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

        void DwellLoop()
        {
            Thread.Sleep(200);
            dwellTime.Set();
        }

        public void SetTracker(TrackerWrapper newTracker)
        {
            tracker = newTracker;
            tracker.GazeDataReceived += new EventHandler<GazeEventArgs>(tracker_GazeDataReceived);
        }

        void tracker_GazeDataReceived(object sender, GazeEventArgs e)
        {
           
            crosshair.Position = e.GazePoint;

            if (!gazeOn)
                return;


            if (eyeArrow == null)
            {
                IRenderable tempArrow= sWidget.FindIntersection2D(e.GazePoint);
                if (tempArrow == null)
                    return;
                else
                {
                    Arrow gazeArrow = (Arrow)tempArrow;
                    gazeArrow.IsDwelling = true;
                    sWidget.ResetColors();
                    sWidget.Select(gazeArrow.Name, Color.Orange);
                    dwellStart = DateTime.Now;
                    eyeArrow = gazeArrow;
                }
            }
            else {
                TimeSpan delta = DateTime.Now.Subtract(dwellStart);
                if (delta.TotalMilliseconds < dwellInterval)
                    return;
                IRenderable dwellCheckArrow= sWidget.FindIntersection2D(e.GazePoint);

                if (dwellCheckArrow == eyeArrow && !((Arrow)dwellCheckArrow).IsSelected)
                    {
                        EyeMoveArrow(e.GazePoint);
                        sWidget.Select(dwellCheckArrow.Name, Color.Red);
                    }
                else {
                    sWidget.ResetColors();
                    ((Arrow)eyeArrow).IsDwelling = false;
                    eyeArrow = null;
                }
            }


            
            // we are looking at one of the arrows 
            // inititate dwell time check
                

            //dwellThread = new Thread(DwellLoop) { Name = "DwellThread" };
            //dwellThread.Start();
            //dwellTime.WaitOne(250);
            

        }

        void EyeMoveArrow(Vector2 gazePoint)
        {
            float delta = Vector2.Subtract(gazePoint, prevEyeLocation).LengthSquared();
            if (delta < maxR)
            {
                MoveArrow(gazePoint, eyeArrow, true);
                //LogEvent.UserInterface.Write(string.Format("Delta: {0:f2}", delta));
            }
            else
            {
                LogEvent.UserInterface.Write(string.Format("Delta: {0:f2}", delta));
                eyeArrow = null;
            }

            prevEyeLocation = gazePoint;
        }

        protected override void OnTouchDown(AvengersUtd.Odyssey.UserInterface.Input.TouchEventArgs e)
        {
            base.OnTouchDown(e);
            window.CaptureTouch(e.TouchDevice);

            IRenderable result;
            bool test = IntersectsArrow(sWidget, GetRay(e.Location), out result);

            if (test)
            {
                arrows.Add(e.TouchDevice, result);
                sWidget.Select(result.Name, Color.Cyan);
                Arrow arrow = (Arrow)result;
                arrow.IsSelected = true;
                if (arrows.Count == 2)
                    gazeOn = true;
                //LogEvent.UserInterface.Write("TouchDown: " + e.TouchDevice.Id + " [" + result.Name+"]");
            }
            else
                LogEvent.UserInterface.Write("TouchDown: " + e.TouchDevice.Id + " No intersection");

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
            arrows.Remove(e.TouchDevice);
            window.ReleaseTouchCapture(e.TouchDevice);
            IRenderable result;
            bool test = IntersectsArrow(sWidget, GetRay(e.Location), out result);

            if (test)
            {
                ((Arrow)result).IsSelected = false;
                sWidget.Select(result.Name, Color.Yellow);
                LogEvent.UserInterface.Write("Deselected " + result.Name);
            }
            //LogEvent.UserInterface.Write("TouchUp " + e.TouchDevice.Id);

        }

        void MoveArrow(Vector2 location, IRenderable arrow, bool eyeMove = false)
        {
            IRenderable arrowHead = ((MeshGroup)arrow).Objects[0];
            const float minSize = 1f;
            const float maxSizeY = 2.5f;
            const float maxSizeZ = 5f;
            const float maxSizeX = 5f;
            const float touchSnapRange = 0.1f;
            const float eyeSnapRange = 0.25f;
            Vector3 pIntersection;
            FixedNode fNode = (FixedNode)arrowHead.ParentNode.Parent;
            float delta;
            bool test;

            float snapRange = eyeMove ? eyeSnapRange : touchSnapRange;
            switch (arrow.Name)
            {
                case "YArrow":
                    pIntersection = GetIntersection(location, -Vector3.UnitZ, arrowHead, out test);
                    if (test)
                    {
                        delta = pIntersection.Y - (box.AbsolutePosition.Y + box.ScalingValues.Y / 2);
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
                        }
                        else
                            fNode.Position = new Vector3(fNode.Position.X, -axisOffset + pIntersection.Y, fNode.Position.Z);

                        box.PositionV3 = new Vector3(box.PositionV3.X,box.ScalingValues.Y / 2 - 0.5f, box.PositionV3.Z);
                    }
                    break;

                case "XArrow":
                    pIntersection = GetIntersection(location, -Vector3.UnitZ, arrowHead, out test);
                    if (test)
                    {
                        delta = sWidget.XInverted ? - pIntersection.X + (box.AbsolutePosition.X - box.ScalingValues.X/2) :
                            pIntersection.X - (box.AbsolutePosition.X + box.ScalingValues.X / 2);
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

                        }
                        else
                        {
                            fNode.Position = new Vector3(pIntersection.X - axisOffset, fNode.Position.Y, fNode.Position.Z);
                            if (sWidget.XInverted)
                            {
                                FixedNode fNodeZ = (FixedNode)sWidget.SelectByName("ZArrow").Objects[0].ParentNode.Parent;
                                FixedNode fNodeY = (FixedNode)sWidget.SelectByName("YArrow").Objects[0].ParentNode.Parent;
                                fNodeZ.Position = new Vector3(fNode.Position.X, fNodeZ.Position.Y, fNodeZ.Position.Z);
                                fNodeY.Position = new Vector3(fNode.Position.X, fNodeY.Position.Y, fNodeY.Position.Z);
                            }
                        }
                        box.PositionV3 = sWidget.XInverted ?
                            new Vector3(-(box.ScalingValues.X / 2 - 0.5f), box.PositionV3.Y, box.PositionV3.Z) :
                            new Vector3(box.ScalingValues.X / 2 - 0.5f, box.PositionV3.Y, box.PositionV3.Z);
                    }
                    break;

                case "ZArrow":
                    pIntersection = GetIntersection(location, Vector3.UnitY, arrowHead, out test);
                    if (test)
                    {
                        delta = pIntersection.Z - (box.AbsolutePosition.Z + box.ScalingValues.Z / 2);
                        delta = Math.Min(minSize, delta);

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
                        }
                        else
                            fNode.Position = new Vector3(fNode.Position.X, fNode.Position.Y, pIntersection.Z - axisOffset);
                        box.PositionV3 = new Vector3(box.PositionV3.X, box.PositionV3.Y, box.ScalingValues.Z / 2 - 0.5f);
                    }
                    break;

            }


            if (xLock && yLock && zLock)
            {
                OnCompleted(this, EventArgs.Empty);
            }

            if (eyeMove && (xLock || yLock  || zLock))
                eyeArrow = null;
        }

        protected override void OnTouchMove(AvengersUtd.Odyssey.UserInterface.Input.TouchEventArgs e)
        {
            base.OnTouchMove(e);

            if (!arrows.ContainsKey(e.TouchDevice))
                return;

            IRenderable arrow = arrows[e.TouchDevice];
            MoveArrow(e.Location, arrow);
        }

        private IRenderable tempArrow;


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
        }

        public void Reset()
        {
            eyeArrow = null;
            xLock = yLock = zLock = false;
            gazeOn = false;
        }

    }
}
