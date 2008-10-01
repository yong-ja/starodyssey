using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using AvengersUtd.Odyssey.UserInterface.RenderableControls.Interfaces;
using AvengersUtd.Odyssey.UserInterface.Style;
using SlimDX;
using SlimDX.Direct3D9;
using AvengersUtd.Odyssey.UserInterface.Helpers;
using System.Windows.Forms;

namespace AvengersUtd.Odyssey.UserInterface.RenderableControls
{
    public class ObjectHostControl : SimpleShapeControl
        
    {
        bool axisSwitched;
        bool isDragging;
        I3dEntity entity;

        Vector2 dragStartPosition;
        Plane p=planeX;

        static Plane planeX = new Plane(new Vector3(0, 1, 0), 0);
        static Plane planeY = new Plane(new Vector3(0, 0, 1), 0);
        bool mode;

        public I3dEntity Entity
        {
            get { return entity; }
            set { entity = value; }
        }

        public ObjectHostControl()
        {
            IsFocusable = false;
            ShapeDescriptors = new ShapeDescriptorCollection(0);
            ApplyControlStyle(StyleManager.GetControlStyle(ControlStyle.Empty));
        }

        public void SwitchMode(bool value)
        {
            mode = value;
        }

        public void SwitchPlane(bool xPlane)
        {
            axisSwitched = !axisSwitched;

            if (xPlane)
                p = new Plane(planeX.Normal, -iP.Y);
            else
                p = new Plane(planeY.Normal, -iP.Z);
        }

        public override bool IntersectTest(System.Drawing.Point cursorLocation)
        {
            Vector3 vNear = new Vector3(cursorLocation.X, cursorLocation.Y, 0);
            Vector3 vFar = new Vector3(cursorLocation.X, cursorLocation.Y, 1);

            vNear = MathHelper.Unproject(
                vNear,
                OdysseyUI.Device.Viewport,
                OdysseyUI.Device.GetTransform(TransformState.Projection),
                OdysseyUI.Device.GetTransform(TransformState.View),
                OdysseyUI.Device.GetTransform(TransformState.World));

            vFar = MathHelper.Unproject(
                vFar,
                OdysseyUI.Device.Viewport,
                OdysseyUI.Device.GetTransform(TransformState.Projection),
                OdysseyUI.Device.GetTransform(TransformState.View),
                OdysseyUI.Device.GetTransform(TransformState.World));

            Vector3 vDir = vFar - vNear;
            vDir.Normalize();

            Ray r = new Ray(vNear, vDir);
            //Plane p = new Plane(new Vector3(0, 1, 0), 0);
            //Vector3 iP = new Vector3();

            //Intersection.RayPlaneTest(r, p, out iP);

            float d;


            bool result = entity.Intersects(r);
            //DebugManager.LogToScreen(string.Format("X: {4} Y:{5} - II - X:{0:f2} Y:{1:f2} Z:{2:f2} - {3}",
            //    iP.X, iP.Y, iP.Z, result, cursorLocation.X, cursorLocation.Y));

            if (!result && isDragging)
                isDragging = result;

            return result;
        }

        protected override void OnMouseDown(System.Windows.Forms.MouseEventArgs e)
        {
            base.OnMouseDown(e);
            isDragging = true;
            OdysseyUI.CurrentHud.CaptureControl = this;
            dragStartPosition = new Vector2(e.X, e.Y);
        }

        protected override void OnMouseUp(System.Windows.Forms.MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (isDragging)
            {
                isDragging = false;
                OnMouseCaptureChanged(e);
            }
        }

        Vector3 iP;
        Point previousMousePosition= new Point(OdysseyUI.CurrentHud.Size.Width / 2, OdysseyUI.CurrentHud.Size.Height / 2);
        protected override void OnMouseMove(System.Windows.Forms.MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (!isDragging) return;

            if (!mode)
            {
                Vector3 vNear = new Vector3(e.X, e.Y, 0);
                Vector3 vFar = new Vector3(e.X, e.Y, 1);

                vNear = //Vector3.
                    MathHelper.Unproject(
                        vNear,
                        OdysseyUI.Device.Viewport,
                        OdysseyUI.Device.GetTransform(TransformState.Projection),
                        OdysseyUI.Device.GetTransform(TransformState.View),
                        OdysseyUI.Device.GetTransform(TransformState.World));

                vFar = //Vector3.
                    MathHelper.Unproject(
                        vFar,
                        OdysseyUI.Device.Viewport,
                        OdysseyUI.Device.GetTransform(TransformState.Projection),
                        OdysseyUI.Device.GetTransform(TransformState.View),
                        OdysseyUI.Device.GetTransform(TransformState.World));


                Vector3 vDir = vFar - vNear;

                Ray r = new Ray(vNear, vDir);
                //Plane p = new Plane(new Vector3(0, 1, 0), 0);
                iP = new Vector3();
                bool result = Intersection.RayPlaneTest(r, p, out iP);
                DebugManager.LogToScreen(string.Format("X: {4} Y:{5} - MM - X:{0:f2} Y:{1:f2} Z:{2:f2} - {3}",
                                                       iP.X,
                                                       iP.Y,
                                                       iP.Z,
                                                       result,
                                                       e.X,
                                                       e.Y));
                entity.PositionV3 = iP;
            }
            else
            {
                const float k = 0.010f;
                Point delta = new Point(e.X - previousMousePosition.X, e.Y - previousMousePosition.Y);
                //float x = 2 * ((float)delta.X / (OdysseyUI.CurrentHud.Size.Width)) - 1;
                //float y = 2 * ((float)delta.Y / (OdysseyUI.CurrentHud.Size.Height)) - 1;
                float x = delta.X * k;
                float y = delta.Y * k;
                DebugManager.LogToScreen(string.Format("R - X:{0:f2} Y:{1:f2}", x, y));
                //previousMousePosition = e.Location;
                Cursor.Position = new Point((int) dragStartPosition.X, (int) dragStartPosition.Y);
                if (axisSwitched)
                    entity.RotationDelta = new Vector3(x, y, 0);
                else
                    entity.RotationDelta = new Vector3(x, 0, y);
            }
        }
    }
}
