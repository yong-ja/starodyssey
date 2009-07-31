#region Disclaimer

/* 
 * Window
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
using AvengersUtd.Odyssey.UserInterface.Style;
#if !(SlimDX)
    using Microsoft.DirectX;
#else
using SlimDX;
#endif

namespace AvengersUtd.Odyssey.UserInterface
{
    public class Window : BaseControl, IContainer
    {
        const string containerPanelTag = "WindowContainer";
        const string ControlTag = "Window";

        static int count;

        CaptionBar captionBar;
        Panel containerPanel;
        bool isModal;
        ControlCollection controls;

        #region Properties

        public bool IsModal
        {
            get { return isModal; }
            protected set { isModal = value; }
        }

        public bool IsActivated
        {
            get { return IsSelected; }
            set { IsSelected = value; }
        }

        public ControlCollection Controls
        {
            get { return PublicControlCollection; }
        }

        protected ControlCollection PublicControlCollection
        {
            get { return containerPanel.Controls; }
        }

        protected ControlCollection PrivateControlCollection
        {
            get { return controls; }
        }

        public string Title
        {
            get { return captionBar.TitleLabel.Text; }
            set { captionBar.TitleLabel.Text = value; }
        }

        internal Size ContainerSize
        {
            get { return containerPanel.Size; }
            set { containerPanel.Size = value; }
        }

        protected internal CaptionBar CaptionBar
        {
            get { return captionBar; }
        }

        protected internal Panel ContainerPanel
        {
            get { return containerPanel; }
        }

        #endregion

        public Window()
        {
            Initialize();
            CreateSubComponents();
        }

        public event EventHandler Closed;

        protected virtual void OnClosed(EventArgs e)
        {
            if (Closed != null)
                Closed(this, e);
        }


        //public Window() : base(ControlTag + count, ControlTag, RenderableControls.CaptionBar.ControlTag)
        //{
        //    Initialize();
        //    CreateSubComponents();
        //}

        void Initialize()
        {
            count++;
            controls = new ControlCollection(this);
            ApplyControlStyle(StyleManager.GetControlStyle(ControlTag));
            IsFocusable = true;
            ApplyStatusChanges = true;
            ShapeDescriptors = new ShapeDescriptorCollection(1);
        }

        protected override void UpdateSizeDependantParameters()
        {
            captionBar.Size = new Size(Size.Width - 2*BorderSize, CaptionBar.DefaultCaptionBarHeight + Padding.Top);
            ClientSize = containerPanel.Size = new Size(Size.Width - (Padding.Horizontal + 2*BorderSize),
                                                        Size.Height -
                                                        (captionBar.Size.Height + Padding.Vertical + 2*BorderSize));
        }


        public void BringToFront()
        {
            OdysseyUI.CurrentHud.WindowManager.BringToFront(this);
        }

        public void Close()
        {
            Hud hud = OdysseyUI.CurrentHud;
            hud.BeginDesign();
            hud.Remove(this);
            hud.EndDesign();

            OnClosed(EventArgs.Empty);
        }

        void CreateSubComponents()
        {
            captionBar = new CaptionBar();
            captionBar.Id = ControlTag + "_Caption";
            captionBar.Position = new Vector2(BorderSize, BorderSize);
            containerPanel = new Panel();
            containerPanel.Id = ControlTag + "_Container";

            containerPanel.ControlStyleClass = containerPanelTag;

            captionBar.IsSubComponent = containerPanel.IsSubComponent = true;
            containerPanel.CanRaiseEvents = false;
            PrivateControlCollection.Add(captionBar);
            PrivateControlCollection.Add(containerPanel);

            TextStyle = captionBar.TextStyle;
        }

        public override void CreateShape()
        {
            base.CreateShape();
            //ShapeDescriptors[0] = Shapes.DrawFullRectangle(AbsolutePosition, Size, InnerAreaColor, BorderColor,
            //                                               ControlStyle.Shading, BorderSize, BorderStyle);
            ShapeDescriptors[0] = Shapes.DrawWindowFrame(AbsolutePosition, TopLeftPosition, Size,
                                                         CaptionBar.Size, ClientSize, Padding, InnerAreaColor,
                                                         BorderColor, ControlStyle.Shading,
                                                         BorderSize, BorderStyle);
            ShapeDescriptors[0].Depth = Depth;
        }

        public override void UpdateShape()
        {
            ShapeDescriptors[0].UpdateShape(Shapes.DrawWindowFrame(AbsolutePosition, TopLeftPosition, Size,
                                                                   CaptionBar.Size, ClientSize, Padding, InnerAreaColor,
                                                                   BorderColor, ControlStyle.Shading,
                                                                   BorderSize, BorderStyle));
            ShapeDescriptors[0].Depth = Depth;
        }

        public override bool IntersectTest(Point cursorLocation)
        {
            return Intersection.RectangleTest(AbsolutePosition, Size, cursorLocation);
        }

        protected override void UpdatePositionDependantParameters()
        {
            containerPanel.Position =
                new Vector2(TopLeftPosition.X, TopLeftPosition.Y + captionBar.Size.Height);
        }

        #region Overriden inherited events

        #endregion

        #region IContainer Members

        ControlCollection IContainer.Controls
        {
            get { return Controls; }
        }

        ControlCollection IContainer.PrivateControlCollection
        {
            get { return PrivateControlCollection; }
        }

        ControlCollection IContainer.PublicControlCollection
        {
            get { return PublicControlCollection; }
        }

        #endregion
    }
}