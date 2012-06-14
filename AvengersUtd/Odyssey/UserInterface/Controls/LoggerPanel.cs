using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AvengersUtd.Odyssey.UserInterface.Text;
using AvengersUtd.Odyssey.UserInterface.Style;
using System.Drawing;
using AvengersUtd.Odyssey.Graphics.Meshes;

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

        public LoggerPanel()
            : base(ControlTag + ++count, "LoggerPanel")
        {
            rowHeight = (int)Math.Ceiling(TextManager.MeasureSize("Test", TextDescription.ToFont()).Height);
            logLines = new List<string>();
            labels = new List<Label>();
        }

        protected internal override void OnControlAdded(ControlEventArgs e)
        {
            base.OnControlAdded(e);
            lines = (int)Math.Floor(ContentAreaSize.Height / (float)rowHeight);
        }


        public void EnqueueMessage(string message)
        {
            bool alreadyInDesignMode = OdysseyUI.CurrentHud.DesignMode;
            if (!alreadyInDesignMode)
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

            if (!alreadyInDesignMode)
                OdysseyUI.CurrentHud.EndDesign();

        }

        void CheckOverflow()
        {
            int totalLines = (from l in labels
                                  select l.Lines).Sum();

            if (totalLines <= lines)
                return;

            while (totalLines > lines)
            {
                Label label = labels[0];
                label.Dispose();
                labels.Remove(label);
                Remove(label);
                totalLines = (from l in labels select l.Lines).Sum();
            }
        }

        void CheckPositions()
        {
            float height = Description.Padding.Top;

            foreach (Label label in labels)
            {
                label.Position = new SlimDX.Vector2(Description.Padding.Left, height);
                if (label.TextLiteral != null)
                    label.TextLiteral.Inited = false;
              
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
