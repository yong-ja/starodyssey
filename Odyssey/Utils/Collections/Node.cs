using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using AvengersUtd.Odyssey.Utils.Properties;

namespace AvengersUtd.Odyssey.Utils.Collections
{
    public class NodeEventArgs : EventArgs
    {
        int index;
        int level;
        INode node;

        public int Index
        {
            get { return index; }
        }

        public int Level
        {
            get { return level; }
        }

        public INode Node
        {
            get { return node; }
        }

        public NodeEventArgs(INode node)
        {
            index = node.Index;
            level = node.Level;
            this.node = node;
        }
    }

    [DebuggerDisplay("{GetType().Name} = {ToString()}")]
    public abstract class Node : INode
    {
        public delegate void NodeEventHandler(object sender, NodeEventArgs e);

        #region Private fields

        bool isLeaf;
        int index;
        int level;
        INode parent;
        INode nextSibling;
        INode previousSibling;
        INode firstChild;
        INode lastChild;

        protected EventHandlerList eventHandlerList = new EventHandlerList();

        #endregion

        #region Events

        static readonly object EventParentChanged;
        static readonly object EventChildAdded;
        static readonly object EventChildRemoved;

        public event NodeEventHandler ParentChanged
        {
            add { eventHandlerList.AddHandler(EventParentChanged, value); }
            remove { eventHandlerList.RemoveHandler(EventParentChanged, value); }
        }

        public event NodeEventHandler ChildAdded
        {
            add { eventHandlerList.AddHandler(EventChildAdded, value); }
            remove { eventHandlerList.RemoveHandler(EventChildAdded, value); }
        }

        public event NodeEventHandler ChildRemoved
        {
            add { eventHandlerList.AddHandler(EventChildRemoved, value); }
            remove { eventHandlerList.RemoveHandler(EventChildRemoved, value); }
        }

        protected virtual void OnParentChanged(object sender, NodeEventArgs e)
        {
            NodeEventHandler handler = (NodeEventHandler)eventHandlerList[EventParentChanged];
            if (handler != null)
                handler(this, e);
        }

        protected virtual void OnChildAdded(object sender, NodeEventArgs e)
        {
            NodeEventHandler handler = (NodeEventHandler)eventHandlerList[EventChildAdded];
            if (handler != null)
                handler(this, e);
        }

        protected virtual void OnChildRemoved(object sender, NodeEventArgs e)
        {
            NodeEventHandler handler = (NodeEventHandler)eventHandlerList[EventChildRemoved];
            if (handler != null)
                handler(this, e);
        }
        #endregion

        #region Constructor

        static Node()
        {
            EventParentChanged = new object();
            EventChildAdded = new object();
            EventChildRemoved = new object();
        }

        protected Node()
        {
            eventHandlerList = new EventHandlerList();
        }
        #endregion

        #region Properties
        public bool HasChildNodes
        {
            get { return lastChild != null; }
        }

        public bool IsLeaf
        {
            get { return isLeaf; }
            protected set { isLeaf = value; }
        }

        public bool HasNextSibling
        {
            get { return nextSibling != null; }
        }

        public int ChildrenCount
        {
            get
            {
                int count=0;
                foreach (Node node in ChildrenNodeIterator)
                    count++;
                return count;
            }
        }
        #endregion
      
        #region Protected INode Properties
        protected IEnumerable<INode> ChildrenNodeIterator
        {
            get
            {
                INode tempNode = firstChild;
                while (tempNode != null)
                {
                    yield return tempNode;
                    tempNode = tempNode.NextSibling;
                }
            }
        }

        protected INode ParentNode
        {
            get { return parent; }
        }

        protected INode FirstChildNode
        {
            get { return firstChild; }
        }

        protected INode LastChildNode
        {
            get { return lastChild; }
        }

        protected INode NextSiblingNode
        {
            get { return nextSibling; }
        }

        protected INode PreviousSiblingNode
        {
            get { return previousSibling; }
        }
        #endregion

