using System;
using System.Drawing;
using AvengersUtd.Odyssey.Geometry;
using AvengersUtd.Odyssey.Graphics.Meshes;
using AvengersUtd.Odyssey.Graphics.Resources;
using AvengersUtd.Odyssey.UserInterface.Controls;
using AvengersUtd.Odyssey.UserInterface.Style;
using SlimDX;
using SlimDX.Direct3D11;

namespace AvengersUtd.Odyssey.UserInterface.Text
{
    public class TextLiteral : SimpleShapeControl, ISpriteObject
    {
        public const string ControlTag = "Label";
        private static int count;

        private TexturedPolygon quad;
        private ShaderResourceView srvTexture;
        private string text;

        public bool Inited
        {
            get { return quad != null && srvTexture != null && quad.Inited; }
        }

        public bool CacheText { get; set; }
        public bool IsDynamic { get; internal set; }
        public bool Wrapping { get; internal set; }

        public Size Bounds { get; set; }
       
        public string Content
        {
            get { return text; }
            set
            {
                if (text == value) return;
                text = value;
                if (DesignMode) return;
                CreateResource();
                CreateShape();
            }
        }

        internal string Key
        {
            get
            {
                StateIndex stateIndex = IsHighlighted
                                            ? StateIndex.Highlighted
                                            : IsSelected ? StateIndex.Selected : StateIndex.Enabled;
                return string.Format("{0} {1}", TextDescription.ActiveCode(stateIndex), text);
            }
        }

        public TextLiteral() : this (false) {}

        public TextLiteral(bool dynamic) :
            base(ControlTag + (++count), "Empty")
        {
            CacheText = !dynamic;
            IsDynamic = dynamic;
            IsFocusable = false;
            CanRaiseEvents = false;
        }

        public void CreateResource()
        {
            if (CacheText)
                if (!ResourceManager.Contains(Key))
                {
                    Texture2D texture = TextManager.DrawText(
                        text,
                        TextDescription,
                        Wrapping ? new SizeF(Bounds.Width, Bounds.Height) : SizeF.Empty,
                        IsHighlighted,
                        IsSelected);
                    ResourceManager.Add(Key , texture);
                    srvTexture = ResourceManager.GetResource(Key);
                }
                else
                    srvTexture = ResourceManager.GetResource(Key);
            else
            {
                Texture2D texture = TextManager.DrawText(text, TextDescription);
                srvTexture = new ShaderResourceView(Game.Context.Device, texture);
            }

                Texture2DDescription tDesc = ((Texture2D)srvTexture.Resource).Description;
                Size = new Size(tDesc.Width, tDesc.Height);

        }

        public override void CreateShape()
        {
            if (quad == null)
            {
                quad = TexturedPolygon.CreateTexturedQuad(Key, Vector3.Zero, Size.Width, Size.Height, IsDynamic);
                quad.PositionV3 = AbsoluteOrthoPosition;
                if (CacheText)
                    quad.DiffuseMapKey = Key;
                else
                    quad.DiffuseMapResource = srvTexture;
            }
            else
            {
                ushort[] indices;
                TexturedVertex[] vertices = TexturedPolygon.CreateTexturedQuad
                    (Vector3.Zero, Size.Width, Size.Height, out indices);

                DataBox db = Game.Context.Immediate.MapSubresource(quad.VertexBuffer, 0,
                    MapMode.WriteDiscard, MapFlags.None);
                db.Data.WriteRange(vertices);

                db.Data.Position = 0;
                Game.Context.Immediate.UnmapSubresource(quad.VertexBuffer, 0);

                if (CacheText)
                {
                    quad.DiffuseMapKey = Key;
                    quad.Name = Key;
                }
                else
                {
                    quad.DiffuseMapResource.Resource.Dispose();
                    quad.DiffuseMapResource.Dispose();
                    quad.DiffuseMapResource = srvTexture;
                }
            }
        }

        public override void ComputeAbsolutePosition()
        {
            base.ComputeAbsolutePosition();
            if (quad != null)
                quad.PositionV3 = AbsoluteOrthoPosition;
        }
        
        protected override void OnHighlightedChanged(EventArgs e)
        {
            base.OnHighlightedChanged(e);
            CreateResource();
            quad.DiffuseMapKey = Key;
        }

        protected override void OnDisposing(EventArgs e)
        {
            base.OnDisposing(e);
            if (quad != null && !quad.Disposed)
                quad.Dispose();
        }

        #region ISpriteObject Members

        public IRenderable RenderableObject
        {
            get { return quad; }
        }

        #endregion

        public override string ToString()
        {
            return string.Format("{0}: {1}", GetType().Name, Content);
        }
    }
}
