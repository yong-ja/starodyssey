using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AvengersUtd.Odyssey.Geometry;
using SlimDX;
using SlimDX.DXGI;
using SlimDX.Direct3D11;

namespace AvengersUtd.Odyssey.Graphics.Meshes
{
    public struct MeshVertex : IMeshVertex
    {
        public Vector4 Position { get; set; }
        public Vector3 Normal { get; set; }
        /// <summary>
        /// Gets or sets the texture coordinate for the vertex.
        /// </summary>
        public Vector2 TextureUV { get; set; }

        public const int Stride = 36;
        public const VertexFormat VertexFormat = Geometry.VertexFormat.TexturedMesh;

        private static readonly InputElement[] inputElements;
        private static readonly VertexDescription description = new VertexDescription(VertexFormat, Stride);

        #region Properties
        public static InputElement[] InputElements
        {
            get { return inputElements; }
        }


        public static VertexDescription Description
        {
            get { return description; }
        } 
        #endregion

        static MeshVertex()
        {
            inputElements = new[]
                                {
                                    new InputElement("POSITION", 0, Format.R32G32B32A32_Float, 0, 0),
                                    new InputElement("NORMAL", 0, Format.R32G32B32_Float, 16, 0),
                                    new InputElement("TEXCOORD", 0, Format.R32G32_Float, 28, 0)
                                };
        }

        public MeshVertex(Vector4 position, Vector3 normal, Vector2 textureUV) : this()
        {
            Position = position;
            TextureUV = textureUV;
            Normal = normal;
        }

        #region Equality members
        public bool Equals(MeshVertex other)
        {
            return other.Position.Equals(Position) && other.Normal.Equals(Normal) && other.TextureUV.Equals(TextureUV);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (obj.GetType() != typeof(MeshVertex)) return false;
            return Equals((MeshVertex)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = Position.GetHashCode();
                result = (result * 397) ^ Normal.GetHashCode();
                result = (result * 397) ^ TextureUV.GetHashCode();
                return result;
            }
        } 
        #endregion
    }
}