        #region Protected INode Methods
        protected virtual void OnAppendChild(INode newChild)
        {
            if (isLeaf)
                throw new ArgumentException(Resources.ERR_NodeIsLeaf);
            
            if (newChild == null)
                throw new ArgumentNullException("newChild", Resources.ERR_NodeIsNull);

            if (!HasChildNodes)
            {
                if (IsNodeAncestorOf(newChild, this))
                    throw new InvalidOperationException(Resources.ERR_NodeIsAncestor);

                firstChild = lastChild = newChild;
                newChild.Parent = this;
                OnChildAdded(this, new NodeEventArgs(newChild));
            }
            else
            {
                OnInsertAfter(newChild, lastChild);
            }
        }
       
        protected virtual void OnRemoveChild(INode oldChild)
        {
            if (oldChild == null)
                throw new ArgumentNullException("oldChild", Resources.ERR_NodeIsNull);

            if (!IsNodeChildOf(oldChild, this))
                throw new ArgumentException(Resources.ERR_NodeNotChild, "oldChild");

            if (firstChild == oldChild)
            {
                if (oldChild == lastChild)
                    firstChild = lastChild = null;
                else
                {
                    firstChild = oldChild.NextSibling;
                    firstChild.PreviousSibling = null;
                    UpdateIndicesForward(firstChild, 0);
                }
            }
            else if (lastChild == oldChild)
            {
                lastChild = oldChild.PreviousSibling;
                lastChild.NextSibling = null;
                UpdateIndicesBackward(lastChild, lastChild.Index);
            }
            else
            {
                INode previousNode = null;
                INode nextNode = null;
                foreach (Node children in ChildrenNodeIterator)
                {
                    if (children == oldChild)
                    {
                        previousNode = oldChild.PreviousSibling;
                        nextNode = oldChild.NextSibling;
                        break;
                    }
                }

                previousNode.NextSibling = nextNode;
                nextNode.PreviousSibling = previousNode;
                UpdateIndicesForward(nextNode, previousNode.Index +1);
            }

            OnChildRemoved(this, new NodeEventArgs(oldChild));
        }

        protected virtual void OnReplaceChild(INode newChild, INode oldChild)
        {
            if (oldChild == null)
                throw new ArgumentNullException("oldChild", Resources.ERR_NodeIsNull);

            if (newChild == null)
                throw new ArgumentNullException("newChild", Resources.ERR_NodeIsNull);

            if (IsNodeAncestorOf(newChild, this))
                throw new InvalidOperationException(Resources.ERR_NodeIsAncestor);

            if (IsNodeChildOf(newChild, this))
                throw new ArgumentException(Resources.ERR_NodeAlreadyChild, "oldChild");

            if (firstChild == oldChild)
            {
                if (oldChild == lastChild)
                {
                    firstChild = lastChild = newChild;
                }
                else
                {
                    newChild.PreviousSibling = null;
                    newChild.NextSibling = firstChild.NextSibling;
                    firstChild.NextSibling.PreviousSibling = newChild;
                    firstChild = newChild;
                    firstChild.Index = 0;
                }

            }
            else if (lastChild == oldChild)
            {
                newChild.Index = lastChild.Index;
                newChild.NextSibling = null;
                newChild.PreviousSibling = lastChild.PreviousSibling;
                lastChild.PreviousSibling.NextSibling = newChild;
                newChild.Index = lastChild.Index;
                lastChild = newChild;
            }
            else
            {
                INode previousNode = null;
                INode nextNode = null;
                foreach (Node children in ChildrenNodeIterator)
                {
                    if (children == oldChild)
                    {
                        previousNode = oldChild.PreviousSibling;
                        nextNode = oldChild.NextSibling;
                        break;
                    }
                }

                previousNode.NextSibling = newChild;
                nextNode.PreviousSibling = newChild;
                newChild.PreviousSibling = previousNode;
                newChild.NextSibling = nextNode;
                newChild.Index = previousNode.Index + 1;
            }

            newChild.Parent = oldChild.Parent;

            OnChildRemoved(this, new NodeEventArgs(oldChild));
            OnChildAdded(this, new NodeEventArgs(newChild));
        }

