﻿#region Disclaimer
// /* 
//  * Author: Adalberto L. Simeone (Taranto, Italy)
//  * E-mail: avengerdragon@gmail.com
//  * Website: http://www.avengersutd.com/blog
//  *
//  * This source code is Intellectual property of the Author
//  * and is released under the Creative Commons Attribution 
//  * NonCommercial License, available at:
//  * http://creativecommons.org/licenses/by-nc/3.0/ 
//  * You can alter and use this source code as you wish, 
//  * provided that you do not use the results in commercial
//  * projects, without the express and written consent of
//  * the Author.
//  *
//  */
#endregion
#region Using Directives

using System;
using System.Collections.Generic;
using System.Linq;
using AvengersUtd.Odyssey.Geometry;
using SlimDX;

#endregion

namespace AvengersUtd.Odyssey.Graphics.Meshes
{
    public partial class PolyMesh
    {
        #region Lines
        public static ColoredVertex[] CreateLineMesh(float width, Color4 color, Vector4 v1, Vector4 v2, out ushort[] indices, ushort baseIndex=(ushort) 0)
        {
            ColoredVertex[] vertices = new ColoredVertex[4];
            float z = v1.Z;
            const float w = 1.0f;
            Vector2 v1i = new Vector2(v1.X, v1.Y);
            Vector2 v2i = new Vector2(v2.X, v2.Y);

            Vector2 vDir = (v1i - v2i);
            vDir = vDir.Perp();
            vDir.Normalize();
            float vLength = (float)Math.Sqrt(vDir.X * vDir.X + vDir.Y * vDir.Y);
            vDir = new Vector2(vDir.X / vLength, vDir.Y / vLength);
            width /= 2;

            Vector2 vTopRight = v1i + (-width * vDir);
            Vector2 vTopLeft = v1i + (width * vDir);
            Vector2 vBottomRight = v2i + (-width * vDir);
            Vector2 vBottomLeft = v2i + (width * vDir);

            vertices[0] = new ColoredVertex(new Vector4(vTopLeft,z, w), color );
            vertices[1] = new ColoredVertex(new Vector4(vTopRight, z, w), color);
            vertices[2] = new ColoredVertex(new Vector4(vBottomRight, z, w), color);
            vertices[3] = new ColoredVertex(new Vector4(vBottomLeft, z, w), color);

            // Top left 0
            // Top right 1
            // Bottom right 2
            // Bottom left 3
            indices = new ushort[]
                      {
                          1, 0, 3,
                          2, 1, 3
                      };

            if (baseIndex>0)
                for (int i = 0; i < indices.Length; i++)
                {
                    indices[i] += baseIndex;
                }

            return vertices;

        }

        public static ColoredVertex[] DrawPolyLine(int width, Color4[] colors, bool closed, IEnumerable<Vector4> points, out ushort[] indices)
        {
            Vector4[] pointsArray = points.ToArray();
            int length = closed ? pointsArray.Length : pointsArray.Length - 1;
            
            ColoredVertex[] vertices = new ColoredVertex[4*length];
            indices = new ushort[6*length];
            ushort vertexCount = 0;
            ushort indexCount = 0;
            Vector4 v1 = pointsArray[0];
            for (int i = 1; i <= length; i++)
            {
                int index = i < length ? i : 0;
                Vector4 v2 = pointsArray[index];
                Color4 color = colors[index];
                ushort[] tempIndices;
                Array.Copy(CreateLineMesh(width, color, v1, v2, out tempIndices, vertexCount), 0, vertices, vertexCount, 4);
                Array.Copy(tempIndices, 0, indices, indexCount, 6);
                vertexCount += 4;
                indexCount += 6;
                v1 = v2;
            }

            return vertices;
        }

        #endregion
        

        #region Triangles
        public static ColoredVertex[] CreateEquilateralTriangle(Vector4 leftVertex, float sideLength, Color4[] colors,
                                                               bool isTriangleUpside, out ushort[] indices)
        {
            ColoredVertex[] vertices = new ColoredVertex[3];
            // Left vertex
            vertices[0] = new ColoredVertex
                              {
                                  Position = new Vector4(leftVertex.X, leftVertex.Y, leftVertex.Z, 1.0f),
                                  Color = colors[0]
                              };
            // Right vertex
            vertices[1] = new ColoredVertex
                              {
                                  Position = new Vector4(leftVertex.X + sideLength, leftVertex.Y, leftVertex.Z, 1.0f),
                                  Color = colors[1]
                              };

            float height = (float) (sideLength/2*Math.Sqrt(3));
            height *= isTriangleUpside ? +1 : -1;

            // Top/Bottom vertex
            vertices[2] = new ColoredVertex
                              {
                                  Position = new Vector4(leftVertex.X+sideLength/2, leftVertex.Y + height, leftVertex.Z, 1.0f),
                                  Color = colors[2]
                              };

            indices = isTriangleUpside
                          ? new ushort[] {0, 1, 2}
                          : new ushort[] {1, 0, 2};
            return vertices;
        }
        #endregion

