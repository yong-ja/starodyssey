#region #Disclaimer

// /* 
//  * Timer
//  *
//  * Created on 21 August 2007
//  * Last update on 29 July 2010
//  * 
//  * Author: Adalberto L. Simeone (Taranto, Italy)
//  * E-Mail: avengerdragon@gmail.com
//  * Website: http://www.avengersutd.com
//  *
//  * Part of the Odyssey Engine.
//  *
//  * This source code is Intellectual property of the Author
//  * and is released under the Creative Commons Attribution 
//  * NonCommercial License, available at:
//  * http://creativecommons.org/licenses/by-nc/3.0/ 
//  * You can alter and use this source code as you wish, 
//  * provided that you do not use the results in commercial
//  * projects, without the express and written consent of
//  * the Author.
//  *
//  */

#endregion

#region Using directives

using System.Collections.Generic;
using System.Linq;
using AvengersUtd.Odyssey.Geometry;
using AvengersUtd.Odyssey.Graphics.Materials;
using AvengersUtd.Odyssey.Graphics.Meshes;
using AvengersUtd.Odyssey.Graphics.Rendering.Management;
using AvengersUtd.Odyssey.UserInterface.Controls;
using SlimDX.Direct3D11;

#endregion

namespace AvengersUtd.Odyssey.Graphics.Rendering
{
    public class UserInterfaceRenderCommand : RenderCommand
    {
        private readonly Hud hud;
        private readonly InputLayout textLayout;
        private readonly IMaterial textMaterial;
        private readonly EffectPass textPass;
        private readonly EffectTechnique textTechnique;

        public UserInterfaceRenderCommand(Renderer renderer, Hud hud) : base(renderer, new UIMaterial())
        {
            this.hud = hud;
            textMaterial =new TextMaterial();
            UpdateSprites(hud.SpriteControls);
            textTechnique = textMaterial.EffectDescription.Technique;
            textPass = textTechnique.GetPassByIndex(textMaterial.EffectDescription.Pass);
            textLayout = new InputLayout(Game.Context.Device, textPass.Description.Signature,
                TextItems.Description.InputElements);
        }
       
        public UserInterfaceRenderCommand(Renderer renderer, IMaterial uiMaterial,
            RenderableCollection uiRCollection, 
            IMaterial textMaterial, Hud hud) : base(renderer, uiMaterial, uiRCollection)
        {
            this.hud = hud;

            UpdateSprites(hud.SpriteControls);

            this.textMaterial = textMaterial;
            textTechnique = textMaterial.EffectDescription.Technique;
            textPass = textTechnique.GetPassByIndex(textMaterial.EffectDescription.Pass);
            textLayout = new InputLayout(Game.Context.Device, textPass.Description.Signature,
                TextItems.Description.InputElements);
        }

        public RenderableCollection TextItems { get; internal set; }

        public override void Init()
        {
            base.Init();
            textMaterial.InitParameters(Renderer);
        }

        public override void Execute()
        {
            Render();
        }

        public void Render()
        {
            RenderableNode rNode = Items[0];

            foreach (RenderStep renderStep in hud.RenderSteps)
            {
                Game.Context.Immediate.InputAssembler.InputLayout = InputLayout;
                Material.ApplyDynamicParameters(Renderer);
                Material.ApplyInstanceParameters(rNode.RenderableObject);
                Pass.Apply(Game.Context.Immediate);

                rNode.RenderableObject.Render(
                    renderStep.PrimitiveCount * 3,
                    startIndex: renderStep.BaseIndex,
                    baseVertex: renderStep.BaseVertex);

                Game.Context.Immediate.InputAssembler.InputLayout = textLayout;
                textMaterial.ApplyDynamicParameters(Renderer);
                for (int i = renderStep.BaseLabelIndex; i < renderStep.LabelCount; i++)
                {
                    IRenderable rObject = TextItems[i].RenderableObject;
                    textMaterial.ApplyInstanceParameters(rObject);
                    textPass.Apply(Game.Context.Immediate);
                    rObject.Render();
                }
            }
        }

        public override void UpdateItems()
        {
            UpdateSprites(hud.SpriteControls);
        }

        void UpdateSprites(IEnumerable<ISpriteObject> spriteControls)
        {
            //textMaterialNode.RemoveAll();
            TextItems = new RenderableCollection(textMaterial.RenderableCollectionDescription);
            foreach (ISpriteObject spriteControl in spriteControls)
                //uiNode.AppendChild(new RenderableNode(spriteControl.RenderableObject));
                TextItems.Add(new RenderableNode(spriteControl.RenderableObject));
            //TextItems = textMaterialNode.RenderableCollection;
            //TextItems = uiNode.GetTextNodes();
        }

        protected override void OnDispose()
        {
            if (!textLayout.Disposed)
                textLayout.Dispose();

            foreach (RenderableNode rNode in TextItems.Where(rNode => !rNode.RenderableObject.Disposed))
            {
                rNode.RenderableObject.Dispose();
            }

            base.OnDispose();
        }
    }
}