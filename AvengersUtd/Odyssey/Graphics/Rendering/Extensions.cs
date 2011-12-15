using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX.Direct3D11;

namespace AvengersUtd.Odyssey.Graphics.Rendering
{
    public static class Extensions
    {
        #region LinkedListNode<BaseCommand>
        public static LinkedListNode<ICommand> FindFirstBackward(this LinkedListNode<ICommand> lNode, ICommand command)
        {
            LinkedListNode<ICommand> currentNode = lNode;

            while (currentNode.Value == command)
            {
                currentNode = currentNode.Previous;
                if (currentNode.Value == lNode.List.First.Value)
                    return null;
            }

            return currentNode;
        }

        public static LinkedListNode<ICommand> FindFirstForward(this LinkedListNode<ICommand> lNode, ICommand command)
        {
            LinkedListNode<ICommand> currentNode = lNode;

            while (currentNode.Value == command)
            {
                currentNode = currentNode.Next;

                if (currentNode.Value == lNode.List.Last.Value)
                    return null;
            }

            return currentNode;
        } 
        #endregion


    }
}
