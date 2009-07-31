#region Disclaimer

/* 
 * LabelPage
 *
 * Created on 21 August 2007
 * Author: Adalberto L. Simeone (Taranto, Italy)
 * Website: http://www.avengersutd.com
 *
 * Part of the Odyssey User Interface Library
 *
 * This source code is Intellectual property of the Author
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

using System.Collections.Generic;
using AvengersUtd.Odyssey.UserInterface.Helpers;
using AvengersUtd.Odyssey.UserInterface;
#if !(SlimDX)
    using Microsoft.DirectX;
#else
using SlimDX;
#endif

namespace AvengersUtd.Odyssey.UserInterface.Style
{
    public class LabelPage
    {
        List<List<Label>> lineCollection;
        int lines;
        int maximumCapacity;

        public LabelPage() :
            this(0)
        {
        }

        public LabelPage(int maxCapacity)
        {
            maximumCapacity = maxCapacity;
            lineCollection = new List<List<Label>>(maximumCapacity);
        }

        public int Lines
        {
            get { return lines; }
        }

        public List<Label> LabelCollection
        {
            get
            {
                List<Label> collection = new List<Label>();
                foreach (List<Label> list in lineCollection)
                {
                    collection.AddRange(list);
                }
                return collection;
            }
        }

        /// <summary>
        /// Retrieves or set the line you specify.
        /// </summary>
        /// <param name="line">The line number you need to access, starting from 0.</param>
        /// <returns></returns>
        public List<Label> this[int line]
        {
            get { return lineCollection[line]; }
            set { lineCollection[line] = value; }
        }


        /// <summary>
        /// Retrieves or set the label at the line and index specified.
        /// </summary>
        /// <param name="line">The line you need to access, starting from 0.</param>
        /// <param name="labelIndex">The label you need to access, starting from 0.</param>
        /// <returns></returns>
        public Label this[int line, int labelIndex]
        {
            get { return lineCollection[line][labelIndex]; }
            set { lineCollection[line][labelIndex] = value; }
        }

        public void AppendAt(int line, Label label)
        {
            this[line].Add(label);
        }

        /// <summary>
        /// Inserts a new line in the last line of the collection.
        /// </summary>
        /// <param name="label">The label you want to add.</param>
        public void Append(Label label)
        {
            AppendAt(lines - 1, label);
        }

        public void RemoveLine(int index)
        {
            if (lineCollection[index] != null)
            {
                lineCollection.RemoveAt(index);
                lines--;
            }
            else
                DebugManager.LogError(
                    string.Format("RemoveLine({0})", index),
                    "The requested line does not exist.", "LabelPage");
        }

        public void InsertLine()
        {
            lineCollection.Add(new List<Label>());
            lines++;

            if (lines == maximumCapacity)
                RemoveLine(0);
        }

        public void InsertLine(int startIndex)
        {
            lineCollection.Insert(startIndex, new List<Label>());
            lines++;

            if (lines == maximumCapacity)
                RemoveLine(0);
        }

        public void AlignLine(int index)
        {
            int maxHeight;

            if (this[index].Count == 0)
                return;
            else
                maxHeight = this[index, 0].Area.Height;

            bool shouldAlign = false;
            int prevHeight = 0;

            foreach (Label l in this[index])
            {
                if (l.Area.Height != maxHeight)
                {
                    shouldAlign = true;

                    if (l.Area.Height > maxHeight)
                    {
                        maxHeight = l.Area.Height;
                    }
                }
            }

            if (shouldAlign)
            {
                foreach (Label l in this[index])
                {
                    if (l.Area.Height < maxHeight)
                        l.Position += new Vector2(0, (maxHeight - l.Area.Height));
                }
            }
        }

        public void Align()
        {
            for (int i = 0; i < lines; i++)
                AlignLine(i);
        }
    }
}