using System;
using System.Windows.Forms;
using AvengersUtd.Odyssey.UserInterface.Style;
using System.Drawing;
using SlimDX;

namespace AvengersUtd.Odyssey.UserInterface.Controls
{
    
    public class ControlSelector : BaseControl
    {
        const string ControlTag = "ControlSelector";
        private static int count;
        bool drag;
        readonly Size handleSize, sensibleArea;
        Vector2[] bounds;
        Vector2 dragStartPosition;
        private Vector2 initialPosition;
        private bool overFlow;
        int width;
        int height;

        private Size initialSize;
        BaseControl targetControl;
        
        HitManager hitmanager;
        IntersectionLocation previousIntersection;

        public BaseControl TargetControl
        {
            get
            {
                return targetControl;
            }
            set
            {
                
                targetControl = value;
                if (targetControl == null) return;
                hitmanager = new HitManager(targetControl, sensibleArea);
                hitmanager.ComputeExtents();
                Position = targetControl.Position;
                Size = targetControl.Size;
            }
        }

        public Grid OwnerGrid { get; set; }

        public ControlSelector():base(ControlTag + ++count, ControlTag)
        {
            ApplyControlDescription(StyleManager.GetControlDescription(ControlTag));
            ApplyStatusChanges = false;
            handleSize = new Size(12, 12);
            sensibleArea = new Size(handleSize.Width, handleSize.Height);
            Shapes = new ShapeCollection(8);
        }

        public override bool IntersectTest(Point cursorLocation)
        {
            return hitmanager.Intersect(cursorLocation);
        }

        public override void CreateShape()
        {
            Depth = Depth.Topmost;
            bounds = ComputeBounds(TargetControl ?? this);
            if (TargetControl!=null)
                hitmanager.ComputeExtents();

            for (int i=0; i < 8; i++)
            {
                Vector2 point = bounds[i];
                Vector2 topLeftCorner = new Vector2(point.X - handleSize.Width/2f, point.Y - handleSize.Height/2f);
                Vector3 topLeftCornerOrtho = OrthographicTransform(topLeftCorner, Depth.ZOrder);
                Shapes[i] = ShapeCreator.DrawFullRectangle(topLeftCornerOrtho, handleSize,
                    Description.ColorShader,
                    Description.ColorArray[ColorIndex.Enabled],
                    Description.BorderSize,
                    Description.BorderStyle,
                    Description.ColorArray[ColorIndex.BorderEnabled]);

                Shapes[i].Depth = Depth;
            }
        }

        public override void UpdateShape()
        {
            for (int i = 0; i < 8; i++)
            {
                //if (!Shapes[i].IsDirty)
                //    return;

                Vector2 point = bounds[i];
                Vector2 topLeftCorner = new Vector2(point.X - handleSize.Width / 2f, point.Y - handleSize.Height / 2f);
                Vector3 topLeftCornerOrtho = OrthographicTransform(topLeftCorner, Depth.ZOrder);
                Shapes[i].UpdateVertices(ShapeCreator.DrawFullRectangle(topLeftCornerOrtho, handleSize,
                    Description.ColorShader,
                    Description.ColorArray[ColorIndex.Enabled],
                    Description.BorderSize,
                    Description.BorderStyle,
                    Description.ColorArray[ColorIndex.BorderEnabled]).Vertices);
                Shapes[i].Depth = Depth;
            }
            //System.Diagnostics.Debug.WriteLine("C:{0} T:{1}", Position, bounds[0]);
        }


