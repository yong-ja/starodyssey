#region Disclaimer

/* 
 * Label
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
    using Microsoft.DirectX.Direct3D;
    using Font=Microsoft.DirectX.Direct3D.Font;
#else
using SlimDX;
using SlimDX.Direct3D9;
using Font=SlimDX.Direct3D9.Font;
#endif


namespace AvengersUtd.Odyssey.UserInterface.RenderableControls
{
    public class Label : SimpleShapeControl, ISpriteControl
    {
        const string ControlTag = "Label";
        const int lineHeight = 10;
        static int count;

        bool applyHighlight;
        bool applyShadowing;
        Rectangle area;
        Color color;
        DrawTextFormat flags;
        HorizontalAlignment horizontalAlignment;
        bool ignoreBounds;
        string text;

        TextManager textManager;
        VerticalAlignment verticalAlignment;

        #region Properties

        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        public Rectangle Area
        {
            get
            {
                if (area.IsEmpty && !string.IsNullOrEmpty(text))
                    area = TextStyle.Font.MeasureString(OdysseyUI.CurrentHud.SpriteManager, text, flags
#if (!SlimDX)
                , color
#endif
                        );
                return area;
            }
        }

        [Obsolete("Use the TextStyle property instead.")]
        public Font Font
        {
            get { return textManager.TextFont; }
            set { textManager.TextFont = value; }
        }

        public Color Color
        {
            get { return color; }
            set { color = value; }
        }

        public HorizontalAlignment HorizontalAlignment
        {
            get { return horizontalAlignment; }
            set
            {
                horizontalAlignment = value;
                flags = BuildFlags(TextStyle);
            }
        }

        public VerticalAlignment VerticalAlignment
        {
            get { return verticalAlignment; }
            set
            {
                verticalAlignment = value;
                flags = BuildFlags(TextStyle);
            }
        }

        public bool IgnoreBounds
        {
            get { return ignoreBounds; }
            set
            {
                ignoreBounds = value;
                flags = BuildFlags(TextStyle);
            }
        }

        public bool ApplyHighlight
        {
            get { return applyHighlight; }
            set { applyHighlight = value; }
        }

        public DrawTextFormat Flags
        {
            get { return flags; }
        }

        #endregion

        #region Constructors

        /// <include file='xdoc\Label.xdoc' path='x.doc/x.member[@name="AvengersUtd.Odyssey.UserInterface.RenderableControls.Label.Label"]/*' />
        public Label()
        {
            ApplyControlStyle(ControlStyle.EmptyStyle);
            count++;
            ApplyTextStyle();

            text = string.Empty;
        }

        #endregion

        #region ISpriteControl Members

        public void Render()
        {
            if (!IsVisible)
                return;

            textManager.SetSize(Parent.ClientSize);

            if (applyShadowing)
            {
                textManager.SetForegroundColor(Color.Black);
                textManager.SetInsertionPoint(AbsolutePosition + new Vector2(1, 1));
                textManager.DrawTextLine(text, flags);
            }

            textManager.SetInsertionPoint(AbsolutePosition);
            textManager.SetForegroundColor(color);

            textManager.DrawTextLine(text, flags);
        }

        #region Overriden inherited events

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);
            if (applyHighlight)
                color = TextStyle.HighlightedColor;
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            if (applyHighlight)
                color = TextStyle.StandardColor;
        }

        protected override void OnTextStyleChanged(EventArgs e)
        {
            base.OnTextStyleChanged(e);
            ApplyTextStyle();
            //textStyle = new TextStyle(textStyle.Name + "_Custom",
            //    textStyle.IsBold,
            //    textStyle.IsItalic,
            //    applyShadowing,
            //    ignoreBounds,
            //    textStyle.StandardColor,
            //    textStyle.HighlightedColor,
            //    textStyle.Size,
            //    textStyle.FontName,
            //    horizontalAlignment,
            //    verticalAlignment);
        }

        #endregion

        #endregion

        protected void ApplyTextStyle()
        {
            TextStyle textStyle = TextStyle;

            textManager = new TextManager(TextStyle.Font, lineHeight);
            applyHighlight = textStyle.ApplyHighlight;
            applyShadowing = textStyle.ApplyShadowing;
            ignoreBounds = textStyle.IgnoreBounds;
            horizontalAlignment = textStyle.HorizontalAlignment;
            verticalAlignment = textStyle.VerticalAlignment;
            color = textStyle.StandardColor;
            flags = BuildFlags(TextStyle);
            Size = Area.Size;
        }


        public override bool IntersectTest(Point cursorLocation)
        {
            if (applyHighlight)
                return Intersection.RectangleTest(AbsolutePosition, Size, cursorLocation);
            else
                return false;
        }

        protected override void UpdatePositionDependantParameters()
        {
            return;
        }

        //public Rectangle MeasureSubString(int startIndex, int length)
        //{
        //    return TextStyle.Font.MeasureString(OdysseyUI.CurrentHud.SpriteManager, text.Substring(startIndex, length),
        //                                        flags, color);
        //}

        public Rectangle MeasureStringIfIncreasedBy(char c)
        {
            return TextStyle.Font.MeasureString(OdysseyUI.CurrentHud.SpriteManager, text + c, flags
#if (!SlimDX)
                , color
#endif
                );
        }

        //public Rectangle MeasureStringIfIncreasedBy(string addition)
        //{
        //    return TextStyle.Font.MeasureString(OdysseyUI.CurrentHud.SpriteManager, text + addition, flags, color);
        //}

        public void SetHighlight(bool enabled)
        {
            if (enabled)
                color = TextStyle.HighlightedColor;
            else
                color = TextStyle.StandardColor;
        }

        public static Rectangle MeasureText(string text, TextStyle style)
        {
            return style.Font.MeasureString(OdysseyUI.CurrentHud.SpriteManager,
                                            text, BuildFlags(style)
#if (!SlimDX)                                            
                                            , style.StandardColor
#endif
                                            );
        }

        public static DrawTextFormat BuildFlags(TextStyle style)
        {
            DrawTextFormat flags;

            if (style.IgnoreBounds)
            {
                flags = DrawTextFormat.NoClip;
            }
            else
            {
                flags = DrawTextFormat.ExpandTabs | DrawTextFormat.Wordbreak;
            }

            switch (style.HorizontalAlignment)
            {
                default:
                case HorizontalAlignment.Left:
                    flags |= DrawTextFormat.Left;
                    break;

                case HorizontalAlignment.Right:
                    flags |= DrawTextFormat.Right;
                    break;

                case HorizontalAlignment.Center:
                    flags |= DrawTextFormat.Center;
                    break;
            }

            switch (style.VerticalAlignment)
            {
                default:
                case VerticalAlignment.Top:
                    flags |= DrawTextFormat.Top;
                    break;

                case VerticalAlignment.Center:
#if (!SlimDX)
                    flags |= DrawTextFormat.VerticalCenter;
#else
                    flags |= DrawTextFormat.VCenter;
#endif
                    break;

                case VerticalAlignment.Bottom:
                    flags |= DrawTextFormat.Bottom;
                    break;
            }
            return flags;
        }
    }
}