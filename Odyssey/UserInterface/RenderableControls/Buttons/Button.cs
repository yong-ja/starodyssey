#region Disclaimer

/* 
 * Button
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
#if !(SlimDX)
    using Microsoft.DirectX;
#else
using SlimDX;
#endif

namespace AvengersUtd.Odyssey.UserInterface.RenderableControls
{
    public class Button : BaseButton, ISpriteControl
    {
        public const string ControlTag = "Button";
        static int count;

        Label label;

        public string Text
        {
            get { return label.Text; }
            set { label.Text = value; }
        }

        #region Constructors

        public Button() : base(ControlTag + count, ControlTag, ControlTag)
        {
            count++;


            label = new Label();
            label.Id = ControlTag + "_Label";
            label.IsSubComponent = true;
            label.Parent = this;

            label.TextStyleClass = TextStyleClass;

            switch (ControlStyle.Shape)
            {
                default:
                case Shape.Rectangle:
                case Shape.RightTrapezoidDownside:
                case Shape.RightTrapezoidUpside:
                    label.Position = TopLeftPosition;
                    break;

                case Shape.LeftTrapezoidDownside:
                case Shape.LeftTrapezoidUpside:
                    label.Position = new Vector2(TabPanel.DefaultTabTriangleWidth, 0);
                    break;
            }
        }

        #endregion

        protected override void OnTextStyleChanged(EventArgs e)
        {
            base.OnTextStyleChanged(e);
            label.TextStyle = TextStyle;
        }


        #region ISpriteControl Members

        public void Render()
        {
            label.Render();
        }

        #endregion

        public override bool IntersectTest(Point cursorLocation)
        {
            return Intersection.RectangleTest(AbsolutePosition, Size, cursorLocation);
        }

        protected override void UpdatePositionDependantParameters()
        {
            label.ComputeAbsolutePosition();
        }
    }
}