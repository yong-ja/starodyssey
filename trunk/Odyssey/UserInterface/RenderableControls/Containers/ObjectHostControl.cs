using System;
using System.Collections.Generic;
using System.Text;
using AvengersUtd.Odyssey.UserInterface.RenderableControls.Interfaces;
using AvengersUtd.Odyssey.UserInterface.Style;
using SlimDX;
using SlimDX.Direct3D9;
using AvengersUtd.Odyssey.UserInterface.Helpers;

namespace AvengersUtd.Odyssey.UserInterface.RenderableControls
{
    public class  ObjectHostControl : SimpleShapeControl
        
    {
        bool isDragging = false;
        public I3dEntity entity;
        Vector2 dragStartPosition;
        Plane p=planeX;

        
        static Plane planeX = new Plane(new Vector3(0, 1, 0), 0);
        static Plane planeY = new Plane(new Vector3(0, 0, 1), 0);

        public ObjectHostControl()
        {
            IsFocusable = false;
            ShapeDescriptors = new ShapeDescriptorCollection(1);
            ApplyControlStyle(StyleManager.GetControlStyle(this.GetType().ToString()));
        }

        public void SwitchPlane(bool xPlane)
        {
            if (xPlane)
                p = new Plane(planeX.Normal, iP.Y);
            else
                p = new Plane(planeY.Normal, iP.Z);
        }

        public override bool IntersectTest(System.Drawing.Point cursorLocation)
        {
            Vector3 vNear = new Vector3(cursorLocation.X, cursorLocation.Y, 0);
            Vector3 vFar = new Vector3(cursorLocation.X, cursorLocation.Y, 1);
            
            vNear = Unproject(
            //vNear=Vector3.Unproject(
                vNear,
                OdysseyUI.Device.Viewport,
                OdysseyUI.Device.GetTransform(TransformState.Projection),
                OdysseyUI.Device.GetTransform(TransformState.View),
                OdysseyUI.Device.GetTransform(TransformState.World));

            vFar = Unproject(
                vFar, 
                OdysseyUI.Device.Viewport, 
                OdysseyUI.Device.GetTransform(TransformState.Projection),
                OdysseyUI.Device.GetTransform(TransformState.View),
                OdysseyUI.Device.GetTransform(TransformState.World));

            //vNear.Normalize();
            //vFar.Normalize();
            Vector3 vDir = vFar - vNear;
            vDir.Normalize();
           
                Ray r = new Ray(vNear, vDir);
                //Plane p = new Plane(new Vector3(0, 1, 0), 0);
                Vector3 iP = new Vector3();
                
                Intersection.RayPlaneTest(r, p, out iP);

                float d;
            bool result = Ray.Intersects(r, new BoundingSphere(entity.Position, 0.5f), out d);
            DebugManager.LogToScreen(string.Format("X: {4} Y:{5} - II - X:{0:f2} Y:{1:f2} Z:{2:f2} - {3}",
                iP.X, iP.Y, iP.Z, result, cursorLocation.X, cursorLocation.Y));

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
        Vector3 iP= new Vector3();
        protected override void OnMouseMove(System.Windows.Forms.MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (isDragging)
            {
                
                Vector3 vNear = new Vector3(e.X, e.Y, 0);
                Vector3 vFar = new Vector3(e.X, e.Y, 1);

                vNear = //Vector3.
                    Unproject(
                    vNear,
                    OdysseyUI.Device.Viewport,
                    OdysseyUI.Device.GetTransform(TransformState.Projection),
                    OdysseyUI.Device.GetTransform(TransformState.View),
                    OdysseyUI.Device.GetTransform(TransformState.World));

                vFar = //Vector3.
                    Unproject(
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
                iP.X, iP.Y, iP.Z, result,e.X,e.Y));
                entity.Position =iP;

                //Vector2 newDragPosition = new Vector2(e.Location.X, e.Location.Y);

                //Vector2 delta = newDragPosition - dragStartPosition;
                //dragStartPosition = newDragPosition;

                

                //entity.Position = new Vector3(entity.Position.X + delta.X/100 ,
                //    entity.Position.Y ,
                //    entity.Position.Z - delta.Y / 100);
            }
        }

         public static Vector3 Unproject(Vector3 screenSpace, Viewport port, Matrix projection, Matrix view, Matrix World)
        {
            //First, convert raw screen coords to unprojectable ones  
            
            Vector3 position = new Vector3();
            projection.Invert();
            view.Invert();
            position.X = (((screenSpace.X - (float)port.X) / ((float)port.Width)) * 2f) - 1f;
            position.Y = -((((screenSpace.Y - (float)port.Y) / ((float)port.Height)) * 2f) - 1f);
            position.Z = (screenSpace.Z - port.MinZ) / (port.MaxZ - port.MinZ);

            //Unproject by transforming the 4d vector by the inverse of the projecttion matrix, followed by the inverse of the view matrix.  
            Vector4 us4 = new Vector4(position, 1f);
            Vector4 up4 = Vector4.Transform(us4, projection);
            Vector3 up3 = new Vector3(up4.X, up4.Y, up4.Z);
            up3 = up3 / up4.W; //better to do this here to reduce precision loss..  
            Vector4 uv3 = Vector3.Transform(up3, view);
            return new Vector3(uv3.X, uv3.Y, uv3.Z);
        } 

        
    }
}
