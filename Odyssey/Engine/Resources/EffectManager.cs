using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using AvengersUtd.Odyssey.Collections;
using AvengersUtd.Odyssey.Graphics;
using AvengersUtd.Odyssey.Graphics.Effects;
using SlimDX.Direct3D9;
using EngineSettings=AvengersUtd.Odyssey.Settings.EngineSettings;

namespace AvengersUtd.Odyssey.Resources
{
    public static class EffectManager
    {
        readonly static Cache<string, CacheNode<Effect>> effectCache = new Cache<string, CacheNode<Effect>>(10*1024*1024);

        public static Effect LoadEffect(string filename)
        {
            if (effectCache.ContainsKey(filename))
            {
                return effectCache[filename].Object;
            }
            else
            {
                try
                {
                    int fileSize = (int)new FileInfo(filename).Length;
                    Macro[] macros = new Macro[]
                                         {
                                             new Macro
                                                 {
                                                     Name = "VSProfile",
                                                     Definition = EngineSettings.Video.VertexShaderTag,//                 
                                                 },
                                                 new Macro
                                                     {
                                                         Name = "PSProfile",
                                                         Definition = EngineSettings.Video.PixelShaderTag
                                                     }

                                         };

                    string compilationErrors;
                    Effect effect = Effect.FromFile(Game.Device, filename, macros, null, null, ShaderFlags.Debug, null,
                        out compilationErrors);

                    effectCache.Add(filename, new CacheNode<Effect>(fileSize, effect));
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

        public static void Dispose()
        {
            if (effectCache.IsEmpty)
                return;

            foreach (CacheNode<Effect> node in effectCache)
                if (!node.Object.Disposed)
                    node.Object.Dispose();
        }

        public static EffectDescriptor CreateEffect(FXType fxType, params object[] data)
        {
            EffectDescriptor fxDescriptor;

            switch (fxType)
            {
                default:

                case FXType.Diffuse:
                    fxDescriptor = new EffectDescriptor("Diffuse.fx");
                    fxDescriptor.SetDynamicParameter(FXParameterType.WorldViewProjection);
                    fxDescriptor.SetDynamicParameter(FXParameterType.World);
                    fxDescriptor.SetDynamicParameter(FXParameterType.WorldInverse);
                    fxDescriptor.SetDynamicParameter(FXParameterType.LightPosition);
                    fxDescriptor.SetDynamicParameter(FXParameterType.AmbientColor);
                    return fxDescriptor;


                case FXType.AtmosphericScattering:
                    fxDescriptor = new EffectDescriptor("AtmosphericScattering.fx");
                    fxDescriptor.Pass = 1;
                    return fxDescriptor;

                case FXType.SurfaceFromSpaceWithAtmosphere:
                    fxDescriptor = new EffectDescriptor("AtmosphericScattering.fx");
                    fxDescriptor.Pass = 0;


                    float ESun = 15.0f;
                    float kr = 0.0025f;
                    float km = 0.0015f;


                    float g = -0.95f;


                    EffectParameter epG = EffectParameter.CreateCustomParameter("g", fxDescriptor.Effect, g);
                    fxDescriptor.SetStaticParameter(epG);

                    EffectParameter epG2 = EffectParameter.CreateCustomParameter("g2", fxDescriptor.Effect, g*g);
                    fxDescriptor.SetStaticParameter(epG2);

                    EffectParameter epkrESun = EffectParameter.CreateCustomParameter("krESun", fxDescriptor.Effect,
                                                                                     kr*ESun);
                    fxDescriptor.SetStaticParameter(epkrESun);

                    EffectParameter epkmESun = EffectParameter.CreateCustomParameter("kmESun", fxDescriptor.Effect,
                                                                                     km*ESun);
                    fxDescriptor.SetStaticParameter(epkmESun);

                    EffectParameter epkr4Pi = EffectParameter.CreateCustomParameter("kr4PI", fxDescriptor.Effect,
                                                                                    (float) (kr*4*Math.PI));
                    fxDescriptor.SetStaticParameter(epkr4Pi);

                    EffectParameter epkm4Pi = EffectParameter.CreateCustomParameter("km4PI", fxDescriptor.Effect,
                                                                                    (float) (km*4*Math.PI));
                    fxDescriptor.SetStaticParameter(epkm4Pi);

                    fxDescriptor.SetStaticParameter(FXParameterType.LightPosition);
                    fxDescriptor.SetDynamicParameter(FXParameterType.EyePosition);
                    fxDescriptor.SetDynamicParameter(FXParameterType.WorldViewProjection);
                    fxDescriptor.SetDynamicParameter(FXParameterType.World);
                    fxDescriptor.SetDynamicParameter(FXParameterType.WorldInverse);
                    fxDescriptor.SetDynamicParameter(FXParameterType.View);

                    return fxDescriptor;

                case FXType.SurfaceFromSpace:
                    fxDescriptor = new EffectDescriptor("SurfaceFromSpace.fx");
                    fxDescriptor.Technique = "Surface";
                    fxDescriptor.SetStaticParameter(FXParameterType.LightPosition);
                    //fxDescriptor.SetDynamicParameter(FXParameterType.EyePosition);
                    fxDescriptor.SetDynamicParameter(FXParameterType.WorldViewProjection);
                    fxDescriptor.SetDynamicParameter(FXParameterType.World);
                    //fxDescriptor.SetDynamicParameter(FXParameterType.WorldInverse);
                    fxDescriptor.SetDynamicParameter(FXParameterType.View);

                    return fxDescriptor;

                case FXType.Textured:
                    fxDescriptor = new EffectDescriptor("Textured.fx");
                    fxDescriptor.SetDynamicParameter(FXParameterType.WorldViewProjection);

                    //fxDescriptor.SetDynamicParameter(FXParameterType.EyePosition);
                    return fxDescriptor;

                case FXType.SelfAlign:

                    fxDescriptor = new EffectDescriptor("SelfAlign.fx");
                    fxDescriptor.SetDynamicParameter(FXParameterType.World);
                    fxDescriptor.SetDynamicParameter(FXParameterType.View);
                    fxDescriptor.SetDynamicParameter(FXParameterType.Projection);
                    return fxDescriptor;
            }
        }
    }
}