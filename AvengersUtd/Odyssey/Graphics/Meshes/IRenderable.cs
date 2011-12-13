
using System;
using AvengersUtd.Odyssey.Geometry;
using AvengersUtd.Odyssey.Graphics.Materials;
using AvengersUtd.Odyssey.Graphics.Rendering.Management;
using AvengersUtd.Odyssey.Assets;
using SlimDX;
using SlimDX.Direct3D11;
using Buffer = SlimDX.Direct3D11.Buffer;

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
        bool Disposed { get; }
        bool IsInViewFrustum();

        Buffer IndexBuffer { get; }
        Buffer VertexBuffer { get; }
        VertexDescription VertexDescription { get; }
        ShaderResourceView[] ShaderResources { get; }

        Matrix World { get; set; }
        Matrix Translation { get; }
        Matrix Rotation { get; }
        Vector3 PositionV3 { get; set; }
        Vector4 PositionV4 { get; }
        Vector3 RotationDelta { get; set; }
        Quaternion CurrentRotation { get; set; }

        RenderableNode ParentNode { get; set; }
       
        /// <summary>
        /// Loads resources and inits object.
        /// </summary>
        void Init();

        /// <summary>
        /// Renders the entity with effects applied.
        /// </summary>
        void Render();
        void Render(int indexCount, int vertexOffset=0, int indexOffet=0, int startIndex=0, int baseVertex=0);

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