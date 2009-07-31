#region Disclaimer

/* 
 * DataViewerColumnCollection
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
using System.Collections.ObjectModel;
using AvengersUtd.Odyssey.UserInterface.Properties;

namespace AvengersUtd.Odyssey.UserInterface
{
    public class DataViewerColumnCollection : Collection<DataViewerColumn>
    {
        DataViewer dataViewer;

        public DataViewerColumnCollection(DataViewer dataViewer)
        {
            this.dataViewer = dataViewer;
        }

        protected override void InsertItem(int index, DataViewerColumn item)
        {
            if (item.CellType == null)
                throw new InvalidOperationException(Exceptions.DataViewerColumn_CellTemplate_Null);

            base.InsertItem(index, item);
            item.DataViewer = dataViewer;
            item.Index = index;
            dataViewer.OnColumnAdded(new ColumnEventArgs(item));
        }
    }
}