#region Disclaimer

/* 
 * Node
 *
 * Created on 30 agosto 2007
 * Author: Adalberto L. Simeone (Taranto, Italy)
 * Website: http://www.avengersutd.com
 *
 * Part of the Odyssey Utils Library
 *
 * This source code is Intellectual Property of the Author
 * and is released under the Creative Commons Attribution 
 * NonCommercial License, available at:
 * http://creativecommons.org/licenses/by-nc/3.0/ 
 * You can alter and use this source code as you wish, 
 * provided that you do not use the results in commercial
 * projects, without the express and written consent of
 * the Author.
 *
 */

#endregion

#region Using Directives

using System;

#endregion

namespace AvengersUtd.Odyssey.Utils.Collections
{
    public class Node<T>
    {
        #region Private fields

        int index;
        int level;
        Node<T> parent;
        Node<T> next;
        Node<T> previous;
        NodeCollection<Node<T>> children;

        #endregion

        #region Properties

        #endregion

        #region Constructors

        #endregion
    }
}