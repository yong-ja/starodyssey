using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AvengersUtd.Odyssey.Geometry
{
    public class VerticesCollection : IList<Vector2D>
    {
        private readonly List<Vector2D> vertices;

        public VerticesCollection()
        {
            vertices = new List<Vector2D>();
        }

        public VerticesCollection(IEnumerable<Vector2D> vertices) : this()
        {
            this.vertices.AddRange(vertices);
        }

        #region IList<Vector2D> Members
        #region ICollection<Vector2D> Members

        public void Add(Vector2D item)
        {
            vertices.Add(item);
        }

        void ICollection<Vector2D>.Clear()
        {
            vertices.Clear();
        }

        bool ICollection<Vector2D>.Contains(Vector2D item)
        {
            return vertices.Contains(item);
        }

        void ICollection<Vector2D>.CopyTo(Vector2D[] array, int arrayIndex)
        {
            vertices.CopyTo(array, arrayIndex);
        }

        bool ICollection<Vector2D>.IsReadOnly
        {
            get { return false; }
        }

        public int Count
        {
            get { return vertices.Count; }
        }

        public bool Remove(Vector2D item)
        {
            if (!vertices.Contains(item))
                return false;

            vertices.Remove(item);
            return true;
        }

        #endregion

        #region IEnumerable<Vector2D> Members

        IEnumerator<Vector2D> IEnumerable<Vector2D>.GetEnumerator()
        {
            return vertices.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return vertices.GetEnumerator();
        }

        #endregion

        #region IList<Vector2D> Members

        int IList<Vector2D>.IndexOf(Vector2D item)
        {
            return vertices.IndexOf(item);
        }

        public void Insert(int index, Vector2D item)
        {
            vertices.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            vertices.RemoveAt(index);
        }

        public Vector2D this[int index]
        {
            get
            {
                if (index < 0 || index > Count)
                    throw Error.IndexNotPresentInArray("this", index);
                return vertices[index];
            }
            set
            {
                if (index < 0 || index > Count)
                    throw Error.IndexNotPresentInArray("this", index);
                vertices[index] = value;
            }
        }


        #endregion 
        #endregion

        public static explicit operator Polygon(VerticesCollection verticesCollection)
        {
            return new Polygon(verticesCollection);
        }
    }
}
