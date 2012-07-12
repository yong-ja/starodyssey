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

namespace WpfTest
{
    public class TouchRayPanel : Panel
    {
        const string ControlTag = "TouchRayPanel";
        readonly Window window;
        readonly Dictionary<TouchDevice, TexturedIcon> crosshairs;
        readonly Dictionary<TouchDevice, Vector3> points;
        readonly Dictionary<TouchDevice, IRenderable> arrows;
        Vector3 defaultRight = new Vector3(3f, 0f, -3f);
        private IRenderable box;
        RenderableNode rNode;
        //TrackerWrapper tracker;
        TexturedIcon crosshair;
        ScalingWidget sWidget;
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

            crosshair = new TexturedIcon
            {
                CanRaiseEvents = false,
                Position = Vector2.Zero,
                Size = new System.Drawing.Size(64, 64),
                Texture = Texture2D.FromFile(Game.Context.Device, "Resources/Textures/crosshair.png")
            };

            Add(crosshair);
        }

        //public void SetTracker(TrackerWrapper tracker)
        //{
        //    this.tracker = tracker;
        //    tracker.GazeDataReceived += (sender, e) => { crosshair.Position = e.GazePoint; };
        //}

        protected override void OnTouchDown(AvengersUtd.Odyssey.UserInterface.Input.TouchEventArgs e)
        {
            base.OnTouchDown(e);

            //if (crosshairs.Count >= 2)
            //    return;

            //TexturedIcon crosshair = new TexturedIcon
            //{
            //    CanRaiseEvents = false,
            //    Position = e.Location,
            //    Size = new System.Drawing.Size(64, 64),
            //    Texture = Texture2D.FromFile(Game.Context.Device, "Resources/Textures/crosshair.png")
            //};
            //crosshairs.Add(e.TouchDevice, crosshair);
            window.CaptureTouch(e.TouchDevice);

            IRenderable result;
            bool test = IntersectsArrow(sWidget, GetRay(e.Location), out result);

            if (test)
            {
                arrows.Add(e.TouchDevice, result);
                LogEvent.UserInterface.Write("TouchDown: " + e.TouchDevice.Id + " [" + result.Name+"]");
            }
            else
                LogEvent.UserInterface.Write("TouchDown: " + e.TouchDevice.Id + " No intersection");


            //points.Add(e.TouchDevice, GetIntersection(e.Location, Vector3.UnitY, out result));
            //OdysseyUI.CurrentHud.BeginDesign();
            //Add(crosshair);
            //OdysseyUI.CurrentHud.EndDesign();
            
            

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
            //OdysseyUI.CurrentHud.BeginDesign();
            ////Remove(crosshairs[e.TouchDevice]);
            //OdysseyUI.CurrentHud.EndDesign();
            
            //crosshairs.Remove(e.TouchDevice);
            //points.Remove(e.TouchDevice);
            arrows.Remove(e.TouchDevice);
            window.ReleaseTouchCapture(e.TouchDevice);
            LogEvent.UserInterface.Write("TouchUp " + e.TouchDevice.Id);

        }

        protected override void OnTouchMove(AvengersUtd.Odyssey.UserInterface.Input.TouchEventArgs e)
        {
            base.OnTouchMove(e);

            if (!arrows.ContainsKey(e.TouchDevice))
                return;

            IRenderable arrow = arrows[e.TouchDevice];
            IRenderable arrowHead = ((MeshGroup)arrow).Objects[0];
            bool test;
            Vector3 pIntersection;
            FixedNode fNode = (FixedNode)arrowHead.ParentNode.Parent;
            const float minSize = 0.25f;
            float delta;
            switch (arrow.Name)
            {
                case "YArrow":
                    pIntersection = GetIntersection(e.Location, -Vector3.UnitZ, arrowHead, out test);
                    if (test)
                    {
                        delta = pIntersection.Y - (box.PositionV3.Y + box.ScalingValues.Y / 2);
                        box.ScalingValues += new Vector3(0, delta, 0);
                        if (box.ScalingValues.Y < minSize)
                        {
                            box.ScalingValues = new Vector3(box.ScalingValues.X, minSize, box.ScalingValues.Z);
                            fNode.Position = new Vector3(fNode.Position.X, (box.PositionV3.Y + box.ScalingValues.Y / 2), fNode.Position.Z);
                        }
                        else
                            fNode.Position = new Vector3(fNode.Position.X, pIntersection.Y, fNode.Position.Z);

                        box.PositionV3 = new Vector3(box.PositionV3.X, box.ScalingValues.Y / 2 - 0.5f, box.PositionV3.Z);
                    }
                    break;

                case "XArrow":
                    pIntersection = GetIntersection(e.Location, -Vector3.UnitZ, arrowHead, out test);
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
                        else
                            fNode.Position = new Vector3(pIntersection.X, fNode.Position.Y, fNode.Position.Z);
                        box.PositionV3 = new Vector3(box.ScalingValues.X / 2 - 0.5f, box.PositionV3.Y, box.PositionV3.Z);
                    }
                    break;

                case "ZArrow":
                    pIntersection = GetIntersection(e.Location, Vector3.UnitY, arrowHead, out test);
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
                        else
                            fNode.Position = new Vector3(fNode.Position.X, fNode.Position.Y, pIntersection.Z);
                        box.PositionV3 = new Vector3(box.PositionV3.X, box.PositionV3.Y, box.ScalingValues.Z / 2 - 0.5f);
                    }
                    break;
            }

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
            bool test;
            Vector3 pIntersection;
            FixedNode fNode = (FixedNode)arrowHead.ParentNode.Parent;
            const float minSize = 0.25f;
            float delta;
            switch (tempArrow.Name)
            {
                case "YArrow":
                    pIntersection = GetIntersection(e.Location, -Vector3.UnitZ, arrowHead, out test);
                    if (test)
                    {
                        delta = pIntersection.Y - (box.PositionV3.Y + box.ScalingValues.Y / 2);
                        box.ScalingValues += new Vector3(0, delta, 0);
                        if (box.ScalingValues.Y < minSize)
                        {
                            box.ScalingValues = new Vector3(box.ScalingValues.X, minSize, box.ScalingValues.Z);
                            fNode.Position = new Vector3(fNode.Position.X, (box.PositionV3.Y + box.ScalingValues.Y / 2), fNode.Position.Z);
                        }
                        else
                            fNode.Position = new Vector3(fNode.Position.X, pIntersection.Y, fNode.Position.Z);

                        box.PositionV3 = new Vector3(box.PositionV3.X, box.ScalingValues.Y / 2 - 0.5f, box.PositionV3.Z);
                    }
                    break;

                case "XArrow":
                    pIntersection = GetIntersection(e.Location, -Vector3.UnitZ, arrowHead, out test);
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
                        else
                            fNode.Position = new Vector3(pIntersection.X, fNode.Position.Y, fNode.Position.Z);
                        box.PositionV3 = new Vector3(box.ScalingValues.X / 2 - 0.5f, box.PositionV3.Y, box.PositionV3.Z);
                    }
                    break;

                case "ZArrow":
                    pIntersection = GetIntersection(e.Location, Vector3.UnitY, arrowHead, out test);
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
                        else
                            fNode.Position = new Vector3(fNode.Position.X, fNode.Position.Y, pIntersection.Z);
                        box.PositionV3 = new Vector3(box.PositionV3.X, box.PositionV3.Y, box.ScalingValues.Z / 2 - 0.5f);
                    }
                    break;
            }

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
