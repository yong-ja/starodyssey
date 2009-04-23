#region Disclaimer

/* 
 * AbstractMaterial
 *
 * Author: Adalberto L. Simeone (Taranto, Italy)
 * E-Mail: avengerdragon@gmail.com
 * Website: http://www.avengersutd.com
 *
 * Part of the Odyssey Engine
 *
 */

#endregion

#region Using Directives

using System;
using System.Collections.Generic;
using AvengersUtd.Odyssey.Engine;
using AvengersUtd.Odyssey.Graphics.Effects;
using AvengersUtd.Odyssey.Graphics.Meshes;
using AvengersUtd.Odyssey.Objects.Entities;
using AvengersUtd.Odyssey.Resources;
using SlimDX;
using SlimDX.Direct3D9;
using AvengersUtd.Odyssey.Settings;

#endregion

namespace AvengersUtd.Odyssey.Graphics.Materials
{
    /// <summary>
    /// This abstract class represents an individual "Material" in the 3D engine.
    /// It holds the basic functionalities that are further specialized by 
    /// derived classes. Each material will actually be rendered by a HLSL shader. 
    /// </summary>
    public abstract class AbstractMaterial : IMaterial
    {
        bool disposed;
        SortedList<string, EffectParameter> individualParameters;
        IRenderable owningEntity;

        protected FXType fxType;
        protected EffectDescriptor effectDescriptor;

        protected LightingAlgorithm lightingAlgorithm;
        protected LightingTechnique lightingTechnique;

        Color4 ambientColor = new Color4(0, 0, 0, 0);
        Color4 diffuseColor = new Color4(0, 0, 0, 1);
        Color4 specularColor;
        float kA;
        float kD;
        float kS;

        protected AbstractMaterial()
        {
            individualParameters = new SortedList<string, EffectParameter>();
        }

        protected AbstractMaterial(LightingAlgorithm algorithm, string filename)
        {
            lightingAlgorithm = algorithm;
            lightingTechnique = LightingTechnique.Diffuse|LightingTechnique.Specular;
            individualParameters = new SortedList<string, EffectParameter>();
            effectDescriptor = new EffectDescriptor(filename);
        }

        #region Properties

        public Color4 DiffuseColor
        {
            get { return diffuseColor; }
            set
            {
                if (diffuseColor != value)
                {
                    diffuseColor = value;
                    SetIndividualParameter(CreateEffectParameter(MaterialParameter.Diffuse, effectDescriptor.Effect));
                }
            }
        }

         Color4 AmbientColor
        {
            get { return ambientColor; }
            set { ambientColor = value; }
        }

        public Color4 SpecularColor
        {
            get { return specularColor; }
            set
            {
                if (specularColor != value)
                {
                    specularColor = value;
                    SetIndividualParameter(CreateEffectParameter(MaterialParameter.Specular, effectDescriptor.Effect));
                }
            }
        }

        /// <summary>
        /// Returns a reference to the entity that uses this Material.
        /// </summary>
        public IRenderable OwningEntity
        {
            get { return owningEntity; }
            set { owningEntity = value; }
        }

        /// <summary>
        /// Returns the type of effect that this Material represents.
        /// </summary>
        public FXType FXType
        {
            get { return fxType; }
        }


        /// <summary>
        /// Returns a reference to the <see cref="EffectDescriptor"/> object this
        /// class uses to setup its shader.
        /// </summary>
        public virtual EffectDescriptor EffectDescriptor
        {
            get { return effectDescriptor; }
        }

        public LightingTechnique LightingTechnique
        {
            get { return lightingTechnique; }
        }

        #endregion

        /// <summary>
        /// This method is called when the entity is being initialized. The caller
        /// passes a reference to the IRenderable that owns this material so that, should it be
        /// needed by the shader, it will be able to obtain information about the entity.
        /// </summary>
        /// <param name="entity">The entity that uses this material.</param>
        public virtual void CreateEffect(IRenderable entity)
        {
            OwningEntity = entity;
            InitParameters();
        }

        #region Parameters initializataion methods
        /// <summary>
        /// Initializes static shader parameters individual of this particular instance of the 
        /// shader.
        /// </summary>
        protected virtual void OnStaticParametersInit()
        {
        }

        protected virtual void OnDynamicParametersInit()
        {
        }

        /// <summary>
        /// Initializes the shader parameters individual to this particular instance of the 
        /// shader.
        /// </summary>
        protected virtual void OnIndividualParametersInit()
        {
            if ((lightingTechnique & LightingTechnique.Diffuse) != LightingTechnique.Diffuse)
            {
                AmbientColor = new Color4(0, 0, 0, 0);
                DiffuseColor = new Color4(1, 0, 1, 0);
            }
            if ((lightingTechnique & LightingTechnique.Specular) != LightingTechnique.None)
                SpecularColor = new Color4(1, 1, 1, 1);
        }

        /// <summary>
        /// Initializes this shader's parameters.
        /// </summary>
        public void InitParameters()
        {
            OnStaticParametersInit();
            HandleLights();
            OnDynamicParametersInit();
            OnIndividualParametersInit();

            ChooseTechnique();
            ApplyStaticParameters();
           
        }

