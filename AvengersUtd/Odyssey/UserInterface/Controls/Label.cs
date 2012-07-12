using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AvengersUtd.Odyssey.UserInterface.Text;
using AvengersUtd.Odyssey.Graphics.Meshes;
using System.Drawing;

namespace AvengersUtd.Odyssey.UserInterface.Controls
{
    public class Label : SimpleShapeControl, ISpriteObject
    {
        const string ControlTag = "Label";

        private static bool wrapping;
        private static int count;
        private string content;
        private string[] lines;

        internal TextLiteral TextLiteral { get; set; }

        public string Content
        {
            get { return content; }
            set
            {
                content = value;
                lines = TextManager.WrapText(Content, Width, TextDescription.ToFont());
                if (TextLiteral != null)
                    TextLiteral.Content = content;
            }
        }

        public SizeF TextSize
        {
            get
            {
                return TextManager.MeasureSize(content, TextDescription.ToFont());
            }
        }

        public int Lines
        {
            get
            {
                return lines.Length;
            }
        }

        public bool Wrapping
        {
            get { return wrapping; }
            set
            {
                wrapping = value;
                if (TextLiteral != null)
                    TextLiteral.Wrapping = wrapping;
            }
        }

        public override bool DesignMode
        {
            get
            {
                return base.DesignMode;
            }
            protected internal set
            {
                base.DesignMode = value;
                if (TextLiteral != null)
                TextLiteral.DesignMode = value;
            }
        }

        protected internal override Style.Depth Depth
        {
            get
            {
                return base.Depth;
            }
            set
            {
                base.Depth = value;
                if (TextLiteral != null)
                    TextLiteral.Depth = Style.Depth.AsChildOf(this.Depth);
            }
        }

        public Label() : base(ControlTag + ++count, "Empty")
        {
            IsFocusable = false;
        }

        protected override void UpdatePositionDependantParameters()
        {
            if (TextLiteral != null)
                TextLiteral.ComputeAbsolutePosition();
        }

        protected override void UpdateSizeDependantParameters()
        {

            ISpriteObject sObj = (ISpriteObject)this;
            sObj.ComputeAbsolutePosition();
        }

        protected override void OnDisposing(EventArgs e)
        {
            base.OnDisposing(e);
            if (TextLiteral != null && !TextLiteral.Disposed)
                TextLiteral.Dispose();
        }

        #region Overriden events
        protected override void OnTextDescriptionChanged(EventArgs e)
        {
            base.OnTextDescriptionChanged(e);
            if (TextLiteral!=null)
                TextLiteral.TextDescription = TextDescription;
        }

        #endregion

        #region ISpriteObject
        IRenderable ISpriteObject.RenderableObject
        {
            get { return TextLiteral.RenderableObject; }
        }

        bool ISpriteObject.Inited
        {
            get { 
                if (TextLiteral == null)
                    return false;

                return TextLiteral.Inited;
            }
        }

        void ISpriteObject.CreateResource()
        {
            if (TextLiteral != null)
                return;

            TextLiteral = new TextLiteral
            {
                Id = ControlTag + TextLiteral.ControlTag,
                IsDynamic = true,
                Content = string.IsNullOrEmpty(content) ? Id : content,
                IsSubComponent = true,
                DesignMode = this.DesignMode,
                Parent = this,
                TextDescriptionClass = TextDescriptionClass,
                Wrapping = this.Wrapping,
                Bounds = new System.Drawing.Size(Width, Height)
            };
            TextLiteral.Position = TextManager.ComputeTextPosition(this, TextLiteral);
            TextLiteral.CreateResource();
        }

        public override void CreateShape()
        {
            if (TextLiteral == null)
                return;
            TextLiteral.CreateShape();
        }

        public override void ComputeAbsolutePosition()
        {
            base.ComputeAbsolutePosition();
            if (TextLiteral == null)
                return;
            TextLiteral.Position = TextManager.ComputeTextPosition(this, TextLiteral);
            TextLiteral.ComputeAbsolutePosition();

        } 
        #endregion



    }
}
