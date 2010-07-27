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
using AvengersUtd.Odyssey.Graphics;
using AvengersUtd.Odyssey.Graphics.Effects;
using AvengersUtd.Odyssey.Graphics.Meshes;
using AvengersUtd.Odyssey.Graphics.Rendering.SceneGraph;
using SlimDX;
using SlimDX.Direct3D11;

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
        
        
        protected FXType fxType;
        static protected EffectDescriptor effectDescriptor;

        protected AbstractMaterial()
        {
        }

        protected AbstractMaterial(string filename)
        {
            //lightingAlgorithm = algorithm;
            //lightingTechnique = LightingTechnique.Diffuse|LightingTechnique.Specular;
            effectDescriptor = new EffectDescriptor(filename);
        }

        #region Properties

        

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

        public MaterialNode OwningNode
        { get; internal set; }


        public string TechniqueName
        {
            get { return effectDescriptor.Technique.Description.Name; }
        }
        #endregion


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
        protected virtual void OnInstanceParametersInit()
        {
        }

        /// <summary>
        /// Initializes this shader's parameters.
        /// </summary>
        public void InitParameters()
        {
            //HandleLights();
            OnStaticParametersInit();
            OnDynamicParametersInit();
            OnInstanceParametersInit();

            //ChooseTechnique();
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

        public void ApplyInstanceParameters(IRenderable rObject)
        {
            effectDescriptor.ApplyInstanceParameters(rObject);
        }

        public void Apply()
        {
            ApplyDynamicParameters();
        }
        
        #endregion

        //protected virtual void ChooseTechnique()
        //{
        //    string effects = string.Empty;

        //    if ((lightingTechnique & this.LightingTechnique.Diffuse) != this.LightingTechnique.None)
        //        effects += this.LightingTechnique.Diffuse;
        //    else if ((lightingTechnique & this.LightingTechnique.DiffuseMap) != this.LightingTechnique.None)
        //        effects += this.LightingTechnique.DiffuseMap;

        //    if ((lightingTechnique & this.LightingTechnique.Specular) != this.LightingTechnique.None)
        //        effects += this.LightingTechnique.Specular;

        //    if ((lightingTechnique & this.LightingTechnique.Shadows) != this.LightingTechnique.None)
        //        effects += EngineSettings.Video.ShadowAlgorithmTag;

        //    string technique = string.Format("{0}{1}", lightingAlgorithm, effects);
        //    effectDescriptor.Technique = technique;
        //}

        //protected void HandleLights()
        //{
        //    if (lightingAlgorithm == LightingAlgorithm.Wireframe || lightingAlgorithm == LightingAlgorithm.None)
        //        return;

        //    Game.CurrentScene.LightManager.HandleMaterial(this);
        //}

      
    }
}