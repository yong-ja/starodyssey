using System;
using System.Collections.Generic;
using System.Text;
using AvengersUtd.Odyssey.UserInterface.Style;
using System.Drawing;
using SlimDX;

namespace AvengersUtd.Odyssey.UserInterface.RenderableControls
{
    public class ControlSelector : BaseControl
    {
        const string ControlTag = "ControlSelector";

        public BaseControl TargetControl
        {
            get;
            set;
        }
        //ShapeDescriptor handleNW;
        //ShapeDescriptor handleN;
        //ShapeDescriptor handleNE;
        //ShapeDescriptor handleE;
        //ShapeDescriptor handleSE;
        //ShapeDescriptor handleS;
        //ShapeDescriptor handleSW;
        //ShapeDescriptor handleW;
        
        

        readonly Size handleSize;

        public ControlSelector()
        {
            ApplyControlStyle(StyleManager.GetControlStyle(ControlTag));
            handleSize = new Size(8, 8);
            ShapeDescriptors = new ShapeDescriptorCollection(8);

            //ShapeDescriptors[0] = handleNW;
            //ShapeDescriptors[1] = handleN;
            //ShapeDescriptors[2] = handleNE;
            //ShapeDescriptors[3] = handleE;
            //ShapeDescriptors[4] = handleSE;
            //ShapeDescriptors[5] = handleS;
            //ShapeDescriptors[6] = handleSW;
            //ShapeDescriptors[7] = handleW;

        }

        public override bool IntersectTest(System.Drawing.Point cursorLocation)
        {
            return false;
        }

        public override void CreateShape()
        {
            Vector2[] bounds = ComputeBounds(TargetControl);
            for (int i=0; i < 8; i++)
            {
                Vector2 point = bounds[i];
                Vector2 topLeftCorner = new Vector2(point.X - handleSize.Width/2f, point.Y - handleSize.Height/2f);
                ShapeDescriptors[i] = Shapes.DrawFullRectangle(topLeftCorner, handleSize,
                                                               ControlStyle.ColorArray[ColorIndex.Enabled],
                                                               ControlStyle.ColorArray[ColorIndex.BorderEnabled],
                                                               ControlStyle.Shading,
                                                               ControlStyle.BorderSize,
                                                               ControlStyle.BorderStyle);
            }
        }

        public override void UpdateShape()
        {
            
        }

        protected override void UpdatePositionDependantParameters()
        {
            
        }
    }
}
