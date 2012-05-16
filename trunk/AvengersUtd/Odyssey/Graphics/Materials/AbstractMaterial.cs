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
using AvengersUtd.Odyssey.Graphics.Rendering;
using AvengersUtd.Odyssey.Graphics.Rendering.Management;
using AvengersUtd.Odyssey.Properties;
using SlimDX;
using SlimDX.Direct3D11;
using EffectDescription = AvengersUtd.Odyssey.Graphics.Effects.EffectDescription;
using AvengersUtd.Odyssey.Utils.Logging;

#endregion

namespace AvengersUtd.Odyssey.Graphics.Materials
{
    /// <summary>
    /// This abstract class represents an individual "Material" in the 3D engine.
    /// It holds the basic functionalities that are further specialized by 
    /// derived classes. Each material will actually be rendered by a HLSL shader. 
    /// </summary>
    public abstract class AbstractMaterial : IMaterial, IDisposable
    {
        private bool disposed;
        RenderableCollectionDescription itemsDescription;

        protected AbstractMaterial(string filename, RenderableCollectionDescription description)
        {
            EffectDescription = new EffectDescription(filename);
            PreRenderStateList = new List<ICommand>();
            PostRenderStateList = new List<ICommand>();
            itemsDescription = description;
        }

        #region Properties

        /// <summary>
        /// Returns the type of effect that this Material represents.
        /// </summary>
        public FXType FXType { get; private set; }

        /// <summary>
        /// Returns a reference to the <see cref="EffectDescription"/> object this
        /// instance uses to setup its shader.
        /// </summary>
        public virtual EffectDescription EffectDescription { get; private set; }
        
        protected List<ICommand> PreRenderStateList { get; private set; }
        protected List<ICommand> PostRenderStateList { get; private set; }

        public MaterialNode ParentNode { get; private set; }

        public bool RequirePreRenderStateChange
        {
            get { return PreRenderStateList.Count > 0; }
        }

        public bool RequirePostRenderStateChange
        {
            get { return PostRenderStateList.Count > 0; }
        }

        public ICommand[] PreRenderStates
        {
            get { return PreRenderStateList.ToArray(); }
        }

        public ICommand[] PostRenderStates
        {
            get { return PostRenderStateList.ToArray(); }
        }

        public string TechniqueName
        {
            get { return EffectDescription.Technique.Description.Name; }
        }

        RenderableCollectionDescription IMaterial.ItemsDescription
        {
            get { return itemsDescription; }
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
        public void InitParameters(Renderer rendererContext)
        {
            //HandleLights();
            OnStaticParametersInit();
            OnDynamicParametersInit();
            OnInstanceParametersInit();

            //ChooseTechnique();
            ApplyStaticParameters(rendererContext);
        }

        public virtual void ApplyStaticParameters(Renderer rendererContext)
        {
            EffectDescription.ApplyStaticParameters(rendererContext);
        }

        /// <summary>
        /// Sets the shader values. Has to be called before rendering.
        /// </summary>
        public virtual void ApplyDynamicParameters(Renderer rendererContext)
        {
            EffectDescription.ApplyDynamicParameters(rendererContext);
        }

        void IMaterial.SetParentNode(MaterialNode mNode)
        {
            ParentNode = mNode;
        }

        public void ApplyInstanceParameters(IRenderable rObject)
        {
            EffectDescription.ApplyInstanceParameters(rObject);
        }

        public void Apply(Renderer rendererContext)
        {
            ApplyDynamicParameters(rendererContext);
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

        //    Game.CurrentRenderer.LightManager.HandleMaterial(this);
        //}

        #region IDisposable Members

       public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose (bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    if (EffectDescription.Effect.Disposed)
                        EffectDescription.Effect.Dispose();
                }

            }
            disposed = true;
        }

        ~AbstractMaterial()
        {
            Dispose(false);
        }

        #endregion



        
    }
}