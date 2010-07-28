using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AvengersUtd.Odyssey.Graphics.Meshes;
using SlimDX;
using SlimDX.Direct3D11;

namespace AvengersUtd.Odyssey.Graphics.Effects
{

    public delegate Result UpdateInstanceParameter(InstanceParameter fxParam, IRenderable rObject);

    public class InstanceParameter : AbstractParameter
    {
        dynamic effectVariable;
        UpdateInstanceParameter update;

        public override dynamic EffectVariable
        {
            get { return effectVariable; }
        }

        /// <summary>
        /// Creates a description of a parameter used in a shader.
        /// </summary>
        /// <param name="name">The variable name used in the shader.</param>
        /// <param name="effect">A reference to the effect instance.</param>
        /// <param name="updateMethod">A delegate method that "polls" the updated value.</param>
        public InstanceParameter(string name, Effect effect, dynamic variableRef, UpdateInstanceParameter updateMethod)
            : base(name, effect)
        {
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
        /// light direction, positionV3, ambient color and so on.
        /// </summary>
        /// <param name="type">The type of the parameter to create.</param>
        /// <param name="effect">A reference of the effect instance.</param>
        /// <returns>An istance of EffectParameter containg all the needed information
        /// to correctly set and update this value into the shader.</returns>
        public static InstanceParameter CreateDefault(FXParameterType type, Effect effect)
        {
            string varName = string.Empty;
            dynamic eV = null;
            UpdateInstanceParameter update = null;

            switch (type)
            {

                case FXParameterType.ObjectWorld:
                    varName = ParamHandles.Matrices.World;
                    eV = effect.GetVariableByName(varName).AsMatrix();

                    update = (fxParam, rObject) => MatrixUpdate(fxParam.EffectVariable, rObject.World);
                    break;


            }

            return new InstanceParameter(varName, effect, eV, update);
        }

        public static InstanceParameter CreateDefault(InstanceVariable type, Effect effect)
        {
            string varName = string.Empty;
            dynamic eV = null;
            UpdateInstanceParameter update = null;

            switch (type)
            {

                case InstanceVariable.ObjectWorld:
                    varName = ParamHandles.Matrices.World;
                    eV = effect.GetVariableByName(varName).AsMatrix();

                    update = (fxParam, rObject) => MatrixUpdate(fxParam.EffectVariable, rObject.World);
                    break;
            }

            return new InstanceParameter(varName, effect, eV, update);
        }

    }
}
