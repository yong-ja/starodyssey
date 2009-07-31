#region Disclaimer

/* 
 * LabelFormatter
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
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using AvengersUtd.Odyssey.UserInterface.Helpers;
using AvengersUtd.Odyssey.UserInterface;
#if !(SlimDX)
    using Microsoft.DirectX;
#else
using SlimDX;
#endif

namespace AvengersUtd.Odyssey.UserInterface.Style
{

    #region Structs

    internal struct CommandInfo
    {
        string command;
        string value;

        public CommandInfo(string command, string value)
        {
            this.command = command;
            this.value = value;
        }


        public string Command
        {
            get { return command; }
        }

        public string Value
        {
            get { return value; }
        }
    }

    internal struct LabelInfo
    {
        Vector2 insertionPoint;
        int linePosition;
        TextStyle style;
        string text;

        public LabelInfo(string text, int linePosition, Vector2 insertionPoint, TextStyle style)
        {
            this.insertionPoint = insertionPoint;
            this.style = style;
            this.text = text;
            this.linePosition = linePosition;
        }

        public int LinePosition
        {
            get { return linePosition; }
        }


        public Vector2 InsertionPoint
        {
            get { return insertionPoint; }
        }

        public TextStyle Style
        {
            get { return style; }
        }

        public string Text
        {
            get { return text; }
        }
    }

    #endregion

    public class LabelFormatter
    {
        string baseTag;
        int currentLine;
        TextStyle currentStyle;
        Vector2 cursor;
        TextStyle defaultStyle;
        List<Label> labelCollection;

        int labelIndex;
        List<LabelInfo> labelInfoCollection;
        LabelPage page;
        Size size;
        Vector2 startCursor;

        public LabelFormatter(string baseTag, TextStyle defaultStyle, Vector2 startCursor, Size size)
        {
            this.baseTag = baseTag;
            this.defaultStyle = defaultStyle;
            this.startCursor = cursor = startCursor;
            this.size = size;

            labelInfoCollection = new List<LabelInfo>();
            labelCollection = new List<Label>();
            page = new LabelPage();
        }

        public LabelPage FormatMarkupCode(string code)
        {
            CreateLabels(ParseLabelMarkup(code));
            return page;
        }

        List<CommandInfo> ParseLabelMarkup(string value)
        {
            char[] charArray = value.ToCharArray();
            string markupText, labelText;
            markupText = labelText = string.Empty;
            List<CommandInfo> commandInfoList = new List<CommandInfo>();

            bool markup = false;


            for (int i = 0; i < charArray.Length; i++)
            {
                char c = charArray[i];
                if (markup)
                {
                    if (c == ']')
                        markup = false;
                    else
                        markupText += c;
                }
                else
                {
                    if (c == '[')
                    {
                        if (!String.IsNullOrEmpty(labelText))
                            commandInfoList.Add(new CommandInfo(markupText, labelText));
                        markupText = labelText = string.Empty;

                        if (charArray[i + 1] == '/')
                        {
                            i += 2;
                            continue;
                        }

                        markup = true;
                    }
                    else
                        labelText += c;
                }
            }
            if (!string.IsNullOrEmpty(labelText))
                commandInfoList.Add(new CommandInfo(markupText, labelText));

            return commandInfoList;
        }

        void CheckForCarriageReturn(string text)
        {
            int index = text.IndexOf('\n');
            if (index != -1)
            {
                List<string> lines = new List<string>();

                while ((index = text.IndexOf('\n')) != -1)
                {
                    if (index == 0)
                    {
                        lines.Add(string.Empty);
                        text = text.Remove(0, 1);
                    }
                    else
                    {
                        lines.Add(text.Substring(0, index));
                        text = text.Substring(index + 1);
                    }
                }

                lines.Add(text);

                page.InsertLine();
                for (int i = 0; i < lines.Count - 1; i++)
                {
                    string s = lines[i];
                    if (String.IsNullOrEmpty(s))
                        GotoNextLine();
                    else
                    {
                        CheckForWordWrap(s);
                        GotoNextLine();
                    }
                }
                CheckForWordWrap(lines[lines.Count - 1]);
            }
            else
            {
                page.InsertLine();
                CheckForWordWrap(text);
            }
        }

        void GotoNextLine()
        {
            cursor = new Vector2(startCursor.X, cursor.Y + currentStyle.Size);
            currentLine++;
            page.InsertLine();
        }


        void CheckForWordWrap(string text)
        {
            string whitespace = Text.DefaultWhitespace;
            string thisLine, thisWord;
            int currentPos = 0;

            // get the first word of the string
            thisLine = Text.GetNextWord(text, currentPos, whitespace);
            currentPos += thisLine.Length;

            while (Text.IndexNotOf(text, whitespace, currentPos) != -1)
            {
                // get the next word of the string
                thisWord = Text.GetNextWord(text, currentPos, whitespace);
                currentPos += thisWord.Length;

                if (ComputeLineLength(cursor.X, Label.MeasureText(thisLine + thisWord, currentStyle).Width) >= size.Width)
                //if ((Label.MeasureText(thisLine, currentStyle).Width) > Size.Width)
                {
                    labelInfoCollection.Add(new LabelInfo(thisLine, currentLine, cursor, currentStyle));

                    // remove whitespace from next word - it will form start of next line
                    thisWord = thisWord.Substring(Text.IndexNotOf(thisWord, whitespace, 0));

                    // reset for a new line
                    thisLine = string.Empty;

                    // update y coordinate for the next line
                    GotoNextLine();
                }
                thisLine += thisWord;
            }
          

            if (!string.IsNullOrEmpty(thisLine))
            {
                if (cursor.X + Label.MeasureText(thisLine, currentStyle).Width >= size.Width)
                {
                    GotoNextLine();
                    CheckForWordWrap(thisLine);
                }
                else
                {
                    labelInfoCollection.Add(new LabelInfo(thisLine, currentLine, cursor, currentStyle));
                    cursor.X += Label.MeasureText(thisLine, currentStyle).Width;
                }
            }
        }

        void DebugCmd(NameValueCollection commandList)
        {
            for (int i = 0; i < commandList.Count; i++)
            {
                string markup = commandList.GetKey(i);
                string labelText = commandList[i];
                System.Diagnostics.Debug.WriteLine(markup + ' ' + labelText);
            }
        }

        void Process(List<CommandInfo> commandInfoList)
        {
            for (int i = 0; i < commandInfoList.Count; i++)
            {
                string markup = commandInfoList[i].Command;
                string labelText = commandInfoList[i].Value;
                int prevLength = labelText.Length;
                labelText = labelText.TrimEnd(null);
                int whiteSpaces = prevLength - labelText.Length;

                if (String.IsNullOrEmpty(markup))
                    currentStyle = defaultStyle;
                else
                    currentStyle = defaultStyle.ApplyMarkup(markup);

                CheckForCarriageReturn(labelText);
                //CheckForWordWrap(labelText);

                // See if any whitespaces are to be added
                if (whiteSpaces > 0)
                    cursor.X += whiteSpaces*(currentStyle.Size/5);
            }
        }

        void CreateLabels(List<CommandInfo> commandInfoList)
        {
            Process(commandInfoList);
            foreach (LabelInfo labelInfo in labelInfoCollection)
            {
                Label label = new Label();
                label.Id = string.Format("{0}_{1}", baseTag, labelIndex);
                label.Text = labelInfo.Text;
                label.Position = labelInfo.InsertionPoint;
                label.IsSubComponent = true;
                label.TextStyle = labelInfo.Style;

                page.AppendAt(labelInfo.LinePosition, label);

                labelIndex++;
            }
        }


        void Debug()
        {
            foreach (Label l in labelCollection)
            {
                Console.WriteLine(l.Text + " - " + l.Id);
            }
        }

        float ComputeLineLength(float insertionPoint, float additionLength)
        {
            return insertionPoint + additionLength - startCursor.X;
        }
    }
}