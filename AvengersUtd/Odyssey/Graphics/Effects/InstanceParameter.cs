using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AvengersUtd.Odyssey.Graphics.Materials;
using AvengersUtd.Odyssey.Graphics.Meshes;
using SlimDX;
using SlimDX.Direct3D11;
using AvengersUtd.Odyssey.Utils.Logging;

namespace AvengersUtd.Odyssey.Graphics.Effects
{
    public delegate Result UpdateInstanceParameter(InstanceParameter fxParam, IRenderable rObject);

    public class InstanceParameter : AbstractParameter
    {
        private dynamic effectVariable;
        private UpdateInstanceParameter update;
        public InstanceVariable Type { get; private set; }

        public override dynamic EffectVariable
        {
            get
            {
                return effectVariable;
            }
        }

        /// <summary>
        /// Creates a description of a parameter used in a shader.
        /// </summary>
        /// <param name="name">The variable name used in the shader.</param>
        /// <param name="type">The identifier of the variable used in the shader.</param>
        /// <param name="effect">A reference to the effect instance.</param>
        /// <param name="updateMethod">A delegate method that "polls" the updated value.</param>
        public InstanceParameter(string name, InstanceVariable type, Effect effect, dynamic variableRef, UpdateInstanceParameter updateMethod)
            : base(name, effect)
        {
            Type = type;
            effectVariable = variableRef;
            update = updateMethod;
        }

        protected override void OnDispose()
        {
            update -= update;
        }

        public void Apply(IRenderable rObject)
        {
            update(this, rObject);
        }

        /// <summary>
        /// Creates a "default parameter", such as World, View, Projection Matrices, 
        /// light direction, positionV3, ambient Color and so on.
        /// </summary>
        /// <param name="type">The type of the parameter to create.</param>
        /// <param name="effect">A reference of the effect instance.</param>
        /// <returns>An istance of EffectParameter containg all the needed information
        /// to correctly set and update this value into the shader.</returns>
        public static InstanceParameter Create(InstanceVariable type, Effect effect)
        {
            string varName = string.Empty;
            dynamic eV = null;
            UpdateInstanceParameter update = null;

            VerboseEvent.InstanceParameterCreation.Log(type.ToString());

            switch (type)
            {
                case InstanceVariable.ObjectWorld:
                    varName = ParamHandles.Matrices.World;
                    eV = effect.GetVariableByName(varName).AsMatrix();

                    update = (fxParam, rObject) => MatrixUpdate(fxParam.EffectVariable, rObject.World);
                    break;

                case InstanceVariable.LightAmbient:
                    varName = ParamHandles.Color4s.LightAmbient;
                    eV = effect.GetVariableByName(varName).AsVector();

                    update =
                        (fxParam, rObject) =>
                        Color44Update(fxParam.EffectVariable,
                                     ((IColorMaterial)rObject.Material).AmbientColor);
                    break;

                case InstanceVariable.Diffuse:
                    varName = ParamHandles.Color4s.MaterialDiffuse;
                    eV = effect.GetVariableByName(varName).AsVector();
                    update =
                        (fxParam, rObject) =>
                        Color44Update(fxParam.EffectVariable,
                                     ((IColorMaterial)rObject.Material).DiffuseColor);
                    break;

                case InstanceVariable.Ambient:
                    varName = ParamHandles.Color4s.MaterialAmbient;
                    eV = effect.GetVariableByName(varName).AsVector();
                    update =
                        (fxParam, rObject) => Color44Update(fxParam.EffectVariable, ((IColorMaterial)rObject.Material).AmbientColor);
                    break;

                case InstanceVariable.Specular:
                    varName = ParamHandles.Color4s.MaterialSpecular;
                    eV = effect.GetVariableByName(varName).AsVector();
                    update =
                        (fxParam, rObject) =>
                        Color44Update(fxParam.EffectVariable,
                                     ((IColorMaterial)rObject.Material).SpecularColor);
                    break;

                case InstanceVariable.DiffuseMap:
                    varName = ParamHandles.Textures.DiffuseMap;
                    eV = effect.GetVariableByName(varName).AsResource();
                    update=(fxParam, rObject) =>
                        ResourceUpdate(fxParam.EffectVariable,
                                     ((IDiffuseMap)rObject).DiffuseMapResource);
                    break;

                case InstanceVariable.AmbientCoefficient:
                    varName = ParamHandles.Color4s.MaterialkA;
                    eV = effect.GetVariableByName(varName).AsScalar();
                    update = (fxParam, rObject) => FloatUpdate(fxParam.EffectVariable, ((IColorMaterial)rObject.Material).AmbientCoefficient);
                    break;

                case InstanceVariable.DiffuseCoefficient:
                    varName = ParamHandles.Color4s.MaterialkD;
                    eV = effect.GetVariableByName(varName).AsScalar();
                    update = (fxParam, rObject) => FloatUpdate(fxParam.EffectVariable, ((IColorMaterial)rObject.Material).AmbientCoefficient);
                    break;

                case InstanceVariable.MaterialBuffer:
                    varName = ParamHandles.CBuffers.MaterialBuffer;
                    eV = effect.GetConstantBufferByName(varName).AsConstantBuffer();
                    update = (fxParam, rObject) => BufferUpdate(fxParam.EffectVariable, MaterialBuffer.FromMaterial((ShaderMaterial)rObject.Material));
                    break;


            }

            return new InstanceParameter(varName, type, effect, eV, update);
        }
    }
}