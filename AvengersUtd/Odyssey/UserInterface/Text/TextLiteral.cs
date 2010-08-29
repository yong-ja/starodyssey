﻿using System;
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
        public bool Dynamic { get; private set; }
       
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
                ColorIndex colorIndex = IsHighlighted
                                            ? ColorIndex.Highlighted
                                            : IsSelected ? ColorIndex.Selected : ColorIndex.Enabled;
                return string.Format("{0} {1}", TextDescription.ActiveCode(colorIndex), text);
            }
        }

        public TextLiteral() : this (false) {}

        public TextLiteral(bool dynamic) :
            base(ControlTag + (++count), "Empty")
        {
            CacheText = true;
            Dynamic = dynamic;
            IsFocusable = false;
            CanRaiseEvents = false;
        }

        public void CreateResource()
        {
            if (CacheText)
                if (!ResourceManager.Contains(Key))
                {
                    Texture2D texture = TextManager.DrawText(text, TextDescription, IsHighlighted, IsSelected);
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
                quad = TexturedPolygon.CreateTexturedQuad(Vector3.Zero, Size.Width, Size.Height, Dynamic);
                quad.PositionV3 = AbsoluteOrthoPosition;
                if (CacheText)
                    quad.DiffuseMapKey = Key;
                else
                    quad.DiffuseMapResource = srvTexture;
            }
            else
            {
                short[] indices;
                TexturedVertex[] vertices = TexturedPolygon.CreateTexturedQuad
                    (Vector3.Zero, Size.Width, Size.Height, out indices);

                DataBox db = Game.Context.Immediate.MapSubresource(quad.VertexBuffer, 0,
                    quad.VertexBuffer.Description.SizeInBytes,
                    MapMode.WriteDiscard, MapFlags.None);
                db.Data.WriteRange(vertices);

                db.Data.Position = 0;
                Game.Context.Immediate.UnmapSubresource(quad.VertexBuffer, 0);

                if (CacheText)
                    quad.DiffuseMapKey = Key;
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
