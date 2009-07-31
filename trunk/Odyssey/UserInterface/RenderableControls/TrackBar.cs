#region Disclaimer

/* 
 * TrackBar
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

namespace AvengersUtd.Odyssey.UserInterface
{
    public class TrackBar : BaseControl
    {
        const string ControlTag = "TrackBar";
        const int DefaultBarHeight = 5;
        const int DefaultMaximumValue = 10;

        const int DefaultMinimumValue = 0;
        const int DefaultSliderWidth = 20;
        const int DefaultTickFrequency = 1;
        const int DefaultTickHeight = 10;
        const int DefaultTickOffset = 10;
        const int DefaultTickWidth = 3;
        static int count;
        Vector2 barAbsolutePosition;
        ShapeDescriptor barDescriptor;
        Vector2 barPosition;

        Size barSize;
        bool drag;
        int maxValue;
        int minValue;
        float pixelsPerTick;
        Size sensitiveArea;
        Vector2 sliderAbsolutePosition;

        ShapeDescriptor sliderDescriptor;
        Vector2 sliderPosition;
        Size sliderSize;
        int tickCount;
        int tickFrequency;

        //TopLeft Corner of the sensitive area when dragging.
        Vector2 topLeft;

        int value;

        #region Properties

        public float Value
        {
            get { return value; }
        }


        public int MinimumValue
        {
            get { return minValue; }
            set
            {
                minValue = value;
                UpdateTicks();
            }
        }

        public int MaximumValue
        {
            get { return maxValue; }
            set
            {
                maxValue = value;
                UpdateTicks();
            }
        }

        public int TickFrequency
        {
            get { return tickFrequency; }
            set
            {
                tickFrequency = value;
                UpdateTicks();
            }
        }

        #endregion

        #region Exposed events

        public event EventHandler ValueChanged;

        protected virtual void OnValueChanged(EventArgs e)
        {
            if (ValueChanged != null)
                ValueChanged(this, e);
        }

        #endregion

        public TrackBar()
            : base(ControlTag + count, ControlTag, ControlTag)
        {
            count++;

            minValue = DefaultMinimumValue;
            maxValue = DefaultMaximumValue;
            tickFrequency = DefaultTickFrequency;
        }

        void UpdateTicks()
        {
            pixelsPerTick = (sensitiveArea.Width - sliderSize.Width)/((maxValue - minValue)/tickFrequency);
            tickCount = 1 + (maxValue - minValue)/tickFrequency;
        }

        public void SetValues(int min, int frequency, int max)
        {
            minValue = min;
            tickFrequency = frequency;
            maxValue = max;

            UpdateTicks();
        }

        public override bool IntersectTest(Point cursorLocation)
        {
            return Intersection.RectangleTest(topLeft, sensitiveArea, cursorLocation);
        }

        public override void CreateShape()
        {
            base.CreateShape();

            barDescriptor = Shapes.DrawFullRectangle(barAbsolutePosition, barSize, InnerAreaColor,
                                                     BorderColor, ControlStyle.Shading, ControlStyle.BorderSize,
                                                     ControlStyle.BorderStyle);
            sliderDescriptor = Shapes.DrawFullRectangle(sliderAbsolutePosition, sliderSize, InnerAreaColor,
                                                        BorderColor, ControlStyle.Shading, ControlStyle.BorderSize,
                                                        ControlStyle.BorderStyle);

            //barDescriptor.Depth = new Depth(windowOrder, layer, zOrder);
            barDescriptor.Depth = Depth;
            sliderDescriptor.Depth = Depth.AsChildOf(Depth);

            // Draw "Ticks"


            ShapeDescriptors = new ShapeDescriptorCollection(2 + tickCount);

            ShapeDescriptors[0] = sliderDescriptor;
            // PositionV3 1 is reserved for the slider descriptor
            ShapeDescriptors[1] = barDescriptor;

            for (int i = 2; i < 2 + tickCount; i++)
            {
                ShapeDescriptor tickDescriptor = Shapes.DrawLine(DefaultTickWidth, ControlStyle.ColorArray.Disabled,
                                                                 new Vector2(
                                                                     topLeft.X + sliderSize.Width/2 +
                                                                     (i - 2)*pixelsPerTick,
                                                                     topLeft.Y + sensitiveArea.Height +
                                                                     DefaultTickOffset),
                                                                 new Vector2(
                                                                     topLeft.X + sliderSize.Width/2 +
                                                                     (i - 2)*pixelsPerTick,
                                                                     topLeft.Y + sensitiveArea.Height +
                                                                     DefaultTickOffset +
                                                                     DefaultTickHeight));
                tickDescriptor.Depth = Depth;

                ShapeDescriptors[i] = tickDescriptor;
            }
        }

        public override void UpdateShape()
        {
            //if (sliderDescriptor.IsDirty)
            sliderDescriptor.UpdateShape(Shapes.DrawFullRectangle(sliderAbsolutePosition, sliderSize, InnerAreaColor,
                                                                  BorderColor, ControlStyle.Shading,
                                                                  ControlStyle.BorderSize,
                                                                  ControlStyle.BorderStyle));

            if (barDescriptor.IsDirty)
            {
                barDescriptor.UpdateShape(Shapes.DrawFullRectangle(barAbsolutePosition, barSize, InnerAreaColor,
                                                                   BorderColor, ControlStyle.Shading,
                                                                   ControlStyle.BorderSize, ControlStyle.BorderStyle));

                for (int i = 2; i < 2 + tickCount; i++)
                {
                    ShapeDescriptor tickDescriptor =
                        Shapes.DrawLine(DefaultTickWidth, ControlStyle.ColorArray.Disabled,
                                        new Vector2(
                                            topLeft.X + sliderSize.Width/2 +
                                            (i - 2)*pixelsPerTick,
                                            topLeft.Y + sensitiveArea.Height +
                                            DefaultTickOffset),
                                        new Vector2(
                                            topLeft.X + sliderSize.Width/2 +
                                            (i - 2)*pixelsPerTick,
                                            topLeft.Y + sensitiveArea.Height +
                                            DefaultTickOffset +
                                            DefaultTickHeight));
                    tickDescriptor.Depth = Depth;
                    ShapeDescriptors[i].UpdateShape(tickDescriptor);
                }
            }
        }


        protected override void UpdatePositionDependantParameters()
        {
            topLeft = new Vector2(AbsolutePosition.X - (sliderSize.Width/2), AbsolutePosition.Y - Size.Height/2);


            sliderAbsolutePosition = sliderPosition + AbsolutePosition;
            barAbsolutePosition = barPosition + AbsolutePosition;
        }

        protected override void UpdateSizeDependantParameters()
        {
            base.UpdateSizeDependantParameters();
            sliderSize = new Size(DefaultSliderWidth, Size.Height);
            barSize = new Size(Size.Width + sliderSize.Width/2, DefaultBarHeight);
            sliderPosition = new Vector2(-(sliderSize.Width/2), (barSize.Height - sliderSize.Height)/2);
            if (value > 0)
                sliderPosition = new Vector2(sliderPosition.X + (value/tickFrequency)*pixelsPerTick, sliderPosition.Y);

            barPosition = new Vector2(-(sliderSize.Width/2), 0);

            sensitiveArea = new Size(barSize.Width, Size.Height);
            UpdateTicks();
        }

        #region Overriden inherited events

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (Intersection.RectangleTest(sliderAbsolutePosition, sliderSize, e.Location))
            {
                drag = true;
                OdysseyUI.CurrentHud.CaptureControl = this;
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (drag)
            {
                drag = false;
                OnMouseCaptureChanged(e);
            }
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            float oldSliderValue = value;
            int tickPosition;
            if (!drag)
            {
                tickPosition = (int) Math.Floor((e.Location.X - topLeft.X)/pixelsPerTick);
                sliderAbsolutePosition.X = topLeft.X + tickPosition*pixelsPerTick;
                value = tickPosition*tickFrequency;
            }

            if (value == oldSliderValue)
                return;
            else
                OnValueChanged(EventArgs.Empty);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (!drag)
                return;
            else
            {
                float oldSliderValue = value;
                int tickPosition;
                tickPosition = (int) Math.Floor((e.Location.X - topLeft.X)/pixelsPerTick);
                sliderAbsolutePosition.X = topLeft.X + tickPosition*pixelsPerTick;
                value = tickPosition*tickFrequency;

                if (sliderAbsolutePosition.X < topLeft.X)
                {
                    sliderAbsolutePosition.X = topLeft.X;
                    value = minValue;
                }
                if (sliderAbsolutePosition.X > AbsolutePosition.X + Size.Width - sliderSize.Width)
                {
                    sliderAbsolutePosition.X = AbsolutePosition.X + Size.Width - sliderSize.Width;
                    value = maxValue;
                }

                if (value == oldSliderValue)
                    return;
                else
                    OnValueChanged(EventArgs.Empty);

                sliderPosition = sliderAbsolutePosition - AbsolutePosition;
                sliderDescriptor.IsDirty = true;

                UpdateAppearance();
            }
        }

        protected override void OnMove(EventArgs e)
        {
            if (DesignMode)
                return;

            // ShapeDescriptor must be set to Dirty otherwise 
            // their position won't be updated till the next
            // Hud.EndDesgin() call.
            for (int i = 0; i < ShapeDescriptors.Length; i++)
            {
                ShapeDescriptor sDesc = ShapeDescriptors[i];
                sDesc.IsDirty = true;
            }
            base.OnMove(e);
        }

        #endregion
    }
}