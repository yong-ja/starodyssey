using System;
using System.IO;
using System.Windows.Forms;
using AvengersUtd.Odyssey.Collections;
using AvengersUtd.Odyssey.Graphics.Effects;
using SlimDX.D3DCompiler;
using SlimDX.Direct3D11;

namespace AvengersUtd.Odyssey.Graphics.Resources
{
    public static class EffectManager
    {
        readonly static Cache<string, CacheNode<Effect>> EffectCache = new Cache<string, CacheNode<Effect>>(10*1024*1024);

        public static Effect LoadEffect(string filename)
        {
            if (EffectCache.ContainsKey(filename))
            {
                return EffectCache[filename].Object;
            }
            else
            {
                string compilationErrors = string.Empty;
                try
                {
                    int fileSize = (int)new FileInfo(filename).Length;
                    //Macro[] macros = new Macro[]
                    //                     {
                    //                         new Macro
                    //                             {
                    //                                 Name = "VSProfile",
                    //                                 Definition = EngineSettings.Video.VertexShaderTag,//                 
                    //                             },
                    //                             new Macro
                    //                                 {
                    //                                     Name = "PSProfile",
                    //                                     Definition = EngineSettings.Video.PixelShaderTag
                    //                                 }

                    //                     };


                    using (ShaderBytecode byteCode = ShaderBytecode.CompileFromFile(filename, "fx_5_0", ShaderFlags.Debug,
                                                                   EffectFlags.None, null, new IncludeHandler(filename), out compilationErrors))
                    {
                        Effect effect = new Effect(Game.Context.Device, byteCode);
                        EffectCache.Add(filename, new CacheNode<Effect>(fileSize, effect));
                        return effect;
                    }
                    
                }
                    catch(SlimDX.CompilationException ex)
                    {
                        MessageBox.Show(ex.Message, "Compilation error in " + filename, MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                        
                    }
                //catch (InvalidDataException ex)
                catch (Exception ex)
                {
                    MessageBox.Show("This file is missing or invalid: " +
                                    filename);
                    
                }

                return null;
            }
        }

        internal static void OnDispose(object sender, EventArgs e)
        {
            if (EffectCache.IsEmpty)
                return;

            foreach (CacheNode<Effect> node in EffectCache)
                if (!node.Object.Disposed)
                    node.Object.Dispose();
        }

        public static void Dispose()
        {
            OnDispose(null, EventArgs.Empty);
        }

        //public static EffectDescription CreateEffect(FXType fxType, params object[] data)
        //{
        //    EffectDescription fxDescriptor;

        //    switch (fxType)
        //    {
        //        default:

        //        case FXType.Diffuse:
        //            fxDescriptor = new EffectDescription("Diffuse.fx");
        //            fxDescriptor.SetDynamicParameter(SceneVariable.WorldViewProjection);
        //            fxDescriptor.SetDynamicParameter(SceneVariable.World);
        //            fxDescriptor.SetDynamicParameter(SceneVariable.WorldInverse);
        //            fxDescriptor.SetDynamicParameter(SceneVariable.LightPosition);
        //            fxDescriptor.SetDynamicParameter(SceneVariable.AmbientColor4);
        //            return fxDescriptor;


        //        case FXType.AtmosphericScattering:
        //            fxDescriptor = new EffectDescription("AtmosphericScattering.fx");
        //            fxDescriptor.Pass = 1;
        //            return fxDescriptor;

        //        case FXType.SurfaceFromSpaceWithAtmosphere:
        //            fxDescriptor = new EffectDescription("AtmosphericScattering.fx");
        //            fxDescriptor.Pass = 0;


        //            float ESun = 15.0f;
        //            float kr = 0.0025f;
        //            float km = 0.0015f;


        //            float g = -0.95f;


        //            EffectParameter epG = EffectParameter.CreateCustom("g", fxDescriptor.Effect, g);
        //            fxDescriptor.SetStaticParameter(epG);

        //            EffectParameter epG2 = EffectParameter.CreateCustom("g2", fxDescriptor.Effect, g*g);
        //            fxDescriptor.SetStaticParameter(epG2);

        //            EffectParameter epkrESun = EffectParameter.CreateCustom("krESun", fxDescriptor.Effect,
        //                                                                             kr*ESun);
        //            fxDescriptor.SetStaticParameter(epkrESun);

        //            EffectParameter epkmESun = EffectParameter.CreateCustom("kmESun", fxDescriptor.Effect,
        //                                                                             km*ESun);
        //            fxDescriptor.SetStaticParameter(epkmESun);

        //            EffectParameter epkr4Pi = EffectParameter.CreateCustom("kr4PI", fxDescriptor.Effect,
        //                                                                            (float) (kr*4*Math.PI));
        //            fxDescriptor.SetStaticParameter(epkr4Pi);

        //            EffectParameter epkm4Pi = EffectParameter.CreateCustom("km4PI", fxDescriptor.Effect,
        //                                                                            (float) (km*4*Math.PI));
        //            fxDescriptor.SetStaticParameter(epkm4Pi);

        //            fxDescriptor.SetStaticParameter(SceneVariable.LightPosition);
        //            fxDescriptor.SetDynamicParameter(SceneVariable.EyePosition);
        //            fxDescriptor.SetDynamicParameter(SceneVariable.WorldViewProjection);
        //            fxDescriptor.SetDynamicParameter(SceneVariable.World);
        //            fxDescriptor.SetDynamicParameter(SceneVariable.WorldInverse);
        //            fxDescriptor.SetDynamicParameter(SceneVariable.View);

        //            return fxDescriptor;

        //        case FXType.SurfaceFromSpace:
        //            fxDescriptor = new EffectDescription("SurfaceFromSpace.fx");
        //            fxDescriptor.Technique = "Surface";
        //            fxDescriptor.SetStaticParameter(SceneVariable.LightPosition);
        //            //fxDescriptor.SetDynamicParameter(SceneVariable.EyePosition);
        //            fxDescriptor.SetDynamicParameter(SceneVariable.WorldViewProjection);
        //            fxDescriptor.SetDynamicParameter(SceneVariable.World);
        //            //fxDescriptor.SetDynamicParameter(SceneVariable.WorldInverse);
        //            fxDescriptor.SetDynamicParameter(SceneVariable.View);

        //            return fxDescriptor;

        //        case FXType.Textured:
        //            fxDescriptor = new EffectDescription("Textured.fx");
        //            fxDescriptor.SetDynamicParameter(SceneVariable.WorldViewProjection);

        //            //fxDescriptor.SetDynamicParameter(SceneVariable.EyePosition);
        //            return fxDescriptor;

        //        case FXType.SelfAlign:

        //            fxDescriptor = new EffectDescription("SelfAlign.fx");
        //            fxDescriptor.SetDynamicParameter(SceneVariable.World);
        //            fxDescriptor.SetDynamicParameter(SceneVariable.View);
        //            fxDescriptor.SetDynamicParameter(SceneVariable.Projection);
        //            return fxDescriptor;
        //    }
        //}
    }
}