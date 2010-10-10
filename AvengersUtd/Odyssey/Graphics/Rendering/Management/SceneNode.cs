﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using AvengersUtd.Odyssey.Utils.Collections;

namespace AvengersUtd.Odyssey.Graphics.Rendering.Management
{
  
    public abstract class SceneNode : Node
    {
       
        #region Private Fields

        string label;
        SceneNodeType nodeType;
        
        #endregion

        #region Properties

        public string Label
        {
            get { return label; }
            set { label = value; }
        }

        public SceneNodeType NodeType
        {
            get { return nodeType; }
        }

        public bool Inited
        {
            get;
            protected set;
        }

        #endregion

        #region Methods

        public virtual void Init()
        {
            Inited = true;
        }

        public abstract void Update();

        protected override void OnChildAdded(object sender, NodeEventArgs e)
        {
            base.OnChildAdded(sender, e);
            SceneNode sNode = ((SceneNode) e.Node);
            if (!sNode.Inited)
                sNode.Init();
        }


        #endregion

        #region Base SceneNode Properties

        public SceneNode Parent
        {
            get { return ParentNode as SceneNode; }
        }

        public SceneNode PreviousSibling
        {
            get { return PreviousSiblingNode as SceneNode; }
        }

        public SceneNode NextSibling
        {
            get { return NextSiblingNode as SceneNode; }
        }

        public SceneNode FirstChild
        {
            get { return FirstChildNode as SceneNode; }
        }

        public SceneNode LastChild
        {
            get { return LastChildNode as SceneNode; }
        }

        public SceneNodeCollection<SceneNode> ChildrenCollection
        {
            get
            {
                SceneNodeCollection<SceneNode> nodeCollection = new SceneNodeCollection<SceneNode>();
                foreach (SceneNode node in ChildrenIterator)
                    nodeCollection.Add(node);
                return nodeCollection;
            }
        }

        #endregion

        #region Constructor

        protected SceneNode(string label, SceneNodeType nodeType)
        {
            this.label = label;
            this.nodeType = nodeType;
        }

        protected SceneNode(SceneNodeType nodeType) : this(nodeType.ToString(), nodeType)
        {
        }
        #endregion

        #region Base Node Methods

        public void AppendChild(SceneNode newChild)
        {
            OnAppendChild(newChild);
        }

        /// <summary>
        /// Removes the node from this node's children.
        /// </summary>
        /// <param name="oldChild">The node to remove.</param>
        public void RemoveChild(SceneNode oldChild)
        {
            OnRemoveChild(oldChild);
        }

        /// <summary>
        /// Replaces a node with another one.
        /// </summary>
        /// <param name="newChild">The new node.</param>
        /// <param name="oldChild">The node to be replaced.</param>
        public void ReplaceChild(SceneNode newChild, SceneNode oldChild)
        {
            OnReplaceChild(newChild, oldChild);
        }

        /// <summary>
        /// Inserts a new node immediately before the referenced one.
        /// </summary>
        /// <param name="newChild">The new node.</param>
        /// <param name="refNode">The referenced node.</param>
        public void InsertBefore(SceneNode newChild, SceneNode refNode)
        {
            OnInsertBefore(newChild, refNode);
        }

        /// <summary>
        /// Inserts a new node immediately after the referenced one.
        /// </summary>
        /// <param name="newChild">The new node.</param>
        /// <param name="refNode">The referenced node.</param>
        public void InsertAfter(SceneNode newChild, Node refNode)
        {
            OnInsertAfter(newChild, refNode);
        }

        /// <summary>
        /// Inserts the specified node before every other node.
        /// </summary>
        /// <param name="newChild">The new child node.</param>
        public void PrependChild(SceneNode newChild)
        {
            OnPrependChild(newChild);
        }

        public bool Contains(string label)
        {
            foreach (SceneNode node in ChildrenIterator)
                if (node.label == label)
                    return true;
            return false;
        }

        public SceneNode Find(string label)
        {
            SceneNode result = null;

            foreach (SceneNode node in ChildrenIterator)
                if (node.label == label)
                {
                    result = node;
                }

            return result;
        }

        public SceneNodeCollection<T> SelectNodes<T>(Predicate<T> predicate)
            where T : SceneNode
        {
            SceneNodeCollection<T> nodeCollection = new SceneNodeCollection<T>();
            foreach (INode node in Node.PreOrderVisit(this))
            {
                T nodeT = node as T;
                if (nodeT != null && predicate(nodeT))
                    nodeCollection.Add(nodeT);
            }

            return nodeCollection;
        }

        public SceneNodeCollection<T> SelectNodes<T>()
            where T : SceneNode
        {
            SceneNodeCollection<T> nodeCollection = new SceneNodeCollection<T>();
            foreach (INode node in Node.PreOrderVisit(this))
            {
                T nodeT = node as T;
                if (nodeT != null)
                    nodeCollection.Add(nodeT);
            }

            return nodeCollection;
        }

        #endregion

        #region Static Methods
        /// <summary>
        /// Finds first node of type TNode that is parent of startingNode.
        /// </summary>
        /// <typeparam name="TNode">The type of the Node, derived from SceneNode.</typeparam>
        /// <param name="startingNode">The node from where to start searching.</param>
        /// <returns>The node of type T if found, otherwise null.</returns>
        public static TNode FindFirstTParentNode<TNode>(SceneNode startingNode)
            where TNode:SceneNode
        {
            SceneNode node = startingNode;
            while (node != null)
            {
                TNode tNode = node as TNode;
                if (tNode == null)
                    node = node.Parent;
                else
                    return tNode;
            }
            return null;
        }

        /// <summary>
        /// Finds first node of type TNode that is a child of startingNode.
        /// </summary>
        /// <typeparam name="TNode">The type of the Node, derived from SceneNode.</typeparam>
        /// <param name="startingNode">The node from where to start searching.</param>
        /// <returns>The node of type T if found, otherwise null.</returns>
        public static TNode FindFirstTChildNode<TNode>(SceneNode startingNode)
            where TNode : SceneNode
        {
            foreach (INode node in PreOrderVisit(startingNode))
            {
                TNode tNode = node as TNode;
                if (tNode == null)
                    continue;
                else
                    return tNode;
            }
            return null;

        }
        #endregion

    }
}