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
        int rowHeight = 20;
        int labelCount;
        private static int count;

        private int lines;
        List<string> logLines;
        List<Label> labels;

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
                   ///RebuildLabels();
                }
            }
        }

        public LoggerPanel()
            : base(ControlTag + ++count, "LoggerPanel")
        {
            rowHeight = (int)Math.Ceiling(TextManager.MeasureSize("Test", TextDescription.ToFont()).Height);
            logLines = new List<string>();
            labels = new List<Label>();
        }

        void RebuildLabels()
        {
            lines = (int)Math.Floor(ContentAreaSize.Height / (float)rowHeight);
            labels.AddRange(BuildLabels(lines, Description.Padding, lineMargin, rowHeight, TextDescriptionClass,
                DesignMode, new Size(ContentAreaSize.Width, 0)));
            if (!Controls.IsEmpty)
                Controls.Clear();
            AddRange(labels);
        
        }

        public void EnqueueMessage(string message)
        {
            //logLines.Add(message);
OdysseyUI.CurrentHud.BeginDesign();
            Label newLabel = new Label
                {
                    Id = string.Format("{0}_{1}{2}", ControlTag, TextLiteral.ControlTag, ++labelCount),
                    //Position = new SlimDX.Vector2(Description.Padding.Left, Description.Padding.Top + (labels.Count * rowHeight)),
                    TextDescriptionClass = TextDescriptionClass,
                    //DesignMode = DesignMode,
                    //IsVisible = true,
                    Wrapping = true,
                    Size = new Size(ContentAreaSize.Width, 0),
                    Content = message
                };

            
            labels.Add(newLabel);
            Add(newLabel);
            CheckOverflow();
            CheckPositions();
            OdysseyUI.CurrentHud.EndDesign();

            

        }

        void ScrollLabelsUp()
        {
            for (int i = 0; i < logLines.Count; i++)
            {
                labels[i].Content = logLines[i];
                labels[i].IsVisible = true;
            }
        }

        void CheckOverflow()
        {
            int totalLines = (from l in labels
                                  select l.Lines).Sum();

            if (totalLines <= lines)
                return;

            while (totalLines > lines)
            {
                //logLines.RemoveAt(0);
                labels.RemoveAt(0);
                Controls.RemoveAt(0);
                totalLines = (from l in labels
                                  select l.Lines).Sum();
            }
        }

        void CheckPositions()
        {
            float height = Description.Padding.Top;

            foreach (Label label in labels)
            {
                label.Position = new SlimDX.Vector2(Description.Padding.Left, height);
                height += rowHeight * label.Lines;
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
