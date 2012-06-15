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
    internal class Decorator : BaseControl
    {
        private const string ControlTag = "Decorator";

        const float DefaultTriangleSideLength = 10f;
        const float DefaultCrossPaddingX = 10.0f;
        const float DefaultCrossPaddingY = 7.0f;
        const float DefaultCrossWidth = 14.0f;

        private static int count;
        private Vector2 decoratorVertexPosition;
        private Vector3 orthoDecoratorVertexPosition;

        readonly Color4[] colors = new[] { new Color4(1.0f, 0f, 0f, 0f), new Color4(1.0f, 0f, 0f, 0f), new Color4(1.0f, 0f, 0f, 0f) };

        public DecorationType DecorationType { get; set; }

        public Decorator()
            : base(ControlTag + (++count), string.Empty)
        {
            IsFocusable = false;
            CanRaiseEvents = false;
            Shapes = new ShapeCollection(1);
        }

        public override void ComputeAbsolutePosition()
        {
            base.ComputeAbsolutePosition();
            decoratorVertexPosition = GetDecoratorVertex(DecorationType, Size);
            orthoDecoratorVertexPosition = Layout.OrthographicTransform(AbsolutePosition + decoratorVertexPosition, Depth.ZOrder, OdysseyUI.CurrentHud.Size);
        }

        public override void CreateShape()
        {
            Shapes[0] = ShapeCreator.DrawEquilateralTriangle(orthoDecoratorVertexPosition, DefaultTriangleSideLength, colors, false);
            Shapes[0].Depth = Depth;
            Shapes[0].Tag = Id;
        }

        public override bool IntersectTest(Vector2 cursorLocation)
        {
            return false;
        }

        public override void UpdateShape()
        {
            ShapeDescription newSDesc = ShapeCreator.DrawEquilateralTriangle(orthoDecoratorVertexPosition,
                DefaultTriangleSideLength, colors, false);
            Shapes[0].UpdateVertices(newSDesc.Vertices);
        }

        static Vector2 GetDecoratorVertex(DecorationType decorationType, Size size)
        {

            switch (decorationType)
            {
                default:
                case DecorationType.DownsideTriangle:
                    return new Vector2(size.Width / 2.0f - DefaultTriangleSideLength / 2,
                                       (size.Height - (float)(DefaultTriangleSideLength / 2 * Math.Sqrt(3))) / 2);

                case DecorationType.Cross:
                    return new Vector2(size.Width / 2.0f - DefaultCrossWidth / 2,
                                       DefaultCrossPaddingY);
            }
        }

        protected override void UpdatePositionDependantParameters()
        {
            return;
        }
        protected override void UpdateSizeDependantParameters()
        {
            return;
        }
    
    }
}
