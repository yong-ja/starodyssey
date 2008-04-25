#region Disclaimer

/* 
 * FontManager
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

#region Using directives

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using AvengersUtd.Odyssey.UserInterface.Style;
#if !(SlimDX)
    using Microsoft.DirectX;
    using Microsoft.DirectX.Direct3D;
    using Font=Microsoft.DirectX.Direct3D.Font;
#else
using SlimDX.Direct3D9;
using Font=SlimDX.Direct3D9.Font;
#endif


#endregion

namespace AvengersUtd.Odyssey.UserInterface.Style
{
    public static class FontManager
    {
        public const string DefaultFontName = "Arial";


        static Dictionary<string, int[]> CharacterLengths;
        static Dictionary<string, Font> fontCache;

        #region Properties

        #endregion

        static FontManager()
        {
            fontCache = new Dictionary<string, Font>();
            CharacterLengths = new Dictionary<string, int[]>();
        }

        public static int GetCharacterLength(char c, TextStyle style)
        {
            return CharacterLengths[style.ToString()][c];
        }

        public static int MeasureString(string text, TextStyle style)
        {
            char[] charArray = text.ToCharArray();
            int width = 0;
            int[] lookupArray = CharacterLengths[style.ToString()];

            for (int i = 0; i < charArray.Length; i++)
            {
                char c = charArray[i];
                width += lookupArray[c];
            }
            return width;
        }

        public static int[] GetCharLengthArray(string text, TextStyle style)
        {
            string fontId = style.ToString();
            char[] charArray = text.ToCharArray();
            int[] charLengthArray = new int[charArray.Length];
            int[] lookupArray = CharacterLengths[fontId];

            for (int i = 0; i < charArray.Length; i++)
            {
                charLengthArray[i] = lookupArray[charArray[i]];
            }

            return charLengthArray;
        }

        static int[] ComputeCharLength(Font font)
        {
            DrawTextFormat flags = DrawTextFormat.NoClip | DrawTextFormat.Left | DrawTextFormat.Top;
            int[] charLengthArray = new int[255];
            for (int i = 0; i < 255; i++)
            {
                Char c = (char) i;
                charLengthArray[i] = font.MeasureString(
                    OdysseyUI.CurrentHud.SpriteManager,
                    c.ToString(),
                    flags
#if (!SlimDX)
                , Color.White
#endif
                    ).Width;

            }
            charLengthArray[32] = font.MeasureString(
                                      OdysseyUI.CurrentHud.SpriteManager,
                                      " .",
                                      flags
#if (!SlimDX)
                                     , Color.White
#endif
                                      ).Width

                                  - charLengthArray[(int) '.'];
            return charLengthArray;
        }

        public static Font CreateFont(TextStyle style)
        {
            return CreateFont(style.FontName, style.Size,
                              (style.IsBold) ? FontWeight.Bold : FontWeight.Regular, style.IsItalic);
        }

        /// <summary>
        /// Creates a user defined font object.
        /// </summary>
        /// <param name="fontName">The font name</param>
        /// <param name="size">Font Size.</param>
        /// <param name="weight">IsBold/Demi bold, etc.</param>
        /// <param name="isItalic">True if the text is too be rendered in italic.</param>
        /// <returns>The font object.</returns>
        public static Font CreateFont(string fontName, int size, FontWeight weight, bool isItalic)
        {
            String fontKey = fontName + size + weight.ToString() + (isItalic ? "Italic" : string.Empty);

            if (fontCache.ContainsKey(fontKey))
                return fontCache[fontKey];
            else
            {
                Font font = new Font(
                    OdysseyUI.Device,
                    size,
                    0,
                    weight,
                    1,
                    isItalic,
                    CharacterSet.Default,
#if (!SlimDX)
                    Precision.Tt,
                    FontQuality.AntiAliased | FontQuality.ClearTypeNatural,
                    PitchAndFamily.FamilyRoman,
#else
                    Precision.TrueType,
                    FontQuality.Antialiased | FontQuality.ClearTypeNatural,
                    PitchAndFamily.Roman,
#endif
                    fontName
                    );

                fontCache.Add(fontKey, font);
                CharacterLengths.Add(fontKey, ComputeCharLength(font));

                return font;
            }
        }

        /// <summary>
        /// Returns a font object from the default type.
        /// </summary>
        /// <param name="Size">Font Size.</param>
        /// <param name="weight">IsBold/Demi bold, etc.</param>
        /// <param name="italic">True if the text is too be rendered in italic.</param>
        /// <returns>The font object.</returns>
        public static Font CreateFont(int size, FontWeight weight, bool italic)
        {
            return CreateFont(DefaultFontName, size, weight, italic);
        }

        public static void Clear()
        {
            if (fontCache != null && fontCache.Count > 0)
                {
                    foreach (Font font in fontCache.Values)
                    {
                        font.Dispose();
                    }
                    fontCache.Clear(); 
                }
        }
    }
}