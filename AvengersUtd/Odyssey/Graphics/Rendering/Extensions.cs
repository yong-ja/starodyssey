using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AvengersUtd.Odyssey.Graphics.Rendering
{
    public static class Extensions
    {
        public static LinkedListNode<BaseCommand> FindFirstBackward(this LinkedListNode<BaseCommand> lNode, BaseCommand command)
        {
            LinkedListNode<BaseCommand> currentNode = lNode;

            while (currentNode.Value == command)
            {
                currentNode = currentNode.Previous;
                if (currentNode.Value == lNode.List.First.Value)
                    return null;
            }

            return currentNode;
        }

        public static LinkedListNode<BaseCommand> FindFirstForward(this LinkedListNode<BaseCommand> lNode, BaseCommand command)
        {
            LinkedListNode<BaseCommand> currentNode = lNode;

            while (currentNode.Value == command)
            {
                currentNode = currentNode.Next;

                if (currentNode.Value == lNode.List.Last.Value)
                    return null;
            }

            return currentNode;
        }
    }
}
