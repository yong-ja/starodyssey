#region Disclaimer

/* 
 * RichTextArea
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
using AvengersUtd.Odyssey.UserInterface.Style;

namespace AvengersUtd.Odyssey.UserInterface.RenderableControls
{
    public class RichTextArea : ContainerControl
    {
        public const string ControlTag = "RichTextArea";
        const int DefaultLabelPaddingX = 8;
        const int DefaultLabelPaddingY = 8;
        static int count = 1;
        string markupText;

        LabelFormatter labelFormatter;
        LabelPage page;
        int rowCount;

        public RichTextArea()
        {
            ApplyControlStyle(StyleManager.GetControlStyle(GetType().Name));
        }


        public string MarkupText
        {
            get { return markupText; }
            set
            {
                if (markupText != value)
                {
                    markupText = value;
                    if (!Size.IsEmpty)
                        FormatText();
                }
            }
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            if (string.IsNullOrEmpty(markupText))
                return;

            FormatText();
        }

        void FormatText()
        {
            labelFormatter = new LabelFormatter(ControlTag, TextStyle, TopLeftPosition, ClientSize);
            page = labelFormatter.FormatMarkupCode(markupText);
            Controls.Clear();
            foreach (Label l in page.LabelCollection)
            {
                if (l != null)
                    Add(l);
            }

            page.Align();
        }
    }
}