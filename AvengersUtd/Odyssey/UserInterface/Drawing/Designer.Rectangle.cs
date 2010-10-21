using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AvengersUtd.Odyssey.Geometry;
using AvengersUtd.Odyssey.UserInterface.Style;
using SlimDX;

namespace AvengersUtd.Odyssey.UserInterface.Drawing
{
    public partial class Designer
    {


        static GradientStop[] SplitGradient(GradientStop[] gradient, float lowerBound, float upperBound)
        {
            IEnumerable<GradientStop> gradientOffsets = from g in gradient
                                                        where
                                                                g.Offset > lowerBound &&
                                                                g.Offset < upperBound
                                                        select g;
            List<GradientStop> gradientList = new List<GradientStop>();
            foreach (GradientStop gradientStop in gradientOffsets)
            {
                GradientStop currentStop = gradientStop;
                GradientStop prevStop = gradient.First(g => g.Offset < currentStop.Offset);
                GradientStop nextStop = gradient.First(g => g.Offset > currentStop.Offset);
                float scaledValue = MathHelper.Scale
                        (currentStop.Offset,
                         prevStop.Offset,
                         nextStop.Offset);
                GradientStop scaledStop = new GradientStop
                                              {
                                                      Color = Color4.Lerp
                                                              (prevStop.Color,
                                                               nextStop.Color,
                                                               scaledValue),
                                                      Offset = scaledValue
                                              };
                gradientList.Add(scaledStop);
            }

            GradientStop firstStop;
            GradientStop lastStop;
            GradientStop upperBoundStop;
            GradientStop lowerBoundStop;

            lowerBoundStop = gradient.FirstOrDefault(g => g.Offset == lowerBound);
            if (lowerBoundStop != null)
            {

                firstStop = lowerBoundStop;
                firstStop.Offset = 0;
            }
            else
            {
                lowerBoundStop = gradient.First(g => g.Offset < lowerBound);
                upperBoundStop = gradient.First(g => g.Offset > lowerBound);
                firstStop = new GradientStop
                        (
                        Color4.Lerp
                                (lowerBoundStop.Color,
                                 upperBoundStop.Color,
                                 MathHelper.Scale
                                         (lowerBound,
                                          lowerBoundStop.Offset,
                                          upperBoundStop.Offset)),
                        0);
            }
            
            upperBoundStop = gradient.FirstOrDefault(g => g.Offset == upperBound);
            if (upperBoundStop != null)
            {
                lastStop = upperBoundStop;
                lastStop.Offset = 1;
            }
            else
            {
                lowerBoundStop = gradient.First(g => g.Offset < upperBound);
                upperBoundStop = gradient.First(g => g.Offset > upperBound);
                lastStop = new GradientStop
                        (
                        Color4.Lerp
                                (lowerBoundStop.Color,
                                 upperBoundStop.Color,
                                 MathHelper.Scale
                                         (upperBound,
                                          lowerBoundStop.Offset,
                                          upperBoundStop.Offset)),
                        1);
            }

            gradientList.Insert(0, firstStop);
            gradientList.Add(lastStop);

            return gradientList.ToArray();
        }

