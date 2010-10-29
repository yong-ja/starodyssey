using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AvengersUtd.Odyssey.Geometry;

namespace AvengersUtd.Odyssey.UserInterface.Style
{
    public class ShapeCollection : IEnumerable<ShapeDescription>
    {
        readonly ShapeDescription[] shapeDescriptions;

        public ShapeCollection(int capacity)
        {
            shapeDescriptions = new ShapeDescription[capacity];
        }

        public ShapeCollection(IEnumerable<ShapeDescription> shapes)
        {
            shapeDescriptions = shapes.ToArray();
        }

        public ShapeDescription this[int index]
        {
            get { return shapeDescriptions[index]; }
            set { shapeDescriptions[index] = value; }
        }

        public int Length
        {
            get { return shapeDescriptions.Length; }
        }

        public bool IsInited
        {
            get
            {
               return shapeDescriptions[0] != null;
            }
        }

        internal ShapeDescription[] Array
        {
            get { return shapeDescriptions; }
        }

        public void SetDirtyFlag(bool flag)
        {
            foreach (ShapeDescription shapeDescription in shapeDescriptions)
            {
                shapeDescription.IsDirty = flag;
            }
        }

        #region IEnumerable
		public IEnumerator<ShapeDescription> GetEnumerator()
        {
            return ((IEnumerable<ShapeDescription>) shapeDescriptions).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return shapeDescriptions.GetEnumerator();
        } 
	#endregion
    }
}