        protected virtual void OnInsertBefore(INode newChild, INode refNode)
        {
            if (isLeaf)
                throw new ArgumentException(Resources.ERR_NodeIsLeaf);

            if (refNode == null)
                throw new ArgumentNullException("refNode", Resources.ERR_NodeIsNull);

            if (newChild == null)
                throw new ArgumentNullException("newChild", Resources.ERR_NodeIsNull);

            if (IsNodeAncestorOf(newChild, this))
                throw new InvalidOperationException(Resources.ERR_NodeIsAncestor);

            if (IsNodeChildOf(newChild, this))
                throw new ArgumentException(Resources.ERR_NodeAlreadyChild, "refNode");

            if (refNode == firstChild)
            {
                newChild.PreviousSibling = null;
                firstChild = refNode;
            }
            else
            {
                INode previousNode = refNode.PreviousSibling;
                newChild.PreviousSibling = previousNode;
                previousNode.NextSibling = newChild;
            }

            newChild.NextSibling = refNode;
            refNode.PreviousSibling = newChild;

            if (refNode == firstChild)
                firstChild = newChild;

           
            UpdateIndicesForward(newChild, refNode.Index);
            
            newChild.Parent = this;

            OnChildAdded(this, new NodeEventArgs(newChild));
        }

        protected virtual void OnInsertAfter(INode newChild, INode refNode)
        {
            if (isLeaf)
                throw new ArgumentException(Resources.ERR_NodeIsLeaf);

            if (refNode == null)
                throw new ArgumentNullException("refNode", Resources.ERR_NodeIsNull);

            if (newChild == null)
                throw new ArgumentNullException("newChild", Resources.ERR_NodeIsNull);

            if (IsNodeAncestorOf(newChild, this))
                throw new InvalidOperationException(Resources.ERR_NodeIsAncestor);

            if (IsNodeChildOf(newChild, this))
                throw new ArgumentException(Resources.ERR_NodeNotChild, "refNode");

            if (refNode == lastChild)
            {
                newChild.NextSibling = null;
                lastChild = newChild;
            }
            else
            {
                INode nextNode = refNode.NextSibling;
                nextNode.PreviousSibling = newChild;
                newChild.NextSibling = nextNode;
            }

            refNode.NextSibling = newChild;
            newChild.PreviousSibling = refNode;
            
            UpdateIndicesForward(newChild, refNode.Index + 1);
            
            newChild.Parent = this;
            OnChildAdded(this, new NodeEventArgs(newChild));

        }

        protected virtual void OnPrependChild(INode newChild)
        {
            if (isLeaf)
                throw new ArgumentException(Resources.ERR_NodeIsLeaf);

            if (newChild == null)
                throw new ArgumentNullException("newChild", Resources.ERR_NodeIsNull);

            if (!HasChildNodes)
            {
                firstChild = lastChild = newChild;
                newChild.Parent = this;
                OnChildAdded(this, new NodeEventArgs(newChild));
            }
            else
                OnInsertBefore(newChild, firstChild);
        }

        #endregion

        #region Methods
        /// <summary>
        /// Removes all children from this node.
        /// </summary>
        public void RemoveAll()
        {
            firstChild = lastChild = null;
        }

        public override string ToString()
        {
            return string.Format("Level {0} - Index {1}", level, index);
        }
        #endregion

