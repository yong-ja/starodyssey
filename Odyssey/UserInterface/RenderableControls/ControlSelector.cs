using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using AvengersUtd.Odyssey.UserInterface.Style;
using System.Drawing;
using SlimDX;

namespace AvengersUtd.Odyssey.UserInterface.RenderableControls
{
    

    public class ControlSelector : BaseControl
    {
        const string ControlTag = "ControlSelector";
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

        //ShapeDescriptor handleNW;
        //ShapeDescriptor handleN;
        //ShapeDescriptor handleNE;
        //ShapeDescriptor handleE;
        //ShapeDescriptor handleSE;
        //ShapeDescriptor handleS;
        //ShapeDescriptor handleSW;
        //ShapeDescriptor handleW;
        
        

       

        public ControlSelector()
        {
            ApplyControlStyle(StyleManager.GetControlStyle(ControlTag));
            handleSize = new Size(12, 12);
            sensibleArea = new Size(handleSize.Width, handleSize.Height);
            ShapeDescriptors = new ShapeDescriptorCollection(8);


            //ShapeDescriptors[0] = handleNW;
            //ShapeDescriptors[1] = handleN;
            //ShapeDescriptors[2] = handleNE;
            //ShapeDescriptors[3] = handleE;
            //ShapeDescriptors[4] = handleSE;
            //ShapeDescriptors[5] = handleS;
            //ShapeDescriptors[6] = handleSW;
            //ShapeDescriptors[7] = handleW;

        }

        public override bool IntersectTest(Point cursorLocation)
        {
            return hitmanager.Intersect(cursorLocation);
        }

        public override void CreateShape()
        {
            Depth = new Depth(0, 0, 998);
            bounds = ComputeBounds(TargetControl);
            for (int i=0; i < 8; i++)
            {
                Vector2 point = bounds[i];
                Vector2 topLeftCorner = new Vector2(point.X - handleSize.Width/2f, point.Y - handleSize.Height/2f);
                ShapeDescriptors[i] = Shapes.DrawFullRectangle(topLeftCorner, handleSize,
                                                               ControlStyle.ColorArray[ColorIndex.Enabled],
                                                               ControlStyle.ColorArray[ColorIndex.BorderEnabled],
                                                               ControlStyle.Shading,
                                                               ControlStyle.BorderSize,
                                                               ControlStyle.BorderStyle);
                ShapeDescriptors[i].Depth = Depth;
            }
        }

        public override void UpdateShape()
        {
            bounds = ComputeBounds(TargetControl);
            for (int i = 0; i < 8; i++)
            {
                Vector2 point = bounds[i];
                Vector2 topLeftCorner = new Vector2(point.X - handleSize.Width / 2f, point.Y - handleSize.Height / 2f);
                ShapeDescriptors[i].UpdateShape(Shapes.DrawFullRectangle(topLeftCorner, handleSize,
                                                               ControlStyle.ColorArray[ColorIndex.Enabled],
                                                               ControlStyle.ColorArray[ColorIndex.BorderEnabled],
                                                               ControlStyle.Shading,
                                                               ControlStyle.BorderSize,
                                                               ControlStyle.BorderStyle));
               // ShapeDescriptors[i].Depth = Depth;
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
                    return Cursors.SizeNWSE;

                case IntersectionLocation.Top:
                    return Cursors.SizeNS;

                case IntersectionLocation.CornerNE:
                    return Cursors.SizeNESW;
                
                case IntersectionLocation.Right:
                    return Cursors.SizeWE;

                case IntersectionLocation.CornerSE:
                    return Cursors.SizeNWSE;
                
                case IntersectionLocation.Bottom:
                    return Cursors.SizeNS;

                case IntersectionLocation.CornerSW:
                    return Cursors.SizeNESW;

                case IntersectionLocation.Left:
                    return Cursors.SizeWE;

                default:
                    return Cursors.No;
            }
        }

        protected override void UpdatePositionDependantParameters()
        {
            return;
        }

        protected override void OnMouseDown(System.Windows.Forms.MouseEventArgs e)
        {
            base.OnMouseDown(e);
            OdysseyUI.CurrentHud.CaptureControl = this;
            //if (previousIntersection != IntersectionLocation.None)
            //{
                drag = true;
                dragStartPosition = new Vector2(e.X, e.Y);
                
            //}
        }

        protected override void OnMouseMove(System.Windows.Forms.MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (drag)
            {
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
                        break;

                    case IntersectionLocation.Top:
                        targetControl.Position = new Vector2(targetControl.Position.X, targetControl.Position.Y + (int)delta.Y);
                        targetControl.Size = new Size(targetControl.Size.Width, targetControl.Size.Height - (int)delta.Y);
                        break;

                    case IntersectionLocation.CornerNE:
                        targetControl.Position = new Vector2(targetControl.Position.X, targetControl.Position.Y + (int)delta.Y);
                        targetControl.Size = new Size(targetControl.Size.Width + (int)delta.X, targetControl.Size.Height - (int)delta.Y);
                        break;

                    case IntersectionLocation.Right:
                        targetControl.Size = new Size(targetControl.Size.Width + (int)delta.X, targetControl.Size.Height);
                        break;

                    case IntersectionLocation.CornerSE:
                        targetControl.Size = new Size(targetControl.Size.Width + (int)delta.X, targetControl.Size.Height + (int)delta.Y);
                        break;

                    case IntersectionLocation.Bottom:
                        targetControl.Size = new Size(targetControl.Size.Width, targetControl.Size.Height + (int)delta.Y);
                        break;

                    case IntersectionLocation.CornerSW:
                        targetControl.Position = new Vector2(targetControl.Position.X + (int)delta.X,
                                                             targetControl.Position.Y);
                        targetControl.Size = new Size(targetControl.Size.Width - (int)delta.X, targetControl.Size.Height + (int)delta.Y);
                        break;


                    case IntersectionLocation.Left:
                        targetControl.Position = new Vector2(targetControl.Position.X + (int) delta.X,
                                                             targetControl.Position.Y);
                        targetControl.Size = new Size(targetControl.Size.Width - (int)delta.X, targetControl.Size.Height);
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }

                if (Options.SnapToGrid)
                {


                    targetControl.Position = SelectionRectangle.SnapPositionToGrid(targetControl.Position);
                    //delta = targetControl.Position - prevPosition;

                    targetControl.Size = SelectionRectangle.SnapSizeToGrid(targetControl.Size, delta.X > 0, true);

                    if (targetControl.Size != prevSize || targetControl.Position != prevPosition)
                        dragStartPosition = currentPosition;

                    //OdysseyUI.CurrentHud.EndDesign();
                }
                else
                {
                    dragStartPosition = currentPosition;
                }
                
                hitmanager.ComputeExtents();
            }
        }

        protected override void OnMouseUp(System.Windows.Forms.MouseEventArgs e)
        {
            base.OnMouseUp(e);
            OdysseyUI.CurrentHud.CaptureControl = null;
            drag = false;
            if (Options.SnapToGrid)
            {
                Vector2 prevPosition = targetControl.Position;
                targetControl.Position = SelectionRectangle.SnapPositionToGrid(prevPosition);
                //delta = targetControl.Position - prevPosition;
                targetControl.Size = SelectionRectangle.SnapSizeToGrid(targetControl.Size, true, true);

                //OdysseyUI.CurrentHud.EndDesign();
            }
            UpdateAppearance();
        }

    }
}
