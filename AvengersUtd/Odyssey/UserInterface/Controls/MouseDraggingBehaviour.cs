using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AvengersUtd.Odyssey.Geometry;
using AvengersUtd.Odyssey.Graphics.Meshes;
using AvengersUtd.Odyssey.Graphics.Rendering.Management;
using AvengersUtd.Odyssey.Utils.Logging;
using SlimDX;
using SlimDX.Direct3D11;

namespace AvengersUtd.Odyssey.UserInterface.Controls
{
    public class MouseDraggingBehaviour : IMouseBehaviour
    {
        private readonly IRenderable rObject;
        private readonly ICamera camera;
        private Vector2 pOffset;
        private bool isDragging;

        public MouseDraggingBehaviour(IRenderable rObject, ICamera camera)
        {
            this.rObject = rObject;
            this.camera = camera;
        }

        IRenderable IMouseBehaviour.RenderableObject
        {
            get { return rObject; }
        }

        void IMouseBehaviour.OnMouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            new Vector2(e.X, e.Y);
            isDragging = true;
            Viewport viewport = camera.Viewport;
            Vector3 pObjCenter3 = Vector3.Project(rObject.AbsolutePosition, viewport.X, viewport.Y, viewport.Width, viewport.Height,
                                                 viewport.MinZ, viewport.MaxZ, camera.WorldViewProjection);
            pOffset = new Vector2(e.Location.X, e.Location.Y) - new Vector2(pObjCenter3.X, pObjCenter3.Y);

        }

        void IMouseBehaviour.OnMouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            return;
        }

        void IMouseBehaviour.OnMouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            isDragging = false;
        }

        void IMouseBehaviour.OnMouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (!isDragging)
                return;

            Viewport viewport = camera.Viewport;
           
            Vector2 pNewPosition2 = new Vector2(e.Location.X, e.Location.Y) -pOffset;
            Vector3 pNear = Vector3.Unproject( new Vector3(pNewPosition2.X, pNewPosition2.Y, 0), viewport.X, viewport.Y, viewport.Width, viewport.Height, viewport.MinZ, viewport.MaxZ,
                              camera.WorldViewProjection);
            Vector3 pFar = Vector3.Unproject( new Vector3(pNewPosition2.X, pNewPosition2.Y, 1), viewport.X, viewport.Y, viewport.Width, viewport.Height, viewport.MinZ, viewport.MaxZ,
                              camera.WorldViewProjection);
            Ray r = new Ray(pNear, pFar - pNear);
            //Ray r = new Ray(pNear, camera.ViewVector);
            Plane p = new Plane(rObject.AbsolutePosition, Vector3.UnitY);

            float distance;
            bool result2 = Ray.Intersects(r, p, out distance);
            Vector3 pIntersection = r.Position + distance * Vector3.Normalize(r.Direction);

            if (result2)
                ((TransformNode)(rObject.ParentNode.Parent)).Position = new Vector3(pIntersection.X, rObject.AbsolutePosition.Y, pIntersection.Z);
        }
    }
}
