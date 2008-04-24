using System.Collections.Generic;
using SlimDX.Direct3D9;
using AvengersUtd.Odyssey.Resources;

namespace AvengersUtd.Odyssey.Objects.Effects
{
    public class EffectDescriptor
    {
        string filename;
        List<EffectParameter> staticParameters;
        List<EffectParameter> dynamicParameters;
        Effect effect;

        public Effect Effect
        {
            get { return effect; }
        }

        public EffectDescriptor(string filename)
        {
            this.filename = filename;
            staticParameters = new List<EffectParameter>();
            dynamicParameters = new List<EffectParameter>();
            effect = EffectManager.LoadEffect(Global.FXPath + filename);
        }

        /// <summary>
        /// Adds a custom static parameter ep. A static parameter is updated only once.
        /// </summary>
        /// <param name="ep">A custom parameter</param>
        public void AddStaticParameter(EffectParameter ep)
        {
            staticParameters.Add(ep);
        }

        /// <summary>
        /// Adds a static default parameter (eg: the World/View/Projection matrix, 
        /// the view vector, etc. A static parameter is updated only once.
        /// </summary>
        /// <param name="fxParam">The parameter type</param>
        public void AddStaticParameter(FXParameterType fxParam)
        {
            staticParameters.Add(EffectParameter.DefaultParameter(fxParam, effect));
        }

        /// <summary>
        /// Adds a custom dynamic parameter ep. A static parameter is updated only once.
        /// </summary>
        /// <param name="ep">A custom parameter</param>
        public void AddDynamicParameter(EffectParameter ep)
        {
            dynamicParameters.Add(ep);
        }

        /// <summary>
        /// Adds a dynamic default parameter (eg: the World/View/Projection matrix, 
        /// the view vector, etc. A dynamic parameter is updated each frame.
        /// </summary>
        /// <param name="fxParam">The parameter type</param>
        public void AddDynamicParameter(FXParameterType fxParam)
        {
            dynamicParameters.Add(EffectParameter.DefaultParameter(fxParam, effect));
        }

        public void UpdateStatic()
        {
            foreach (EffectParameter p in staticParameters)
                p.Apply();
        }

        public void UpdateDynamic()
        {
            foreach (EffectParameter p in dynamicParameters)
                p.Apply();
        }
    }
}