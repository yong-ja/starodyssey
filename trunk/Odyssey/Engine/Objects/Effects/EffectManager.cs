using System.Collections.Generic;
using System.Windows.Forms;
using AvengersUtd.Odyssey.Engine;
using AvengersUtd.Odyssey.Objects.Materials;
using AvengersUtd.Odyssey.Objects;
using SlimDX.Direct3D9;
using AvengersUtd.Odyssey.Objects.Effects;
using AvengersUtd.Odyssey.Meshes;
using SlimDX;

namespace AvengersUtd.Odyssey.Resources
{
    public static class EffectManager
    {
        static SortedDictionary<string, Effect> effectCache = new SortedDictionary<string, Effect>();

        public static Effect LoadEffect(string filename)
        {
            if (effectCache.ContainsKey(filename))
            {
                return effectCache[filename];
            }
            else
            {
                Effect effect;
                try
                {
                    effect = Effect.FromFile(Game.Device, filename, null, null, ShaderFlags.None, null);
                    return effect;
                }
                catch (InvalidDataException ex)
                {
                    MessageBox.Show("This file is missing or invalid: " +
                                    filename);
                    return null;
                }
            }
        }

        public static EffectDescriptor CreateEffect<MaterialT>(FXType fxType, BaseEntity<MaterialT> entity)
            where MaterialT : IMaterialContainer, new()
        {
            EffectDescriptor fxDescriptor;

            switch (fxType)
            {
                default:
                case FXType.Diffuse:
                    fxDescriptor = new EffectDescriptor("Diffuse.fx");
                    //fxDescriptor.AddStaticParameter(FXParameterType.WorldViewProjection);
                    fxDescriptor.AddDynamicParameter(FXParameterType.World);
                    fxDescriptor.AddDynamicParameter(FXParameterType.View);
                    fxDescriptor.AddDynamicParameter(FXParameterType.Projection);
                    fxDescriptor.AddStaticParameter(FXParameterType.LightDirection);
                    fxDescriptor.AddStaticParameter(FXParameterType.AmbientColor);
                    return fxDescriptor;

                case FXType.SelfAlign:

                    TexturedEffectMaterial teMat = entity.MeshObject.Materials[0] as TexturedEffectMaterial;
                    fxDescriptor = new EffectDescriptor("SelfAlign.fx");
                    fxDescriptor.AddStaticParameter(EffectParameter.CreateCustomParameter(
                                                        ParamHandles.Floats.Scale, fxDescriptor.Effect,
                                                        ((IScaleable) entity).Scale));

                    fxDescriptor.AddStaticParameter(EffectParameter.CreateCustomParameter(
                                                        ParamHandles.Textures.Texture1, fxDescriptor.Effect,
                                                        teMat.Diffuse));
                    fxDescriptor.AddDynamicParameter(FXParameterType.World);
                    fxDescriptor.AddDynamicParameter(FXParameterType.View);
                    fxDescriptor.AddDynamicParameter(FXParameterType.Projection);
                    return fxDescriptor;

                case FXType.SpecularBump:
                    fxDescriptor = new EffectDescriptor("SpecularBump.fx");
                    SpecularBumpMaterial sbMat = entity.MeshObject.Materials[0] as SpecularBumpMaterial;

                    fxDescriptor.AddStaticParameter(EffectParameter.CreateCustomParameter(
                                                        ParamHandles.Textures.Texture1, fxDescriptor.Effect,
                                                        sbMat.Diffuse));
                    fxDescriptor.AddStaticParameter(EffectParameter.CreateCustomParameter(
                                                        ParamHandles.Textures.Texture2, fxDescriptor.Effect,
                                                        sbMat.Normal));
                    fxDescriptor.AddStaticParameter(FXParameterType.AmbientColor);
                    //fxDescriptor.AddDynamicParameter(FXParameterType.WorldViewProjection);
                    fxDescriptor.AddDynamicParameter(FXParameterType.World);
                    fxDescriptor.AddDynamicParameter(FXParameterType.View);
                    fxDescriptor.AddDynamicParameter(FXParameterType.Projection);
                    fxDescriptor.AddDynamicParameter(FXParameterType.EyePosition);

                    VectorOp vectorOp = delegate()
                                            {
                                                Vector4 vLDir =
                                                    Game.CurrentScene.LightManager.GetParameter(0,
                                                                                                FXParameterType.
                                                                                                    LightDirection);
                                                Vector4 vPos =
                                                    new Vector4(entity.Position.X, entity.Position.Y, entity.Position.Z,
                                                                1);
                                                Vector4 vLightPos = vLDir - vPos;
                                                vLightPos.Normalize();
                                                return vLightPos;
                                            };
                    fxDescriptor.AddDynamicParameter(EffectParameter.CreateCustomParameter(
                                                         ParamHandles.Vectors.LightPosition, fxDescriptor.Effect,
                                                         vectorOp)
                        );

                    return fxDescriptor;
            }
        }
    }
}