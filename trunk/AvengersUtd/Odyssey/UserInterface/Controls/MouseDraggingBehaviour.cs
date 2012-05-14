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
        private readonly ICamera camera;
        private Vector2 pOffset;
        private bool isDragging;

        public MouseDraggingBehaviour(ICamera camera)
        {
            this.camera = camera;
        }

        public string Name
        {
            get { return GetType().Name; }
        }

        public IRenderable RenderableObject
        {
            get;
            set;
        }

        void IMouseBehaviour.OnMouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            new Vector2(e.X, e.Y);
            isDragging = true;
            Viewport viewport = camera.Viewport;
            Vector3 pObjCenter3 = Vector3.Project(RenderableObject.AbsolutePosition, viewport.X, viewport.Y, viewport.Width, viewport.Height,
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
            Plane p = new Plane(RenderableObject.AbsolutePosition, Vector3.UnitY);

            float distance;
            bool result2 = Ray.Intersects(r, p, out distance);
            Vector3 pIntersection = r.Position + distance * Vector3.Normalize(r.Direction);

            if (result2)
                ((TransformNode)(RenderableObject.ParentNode.Parent)).Position = new Vector3(pIntersection.X, RenderableObject.AbsolutePosition.Y, pIntersection.Z);
        }

        void IBehaviour.Add()
        {
            IMouseBehaviour mBehaviour = ((IMouseBehaviour)this);
            RenderableObject.MouseClick += mBehaviour.OnMouseClick;
            RenderableObject.MouseDown += mBehaviour.OnMouseDown;
            RenderableObject.MouseUp += mBehaviour.OnMouseUp;
            RenderableObject.MouseMove += mBehaviour.OnMouseMove;
        }

        void IBehaviour.Remove()
        {
            IMouseBehaviour mBehaviour = ((IMouseBehaviour)this);
            RenderableObject.MouseClick -= mBehaviour.OnMouseClick;
            RenderableObject.MouseDown -= mBehaviour.OnMouseDown;
            RenderableObject.MouseUp -= mBehaviour.OnMouseUp;
            RenderableObject.MouseMove -= mBehaviour.OnMouseMove;

        }
    }
}
