using System.Collections.Generic;
using AvengersUtd.Odyssey.Engine;
using SlimDX.Direct3D9;

namespace AvengersUtd.Odyssey.Objects.Effects
{
    /// <summary>
    /// Descrizione di riepilogo per EffectFactory.
    /// </summary>
    public class EffectFactory
    {
        //private static EffectCollection fxCollection = new EffectCollection();
        static SortedDictionary<string, Effect> fxCollection = new SortedDictionary<string, Effect>();

        EffectFactory()
        {
        }

        public static Effect CreateEffect(ColorOp type)
        {
            string sType = type.ToString();
            Effect effect = null;
            if (fxCollection.ContainsKey(sType))
            {
                effect = fxCollection[sType];
            }
            else
            {
                switch (type)
                {
                    case ColorOp.Brightness:
                    case ColorOp.Colorize:
                    case ColorOp.Contrast:

                        effect = Effect.FromFile(
                            Game.Device,
                            Global.FXPath + "ColorAdjust.fx",
                            ShaderFlags.None);
                        fxCollection.Add(sType, effect);
                        break;
                }
            }
            return effect;
        }

        public static Effect CreateEffect(ScaleOp type)
        {
            string sType = type.ToString();
            Effect effect = null;
            if (fxCollection.ContainsKey(sType))
            {
                effect = fxCollection[sType];
            }
            else
            {
                switch (type)
                {
                    case ScaleOp.Downsample:
                        effect = Effect.FromFile(
                            Game.Device,
                            Global.FXPath + "Downsampling.fx",
                            ShaderFlags.None);
                        fxCollection.Add(sType, effect);
                        break;
                }
            }
            return effect;
        }

        public static Effect CreateEffect(BlurOp type)
        {
            string sType = type.ToString();
            string filename = string.Empty;
            Effect effect = null;
            if (fxCollection.ContainsKey(sType))
            {
                effect = fxCollection[sType];
            }
            else
            {
                switch (type)
                {
                    case BlurOp.GaussianH:
                        filename = "GaussianH.fx";
                        break;

                    case BlurOp.GaussianV:
                        filename = "GaussianV.fx";
                        break;

                    case BlurOp.BloomH:
                        filename = "BloomH.fx";
                        break;

                    case BlurOp.BloomV:
                        filename = "BloomV.fx";
                        break;
                }

                effect = Effect.FromFile(
                    Game.Device,
                    Global.FXPath + filename,                   
                    ShaderFlags.None);
                fxCollection.Add(sType, effect);
            }
            return effect;
        }

        public static Effect CreateEffect(FilterOp type)
        {
            string sType = type.ToString();
            Effect effect = null;
            if (fxCollection.ContainsKey(sType))
            {
                effect = fxCollection[sType];
            }
            else
            {
                switch (type)
                {
                    case FilterOp.SobelEdge:
                        effect = Effect.FromFile(
                            Game.Device,
                            Global.FXPath + "SobelEdge.fx",
                            
                            ShaderFlags.None);
                        fxCollection.Add(sType, effect);
                        break;
                }
            }
            return effect;
        }

        public static Effect CreateEffect(FXType type)
        {
            string sType = type.ToString();
            string filename = string.Empty;
            Effect effect = null;
            if (fxCollection.ContainsKey(sType))
            {
                effect = fxCollection[sType];
            }
            else
            {
                switch (type)
                {
                    case FXType.Diffuse:
                        filename = "Diffuse.fx";
                        break;

                    case FXType.SpecularBump:
                        filename = "SpecularBump.fx";
                        break;

                    case FXType.SelfAlign:
                        filename = "SelfAlign.fx";
                        break;
                }
                effect = Effect.FromFile(
                    Game.Device,
                    Global.FXPath + filename,
                    ShaderFlags.None);
                fxCollection.Add(sType, effect);
            }
            return effect;
        }
    }
}