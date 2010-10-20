#region Disclaimer

/* 
 * StyleManager
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

#region Using directives

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;
using AvengersUtd.Odyssey.UserInterface.Controls;
using AvengersUtd.Odyssey.UserInterface.Text;
using AvengersUtd.Odyssey.UserInterface.Xml;
using System.IO;

#endregion

namespace AvengersUtd.Odyssey.UserInterface.Style
{
    public static class StyleManager
    {
        const int DefaultFontSize = 20;

        const string DefaultNamespace = "http://www.avengersutd.com";
        const string StyleDeclarationsTag = "StyleDeclarations";

        static Dictionary<string, ControlDescription> controlStyles;
        static Dictionary<string, TextDescription> textStyles;

        #region Properties

        public static int DefaultFontSizeScaled
        {
            // not yet fully implemented
            get { return DefaultFontSize; }
        }

        public static IEnumerable<ControlDescription> ControlDescriptions
        {
            get {
                return controlStyles != null ? controlStyles.Values : null;
            }
        }

        public static IEnumerable<TextDescription> TextDescriptions
        {
            get
            {
                return textStyles != null ? textStyles.Values : null;
            }
        }

        #endregion

        public static TextDescription GetTextDescription(string id)
        {
            if (textStyles == null)
            {
                throw Error.ArgumentNull("textStyles", typeof(StyleManager),"GetTextDescription", Properties.Resources.ERR_NoTextDescription);
            }
            else    if (id == null || !textStyles.ContainsKey(id))
            {
                return textStyles[TextDescription.Error];
            }
            else
                return textStyles[id];
        }

        public static ControlDescription GetControlDescription(string id)
        {
            if (controlStyles == null)
            {
                throw Error.ArgumentNull("controlStyles", typeof(StyleManager), "GetControlDescription", Properties.Resources.ERR_NoControlDescription);
            }
            else if (id == null || !controlStyles.ContainsKey(id))
                return controlStyles[ControlDescription.Error];
            else
                return controlStyles[id];
        }

        public static bool ContainsControlStyle(string id)
        {
            return controlStyles.ContainsKey(id);
        }

        public static bool ContainsTextStyle(string id)
        {
            return textStyles.ContainsKey(id);
        }

        public static void AddControlStyle(ControlDescription newStyle)
        {
            controlStyles.Add(newStyle.Name, newStyle);
        }

        public static void AddTextStyle(TextDescription newTextStyle)
        {
            textStyles.Add(newTextStyle.Name, newTextStyle);
        }

        public static void Remove(ControlDescription style)
        {
            controlStyles.Remove(style.Name);
        }

        //public static void SaveControlStyles(Hud hud)
        //{
        //    List<XmlControlStyle> xmlControlStyles = new List<XmlControlStyle>();

        //    foreach (BaseControl ctl in TreeTraversal.PreOrderControlVisit(hud))
        //    {
        //        xmlControlStyles.Add(new XmlControlStyle(ctl.ControlStyle));
        //    }

        //    XmlSerializer xmlSerializer = new XmlSerializer(typeof (List<XmlControlStyle>));
        //    XmlWriterSettings xmlSettings = new XmlWriterSettings();
        //    xmlSettings.Indent = true;
        //    XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
        //    ns.Add("Style", DefaultNamespace);

        //    using (XmlWriter xmlWriter = XmlWriter.Create("styles.xml", xmlSettings))
        //    {
        //        xmlWriter.WriteStartDocument();
        //        xmlWriter.WriteComment(
        //            string.Format(
        //                "This is an Odyssey UI Style Declaration file, generated on {0}.\nPlease do not modify it if you don't know what you are doing. Visit the Odyssey UI website at http://www.avengersutd.com/wiki/OdysseyUI for more information.\nThanks for using the Odyssey UI!",
        //                DateTime.Now.ToString("f"), Assembly.GetExecutingAssembly().GetName().Version.ToString(3)));
        //        xmlSerializer.Serialize(xmlWriter, xmlControlStyles, ns);
        //        xmlWriter.WriteEndDocument();
        //        xmlWriter.Flush();
        //    }
        //}

        //public static void SaveTextStyles(TextDescription style)
        //{
        //    XmlSerializer xmlSerializer = new XmlSerializer(typeof (XmlTextDescription[]));
        //    XmlWriterSettings xmlSettings = new XmlWriterSettings();
        //    xmlSettings.Indent = true;
        //    XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
        //    ns.Add("Style", DefaultNamespace);

        //    using (XmlWriter xmlWriter = XmlWriter.Create("textstyles.xml", xmlSettings))
        //    {
        //        xmlWriter.WriteStartDocument();
        //        xmlWriter.WriteComment(
        //            string.Format(
        //                "This is an Odyssey UI Style Declaration file, generated on {0}.\nPlease do not modify it if you don't know what you are doing. Visit the Odyssey UI website at http://www.avengersutd.com/wiki/OdysseyUI for more information.\nThanks for using the Odyssey UI!",
        //                DateTime.Now.ToString("f")));
        //        xmlSerializer.Serialize(xmlWriter, new XmlTextDescription[] {new XmlTextDescription(style)}, ns);
        //        xmlWriter.WriteEndDocument();
        //        xmlWriter.Flush();
        //    }
        //}

        public static void LoadTextDescription(string filename)
        {
            if (!File.Exists(filename))
                Error.MessageMissingFile(Properties.Resources.ERR_StyleNotFound, filename);

            XmlTextDescription[] xmlcStyle;
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(XmlTextDescription[]));
            using (XmlTextReader xmlReader = new XmlTextReader(filename))
            {
                xmlcStyle = (XmlTextDescription[])xmlSerializer.Deserialize(xmlReader);
            }

            if (textStyles == null)
                textStyles = new Dictionary<string, TextDescription>();

            foreach (XmlTextDescription t in xmlcStyle)
            {
                textStyles.Add(t.Name, t.ToTextDescription());
            }
        }

        public static void LoadControlDescription(string filename)
        {
            if (!File.Exists(filename))
                Error.MessageMissingFile(filename, Properties.Resources.ERR_StyleNotFound);

            XmlControlDescription[] xmlcStyle;
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(XmlControlDescription[]));
            using (XmlTextReader xmlReader = new XmlTextReader(filename))
            {
                xmlcStyle = (XmlControlDescription[])xmlSerializer.Deserialize(xmlReader);
            }

            if (controlStyles == null)
                controlStyles = new Dictionary<string, ControlDescription>();

            for (int i = 0; i < xmlcStyle.Length; i++)
            {
                controlStyles.Add(xmlcStyle[i].Name, xmlcStyle[i].ToControlDescription());
            }
        }
    }
}