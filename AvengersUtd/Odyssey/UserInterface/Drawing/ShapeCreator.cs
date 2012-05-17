
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using AvengersUtd.Odyssey.Geometry;
using AvengersUtd.Odyssey.Geometry.Triangulation;
using AvengersUtd.Odyssey.UserInterface.Controls;
using AvengersUtd.Odyssey.UserInterface.Style;
using SlimDX;

namespace AvengersUtd.Odyssey.UserInterface.Drawing
{
    public static partial class ShapeCreator
    {
        public static ShapeDescription ComputeData(BaseControl ctl)
        {
            ShapeDescription shape = default(ShapeDescription);

            switch (ctl.Description.Shape)
            {
                case Shape.None:
                    break;
                case Shape.Custom:
                    break;

                case Shape.Triangle:
                    break;

                case Shape.Circle:
                    break;
                case Shape.LeftTrapezoidUpside:
                    break;
                case Shape.LeftTrapezoidDownside:
                    break;
                case Shape.RightTrapezoidUpside:
                    break;
                case Shape.RightTrapezoidDownside:
                    break;

                case Shape.Rectangle:
                case Shape.RectangleMesh:
                    shape = DrawRectangle(ctl);
                    break;
                default:
                    throw Error.WrongCase("shape", "ComputeData", shape);
            }
            shape.Tag = ctl.Id;
            shape.Depth = ctl.Depth;
            return shape;
        }

        static ShapeDescription DrawRectangle(BaseControl control)
        {
            ControlDescription desc = control.Description;
            Designer d = control.GetDesigner();
            
            d.Position = control.AbsoluteOrthoPosition;


            foreach (BorderShader borderShader in desc.BorderShaders)
            {

                d.BorderSize = borderShader.Borders != Borders.All
                                       ? MaskBorderSize(borderShader.Borders, desc.BorderSize)
                                       : desc.BorderSize;
                d.Shader = borderShader;
                d.DrawRectangle();
            }
            
            d.Position = new Vector3
                    (d.Position.X + desc.BorderSize.Left, d.Position.Y - desc.BorderSize.Top, d.Position.Z);
            d.Width = control.ClientSize.Width;
            d.Height = control.ClientSize.Height;
            foreach (IGradientShader colorShader in desc.Enabled)
            {

                switch (colorShader.GradientType)
                {
                    default:
                        d.Shader = colorShader;
                        d.FillRectangle();
                        break;

                    case GradientType.Radial:
                        RadialShader rs = (RadialShader)colorShader;

                        //// First draw background color from the radial shader's outermost color
                        //d.Shader = LinearShader.CreateUniform(rs.Gradient[rs.Gradient.Length - 1].Color);
                        //d.FillRectangle();

                        d.Shader = rs;
                        OrthoRectangle rectangle = new OrthoRectangle(d.Position.X, d.Position.Y, d.Width, d.Height);

                        bool test = Intersection.EllipseRectangleTest(rs.CreateEllipse(rectangle), rectangle);
                        if (!test)
                            d.DrawEllipse();
                        else 
                            d.DrawClippedEllipse();
                        break;

                        
                }

            }


            //d.Vertices = new Vector4[] { d.Position.ToVector4(), d.Position.ToVector4() + new Vector4(50, -50f, 0, 1.0f),
            //    d.Position.ToVector4() + new Vector4(75, 200, 0, 1.0f) };
            
            return d.Output;
        }

        static List<Vector2> ComputeSuperTriangle(List<Vector2> vertex)
        {
            List<Vector2> vertices = new List<Vector2>();
            int nv = vertex.Count;
            // Find the maximum and minimum vertex bounds.
            // This is to allow calculation of the bounding supertriangle
            float xmin = vertex[0].X;
            float ymin = vertex[0].Y;
            float xmax = xmin;
            float ymax = ymin;
            for (int i = 1; i < nv; i++)
            {
                if (vertex[i].X < xmin) xmin = vertex[i].X;
                if (vertex[i].X > xmax) xmax = vertex[i].X;
                if (vertex[i].Y < ymin) ymin = vertex[i].Y;
                if (vertex[i].Y > ymax) ymax = vertex[i].Y;
            }
            float dx = xmax - xmin;
            float dy = ymax - ymin;

            float dmax = (dx > dy) ? dx : dy;

            float xmid = (xmax + xmin) * 0.5f;
            float ymid = (ymax + ymin) * 0.5f;


            // Set up the supertriangle
            // This is a triangle which encompasses all the sample points.
            // The supertriangle coordinates are added to the end of the
            // vertex list. The supertriangle is the first triangle in
            // the triangle list.
            vertices.Add(new Vector2((xmid - 2 * dmax), (ymid - dmax)));
            vertices.Add(new Vector2(xmid, (ymid + 2 * dmax)));
            vertices.Add(new Vector2((xmid + 2 * dmax), (ymid - dmax)));

            return vertices;
        }
     

        static Thickness MaskBorderSize(Borders borders, Thickness borderSize)
        {
            int left=0, top=0, bottom=0, right=0;
            if ((borders & Borders.Left) == Borders.Left)
                left = borderSize.Left;
            if ((borders & Borders.Top) == Borders.Top)
                top = borderSize.Top;
            if ((borders & Borders.Right) == Borders.Right)
                right = borderSize.Right;
            if ((borders & Borders.Bottom) == Borders.Bottom)
                bottom = borderSize.Bottom;

            return new Thickness(left,top,right,bottom);
        }
    }
}
