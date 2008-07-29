﻿using System;
using System.Collections.Generic;
using System.Text;
using AvengersUtd.Odyssey.Objects.Materials;
using AvengersUtd.Odyssey.Meshes;
using AvengersUtd.Odyssey.Resources;
using AvengersUtd.Odyssey;
using SlimDX.Direct3D9;
using SlimDX;

namespace AvengersUtd.StarOdyssey
{
    public class Airplane : BaseEntity
    {

        public Matrix rotation=Matrix.Identity;
        public override void Render()
        {
            Device device = Game.Device;
            device.SetTransform(TransformState.World, rotation * Matrix.Translation(position));
            base.Render();
            
        }

        public Airplane() :
            base(new AvengersUtd.Odyssey.Resources.EntityDescriptor(
                new MeshDescriptor("Airplane", "Meshes\\mouse.x"),
                new MaterialDescriptor(typeof(SpecularMaterial))))
        {
            ((SpecularMaterial)mesh.Materials[0]).Diffuse = new Color4(0f, 1.0f, 1.0f);
        }
    }
}