        #region Static Methods
        /// <summary>
        /// Checks whether the node is a child of the specified parent node.
        /// </summary>
        /// <param name="childNode">The child node.</param>
        /// <param name="parentNode">The parent node.</param>
        /// <returns><c>True</c> if childNode is a child of parentNode. <c>False</c> otherwise.</returns>
        internal static bool IsNodeChildOf(INode childNode, INode parentNode)
        {
            foreach (INode node in parentNode.ChildrenIterator)
            {
                if (node == childNode)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Checks whether the node is a parent of the specified node or whether the two nodes are
        /// the same.
        /// </summary>
        /// <param name="node">The child node.</param>
        /// <param name="refNode">The parent node.</param>
        /// <returns><c>True</c> if node is a parent of refNode. <c>False</c> otherwise.</returns>
        internal static bool IsNodeAncestorOf(INode node, INode refNode)
        {
            if (node == refNode)
                return true;

            INode currentNode = refNode;
            while (currentNode.Parent != null)
            {
                if (currentNode.Parent == node)
                    return true;
                currentNode = currentNode.Parent;
            }
            return false;
        }

        internal static void UpdateIndicesForward(INode headNode, int startIndex)
        {
            INode node = headNode;
            int i = startIndex;
            while (node != null)
            {
                node.Index = i;
                node = node.NextSibling;
                i++;
            }
        }

        internal static void UpdateIndicesBackward(INode headNode, int startIndex)
        {
            INode node = headNode;
            int i = startIndex;
            while (node != null)
            {
                node.Index = i;
                node = node.PreviousSibling;
                i--;
            }
        }
        #endregion

        #region Visit algorithms

        public static IEnumerable<INode> PreOrderVisit(INode headNode)
        {
            yield return headNode;

            if (headNode.HasChildNodes)
                foreach (INode node in PreOrderVisit(headNode.FirstChild))
                    yield return node;
            if (headNode.HasNextSibling)
                foreach (INode node in PreOrderVisit(headNode.NextSibling))
                    yield return node;
        }
       
        public static IEnumerable<INode> PostOrderVisit(INode headNode)
        {
            if (headNode.HasChildNodes)
            {
                foreach (INode childNode in PreOrderVisit(headNode.FirstChild))
                    yield return childNode;
            }
            else
            {
                INode node = headNode.NextSibling;
                while (node != null)
                {
                    yield return node;
                    node = node.NextSibling;
                }
            }
            yield return headNode;
        } 
        
        #endregion

        #region INode Members

        int INode.Index
        {
            get
            {
                return index;
            }
            set
            {
                index = value;
            }
        }

        INode INode.Parent
        {
            get
            {
                return parent;
            }
            set
            {
                if (parent != value)
                {
                    parent = value;
                    foreach (INode node in PreOrderVisit(this))
                        node.Level = node.Parent.Level + 1;
                    OnParentChanged(this, new NodeEventArgs(parent));
                }
            }
        }

        int INode.Level
        {
            get { return level; }
            set
            {
                level = value;
            }
        }

        INode INode.PreviousSibling
        {
            get
            {
                return previousSibling;
            }
            set
            {
                previousSibling = value;
            }
        }

        INode INode.NextSibling
        {
            get
            {
                return nextSibling;
            }
            set { nextSibling = value; }
        }

        INode INode.FirstChild
        {
            get { return firstChild; }
        }

        INode INode.LastChild
        {
            get { return lastChild; }
        }

        IEnumerable<INode> INode.ChildrenIterator
        {
            get
            {
                return ChildrenNodeIterator;
            }
        }

        void INode.AppendChild(INode newNode)
        {
            OnAppendChild(newNode );
        }

        void INode.InsertBefore(INode newChild, INode refNode)
        {
            OnInsertBefore(newChild , refNode );
        }

        void INode.InsertAfter(INode newChild, INode refNode)
        {
            OnInsertAfter(newChild , refNode );
        }

        void INode.RemoveChild(INode oldChild)
        {
            OnRemoveChild(oldChild );
        }

        void INode.ReplaceChild(INode newChild, INode oldChild)
        {
            OnReplaceChild(newChild , oldChild );
        }

        void INode.PrependChild(INode newChild)
        {
            OnPrependChild(newChild );
        }

        #endregion

        

    }
}
