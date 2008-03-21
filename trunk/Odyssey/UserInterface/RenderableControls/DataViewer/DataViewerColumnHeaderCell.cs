#region Disclaimer

/* 
 * DataViewerColumnHeaderCell
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

namespace AvengersUtd.Odyssey.UserInterface.RenderableControls
{
    public class DataViewerColumnHeaderCell : DataViewerHeaderCell
    {
        const string DefaultTextStyleClass = "ColumnHeader";


        public DataViewerColumnHeaderCell()
        {
        }

        //public override DataViewerCellStyle Style
        //{
        //    get
        //    {
        //        if (!HasStyle)
        //        {
        //            DataViewerCellStyle columnHeaderStyle = new DataViewerCellStyle();
        //            columnHeaderStyle.CellColorArray = new ColorArray(
        //                Color.Transparent, Color.Transparent, Color.Transparent, Color.Transparent, Color.Transparent,
        //                Color.Transparent,
        //                Color.Gray, Color.Gray);
        //            columnHeaderStyle.TextStyle = StyleManager.GetTextStyle(DefaultTextStyleClass);

        //            base.Style = columnHeaderStyle;
        //        }
        //        return base.Style;
        //    }
        //    set { base.Style = value; }
        //}
    }
}