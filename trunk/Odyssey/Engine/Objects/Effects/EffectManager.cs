using System.Collections.Generic;
using System.Windows.Forms;
using AvengersUtd.Odyssey.Engine;
using AvengersUtd.Odyssey.Objects.Materials;
using AvengersUtd.Odyssey.Objects;
using SlimDX.Direct3D9;
using AvengersUtd.Odyssey.Objects.Effects;
using AvengersUtd.Odyssey.Meshes;
using SlimDX;
using System.IO;
using AvengersUtd.Odyssey.Objects.Meshes;

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

        public static EffectDescriptor CreateEffect(FXType fxType, params object[] data)
        {
            EffectDescriptor fxDescriptor;

            switch (fxType)
            {
                default:
                case FXType.Diffuse:
                    fxDescriptor = new EffectDescriptor("Diffuse.fx");
                    fxDescriptor.AddDynamicParameter(FXParameterType.World);
                    fxDescriptor.AddDynamicParameter(FXParameterType.View);
                    fxDescriptor.AddDynamicParameter(FXParameterType.Projection);
                    fxDescriptor.AddStaticParameter(FXParameterType.LightDirection);
                    fxDescriptor.AddStaticParameter(FXParameterType.AmbientColor);
                    return fxDescriptor;

                case FXType.Specular:
                    fxDescriptor = new EffectDescriptor("Specular.fx");
                    fxDescriptor.AddDynamicParameter(FXParameterType.World);
                    fxDescriptor.AddDynamicParameter(FXParameterType.WorldViewProjection);
                    fxDescriptor.AddDynamicParameter(FXParameterType.LightDirection);
                    fxDescriptor.AddDynamicParameter(FXParameterType.EyePosition);
                    fxDescriptor.AddStaticParameter(FXParameterType.LightDirection);
                    fxDescriptor.AddStaticParameter(FXParameterType.AmbientColor);
                    fxDescriptor.AddStaticParameter(EffectParameter.CreateCustomParameter(ParamHandles.Colors.Diffuse,
                        fxDescriptor.Effect, (Color4)data[0]));
                    return fxDescriptor;

                case FXType.SelfAlign:

                    fxDescriptor = new EffectDescriptor("SelfAlign.fx");
                    fxDescriptor.AddStaticParameter(EffectParameter.CreateCustomParameter(
                                                        ParamHandles.Floats.Scale, fxDescriptor.Effect,
                                                        (float)data[0]));

                    fxDescriptor.AddStaticParameter(EffectParameter.CreateCustomParameter(
                                                        ParamHandles.Textures.Texture1, fxDescriptor.Effect,
                                                        data[1] as Texture));
                    fxDescriptor.AddDynamicParameter(FXParameterType.World);
                    fxDescriptor.AddDynamicParameter(FXParameterType.View);
                    fxDescriptor.AddDynamicParameter(FXParameterType.Projection);
                    return fxDescriptor;

                case FXType.SpecularBump:
                    fxDescriptor = new EffectDescriptor("SpecularBump.fx");
                    Texture diffuse = data[0] as Texture;
                    Texture normal = data[1] as Texture;


                    fxDescriptor.AddStaticParameter(EffectParameter.CreateCustomParameter(
                                                        ParamHandles.Textures.Texture1, fxDescriptor.Effect,
                                                        diffuse));
                    fxDescriptor.AddStaticParameter(EffectParameter.CreateCustomParameter(
                                                        ParamHandles.Textures.Texture2, fxDescriptor.Effect,
                                                        normal));
                    fxDescriptor.AddStaticParameter(FXParameterType.AmbientColor);
                    //fxDescriptor.AddDynamicParameter(FXParameterType.WorldViewProjection);
                    fxDescriptor.AddDynamicParameter(FXParameterType.World);
                    fxDescriptor.AddDynamicParameter(FXParameterType.View);
                    fxDescriptor.AddDynamicParameter(FXParameterType.Projection);
                    fxDescriptor.AddDynamicParameter(FXParameterType.EyePosition);

                    IEntity entity = data[0] as IEntity;

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