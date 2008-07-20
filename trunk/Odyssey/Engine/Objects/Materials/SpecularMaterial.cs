﻿using System;
using System.Collections.Generic;
using System.Text;
using AvengersUtd.Odyssey.Resources;
using SlimDX;

namespace AvengersUtd.Odyssey.Objects.Materials
{
    public class SpecularMaterial : AbstractMaterial
    {
        Color4 diffuseColor;


        public Color4 Diffuse
        {
            get
            {
                return diffuseColor;
            }
            set {
                if (value != diffuseColor)
                {
                    diffuseColor = value;
                    //effectDescriptor = EffectManager.CreateEffect(FXType.Specular, diffuseColor);
                    effectDescriptor = EffectManager.CreateEffect(OwningEntity ,FXType.Specular, diffuseColor);
                    effectDescriptor.UpdateStatic();
                    
                }
                
            }
        }

        public SpecularMaterial()
        {
            fxType = FXType.DepthMap;
        }

        public override void Create(params object[] data)
        {
            diffuseColor = new Color4(0f, 1.0f, 0f);
            
            base.Create(diffuseColor);
            
        }
        public override void Apply()
        {
            effectDescriptor.UpdateDynamic();
            effectDescriptor.Effect.CommitChanges();
        }

        public override bool Disposed
        {
            get { return false; }
        }
    }
}