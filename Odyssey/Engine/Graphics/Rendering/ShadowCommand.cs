﻿using System;
using System.Collections.Generic;
using System.Text;
using AvengersUtd.Odyssey.Graphics.Materials;
using AvengersUtd.Odyssey.Graphics.Rendering.SceneGraph;
using SlimDX.Direct3D9;
using AvengersUtd.Odyssey.Graphics.Effects;
using AvengersUtd.Odyssey.Settings;
using AvengersUtd.Odyssey.Resources;

namespace AvengersUtd.Odyssey.Graphics.Rendering
{
    public class ShadowMappingCommand : BaseCommand
    {
        const string commandTag = "SSMtexture";
        Texture shadowMap;
        DepthMaterial depthMappingMaterial;

        public Texture ShadowMap
        {
            get { return shadowMap; }
        }

        public ShadowMappingCommand(SceneNodeCollection nodeCollection) :
            base(CommandType.ComputeShadows, nodeCollection)
        {
            depthMappingMaterial = new DepthMaterial();
            depthMappingMaterial.CreateEffect(((RenderableNode)Items[0]).RenderableObject);

            shadowMap = new Texture(Game.Device, 256, 256, 0, Usage.RenderTarget | Usage.AutoGenerateMipMap,
                                         Format.X8R8G8B8, Pool.Default);
            TextureManager.Add(commandTag, shadowMap);
            PurgeNonShadowCasters();
        }

        void PurgeNonShadowCasters()
        {
            SceneNodeCollection newCollection = new SceneNodeCollection();
            foreach (RenderableNode rNode in Items )
            {
                if (rNode.RenderableObject.CastsShadows)
                    newCollection.Add(rNode);
            }

            Items = newCollection;
        }

        public override void Execute()
        {
            //Provvisorio
            ScreenHelper.RenderToTexture(256, 256, depthMappingMaterial.Format, shadowMap, new SlimDX.Color4(1f, 1f, 1f, 1f), this);
            //BaseTexture.ToFile(shadowMap, "prova.jpg", ImageFileFormat.Jpg);
            foreach (RenderableNode rNode in Items)
            {
                foreach (AbstractMaterial material in rNode.RenderableObject.Materials)
                {
                    ICastsShadows shadowMaterial = material as ICastsShadows;
                    if (shadowMaterial != null)
                        shadowMaterial.ShadowMap = shadowMap;
                }
            }

        }

        public override void PerformRender()
        {
            Items.RenderWithMaterial(depthMappingMaterial);
        }

        protected override void OnDispose()
        {
            shadowMap.Dispose();
        }
    }
}