        #region Quads
        public static ColoredVertex[] CreateQuad(Vector4 topLeftVertex, float width, float height, Color4[] colors,
                                                out ushort[] indices)
        {
            ColoredVertex[] vertices = new ColoredVertex[4];
            // Top left 0
            vertices[0] = new ColoredVertex
            {
                Position =
                    new Vector4(topLeftVertex.X, topLeftVertex.Y,
                    topLeftVertex.Z, 1.0f),
                Color = colors[0]
            };

            // Top right 1
            vertices[1] = new ColoredVertex
            {
                Position =
                    new Vector4(topLeftVertex.X + width, topLeftVertex.Y,
                    topLeftVertex.Z, 1.0f),
                Color = colors[1]
            };
            // Bottom right 2
            vertices[2] = new ColoredVertex
            {
                Position =
                    new Vector4(topLeftVertex.X + width, topLeftVertex.Y - height,
                    topLeftVertex.Z, 1.0f),
                Color = colors[2]
            };

            // Bottom left 3
            vertices[3] = new ColoredVertex
            {
                Position =
                    new Vector4(topLeftVertex.X, topLeftVertex.Y - height,
                    topLeftVertex.Z, 1.0f),
                Color = colors[3]
            };


            
            // Top left 0
            // Top right 1
            // Bottom right 2
            // Bottom left 3
            indices = new ushort[]
                          {
                              1, 0, 3,
                              2, 1, 3
                          };

            return vertices;
        }

        public static ColoredVertex[] CreateRectangleMesh(Vector4 topLeftVertex, float width, float height, int widthSegments, int heightSegments, Color4[] colors,
            out ushort[] indices, float[] widthOffsets=null, float[] heightOffsets=null)
        {
            ColoredVertex[] vertices = new ColoredVertex[(1+widthSegments)*(1+heightSegments)];
            if (widthOffsets != null && widthOffsets.Length != widthSegments+1)
                throw Error.ArgumentInvalid("widthOffsets", typeof (float[]), "CreateRectangleMesh");
            if (heightOffsets != null && heightOffsets.Length != heightSegments+1)
                throw Error.ArgumentInvalid("heightSegments", typeof(float[]), "CreateRectangleMesh");

            float x = topLeftVertex.X;
            float y = topLeftVertex.Y;
            float z = topLeftVertex.Z;

            int vertexCount=0, indexCount = 0;
            indices = new ushort[widthSegments*heightSegments*6];
            // Compute vertices, one row at a time
            for (int i=0; i < heightSegments+1;i++)
            {
                for (int j = 0; j < widthSegments + 1; j++)
                {
                    float hOffset = widthOffsets == null ? j * (width/widthSegments) : widthOffsets[j]*width;
                    float vOffset = heightOffsets == null ? i* (height/heightSegments) : heightOffsets[i]*height;

                    vertices[vertexCount] = new ColoredVertex
                                                {
                                                    Position = new Vector4(x + hOffset, y -vOffset, z, 1.0f),
                                                    Color = colors[vertexCount]
                                                };

                    if (i < heightSegments && j < widthSegments)
                    {

                        indices[indexCount] = (ushort) (vertexCount + 1);
                        indices[indexCount + 1] = (ushort) (vertexCount);
                        indices[indexCount + 2] = (ushort) (vertexCount + widthSegments + 1);

                        indices[indexCount + 3] = (ushort) (indices[indexCount + 2] + 1);
                        indices[indexCount + 4] = indices[indexCount];
                        indices[indexCount + 5] = indices[indexCount + 2];
                        indexCount += 6;
                    }

                    vertexCount++;

                }
            }

            return vertices;
        }

        //public static PolyMesh CreateColoredPolygon(Vector3 topLeftVertex, float width, float height, Color4[] colors)
        //{
        //    PolyMesh coloredPolygon = new PolyMesh(topLeftVertex, width, height, colors);
        //    coloredPolygon.Init();
        //    return coloredPolygon;
        //}

        //public static ColoredVertex[] CreateRectangleOutline(Vector4 topLeftVertex, float width,
        //    float height, Thickness borderSize, int widthSegments, int heightSegments, Color4[] colors,
        //    out ushort[] indices, float[] widthOffsets=null, float[] heightOffsets=null)
        //{
        //    float leftSegmentOffset = borderSize.Left/width;
        //    float rightSegmentOffset = (width - borderSize.Right)/width;
        //    float topSegmentOffset = borderSize.Top/height;
        //    float bottomSegmentOffset = (height - borderSize.Bottom)/height;

