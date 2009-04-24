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

        public Vector2 Offset
        {
            get;
            set;
        }

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
            if (Options.SnapToGrid)
                selectionEnd = SnapPositionToGrid(selectionEnd);

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

            if (Options.SnapToGrid)
                selectionStart = SnapPositionToGrid(selectionStart);
            
            Position = selectionStart;
            IsVisible = true;
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

        public override void UpdateAppearance()
        {
            base.UpdateAppearance();
            Depth = new Depth(0, 0, 999);
        }

        public static Vector2 SnapPositionToGrid(Vector2 position)
        {
            int deltaX = (int)(position.X%Options.GridSpacing);
            int deltaY = (int)(position.Y%Options.GridSpacing);
            int newX, newY;
            if (deltaX > Options.GridSpacing / 2)
                newX = (int)position.X  + (Options.GridSpacing - deltaX);
            else
                newX = (int)position.X - deltaX;

            if (deltaY > Options.GridSpacing / 2)
                newY = (int)position.Y + (Options.GridSpacing - deltaY);
            else
                newY = (int)position.Y - deltaY;

            return new Vector2(newX, newY);
        }

        public static Size SnapSizeToGrid(Size size, bool positiveX, bool positiveY)
        {
            int deltaX = (size.Width % Options.GridSpacing);
            int deltaY = (size.Height % Options.GridSpacing);
            int newWidth=0, newHeight=0;

            if (positiveX)
            {
                //if (deltaX > Options.GridSpacing - Options.GridSpacing/4)
                //    newWidth = size.Width + (Options.GridSpacing - deltaX);
                //else
                    newWidth = size.Width - deltaX;
            }
            else
            {
                //if (deltaX < Options.GridSpacing / 4)
                //    newWidth = size.Width + (Options.GridSpacing - deltaX);
                //else
                    newWidth = size.Width + (Options.GridSpacing -deltaX);
            }

            if (deltaY > Options.GridSpacing)
                newHeight = size.Height + (Options.GridSpacing - deltaY);
            else
                newHeight = size.Height - deltaY;

            return new Size(newWidth, newHeight);
        }

    }
}
