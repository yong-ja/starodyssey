using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AvengersUtd.Odyssey.Geometry
{
    public class Vertices : IList<Vector2D>
    {
        private readonly List<Vector2D> vertices;

        public Vertices()
        {
            vertices = new List<Vector2D>();
        }

        public Vertices(IEnumerable<Vector2D> vertices) : this()
        {
            this.vertices.AddRange(vertices);
        }

        #region IList<Vector2D> Members
        #region ICollection<Vector2D> Members

        public void Add(Vector2D item)
        {
            vertices.Add(item);
        }

        public void AddRange(IEnumerable<Vector2D> points)
        {
            vertices.AddRange(points);
        }

        public void Clear()
        {
            vertices.Clear();
        }

        public bool Contains(Vector2D item)
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

        public IEnumerator<Vector2D> GetEnumerator()
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

        public int IndexOf(Vector2D index)
        {
            return vertices.IndexOf(index);
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

        /// <summary>
        /// Advances the index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        public int NextIndex(int index)
        {
            if (index == Count - 1)
            {
                return 0;
            }
            return index + 1;
        }

        /// <summary>
        /// Gets the previous index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        public int PreviousIndex(int index)
        {
            if (index == 0)
            {
                return Count - 1;
            }
            return index - 1;
        }

        public Vector2D NextVertex(int index)
        {
            return this[NextIndex(index)];
        }

        
        public void Reverse()
        {
            vertices.Reverse();
        }

        public static explicit operator Polygon(Vertices vertices)
        {
            return new Polygon(vertices);
        }
    }
}
