using System;
using System.Drawing;
using AvengersUtd.Odyssey.UserInterface.Style;
using SlimDX;

namespace AvengersUtd.Odyssey.UserInterface.Controls
{
    public class Grid:BaseControl
    {
        const string ControlTag = "Grid";
        public const int DefaultMajorGridLinesFrequency = 5;
        public const int DefaultGridSpacing = 32;

        static int count;

        public int MajorGridLinesFrequency
        {
            get;
            set;
        }
        public int GridSpacing
        {
            get;
            set;
        }

        public bool SnapToGrid { get; set; }

        public Grid()
            : base(ControlTag + ++count, ControlTag)
        {
            count++;
            GridSpacing = DefaultGridSpacing;
            MajorGridLinesFrequency = DefaultMajorGridLinesFrequency;
            CanRaiseEvents = false;
        }

        public override bool IntersectTest(Point cursorLocation)
        {
            return false;
        }

        public override void CreateShape()
        {
            Size = Parent.Size;
            DrawGrid();
        }

        public override void UpdateShape()
        {
            Size = Parent.Size;
            DrawGrid();
        }

        protected override void UpdatePositionDependantParameters()
        {
            return;
        }

        protected override void UpdateSizeDependantParameters()
        {
            return;
        }

        void DrawGrid()
        {
            int horizontalLines = Size.Height / GridSpacing + 1;
            int verticalLines = Size.Width / GridSpacing;

            Shapes = new ShapeCollection(horizontalLines + verticalLines);
            Size horizontalLineSize = new Size(Size.Width, 1);
            Size verticalLineSize = new Size(1, Size.Height);
            Size majorHorizontalLineSize = new Size(Size.Width, 2);
            Size majorVerticalLineSize = new Size(2, Size.Height);

            for (int i = 0; i < horizontalLines; i++)
            {
                Vector2 linePosition = new Vector2(0, GridSpacing * i);
                Vector3 lineOrthoPosition = OrthographicTransform(AbsolutePosition + linePosition, Depth.ZOrder);
                if (i % MajorGridLinesFrequency == 0)
                    Shapes[i] = ShapeCreator.DrawRectangle(lineOrthoPosition,
                                                               majorHorizontalLineSize, Color.Silver);
                else
                    Shapes[i] = ShapeCreator.DrawRectangle(lineOrthoPosition,
                                                               horizontalLineSize, Color.DarkGray);

            }
            for (int i = horizontalLines; i < horizontalLines + verticalLines; i++)
            {
                Vector2 linePosition = new Vector2(GridSpacing * (i - horizontalLines), 0);
                Vector3 lineOrthoPosition = OrthographicTransform(AbsolutePosition + linePosition, Depth.ZOrder);
                if ((i - horizontalLines) % MajorGridLinesFrequency == 0)
                    Shapes[i] = ShapeCreator.DrawRectangle(lineOrthoPosition,
                                                           majorVerticalLineSize, Color.Silver);
                else
                    Shapes[i] = ShapeCreator.DrawRectangle(lineOrthoPosition,
                                                           verticalLineSize, Color.DarkGray);
            }

            foreach (ShapeDescription shape in Shapes)
            {
                shape.Tag = Id;
                shape.Depth = Depth;
            }
        }

        
    }
}
