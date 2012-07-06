using System.Collections.Generic;
using System.Windows.Forms;
using AvengersUtd.Odyssey.Graphics.Rendering;
using AvengersUtd.Odyssey.Graphics.Resources;
using AvengersUtd.Odyssey.Assets;
using SlimDX.Direct3D11;
using AvengersUtd.Odyssey.Graphics.Meshes;
using AvengersUtd.Odyssey.Utils.Logging;

namespace AvengersUtd.Odyssey.Graphics.Effects
{
    public class EffectDescription
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

        public EffectDescription(string filename)
        {
            this.filename = filename;
            instanceParameters = new SortedList<string, InstanceParameter>();
            staticParameters = new SortedList<string, SharedParameter>();
            dynamicParameters = new SortedList<string, SharedParameter>();
            
            effect = EffectManager.LoadEffect(Global.FXPath + filename);

            if (effect == null)
                Application.Exit();
            else 
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
        public void SetStaticParameter(SceneVariable fxParam)
        {
            SharedParameter ep = SharedParameter.Create(fxParam, effect);
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
        public void SetDynamicParameter(SceneVariable fxParam)
        {
            SharedParameter ep = SharedParameter.Create(fxParam, effect);
            SetDynamicParameter(ep);
        }


        public void SetInstanceParameter(InstanceVariable instanceVariable)
        {
            InstanceParameter ip = InstanceParameter.Create(instanceVariable, effect);
            SetInstanceParameter(ip);
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

        public void ApplyStaticParameters(Renderer rendererContext)
        {
            foreach (SharedParameter p in staticParameters.Values)
                p.Apply(rendererContext);
        }

        public void ApplyDynamicParameters(Renderer rendererContext)
        {
            foreach (SharedParameter p in dynamicParameters.Values)
            {
                VerboseEvent.DynamicParameterSetting.Log(rendererContext.GetType().Name.ToString(), p.Type.ToString());
                p.Apply(rendererContext);
            }
        }

        public void ApplyInstanceParameters(IRenderable rObject)
        {
            foreach (InstanceParameter p in instanceParameters.Values)
            {
                //VerboseEvent.InstanceParameterSetting.Log(rObject.Name, p.Type.ToString());
                p.Apply(rObject);
            }
        }
    }
}