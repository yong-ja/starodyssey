using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AvengersUtd.Odyssey.Utils.Logging;
using SlimDX;
using SlimDX.Direct3D11;
using System.Diagnostics.Contracts;
using AvengersUtd.Odyssey.Graphics.Meshes;

namespace AvengersUtd.Odyssey.UserInterface.Controls
{
    public class RayPickingPanel : Panel
    {
        const string ControlTag = "RayPickingPanel";
        private static int count;
        private IRenderable rObject;

        #region Constructors

        public RayPickingPanel()
            : base(ControlTag + ++count, "Empty")
        {
        }

        #endregion

        public ICamera Camera { get; set; }

        protected override void OnMouseDown(System.Windows.Forms.MouseEventArgs e)
        {
            Contract.Requires(Camera != null);
            base.OnMouseClick(e);
            Ray ray = GetRay(Camera, e);
            LogEvent.UserInterface.Log(ray.ToString());

            bool result = Game.CurrentRenderer.Scene.CheckIntersection(ray, out rObject);

            if (!result)
            {
                LogEvent.UserInterface.Log("Intersection failed)");
            }
            else
            {
                LogEvent.UserInterface.Log("Intersected [" + rObject.Name + "]");
                rObject.ProcessMouseEvent(MouseEventType.MouseDown, e);
            }

        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            Ray ray = GetRay(Camera, e);
            //

            //if (!result)
            //    return;
            if (rObject != null && rObject.HasBehaviour(typeof(MouseDraggingBehaviour).Name))
            {
                rObject.ProcessMouseEvent(MouseEventType.MouseMove, e);
                return;
            }
            bool result = Game.CurrentRenderer.Scene.CheckIntersection(ray, out rObject);
            if (result)
                rObject.ProcessMouseEvent(MouseEventType.MouseMove, e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            Ray ray = GetRay(Camera, e);
            IRenderable rObject;
            bool result = Game.CurrentRenderer.Scene.CheckIntersection(ray, out rObject);

            if (!result)
                return;
            rObject.ProcessMouseEvent(MouseEventType.MouseUp, e);
        }


        public static Vector4 UnprojectD(ICamera camera, Vector3 screenSpace)
        {
            Matrix transform = camera.World * camera.View * camera.Projection;
            Matrix inverse = Matrix.Invert(transform);
            Vector4 result = Vector3.Transform(screenSpace, inverse);
            return result;
        }

        public static Ray UnprojectPoint(ICamera camera, int x, int y)
        {

            Vector3 screenNear = new Vector3((float)x, (float)y, 0f);
            Vector3 screenFar = new Vector3((float)x, (float)y, 1f);
            //multiplying by 10 makes sure the line's direction is accurate - by casting out   
            //a very far distance. The direction is all we really care about, so the further  
            //away the point, the more accurate it will be....  
            Vector4 worldNear = Unproject(camera, screenNear);
            Vector4 worldFar = Unproject(camera, screenFar);
            Vector4 direction = worldFar - worldNear;
            float len = 1f / direction.Length();
            direction = direction * len; //manual normalisation  

            Ray r = new Ray(new Vector3(worldNear.X, worldNear.Y, worldNear.Z), 
                new Vector3(direction.X, direction.Y, direction.Z));

            return (r);
        }

        public static Vector4 Unproject(ICamera camera, Vector3 screenSpace)
        {
            //First, convert raw screen coords to unprojectable ones  
            Viewport port = new Viewport(0, 0, 1920, 1080,0.1f, 100f);
            Vector3 position = new Vector3();
            position.X = (((screenSpace.X - (float)port.X) / ((float)port.Width)) * 2f) - 1f;
            position.Y = -((((screenSpace.Y - (float)port.Y) / ((float)port.Height)) * 2f) - 1f);
            position.Z = (screenSpace.Z - port.MinZ) / (port.MaxZ - port.MinZ);

            //Unproject by transforming the 4d vector by the inverse of the projecttion matrix, followed by the inverse of the view matrix.  
            Vector4 us4 = new Vector4(position, 1f);
            Vector4 up4 = Vector4.Transform(us4, Matrix.Invert(camera.Projection));
            Vector3 up3 = new Vector3(up4.X, up4.Y, up4.Z);
            up3 = up3 / up4.W; //better to do this here to reduce precision loss..  
            Vector4 uv3 = Vector3.Transform(up3, Matrix.Invert(camera.View));
            return (uv3);
        } 

        static Ray GetRay(ICamera camera, MouseEventArgs e)
        {
            Matrix mWVP = camera.WorldViewProjection;
            Viewport viewport = camera.Viewport;
            Vector3 vNear = Vector3.Unproject(new Vector3(e.X, e.Y, 0), viewport.X, viewport.Y,
                viewport.Width, viewport.Height, viewport.MinZ, viewport.MaxZ, mWVP);
            Vector3 vFar = Vector3.Unproject(new Vector3(e.X, e.Y, 1), viewport.X, viewport.Y,
                viewport.Width, viewport.Height, viewport.MinZ, viewport.MaxZ, mWVP);
            return new Ray(vNear, vFar - vNear);
        }
    }
}
