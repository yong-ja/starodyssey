using System;
using System.Collections;
using AvengersUtd.Odyssey.UserInterface.RenderableControls;
using AvengersUtd.Odyssey.UserInterface.Style;

#if (!SlimDX)
using Microsoft.DirectX.Direct3D;
using TransformedColored = Microsoft.DirectX.Direct3D.CustomVertex.TransformedColored;
#else

#endif

namespace AvengersUtd.Odyssey.UserInterface
{
    public struct Depth : IComparable<Depth>
    {
        int componentLayer;
        int windowLayer;
        int zOrder;

        #region Properties

        public int WindowLayer
        {
            get { return windowLayer; }
            set { componentLayer = value; }
        }

        public int ComponentLayer
        {
            get { return componentLayer; }
            set { componentLayer = value; }
        }

        public int ZOrder
        {
            get { return zOrder; }
            set { zOrder = value; }
        }

        #endregion

        /// <summary>
        /// Creates depth information for the control or ShapeDescriptor.
        /// </summary>
        /// <param name="window">The window layer. A value of 0 means the background. The highest value represents the currently focused window.</param>
        /// <param name="component">Inside a window there can be controls that have to overlap other ones (such as the expandable panel of the DropDownBox). Increase this value to render them correctly.</param>
        /// <param name="zOrder">A value of 0 means that the control is in the background of the window layer. An higher value means that the control has to be drawn on top of the other ones.</param>
        public Depth(int window, int component, int zOrder)
        {
            windowLayer = window;
            componentLayer = component;
            this.zOrder = zOrder;
        }

        #region IComparable<Depth> Members

        public int CompareTo(Depth other)
        {
            if (windowLayer > other.windowLayer)
                return +100;
            else if (windowLayer < other.windowLayer)
                return -100;
            else if (componentLayer > other.componentLayer)
                return +50;
            else if (componentLayer < other.componentLayer)
                return -50;
            else
                return (zOrder - other.zOrder);
        }

        #endregion

        /// <summary>
        /// Creates Depth information assuming that the current object will be the child of the control whose depth is passed as a parameter.
        /// </summary>
        /// <param name="parentDepth">The parent depth information.</param>
        public static Depth AsChildOf(Depth parentDepth)
        {
            return new Depth(parentDepth.windowLayer,
                             parentDepth.componentLayer,
                             parentDepth.zOrder + 1);
        }

        public void IncreaseComponentLayer()
        {
            componentLayer++;
        }

        public override string ToString()
        {
            return string.Format("{0}.{1}.{2}", windowLayer, componentLayer, zOrder);
        }

        public static bool operator ==(Depth depth1, Depth depth2)
        {
            if (depth1.Equals(depth2))
                return true;
            else return false;
        }

        public static bool operator !=(Depth depth1, Depth depth2)
        {
            return !(depth1 == depth2);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Depth)) return false;

