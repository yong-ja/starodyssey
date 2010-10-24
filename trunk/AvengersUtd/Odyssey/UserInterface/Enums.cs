using System;
using System.Xml.Serialization;

namespace AvengersUtd.Odyssey.UserInterface
{
    public enum HorizontalAlignment
    {
        NotSet,
        Left,
        Center,
        Right,
    }

    public enum VerticalAlignment
    {
        NotSet,
        Top,
        Center,
        Bottom
    }

    public enum BorderStyle
    {
        None,
        Flat,
        Raised,
        Sunken,
    }

    [Flags]
    public enum Borders
    {
        None = 0,
        Top = 1,
        Bottom = 2,
        Left = 4,
        Right = 8,
        All = Top | Bottom | Left | Right,
   }

    public enum Shape
    {
        None = 0,
        Custom,
        Rectangle,
        Circle,
        LeftTrapezoidUpside,
        LeftTrapezoidDownside,
        RightTrapezoidUpside,
        RightTrapezoidDownside,
        Triangle,
        RectangleWithOutline,
        RectangleMesh
    }

    public enum DecorationType
    {
        None = 0,
        UpsideTriangle,
        DownsideTriangle,
        Cross
    }

    public enum UpdateAction
    {
        None = 0,
        UpdateShape,
        Add,
        Remove,
        Move,
        Recompute
    }

    public enum GradientType
    {
        Uniform,
        LinearVerticalGradient,
        LinearHorizontalGradient,
        Radial
    }

    public enum IntersectionLocation
    {
        None,
        Inner,
        CornerNW,
        Top,
        CornerNE,
        Right,
        CornerSE,
        Bottom,
        CornerSW,
        Left
    }
    
}
