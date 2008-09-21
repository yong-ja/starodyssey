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
using AvengersUtd.Odyssey.UserInterface.Helpers;
using AvengersUtd.Odyssey.UserInterface.Properties;
using AvengersUtd.Odyssey.UserInterface.RenderableControls;
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

        static Dictionary<string, ControlStyle> controlStyles;
        static Dictionary<string, TextStyle> textStyles;

        #region Properties

        public static int DefaultFontSizeScaled
        {
            // not yet fully implemented
            get { return DefaultFontSize; }
        }

        #endregion

        public static TextStyle GetTextStyle(string id)
        {
            if (textStyles == null)
            {
                throw new ArgumentException("No control styles loaded! Be sure to call LoadTextStyles first.");
            }
            else if (id == null || !textStyles.ContainsKey(id))
            {
                return textStyles[ControlStyle.Error];
            }
            else
                return textStyles[id];
        }

        public static ControlStyle GetControlStyle(string id)
        {
            if (controlStyles == null)
            {
                throw new ArgumentException("No control styles loaded! Be sure to call LoadControlStyles first.");
            }
            else if (id == null || !controlStyles.ContainsKey(id))
                return controlStyles[ControlStyle.Error];
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

        public static void AddControlStyle(ControlStyle newStyle)
        {
            controlStyles.Add(newStyle.Name, newStyle);
        }

        public static void AddTextStyle(TextStyle newTextStyle)
        {
            textStyles.Add(newTextStyle.Name, newTextStyle);
        }

        public static void Remove(ControlStyle style)
        {
            controlStyles.Remove(style.Name);
        }

        public static void SaveControlStyles(Hud hud)
        {
            List<XmlControlStyle> xmlControlStyles = new List<XmlControlStyle>();

            foreach (BaseControl ctl in TreeTraversal.PreOrderControlVisit(hud))
            {
                xmlControlStyles.Add(new XmlControlStyle(ctl.ControlStyle));
            }

            XmlSerializer xmlSerializer = new XmlSerializer(typeof (List<XmlControlStyle>));
            XmlWriterSettings xmlSettings = new XmlWriterSettings();
            xmlSettings.Indent = true;
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("Style", DefaultNamespace);

            using (XmlWriter xmlWriter = XmlWriter.Create("styles.xml", xmlSettings))
            {
                xmlWriter.WriteStartDocument();
                xmlWriter.WriteComment(
                    string.Format(
                        "This is an Odyssey UI Style Declaration file, generated on {0}.\nPlease do not modify it if you don't know what you are doing. Visit the Odyssey UI website at http://www.avengersutd.com/wiki/OdysseyUI for more information.\nThanks for using the Odyssey UI!",
                        DateTime.Now.ToString("f"), Assembly.GetExecutingAssembly().GetName().Version.ToString(3)));
                xmlSerializer.Serialize(xmlWriter, xmlControlStyles, ns);
                xmlWriter.WriteEndDocument();
                xmlWriter.Flush();
            }
        }

        public static void SaveTextStyles(TextStyle style)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof (XmlTextStyle[]));
            XmlWriterSettings xmlSettings = new XmlWriterSettings();
            xmlSettings.Indent = true;
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("Style", DefaultNamespace);

            using (XmlWriter xmlWriter = XmlWriter.Create("textstyles.xml", xmlSettings))
            {
                xmlWriter.WriteStartDocument();
                xmlWriter.WriteComment(
                    string.Format(
                        "This is an Odyssey UI Style Declaration file, generated on {0}.\nPlease do not modify it if you don't know what you are doing. Visit the Odyssey UI website at http://www.avengersutd.com/wiki/OdysseyUI for more information.\nThanks for using the Odyssey UI!",
                        DateTime.Now.ToString("f")));
                xmlSerializer.Serialize(xmlWriter, new XmlTextStyle[] {new XmlTextStyle(style)}, ns);
                xmlWriter.WriteEndDocument();
                xmlWriter.Flush();
            }
        }

        public static void LoadTextStyles(string filename)
        {
            if (!File.Exists(filename))
                throw new FileNotFoundException(Exceptions.ERR_StyleNotFound, filename);

            XmlTextStyle[] xmlcStyle;
            XmlSerializer xmlSerializer = new XmlSerializer(typeof (XmlTextStyle[]));
            using (XmlTextReader xmlReader = new XmlTextReader(filename))
            {
                xmlcStyle = (XmlTextStyle[]) xmlSerializer.Deserialize(xmlReader);
            }
            
            if (textStyles== null)
                textStyles = new Dictionary<string, TextStyle>();

            for (int i = 0; i < xmlcStyle.Length; i++)
            {
                textStyles.Add(xmlcStyle[i].Name, xmlcStyle[i].ToTextStyle());
            }
        }

        public static void LoadControlStyles(string filename)
        {
            if (!File.Exists(filename))
                throw new FileNotFoundException(Exceptions.ERR_StyleNotFound, filename);

            XmlControlStyle[] xmlcStyle;
            XmlSerializer xmlSerializer = new XmlSerializer(typeof (XmlControlStyle[]));
            using (XmlTextReader xmlReader = new XmlTextReader(filename))
            {
                xmlcStyle = (XmlControlStyle[]) xmlSerializer.Deserialize(xmlReader);
            }

            if (controlStyles==null)
                controlStyles = new Dictionary<string, ControlStyle>();

            for (int i = 0; i < xmlcStyle.Length; i++)
            {
                controlStyles.Add(xmlcStyle[i].Name, xmlcStyle[i].ToControlStyle());
            }
        }
    }
}