using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml.Serialization;
using AvengersUtd.Odyssey.UserInterface;

namespace AvengersUtd.Odyssey.UserInterface.Xml
{
    public abstract class XmlContainerControl : XmlBaseControl
    {
        List<XmlBaseControl> xmlControlList;

        protected XmlContainerControl()
        {
            xmlControlList = new List<XmlBaseControl>();
        }

        [XmlArray("Controls")]
        public virtual List<XmlBaseControl> XmlControlList
        {
            get
            {
                if (xmlControlList.Count > 0)
                    return xmlControlList;
                else return null;
            }
            set { xmlControlList = value; }
        }

        void CreateWrapperForControl<WrapperClass>(BaseControl ctl)
            where WrapperClass : XmlBaseControl, new()
        {
            WrapperClass wrapper = new WrapperClass();
            wrapper.FromControl(ctl);
            xmlControlList.Add(wrapper);
        }

        public override void FromControl(BaseControl control)
        {
            base.FromControl(control);

            xmlControlList = new List<XmlBaseControl>();
            ContainerControl containerControl = (ContainerControl) control;

            foreach (BaseControl childControl in containerControl.Controls)
            {
                MethodInfo mi =
                    typeof (XmlContainerControl).GetMethod("CreateWrapperForControl",
                                                           BindingFlags.NonPublic | BindingFlags.Instance);
                mi.MakeGenericMethod(UIParser.GetWrapperTypeForControl(childControl.GetType())).Invoke(this,
                                                                                                       new object[]
                                                                                                           {
                                                                                                               childControl
                                                                                                           });
            }
        }
    }

    [Serializable]
    [XmlType("Panel")]
    public class XmlPanel : XmlContainerControl
    {
        public XmlPanel() : base()
        {
        }
    }

    [XmlType(TypeName = "GroupBox")]
    public class XmlGroupBox : XmlContainerControl
    {
        string caption;

        public XmlGroupBox() : base()
        {
            caption = string.Empty;
        }

        [XmlAttribute]
        public string Caption
        {
            get { return caption; }
            set { caption = value; }
        }

        public override void FromControl(BaseControl control)
        {
            base.FromControl(control);
            GroupBox groupBox = control as GroupBox;
            caption = groupBox.Caption;
        }
    }

    //public class XmlTabPanel : XmlContainerControl
    //{
    //    string textStyleClass;
    //}

    [XmlType("Window")]
    public class XmlWindow : XmlContainerControl
    {
        string title;

        public XmlWindow() : base()
        {
            title = string.Empty;
        }

        [XmlAttribute]
        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        public override void FromControl(BaseControl control)
        {
            base.FromControl(control);
            Window window = control as Window;
            title = window.Title;
        }
    }

    [Serializable]
    [XmlType("HUD")]
    public class XmlHud : XmlContainerControl
    {
        public XmlHud() : base()
        {
        }
    }
}