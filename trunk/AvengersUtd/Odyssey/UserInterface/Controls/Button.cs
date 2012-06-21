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
using AvengersUtd.Odyssey.Graphics.Meshes;
using AvengersUtd.Odyssey.UserInterface.Text;
using SlimDX;

namespace AvengersUtd.Odyssey.UserInterface.Controls
{
    public class Button : SimpleShapeControl, ISpriteObject
    {
        public const string ControlTag = "Button";
        static int count;

        private TextLiteral label;

        #region Properties
        public string Content { get { return label.Content; } set { label.Content = value; } }

        public override bool DesignMode
        {
            get
            {
                return base.DesignMode;
            }
            protected internal set
            {
                base.DesignMode = value;
                label.DesignMode = value;
            }
        } 
        #endregion

        #region Constructors

        public Button() : base(ControlTag + (++count), ControlTag)
        {
            IsFocusable = false;
        }
       
        #endregion

        #region Overriden events
        protected override void OnTextDescriptionChanged(EventArgs e)
        {
            base.OnTextDescriptionChanged(e);
            if (label != null)
                label.TextDescription = TextDescription;
        }

        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);
            if (label != null)
                label.Depth = Style.Depth.AsChildOf(Depth);
        } 
        #endregion

        public override bool IntersectTest(Vector2 cursorLocation)
        {
            return Geometry.Intersection.RectangleTest(AbsolutePosition, Size, cursorLocation);
        }

        protected override void UpdatePositionDependantParameters()
        {
            label.ComputeAbsolutePosition();
        }

        protected override void UpdateSizeDependantParameters()
        {
            label.Position = TextManager.ComputeTextPosition(this, label);
            label.ComputeAbsolutePosition();
        }

        protected override void OnDisposing(EventArgs e)
        {
            base.OnDisposing(e);
            if (!label.Disposed)
                label.Dispose();
        }
        
        #region ISpriteObject

        IRenderable ISpriteObject.RenderableObject
        {
            get { return label.RenderableObject; }
        }

        bool ISpriteObject.Inited
        {
            get
            {
                if (label == null)
                    return false; 
                return label.Inited;
            }
        }


        void ISpriteObject.CreateResource()
        {
            if (label != null)
                return;
            label = new TextLiteral
            {
                Id = ControlTag + TextLiteral.ControlTag,
                Content = Id,
                IsSubComponent = true,
                Parent = this,
                TextDescriptionClass = TextDescriptionClass,
                IsDynamic = true
            };
            label.CreateResource();
        }

        void ISpriteObject.CreateShape()
        {
            label.Position = TextManager.ComputeTextPosition(this, label);
            label.CreateShape();
        }

        void ISpriteObject.ComputeAbsolutePosition()
        {
            label.Position = TextManager.ComputeTextPosition(this, label);
            label.ComputeAbsolutePosition();
            
        } 
        #endregion
    }
}