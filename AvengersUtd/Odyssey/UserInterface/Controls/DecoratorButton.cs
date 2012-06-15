using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using AvengersUtd.Odyssey.UserInterface.Drawing;
using AvengersUtd.Odyssey.UserInterface.Style;
using SlimDX;

namespace AvengersUtd.Odyssey.UserInterface.Controls
{
    public class DecoratorButton : BaseControl
    {
        private const string ControlTag = "DecoratorButton";

        private static int count;
        private readonly Decorator decorator;

        public DecorationType DecorationType
        {
            get { return decorator.DecorationType; }
            set { decorator.DecorationType = value; }
        }

        public DecoratorButton()
            : base(ControlTag + (++count), ControlTag)
        {
            IsFocusable = false;
            Shapes = new ShapeCollection(2);
            decorator = new Decorator
                            {
                                Parent = this,
                                DecorationType = DecorationType.DownsideTriangle,
                            };
        }

        public override void CreateShape()
        {
            Shapes[0] = ShapeCreator.ComputeData(this);
            decorator.Depth = Depth.AsChildOf(Depth);
            decorator.Size = ClientSize;
            decorator.ComputeAbsolutePosition();
            decorator.CreateShape();
            Shapes[1] = decorator.Shapes[0];
        }

        public override bool IntersectTest(Vector2 cursorLocation)
        {
            return Geometry.Intersection.RectangleTest(AbsolutePosition, Size, cursorLocation);
        }

        public override void UpdateShape()
        {
            Shapes[0].UpdateVertices(ShapeCreator.ComputeData(this).Vertices);
            decorator.UpdateShape();
            Shapes[1].UpdateVertices(decorator.Shapes[0].Vertices);
        }

        protected override void UpdatePositionDependantParameters()
        {
            decorator.ComputeAbsolutePosition();
        }

        protected override void UpdateSizeDependantParameters()
        {
            if (decorator == null) return;
            decorator.Size = Size;
        }

    }
}
