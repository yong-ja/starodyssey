using System;
using System.Reflection;
using AvengersUtd.Odyssey.Graphics;
using AvengersUtd.Odyssey.Graphics.Materials;
using SlimDX;
using SlimDX.Direct3D11;
using AvengersUtd.Odyssey.Graphics.Meshes;


namespace AvengersUtd.Odyssey.Graphics.Effects
{


    public delegate Result UpdateSharedParameter(SharedParameter fxParam);
   


    public class SharedParameter : AbstractParameter, IDisposable
   {
        readonly dynamic effectVariable;
        UpdateSharedParameter update;

        public override dynamic EffectVariable
        {
            get { return effectVariable; }
        }


        protected override void OnDispose()
        {
            update -= update;
        }

        /// <summary>
        /// Creates a description of a parameter used in a shader.
        /// </summary>
        /// <param name="name">The variable name used in the shader.</param>
        /// <param name="effect">A reference to the effect instance.</param>
        /// <param name="updateMethod">A delegate method that "polls" the updated value.</param>
        public SharedParameter(string name, Effect effect, dynamic variableRef, UpdateSharedParameter updateMethod)
            : base(name, effect)
        {
            effectVariable = variableRef;
            update = updateMethod ;
        }

        public void Apply()
        {
            update(this);
        }


