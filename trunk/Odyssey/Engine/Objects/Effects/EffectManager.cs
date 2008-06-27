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
using System;

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
                    effect = Effect.FromFile(Game.Device, filename, ShaderFlags.None);
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

                case FXType.AtmosphericScattering:
                    fxDescriptor = new EffectDescriptor("Atmosphere.fx");
                    fxDescriptor.AddDynamicParameter(FXParameterType.WorldViewProjection);
                    fxDescriptor.AddDynamicParameter(FXParameterType.EyePosition);
                    fxDescriptor.AddStaticParameter(FXParameterType.LightDirection);

                    float innerRadius = 1.0f;
                    float outerRadius = innerRadius * 1.25f;
                    float ESun = 15.0f;
                    float kr = 0.0025f;
                    float km = 0.0015f;
                    float scale = 1.0f / (1.25f-1.0f);
                    float scaleDepth = 0.05f;

                    Vector4 wavelenght = new Vector4(0.650f, 0.570f, 0.450f, 1.0f);
                    Vector4 invWavelenght = new Vector4(
                        (float)(1.0/Math.Pow(wavelenght.X,4.0)),
                        (float)(1.0 / Math.Pow(wavelenght.Y,4.0)),
                        (float)(1.0 / Math.Pow(wavelenght.Z,4.0)),
                        1);

          
                    FloatOp cameraHeight = delegate() {
                           return Game.CurrentScene.Camera.PositionV3.Length();};
                    EffectParameter epCameraHeight = EffectParameter.CreateCustomParameter("cameraHeight", fxDescriptor.Effect, cameraHeight);
                    fxDescriptor.AddDynamicParameter(epCameraHeight);

                    FloatOp cameraHeight2 = delegate()
                    {
                           return Game.CurrentScene.Camera.PositionV3.LengthSquared();};
                    EffectParameter epCameraHeight2 = EffectParameter.CreateCustomParameter("cameraHeight2", fxDescriptor.Effect,cameraHeight2);
                    fxDescriptor.AddDynamicParameter(epCameraHeight2);

                    EffectParameter epInnerRadius = EffectParameter.CreateCustomParameter("innerRadius", fxDescriptor.Effect, innerRadius);
                    fxDescriptor.AddStaticParameter(epInnerRadius);

                    EffectParameter epOuterRadius = EffectParameter.CreateCustomParameter("outerRadius", fxDescriptor.Effect, outerRadius);
                    fxDescriptor.AddStaticParameter(epOuterRadius);

                    EffectParameter epOuterRadius2 = EffectParameter.CreateCustomParameter("outerRadius2", fxDescriptor.Effect, innerRadius * innerRadius);
                    fxDescriptor.AddStaticParameter(epOuterRadius2);

                    EffectParameter epkrESun = EffectParameter.CreateCustomParameter("krESun", fxDescriptor.Effect, kr*ESun);
                    fxDescriptor.AddStaticParameter(epkrESun);
                    
                    EffectParameter epkmESun = EffectParameter.CreateCustomParameter("kmESun", fxDescriptor.Effect, km * ESun);
                    fxDescriptor.AddStaticParameter(epkmESun);

                    EffectParameter epkr4Pi = EffectParameter.CreateCustomParameter("kr4PI", fxDescriptor.Effect, (float)(kr * 4 * Math.PI));
                    fxDescriptor.AddStaticParameter(epkr4Pi);

                    EffectParameter epkm4Pi = EffectParameter.CreateCustomParameter("km4PI", fxDescriptor.Effect, (float)(km * 4 * Math.PI));
                    fxDescriptor.AddStaticParameter(epkm4Pi);

                    EffectParameter epScale = EffectParameter.CreateCustomParameter("scale", fxDescriptor.Effect, scale);
                    fxDescriptor.AddStaticParameter(epScale);

                    EffectParameter epScaleDepth = EffectParameter.CreateCustomParameter("scaleDepth", fxDescriptor.Effect, scaleDepth);
                    fxDescriptor.AddStaticParameter(epScaleDepth);

                    EffectParameter epScaleOverScaleDepth = EffectParameter.CreateCustomParameter("scaleOverScaleDepth", fxDescriptor.Effect, scale/0.05f);
                    fxDescriptor.AddStaticParameter(epScaleOverScaleDepth);

                    EffectParameter epG = EffectParameter.CreateCustomParameter("g", fxDescriptor.Effect, 0.95f);
                    fxDescriptor.AddStaticParameter(epG);

                    EffectParameter epG2 = EffectParameter.CreateCustomParameter("g2", fxDescriptor.Effect, 0.95f*0.95f);
                    fxDescriptor.AddStaticParameter(epG);

                    EffectParameter epInvWavelength = EffectParameter.CreateCustomParameter("invWavelength", fxDescriptor.Effect, invWavelenght);
                    fxDescriptor.AddStaticParameter(epInvWavelength);
                    
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