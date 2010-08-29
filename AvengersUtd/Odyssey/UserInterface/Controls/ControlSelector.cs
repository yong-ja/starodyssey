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
                hitmanager = new HitManager(targetControl, sensibleArea);
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
                
            //}
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (!drag) return;

            bool rightSideX = false, rightSideY = false;
            bool canResizeX = false, canResizeY = false;
            bool canMoveX = false, canMoveY = false;
            bool recomputePosition = false;
            Vector2 prevPosition = targetControl.Position;
            Size prevSize = targetControl.Size;
            Vector2 currentPosition = new Vector2(e.X, e.Y);

                
            Vector2 delta = currentPosition-dragStartPosition  ;


            switch (previousIntersection)
            {
                case IntersectionLocation.None:
                    break;

                case IntersectionLocation.Inner:
                    targetControl.Position += delta;
                    Position += delta;
                    break;

                case IntersectionLocation.CornerNW:
                    targetControl.Position = new Vector2(targetControl.Position.X+ (int)delta.X,
                        targetControl.Position.Y + (int) delta.Y);
                    targetControl.Size = new Size(targetControl.Size.Width - (int)delta.X, targetControl.Size.Height - (int)delta.Y);
                    rightSideY = true;
                    canMoveX = true;
                    canMoveY = true;
                    canResizeY = true;
                    canResizeX = true;
                    break;

                case IntersectionLocation.Top:
                    targetControl.Position = new Vector2(targetControl.Position.X, targetControl.Position.Y + (int)delta.Y);
                    targetControl.Size = new Size(targetControl.Size.Width, targetControl.Size.Height - (int)delta.Y);
                    recomputePosition = true;
                    rightSideY = true;
                    canResizeY = true;
                    canMoveY = true;
                    break;

                case IntersectionLocation.CornerNE:
                    targetControl.Position = new Vector2(targetControl.Position.X, targetControl.Position.Y + (int)delta.Y);
                    targetControl.Size = new Size(targetControl.Size.Width + (int)delta.X, targetControl.Size.Height - (int)delta.Y);
                    rightSideX = true;
                    rightSideY = true;
                    canResizeX = true;
                    canResizeY = true;
                    canMoveY = true;
                    break;

                case IntersectionLocation.Right:
                    targetControl.Size = new Size(targetControl.Size.Width + (int)delta.X, targetControl.Size.Height);
                    rightSideX = true;
                    canResizeX = true;
                    break;

                case IntersectionLocation.CornerSE:
                    targetControl.Size = new Size(targetControl.Size.Width + (int)delta.X, targetControl.Size.Height + (int)delta.Y);
                    rightSideX = true;
                    canResizeX = true;
                    canResizeY = true;
                    break;

                case IntersectionLocation.Bottom:
                    targetControl.Size = new Size(targetControl.Size.Width, targetControl.Size.Height + (int)delta.Y);
                    canResizeY = true;
                    break;

                case IntersectionLocation.CornerSW:
                    targetControl.Position = new Vector2(targetControl.Position.X + (int)delta.X,
                        targetControl.Position.Y);
                    targetControl.Size = new Size(targetControl.Size.Width - (int)delta.X, targetControl.Size.Height + (int)delta.Y);
                    recomputePosition = true;
                    canMoveX = true;
                    canResizeX = true;
                    canResizeY = true;
                    break;


                case IntersectionLocation.Left:
                    targetControl.Position = new Vector2(targetControl.Position.X + (int) delta.X,
                        targetControl.Position.Y);
                    targetControl.Size = new Size(targetControl.Size.Width - (int)delta.X, targetControl.Size.Height);
                    recomputePosition = true;
                    canResizeX = true;
                    canMoveX = true;
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

                

            if (OwnerGrid.SnapToGrid)
            {
                bool doX = (delta.X > OwnerGrid.GridSpacing || delta.X < -OwnerGrid.GridSpacing) ? true : false;
                bool doY = (delta.Y > OwnerGrid.GridSpacing || delta.Y < -OwnerGrid.GridSpacing) ? true : false;
                bool positiveX = false;
                bool positiveY = false;

                positiveX = rightSideX ? delta.X > 0 : delta.X < 0;
                positiveY = rightSideY ? delta.Y > 0 : delta.Y < 0;


                if (previousIntersection == IntersectionLocation.Inner)
                {
                    targetControl.Position = SelectionRectangle.SnapPositionToGrid(targetControl.Position, OwnerGrid.GridSpacing);
                    //targetControl.Position = targetControl.Position = SelectionRectangle.SnapPositionToGrid(targetControl.Position);
                                                                                       
                    if (targetControl.Position != prevPosition)
                        dragStartPosition = currentPosition;
                        
                    Shapes.SetDirtyFlag(true);
                    UpdateAppearance();
                    //delta = targetControl.Position - prevPosition;
                }
                else
                {

                    if (doX || doY)
                    {
                        //DebugManager.LogToScreen(string.Format("X: {0:f0} Y: {1:f0}", delta.X, delta.Y));
                        Vector2 newPosition = SelectionRectangle.SnapPositionToGrid(targetControl.Position, prevPosition, delta, 
                            OwnerGrid.GridSpacing, doX & canMoveX, doY & canMoveY, positiveX, positiveY);

                        if (targetControl.Size.Height < 0 || targetControl.Size.Width < 0)
                            newPosition = prevPosition;

                        targetControl.Position = newPosition;
                        //targetControl.Position = SelectionRectangle.CheckFlip(prevPosition, newPosition,
                        //                                                      targetControl.Size);

                        targetControl.Size = SelectionRectangle.SnapSizeToGrid(targetControl.Size, prevSize,
                            OwnerGrid.GridSpacing,
                            positiveX, positiveY,
                            doX & canResizeX, doY & canResizeY);

                            

                        if (doX && !doY)
                            dragStartPosition = new Vector2(currentPosition.X, dragStartPosition.Y);
                        else if (!doX && doY)
                            dragStartPosition = new Vector2(dragStartPosition.X, currentPosition.Y);
                        else
                            dragStartPosition = currentPosition;

                        Shapes.SetDirtyFlag(true);
                        UpdateAppearance();

                    }
                    else
                    {
                        targetControl.Position = prevPosition;
                        targetControl.Size = prevSize;
                    }
                }
                    

                //UpdateAppearance();
            }
            else
            {
                dragStartPosition = currentPosition;
            }
                
            hitmanager.ComputeExtents();
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            OdysseyUI.CurrentHud.CaptureControl = null;
            drag = false;
            UpdatePositionDependantParameters();
            IsVisible = true;
           
        }

        protected override void UpdateSizeDependantParameters()
        {
            return;
        }
    }
}
