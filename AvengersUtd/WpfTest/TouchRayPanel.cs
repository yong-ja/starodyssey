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

namespace WpfTest
{
    public class TouchRayPanel : RayPickingPanel
    {
        readonly Window window;
        readonly Dictionary<TouchDevice, TexturedIcon> crosshairs;
        readonly Dictionary<TouchDevice, Vector3> points;
        Vector3 defaultRight = new Vector3(3f, 0f, -3f);
        RenderableNode rNode;
        TrackerWrapper tracker;

        TexturedIcon crosshair;


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


        public TouchRayPanel()
        {
            crosshairs = new Dictionary<TouchDevice, TexturedIcon>();
            points = new Dictionary<TouchDevice, Vector3>();
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

        public void SetTracker(TrackerWrapper tracker)
        {
            this.tracker = tracker;
            tracker.GazeDataReceived += (sender, e) => { crosshair.Position = e.GazePoint; };
        }

        protected override void OnTouchDown(AvengersUtd.Odyssey.UserInterface.Input.TouchEventArgs e)
        {
            base.OnTouchDown(e);
            window.CaptureTouch(e.TouchDevice);

            TexturedIcon crosshair = new TexturedIcon
            {
                CanRaiseEvents = false,
                Position = e.Location,
                Size = new System.Drawing.Size(64, 64),
                Texture = Texture2D.FromFile(Game.Context.Device, "Resources/Textures/crosshair.png")
            };
            crosshairs.Add(e.TouchDevice, crosshair);
            bool result;
            points.Add(e.TouchDevice, GetIntersection(e.Location, Vector3.UnitY, out result));
            OdysseyUI.CurrentHud.BeginDesign();
            Add(crosshair);
            OdysseyUI.CurrentHud.EndDesign();

        }

        Vector3 GetIntersection(Vector2 location, Vector3 axis, out bool result)
        {
            QuaternionCam camera = Game.CurrentRenderer.Camera;
           

            Viewport viewport = camera.Viewport;

            Vector3 pNear = Vector3.Unproject(new Vector3(location.X, location.Y, 0), viewport.X, viewport.Y, viewport.Width, viewport.Height, viewport.MinZ, viewport.MaxZ,
                              camera.WorldViewProjection);
            Vector3 pFar = Vector3.Unproject(new Vector3(location.X, location.Y, 1), viewport.X, viewport.Y, viewport.Width, viewport.Height, viewport.MinZ, viewport.MaxZ,
                              camera.WorldViewProjection);
            Ray r = new Ray(pNear, pFar - pNear);
            Plane p = new Plane(RNode.RenderableObject.AbsolutePosition, axis);

            float distance;
            result = Ray.Intersects(r, p, out distance);
            Vector3 pIntersection = r.Position + distance * Vector3.Normalize(r.Direction);

            return pIntersection;
        }

        protected override void OnTouchUp(AvengersUtd.Odyssey.UserInterface.Input.TouchEventArgs e)
        {
            base.OnTouchUp(e);
            window.ReleaseTouchCapture(e.TouchDevice);
            OdysseyUI.CurrentHud.BeginDesign();
            Remove(crosshairs[e.TouchDevice]);
            OdysseyUI.CurrentHud.EndDesign();
            
            crosshairs.Remove(e.TouchDevice);
            points.Remove(e.TouchDevice);

        }

        protected override void OnTouchMove(AvengersUtd.Odyssey.UserInterface.Input.TouchEventArgs e)
        {
            base.OnTouchMove(e);

            bool result;

            Vector3 pIntersection = GetIntersection(e.Location, Vector3.UnitY, out result);

            if (result)
            {
                points[e.TouchDevice] = pIntersection;
                Vector3[] vPoints = points.Values.ToArray();
                Vector3 vLeft=vPoints[0];
                Vector3 vRight = vPoints.Length < 2 ? defaultRight : vPoints[1];

                RNode.RenderableObject.ScalingValues = FindScalingValues(vLeft, vRight);
                RNode.RenderableObject.PositionV3 = FindPosition(vLeft, vRight,
                RNode.RenderableObject.ScalingValues);
            }

        }

        protected override void OnMouseClick(AvengersUtd.Odyssey.UserInterface.Input.MouseEventArgs e)
        {
            base.OnMouseClick(e);

            Vector3 scaling = RNode.RenderableObject.ScalingValues;
            bool result;
            Vector3 pIntersection =GetIntersection(tracker.GazePoint, Vector3.UnitZ, out result);
            Vector3 pPosition = RNode.RenderableObject.PositionV3;
            float y = Math.Max(0, pIntersection.Y);
            if (result)
            {
                RNode.RenderableObject.ScalingValues = new Vector3(scaling.X, y, scaling.Z);
                RNode.RenderableObject.PositionV3 = new Vector3(pPosition.X, RNode.RenderableObject.ScalingValues.Y/2, pPosition.Z);
            }
            //rNode.Update();
        }

        protected override void OnMouseMove(AvengersUtd.Odyssey.UserInterface.Input.MouseEventArgs e)
        {
            base.OnMouseMove(e);
            return;
            QuaternionCam camera = Game.CurrentRenderer.Camera;
            RenderableNode rNode = (RenderableNode)Game.CurrentRenderer.Scene.Tree.FindNode("RBox");


            Viewport viewport = camera.Viewport;

            Vector2 pNewPosition2 = new Vector2(e.Location.X, e.Location.Y);
            Vector3 pNear = Vector3.Unproject(new Vector3(pNewPosition2.X, pNewPosition2.Y, 0), viewport.X, viewport.Y, viewport.Width, viewport.Height, viewport.MinZ, viewport.MaxZ,
                              camera.WorldViewProjection);
            Vector3 pFar = Vector3.Unproject(new Vector3(pNewPosition2.X, pNewPosition2.Y, 1), viewport.X, viewport.Y, viewport.Width, viewport.Height, viewport.MinZ, viewport.MaxZ,
                              camera.WorldViewProjection);
            Ray r = new Ray(pNear, pFar - pNear);
            Plane p = new Plane(rNode.RenderableObject.AbsolutePosition, Vector3.UnitY);

            float distance;
            bool result2 = Ray.Intersects(r, p, out distance);
            Vector3 pIntersection = r.Position + distance * Vector3.Normalize(r.Direction);

            if (result2)
            {
                //Height
                //rNode.RenderableObject.ScalingValues = new Vector3(1f, pIntersection.Y, 1f);
                //rNode.RenderableObject.PositionV3 = new Vector3(0f, 0.25f*rNode.RenderableObject.ScalingValues.Y, 0f);

                //Width
                //rNode.RenderableObject.ScalingValues = new Vector3(pIntersection.X, 1f, 1f);
                //rNode.RenderableObject.PositionV3 = new Vector3(0.25f*rNode.RenderableObject.ScalingValues.X, 0f, 0f);
                //Depth
                //rNode.RenderableObject.ScalingValues = new Vector3(1f, 1f, pIntersection.Z);
                //rNode.RenderableObject.PositionV3 = new Vector3(0.25f, 0.25f, 0.25f * rNode.RenderableObject.ScalingValues.Z);

                rNode.RenderableObject.ScalingValues = FindScalingValues(pIntersection, new Vector3(2.5f, 0f, -2.5f));
                rNode.RenderableObject.PositionV3 = FindPosition(pIntersection, new Vector3(2.5f, 0f, -2.5f),
                    rNode.RenderableObject.ScalingValues);
            }

        }

        Vector3 FindScalingValues(Vector3 p1, Vector3 p2)
        {
            float scalingX, scalingZ;
            if (p1.Z < 0 && p2.Z < 0)
                scalingZ = Math.Abs(Math.Min(p1.Z, p2.Z)) + Math.Max(p1.Z, p2.Z);
            else
                scalingZ = Math.Abs(p1.Z) + Math.Abs(p2.Z);

            scalingX = Math.Abs(p1.X) + Math.Abs(p2.X);
            

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
        }

    }
}
