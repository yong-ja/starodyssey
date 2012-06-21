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

        public TouchRayPanel()
        {
            crosshairs = new Dictionary<TouchDevice, TexturedIcon>();
            window = Global.Window;
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
            OdysseyUI.CurrentHud.BeginDesign();
            Add(crosshair);
            OdysseyUI.CurrentHud.EndDesign();

        }

        protected override void OnTouchUp(AvengersUtd.Odyssey.UserInterface.Input.TouchEventArgs e)
        {
            base.OnTouchUp(e);
            window.ReleaseTouchCapture(e.TouchDevice);
            OdysseyUI.CurrentHud.BeginDesign();
            Remove(crosshairs[e.TouchDevice]);
            OdysseyUI.CurrentHud.EndDesign();
            
            crosshairs.Remove(e.TouchDevice);

        }

        protected override void OnTouchMove(AvengersUtd.Odyssey.UserInterface.Input.TouchEventArgs e)
        {
            base.OnTouchMove(e);

            TexturedIcon crosshair = crosshairs[e.TouchDevice];
            if (crosshair == null) return;

            crosshair.Position = e.Location - new Vector2(crosshair.Width / 2f, crosshair.Height / 2f);

        }

        protected override void OnMouseClick(AvengersUtd.Odyssey.UserInterface.Input.MouseEventArgs e)
        {
            base.OnMouseClick(e);
            RenderableNode rNode = (RenderableNode) Game.CurrentRenderer.Scene.Tree.FindNode("RBox");

            rNode.RenderableObject.ScalingValues = new Vector3(1f, rNode.RenderableObject.ScalingValues.Y+1, 1f);
            rNode.RenderableObject.PositionV3 = new Vector3(0.25f, 0.25f*rNode.RenderableObject.ScalingValues.Y, 0.25f);
            //rNode.Update();
        }

        protected override void OnMouseMove(AvengersUtd.Odyssey.UserInterface.Input.MouseEventArgs e)
        {
            base.OnMouseMove(e);
            QuaternionCam camera = Game.CurrentRenderer.Camera;
            RenderableNode rNode = (RenderableNode)Game.CurrentRenderer.Scene.Tree.FindNode("RBox");

            //rNode.RenderableObject.ScalingValues = new Vector3(1f, rNode.RenderableObject.ScalingValues.Y + 1, 1f);
            //rNode.RenderableObject.PositionV3 = new Vector3(0f, 0.25f * rNode.RenderableObject.ScalingValues.Y, 0f);

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

                //rNode.RenderableObject.ScalingValues = FindScalingValues(pIntersection, new Vector3(2.5f, 0f, -2.5f));
                rNode.RenderableObject.ScalingValues = FindScalingValues(pIntersection, new Vector3(3f, 0f, -3));
                rNode.RenderableObject.PositionV3 = FindPosition(pIntersection, new Vector3(3f, 0f, -3f),
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
