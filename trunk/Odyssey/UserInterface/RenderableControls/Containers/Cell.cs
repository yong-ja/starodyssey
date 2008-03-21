#region Disclaimer

/* 
 * Cell<TControl>
 *
 * Created on 21 August 2007
 * Author: Adalberto L. Simeone (Taranto, Italy)
 * Website: http://www.avengersutd.com
 *
 * Part of the Odyssey User Interface Library
 *
 * This source code is Intellectual property of the Author
 * and is released under the Creative Commons Attribution 
 * NonCommercial License, available at:
 * http://creativecommons.org/licenses/by-nc/3.0/ 
 * You can alter and use this source code as you wish, 
 * provided that you do not use the results in commercial
 * projects, without the express and written consent of
 * the Author.
 *
 */

#endregion

#region Using directives

using System;
using System.Drawing;
using AvengersUtd.Odyssey.UserInterface.Style;

#endregion

namespace AvengersUtd.Odyssey.UserInterface.RenderableControls
{
    public class Cell<TControl> : SimpleShapeControl, ICell where TControl : BaseControl
    {
        static int count;
        Border borders;

        ShapeDescriptor cellDescriptor;
        ControlCollection controlContainer;
        TControl hostedControl;

        #region Properties

        protected ControlCollection ControlContainer
        {
            get { return controlContainer; }
        }

        protected bool IsEmpty
        {
            get { return controlContainer.IsEmpty; }
        }

        public Border Borders
        {
            get { return borders; }
            set { borders = value; }
        }

        #endregion

        public Cell()
        {
            ShapeDescriptors = new ShapeDescriptorCollection(1);
            ApplyStatusChanges = false;
            controlContainer = new ControlCollection(this);
        }

        #region IRenderable members

        public override bool IntersectTest(Point cursorLocation)
        {
            return Intersection.RectangleTest(AbsolutePosition, Size, cursorLocation);
        }

        public override void CreateShape()
        {
            base.CreateShape();

            if (Padding.IsEmpty)
                hostedControl.ComputeAbsolutePosition();

            cellDescriptor = Shapes.DrawFullRectangle(AbsolutePosition, Size, InnerAreaColor, BorderColor,
                                                      ControlStyle.Shading, BorderSize, BorderStyle,
                                                      Border.All);
            cellDescriptor.Depth = Depth;

            ShapeDescriptors[0] = cellDescriptor;
        }

        public override void UpdateShape()
        {
            cellDescriptor.UpdateShape(Shapes.DrawFullRectangle(AbsolutePosition, Size, InnerAreaColor, BorderColor,
                                                                ControlStyle.Shading, BorderSize, BorderStyle,
                                                                Border.All));
        }


        protected override void UpdatePositionDependantParameters()
        {
            //hostedControl.Position = TopLeftPosition;
        }

        #endregion

        public TControl HostedControl
        {
            get { return hostedControl; }
            internal set
            {
                hostedControl = value;
                controlContainer.Clear();
                controlContainer.Add(hostedControl);
            }
        }

        #region ICell Members

        ControlCollection IContainer.Controls
        {
            get { return ControlContainer; }
        }

        ControlCollection IContainer.PrivateControlCollection
        {
            get { return ControlContainer; }
        }

        ControlCollection IContainer.PublicControlCollection
        {
            get { return ControlContainer; }
        }

        BaseControl ICell.HostedControl
        {
            get { return HostedControl; }
        }

        #endregion

        protected override void OnControlStyleChanged(EventArgs e)
        {
            base.OnControlStyleChanged(e);
            hostedControl.TextStyle = StyleManager.GetTextStyle(ControlStyle.TextStyleClass);
        }

        protected override void UpdateSizeDependantParameters()
        {
            base.UpdateSizeDependantParameters();
            hostedControl.Size = ClientSize;
        }
    }
}