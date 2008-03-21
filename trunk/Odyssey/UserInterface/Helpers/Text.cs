#region Disclaimer

/* 
 * Text
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
 * provided that you do not use the results in any 
 * commercial project without the prior express and 
 * written consent of the Author.
 *
 */

#endregion

using System.Text;

namespace AvengersUtd.Odyssey.UserInterface.Helpers
{
    public static class Text
    {
        public const string DefaultWhitespace = " \n\t\r";

        public static string MakePasswordString(string input, char passwordChar)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < input.Length; i++)
            {
                sb.Append(passwordChar);
            }

            return sb.ToString();
        }

        public static int IndexNotOf(string text, string delimiters, int startIndex)
        {
            int index = startIndex;

            while (index < text.Length)
            {
                if (delimiters.IndexOf(text[index]) == -1)
                {
                    return index;
                }

                index++;
            }
            return -1;
        }

        public static string GetNextWord(string text, int startIndex, string delimeters)
        {
            int wordStart = IndexNotOf(text, delimeters, startIndex);

            if (wordStart == -1)
            {
                wordStart = startIndex;
            }

            int wordEnd = text.IndexOfAny(delimeters.ToCharArray(), wordStart);

            if (wordEnd == -1)
            {
                wordEnd = text.Length;
            }

            return text.Substring(startIndex, (wordEnd - startIndex));
        }
    }
}