        public void DrawRectangle()
        {
            CheckParameters(Options.Size | Options.BorderShader);

            float actualWidth = Width;
            float actualHeight = Height;
            Vector3 actualPosition = Position;
            float leftSegmentOffset = BorderSize.Left / Width;
            float rightSegmentOffset = (Width - BorderSize.Right) / Width;
            float topSegmentOffset = BorderSize.Top / Height;
            float bottomSegmentOffset = (Height - BorderSize.Bottom) / Height;
         
            SaveState();
            if (BorderSize.Top > 0)
            {
                GradientStop[] gradient;
                switch (BorderShader.GradientType)
                {
                    case GradientType.LinearVerticalGradient:
                        gradient = SplitGradient(BorderShader.Gradient, 0, topSegmentOffset);
                        break;
                    case GradientType.LinearHorizontalGradient:
                        gradient = BorderShader.Gradient;
                        break;
                    default:
                        throw Error.WrongCase("BorderShader.GradientType", "DrawSubdividedRectangleWithOutline",
                        BorderShader.GradientType);
                }
                ColorShader topShader = new ColorShader
                {
                    Gradient = gradient,
                    GradientType = BorderShader.GradientType,
                    Method = BorderShader.Method
                };
                
                Width = actualWidth;
                Height = BorderSize.Top;
                FillShader = topShader;
                FillRectangle();
            }
            if (BorderSize.Left > 0)
            {
                GradientStop[] gradient;
                switch (BorderShader.GradientType)
                {
                    case GradientType.LinearVerticalGradient:
                        gradient = SplitGradient(BorderShader.Gradient, topSegmentOffset, bottomSegmentOffset);
                        break;
                    case GradientType.LinearHorizontalGradient:
                        gradient = SplitGradient(BorderShader.Gradient, 0, leftSegmentOffset);
                        break;
                    default:
                        throw Error.WrongCase("BorderShader.GradientType", "DrawSubdividedRectangleWithOutline",
                        BorderShader.GradientType);
                }
                ColorShader leftShader = new ColorShader
                                             {
                    Gradient = gradient,
                    GradientType = BorderShader.GradientType,
                    Method = BorderShader.Method
                };

                Width = BorderSize.Left;
                Height = actualHeight - BorderSize.Vertical;
                Position = new Vector3(actualPosition.X, actualPosition.Y - BorderSize.Top, actualPosition.Z);
                FillShader = leftShader;
                FillRectangle();
            }
            if (BorderSize.Bottom > 0)
            {
                GradientStop[] gradient;
                switch (BorderShader.GradientType)
                {
                    case GradientType.LinearVerticalGradient:
                        gradient = SplitGradient(BorderShader.Gradient, bottomSegmentOffset, 1);
                        break;
                    case GradientType.LinearHorizontalGradient:
                        gradient = BorderShader.Gradient;
                        break;
                    default:
                        throw Error.WrongCase("BorderShader.GradientType", "DrawSubdividedRectangleWithOutline",
                        BorderShader.GradientType);
                }
                ColorShader bottomShader = new ColorShader
                {
                    Gradient = gradient,
                    GradientType = BorderShader.GradientType,
                    Method = BorderShader.Method
                };

                Width = actualWidth;
                Height = BorderSize.Bottom;
                Position = new Vector3(actualPosition.X, actualPosition.Y - actualHeight + BorderSize.Bottom, actualPosition.Z);
                FillShader = bottomShader;
                FillRectangle();
            }
            if (BorderSize.Right > 0)
            {
                GradientStop[] gradient;
                switch (BorderShader.GradientType)
                {
                    case GradientType.LinearVerticalGradient:
                        gradient = SplitGradient(BorderShader.Gradient, topSegmentOffset, bottomSegmentOffset);
                        break;
                    case GradientType.LinearHorizontalGradient:
                        gradient = SplitGradient(BorderShader.Gradient, rightSegmentOffset, 1);
                        break;
                    default:
                        throw Error.WrongCase("BorderShader.GradientType", "DrawSubdividedRectangleWithOutline",
                        BorderShader.GradientType);
                }
                ColorShader rightShader = new ColorShader
                {
                    Gradient =gradient ,
                    GradientType = BorderShader.GradientType,
                    Method = BorderShader.Method
                };

                Width = BorderSize.Right;
                Height = actualHeight - BorderSize.Vertical;
                Position = new Vector3(actualPosition.X + actualWidth - BorderSize.Right, actualPosition.Y - BorderSize.Top, actualPosition.Z);
                FillShader = rightShader;
                FillRectangle();
            }
            RestoreState();
            
        }

        public void FillRectangle()
        {
            CheckParameters(Options.Size | Options.FillShader);
            int widthSegments;
            int heightSegments;
            float[] offsets = FillShader.Gradient.Select(g => g.Offset).ToArray();
            float[] widthOffsets = null;
            float[] heightOffsets = null;

            switch (FillShader.GradientType)
            {
                case GradientType.Uniform:
                    widthSegments = heightSegments = 1;
                    break;
                case GradientType.LinearVerticalGradient:
                    widthSegments = 1;
                    heightSegments = offsets.Length - 1;
                    heightOffsets = offsets;
                    break;
                case GradientType.LinearHorizontalGradient:
                    widthSegments = offsets.Length - 1;
                    heightSegments = 1;
                    widthOffsets = offsets;
                    break;
                default:
                    throw Error.WrongCase("colorshader.GradientType", "DrawSubdividedRectangleWithOutline",
                        FillShader.GradientType);
            }
            Color4[] colors = FillShader.Method(FillShader, (1 + widthSegments) * (1 + heightSegments), Shape.Rectangle);



            short[] indices;
            ColoredVertex[] vertices = Polygon.CreateRectangleMesh
                    (Position.ToVector4(),
                     Width,
                     Height,
                     widthSegments,
                     heightSegments,
                     colors,
                     out indices,
                     widthOffsets,
                     heightOffsets);

            ShapeDescription rectangleShape = new ShapeDescription
            {
                Vertices = vertices,
                Indices = indices,
                Primitives = indices.Length/3,
                Shape = Shape.RectangleMesh
            };

            shapes.Add(rectangleShape);
        }
    }
}
