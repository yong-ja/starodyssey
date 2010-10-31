using System;
using System.Diagnostics;
using AvengersUtd.Odyssey.Geometry;
using AvengersUtd.Odyssey.Graphics.Materials;
using SlimDX;

namespace AvengersUtd.Odyssey.Graphics.Meshes
{
    /// <summary>
    /// Generate a spherical mesh of VertexPositionNormalTexture vertices
    /// </summary>
    public class Sphere : BaseMesh<TexturedMeshVertex>, IColor4Material
    {
        private readonly int numStrips;
        private int nextIndex;
        private int nextVertex;

        public float Radius { get; private set; }

        public Color4 DiffuseColor { get; set; }

        public Color4 SpecularColor4 { get; set;}

        public Color4 AmbientColor4 { get; set;}

        /// <summary>
        /// construtor
        /// <param name="numStrips">Number of strips we sphere has between pole and equator</param>
        /// </summary>
        public Sphere(float radius, int numStrips):base(TexturedMeshVertex.Description)
        {
            this.numStrips = numStrips;
            Debug.Assert(1 <= numStrips && numStrips < 20);
            DiffuseColor = new Color4(1, 1, 0, 0);
            Radius = radius;
            AllocateStorage();
            ConstructMeshStructure();

            // check calculations
            Debug.Assert(VertexCount == nextVertex);
            Debug.Assert(IndexCount == nextIndex * 2);

            
        }

        

        
        /// <summary>
        /// Based on number of strips, calculate number space
        /// needed to store Vertexes and allocate it.  
        /// </summary>
        private void AllocateStorage()
        {
            nextIndex = 0;
            nextVertex = 0;

            int numFaces = 8 * numStrips * numStrips;
            Indices = new short[numFaces * 3];

            // nodes in a given strip = (4 x (n-1)) + 1
            // and there are 2n - 1 strips. So:
            int numVertexes = ((4 * (numStrips - 1)) + 10) * (numStrips - 1) + 7;
            Vertices = new TexturedMeshVertex[numVertexes];
        }

        private void ConstructMeshStructure()
        {
            AddFirstMeshStrip();

            // first two vertices in array are the poles, so first vertex of strip will be at [2]
            int previousStripStart = 2;
            for (int strip = 2; strip <= numStrips; ++strip)
            {
                previousStripStart = AddTriangleStrip(strip, previousStripStart);
            }
        }

        /// <summary>
        /// first strip requires special handling, as all vertices attach to the pole
        /// </summary>
        private void AddFirstMeshStrip()
        {
            Vector3 normal = Vector3.UnitY;
            Vector3 tangent = Vector3.Cross(normal, Vector3.UnitX);
            Vector3 binormal = Vector3.Cross(normal, tangent);

            // add north (and south) pole vertex
            AddVertex(normal, tangent, binormal, new Vector2(0.5f, 0.0f), false);

            // first two vertices in array are the poles, so first vertex of strip will be at [2]
            int firstIndex = 2;

            // if we're on the equator, then vertices are adjacent, otherwise they're alternating
            int stepSize = IsEquator(1) ? 1 : 2;

            // now compute the Vertices along this strip
            int numVertex = CreateStripVertexes(1, IsEquator(1));

            // construct the faces
            for (int i = 0; i < numVertex; ++i)
            {
                AddFace(0, firstIndex, firstIndex + stepSize);
                firstIndex += stepSize;
            }
        }

        /// <summary>
        /// create set of faces, between previous latitude and this one
        /// <param name="strip">Band on sphere we're rendering</param>
        /// <param name="previousStripVertex">Index into vertices to first vertex of pervious "strip"</param>
        /// </summary>
        private int AddTriangleStrip(int strip, int previousStripVertex)
        {
            // the vertices for this strip will start here
            int thisStripVertex = nextVertex;

            // create the strip
            CreateStripVertexes(strip, IsEquator(strip));

            // if we're on the equator, then vertices are adjacent, otherwise they're alternating
            // with their southern hemisphere counterpart
            int stepSize = IsEquator(strip) ? 1 : 2;

            // now do the faces between the strips
            // construct the faces
            for (int i = 0; i < 4; ++i)
            {
                for (int j = 0; j < strip - 1; ++j)
                {
                    AddFace(previousStripVertex, thisStripVertex, thisStripVertex + stepSize);
                    thisStripVertex += stepSize;

                    AddFace(thisStripVertex, previousStripVertex + 2, previousStripVertex);
                    previousStripVertex += 2;
                }
                AddFace(previousStripVertex, thisStripVertex, thisStripVertex + stepSize);
                thisStripVertex += stepSize;
            }

            // return pointer to first vectex in this strip
            return previousStripVertex + 2;
        }

