using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using AvengersUtd.Odyssey.UserInterface.Style;
using SlimDX;

namespace AvengersUtd.Odyssey.UserInterface
{
    public class Grid:BaseControl
    {
        const string ControlTag = "Grid";
        public const int DefaultMajorGridLinesFrequency = 5;
        public const int DefaultGridSpacing = 32;

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
            MajorGridLinesFrequency = DefaultMajorGridLinesFrequency;
            CanRaiseEvents = false;
        }

        public override bool IntersectTest(System.Drawing.Point cursorLocation)
        {
            return false;
        }

        public override void CreateShape()
        {
            int horizontalLines = OdysseyUI.CurrentHud.Size.Height/GridSpacing +1;
            int verticalLines = OdysseyUI.CurrentHud.Size.Width/GridSpacing;
           
            ShapeDescriptors = new ShapeDescriptorCollection(horizontalLines+verticalLines);
            Size horizontalLineSize = new Size(OdysseyUI.CurrentHud.Size.Width, 1);
            Size verticalLineSize = new Size(1,OdysseyUI.CurrentHud.Size.Height);
            Size majorHorizontalLineSize = new Size(OdysseyUI.CurrentHud.Size.Width, 2);
            Size majorVerticalLineSize = new Size(2, OdysseyUI.CurrentHud.Size.Height);
            for (int i=0; i < horizontalLines; i++)
            {
                Vector2 linePosition = new Vector2(0, GridSpacing*i);
                if (i  % MajorGridLinesFrequency == 0)
                    ShapeDescriptors[i] = Shapes.DrawRectangle(AbsolutePosition + linePosition,
                                                               majorHorizontalLineSize, Color.Silver);
                else
                    ShapeDescriptors[i] = Shapes.DrawRectangle(AbsolutePosition + linePosition,
                                                               horizontalLineSize, Color.DarkGray);

            }
            for (int i = horizontalLines; i < horizontalLines+ verticalLines; i++)
            {
                Vector2 linePosition = new Vector2(GridSpacing * (i-horizontalLines),0);
                if ((i-horizontalLines) % MajorGridLinesFrequency == 0)
                    ShapeDescriptors[i] = Shapes.DrawRectangle(AbsolutePosition + linePosition,
                                                           majorVerticalLineSize, Color.Silver);
                else
                    ShapeDescriptors[i] = Shapes.DrawRectangle(AbsolutePosition + linePosition,
                                                           verticalLineSize, Color.DarkGray);
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
