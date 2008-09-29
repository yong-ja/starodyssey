using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using AvengersUtd.Odyssey.UserInterface.Style;
using SlimDX;

namespace AvengersUtd.Odyssey.UserInterface.RenderableControls
{
    public class SelectionEventArgs : EventArgs
    {
        public Vector2 Position
        {
            get;
            private set;
        }
        public Size Size
        {
            get;
            private set;
        }

        public SelectionEventArgs(Vector2 position, Size size)
        {
            Position = position;
            Size = size;
        }
    }

    public class SelectionRectangle : SimpleShapeControl
    {
        const string ControlTag = "SelectorRectangle";

        bool isDragging;
        Vector2 selectionStart;
        Vector2 selectionEnd;

        readonly static object EventSelectionFinalized;

        public event EventHandler<SelectionEventArgs> SelectionFinalized
        {
            add { Events.AddHandler(EventSelectionFinalized, value); }
            remove { Events.RemoveHandler(EventSelectionFinalized, value); }
        }

        protected virtual void OnSelectionFinalized(object sender, SelectionEventArgs e)
        {
            EventHandler<SelectionEventArgs> handler =
                (EventHandler<SelectionEventArgs>)Events[EventSelectionFinalized];
            if (handler != null)
                handler(this, e);

        }
    
        static SelectionRectangle()
        {
            EventSelectionFinalized = new object();
        }

        public SelectionRectangle()
        {
            ApplyControlStyle(StyleManager.GetControlStyle(ControlTag));
            CanRaiseEvents = false;
        }

        void UpdateSelectionExtents(MouseEventArgs e)
        {
            selectionEnd = new Vector2(e.X, e.Y);
            Vector2 selectionDelta = selectionStart - selectionEnd;
            int width = (int)Math.Abs(selectionDelta.X);
            int height = (int)Math.Abs(selectionDelta.Y);

            Size = new Size(width, height);


            int finalStartX = (int)Math.Min(selectionStart.X, selectionEnd.X);
            int finalStartY = (int) Math.Min(selectionStart.Y, selectionEnd.Y);
            int finalEndX = (int)Math.Max(selectionStart.X, selectionEnd.X);
            int finalEndY = (int)Math.Max(selectionStart.Y, selectionEnd.Y);

            Position = new Vector2(finalStartX, finalStartY);
            selectionEnd = new Vector2(finalEndX, finalEndY);

        }
        public void StartSelection(object sender, MouseEventArgs e)
        {
            isDragging = true;
            selectionStart = new Vector2(e.X, e.Y);
            
            Position = selectionStart;
            Size = Size.Empty;
        }

        public void UpdateSelection(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                UpdateSelectionExtents(e);
            }
        }

        public void FinalizeSelection(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                UpdateSelectionExtents(e);
                isDragging = false;
                OnSelectionFinalized(this, new SelectionEventArgs(Position, Size));
            }
            
        }
    }
}