            Depth otherDepth = (Depth) obj;
            if (windowLayer == otherDepth.windowLayer &&
                componentLayer == otherDepth.componentLayer &&
                zOrder == otherDepth.zOrder)
                return true;
            else
                return false;
        }

        public override int GetHashCode()
        {
            return windowLayer.GetHashCode() ^ componentLayer.GetHashCode() ^ zOrder.GetHashCode();
        }
    }

    public class ShapeDescriptorCollection : IEnumerable
    {
        ShapeDescriptor[] shapeDescriptors;

        public ShapeDescriptorCollection(int capacity)
        {
            shapeDescriptors = new ShapeDescriptor[capacity];
        }

        public ShapeDescriptor this[int index]
        {
            get { return shapeDescriptors[index]; }
            set { shapeDescriptors[index] = value; }
        }

        public int Length
        {
            get { return shapeDescriptors.Length; }
        }

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return shapeDescriptors.GetEnumerator();
        }

        #endregion

        public void Sort()
        {
            Array.Sort(shapeDescriptors);
        }

        internal ShapeDescriptor[] GetArray()
        {
            return shapeDescriptors;
        }
    }


    public class ShapeDescriptor : IComparable<ShapeDescriptor>
    {
        int arrayOffset;
        Depth depth;

        int[] indices;
        bool isDirty;
        int numPrimitives;
        TransformedColored[] vertices;

        #region Properties

        public int NumPrimitives
        {
            get { return numPrimitives; }
            set { numPrimitives = value; }
        }

        public bool IsDirty
        {
            get { return isDirty; }
            set { isDirty = value; }
        }

        public TransformedColored[] Vertices
        {
            get { return vertices; }
        }

        public Depth Depth
        {
            get { return depth; }
            set { depth = value; }
        }


        /// <summary>
        /// Returns or set the array offset in the descripted VertexBuffer object
        /// (ie which index the related control has in the vertexbuffer: useful
        /// when updating it
        /// </summary>
        public int ArrayOffset
        {
            get { return arrayOffset; }
            set { arrayOffset = value; }
        }

        public static ShapeDescriptor Empty
        {
            get { return new ShapeDescriptor(0, new TransformedColored[0], new int[0]); }
        }

        public int[] Indices
        {
            get { return indices; }
        }

        #endregion

        public ShapeDescriptor(int numPrimitives, TransformedColored[] vertices, int[] indices)
        {
            this.numPrimitives = numPrimitives;
            this.vertices = vertices;
            this.indices = indices;
        }

        #region IComparable<ShapeDescriptor> Members

        public int CompareTo(ShapeDescriptor other)
        {
            return depth.CompareTo(other.depth);
        }

        #endregion

        public static ShapeDescriptor Join(params ShapeDescriptor[] descriptors)
        {
            int vbTotal = 0;
            int ibTotal = 0;
            int numPrimitives = 0;

            int arrayOffset = descriptors[0].arrayOffset;

            TransformedColored[] vertices;
            int[] indices;

            for (int i = 0; i < descriptors.Length; i++)
            {
                ShapeDescriptor descriptor = descriptors[i];
                vbTotal += descriptor.Vertices.Length;
                ibTotal += descriptor.Indices.Length;
                numPrimitives += descriptor.numPrimitives;
            }

            vertices = new TransformedColored[vbTotal];
            indices = new int[ibTotal];

            int vbOffset = 0;
            int ibOffset = 0;

            for (int i = 0; i < descriptors.Length; i++)
            {
                ShapeDescriptor descriptor = descriptors[i];
                Array.Copy(descriptor.Vertices, 0, vertices, vbOffset, descriptor.Vertices.Length);

                for (int j = 0; j < descriptor.Indices.Length; j++)
                    indices[j + ibOffset] = descriptor.Indices[j] + vbOffset;

                vbOffset += descriptor.Vertices.Length;
                ibOffset += descriptor.Indices.Length;
            }

            ShapeDescriptor newSDesc = new ShapeDescriptor(numPrimitives, vertices, indices);
            newSDesc.arrayOffset = arrayOffset;
            return newSDesc;
        }

        /// <summary>
        /// Updates shape with the information provided by the new descriptor.
        /// <remarks>It must have the same number of vertices as the old one. Use this to
        /// change the position/color of the control. If the appearance must be changed
        /// have it already precomputed and set its color to invisible and bring it in the
        /// foreground when needed by changing the color</remarks>
        /// </summary>
        /// <param name="newShape"></param>
        public void UpdateShape(ShapeDescriptor newShape)
        {
            vertices = newShape.vertices;
            indices = newShape.Indices;
            numPrimitives = newShape.numPrimitives;
        }

        public static ShapeDescriptor ComputeShape(BaseControl ctl, Shape shape)
        {
            if (ctl == null)
                throw new ArgumentNullException("ctl",
                                                "The control passed as parameter to the ComputeShape method is null.");

            switch (shape)
            {
                case Shape.Rectangle:
                    return Shapes.DrawFullRectangle(ctl.AbsolutePosition, ctl.Size,
                                                    ctl.InnerAreaColor, ctl.BorderColor,
                                                    ctl.ControlStyle.Shading,
                                                    ctl.BorderSize,
                                                    ctl.BorderStyle);


                case Shape.Circle:
                    ICircularControl circleCtl = ctl as ICircularControl;
                    return
                        Shapes.DrawFullCircle(circleCtl.CenterAbsolutePosition, circleCtl.Radius,
                                              circleCtl.OutlineRadius,
                                              circleCtl.Slices, ctl.BorderSize, ctl.InnerAreaColor,
                                              ctl.BorderColor);

                case Shape.LeftTrapezoidUpside:
                    return Shapes.DrawFullLeftTrapezoid(
                        ctl.AbsolutePosition, ctl.Size, TabPanel.DefaultTabTriangleWidth, true, ctl.InnerAreaColor,
                        ctl.BorderSize, ctl.BorderColor, ctl.BorderStyle, Border.All & ~Border.Left, Border.All,
                        ctl.ControlStyle.Shading);

                case Shape.LeftTrapezoidDownside:
                    return Shapes.DrawFullLeftTrapezoid(
                        ctl.AbsolutePosition, ctl.Size, TabPanel.DefaultTabTriangleWidth, false, ctl.InnerAreaColor,
                        ctl.BorderSize, ctl.BorderColor, ctl.BorderStyle, Border.All & ~Border.Left, Border.All,
                        ctl.ControlStyle.Shading);

                case Shape.RightTrapezoidUpside:
                    return Shapes.DrawFullRightTrapezoid(
                        ctl.AbsolutePosition, ctl.Size, TabPanel.DefaultTabTriangleWidth, true, ctl.InnerAreaColor,
                        ctl.BorderSize, ctl.BorderColor, ctl.BorderStyle, Border.All & ~Border.Right, Border.All,
                        ctl.ControlStyle.Shading);

                case Shape.RightTrapezoidDownside:
                    return Shapes.DrawFullRightTrapezoid(
                        ctl.AbsolutePosition, ctl.Size, TabPanel.DefaultTabTriangleWidth, false, ctl.InnerAreaColor,
                        ctl.BorderSize, ctl.BorderColor, ctl.BorderStyle, Border.All & ~Border.Right, Border.All,
                        ctl.ControlStyle.Shading);

                default:
                    return Empty;
            }
        }
    }
}