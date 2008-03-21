#region Disclaimer

/* 
 * DataViewerCellCollection
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

using System.Collections.ObjectModel;

namespace AvengersUtd.Odyssey.UserInterface.RenderableControls
{
    public class DataViewerCellCollection : Collection<DataViewerCell>
    {
        DataViewerRow row;

        public DataViewerCellCollection(DataViewerRow row)
        {
            this.row = row;
        }

        protected override void InsertItem(int index, DataViewerCell item)
        {
            base.InsertItem(index, item);
            item.OwningRow = row;
        }
    }
}