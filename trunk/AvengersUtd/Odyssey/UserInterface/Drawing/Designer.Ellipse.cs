using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AvengersUtd.Odyssey.Geometry;
using AvengersUtd.Odyssey.Geometry.Clipping;
using AvengersUtd.Odyssey.Geometry.Triangulation;
using AvengersUtd.Odyssey.Geometry.Triangulation.Delaunay;
using AvengersUtd.Odyssey.Graphics.Meshes;
using AvengersUtd.Odyssey.UserInterface.Style;
using SlimDX;
using PolyMesh = AvengersUtd.Odyssey.Graphics.Meshes.PolyMesh;

namespace AvengersUtd.Odyssey.UserInterface.Drawing
{
    public partial class Designer
    {
        private const int EllipseSegments = 32;

        public void DrawEllipse()
        {
            CheckParameters(Options.Size | Options.Shader);
            RadialShader rs = (RadialShader) Shader;

            float[] offsets = Shader.Gradient.Select(g => g.Offset).ToArray();
            int segments = offsets.Length;
            Color4[] colors = Shader.Method(Shader, (segments - 1)*(rs.Slices) + 1,
                                            Shape.Rectangle);
            ushort[] indices;
            ColoredVertex[] vertices = PolyMesh.CreateEllipseMesh
                (Position.ToVector4(),
                 Width/2,
                 Height/2,
                 rs.Slices,
                 segments,
                 colors,
                 out indices,
                 offsets);

            ShapeDescription ellipseShape = new ShapeDescription
                                            {
                                                Vertices = vertices,
                                                Indices = indices,
                                                Primitives = indices.Length/3,
                                                Shape = Shape.RectangleMesh
                                            };

            shapes.Add(ellipseShape);
        }

        public void DrawEllipse(Ellipse ellipse)
        {
            Width = (float) ellipse.RadiusX*2;
            Height = (float) ellipse.RadiusY*2;
            Position = new Vector3(ellipse.Center, Position.Z);
            DrawEllipse();
        }

        public void DrawClippedEllipse()
        {
            CheckParameters(Options.Size | Options.Shader);
            RadialShader rs = (RadialShader) Shader;
            // Create a polygon for the control bounds
            OrthoRectangle rectangle = new OrthoRectangle(Position.X, Position.Y, Width, Height);
            // Create the ellipse representing the radial effect

            Ellipse ellipse = rs.CreateEllipse(rectangle);

            // Create the ellipse polygon for the radial gradient
            PolyEllipse ellipsePolygon = PolyEllipse.CreateEllipse(ellipse, EllipseSegments);
            Polygon rectanglePolygon = (Polygon) rectangle;
            //rectanglePolygon.Detail(16);

            float[] offsets = Shader.Gradient.Skip(1).Select(g => g.Offset).ToArray();

            // Determine if we need to clip the inner ellipses
            int innerSegments = DetermineEllipsesToClip(ellipse, rectangle, offsets);

            Ellipse[] ellipses = (from f in offsets 
                                  select new Ellipse(ellipse.Center, f*ellipse.RadiusX, f*ellipse.RadiusY)).ToArray();

            GradientStop[] gradient = rs.Gradient;

            List<Vector2D> points = new List<Vector2D>();
            //List<Polygon> clipResult = new List<Polygon>();
            PolyClipError polyClipError;

            //if (innerSegments < ellipses.Length)
            //{
            PolyEllipse outerEllipse = PolyEllipse.CreateEllipse(ellipse, rs.Slices, offsets);
            //EllipseClipper.ClipAgainstPolygon(outerEllipse, rectanglePolygon);

            for (int i = 0; i < outerEllipse.Slices; i++)
            {

                Polygon ringSlice = outerEllipse.GetRingSlice(i, 0, 3);
                Polygon clipResult = YuPengClipper.Intersect(ringSlice, rectanglePolygon, out polyClipError)[0];
                points.AddRange(clipResult.Vertices);
            }
            //}

            //if (innerSegments < ellipses.Length)
            //{
            //    for (int i = innerSegments; i < ellipses.Length; i++)
            //    {

            //        PolyEllipse firstClippedEllipse = PolyEllipse.CreateEllipse(ellipses[i], rs.Slices);

            //        Polygon clippedAgainstRectangle =
            //            YuPengClipper.Intersect(firstClippedEllipse, rectanglePolygon, out polyClipError)[0];
            //        points.AddRange(clippedAgainstRectangle.Vertices);
            //    }

            //    for (int i = 0; i < innerSegments; i++)
            //    {
            //        PolyEllipse innerEllipse = PolyEllipse.CreateEllipse(ellipses[i], rs.Slices);
            //        points.AddRange(innerEllipse.Vertices);
            //    }

                //points.Insert(0, ellipse.Center);
                ushort[] indices = Delauney.Triangulate(points);

                Color4[] colors = RadialShader.RadialManual2(gradient, points.Count, points, ellipse);
                ShapeDescription ellipseShape = new ShapeDescription
                                                {
                                                    Vertices = points.Select(
                                                        (v, index) =>
                                                        new ColoredVertex(new Vector4(v, Position.Z, 1.0f),
                                                                          colors[index])).ToArray(),
                                                    Indices = indices,
                                                    Primitives = indices.Length / 3,
                                                    Shape = Shape.RectangleMesh
                                                };

                shapes.Add(ellipseShape);
            //}

            //if (innerSegments != 0)
            //{

            //    rs.Gradient = SplitGradient(gradient, 0, innerSegments);
            //    DrawEllipse(ellipses[innerSegments - 1]);
            //    rs.Gradient = gradient;
            //}
        }

