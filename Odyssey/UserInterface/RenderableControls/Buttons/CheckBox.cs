#region Disclaimer

/* 
 * CheckBox
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

using System;
using System.Drawing;
using System.Windows.Forms;
using AvengersUtd.Odyssey.UserInterface.Style;
#if !(SlimDX)
    using Microsoft.DirectX;
#else
using SlimDX;
#endif

namespace AvengersUtd.Odyssey.UserInterface.RenderableControls
{
    public class CheckBox : BaseButton, ISpriteControl
    {
        const string ControlTag = "CheckBox";
        public const int DefaultCheckBoxLabelOffset = 24;
        public const int DefaultCheckBoxSize = 18;
        public const int DefaultCheckOffset = 4;
        static readonly object EventCheckedChanged = new object();
        static int count;
        ShapeDescriptor boxDescriptor;


        Vector2 checkBottomPosition, checkBottomPositionAbsolutePosition;
        Size checkBoxSize;
        ShapeDescriptor checkDescriptor;
        Vector2 checkTopLeftAbsolutePosition;
        Vector2 checkTopLeftPosition;
        Vector2 checkTopRightAbsolutePosition;
        Vector2 checkTopRightPosition;
        Label label;

        #region Properties 

        public bool IsChecked
        {
            get { return IsSelected; }
            set
            {
                IsSelected = value;
                OnCheckedChanged(EventArgs.Empty);
            }
        }

        public string Caption
        {
            get { return label.Text; }
            set
            {
                label.Text = value;
                OnSizeChanged(EventArgs.Empty);
            }
        }

        #endregion

        public CheckBox() : base(ControlTag + count, ControlTag, ControlTag)
        {
            count++;
            IsFocusable = false;

            label = new Label();
            label.Id = ControlTag + "_Label";
            label.Parent = this;
            label.IsSubComponent = false;
            label.Position = new Vector2(DefaultCheckBoxLabelOffset, 0);
            label.TextStyleClass = ControlTag;

            checkTopLeftPosition = new Vector2(DefaultCheckOffset, DefaultCheckBoxSize/2);
            checkBottomPosition = new Vector2(DefaultCheckBoxSize/2, DefaultCheckBoxSize - DefaultCheckOffset);
            checkTopRightPosition = new Vector2(DefaultCheckBoxSize - DefaultCheckOffset, DefaultCheckOffset);

            checkBoxSize = new Size(DefaultCheckBoxSize, DefaultCheckBoxSize);
        }

        #region ISpriteControl Members

        public void Render()
        {
            label.Render();
        }

        #endregion

        public event EventHandler CheckedChanged
        {
            add { Events.AddHandler(EventCheckedChanged, value); }
            remove { Events.AddHandler(EventCheckedChanged, value); }
        }

        protected virtual void OnCheckedChanged(EventArgs e)
        {
            EventHandler handler = (EventHandler) Events[EventCheckedChanged];
            if (handler != null)
                handler(this, e);
        }

        protected override void UpdateSizeDependantParameters()
        {
            //base.UpdateSizeDependantParameters();

            // Compute individual Size areas
            Size labelSize = label.Area.Size;

            // Compute total Size
            Size = ClientSize = new Size(checkBoxSize.Width + DefaultCheckOffset + labelSize.Width, checkBoxSize.Height);
        }


        public override void CreateShape()
        {
            base.CreateShape();
            boxDescriptor =
                Shapes.DrawFullRectangle(AbsolutePosition, checkBoxSize, InnerAreaColor, BorderColor,
                                         ControlStyle.Shading, BorderSize, BorderStyle, Border.All);
            checkDescriptor =
                Shapes.DrawPolyLine(BorderSize, (IsSelected ? ControlStyle.ColorArray.BorderEnabled : Color.Transparent),
                                    false,
                                    checkTopLeftAbsolutePosition, checkBottomPositionAbsolutePosition,
                                    checkTopRightAbsolutePosition);

            boxDescriptor.Depth = Depth;
            checkDescriptor.Depth = Depth.AsChildOf(Depth);

            ShapeDescriptors = new ShapeDescriptorCollection(2);
            ShapeDescriptors[0] = boxDescriptor;
            ShapeDescriptors[1] = checkDescriptor;
        }

        public override void UpdateShape()
        {
            boxDescriptor.UpdateShape(
                Shapes.DrawFullRectangle(AbsolutePosition, checkBoxSize, InnerAreaColor, BorderColor,
                                         ControlStyle.Shading, BorderSize, BorderStyle, Border.All));

            if (checkDescriptor.IsDirty)
                checkDescriptor.UpdateShape(
                    Shapes.DrawPolyLine(ControlStyle.BorderSize,
                                        (IsSelected ? ControlStyle.ColorArray.BorderEnabled : Color.Transparent), false,
                                        checkTopLeftAbsolutePosition, checkBottomPositionAbsolutePosition,
                                        checkTopRightAbsolutePosition));
        }

        protected override void UpdatePositionDependantParameters()
        {
            checkTopLeftAbsolutePosition = checkTopLeftPosition + AbsolutePosition;
            checkTopRightAbsolutePosition = checkTopRightPosition + AbsolutePosition;
            checkBottomPositionAbsolutePosition = checkBottomPosition + AbsolutePosition;

            label.ComputeAbsolutePosition();
        }

        protected override void OnMove(EventArgs e)
        {
            checkDescriptor.IsDirty = true;
            base.OnMove(e);
        }

        public override bool IntersectTest(Point cursorLocation)
        {
            return Intersection.RectangleTest(AbsolutePosition, Size, cursorLocation);
        }

        #region Exposed events

        protected override void OnMouseClick(MouseEventArgs e)
        {
            checkDescriptor.IsDirty = true;
            IsSelected = !IsSelected;

            base.OnMouseClick(e);
        }


        protected override void OnTextStyleChanged(EventArgs e)
        {
            base.OnTextStyleChanged(e);
            label.TextStyle = TextStyle;
        }

        #endregion
    }
}