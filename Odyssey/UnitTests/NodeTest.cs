using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AvengersUtd.Odyssey.Graphics.Rendering.SceneGraph;
using AvengersUtd.Odyssey.Utils.Collections;
using NUnit.Framework;

namespace UnitTests
{
    [TestFixture]
    public class NodeTest
    {
        [Test]
        public void AppendTest()
        {
            FixedNode rootNode = new FixedNode();
            FixedNode newNode = new FixedNode();
            rootNode.AppendChild(newNode);

            Assert.AreEqual(rootNode.FirstChild, newNode);
            Assert.AreEqual(newNode.Parent, rootNode);
            Assert.AreEqual(rootNode.LastChild, newNode);
            Assert.IsNull(rootNode.NextSibling);
            Assert.IsNull(rootNode.PreviousSibling);
            Assert.IsTrue(rootNode.HasChildNodes);
            Assert.IsTrue((rootNode as INode).Level == 0);
            Assert.IsTrue((newNode as INode).Level == 1);
        }

        [Test]
        public void PrependTest()
        {
            FixedNode rootNode = new FixedNode();
            FixedNode newChild1 = new FixedNode();
            FixedNode newChild2 = new FixedNode();

            rootNode.PrependChild(newChild1);
            rootNode.PrependChild(newChild2);

            Assert.AreEqual(rootNode.FirstChild, newChild2);
        }

        [Test]
        public void InsertAfterTest()
        {
            FixedNode rootNode = new FixedNode();
            FixedNode newChild1 = new FixedNode();
            FixedNode newChild2 = new FixedNode();
            FixedNode newChildBetween1e2 = new FixedNode();
            FixedNode newLastNode = new FixedNode();

            rootNode.AppendChild(newChild1);
            rootNode.AppendChild(newChild2);

            Assert.IsTrue((newChild1 as INode).Index == 0);
            Assert.IsTrue((newChild2 as INode).Index == 1);

            rootNode.InsertAfter(newChildBetween1e2, newChild1);

            Assert.IsTrue((newChild1 as INode).Index == 0);
            Assert.IsTrue((newChildBetween1e2 as INode).Index == 1);
            Assert.IsTrue((newChild2 as INode).Index==2);

            Assert.IsTrue(newChild1.HasNextSibling);
            Assert.AreEqual(newChild1.NextSibling, newChildBetween1e2);
            Assert.AreEqual(newChildBetween1e2.PreviousSibling, newChild1);
            Assert.AreEqual(newChild2.PreviousSibling, newChildBetween1e2);
            Assert.IsNull(newChild1.PreviousSibling);
            Assert.IsNull(newChild2.NextSibling);
            Assert.IsTrue(rootNode.LastChild == newChild2);

            rootNode.InsertAfter(newLastNode, rootNode.LastChild);

            Assert.AreEqual(rootNode.LastChild, newLastNode);
            Assert.IsNull(newLastNode.NextSibling);
            Assert.AreEqual(newLastNode.PreviousSibling, newChild2);
            Assert.AreEqual(newChild2.NextSibling, newLastNode);
            Assert.IsTrue((newLastNode as INode).Index == 3);
 
        }

        [Test]
        public void InsertBeforeTest()
        {
            FixedNode rootNode = new FixedNode();
            FixedNode newChild1 = new FixedNode();
            FixedNode newChild2 = new FixedNode();
            FixedNode newChildBetween1e2 = new FixedNode();
            FixedNode newFirstNode = new FixedNode();
            
            rootNode.AppendChild(newChild1);
            rootNode.AppendChild(newChild2);
            rootNode.InsertBefore(newChildBetween1e2, newChild2);

            Assert.IsTrue((newChild1 as INode).Index == 0);
            Assert.IsTrue((newChildBetween1e2 as INode).Index == 1);
            Assert.IsTrue((newChild2 as INode).Index == 2);

            Assert.IsTrue(newChild1.HasNextSibling);
            Assert.AreEqual(newChild1.NextSibling, newChildBetween1e2);
            Assert.AreEqual(newChildBetween1e2.PreviousSibling, newChild1);
            Assert.AreEqual(newChild2.PreviousSibling, newChildBetween1e2);
            Assert.IsNull(newChild1.PreviousSibling);
            Assert.IsNull(newChild2.NextSibling);
            Assert.IsTrue(rootNode.LastChild == newChild2);

            rootNode.InsertBefore(newFirstNode, rootNode.FirstChild);

            Assert.AreEqual(rootNode.FirstChild, newFirstNode);
            Assert.IsNull(newFirstNode.PreviousSibling);
            Assert.AreEqual(newFirstNode.NextSibling, newChild1);
            Assert.AreEqual(newChild1.PreviousSibling, newFirstNode);

            Assert.IsTrue((newFirstNode as INode).Index == 0);
            Assert.IsTrue((newChild1 as INode).Index == 1);
            Assert.IsTrue((newChildBetween1e2 as INode).Index == 2);
            Assert.IsTrue((newChild2 as INode).Index == 3);
        }

