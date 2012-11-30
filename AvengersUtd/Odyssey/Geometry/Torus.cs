using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AvengersUtd.Odyssey.Graphics.Meshes;
using SlimDX;
using AvengersUtd.Odyssey.Graphics.Materials;

namespace AvengersUtd.Odyssey.Geometry
{
    public class Torus : BaseMesh<TexturedMeshVertex>, ITorus
    {
        public float InnerRadius { get; set; }
        public float SectionRadius { get; set; }
        public float RingRadius { get; set; }

        public Torus(float innerRadius, float sectionRadius, int numSteps1, int numSteps2) : base(TexturedMeshVertex.Description)
        {
            InnerRadius = innerRadius;
            SectionRadius = sectionRadius;
            CreateTorus(innerRadius, sectionRadius, numSteps1, numSteps2);
            Material = new PhongMaterial() { DiffuseColor = new Color4(1.0f, 0.867f, 0.737f, 1.0f) };
            CurrentRotation = Quaternion.RotationAxis(Vector3.UnitX, (float)Math.PI / 2f);
            RingRadius = InnerRadius + SectionRadius / 2;
        }

        void CreateTorus(float innerRadius, float sectionRadius, int numSteps1, int numSteps2)
        {
            // We'll build the torus along the z axis, and then re-orient.

            // First we need the number of vertices. Basically, for every step1, we have step2
            // vertices.
            Vertices = new TexturedMeshVertex[numSteps1 * numSteps2];


            // This will be different than other meshes in that we need some intermediate
            // points, the inner ring.

            Vector3[] ring = new Vector3[numSteps1];
            float angle=0;

            // Find the points for the inner ring

            float step = MathHelper.TwoPi / (float)numSteps1;

            for (int i = 0; i < numSteps1; ++i)
            {
                float x = (float)(Math.Cos(angle) * innerRadius);
                float y = (float)(Math.Sin(angle) * innerRadius);
                ring[i] = new Vector3(x, y, 0);
                angle += step;
            }

            // Now for the actual verts, we create a ring around each point in the inner ring

            for (int i = 0; i < numSteps1; ++i)
            {
                // Basis vectors for this ring are the z axis and the normalized vector
                // from the point to the origin
                Vector3 u = Vector3.Normalize(-ring[i]) * sectionRadius;
                Vector3 v = Vector3.UnitZ * sectionRadius;
                step = MathHelper.TwoPi / (float)numSteps2;

                for (int j = 0; j < numSteps2; ++j)
                {
                    float c = (float)Math.Cos(angle);
                    float s = (float)Math.Sin(angle);
                    Vector3 position = ring[i] + c * u + s * v;
                    Vector3 normal = Vector3.Normalize(position - ring[i]);
                    Vector3 tangent = Vector3.Cross(normal, Vector3.UnitX);
                    Vector3 binormal = Vector3.Cross(normal, tangent);
                    // TODO: calculate torus texture coordinates
                    Vector2 textureUV = new Vector2(i / (float)numSteps1, j / (float)numSteps2);
                    Vertices[i * numSteps2 + j] = new TexturedMeshVertex()
                    {
                        Position = position.ToVector4(),
                        Normal = normal,
                        Tangent = tangent,
                        Binormal = binormal,
                        TextureCoordinate = textureUV
                    };

                    angle += step;
                }
            }

            // Now comes the hard part - triangulating. Let's start by figuring out how
            // many triangles we need. We have numSteps1 'slices', each with numSteps2
            // quads, each with two tris, each with 3 indices. So:

            Indices = new ushort[numSteps1 * numSteps2 * 6];

            // Triangulate

            int index = 0;
            for (int i = 0; i < numSteps1; ++i)
            {
                int i1 = i;
                int i2 = (i1 + 1) % numSteps1;

                for (int j = 0; j < numSteps2; ++j)
                {
                    int j1 = j;
                    int j2 = (j1 + 1) % numSteps2;

                    //Indices[index++] = (ushort)(i1 * numSteps2 + j1);
                    //Indices[index++] = (ushort)(i1 * numSteps2 + j2);
                    //Indices[index++] = (ushort)(i2 * numSteps2 + j1);

                    //Indices[index++] = (ushort)(i2 * numSteps2 + j2);
                    //Indices[index++] = (ushort)(i2 * numSteps2 + j1);
                    //Indices[index++] = (ushort)(i1 * numSteps2 + j2);

                    Indices[index++] = (ushort)(i1 * numSteps2 + j2);
                    Indices[index++] = (ushort)(i2 * numSteps2 + j1);
                    Indices[index++] = (ushort)(i2 * numSteps2 + j2);
                    Indices[index++] = (ushort)(i2 * numSteps2 + j1);
                    Indices[index++] = (ushort)(i1 * numSteps2 + j2);
                    Indices[index++] = (ushort)(i1 * numSteps2 + j1);
                }
            }
        }
    }
}
