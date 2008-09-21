#region Disclaimer

/* 
 * OptionButton
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
#if !(SlimDX)
    using Microsoft.DirectX;
#else
using SlimDX;
#endif

namespace AvengersUtd.Odyssey.UserInterface.RenderableControls
{
    /// <summary>
    /// This is the OptionButton control - also known as Radio Button. Instead of creating
    /// several stand-alone OptionButtons, instantiate a single OptionGroup control (which is
    /// a set of OptionButton). To check whether an OptionButton is selected use the IsSelected 
    /// property.
    /// </summary>
    internal class OptionButton : BaseButton, ICircularControl, ISpriteControl
    {
        internal const string ControlTag = "OptionButton";
        public const int DefaultOptionButtonBorderSize = 2;

        public const int DefaultOptionButtonLabelOffset = 26;
        public const int DefaultOptionButtonOutlineRadius = 10;
        public const int DefaultOptionButtonSelectedRadius = 6;
        public const int DefaultOptionButtonSlices = 10;
        static readonly object EventCheckedChanged = new object();
        static int count;
        protected Vector2 center, centerAbsolutePosition;
        protected Label label;
        protected float outlineRadius;
        protected float radius;
        protected int slices;

        #region Properties

        public string Caption
        {
            get { return label.Text; }
            set { label.Text = value; }
        }

        public bool IsChecked
        {
            get { return IsSelected; }
            set
            {
                IsSelected = value;
                OnCheckedChanged(EventArgs.Empty);
            }
        }

        #endregion

        public OptionButton() :
            base(ControlTag + count, ControlTag, ControlTag)
        {
            count ++;

            radius = DefaultOptionButtonSelectedRadius;
            outlineRadius = DefaultOptionButtonOutlineRadius;
            slices = DefaultOptionButtonSlices;
            BorderSize = DefaultOptionButtonBorderSize;

            label = new Label();
            label.Id = ControlTag + "_Label";


            label.Parent = this;
        }

        #region ICircularControl Members

        public Vector2 Center
        {
            get { return center; }
            set { center = value; }
        }

        public Vector2 CenterAbsolutePosition
        {
            get { return centerAbsolutePosition; }
        }

        public float Radius
        {
            get { return radius; }
            set { radius = value; }
        }

        public float OutlineRadius
        {
            get { return outlineRadius; }
            set { outlineRadius = value; }
        }

        public int Slices
        {
            get { return slices; }
            set { slices = value; }
        }

        #endregion

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

        public override bool IntersectTest(Point cursorLocation)
        {
            return Intersection.CircleTest(centerAbsolutePosition, outlineRadius, cursorLocation) ||
                   Intersection.RectangleTest(label.AbsolutePosition, label.Area.Size, cursorLocation);
        }

        protected override void UpdatePositionDependantParameters()
        {
            center = new Vector2(Position.X + DefaultOptionButtonOutlineRadius,
                                 Position.Y + DefaultOptionButtonOutlineRadius);

            label.Position = new Vector2(DefaultOptionButtonLabelOffset, Position.Y);
            centerAbsolutePosition = AbsolutePosition +center;
            label.ComputeAbsolutePosition();
            return;
        }

        #region Overriden inherited events

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            OptionGroup optionGroup = (OptionGroup) Parent;
            optionGroup.Select(Index);
        }

        protected override void OnTextStyleChanged(EventArgs e)
        {
            base.OnTextStyleChanged(e);
            label.TextStyle = TextStyle;
        }

        #endregion
    }
}