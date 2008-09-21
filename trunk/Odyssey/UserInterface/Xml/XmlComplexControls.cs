#region Disclaimer

/* 
 * Xml Wrapper for complex controls
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
using System.Xml.Serialization;
using AvengersUtd.Odyssey.UserInterface.RenderableControls;

namespace AvengersUtd.Odyssey.UserInterface.Xml
{
    [XmlType(TypeName = "DropDownList")]
    public class XmlDropDownList : XmlBaseControl
    {
        List<string> items;
        int selectedItemIdx;

        public XmlDropDownList()
            : base()
        {
            TextStyleClass = string.Empty;
            items = new List<string>();
        }

        [XmlAttribute]
        public int SelectedItemIndex
        {
            get { return selectedItemIdx; }
            set { selectedItemIdx = value; }
        }


        [XmlArray("Items")]
        [XmlArrayItem("Item")]
        public List<string> Items
        {
            get { return items; }
            set { items = value; }
        }

        public override void FromControl(BaseControl control)
        {
            base.FromControl(control);

            DropDownList dropDownList = control as DropDownList;
            selectedItemIdx = dropDownList.SelectedIndex;

            foreach (Label Label in dropDownList.Controls)
                items.Add(Label.Text);
        }
    }

    [XmlType(TypeName = "OptionGroup")]
    public class XmlOptionGroup : XmlBaseControl
    {
        List<string> options;
        int selectedOptionIdx;

        public XmlOptionGroup()
            : base()
        {
            options = new List<string>();
        }

        [XmlAttribute]
        public int SelectedOptionIndex
        {
            get { return selectedOptionIdx; }
            set { selectedOptionIdx = value; }
        }

        [XmlArray("OptionButtons")]
        [XmlArrayItem("OptionButton")]
        public List<string> Options
        {
            get { return options; }
            set { options = value; }
        }

        public override void FromControl(BaseControl control)
        {
            base.FromControl(control);
            OptionGroup optionGroup = control as OptionGroup;

            selectedOptionIdx = optionGroup.SelectedIndex;
            foreach (OptionButton optionButton in optionGroup.Controls)
            {
                options.Add(optionButton.Caption);
            }
        }
    }
}