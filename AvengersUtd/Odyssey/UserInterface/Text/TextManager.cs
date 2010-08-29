using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using AvengersUtd.Odyssey.UserInterface.Controls;
using AvengersUtd.Odyssey.UserInterface.Style;
using AvengersUtd.Odyssey.Utils;
using SlimDX;
using SlimDX.Direct3D11;
using SlimDX.DXGI;

namespace AvengersUtd.Odyssey.UserInterface.Text
{
    public static class TextManager
    {
        private static readonly StringFormat StringFormat;

        static TextManager()
        {
            StringFormat = new StringFormat
                               {
                                   Alignment = StringAlignment.Near,
                                   FormatFlags = StringFormatFlags.FitBlackBox | StringFormatFlags.NoClip,
                                   HotkeyPrefix = HotkeyPrefix.None,
                                   LineAlignment = StringAlignment.Near,
                                   Trimming =  StringTrimming.None,
                               };
        }
        
        
        public static Texture2D DrawText(string text, TextDescription description, bool isHighlighted = false, bool isSelected=false)
        {
            //Our return value.
            Texture2D t;

            using (Font font = description.ToFont())
            {
                //The size of the rendered string.
                SizeF strsize;

                //Create a dummy GDI object so we can figure out the size of the string...
                using (Bitmap dummyimage = new Bitmap(1, 1))
                {
                    using (System.Drawing.Graphics dummygraphics = System.Drawing.Graphics.FromImage(dummyimage))
                    {
                        //Measure the string...
                        strsize = dummygraphics.MeasureString(text, font,PointF.Empty, StringFormat);
                    }
                }

                //Now create the REAL Drawing objects to render the text with.
                using (Bitmap image = new Bitmap((int)strsize.Width, (int)strsize.Height, PixelFormat.Format32bppArgb))
                {
                    using (System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(image))
                    {
                        //Fill the rect with a transparent Color.
                        graphics.Clear(Color.FromArgb(0));

                        //Draw the text
                        Color textColor = (isHighlighted ? description.HighlightedColor :
                            (isSelected ? description.SelectedColor : description.Color)).ToColor();
                        using (Brush brush = new SolidBrush(textColor))
                        {
                            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                            graphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
                            graphics.SmoothingMode = SmoothingMode.HighQuality;

                            GraphicsPath path = new GraphicsPath();
                            path.AddString(text, font.FontFamily, (int)description.FontStyle, description.Size,
                                PointF.Empty, StringFormat);

                            if (description.IsOutlined)
                                graphics.DrawPath(Pens.Black, path);
                            graphics.FillPath(brush, path);
                        }

                        t = ConvertFromBitmap(image);
                    }
                }
            }
            return t;
        }

        static Texture2D ConvertFromBitmap(Bitmap image)
        {
            BitmapData data = image.LockBits(new Rectangle(0, 0, image.Width, image.Height),
										ImageLockMode.ReadWrite, image.PixelFormat);
            int bytes = data.Stride * image.Height;
            DataStream stream = new DataStream(bytes, true, true);
            stream.WriteRange(data.Scan0,bytes);
            stream.Position = 0;
            DataRectangle dRect = new DataRectangle(data.Stride, stream);

            Texture2DDescription texDesc = new Texture2DDescription
            {
                ArraySize = 1,
                MipLevels = 1,
                SampleDescription = new SampleDescription(1,0),
                Format = Format.B8G8R8A8_UNorm,
                CpuAccessFlags = CpuAccessFlags.None,
                BindFlags = BindFlags.ShaderResource,
                Usage = ResourceUsage.Immutable,
                Height = image.Height,
                Width = image.Width
            };

            image.UnlockBits(data);
            image.Dispose();
            Texture2D texture = new Texture2D(Game.Context.Device, texDesc, dRect);
            stream.Dispose();
            //Texture2D.ToFile(Game.Context.Immediate, texture, ImageFileFormat.Jpg, "prova.jpg");
            return texture;

        }

        internal static Vector2 ComputeTextPosition(BaseControl hostControl, TextLiteral textControl)
        {
            float x;
            float y;

            Padding padding = hostControl.Description.Padding;
            int borderSize = hostControl.Description.BorderSize;
            switch (textControl.TextDescription.HorizontalAlignment)
            {
                case HorizontalAlignment.NotSet:
                case HorizontalAlignment.Left:
                    x = padding.Left;
                    break;
                case HorizontalAlignment.Center:
                    x = padding.Horizontal + borderSize + (hostControl.ClientSize.Width/2 - textControl.Size.Width/2);
                    break;
                case HorizontalAlignment.Right:
                    x = (hostControl.ClientSize.Width - textControl.Size.Width);
                    break;
                default:
                    throw Error.WrongCase("HorizontalAlignment", "ComputeTextPosition",
                        textControl.TextDescription.HorizontalAlignment);

            }

            switch (textControl.TextDescription.VerticalAlignment)
            {
                case VerticalAlignment.NotSet:
                case VerticalAlignment.Top:
                    y = padding.Top;
                    break;
                case VerticalAlignment.Center:
                    y = padding.Top + (hostControl.ClientSize.Height/2 - textControl.Size.Height/2);
                    break;
                case VerticalAlignment.Bottom:
                    y = (hostControl.ClientSize.Height - textControl.Size.Height);
                    break;
                default:
                    throw Error.WrongCase("VerticalAlignment", "ComputeTextPosition",
                       textControl.TextDescription.VerticalAlignment);
            }

            return new Vector2(x, y);
        }
    }
}
