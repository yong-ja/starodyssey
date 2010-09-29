using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;
using AvengersUtd.Odyssey.UserInterface.Controls;

namespace AvengersUtd.Odyssey.UserInterface.Xml
{
    public abstract class XmlContainerControl : XmlBaseControl
    {
        List<XmlBaseControl> xmlControlList;

        protected XmlContainerControl(BaseControl control) : base(control)
        {
            xmlControlList = new List<XmlBaseControl>();
        }

        [XmlArray("Controls")]
        public virtual List<XmlBaseControl> XmlControlList
        {
            get {
                return xmlControlList.Count > 0 ? xmlControlList : null;
            }
            set { xmlControlList = value; }
        }

        //void CreateWrapperForControl<TWrapperClass>(BaseControl ctl)
        //    where TWrapperClass : XmlBaseControl, new()
        //{
        //    TWrapperClass wrapper = new TWrapperClass();
        //    wrapper.FromControl(ctl);
        //    xmlControlList.Add(wrapper);
        //}

        //internal override void FromControl(BaseControl control)
        //{
        //    base.FromControl(control);

        //    xmlControlList = new List<XmlBaseControl>();
        //    ContainerControl containerControl = (ContainerControl) control;

        //    foreach (BaseControl childControl in containerControl.Controls)
        //    {

        //        Type wrapperType = UIParser.GetWrapperTypeForControl(childControl.GetType());
        //        if (wrapperType == null) continue;

        //        MethodInfo mi =
        //            typeof (XmlContainerControl).GetMethod("CreateWrapperForControl",
        //                                                   BindingFlags.NonPublic | BindingFlags.Instance);
                
        //        mi = mi.MakeGenericMethod();
        //        mi.Invoke(this, new object[]
        //                      {
        //                          childControl
        //                      });
        //    }
        //}

        protected override void WriteCustomCSCode(StringBuilder sb)
        {
            return;
        }

        public virtual void WriteContainerCSCode(StringBuilder sb)
        {
            foreach (XmlBaseControl xmlBaseControl in XmlControlList)
            {
                XmlContainerControl xmlContainerControl = xmlBaseControl as XmlContainerControl;
                if ( xmlContainerControl!= null)
                    xmlContainerControl.WriteContainerCSCode(sb);
                    
                sb.AppendFormat("{0}.Add({1});\n", this.Id, xmlBaseControl.Id);

            }
        }
    }

    [Serializable]
    [XmlType("Panel")]
    public class XmlPanel : XmlContainerControl
    {
        public XmlPanel(BaseControl control) : base(control)
        {

        }

        
    }

    //[XmlType(TypeName = "GroupBox")]
    //public class XmlGroupBox : XmlContainerControl
    //{
    //    string caption;

    //    public XmlGroupBox() : base()
    //    {
    //        caption = string.Empty;
    //    }

    //    [XmlAttribute]
    //    public string Caption
    //    {
    //        get { return caption; }
    //        set { caption = value; }
    //    }

    //    public override void FromControl(BaseControl control)
    //    {
    //        base.FromControl(control);
    //        GroupBox groupBox = control as GroupBox;
    //        caption = groupBox.Caption;
    //    }
    //}

    //public class XmlTabPanel : XmlContainerControl
    //{
    //    string textStyleClass;
    //}

    //[XmlType("Window")]
    //public class XmlWindow : XmlContainerControl
    //{
    //    string title;

    //    public XmlWindow() : base()
    //    {
    //        title = string.Empty;
    //    }

    //    [XmlAttribute]
    //    public string Title
    //    {
    //        get { return title; }
    //        set { title = value; }
    //    }

    //    public override void FromControl(BaseControl control)
    //    {
    //        base.FromControl(control);
    //        Window window = control as Window;
    //        title = window.Title;
    //    }
    //}

    [Serializable]
    [XmlType("HUD")]
    public class XmlHud : XmlContainerControl
    {
        public XmlHud(Hud hud) : base(hud)
        {
        }

        public override void WriteCSharpCode(StringBuilder sb)
        {
            sb.AppendLine("Hud = Hud.FromDescription(Game.Context.Device, hudDescription);");
        }


    }
}