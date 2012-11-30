using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX;
using System.Drawing;
using AvengersUtd.Odyssey.Graphics.Materials;

namespace AvengersUtd.Odyssey.Graphics.Meshes
{
    public class Quad:BaseMesh<MeshVertex>
    {

        public Quad()
            : base(MeshVertex.Description)
        {
            
            ushort[] indices;

            //Vertices =PolyMesh.CreateBox(PositionV4, 1, 1, 1, out indices);

            Vector4[] vertices = PolyMesh.CreateRectangleMesh(Vector4.Zero, 1, 1, 1, 1, out indices);

            Vertices = (from v in vertices select new MeshVertex(v, Vector3.UnitY, Vector2.Zero)).ToArray();
            Indices = indices;
            Material = new PhongMaterial { DiffuseColor = Color.Green };
        }
    }
}
