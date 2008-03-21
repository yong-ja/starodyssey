#region Disclaimer

/* 
 * CaptionBar
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
using System.Windows.Forms;
using AvengersUtd.Odyssey.UserInterface.Style;
#if !(SlimDX)
    using Microsoft.DirectX;
#else
using SlimDX;
#endif

namespace AvengersUtd.Odyssey.UserInterface.RenderableControls
{
    public enum CaptionButtonType
    {
        Close,
        Help,
        Maximize,
        Minimize,
        Restore
    }

    /// <summary>
    /// This control represents the bar that appears on the top of
    /// windows and dialog panels. It should only be used by the
    /// Window control.
    /// </summary>
    public class CaptionBar : ContainerControl, ISpriteControl
    {
        const string closeButtonTag = "WindowCloseButton";
        internal const string ControlTag = "WindowCaptionBar";
        const int DefaultTitlePaddingX = 8;
        const int DefaultTitlePaddingY = 5;
        internal const int DefaultCaptionBarHeight = 20;
        static int count;
        DecoratedButton closeButton;

        bool drag;
        Vector2 dragStartPosition;
        Label titleLabel;

        #region Properties

        internal Label TitleLabel
        {
            get { return titleLabel; }
        }

        internal DecoratedButton CloseButton
        {
            get { return closeButton; }
        }

        #endregion

        public CaptionBar() : base(ControlTag + count, ControlTag, ControlTag)
        {
            count++;
            CreateSubComponents();
        }

        #region Overriden inherited events

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            drag = true;
            OdysseyUI.CurrentHud.CaptureControl = this;
            dragStartPosition = new Vector2(e.X, e.Y);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (drag)
            {
                Vector2 newDragPosition = new Vector2(e.Location.X, e.Location.Y);
                Parent.Position += newDragPosition - dragStartPosition;

                dragStartPosition = newDragPosition;
            }
            else
                return;
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (drag)
            {
                drag = false;
                OnMouseCaptureChanged(e);
            }
        }

        protected override void OnTextStyleChanged(EventArgs e)
        {
            base.OnTextStyleChanged(e);
            titleLabel.TextStyle = TextStyle;
        }

        #endregion

        #region ISpriteControl Members

        public void Render()
        {
            titleLabel.Render();
        }

        #endregion

        void CreateSubComponents()
        {
            titleLabel = new Label();
            titleLabel.Id = ControlTag + "_Title";
            titleLabel.Position = new Vector2(DefaultTitlePaddingX, DefaultTitlePaddingY);
            titleLabel.TextStyleClass = ControlTag;
            titleLabel.IsSubComponent = true;
            PrivateControlCollection.Add(titleLabel);

            closeButton = new DecoratedButton();
            closeButton.Id = ControlTag + "_CloseButton";
            closeButton.ControlStyle = StyleManager.GetControlStyle(closeButtonTag);
            closeButton.DecorationType = DecorationType.Cross;
            closeButton.IsSubComponent = true;

            closeButton.MouseUp += delegate(object sender, MouseEventArgs e)
                                       {
                                           // Close main window - this refers to the caption bar 
                                           // because we're in its scoe.
                                           ((Window) Parent).Close();
                                       };

            PrivateControlCollection.Add(closeButton);
        }

        protected override void UpdateSizeDependantParameters()
        {
            base.UpdateSizeDependantParameters();
            closeButton.Position =
                new Vector2(Size.Width - closeButton.Size.Width - Padding.Right - Parent.BorderSize, Padding.Top);
        }
    }
}