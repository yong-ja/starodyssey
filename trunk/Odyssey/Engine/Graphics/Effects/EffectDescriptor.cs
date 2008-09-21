using System.Collections.Generic;
using SlimDX.Direct3D9;
using AvengersUtd.Odyssey.Resources;

namespace AvengersUtd.Odyssey.Graphics.Effects
{
    public class EffectDescriptor
    {
        string filename;
        string technique = string.Empty;
        int pass = 0;
        SortedList<string, EffectParameter> staticParameters;
        SortedList<string, EffectParameter> dynamicParameters;
        Effect effect;

        #region

        public string Technique
        {
            get { return technique; }
            set
            {
                if (technique != value)
                {
                    technique = value;
                    effect.Technique = technique;
                }
            }
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
            staticParameters = new SortedList<string, EffectParameter>();
            dynamicParameters = new SortedList<string, EffectParameter>();
            effect = EffectManager.LoadEffect(Global.FXPath + filename);
        }

        /// <summary>
        /// Sets a custom static parameter ep. A static parameter is updated only once.
        /// </summary>
        /// <param name="ep">A custom parameter</param>
        public void SetStaticParameter(EffectParameter ep)
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
            EffectParameter ep = EffectParameter.DefaultParameter(fxParam, effect);
            SetStaticParameter(ep);
        }

        /// <summary>
        /// Adds a custom dynamic parameter ep. A dynamic parameter is updated every frame.
        /// </summary>
        /// <param name="ep">A custom parameter</param>
        public void SetDynamicParameter(EffectParameter ep)
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
            EffectParameter ep = EffectParameter.DefaultParameter(fxParam, effect);
            SetDynamicParameter(ep);
        }

        public void ApplyStaticParameters()
        {
            foreach (EffectParameter p in staticParameters.Values)
                p.Apply();
        }

        public void ApplyDynamicParameters()
        {
            foreach (EffectParameter p in dynamicParameters.Values)
                p.Apply();
        }
    }
}