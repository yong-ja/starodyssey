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
        readonly RenderableNode rNode;
        readonly TransformNode tNode;
        public RenderableCollection TextItems { get; internal set; }

        public UserInterfaceRenderCommand(Renderer renderer, Hud hud, RenderableNode rNode)
            : base(renderer, new UIMaterial(),
                new RenderableCollection(UIMaterial.ItemsDescription, new [] {rNode}))
        {
            CommandType = CommandType.UserInterfaceRenderCommand;
            CommandAttributes |= Graphics.CommandAttributes.MonoRendering;
            this.hud = hud;
            textMaterial = new TextMaterial();
            this.rNode = rNode;
            this.rNode.RenderableObject.Material = Material;
            tNode = (TransformNode)rNode.Parent;
            UpdateSprites(hud.SpriteControls);
            textTechnique = textMaterial.EffectDescription.Technique;
            textPass = textTechnique.GetPassByIndex(textMaterial.EffectDescription.Pass);
            textLayout = new InputLayout(Game.Context.Device, textPass.Description.Signature,
                TextItems.Description.InputElements);

            
        }

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
            RenderableNode rNodeInterface = Items[0];

            foreach (RenderStep renderStep in hud.RenderSteps)
            {
                Game.Context.Immediate.InputAssembler.InputLayout = InputLayout;
                Material.ApplyDynamicParameters(Renderer);
                Material.ApplyInstanceParameters(rNodeInterface.RenderableObject);
                Pass.Apply(Game.Context.Immediate);

                rNodeInterface.RenderableObject.Render(
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
            TextItems = new RenderableCollection(TextMaterial.ItemsDescription);
            tNode.RemoveAll();
            tNode.AppendChild(rNode);
            foreach (ISpriteObject spriteControl in spriteControls)
            {
                RenderableNode textNode = new RenderableNode(spriteControl.RenderableObject);
                tNode.AppendChild(textNode);
                TextItems.Add(textNode);
                textNode.Update();
            }
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