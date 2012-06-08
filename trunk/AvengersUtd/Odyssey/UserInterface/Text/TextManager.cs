using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using AvengersUtd.Odyssey.Graphics.ImageProcessing;
using AvengersUtd.Odyssey.UserInterface.Controls;
using AvengersUtd.Odyssey.UserInterface.Style;
using AvengersUtd.Odyssey.Utils;
using SlimDX;
using SlimDX.Direct3D11;
using SlimDX.DXGI;
using System.Collections.Generic;
using System.Text;

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

        public static SizeF MeasureSize(string text, Font font)
        {
            using (Bitmap dummyimage = new Bitmap(1, 1))
            {
                using (System.Drawing.Graphics dummygraphics = System.Drawing.Graphics.FromImage(dummyimage))
                {
                    return dummygraphics.MeasureString(text, font, PointF.Empty, StringFormat);
                }
            }
        }
        
        
        public static Texture2D DrawText(string text, TextDescription description, SizeF size=default(SizeF), bool isHighlighted = false, bool isSelected=false)
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
                        strsize = dummygraphics.MeasureString(text, font, PointF.Empty, StringFormat);

                        if (size.Width > 0  && strsize.Width > size.Width)
                        {
                            string[] lines = WrapText(text, size.Width,  font);
                            strsize = new SizeF(size.Width, strsize.Height * lines.Length);
                        }
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
                            //path.AddString(text, font.FontFamily, (int)description.FontStyle, description.Size,
                                //PointF.Empty, StringFormat);
                            path.AddString(text,font.FontFamily, (int)description.FontStyle, description.Size,
                                new RectangleF(PointF.Empty, strsize), StringFormat);
                            if (description.IsOutlined)
                                graphics.DrawPath(Pens.Black, path);
                            graphics.FillPath(brush, path);
                        }
                        t = ImageHelper.TextureFromBitmap(image);
                    }
                }
            }
            return t;
        }

        public static string[] WrapText(string text, double pixels, Font font)
        {
            List<string> wrappedLines = new List<string>();

            using (Bitmap dummyimage = new Bitmap(1, 1))
            using (System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(dummyimage))
            {
                string[] originalLines = text.Split(new string[] { " " }, StringSplitOptions.None);

                StringBuilder actualLine = new StringBuilder();
                double actualWidth = 0;

                foreach (string item in originalLines)
                {
                    actualLine.Append(item + " ");
                    actualWidth += graphics.MeasureString(item, font, PointF.Empty, StringFormat).Width;

                    if (actualWidth > pixels)
                    {
                        wrappedLines.Add(actualLine.ToString());
                        actualLine.Clear();
                        actualWidth = 0;
                    }
                }

                if (actualWidth > 0)
                    wrappedLines.Add(actualLine.ToString());
            }

            return wrappedLines.ToArray();
        }

        internal static Vector2 ComputeTextPosition(BaseControl hostControl, TextLiteral textControl)
        {
            float x;
            float y;

            Thickness padding = hostControl.Description.Padding;
            Thickness borderSize = hostControl.Description.BorderSize;
            switch (textControl.TextDescription.HorizontalAlignment)
            {
                case HorizontalAlignment.NotSet:
                case HorizontalAlignment.Left:
                    x = borderSize.Left + padding.Left;
                    break;
                case HorizontalAlignment.Center:
                    x = padding.Left + borderSize.Left + (hostControl.ContentAreaSize.Width/2 - textControl.Size.Width/2);
                    break;
                case HorizontalAlignment.Right:
                    x = (hostControl.ContentAreaSize.Width - textControl.Size.Width) - borderSize.Right - padding.Right;
                    break;
                default:
                    throw Error.WrongCase("HorizontalAlignment", "ComputeTextPosition",
                        textControl.TextDescription.HorizontalAlignment);

            }

            switch (textControl.TextDescription.VerticalAlignment)
            {
                case VerticalAlignment.NotSet:
                case VerticalAlignment.Top:
                    y = borderSize.Top + padding.Top;
                    break;
                case VerticalAlignment.Center:
                    y = borderSize.Top + padding.Top + (hostControl.ContentAreaSize.Height/2 - textControl.Size.Height/2);
                    break;
                case VerticalAlignment.Bottom:
                    y = (hostControl.ContentAreaSize.Height - textControl.Size.Height) -borderSize.Bottom - padding.Bottom;
                    break;
                default:
                    throw Error.WrongCase("VerticalAlignment", "ComputeTextPosition",
                       textControl.TextDescription.VerticalAlignment);
            }

            return new Vector2(x, y);
        }
    }
}
