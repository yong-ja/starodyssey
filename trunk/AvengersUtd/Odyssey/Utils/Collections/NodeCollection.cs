#region Using directives

using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Generic;

#endregion

namespace AvengersUtd.Odyssey.Utils.Collections
{
    public class NodeCollection<T> : IEnumerable<Node<T>>
    {
        #region Private fields

        SortedList<int, Node<T>> list;
        #endregion

        #region Properties
        public Node<T> this[int index]
        {
            get
            {

                return list[index];
            }
        }

        public int Count
        {
            get { return list.Count; }
        }

        #endregion

        #region Constructors
        internal NodeCollection()
        {
            list = new SortedList<int, Node<T>>();
        }

        /// <summary>
        /// Creates a ordered list containing this node's subtree.
        /// </summary>
        /// <param name="node">The root of the subtree.</param>
        //internal NodeCollection(GenericNode<T> node) : this()
        //{
        //    foreach (Node<T> childNode in node.PreOrderVisitIterator)
        //        Add(childNode);
        //}

        #endregion

        //internal void Add(GenericNode<T> node)
        //{
        //    list.Add(node.Index, node);
        //}

        
        

        



        #region IEnumerable<Node<T>> Members

        IEnumerator<Node<T>> IEnumerable<Node<T>>.GetEnumerator()
        {
            return list.Values.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return list.Values.GetEnumerator();
        }

        #endregion
    }
}