        public virtual void ApplyStaticParameters()
        {
            effectDescriptor.ApplyStaticParameters();
        }

        /// <summary>
        /// Sets the shader values. Has to be called before rendering.
        /// </summary>
        public virtual void ApplyDynamicParameters()
        {
            effectDescriptor.ApplyDynamicParameters();
        }

        public void ApplyIndividualParameters()
        {
            foreach (EffectParameter p in individualParameters.Values)
                p.Apply();
        }

        public void CommitChanges()
        {
            effectDescriptor.Effect.CommitChanges();
        }

        public void Apply()
        {
            ApplyDynamicParameters();
            ApplyIndividualParameters();
            CommitChanges();
        }

        public void SetLightingTechnique(LightingTechnique technique, bool value)
        {
            bool previousValue = (LightingTechnique & technique) == technique;
            if (value)
                lightingTechnique |= technique;
            else
                lightingTechnique ^= technique;

            if (value != previousValue)
                ChooseTechnique();
        }

        /// <summary>
        /// Adds the description of an individual parameter. 
        /// </summary>
        /// <param name="effectParameter">The description of the parameter.</param>
        protected void SetIndividualParameter(EffectParameter effectParameter)
        {
            int index;
            if ((index = individualParameters.IndexOfKey(effectParameter.Name)) != -1)
            {
                individualParameters.Values[index].Dispose();
                individualParameters.RemoveAt(index);
            }

            individualParameters.Add(effectParameter.Name, effectParameter);
        }
        #endregion

        protected virtual void ChooseTechnique()
        {
            string effects = string.Empty;

            if ((lightingTechnique & LightingTechnique.Diffuse) != LightingTechnique.None)
                effects += LightingTechnique.Diffuse;
            else if ((lightingTechnique & LightingTechnique.DiffuseMap) != LightingTechnique.None)
                effects += LightingTechnique.DiffuseMap;

            if ((lightingTechnique & LightingTechnique.Specular) != LightingTechnique.None)
                effects += LightingTechnique.Specular;

            if ((lightingTechnique & LightingTechnique.Shadows) != LightingTechnique.None)
                effects += EngineSettings.Video.ShadowAlgorithmTag;

            string technique = string.Format("{0}{1}", lightingAlgorithm, effects);
            effectDescriptor.Technique = technique;
        }

        protected void HandleLights()
        {
            if (lightingAlgorithm == LightingAlgorithm.Wireframe)
                return;

            LightManager lightManager = Game.CurrentScene.LightManager;
            Spotlight[] spotlights = lightManager.GetLights<Spotlight>();
            PointLight[] pointLights = lightManager.GetLights<PointLight>();

            if (spotlights.Length == 1)
                    HandleSpotlight(spotlights[0]);
            else
                throw new NotSupportedException(Properties.Resources.ERR_FeatureNotSupported);
        }

        protected virtual void HandleSpotlight(Spotlight spotlight)
        {
            Effect effect = effectDescriptor.Effect;

            EffectParameter epRadius = spotlight.CreateEffectParameter(LightParameter.Radius, effect);
            EffectParameter epInnerConeAngleCosine = spotlight.CreateEffectParameter(LightParameter.SpotlightInnerConeCosine, effect);
            EffectParameter epOuterConeAngleCosine = spotlight.CreateEffectParameter(LightParameter.SpotlightOuterConeCosine, effect);
            EffectParameter epFalloff = spotlight.CreateEffectParameter(LightParameter.SpotlightFalloff, effect);
            EffectParameter epSpotlightDirection = spotlight.CreateEffectParameter(LightParameter.SpotlightDirection, effect);
            EffectParameter epLightPosition = spotlight.CreateEffectParameter(LightParameter.Position, effect);
            effectDescriptor.SetStaticParameter(epRadius);
            effectDescriptor.SetStaticParameter(epInnerConeAngleCosine);
            effectDescriptor.SetStaticParameter(epOuterConeAngleCosine);
            effectDescriptor.SetStaticParameter(epFalloff);
            effectDescriptor.SetStaticParameter(epSpotlightDirection);
            effectDescriptor.SetStaticParameter(epLightPosition);

            if (spotlight.CastsShadows && (lightingTechnique & LightingTechnique.Shadows) != LightingTechnique.None)
            {
                EffectParameter epLightWorldViewProjection =
                    spotlight.CreateEffectParameter(LightParameter.LightWorldViewProjection, effect);
                effectDescriptor.SetDynamicParameter(epLightWorldViewProjection);
            }
        }

      public virtual EffectParameter CreateEffectParameter(MaterialParameter parameter, Effect effect)
      {
          string varName;
          EffectHandle eh;
          Update update;

          switch (parameter)
          {
              case MaterialParameter.Diffuse:
                  varName = ParamHandles.Colors.MaterialDiffuse;
                  eh = new EffectHandle(varName);
                  update = (fxParam => fxParam.OwnerEffect.SetValue(eh, diffuseColor));
                  break;
              default:
                  throw new ArgumentOutOfRangeException(parameter.ToString(), Properties.Resources.ERR_MaterialParameter);


          }
          return new EffectParameter(varName, effect, update);
      }
    }
}