        public Cursor GetCursorFor(Point point)
        {
            if (OdysseyUI.CurrentHud.CaptureControl == null)
                previousIntersection = hitmanager.FindIntersection(point);

            switch (previousIntersection)
            {
                case IntersectionLocation.None:
                    return Cursors.Arrow;

                case IntersectionLocation.Inner:
                    return Cursors.SizeAll;

                case IntersectionLocation.CornerNW:
                case IntersectionLocation.CornerSE:
                    return Cursors.SizeNWSE;

                case IntersectionLocation.Top:
                case IntersectionLocation.Bottom:
                    return Cursors.SizeNS;

                case IntersectionLocation.CornerNE:
                case IntersectionLocation.CornerSW:
                    return Cursors.SizeNESW;
                
                case IntersectionLocation.Right:
                case IntersectionLocation.Left:
                    return Cursors.SizeWE;
                    
                default:
                    return Cursors.No;
            }
        }

        protected override void UpdatePositionDependantParameters()
        {
            hitmanager.ComputeExtents();
            bounds = ComputeBounds(TargetControl);
            UpdateShape();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            IsVisible = false;

            OdysseyUI.CurrentHud.CaptureControl = this;
            //if (previousIntersection != IntersectionLocation.None)
            //{
                drag = true;
                dragStartPosition = new Vector2(e.X, e.Y);
            initialSize = targetControl.Size;
            initialPosition = targetControl.Position;
            //width = initialSize.Width;
            //height = initialSize.Height;

            //}
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (!drag) return;

            const int minimumSize = 10;

            Vector2 prevPosition = targetControl.Position;
            Vector2 newPosition;
            Size prevSize = targetControl.Size;
            Size newSize;
            Vector2 currentPosition = new Vector2(e.X, e.Y);
            Vector2 delta = currentPosition - dragStartPosition;

            switch (previousIntersection)
            {
                case IntersectionLocation.None:
                    break;

                case IntersectionLocation.Inner:
                    targetControl.Position += delta;
                    Position += delta;
                    break;

                case IntersectionLocation.CornerNW:
                    height = (int)((initialPosition.Y - currentPosition.Y + initialSize.Height));
                    width = (int)(initialPosition.X - currentPosition.X + initialSize.Width);

                    newSize = new Size(width, height);
                    newPosition = new Vector2(currentPosition.X, currentPosition.Y);

                    if (newSize.Width <= minimumSize || currentPosition.X >= initialPosition.X + initialSize.Width - minimumSize)
                    {
                        newSize.Width = minimumSize;
                        newPosition.X = initialPosition.X + initialSize.Width - minimumSize;
                    }

                    if (newSize.Height <= minimumSize || currentPosition.Y >= initialPosition.Y + initialSize.Height - minimumSize)
                    {
                        newSize.Height = minimumSize;
                        newPosition.Y = initialPosition.Y + initialSize.Height - minimumSize;
                    }

                    targetControl.Position = newPosition;
                    targetControl.Size = newSize;

                    break;

                    case IntersectionLocation.Top:
                    height = (int)((initialPosition.Y - currentPosition.Y + initialSize.Height));
                    
                    newSize = new Size(prevSize.Width, height);
                    newPosition = new Vector2(prevPosition.X, currentPosition.Y);

                    if (newSize.Height <= minimumSize || currentPosition.Y >= initialPosition.Y + initialSize.Height - minimumSize)
                    {
                        newSize.Height = minimumSize;
                        newPosition.Y = initialPosition.Y + initialSize.Height - minimumSize;
                    }

                    targetControl.Position = newPosition;
                    targetControl.Size = newSize;
                    
                    //System.Diagnostics.Debug.WriteLine("{1}) H:{0} Y:{2} P:{3}", newSize.Height, t++, targetControl.Position.Y, targetControl.Position.Y + targetControl.Size.Height);
                    break;

                case IntersectionLocation.Bottom:
                    height = (int) (currentPosition.Y - initialPosition.Y);
                    newSize = new Size(prevSize.Width, height);

                    if (newSize.Height <= minimumSize || currentPosition.Y <= initialPosition.Y + minimumSize)
                    {
                        newSize.Height = minimumSize;
                    }
                    targetControl.Size = newSize;
                    break;

                case IntersectionLocation.CornerNE:
                    height = (int)((initialPosition.Y - currentPosition.Y + initialSize.Height));
                    width = (int)(currentPosition.X - initialPosition.X);

                    newSize = new Size(width, height);
                    newPosition = new Vector2(prevPosition.X, currentPosition.Y);

                    if (newSize.Height <= minimumSize || currentPosition.Y >= initialPosition.Y + initialSize.Height - minimumSize)
                    {
                        newSize.Height = minimumSize;
                        newPosition.Y = initialPosition.Y + initialSize.Height - minimumSize;
                    }

                    if (newSize.Width <= minimumSize || currentPosition.X <= initialPosition.X + minimumSize)
                        newSize.Width = minimumSize;
                    
                    targetControl.Position = newPosition;
                    targetControl.Size = newSize;
                    break;

                case IntersectionLocation.Right:
                    width = (int)(currentPosition.X - initialPosition.X);
                    newSize = new Size(width, prevSize.Height);

                    if (newSize.Width <= minimumSize || currentPosition.X <= initialPosition.X + minimumSize)
                        newSize.Width = minimumSize;

                    targetControl.Size = newSize;
                    //width = targetControl.ClientSize.Width;
                    break;

                case IntersectionLocation.CornerSE:
                    width = (int)(currentPosition.X - initialPosition.X);
                    height = (int) (currentPosition.Y - initialPosition.Y);
                    newSize = new Size(width, height);

                    if (newSize.Height <= minimumSize || currentPosition.Y <= initialPosition.Y + minimumSize)
                    {
                        newSize.Height = minimumSize;
                    }
                    if (newSize.Width <= minimumSize || currentPosition.X <= initialPosition.X + minimumSize)
                        newSize.Width = minimumSize;

                    targetControl.Size = newSize;

                    break;

                case IntersectionLocation.CornerSW:
                    width = (int)(initialPosition.X - currentPosition.X  +initialSize.Width);
                    height = (int) (currentPosition.Y - initialPosition.Y);
                    newSize = new Size(width, height);

                    newPosition = new Vector2(currentPosition.X, prevPosition.Y);
                    if (newSize.Width <= minimumSize || currentPosition.X >= initialPosition.X + initialSize.Width - minimumSize)
                    {
                        newSize.Width = minimumSize;
                        newPosition.X = initialPosition.X + initialSize.Width - minimumSize;
                    }
                    if (newSize.Height <= minimumSize || currentPosition.Y <= initialPosition.Y + minimumSize)
                    {
                        newSize.Height = minimumSize;
                    }

                    targetControl.Position = newPosition;
                    targetControl.Size = newSize;
                    break;


                case IntersectionLocation.Left:
                    width = (int)(initialPosition.X - currentPosition.X  +initialSize.Width);
                    newSize = new Size(width, prevSize.Height);
                    newPosition = new Vector2(currentPosition.X, prevPosition.Y);

                    if (newSize.Width <= minimumSize || currentPosition.X >= initialPosition.X + initialSize.Width - minimumSize)
                    {
                        newSize.Width = minimumSize;
                        newPosition.X = initialPosition.X + initialSize.Width - minimumSize;
                    }

                    targetControl.Position = newPosition;
                    targetControl.Size = newSize;
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
            {
                dragStartPosition = currentPosition;
            }
            
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (OwnerGrid.SnapToGrid)
            {
                targetControl.Position = SelectionRectangle.SnapPositionToGrid(targetControl.Position,
                    OwnerGrid.GridSpacing);
                targetControl.Size = SelectionRectangle.SnapSizeToGrid(targetControl.Size, OwnerGrid.GridSpacing);
            }

            base.OnMouseUp(e);
            UpdatePositionDependantParameters();
            
            OdysseyUI.CurrentHud.CaptureControl = null;
            drag = false;
            IsVisible = true;
           
        }

        protected override void UpdateSizeDependantParameters()
        {
            return;
        }
    }
}