        /// <summary>
        /// compute the vertices in this strip, and load into the array
        /// <param name="strip">Band on sphere we're rendering</param>
        /// <param name="isOnEquator">Does this band touch the equator?</param>
        /// <returns>number of vertices added</returns>
        /// </summary>
        private int CreateStripVertexes(int strip, bool isOnEquator)
        {
            // we're going to cheat a bit and go from 0 to 90 degrees
            double longitude = (Math.PI * 0.5 * strip) / numStrips;
            float y = (float)Math.Cos(longitude);
            double s = Math.Sin(longitude);

            Vector3 normal;
            Vector3 tangent;
            Vector3 binormal;

            // number of vertices in this strip
            int numVertexes = (strip * 4);
            for (int v = 0; v < numVertexes; ++v)
            {
                double latitudue = (Math.PI * 2 * v) / numVertexes;
                float x = (float)(-s * Math.Cos(latitudue));
                float z = (float)(s * Math.Sin(latitudue));

                normal = new Vector3(x, y, z);
                tangent = Vector3.Cross(normal, Vector3.UnitX);
                binormal = Vector3.Cross(normal, tangent);

                AddVertex(normal,
                            tangent,
                            binormal,
                            new Vector2((float)(v) / numVertexes, (float)(strip) * 0.5f / numStrips),
                            isOnEquator);
            }

            normal = new Vector3((float)-s, y, 0.0f);
            tangent = Vector3.Cross(normal, Vector3.UnitX);
            binormal = Vector3.Cross(normal, tangent);

            // and we need to add one extra one, to let texture wrap around
            AddVertex(normal,
                       tangent,
                       binormal,
                       new Vector2(1.0f, (float)(strip) * 0.5f / numStrips),
                       isOnEquator);

            return numVertexes;
        }

        /// <summary>
        /// add the vectorIndexes making up a northern face (and it's southern complement) to the index
        /// </summary>
        private void AddFace(int vectorIndex1, int vectorIndex2, int vectorIndex3)
        {
            //// northern face
            AddIndexIntoBuffer(vectorIndex1);
            AddIndexIntoBuffer(vectorIndex3);
            AddIndexIntoBuffer(vectorIndex2);
        }

        /// <summary>
        /// add vectorIndex to end of northern indexes we've added so far
        /// and add the matching southern hemisphere index as well
        /// and update count of number
        /// </summary>
        private void AddIndexIntoBuffer(int vectorIndex)
        {
            // as we need to add a southern face for every northen one
            // we must not fill more than half the index array with northern vertex indices.
            Debug.Assert(nextIndex < (Indices.Length / 2));


            // yes, I know that the vectorIndex should be a short, but due to the nature of
            // how it's called, its cleaner to put one cast in here than multiple elsewhere
            Indices[nextIndex] = (short)vectorIndex;

            // we put southern faces at end of list, in reverse order, because
            // vertex order needs to change (because faces are inverted)
            Indices[IndexCount - nextIndex - 1] = GetSouthernVertexIndex((short)vectorIndex);
            ++nextIndex;
        }

        /// <summary>
        /// return the index to where the matching vertex in southern hemisphere is
        /// </summary>
        private short GetSouthernVertexIndex(short index)
        {
            // if we're on the equator then vertex is it's own complement
            // otherwise, it's the next vertex in the array
            if (index < VertexCount - ((4 * numStrips) + 1))
            {
                ++index;
            }
            return index;
        }

        /// <summary>
        /// add VertexPositionNormalTexture to end of vertices we've calculated so far
        /// note that vertices will be loaded into array as pairs, with the matching
        /// southern hemisphere immediately after the northern one.
        /// </summary>

        private void AddVertex(Vector3 normal, Vector3 tangent, Vector3 binormal, Vector2 texture, bool isOnEquator)
        {
            Vertices[nextVertex] = new TexturedMeshVertex((normal*Radius).ToVector4(), normal, tangent, binormal, texture);

            nextVertex++;
            if (!isOnEquator)
            {
                Vertices[nextVertex] = CreateSouthernVertex(Radius, normal, tangent, texture);
                nextVertex++;
            }
        }

        /// <summary>
        /// returns a VertexPositionNormalTexture that is the southern hemisphere's complement
        /// to the supplied "Vertex"
        /// </summary>
        private static TexturedMeshVertex CreateSouthernVertex(float radius, Vector3 normal, Vector3 tangent, Vector2 texture)
        {
            Vector3 southNormal = new Vector3(normal.X, -normal.Y, normal.Z);
            Vector3 southBinormal = Vector3.Cross(southNormal, tangent);
            Vector2 southTexture = new Vector2(texture.X, 1.0f - texture.Y);

            return new TexturedMeshVertex((southNormal * radius).ToVector4(), southNormal, tangent, southBinormal, southTexture);
        }

        /// <summary>
        /// <returns>true if this strip is the "equator"</returns>
        /// </summary>
        private bool IsEquator(int strip)
        {
            return (strip == numStrips);
        }

        /*
        /// <summary>
        /// Diagnostic output, dump the list of vertices
        /// </summary>
        private void dumpVertexes()
        {
            foreach (VertexPositionNormalTexture v in vertices)
            {
                Debug.WriteLine(v.ToString());
            }
        }

        /// <summary>
        /// Diagnostic output, dump the vertex indexes making up the faces
        /// </summary>
        private void dumpFaces()
        {
            for (int i = 0; i < TotalIndexes; i += 3)
            {
                Debug.WriteLine(
                    String.Format("( {0}, {1}, {2} )",
                        indices[i],
                        indices[i+1],
                        indices[i+2]
                        ) );
            }
        }
        */
    }
}
