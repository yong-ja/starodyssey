using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AvengersUtd.Odyssey.UserInterface.Text;
using AvengersUtd.Odyssey.UserInterface.Style;
using System.Drawing;

namespace AvengersUtd.Odyssey.UserInterface.Controls
{
    public class LoggerPanel :Panel
    {
        const int minimumLines = 4;
        const string ControlTag = "LoggerPanel";
        const int lineMargin = 4;
        const int rowHeight = 20;
        private static int count;

        private int lines;
        List<string> logLines;
        Label[] labels;

        public int Lines
        {
            get { return lines; }
            set
            {
                if (logLines.Count == value)
                    return;
                else
                {
                    lines = value;
                   RebuildLabels();
                }
            }
        }

        public LoggerPanel()
            : base(ControlTag + ++count, "LoggerPanel")
        {
            lines = -1;
            logLines = new List<string>();
        }

        void RebuildLabels()
        {
            labels = BuildLabels(lines, Description.Padding, lineMargin, rowHeight, TextDescriptionClass,
                DesignMode, new Size(ContentAreaSize.Width, 0));
            if (!Controls.IsEmpty)
                Controls.Clear();
            AddRange(labels);
        
        }

        public void EnqueueMessage(string message)
        {
            if (logLines.Count == lines)
                logLines.RemoveAt(0);
            logLines.Add(message);

            for (int i = 0; i < logLines.Count; i++)
            {
                labels[i].Content = logLines[i];
                labels[i].IsVisible = true;
            }
            if (!DesignMode)
                CheckPositions();
        }

        void CheckPositions()
        {
            float height = Description.Padding.Top;
            float lineHeight = labels[0].TextSize.Height;

            for (int i = 0; i < logLines.Count; i++)
            {
                labels[i].Position = new SlimDX.Vector2(Description.Padding.Left, height);
                height += lineHeight * labels[i].Lines;
            }
        }

        static Label[] BuildLabels(int lines, Thickness padding, int margin, int rowHeight, string tClass, bool designMode, Size size)
        {
            Label[] labels = new Label[lines] ;

            for (int i = 0; i < lines; i++)
                labels[i] = new Label
                {
                    Id = string.Format("{0}_{1}{2}", ControlTag, TextLiteral.ControlTag, i),
                    Position = new SlimDX.Vector2(padding.Left, padding.Top + (i * rowHeight)),
                    TextDescriptionClass = tClass,
                    DesignMode = designMode,
                    IsVisible = false,
                    Wrapping = true,
                    Size = size
                };
                

            return labels;
        }


     
    }
}