        [Test]
        public void FiveLevelDeep()
        {
            FixedNode rootNode = new FixedNode();
            FixedNode parentNode = rootNode;
            
            for (int i=0; i< 5; i++)
            {
                FixedNode newNode = new FixedNode();
                parentNode.AppendChild(newNode);
                Assert.AreEqual(newNode.Parent, parentNode);
                parentNode = newNode;
            }
            Assert.IsTrue((parentNode as INode).Level== 5);

        }

        [Test]
        public void FailToAddNodeToSelf()
        {
            FixedNode rootNode = new FixedNode();
            
            try
            {
                rootNode.AppendChild(rootNode);
            }
            catch(Exception ex)
            {
                Assert.IsInstanceOfType(typeof (InvalidOperationException), ex);
            }
            Assert.IsNull(rootNode.FirstChild);
        }

        [Test]
        public void FailToAddNodeThatIsAlreadyChild()
        {
            FixedNode rootNode = new FixedNode();
            FixedNode newNode = new FixedNode();
            rootNode.AppendChild(newNode);
            try
            {
                rootNode.AppendChild(newNode);
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(typeof (ArgumentException), ex);
            }
            Assert.IsFalse(rootNode.FirstChild.HasNextSibling);
        }

        [Test]
        public void FailToAddAncestorNode()
        {
            FixedNode rootNode = new FixedNode();
            FixedNode newNode1 = new FixedNode();
            FixedNode newNode2 = new FixedNode();
            rootNode.AppendChild(newNode1);
            newNode1.AppendChild(newNode2);
            
            try
            {
                newNode2.AppendChild(rootNode);
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(typeof(InvalidOperationException), ex);
            }
            Assert.IsFalse(newNode2.HasChildNodes);
        }

        [Test]
        public void RemoveChild()
        {
            FixedNode rootNode = new FixedNode();
            FixedNode newChild1 = new FixedNode();
            FixedNode newChild2 = new FixedNode();
            FixedNode newChild3 = new FixedNode();

            rootNode.AppendChild(newChild1);
            rootNode.AppendChild(newChild2);
            rootNode.AppendChild(newChild3);

            rootNode.RemoveChild(newChild2);

            Assert.IsTrue((newChild1 as INode).Index == 0);
            Assert.IsTrue((newChild3 as INode).Index == 1);
            Assert.AreEqual(newChild1.NextSibling, newChild3);
            Assert.AreEqual(newChild3.PreviousSibling, newChild1);

            rootNode.RemoveChild(rootNode.FirstChild);

            Assert.AreEqual(rootNode.FirstChild, newChild3);
            Assert.IsNull(newChild3.PreviousSibling);
            Assert.IsTrue((newChild3 as INode).Index == 0);

            rootNode.InsertBefore(newChild1, newChild3);
            rootNode.RemoveChild(rootNode.LastChild);

            Assert.AreEqual(rootNode.LastChild, newChild1);
            Assert.IsNull(newChild1.NextSibling);
            Assert.IsTrue((newChild1 as INode).Index == 0);

            rootNode.RemoveChild(rootNode.FirstChild);

            Assert.IsFalse(rootNode.HasChildNodes);
        }

        [Test]
        public void ReplaceChild()
        {
            FixedNode rootNode = new FixedNode();
            FixedNode newChild1 = new FixedNode();
            FixedNode newChild2 = new FixedNode();
            FixedNode newChild3 = new FixedNode();
            FixedNode newChildX = new FixedNode();

            rootNode.AppendChild(newChild1);
            rootNode.AppendChild(newChild2);
            rootNode.AppendChild(newChild3);

            rootNode.ReplaceChild(newChildX, newChild2);

            Assert.AreEqual(newChild1.NextSibling, newChildX);
            Assert.AreEqual(newChildX.PreviousSibling, newChild1);
            Assert.AreEqual(newChildX.NextSibling, newChild3);
            Assert.IsTrue((newChildX as INode).Index == 1);

            rootNode.ReplaceChild(newChild2, newChildX);
            rootNode.ReplaceChild(newChildX, newChild1);

            Assert.AreEqual(rootNode.FirstChild, newChildX);
            Assert.AreEqual(newChildX.NextSibling, newChild2);
            Assert.AreEqual(newChild2.PreviousSibling, newChildX);
            Assert.IsTrue((newChildX as INode).Index == 0);

            rootNode.ReplaceChild(newChild1, newChildX);
            rootNode.ReplaceChild(newChildX, newChild3);

            Assert.AreEqual(rootNode.LastChild, newChildX);
            Assert.AreEqual(newChild2.NextSibling, newChildX);
            Assert.AreEqual(newChildX.PreviousSibling, newChild2);
            Assert.IsTrue((newChildX as INode).Index == 2);

        }

    }

}
