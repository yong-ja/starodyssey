using System;
using System.Drawing;
using AvengersUtd.Odyssey.UserInterface.Style;
using SlimDX;
using AvengersUtd.Odyssey.UserInterface.Input;

namespace AvengersUtd.Odyssey.UserInterface.Controls
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
        private static int count;

        bool isDragging;
        Vector2 selectionStart;
        Vector2 selectionEnd;

        public Vector2 Offset{get;set;}

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

        public Grid OwnerGrid { get; set; }
    
        static SelectionRectangle()
        {
            EventSelectionFinalized = new object();
        }

        public SelectionRectangle() : base(ControlTag + (++count), ControlTag)
        {
            CanRaiseEvents = false;
        }

        void UpdateSelectionExtents(MouseEventArgs e)
        {
            selectionEnd = e.Location;
            if (OwnerGrid.SnapToGrid)
                selectionEnd = SnapPositionToGrid(selectionEnd, OwnerGrid.GridSpacing);

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
            selectionStart = e.Location;

            if (OwnerGrid.SnapToGrid)
                selectionStart = SnapPositionToGrid(selectionStart, OwnerGrid.GridSpacing);
            
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
            if (!isDragging) return;

            UpdateSelectionExtents(e);
            isDragging = false;
            OnSelectionFinalized(this, new SelectionEventArgs(Position, Size));
        }

        public static Vector2 CheckFlip(Vector2 oldPosition, Vector2 newPosition, Size size)
        {
            return oldPosition.Y + size.Height == newPosition.Y ? oldPosition : newPosition;
        }

        public static Vector2 SnapPositionToGrid(Vector2 position, Vector2 prevPosition, Vector2 delta, int gridSpacing,
            bool doX, bool doY, bool positiveX, bool positiveY)
        {
           
            int newX = (int)prevPosition.X, newY = (int)prevPosition.Y;
            int currentX = (int)position.X, currentY = (int)position.Y;


            int deltaX = currentX % gridSpacing;
            int deltaY = currentY % gridSpacing;

            if (currentY < 0)
                positiveY = !positiveY;

            if (doX)
            {
                if (positiveX)
                    newX = currentX + (gridSpacing - deltaX);
                else
                    newX = currentX - deltaX;
            }

            if (doY)
            {
                if (!positiveY)
                    newY = currentY + (gridSpacing - deltaY);
                else
                    newY = currentY - deltaY;
            }

            
            return new Vector2(newX, newY);
        }


        public static Vector2 SnapPositionToGrid(Vector2 position, int gridSpacing)
        {
            int deltaX = (int)(position.X % gridSpacing);
            int deltaY = (int)(position.Y % gridSpacing);
            int newX, newY;
            if (deltaX > gridSpacing / 2)
                newX = (int)position.X + (gridSpacing - deltaX);
            else
                newX = (int)position.X - deltaX;

            if (deltaY > gridSpacing / 2)
                newY = (int)position.Y + (gridSpacing - deltaY);
            else
                newY = (int)position.Y - deltaY;

            return new Vector2(newX, newY);
        }

        public static Size SnapSizeToGrid(Size size, int gridSpacing)
        {
            int deltaX = (size.Width % gridSpacing);
            int deltaY = (size.Height % gridSpacing);
            int newWidth = size.Width, newHeight = size.Height;

            int threshold = (int)Math.Round(gridSpacing / 2f);

            if (deltaX > threshold)
            {
                newWidth = size.Width + (gridSpacing - deltaX);
            }
            else
            {
                newWidth = size.Width - deltaX;
            }

            if (deltaY > threshold)
            {
                newHeight = size.Height + (gridSpacing - deltaY);
            }
            else
                newHeight = size.Height - deltaY;

            if (newWidth == 0)
                newWidth = gridSpacing;
            if (newHeight == 0)
                newHeight = gridSpacing;

            return new Size(newWidth, newHeight);
        }

    }
}
