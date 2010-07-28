
using System;
using AvengersUtd.Odyssey.Graphics.Materials;
using AvengersUtd.Odyssey.Graphics.Rendering.SceneGraph;
using AvengersUtd.Odyssey.Resources;
using SlimDX;

namespace AvengersUtd.Odyssey.Graphics.Meshes
{
    public interface IRenderable : IDisposable
    {
        //EntityDescriptor Descriptor { get; }
        //Mesh Mesh { get; }

        bool Inited { get; }
        bool IsCollidable { get; }
        bool IsVisible { get; }
        bool CastsShadows { get; }

        Matrix World { get; set; }
        Vector3 PositionV3 { get; }
        Vector4 PositionV4 { get; set; }

        Vector3 RotationDelta { get; set; }
        Quaternion CurrentRotation { get; set; }

        bool IsInViewFrustum();
        RenderableNode ParentNode { get; set; }
       
        /// <summary>
        /// Loads resources and inits object.
        /// </summary>
        void Init();

        /// <summary>
        /// Renders the entity with effects applied.
        /// </summary>
        void Render();

        /// <summary>
        /// Updates the position of the entity.
        /// </summary>
        void UpdatePosition();

        /// <summary>
        /// Programmatically causes a collision event involving this object with
        /// another entity.
        /// </summary>
        void CollideWith(IRenderable collidedObject);
    }
}