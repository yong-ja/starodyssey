#region Disclaimer

/* 
 * DataViewerColumn
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
using AvengersUtd.Odyssey.UserInterface.Helpers;
using AvengersUtd.Odyssey.UserInterface.Style;
using AvengersUtd.Odyssey.UserInterface.Properties;

namespace AvengersUtd.Odyssey.UserInterface.RenderableControls
{
    public class DataViewerColumn : DataViewerBand
    {
        const int defaultMinimumWidth = 10;
        const int defaultWidth = 100;
        Type cellType;
        Type cellValueType;
        string dataPropertyName;

        int displayIndex;
        int minimumWidth;
        string name;
        DynamicPropertyAccessor propertyAccessor;
        int width;

        #region Properties

        public int MinimumWidth
        {
            get { return minimumWidth; }
            set { minimumWidth = value; }
        }


        public int Width
        {
            get { return width; }
            set
            {
                if (width != value)
                {
                    int newValue = value;

                    if (value < minimumWidth && width != minimumWidth)
                        newValue = minimumWidth;

                    width = newValue;
                    DataViewer.OnColumnWidthChanged(new ColumnEventArgs(this));
                }
            }
        }

        public int DisplayIndex
        {
            get { return displayIndex; }
            set { displayIndex = value; }
        }

        public string Name
        {
            get { return ((DataViewerColumnHeaderCell) HeaderCell).Caption; }
            set { ((DataViewerColumnHeaderCell) HeaderCell).Caption = value; }
        }

        public Type ValueType
        {
            get { return cellValueType; }
            set { cellValueType = value; }
        }

        public string DataPropertyName
        {
            get { return dataPropertyName; }
            set
            {
                if (dataPropertyName != value)
                {
                    dataPropertyName = value;
                }
            }
        }

        internal DynamicPropertyAccessor PropertyAccessor
        {
            get
            {
                if (propertyAccessor == null)
                    propertyAccessor = new DynamicPropertyAccessor(DataViewer.DataElementType, dataPropertyName);

                return propertyAccessor;
            }
        }

        public Type CellType
        {
            get
            {
                if (cellType == null)
                    throw new InvalidOperationException(Exceptions.DataViewerColumn_CellTemplate_Null);

                return cellType;
            }
            set { cellType = value; }
        }

        public override DataViewerCellStyle InheritedStyle
        {
            get
            {
                if (DataViewer == null)
                    throw new InvalidOperationException(Exceptions.DataViewerCell_InheritedStyle_Unbounded);
                else
                {
                    if (HasDefaultCellStyle)
                        DefaultCellStyle.MergeStyle(DataViewer.DefaultCellStyle);
                    return DefaultCellStyle;
                }
            }
        }

        #endregion

        public DataViewerColumn() : this(null)
        {
        }

        public DataViewerColumn(Type cellType)
        {
            displayIndex = -1;
            minimumWidth = defaultMinimumWidth;
            width = defaultWidth;
            this.cellType = cellType;
        }
    }
}