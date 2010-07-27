using System.Collections.Generic;
using AvengersUtd.Odyssey.Resources;
using SlimDX.Direct3D11;
using AvengersUtd.Odyssey.Graphics.Meshes;

namespace AvengersUtd.Odyssey.Graphics.Effects
{
    public class EffectDescriptor
    {
        string filename;
        private EffectTechnique technique;
        int pass = 0;
        readonly SortedList<string, InstanceParameter> instanceParameters;
        SortedList<string, SharedParameter> staticParameters;
        SortedList<string, SharedParameter> dynamicParameters;
        Effect effect;

        #region

        public EffectTechnique Technique
        {
            get { return technique; }
            set { technique = value; }
        }

        public int Pass
        {
            get { return pass; }
            set { pass = value; }
        }

        public Effect Effect
        {
            get { return effect; }
        }

        #endregion

        public EffectDescriptor(string filename)
        {
            this.filename = filename;
            instanceParameters = new SortedList<string, InstanceParameter>();
            staticParameters = new SortedList<string, SharedParameter>();
            dynamicParameters = new SortedList<string, SharedParameter>();
            effect = EffectManager.LoadEffect(Global.FXPath + filename);
            technique = effect.GetTechniqueByIndex(0);
        }

        /// <summary>
        /// Sets a custom static parameter ep. A static parameter is updated only once.
        /// </summary>
        /// <param name="ep">A custom parameter</param>
        public void SetStaticParameter(SharedParameter ep)
        {
            int index;
            if ((index = staticParameters.IndexOfKey(ep.Name)) != -1)
                staticParameters.RemoveAt(index);

            staticParameters.Add(ep.Name, ep);
        }

        /// <summary>
        /// Sets a static default parameter (eg: the World/View/Projection matrix, 
        /// the view vector, etc. A static parameter is updated only once.
        /// </summary>
        /// <param name="fxParam">The parameter type</param>
        public void SetStaticParameter(FXParameterType fxParam)
        {
            SharedParameter ep = SharedParameter.CreateDefault(fxParam, effect);
            SetStaticParameter(ep);
        }

        /// <summary>
        /// Adds a custom dynamic parameter ep. A dynamic parameter is updated every frame.
        /// </summary>
        /// <param name="ep">A custom parameter</param>
        public void SetDynamicParameter(SharedParameter ep)
        {
            int index;
            if ((index = dynamicParameters.IndexOfKey(ep.Name)) != -1)
                dynamicParameters.RemoveAt(index);

            dynamicParameters.Add(ep.Name, ep);
        }

        /// <summary>
        /// Adds a dynamic default parameter (eg: the World/View/Projection matrix, 
        /// the view vector, etc. A dynamic parameter is updated each frame.
        /// </summary>
        /// <param name="fxParam">The parameter type</param>
        public void SetDynamicParameter(FXParameterType fxParam)
        {
            SharedParameter ep = SharedParameter.CreateDefault(fxParam, effect);
            SetDynamicParameter(ep);
        }

        /// <summary>
        /// Adds the description of an instance parameter. 
        /// </summary>
        /// <param name="effectParameter">The description of the parameter.</param>
        public void SetInstanceParameter(InstanceParameter effectParameter)
        {
            int index;
            if ((index = instanceParameters.IndexOfKey(effectParameter.Name)) != -1)
            {
                instanceParameters.Values[index].Dispose();
                instanceParameters.RemoveAt(index);
            }

            instanceParameters.Add(effectParameter.Name, effectParameter);
        }

        public void ApplyStaticParameters()
        {
            foreach (SharedParameter p in staticParameters.Values)
                p.Apply();
        }

        public void ApplyDynamicParameters()
        {
            foreach (SharedParameter p in dynamicParameters.Values)
                p.Apply();
        }

        public void ApplyInstanceParameters(IRenderable rObject)
        {
            foreach (InstanceParameter p in instanceParameters.Values)
                p.Apply(rObject);
        }
    }
}