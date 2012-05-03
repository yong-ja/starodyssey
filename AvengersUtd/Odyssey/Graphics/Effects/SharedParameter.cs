using System;
using System.Reflection;
using AvengersUtd.Odyssey.Graphics;
using AvengersUtd.Odyssey.Graphics.Materials;
using AvengersUtd.Odyssey.Graphics.Rendering;
using SlimDX;
using SlimDX.Direct3D11;
using AvengersUtd.Odyssey.Graphics.Meshes;


namespace AvengersUtd.Odyssey.Graphics.Effects
{


    public delegate Result UpdateSharedParameter(SharedParameter fxParam, Renderer renderer);



    public class SharedParameter : AbstractParameter, IDisposable
    {
        readonly dynamic effectVariable;
        UpdateSharedParameter update;

        public SceneVariable Type { get; private set; }

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
        /// <param name="type">An identifier of the variable used in the shader.</param>
        /// <param name="effect">A reference to the effect instance.</param>
        /// <param name="updateMethod">A delegate method that "polls" the updated value.</param>
        public SharedParameter(string name, SceneVariable type, Effect effect, dynamic variableRef, UpdateSharedParameter updateMethod)
            : base(name, effect)
        {
            Type = type;
            effectVariable = variableRef;
            update = updateMethod;
        }

        public void Apply(Renderer rendererContext)
        {
            update(this,rendererContext);
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

                //        Matrix mWorld = renderer.Camera.World;

                //        fxParam.ownerEffect.SetValue(eH,mWorld * mLightVP);
                //    };
                //    break;

                case SceneVariable.EyePosition:
                    varName = ParamHandles.Vectors.EyePosition;
                    eV = effect.GetVariableByName(varName).AsVector();

                    update = ((fxParam, renderer) => Vector3Update(fxParam.EffectVariable, renderer.Camera.PositionV3));
                    break;

                case SceneVariable.FarClip:
                    varName = ParamHandles.Floats.FarClip;
                    eV = effect.GetVariableByName(varName).AsScalar();
                    update = ((fxParam, renderer) => FloatUpdate(fxParam.EffectVariable, renderer.Camera.FarClip));
                    break;

                case SceneVariable.CameraProjection:
                    varName = ParamHandles.Matrices.Projection;
                    eV = effect.GetVariableByName(varName).AsMatrix();

                    update = ((fxParam, renderer) => MatrixUpdate(fxParam.EffectVariable, renderer.Camera.Projection));
                    break;

                case SceneVariable.CameraProjectionTranspose:
                    varName = ParamHandles.Matrices.Projection;
                    eV = effect.GetVariableByName(varName).AsMatrix();

                    update = ((fxParam, renderer) => MatrixUpdate(fxParam.EffectVariable, Matrix.Transpose(renderer.Camera.Projection)));
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

                    update = ((fxParam, renderer) => MatrixUpdate(fxParam.EffectVariable, renderer.Camera.View));
                    break;

                case SceneVariable.CameraRotation:
                    varName = ParamHandles.Matrices.View;
                    eV = effect.GetVariableByName(varName).AsMatrix();

                    update = ((fxParam, renderer) => MatrixUpdate(fxParam.EffectVariable, renderer.Camera.Rotation));
                    break;

                case SceneVariable.CameraViewTranspose:
                    varName = ParamHandles.Matrices.View;
                    eV = effect.GetVariableByName(varName).AsMatrix();

                    update = ((fxParam, renderer) => MatrixUpdate(fxParam.EffectVariable, Matrix.Transpose(renderer.Camera.View)));
                    break;

                case SceneVariable.CameraViewInverse:
                    varName = ParamHandles.Matrices.ViewInverse;
                    eV = effect.GetVariableByName(varName).AsMatrix();

                    update = ((fxParam, renderer) => MatrixUpdate(fxParam.EffectVariable, Matrix.Invert(renderer.Camera.View)));
                    break;

                case SceneVariable.CameraWorld:
                    varName = ParamHandles.Matrices.World;
                    eV = effect.GetVariableByName(varName).AsMatrix();

                    update = ((fxParam, renderer) => MatrixUpdate(fxParam.EffectVariable, renderer.Camera.World));
                    break;

                case SceneVariable.CameraWorldInverse:
                    varName = ParamHandles.Matrices.WorldInverse;
                    eV = effect.GetVariableByName(varName).AsMatrix();

                    update = ((fxParam, renderer) => MatrixUpdate(fxParam.EffectVariable, Matrix.Invert(renderer.Camera.World)));
                    break;

                    case SceneVariable.CameraWorldView:
                    varName = ParamHandles.Matrices.WorldView;
                    eV = effect.GetVariableByName(varName).AsMatrix();

                    update = (fxParam, renderer) => MatrixUpdate(fxParam.EffectVariable, 
                        Matrix.Multiply(renderer.Camera.World,renderer.Camera.View));
                    break;

                case SceneVariable.CameraWorldViewInverse:
                    varName = ParamHandles.Matrices.WorldViewInverse;
                    eV = effect.GetVariableByName(varName).AsMatrix();

                    update = ((fxParam, renderer) => MatrixUpdate(fxParam.EffectVariable, Matrix.Invert(renderer.Camera.World * renderer.Camera.View)));
                    break;

                case SceneVariable.CameraWorldViewProjection:
                    varName = ParamHandles.Matrices.WorldViewProjection;
                    eV = effect.GetVariableByName(varName).AsMatrix();

                    update =
                        (fxParam, renderer) =>
                         MatrixUpdate(fxParam.EffectVariable,
                                      renderer.Camera.World * renderer.Camera.View * renderer.Camera.Projection);
                    break;

                case SceneVariable.CameraOrthographicProjection:
                    varName = ParamHandles.Matrices.Projection;
                    eV = effect.GetVariableByName(varName).AsMatrix();
                    update = ((fxParam, renderer) => MatrixUpdate(fxParam.EffectVariable, renderer.Camera.OrthoProjection));
                    break;

            }