        /// <summary>
        /// Creates a "default parameter", such as World, View, Projection Matrices, 
        /// light direction, positionV3, ambient Color and so on.
        /// </summary>
        /// <param name="type">The type of the parameter to create.</param>
        /// <param name="effect">A reference of the effect instance.</param>
        /// <returns>An istance of EffectParameter containg all the needed information
        /// to correctly set and update this value into the shader.</returns>
        public static SharedParameter Create(SceneVariable type, Effect effect)
        {
            string varName = string.Empty;
            dynamic eV = null;
            UpdateSharedParameter update = null;
           
            switch (type)
            {

                //case SceneVariable.LightWorldViewProjection:
                //    varName = ParamHandles.Matrices.LightWorldViewProjection;
                //    eV = effect.GetVariableByName(varName);

                //    update = delegate(EffectParameter fxParam)
                //    {
                //        Matrix mLightVP =
                //            BaseLight.CreateLightViewProjectionMatrix(
                //                (Spotlight) Game.CurrentRenderer.LightManager.GetLight(0));

                //        Matrix mWorld = Game.CurrentRenderer.Camera.World;

                //        fxParam.ownerEffect.SetValue(eH,mWorld * mLightVP);
                //    };
                //    break;

                case SceneVariable.EyePosition:
                    varName = ParamHandles.Vectors.EyePosition;
                    eV = effect.GetVariableByName(varName).AsVector();

                    update = (fxParam => Vector3Update(fxParam.EffectVariable, Game.CurrentRenderer.Camera.PositionV3));
                    break;

                case SceneVariable.FarClip:
                    varName = ParamHandles.Floats.FarClip;
                    eV = effect.GetVariableByName(varName).AsScalar();
                    update = (fxParam => FloatUpdate(fxParam.EffectVariable, Game.CurrentRenderer.Camera.FarClip));
                    break;

                case SceneVariable.CameraProjection:
                    varName = ParamHandles.Matrices.Projection;
                    eV = effect.GetVariableByName(varName).AsMatrix();

                    update = (fxParam => MatrixUpdate(fxParam.EffectVariable, Game.CurrentRenderer.Camera.Projection));
                    break;

                case SceneVariable.CameraProjectionTranspose:
                    varName = ParamHandles.Matrices.Projection;
                    eV = effect.GetVariableByName(varName).AsMatrix();

                    update = (fxParam => MatrixUpdate(fxParam.EffectVariable, Matrix.Transpose(Game.CurrentRenderer.Camera.Projection)));
                    break;

                //case SceneVariable.TextureBias:
                //    varName = ParamHandles.Matrices.TextureBias;
                //    eV = effect.GetVariableByName(varName);
                //    update =
                //        (fxParam => fxParam.ownerEffect.SetValue(
                //                        eH,
                //                        BaseLight.CreateTextureBiasMatrix(256, 0.001f))
                //                        );
                //    break;

                
                case SceneVariable.CameraView:
                    varName = ParamHandles.Matrices.View;
                    eV = effect.GetVariableByName(varName).AsMatrix();

                    update = (fxParam => MatrixUpdate(fxParam.EffectVariable, Game.CurrentRenderer.Camera.View));
                    break;

                case SceneVariable.CameraRotation:
                    varName = ParamHandles.Matrices.View;
                    eV = effect.GetVariableByName(varName).AsMatrix();

                    update = (fxParam => MatrixUpdate(fxParam.EffectVariable, Game.CurrentRenderer.Camera.Rotation));
                    break;

                case SceneVariable.CameraViewTranspose:
                    varName = ParamHandles.Matrices.View;
                    eV = effect.GetVariableByName(varName).AsMatrix();

                    update = (fxParam => MatrixUpdate(fxParam.EffectVariable, Matrix.Transpose(Game.CurrentRenderer.Camera.View)));
                    break;

                case SceneVariable.CameraViewInverse:
                    varName = ParamHandles.Matrices.ViewInverse;
                    eV = effect.GetVariableByName(varName).AsMatrix();

                     update = (fxParam => MatrixUpdate(fxParam.EffectVariable, Matrix.Invert(Game.CurrentRenderer.Camera.View)));
                    break;

                case SceneVariable.CameraWorld:
                    varName = ParamHandles.Matrices.World;
                    eV = effect.GetVariableByName(varName).AsMatrix();

                    update = (fxParam => MatrixUpdate(fxParam.EffectVariable, Game.CurrentRenderer.Camera.World));
                    break;

                case SceneVariable.CameraWorldInverse:
                    varName = ParamHandles.Matrices.WorldInverse;
                    eV = effect.GetVariableByName(varName).AsMatrix();

                    update = (fxParam => MatrixUpdate(fxParam.EffectVariable, Matrix.Invert(Game.CurrentRenderer.Camera.World)));
                    break;

                case SceneVariable.CameraWorldViewInverse:
                    varName = ParamHandles.Matrices.WorldViewInverse;
                    eV = effect.GetVariableByName(varName).AsMatrix();

                    update = (fxParam => MatrixUpdate(fxParam.EffectVariable, Matrix.Invert(Game.CurrentRenderer.Camera.World * Game.CurrentRenderer.Camera.View)));
                    break;

                case SceneVariable.CameraWorldViewProjection:
                    varName = ParamHandles.Matrices.WorldViewProjection;
                    eV = effect.GetVariableByName(varName).AsMatrix();

                    update =
                        (fxParam =>
                         MatrixUpdate(fxParam.EffectVariable,
                                      Matrix.Invert(Game.CurrentRenderer.Camera.World*Game.CurrentRenderer.Camera.View*
                                                    Game.CurrentRenderer.Camera.Projection)));
                    break;

                case SceneVariable.CameraOrthographicProjection:
                    varName = ParamHandles.Matrices.Projection;
                    eV = effect.GetVariableByName(varName).AsMatrix();
                    update = (fxParam => MatrixUpdate(fxParam.EffectVariable, Game.CurrentRenderer.Camera.OrthoProjection));
                    break;

            }

            return new SharedParameter(varName, effect, eV, update);
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
        public static SharedParameter CreateCustom(string varName, Effect effect, float value)
		 {
		     EffectScalarVariable eV = effect.GetVariableByName(varName).AsScalar();
		     UpdateSharedParameter update = (fxParam => FloatUpdate(fxParam.EffectVariable, value));
		     return new SharedParameter(varName, effect, eV, update);
		 }

        public static SharedParameter CreateCustom(string varName, Effect effect, int value)
        {
            EffectScalarVariable eV = effect.GetVariableByName(varName).AsScalar();
            UpdateSharedParameter update = (fxParam => IntUpdate(fxParam.EffectVariable, value));
            return new SharedParameter(varName, effect, eV, update);
        }

        public static SharedParameter CreateCustom(String varName, Effect effect, Color4 value)
        {
            EffectVectorVariable eV = effect.GetVariableByName(varName).AsVector();
            UpdateSharedParameter update = (fxParam =>Color44Update(fxParam.EffectVariable, value));
            return new SharedParameter(varName, effect, eV, update);
        }

        public static SharedParameter CreateCustom(String varName, Effect effect, Matrix value)
        {
            EffectMatrixVariable eV = effect.GetVariableByName(varName).AsMatrix();
            UpdateSharedParameter update = (fxParam => MatrixUpdate(fxParam.EffectVariable, value));
            return new SharedParameter(varName, effect, eV, update);
        }

        public static SharedParameter CreateCustom(String varName, Effect effect, Texture2D value)
        {
            EffectResourceVariable eV = effect.GetVariableByName(varName).AsResource();
            UpdateSharedParameter update = (fxParam => TextureUpdate(fxParam.EffectVariable, value));
            return new SharedParameter(varName, effect, eV, update);
        }

        //public static EffectParameter CreateCustom(String varName, Effect effect, ICastsShadows value)
        //{
        //    EffectHandle eV = effect.GetVariableByName(varName);
        //    Update update = fxParam => fxParam.ownerEffect.SetTexture(eH, value.ShadowMap);
        //    return new EffectParameter(varName, effect, update);
        //}

        public static SharedParameter CreateCustom(String varName, Effect effect, Vector4 value)
        {
            EffectVectorVariable eV = effect.GetVariableByName(varName).AsVector();
            UpdateSharedParameter update = (fxParam => Vector4Update(fxParam.EffectVariable, value));
            return new SharedParameter(varName, effect, eV, update);
        }

        public static SharedParameter CreateCustom(String varName, Effect effect, FloatOp floatOp)
        {
            EffectScalarVariable eV = effect.GetVariableByName(varName).AsScalar();
            UpdateSharedParameter update = (fxParam => FloatUpdate(fxParam.EffectVariable, floatOp()));
            return new SharedParameter(varName, effect, eV, update);
        }

        public static SharedParameter CreateCustom(String varName, Effect effect, MatrixOp matrixOp)
        {
            EffectMatrixVariable eV = effect.GetVariableByName(varName).AsMatrix();
            UpdateSharedParameter update = (fxParam => MatrixUpdate(fxParam.EffectVariable, matrixOp()));
            return new SharedParameter(varName, effect, eV, update);
        }

        /// <summary>
        /// Creates a custom EffectParameter that sets a dynamic vector value in the shader.
        /// </summary>
        /// <param name="varName">The variable name used in the shader.</param>
        /// <param name="effect">A reference to the effect instance.</param>
        /// <param name="vectorOp">The vector value to set.</param>
        /// <returns>An istance of EffectParameter containg all the needed information
        /// to correctly set and update this value into the shader.</returns>
        public static SharedParameter CreateCustom(String varName, Effect effect, VectorOp vectorOp)
        {
            EffectVectorVariable eV = effect.GetVariableByName(varName).AsVector();
            UpdateSharedParameter update = (fxParam => Vector4Update(fxParam.EffectVariable, vectorOp()));
            return new SharedParameter(varName, effect, eV, update);
        }

        
	#endregion



   }
}