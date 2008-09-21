using System;
using System.Reflection;
using SlimDX.Direct3D9;
using SlimDX;
using AvengersUtd.Odyssey.Objects.Entities;
using AvengersUtd.Odyssey.Engine;
using AvengersUtd.Odyssey.Graphics.Materials;

namespace AvengersUtd.Odyssey.Graphics.Effects
{
    public delegate void Update(EffectParameter ep);

    public delegate Vector4 VectorOp();

    public delegate float FloatOp();

    public delegate Matrix MatrixOp();

    public class EffectParameter : IDisposable
    {
        string name;
        bool disposed;
        EffectHandle effectHandle;
        Effect ownerEffect;
        Update update;
 
        public string Name
        {
            get { return name; }
        }

        internal Effect OwnerEffect
        {
            get { return ownerEffect; }
        }

        /// <summary>
        /// Creates a description of a parameter used in a shader.
        /// </summary>
        /// <param name="name">The variable name used in the shader.</param>
        /// <param name="effect">A reference to the effect instance.</param>
        /// <param name="updateMethod">A delegate method that "polls" the updated value.</param>
        public EffectParameter(string name, Effect effect, Update updateMethod)
        {
            this.name = name;

            effectHandle = effect.GetParameter(null, name);
            ownerEffect = effect;
            update = updateMethod;
        }

        /// <summary>
        /// Sets the value into the shader.
        /// </summary>
        public void Apply()
        {
            update(this);
        }


        /// <summary>
        /// Creates a "default parameter", such as World, View, Projection Matrices, 
        /// light direction, positionV3, ambient color and so on.
        /// </summary>
        /// <param name="type">The type of the parameter to create.</param>
        /// <param name="effect">A reference of the effect instance.</param>
        /// <returns>An istance of EffectParameter containg all the needed information
        /// to correctly set and update this value into the shader.</returns>
        public static EffectParameter DefaultParameter(FXParameterType type, Effect effect)
        {
            string varName = string.Empty;
            Update update = null;
            EffectHandle eH;

            switch (type)
            {

                case FXParameterType.LightWorldViewProjection:
                    varName = ParamHandles.Matrices.LightWorldViewProjection;
                    eH = effect.GetParameter(null, varName);

                    update = delegate(EffectParameter fxParam)
                    {
                        Matrix mLightVP =
                            BaseLight.CreateLightViewProjectionMatrix(
                                (Spotlight) Game.CurrentScene.LightManager.GetLight(0));

                        Matrix mWorld = Game.CurrentScene.Camera.World;

                        fxParam.ownerEffect.SetValue(eH,mWorld * mLightVP);
                    };
                    break;

                case FXParameterType.EyePosition:
                    varName = ParamHandles.Vectors.EyePosition;
                    update = delegate(EffectParameter fxParam)
                    {
                        fxParam.ownerEffect.SetValue(fxParam.effectHandle,
                                                     Game.CurrentScene.Camera.PositionV4);
                    };
                    break;

                case FXParameterType.FarClip:
                    varName = ParamHandles.Floats.FarClip;
                    eH = effect.GetParameter(null, varName);
                    update = (fxParam => fxParam.ownerEffect.SetValue(eH,
                                                                      Game.CurrentScene.Camera.FarClip));
                    break;

                case FXParameterType.Projection:
                    varName = ParamHandles.Matrices.Projection;
                    eH = effect.GetParameter(null, varName);

                    update =
                        delegate(EffectParameter fxParam) { fxParam.ownerEffect.SetValue(eH, Game.CurrentScene.Camera.Projection); };
                    break;

                case FXParameterType.TextureBias:
                    varName = ParamHandles.Matrices.TextureBias;
                    eH = effect.GetParameter(null, varName);
                    update =
                        (fxParam => fxParam.ownerEffect.SetValue(
                                        eH,
                                        BaseLight.CreateTextureBiasMatrix(512, 0.001f))
                                        );
                    break;

                
                case FXParameterType.View:
                    varName = ParamHandles.Matrices.View;
                    eH = effect.GetParameter(null, varName);

                    update =
                        delegate(EffectParameter fxParam) { fxParam.ownerEffect.SetValue(eH, Game.CurrentScene.Camera.View); };
                    break;

                case FXParameterType.ViewInverse:
                    varName = ParamHandles.Matrices.ViewInverse;
                    eH = effect.GetParameter(null, varName);

                    update =
                        delegate(EffectParameter fxParam) { fxParam.ownerEffect.SetValue(eH, Matrix.Invert(Game.CurrentScene.Camera.View)); };
                    break;

                case FXParameterType.World:
                    varName = ParamHandles.Matrices.World;
                    eH = effect.GetParameter(null, varName);

                    update =
                        delegate(EffectParameter fxParam) { fxParam.ownerEffect.SetValue(eH, Game.CurrentScene.Camera.World); };
                    break;

                case FXParameterType.WorldInverse:
                    varName = ParamHandles.Matrices.WorldInverse;
                    eH = effect.GetParameter(null, varName);

                    update =
                        delegate(EffectParameter fxParam) { fxParam.ownerEffect.SetValue(eH, Matrix.Invert(Game.CurrentScene.Camera.World)); };
                    break;

                case FXParameterType.WorldViewInverse:
                    varName = ParamHandles.Matrices.WorldViewInverse;
                    eH = effect.GetParameter(null, varName);

                    update =
                        delegate(EffectParameter fxParam) { fxParam.ownerEffect.SetValue(eH, Matrix.Invert(Game.CurrentScene.Camera.View)); };
                    break;

                case FXParameterType.WorldViewProjection:
                    varName = ParamHandles.Matrices.WorldViewProjection;
                    eH = effect.GetParameter(null, varName);

                    update = (fxParam => fxParam.ownerEffect.SetValue(eH, Game.CurrentScene.Camera.World *
                                                                          Game.CurrentScene.Camera.View *
                                                                          Game.CurrentScene.Camera.Projection));
                    break;

               
                

               

                case FXParameterType.AmbientColor:
                    varName = ParamHandles.Colors.LightAmbient;
                    eH = effect.GetParameter(null, varName);

                    update = delegate(EffectParameter fxParam)
                                 {
                                     fxParam.ownerEffect.SetValue(eH,
                                                                  Game.CurrentScene.LightManager.GetParameter(0,
                                                                                                              FXParameterType
                                                                                                                  .
                                                                                                                  AmbientColor));
                                 };
                    break;


            }

            return new EffectParameter(varName, effect, update);
        }

        