        //    if (widthOffsets == null)
        //        widthOffsets = new float[]{0, leftSegmentOffset, rightSegmentOffset, 1};
        //    else
        //    {
        //        List<float> tempList = new List<float>(widthOffsets);
        //        if (leftSegmentOffset > 0)
        //            tempList.Add(leftSegmentOffset);
        //        if (rightSegmentOffset>0)
        //            tempList.Add(rightSegmentOffset);
        //        if (tempList.Count > widthOffsets.Length)
        //            tempList.Sort();
        //    }
        //    if (heightOffsets == null)
        //        heightOffsets = new float[] {0, topSegmentOffset, bottomSegmentOffset, 1};
        //    else
        //    {
        //        List<float> tempList = new List<float>(widthOffsets);
        //        if (topSegmentOffset > 0)
        //            tempList.Add(topSegmentOffset);
        //        if (bottomSegmentOffset > 0)
        //            tempList.Add(bottomSegmentOffset);
        //        if (tempList.Count > heightOffsets.Length)
        //            tempList.Sort();
        //    }

        //    List<ColoredVertex> vertices = new List<ColoredVertex>();
        //    int topHeightSegments= heightOffsets.Count(f => f <= topSegmentOffset);
        //    ushort[] tempIndices;
        //    // Draw top border
        //    vertices.AddRange(CreateRectangleMesh(topLeftVertex,
        //        width, borderSize.Top,
        //        widthSegments,
        //        topHeightSegments,
        //        colors,
        //        out tempIndices,
        //        widthOffsets,
        //        heightOffsets));
        // }


        #endregion

        #region Ellipse

        static Vector4 CreateEllipseVertex(Vector4 center, float theta, float radiusX, float radiusY, float ringOffset=1)
        {
            float x = center.X;
            float y = center.Y;
            float z = center.Z;
            return new Vector4
                        (x+(float)Math.Cos(theta) * (ringOffset * radiusX) ,
                         y-(float)Math.Sin(theta) * (ringOffset * radiusY) ,
                         z,
                         1.0f);
        }

        public static ColoredVertex[] CreateEllipseMesh(Vector4 center, float radiusX, float radiusY, int slices, int segments, Color4[] colors,
            out ushort[] indices, float[] ringOffsets=null)
        {
            float x = center.X;
            float y = center.Y;
            const float radFrom = 0;
            const float radTo = MathHelper.TwoPi;
            float delta = radTo/slices;
            if (ringOffsets == null)
                ringOffsets = new[] {0.0f, 1.0f};
            int rings = ringOffsets.Length;
            ColoredVertex[] vertices = new ColoredVertex[((rings-1)*slices) + 1];

            vertices[0] = new ColoredVertex(center, colors[0]);

            // First ring vertices
            // ringOffsets[0] is assumed to be the center
            // ringOffsets[1] the first ring
            for (int i=0; i<slices; i++)
            {
                float ringOffset = ringOffsets[1];
                float theta = i*delta;
                Vector4 vertexPos = CreateEllipseVertex(center, theta, radiusX, radiusY, ringOffset);

                vertices[i+1] = new ColoredVertex(vertexPos, colors[i+1]);
            }
            indices = new ushort[3*slices*((2*(rings-2))+1)];
            
            // First ring indices
            for (int i = 0; i < slices; i++)
            {
                indices[3 * i] = 0;
                indices[(3 * i) + 1] = (ushort)(i + 2);
                indices[(3 * i) + 2] = (ushort)(i + 1);
            }
            indices[(slices*3) - 2] = 1;

            int indexCount = 0;
            int baseIndex = 3*slices;
            for (int r = 1; r < rings-1; r++)
            {
                // Other rings vertices
                for (int i = 0; i < slices; i++)
                {
                    float ringOffset = ringOffsets[r+1];
                    float theta = i * delta;
                    Vector4 vertexPos = CreateEllipseVertex(center, theta, radiusX, radiusY, ringOffset);

                    vertices[(r*slices) + i+1] = new ColoredVertex(vertexPos, colors[(r*slices) + i + 1]);
                }

                // Other rings indices
                int j = r * slices;
                int k = (r - 1) * slices;
                
                for (int i = 0; i < slices; i++)
                {
                    // current ring
                    
                    // first face
                    indices[baseIndex + indexCount] = (ushort) (j + i+2);
                    indices[baseIndex + indexCount + 1] = (ushort)(j + i + 1);
                    indices[baseIndex + indexCount + 2] = (ushort)(k + i + 1);
                    // second face
                    indices[baseIndex + indexCount + 3] = (ushort)(k + i + 2);
                    indices[baseIndex + indexCount + 4] = (ushort)(j + i + 2);
                    indices[baseIndex + indexCount + 5] = (ushort)(k + i + 1);
                    indexCount += 6;
                }
                // Wrap faces
                indices[baseIndex + indexCount - 2] = (ushort) (r*slices+1);
                indices[baseIndex+indexCount - 3] = (ushort)((r - 1) * slices + 1);

                indices[baseIndex +indexCount - 6] = (ushort)(r * slices + 1);
            }
            return vertices;
        }
        #endregion
    }
}