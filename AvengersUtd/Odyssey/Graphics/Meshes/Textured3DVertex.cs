#region #Disclaimer

// /* 
//  * Timer
//  *
//  * Created on 21 August 2007
//  * Last update on 29 July 2010
//  * 
//  * Author: Adalberto L. Simeone (Taranto, Italy)
//  * E-Mail: avengerdragon@gmail.com
//  * Website: http://www.avengersutd.com
//  *
//  * Part of the Odyssey Engine.
//  *
//  * This source code is Intellectual property of the Author
//  * and is released under the Creative Commons Attribution 
//  * NonCommercial License, available at:
//  * http://creativecommons.org/licenses/by-nc/3.0/ 
//  * You can alter and use this source code as you wish, 
//  * provided that you do not use the results in commercial
//  * projects, without the express and written consent of
//  * the Author.
//  *
//  */

#endregion

#region Using directives

using System.Runtime.InteropServices;
using AvengersUtd.Odyssey.Geometry;
using SlimDX;
using SlimDX.Direct3D11;
using SlimDX.DXGI;

#endregion

namespace AvengersUtd.Odyssey.Graphics.Meshes
{
    /// <summary>
    /// Represents a vertex with a position and a texture coordinate.
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Size=28)]
    public struct Textured3DVertex : IPositionVertex
    {
        [FieldOffset(0)]
        private Vector4 position;
        [FieldOffset(16)]
        private Vector3 textureCoordinate;

        /// <summary>
        /// Gets or sets the position of the vertex.
        /// </summary>
        
        public Vector4 Position
        {
            get { return position; }
            set { position = value; }
        }

        /// <summary>
        /// Gets or sets the texture coordinate for the vertex.
        /// </summary>
        public Vector3 TextureCoordinate
        {
            get { return textureCoordinate; }
            set { textureCoordinate = value; }
        }

        public const int Stride = 28;
        public const VertexFormat VertexFormat = Geometry.VertexFormat.PositionTextureUVW;

        private static readonly InputElement[] inputElements;
        private static readonly VertexDescription description = new VertexDescription(VertexFormat, Stride);

 
        static Textured3DVertex()
        {
            inputElements = new[]
                                {
                                    new InputElement("POSITION", 0, Format.R32G32B32A32_Float, 0, 0),
                                    new InputElement("TEXCOORD", 0, Format.R32G32B32_Float, 16, 0)
                                };
        }

        

        public static InputElement[] InputElements
        {
            get { return inputElements; }
        }


        public static VertexDescription Description
        {
            get { return description; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TexturedVertex"/> struct.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="Color4">The Color.</param>
        public Textured3DVertex(Vector4 position, Vector3 textureCoordinate)
            : this()
        {
            Position = position;
            TextureCoordinate = textureCoordinate;
        }

        /// <summary>
        /// Implements operator ==.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(Textured3DVertex left, Textured3DVertex right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Implements operator !=.
        /// </summary>
        /// <param name="left">The left side of the operator.</param>
        /// <param name="right">The right side of the operator.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(Textured3DVertex left, Textured3DVertex right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer that is the hash code for this instance.
        /// </returns>
        public override int GetHashCode()
        {
            return Position.GetHashCode() + TextureCoordinate.GetHashCode();
        }

        /// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <param name="obj">Another object to compare to.</param>
        /// <returns>
        /// true if <paramref name="obj"/> and this instance are the same type and represent the same value; otherwise, false.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (GetType() != obj.GetType())
                return false;

            return Equals((Textured3DVertex) obj);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        public bool Equals(Textured3DVertex other)
        {
            return (Position == other.Position && TextureCoordinate == other.TextureCoordinate);
        }
    }
}