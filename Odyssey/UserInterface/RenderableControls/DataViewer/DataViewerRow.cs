#region Disclaimer

/* 
 * DataViewerRow
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

using System;
using System.Drawing;
using AvengersUtd.Odyssey.UserInterface.Style;

namespace AvengersUtd.Odyssey.UserInterface
{
    public class DataViewerRow : DataViewerBand
    {
        // [rgn] Private fields 

        DataViewerCellCollection cells;
        Size size;

        #region [rgn] Public properties

        public int Height
        {
            get
            {
                int margin = 10;

                if (HasDefaultCellStyle)
                    return margin + DefaultCellStyle.TextStyle.Size;
                else
                    return margin + DataViewer.DefaultCellStyle.TextStyle.Size;
            }
            set { throw new NotImplementedException(); }
        }

        public DataViewerCellCollection Cells
        {
            get { return cells; }
        }

        public DataViewerCell this[int index]
        {
            get { return cells[index]; }
            set { cells[index] = value; }
        }

        public Size Size
        {
            get { return size; }
        }

        #endregion [rgn]

        #region [rgn] Constructors 

        public DataViewerRow()
        {
            cells = new DataViewerCellCollection(this);
            IsRow = true;
        }

        #endregion [rgn]

        public override DataViewerCellStyle InheritedStyle
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }
    }
}