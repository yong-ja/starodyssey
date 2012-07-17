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

namespace WpfTest
{
    public class TouchRayPanel : Panel
    {
        const string ControlTag = "TouchRayPanel";
        const int dwellInterval = 200;
        const float maxR = (ScalingWidget.ArrowIntersectionRadius * ScalingWidget.ArrowIntersectionRadius);
        readonly Window window;
        readonly Dictionary<TouchDevice, TexturedIcon> crosshairs;
        readonly Dictionary<TouchDevice, Vector3> points;
        readonly Dictionary<TouchDevice, IRenderable> arrows;
        Vector2 prevEyeLocation;
        private IRenderable box;
        private IRenderable eyeArrow;
        RenderableNode rNode;
        TrackerWrapper tracker;
        TexturedIcon crosshair;
        ScalingWidget sWidget;
        
        EventWaitHandle dwellTime;
        Thread dwellThread;

        DateTime dwellStart;
        
        static int count;

        RenderableNode RNode
        {
            get
            {
                if (rNode == null)
                    rNode = (RenderableNode)Game.CurrentRenderer.Scene.Tree.FindNode("RBox");
                return rNode;
            }
            set {rNode = value;}
        }


        public TouchRayPanel() : base(ControlTag + ++count, "Empty")
        {
            crosshairs = new Dictionary<TouchDevice, TexturedIcon>();
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

        void DwellLoop()
        {
            Thread.Sleep(200);
            dwellTime.Set();
        }

        public void SetTracker(TrackerWrapper tracker)
        {
            this.tracker = tracker;
            tracker.GazeDataReceived += new EventHandler<GazeEventArgs>(tracker_GazeDataReceived);
        }

        void tracker_GazeDataReceived(object sender, GazeEventArgs e)
        {
            
            crosshair.Position = e.GazePoint;

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

                if (dwellCheckArrow == eyeArrow) {
                    EyeMoveArrow(e.GazePoint);
                    sWidget.Select(dwellCheckArrow.Name, Color.Red);
                }
                else {
                    sWidget.ResetColors();
                    ((Arrow)eyeArrow).IsDwelling = false;
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
                MoveArrow(gazePoint, eyeArrow);
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
                LogEvent.UserInterface.Write("TouchDown: " + e.TouchDevice.Id + " [" + result.Name+"]");
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
            LogEvent.UserInterface.Write("TouchUp " + e.TouchDevice.Id);

        }

        void MoveArrow(Vector2 location, IRenderable arrow)
        {
            IRenderable arrowHead = ((MeshGroup)arrow).Objects[0];
            const float minSize = 0.25f;
            const float maxSizeY = 2.5f;
            const float maxSizeZ = 5f;
            const float maxSizeX = 5f;
            Vector3 pIntersection;
            FixedNode fNode = (FixedNode)arrowHead.ParentNode.Parent;
            float delta;
            bool test;
            switch (arrow.Name)
            {
                case "YArrow":
                    pIntersection = GetIntersection(location, -Vector3.UnitZ, arrowHead, out test);
                    if (test)
                    {
                        delta = pIntersection.Y - (box.PositionV3.Y + box.ScalingValues.Y / 2);
                        box.ScalingValues += new Vector3(0, delta, 0);
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
                        else
                            fNode.Position = new Vector3(fNode.Position.X, pIntersection.Y, fNode.Position.Z);

                        box.PositionV3 = new Vector3(box.PositionV3.X, box.ScalingValues.Y / 2 - 0.5f, box.PositionV3.Z);
                    }
                    break;

                case "XArrow":
                    pIntersection = GetIntersection(location, -Vector3.UnitZ, arrowHead, out test);
                    if (test)
                    {
                        delta = pIntersection.X - (box.PositionV3.X + box.ScalingValues.X / 2);
                        delta = Math.Min(minSize, delta);

                        box.ScalingValues += new Vector3(delta, 0, 0);
                        if (box.ScalingValues.X < minSize)
                        {
                            box.ScalingValues = new Vector3(minSize, box.ScalingValues.Y, box.ScalingValues.Z);
                            fNode.Position = new Vector3((box.PositionV3.X + box.ScalingValues.X / 2), fNode.Position.Y, fNode.Position.Z);
                        }
                        else if (box.ScalingValues.X> maxSizeX)
                        {
                            box.ScalingValues = new Vector3(maxSizeX, box.ScalingValues.Y, box.ScalingValues.Z);
                            fNode.Position = new Vector3((box.PositionV3.X + box.ScalingValues.X / 2), fNode.Position.Y, fNode.Position.Z);
                        }
                        else
                            fNode.Position = new Vector3(pIntersection.X, fNode.Position.Y, fNode.Position.Z);
                        box.PositionV3 = new Vector3(box.ScalingValues.X / 2 - 0.5f, box.PositionV3.Y, box.PositionV3.Z);
                    }
                    break;

                case "ZArrow":
                    pIntersection = GetIntersection(location, Vector3.UnitY, arrowHead, out test);
                    if (test)
                    {
                        delta = pIntersection.Z - (box.PositionV3.Z + box.ScalingValues.Z / 2);
                        delta = Math.Min(minSize, delta);

                        box.ScalingValues += new Vector3(0, 0, delta);
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
                        else
                            fNode.Position = new Vector3(fNode.Position.X, fNode.Position.Y, pIntersection.Z);
                        box.PositionV3 = new Vector3(box.PositionV3.X, box.PositionV3.Y, box.ScalingValues.Z / 2 - 0.5f);
                    }
                    break;
            }
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

        /*
        //protected override void OnMouseClick(AvengersUtd.Odyssey.UserInterface.Input.MouseEventArgs e)
        //{
        //    base.OnMouseClick(e);

        //    //Vector3 scaling = RNode.RenderableObject.ScalingValues;
        //    //bool result;
        //    //Vector3 pIntersection =GetIntersection(tracker.GazePoint, Vector3.UnitZ, out result);
        //    //Vector3 pPosition = RNode.RenderableObject.PositionV3;
        //    //float y = Math.Max(0, pIntersection.Y);
        //    //if (result)
        //    //{
        //    //    RNode.RenderableObject.ScalingValues = new Vector3(scaling.X, y, scaling.Z);
        //    //    RNode.RenderableObject.PositionV3 = new Vector3(pPosition.X, RNode.RenderableObject.ScalingValues.Y/2, pPosition.Z);
        //    //}
        //    //rNode.Update();
        //}

        //protected override void OnMouseMove(AvengersUtd.Odyssey.UserInterface.Input.MouseEventArgs e)
        //{
        //    base.OnMouseMove(e);
        //    return;

        //    //QuaternionCam camera = Game.CurrentRenderer.Camera;
        //    //RenderableNode rNode = (RenderableNode)Game.CurrentRenderer.Scene.Tree.FindNode("RBox");


        //    //Viewport viewport = camera.Viewport;

        //    //Vector2 pNewPosition2 = new Vector2(e.Location.X, e.Location.Y);
        //    //Vector3 pNear = Vector3.Unproject(new Vector3(pNewPosition2.X, pNewPosition2.Y, 0), viewport.X, viewport.Y, viewport.Width, viewport.Height, viewport.MinZ, viewport.MaxZ,
        //    //                  camera.WorldViewProjection);
        //    //Vector3 pFar = Vector3.Unproject(new Vector3(pNewPosition2.X, pNewPosition2.Y, 1), viewport.X, viewport.Y, viewport.Width, viewport.Height, viewport.MinZ, viewport.MaxZ,
        //    //                  camera.WorldViewProjection);
        //    //Ray r = new Ray(pNear, pFar - pNear);
        //    //Plane p = new Plane(rNode.RenderableObject.AbsolutePosition, Vector3.UnitY);

        //    //float distance;
        //    //bool result2 = Ray.Intersects(r, p, out distance);
        //    //Vector3 pIntersection = r.Position + distance * Vector3.Normalize(r.Direction);

        //    //if (result2)
        //    //{
        //    //    //Height
        //    //    //rNode.RenderableObject.ScalingValues = new Vector3(1f, pIntersection.Y, 1f);
        //    //    //rNode.RenderableObject.PositionV3 = new Vector3(0f, 0.25f*rNode.RenderableObject.ScalingValues.Y, 0f);

        //    //    //Width
        //    //    //rNode.RenderableObject.ScalingValues = new Vector3(pIntersection.X, 1f, 1f);
        //    //    //rNode.RenderableObject.PositionV3 = new Vector3(0.25f*rNode.RenderableObject.ScalingValues.X, 0f, 0f);
        //    //    //Depth
        //    //    //rNode.RenderableObject.ScalingValues = new Vector3(1f, 1f, pIntersection.Z);
        //    //    //rNode.RenderableObject.PositionV3 = new Vector3(0.25f, 0.25f, 0.25f * rNode.RenderableObject.ScalingValues.Z);

        //    //    rNode.RenderableObject.ScalingValues = FindScalingValues(pIntersection, new Vector3(2.5f, 0f, -2.5f));
        //    //    rNode.RenderableObject.PositionV3 = FindPosition(pIntersection, new Vector3(2.5f, 0f, -2.5f),
        //    //        rNode.RenderableObject.ScalingValues);
        //    //}

        //}

        Vector3 FindScalingValues(Vector3 p1, Vector3 p2)
        {
            float scalingX, scalingZ;
            if (p1.Z > 0 && p2.Z > 0)
                scalingZ = Math.Max(p1.Z, p2.Z) - Math.Min(p1.Z, p2.Z);
            else if (p1.Z < 0 && p2.Z < 0)
                scalingZ = Math.Abs(Math.Min(p1.Z, p2.Z)) + Math.Max(p1.Z, p2.Z);
            else if (p1.Z < 0 && p2.Z > 0)
                scalingZ = Math.Abs(p1.Z) + p2.Z;
            else //(p1 positive p2 negative)
                scalingZ = p1.Z + Math.Abs(p2.Z);

            // left positive right positive
            if (p1.X > 0 && p2.X > 0)
                scalingX = p2.X - p1.X;
            else if (p1.X < 0 && p2.X > 0) //left negative right positive
                scalingX = Math.Abs(p1.X) + p2.X;
            else //if (p1.X < 0 && p2.X < 0)
                scalingX = Math.Abs(p1.X) + p2.X;

            

            return new Vector3(scalingX, 1f, scalingZ);
        }

        Vector3 FindPosition(Vector3 p1, Vector3 p2, Vector3 scalingValues)
        {
            float posX = Math.Max(p1.X, p2.X);
            float negX = Math.Min(p1.X, p2.X);

            float posZ = Math.Max(p1.Z, p2.Z);
            float negZ = Math.Min(p1.Z, p2.Z);

            float tX,tZ;
            if (Math.Abs(negX) > posX)
                tX = ((posX) - Math.Abs(negX))/2;
            else
                tX = (posX + negX)/2f;

            if (posZ < 0 && negZ < 0)
                tZ = posZ - (Math.Abs(negZ) + posZ)/2f;
            else if (Math.Abs(negZ) > posZ)
                tZ = ((posZ) - Math.Abs(negZ))/2;
            else
                tZ = (posZ + negZ) / 2f;
            

            //return new Vector3(0.25f * scalingValues.X, 0.25f, 0.25f * scalingValues.Z);

            //return new Vector3(scalingValues.X/4f);

            return new Vector3(tX, 0f, tZ);
        }*/

        public void SetScalingWidget(ScalingWidget sWidget)
        {
            this.sWidget = sWidget;
        }

        public void SetBox(IRenderable box)
        {
            this.box = box;
        }

    }
}
