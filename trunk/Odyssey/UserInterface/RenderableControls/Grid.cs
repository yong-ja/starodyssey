using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using AvengersUtd.Odyssey.UserInterface.Style;
using SlimDX;

namespace AvengersUtd.Odyssey.UserInterface.RenderableControls
{
    public class Grid:BaseControl
    {
        const string ControlTag = "Grid";
        const int DefaultMajorGridLinesFrequency = 10;
        const int DefaultGridSpacing = 64;

        static int count=1;

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

        public Grid()
            : base(ControlTag + count, ControlTag, ControlTag)
        {
            count++;
            GridSpacing = DefaultGridSpacing;
            MajorGridLinesFrequency = DefaultGridSpacing;
            CanRaiseEvents = false;
        }

        public override bool IntersectTest(System.Drawing.Point cursorLocation)
        {
            return false;
        }

        public override void CreateShape()
        {
            int horizontalLines = OdysseyUI.CurrentHud.Size.Height/GridSpacing;
            int verticalLines = OdysseyUI.CurrentHud.Size.Width/GridSpacing;
            int majorHorizontalLines = horizontalLines/MajorGridLinesFrequency+1;
            int majorVerticalLines = verticalLines/MajorGridLinesFrequency +1;

            horizontalLines -= majorHorizontalLines;
            verticalLines -= majorVerticalLines;
            ShapeDescriptors = new ShapeDescriptorCollection(horizontalLines+verticalLines);
            Size horizontalLineSize = new Size(OdysseyUI.CurrentHud.Size.Width, 1);
            Size verticalLineSize = new Size(1,OdysseyUI.CurrentHud.Size.Height);
            for (int i=0; i < horizontalLines; i++)
            {
                Vector2 linePosition = new Vector2(0, GridSpacing*i);
                ShapeDescriptors[i] = Shapes.DrawRectangle(AbsolutePosition + linePosition,
                                                           horizontalLineSize, Color.LightGray);
            }
            for (int i = horizontalLines; i < horizontalLines+ verticalLines; i++)
            {
                Vector2 linePosition = new Vector2(GridSpacing * (i-horizontalLines),0);
                ShapeDescriptors[i] = Shapes.DrawRectangle(AbsolutePosition + linePosition,
                                                           verticalLineSize, Color.LightGray);
            }

        }

        public override void UpdateShape()
        {
            return;
        }

        protected override void UpdatePositionDependantParameters()
        {
            return;
        }
    }
}