       #region Static Methods
		 /// <summary>
        /// Creates a custom EffectParameter that sets a constant float value in the shader.
        /// </summary>
        /// <param name="varName">The variable name used in the shader.</param>
        /// <param name="effect">A reference to the effect instance.</param>
        /// <param name="value">The float value to set.</param>
        /// <returns>An istance of EffectParameter containg all the needed information
        /// to correctly set and update this value into the shader.</returns>
        public static EffectParameter CreateCustomParameter(string varName, Effect effect, float value)
        {
            EffectHandle eH = effect.GetParameter(null, varName);
            Update update = delegate(EffectParameter fxParam) { fxParam.ownerEffect.SetValue(eH, value); };
            return new EffectParameter(varName, effect, update);
        }

        public static EffectParameter CreateCustomParameter(string varName, Effect effect, int value)
        {
            EffectHandle eH = effect.GetParameter(null, varName);
            Update update = delegate(EffectParameter fxParam) { fxParam.ownerEffect.SetValue(eH, value); };
            return new EffectParameter(varName, effect, update);
        }

        public static EffectParameter CreateCustomParameter(String varName, Effect effect, Color4 value)
        {
            EffectHandle eH = effect.GetParameter(null, varName);
            Update update = delegate(EffectParameter fxParam) { fxParam.ownerEffect.SetValue(eH, value); };
            return new EffectParameter(varName, effect, update);
        }

        public static EffectParameter CreateCustomParameter(String varName, Effect effect, Matrix value)
        {
            EffectHandle eH = effect.GetParameter(null, varName);
            Update update = delegate(EffectParameter fxParam) { fxParam.ownerEffect.SetValue(eH, value); };
            return new EffectParameter(varName, effect, update);
        }

        public static EffectParameter CreateCustomParameter(String varName, Effect effect, Texture value)
        {
            EffectHandle eH = effect.GetParameter(null, varName);
            Update update = fxParam => fxParam.ownerEffect.SetTexture(eH, value);
            return new EffectParameter(varName, effect, update);
        }

        public static EffectParameter CreateCustomParameter(String varName, Effect effect, ICastsShadows value)
        {
            EffectHandle eH = effect.GetParameter(null, varName);
            Update update = fxParam => fxParam.ownerEffect.SetTexture(eH, value.ShadowMap);
            return new EffectParameter(varName, effect, update);
        }

        public static EffectParameter CreateCustomParameter(String varName, Effect effect, Vector4 value)
        {
            EffectHandle eH = effect.GetParameter(null, varName);
            Update update = fxParam => fxParam.ownerEffect.SetValue(eH, value);
            return new EffectParameter(varName, effect, update);
        }

        public static EffectParameter CreateCustomParameter(String varName, Effect effect, FloatOp floatOp)
        {
            EffectHandle eH = effect.GetParameter(null, varName);
            Update update = delegate(EffectParameter fxParam) { fxParam.ownerEffect.SetValue(eH, floatOp()); };
            return new EffectParameter(varName, effect, update);
        }

        public static EffectParameter CreateCustomParameter(String varName, Effect effect, MatrixOp matrixOp)
        {
            EffectHandle eH = effect.GetParameter(null, varName);
            Update update = delegate(EffectParameter fxParam) { fxParam.ownerEffect.SetValue(eH, matrixOp()); };
            return new EffectParameter(varName, effect, update);
        }

        /// <summary>
        /// Creates a custom EffectParameter that sets a dynamic vector value in the shader.
        /// </summary>
        /// <param name="varName">The variable name used in the shader.</param>
        /// <param name="effect">A reference to the effect instance.</param>
        /// <param name="vectorOp">The vector value to set.</param>
        /// <returns>An istance of EffectParameter containg all the needed information
        /// to correctly set and update this value into the shader.</returns>
        public static EffectParameter CreateCustomParameter(String varName, Effect effect, VectorOp vectorOp)
        {
            EffectHandle eH = effect.GetParameter(null, varName);
            Update update = delegate(EffectParameter fxParam) { fxParam.ownerEffect.SetValue(eH, vectorOp()); };
            return new EffectParameter(varName, effect, update);
        } 
	#endregion


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
                    // release invocation list.
                    update -= update;
                }

                // dispose unmanaged components
                
            }
            disposed = true;
        }

        ~EffectParameter()
        {
            Dispose(false);
        }
        #endregion
    }
}