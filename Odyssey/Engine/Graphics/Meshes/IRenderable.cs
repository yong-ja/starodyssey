using System;
using AvengersUtd.Odyssey.Graphics.Materials;
using AvengersUtd.Odyssey.Graphics.Rendering.SceneGraph;
using AvengersUtd.Odyssey.Resources;
using SlimDX;
using SlimDX.Direct3D9;

namespace AvengersUtd.Odyssey.Graphics.Meshes
{
    public interface IRenderable : IDisposable, IEntity
    {
        EntityDescriptor Descriptor { get; }
        AbstractMaterial[] Materials { get; }
        Mesh Mesh { get; }

        bool IsCollidable { get; }
        bool CastsShadows { get; }
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
        /// Renders just the mesh without applying effects.
        /// </summary>
        void DrawMesh();

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