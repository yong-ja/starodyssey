
using System;
using System.Drawing;
using System.Linq;
using AvengersUtd.Odyssey.Geometry;
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
                        //d.Shader = LinearShader.CreateUniform
                        //    (colorShader.Gradient[colorShader.Gradient.Length - 1].Color);
                        //d.FillRectangle();
                        //RadialShader rs = (RadialShader)colorShader;
                        
                        //d.SaveState();
                        //d.Position += new Vector3(rs.Center.X * d.Width, -rs.Center.Y* d.Height, 0);
                        //d.Shader = rs;
                        //d.Width = rs.RadiusX * d.Width;
                        //d.Height = rs.RadiusY * d.Height;
                        //d.DrawEllipse();
                        //d.RestoreState();


                        break;
                }

            }

            float actualWidth = d.Width;
            d.Width = 2;
            d.Shader = LinearShader.CreateUniform(new Color4(0.3f, 0.3f, 0.3f));
            Polygon poly = Polygon.CreateEllipse(new PointF(d.Position.X, d.Position.Y), 50, 125, 24);
            if (actualWidth > 50)
                poly = Polygon.SutherlandHodgmanClip(new OrthoRectangleF(d.Position.X, d.Position.Y, actualWidth, d.Height), poly);
            d.Points = poly.ComputeVector4Array(99);

            //d.Points = new Vector4[] { d.Position.ToVector4(), d.Position.ToVector4() + new Vector4(50, -50f, 0, 1.0f),
            //    d.Position.ToVector4() + new Vector4(75, 200, 0, 1.0f) };
            d.DrawClosedPath();
            return d.Output;
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
