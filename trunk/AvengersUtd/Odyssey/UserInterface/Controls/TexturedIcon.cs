using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AvengersUtd.Odyssey.UserInterface.Controls;
using AvengersUtd.Odyssey.Graphics.Meshes;
using System.Diagnostics.Contracts;
using SlimDX.Direct3D11;
using System.Drawing;
using SlimDX;

namespace AvengersUtd.Odyssey.UserInterface.Controls
{
    public class TexturedIcon : SimpleShapeControl, ISpriteObject
    {
        const string ControlTag = "Icon";
        static int count;

        private TexturedPolygon quad;
        private ShaderResourceView srvTexture;

        public Texture2D Texture { get; set; }

        public TexturedIcon()
            : base(ControlTag + ++count, "Sprite")
        {
        }

        protected override void UpdatePositionDependantParameters()
        {
            base.UpdatePositionDependantParameters();
            quad.PositionV3 = AbsoluteOrthoPosition;
        }

        protected override void OnDisposing(EventArgs e)
        {
            base.OnDisposing(e);
            if (quad != null && !quad.Disposed)
                quad.Dispose();
            
        }

        #region ISpriteObject
        IRenderable ISpriteObject.RenderableObject
        {
            get { return quad; }
        }

        bool ISpriteObject.Inited
        {
            get
            {
                if (quad == null)
                    return false;

                return quad.Inited;
            }
        }

        void ISpriteObject.CreateResource()
        {
            Contract.Requires<ArgumentNullException>(Texture != null);

            srvTexture = new ShaderResourceView(Game.Context.Device, Texture);
            Texture2DDescription tDesc = ((Texture2D)srvTexture.Resource).Description;
            if (Size == Size.Empty)
                Size = new Size(tDesc.Width, tDesc.Height);
        }

        void ISpriteObject.CreateShape()
        {

            quad = TexturedPolygon.CreateTexturedQuad(Id, Vector3.Zero, Size.Width, Size.Height, false);
            quad.PositionV3 = AbsoluteOrthoPosition;
            quad.DiffuseMapResource = srvTexture;
        }

        void ISpriteObject.ComputeAbsolutePosition()
        {
            base.ComputeAbsolutePosition();
            if (quad != null)
                quad.PositionV3 = AbsoluteOrthoPosition;
        }
        #endregion
    }
}
