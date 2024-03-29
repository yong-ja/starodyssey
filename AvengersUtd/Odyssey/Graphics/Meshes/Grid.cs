﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AvengersUtd.Odyssey.Geometry;
using SlimDX;
using AvengersUtd.Odyssey.Graphics.Materials;
using System.Drawing;

namespace AvengersUtd.Odyssey.Graphics.Meshes
{
    public class Grid : BaseMesh<MeshVertex>
    {
        public float Width { get; private set; }
        public float Height { get; private set; }
        public int Rows { get; private set; }
        public int Columns { get; private set; }

        public Grid(float width, float height, int rows, int columns)
            : base(MeshVertex.Description)
        {
            Width = width;
            Height = height;
            Rows = rows;
            Columns = columns;
            ushort[] indices;
            Vector4[] vertices = PolyMesh.CreateRectangleMesh(Vector4.Zero, width, height, Columns, Rows, out indices);

            Vertices = (from v in vertices select new MeshVertex(v, Vector3.UnitY, Vector2.Zero)).ToArray();
            Indices = indices;
            CurrentRotation = Quaternion.RotationAxis(Vector3.UnitX, MathHelper.PiOver2);
            Material = new WireframeMaterial { DiffuseColor = Color.Green };            
        }



    }
}
