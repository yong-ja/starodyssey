
using System;
using System.Windows.Forms;
using AvengersUtd.Odyssey.Geometry;
using AvengersUtd.Odyssey.Graphics.Materials;
using AvengersUtd.Odyssey.Graphics.Rendering.Management;
using AvengersUtd.Odyssey.Assets;
using AvengersUtd.Odyssey.UserInterface.Controls;
using SlimDX;
using SlimDX.Direct3D11;
using Buffer = SlimDX.Direct3D11.Buffer;

namespace AvengersUtd.Odyssey.Graphics.Meshes
{

    public interface IMesh : IRenderable
    {
        Buffer IndexBuffer { get; }
        Buffer VertexBuffer { get; }
        VertexDescription VertexDescription { get; }
        ShaderResourceView[] ShaderResources { get; }
    }

    public interface IRenderable : IDisposable
    {
        //EntityDescriptor Descriptor { get; }
        //Mesh Mesh { get; }

        string Name { get; }
        bool Inited { get; }
        bool IsVisible { get; }
        bool CastsShadows { get; }
        bool Disposed { get; }
        bool IsInViewFrustum();

        Matrix World { get; set; }
        Matrix Translation { get; }
        Matrix Rotation { get; }
        Vector3 AbsolutePosition { get; }
        Vector3 PositionV3 { get; set; }
        Vector4 PositionV4 { get; }
        Vector3 RotationDelta { get; set; }
        Quaternion CurrentRotation { get; set; }

        RenderableNode ParentNode { get; set; }
        IColorMaterial Material { get; }

        /// <summary>
        /// Loads resources and inits object.
        /// </summary>
        void Init();
        /// <summary>
        /// Renders the entity with effects applied.
        /// </summary>
        void Render();
        void Render(int indexCount, int vertexOffset=0, int indexOffet=0, int startIndex=0, int baseVertex=0);
        void SetBehaviour(IMouseBehaviour mouseBehaviour);
        void SetBehaviour(IGamepadBehaviour gBehaviour);
        void RemoveBehaviour(IMouseBehaviour mouseBehaviour);
        void RemoveBehaviour(IGamepadBehaviour gamepadBehaviour);
        void ProcessMouseEvent(MouseEventType type, MouseEventArgs e);
        bool HasBehaviour(string behaviourName);

    }
}