        private static GradientStop[] SplitGradient(GradientStop[] gradient, int baseIndex, int count)
        {
            count++;
            GradientStop[] newGradient = new GradientStop[count];
            
            Array.Copy(gradient, baseIndex, newGradient, 0, count);
            float minOffset = newGradient[0].Offset;
            float maxOffset = newGradient[count - 1].Offset;
            for (int i = 1; i < count - 1; i++)
            {
                newGradient[i].Offset =
                    (float) MathHelper.ConvertRange(minOffset, maxOffset, 0, 1, newGradient[i].Offset);
            }
            newGradient[count - 1].Offset = 1.0f;
            return newGradient;
        }

        private float[] DetailOffsets(float minDelta, int innerSegments, IGradientShader rs, out GradientStop[] gradient)
        {
            Dictionary<float, GradientStop> gradientStops = new Dictionary<float, GradientStop>
                                                            {{rs.Gradient[0].Offset, rs.Gradient[0]}};

            float prevOffset = rs.Gradient[0].Offset;
            //foreach (float offset in offsets.Skip(1))
            for (int i = 1; i < rs.Gradient.Length; i++)
            {
                GradientStop gs = rs.Gradient[i];
                float offset = gs.Offset;
                float range = offset - prevOffset;

                if (range <= minDelta)
                {
                    gradientStops.Add(gs.Offset, gs);
                    continue;
                }
                if (i > innerSegments) break;

                int numOffsets = (int) Math.Truncate(range/minDelta);
                for (int j = 1; j <= numOffsets; j++)
                {
                    float newOffset = prevOffset + j*minDelta; //(float)Math.Pow(j*minDelta,2);
                    float weight = (float) MathHelper.ConvertRange(prevOffset, offset, 0, 1.0, newOffset);

                    Color4 newColor = Color4.Lerp(rs.Gradient[i - 1].Color, rs.Gradient[i].Color, weight);
                    if (!gradientStops.Keys.Contains(newOffset))
                        gradientStops.Add(newOffset, new GradientStop(newColor, newOffset));
                }
                //if (!gradientStops.Keys.Contains(offset))
                //    gradientStops.Add(offset,gs);
                prevOffset = offset;
            }

            List<GradientStop> gList = gradientStops.Values.ToList();

            gradient = gList.ToArray();
            return (from g in gList
                    select g.Offset).ToArray();
        }


        private int DetermineEllipsesToClip(Ellipse outer, OrthoRectangle rectangle, float[] segmentOffsets)
        {
            int segments = 0;

            for (int i = 0; i < segmentOffsets.Length; i++)
            {
                float offset = segmentOffsets[i];
                double radiusX = outer.RadiusX*offset;
                double radiusY = outer.RadiusY*offset;
                Ellipse inner = new Ellipse(outer.Center, radiusX, radiusY);

                if (Intersection.RectangleContainsEllipse(rectangle, inner))
                    segments++;
            }
            return segments;
        }
    }
}