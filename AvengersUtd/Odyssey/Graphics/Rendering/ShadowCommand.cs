using System;
using System.Collections.Generic;
using System.Text;
using AvengersUtd.Odyssey;
using AvengersUtd.Odyssey.Utils.Collections;
using AvengersUtd.Odyssey.Resources;

namespace AvengersUtd.Odyssey.Graphics.Rendering
{
    //public class ShadowMappingCommand : BaseCommand
    //{
    //    const string commandTag = "SSMtexture";
    //    Texture shadowMap;
    //    DepthMaterial depthMappingMaterial;

    //    public Texture ShadowMap
    //    {
    //        get { return shadowMap; }
    //    }

    //    public ShadowMappingCommand(SceneNodeCollection sceneNodeCollection) :
    //        base(CommandType.ComputeShadows, sceneNodeCollection)
    //    {
    //        depthMappingMaterial = new DepthMaterial();
    //        depthMappingMaterial.CreateEffect(((RenderableNode)Items[0]).renderableObjectObject);

    //        shadowMap = new Texture(Game.Device, 256, 256, 0, Usage.RenderTarget | Usage.AutoGenerateMipMap,
    //                                     Format.X8R8G8B8, Pool.Default);
    //        ResourceManager.Add(commandTag, shadowMap);
    //        PurgeNonShadowCasters();
    //    }

    //    void PurgeNonShadowCasters()
    //    {
    //        SceneNodeCollection newCollection = new SceneNodeCollection();
    //        foreach (RenderableNode rNode in Items)
    //        {
    //            if (rNode.renderableObjectObject.CastsShadows)
    //                newCollection.Add(rNode);
    //        }

    //        Items = newCollection;

    //    }

    //    public override void Execute()
    //    {
    //        //Provvisorio
    //        ScreenHelper.RenderToTexture(256, 256, depthMappingMaterial.Format, shadowMap, new SlimDX.Color4(1f, 1f, 1f, 1f), this);
    //        //BaseTexture.ToFile(shadowMap, "prova.jpg", ImageFileFormat.Jpg);
    //        foreach (RenderableNode rNode in Items)
    //        {
    //            foreach (AbstractMaterial material in rNode.renderableObjectObject.Materials)
    //            {
    //                ICastsShadows shadowMaterial = material as ICastsShadows;
    //                if (shadowMaterial != null)
    //                    shadowMaterial.ShadowMap = shadowMap;
    //            }
    //        }

    //    }

    //    public override void PerformRender()
    //    {
    //        Items.RenderWithMaterial(depthMappingMaterial);
    //    }

    //    protected override void OnDispose()
    //    {
    //        shadowMap.Dispose();
    //    }
    //}
}