#region Disclaimer

/* 
 * TextManager
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
#if !(SlimDX)
    using Microsoft.DirectX;
    using Microsoft.DirectX.Direct3D;
    using Font=Microsoft.DirectX.Direct3D.Font;
#else
using SlimDX;
using SlimDX.Direct3D9;
using Font = SlimDX.Direct3D9.Font;
#endif

namespace AvengersUtd.Odyssey.UserInterface
{
    /// <summary>
    /// Manages the insertion point when drawing text
    /// </summary>
    public class TextManager
    {
        //private Sprite textSprite; // Used to cache the drawn text
        int color; // Color to draw the text
        bool disposed;
        int lineHeight; // Height of the lines
        Point point; // Where to draw the text
        Size size;
        Font textFont; // Used to draw the text

        /// <summary>
        /// Create a new instance of the text helper class
        /// </summary>
        public TextManager(Font f, int l)
        {
            textFont = f;
            //textSprite = s;
            lineHeight = l;
            color = unchecked((int) 0xffffffff);
            point = Point.Empty;
            size = Size.Empty;
        }

        public Font TextFont
        {
            get { return textFont; }
            set { textFont = value; }
        }

        public int LineHeight
        {
            set { lineHeight = value; }
        }

        /// <summary>
        /// Draw a line of text
        /// </summary>
        public void DrawTextLine(string text, DrawTextFormat flags)
        {
            if (textFont == null)
            {
                throw new InvalidOperationException("You cannot draw text.  There is no font object.");
            }
            // Create the rectangle to draw to
            Rectangle rect = new Rectangle(point, size);
#if (!SlimDX)
            textFont.DrawText(OdysseyUI.CurrentHud.SpriteManager, text, rect, flags, color);
#else
            textFont.DrawString(OdysseyUI.CurrentHud.SpriteManager, text, rect, flags, color);
#endif

            // Increase the line height
            point.Y += lineHeight;
        }

        /// <summary>
        /// Draw a line of text
        /// </summary>
        public void DrawTextLine(string text, params object[] args)
        {
            // Simply format the string and pass it on
            DrawTextLine(string.Format(text, args), DrawTextFormat.NoClip);
        }

        /// <summary>
        /// Draw a formatted line of text
        /// </summary>
        public void DrawTextLine(string text, DrawTextFormat flags, params object[] args)
        {
            DrawTextLine(string.Format(text, args), flags);
        }

        /// <summary>
        /// Insertion point of the text
        /// </summary>
        public void SetInsertionPoint(Point p)
        {
            point = p;
        }

        public void SetInsertionPoint(int x, int y)
        {
            point.X = x;
            point.Y = y;
        }

        public void SetInsertionPoint(Vector2 v)
        {
            point.X = (int) v.X;
            point.Y = (int) v.Y;
        }

        public void SetSize(Size size)
        {
            this.size = size;
        }

        /// <summary>
        /// The color of the text
        /// </summary>
        public void SetForegroundColor(int c)
        {
            color = c;
        }

        public void SetForegroundColor(Color c)
        {
            color = c.ToArgb();
        }

        protected void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    // dispose managed components
                    textFont.Dispose();
                }

                // dispose unmanaged components
            }
            disposed = true;
        }


        ~TextManager()
        {
            Dispose(false);
        }
    }
}