            return new SharedParameter(varName, type, effect, eV, update);
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
            UpdateSharedParameter update = ((fxParam, renderer) => FloatUpdate(fxParam.EffectVariable, value));
            return new SharedParameter(varName, SceneVariable.CustomFloat, effect, eV, update);
        }

        public static SharedParameter CreateCustom(string varName, Effect effect, int value)
        {
            EffectScalarVariable eV = effect.GetVariableByName(varName).AsScalar();
            UpdateSharedParameter update = ((fxParam, renderer) => IntUpdate(fxParam.EffectVariable, value));
            return new SharedParameter(varName,SceneVariable.CustomInt, effect, eV, update);
        }

        public static SharedParameter CreateCustom(String varName, Effect effect, Color4 value)
        {
            EffectVectorVariable eV = effect.GetVariableByName(varName).AsVector();
            UpdateSharedParameter update = ((fxParam, renderer) => Color44Update(fxParam.EffectVariable, value));
            return new SharedParameter(varName, SceneVariable.CustomColor4, effect, eV, update);
        }

        public static SharedParameter CreateCustom(String varName, Effect effect, Matrix value)
        {
            EffectMatrixVariable eV = effect.GetVariableByName(varName).AsMatrix();
            UpdateSharedParameter update = ((fxParam, renderer) => MatrixUpdate(fxParam.EffectVariable, value));
            return new SharedParameter(varName, SceneVariable.CustomMatrix, effect, eV, update);
        }

        public static SharedParameter CreateCustom(String varName, Effect effect, Texture2D value)
        {
            EffectResourceVariable eV = effect.GetVariableByName(varName).AsResource();
            UpdateSharedParameter update = ((fxParam, renderer) => TextureUpdate(fxParam.EffectVariable, value));
            return new SharedParameter(varName, SceneVariable.CustomTexture2D, effect, eV, update);
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
            UpdateSharedParameter update = ((fxParam, renderer) => Vector4Update(fxParam.EffectVariable, value));
            return new SharedParameter(varName, SceneVariable.CustomVector4, effect, eV, update);
        }

        public static SharedParameter CreateCustom(String varName, Effect effect, FloatOp floatOp)
        {
            EffectScalarVariable eV = effect.GetVariableByName(varName).AsScalar();
            UpdateSharedParameter update = ((fxParam, renderer) => FloatUpdate(fxParam.EffectVariable, floatOp()));
            return new SharedParameter(varName, SceneVariable.FloatOp, effect, eV, update);
        }

        public static SharedParameter CreateCustom(String varName, Effect effect, MatrixOp matrixOp)
        {
            EffectMatrixVariable eV = effect.GetVariableByName(varName).AsMatrix();
            UpdateSharedParameter update = ((fxParam, renderer) => MatrixUpdate(fxParam.EffectVariable, matrixOp()));
            return new SharedParameter(varName, SceneVariable.MatrixOp, effect, eV, update);
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
            UpdateSharedParameter update = ((fxParam, renderer) => Vector4Update(fxParam.EffectVariable, vectorOp()));
            return new SharedParameter(varName, SceneVariable.VectorOp,effect, eV, update);
        }


        #endregion
    }
}