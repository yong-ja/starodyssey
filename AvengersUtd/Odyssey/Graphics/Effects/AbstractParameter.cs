using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX;
using SlimDX.Direct3D11;

namespace AvengersUtd.Odyssey.Graphics.Effects
{
    public delegate Vector4 VectorOp();
    public delegate float FloatOp();
    public delegate Matrix MatrixOp();

    public abstract class AbstractParameter
    {

        string name;
        bool disposed;

        private Effect ownerEffect;

        public abstract dynamic EffectVariable { get; }

        public string Name
        {
            get { return name; }
        }

        public Effect OwnerEffect
        {
            get { return ownerEffect; }
        }

        /// <summary>
        /// Creates a description of a parameter used in a shader.
        /// </summary>
        /// <param name="name">The variable name used in the shader.</param>
        /// <param name="effect">A reference to the effect instance.</param>
        /// <param name="updateMethod">A delegate method that "polls" the updated value.</param>
        protected AbstractParameter(string name, Effect effect)
        {
            this.name = name;
            ownerEffect = effect;
        }

        protected abstract void OnDispose();

        #region IDisposable Members

        /// <summary>
        /// Dispose meshObject and materials
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    // dispose managed components
                    OnDispose();
                }

                // dispose unmanaged components
            }
            disposed = true;
        }

        ~AbstractParameter()
        {
            Dispose(false);
        }
        #endregion

         #region Update delegates
        public static Result IntUpdate(EffectScalarVariable scalarVar, int var)
        {

            return scalarVar.Set(var);
        }

        public static Result FloatUpdate(EffectScalarVariable scalarVar, float var)
        {
            return scalarVar.Set(var);
        }

        public static Result MatrixUpdate(EffectMatrixVariable matrixVar, Matrix matrix)
        {
            return matrixVar.SetMatrix(matrix);
        }

        public static Result Vector3Update(EffectVectorVariable vectorVar, Vector3 vector3)
        {
            return vectorVar.Set(vector3);
        }

        public static Result Vector4Update(EffectVectorVariable vectorVar, Vector4 vector4)
        {
            return vectorVar.Set(vector4);
        }

        public static Result Color44Update(EffectVectorVariable vectorVar, Color4 vector4)
        {
            return vectorVar.Set(vector4);
        }

        public static Result ResourceUpdate(EffectResourceVariable resourceVar, ShaderResourceView texture)
        {
            return resourceVar.SetResource(texture);
        }

        public static Result TextureUpdate(EffectResourceVariable textureVar, Texture2D texture)
        {
            return textureVar.SetResource(new ShaderResourceView(Game.Context.Device, texture));
        } 
        #endregion

    }
}
