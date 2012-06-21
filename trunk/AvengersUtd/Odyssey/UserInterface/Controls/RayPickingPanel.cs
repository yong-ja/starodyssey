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
using MouseEventArgs = AvengersUtd.Odyssey.UserInterface.Input.MouseEventArgs;

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

        protected override void OnMouseDown(MouseEventArgs e)
        {
            Contract.Requires(Camera != null);
            base.OnMouseDown(e);
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


        static Ray GetRay(ICamera camera, MouseEventArgs e)
        {
            Vector2 p = e.Location;
            Matrix mWVP = camera.WorldViewProjection;
            Viewport viewport = camera.Viewport;
            Vector3 vNear = Vector3.Unproject(new Vector3(p, 0), viewport.X, viewport.Y,
                viewport.Width, viewport.Height, viewport.MinZ, viewport.MaxZ, mWVP);
            Vector3 vFar = Vector3.Unproject(new Vector3(p, 1), viewport.X, viewport.Y,
                viewport.Width, viewport.Height, viewport.MinZ, viewport.MaxZ, mWVP);
            return new Ray(vNear, vFar - vNear);
        }
    }
}
