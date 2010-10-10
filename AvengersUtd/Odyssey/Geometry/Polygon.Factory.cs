#region #Disclaimer

// /* 
//  * Timer
//  *
//  * Created on 21 August 2007
//  * Last update on 29 July 2010
//  * 
//  * Author: Adalberto L. Simeone (Taranto, Italy)
//  * E-Mail: avengerdragon@gmail.com
//  * Website: http://www.avengersutd.com
//  *
//  * Part of the Odyssey Engine.
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

#region Using directives

using System;
using System.Drawing;
using AvengersUtd.Odyssey.UserInterface;
using SlimDX;
using SlimDX.Direct3D11;

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

        public static ColoredVertex[] CreateSubdividedRectangle(Vector4 topLeftVertex, float width, float height, int widthSegments, int heightSegments, Color4[] colors,
                                                out short[] indices)
        {
            ColoredVertex[] vertices = new ColoredVertex[(1+widthSegments)*(1+heightSegments)];
           
            float x = topLeftVertex.X;
            float y = topLeftVertex.Y;
            float z = topLeftVertex.Z;

            float hOffset = width/(widthSegments);
            float vOffset = height/heightSegments;

            int vertexCount=0, indexCount = 0;
            indices = new short[widthSegments*heightSegments*6];
            // Compute vertices, one row at a time
            for (int i=0; i < heightSegments+1;i++)
            {
                for (int j = 0; j < widthSegments + 1; j++)
                {
                    vertices[vertexCount] = new ColoredVertex
                                                {
                                                    Position = new Vector4(x + j*hOffset, y - i*vOffset, z, 1.0f),
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

        #endregion
    }
}