#region Disclaimer
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
using AvengersUtd.Odyssey.UserInterface.Style;
using SlimDX;

#endregion

namespace AvengersUtd.Odyssey.Geometry
{
    public partial class Polygon
    {
        #region Triangles
        public static ColoredVertex[] CreateEquilateralTriangle(Vector4 leftVertex, float sideLength, Color4[] colors,
                                                               bool isTriangleUpside, out short[] indices)
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
                          ? new short[] {0, 1, 2}
                          : new short[] {1, 0, 2};
            return vertices;
        }
        #endregion

        #region Quads
        public static ColoredVertex[] CreateQuad(Vector4 topLeftVertex, float width, float height, Color4[] colors,
                                                out short[] indices)
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
            indices = new short[]
                          {
                              1, 0, 3,
                              2, 1, 3
                          };

            return vertices;
        }

        public static ColoredVertex[] CreateRectangleMesh(Vector4 topLeftVertex, float width, float height, int widthSegments, int heightSegments, Color4[] colors,
            out short[] indices, float[] widthOffsets=null, float[] heightOffsets=null)
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
            indices = new short[widthSegments*heightSegments*6];
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

                        indices[indexCount] = (short) (vertexCount + 1);
                        indices[indexCount + 1] = (short) (vertexCount);
                        indices[indexCount + 2] = (short) (vertexCount + widthSegments + 1);

                        indices[indexCount + 3] = (short) (indices[indexCount + 2] + 1);
                        indices[indexCount + 4] = indices[indexCount];
                        indices[indexCount + 5] = indices[indexCount + 2];
                        indexCount += 6;
                    }

                    vertexCount++;

                }
            }

            return vertices;
        }

        //public static Polygon CreateColoredPolygon(Vector3 topLeftVertex, float width, float height, Color4[] colors)
        //{
        //    Polygon coloredPolygon = new Polygon(topLeftVertex, width, height, colors);
        //    coloredPolygon.Init();
        //    return coloredPolygon;
        //}

        //public static ColoredVertex[] CreateRectangleOutline(Vector4 topLeftVertex, float width,
        //    float height, Thickness borderSize, int widthSegments, int heightSegments, Color4[] colors,
        //    out short[] indices, float[] widthOffsets=null, float[] heightOffsets=null)
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
        //    short[] tempIndices;
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
        public static ColoredVertex[] CreateEllipseMesh(Vector4 center, float radiusX, float radiusY, int slices, int segments, Color4[] colors,
            out short[] indices, float[] widthOffsets=null, float[] heightOffsets=null)
        {
            float x = center.X;
            float y = center.Y;
            const float radFrom = 0;
            const float radTo = 2;
            float deltaRad = radTo/slices;
            float delta = radFrom;
            ColoredVertex[] vertices = new ColoredVertex[slices +2 ];

            vertices[0] = new ColoredVertex(center, colors[0]);

            for (int i=1; i<slices; i++)
            {
                Vector4 vertexPos = new Vector4
                        ((float) Math.Cos(delta)*radiusX + x,
                         (float) Math.Sin(delta)*radiusY + y,
                         center.Z,
                         1.0f);
                delta -= deltaRad;
                
                vertices[i] = new ColoredVertex(vertexPos, colors[i]);
            }

            indices = new short[slices*3];
            indices[0] = 0;
            indices[1] = 1;

            for (int i = 0; i < slices; i++)
            {
                indices[3 * i] = 0;
                indices[(3 * i) + 1] = (short)(i + 2);
                indices[(3 * i) + 2] = (short)(i + 1);
            }

            return vertices;
        }
        #endregion
    }
}