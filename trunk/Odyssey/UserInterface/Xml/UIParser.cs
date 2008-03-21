#region Disclaimer

/* 
 * UIPaerser
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
using System.Collections.Generic;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;
using AvengersUtd.Odyssey.UserInterface.RenderableControls;

namespace AvengersUtd.Odyssey.UserInterface.Xml
{
    /// <summary>
    /// Allows Xml serialization and deserialization of interfaces built with 
    /// the Odyssey UI
    /// </summary>
    public static class UIParser
    {
        static Dictionary<Type, Type> registeredTypes;


        static UIParser()
        {
            registeredTypes = new Dictionary<Type, Type>();

            registeredTypes.Add(typeof (Button), typeof (XmlButton));
            registeredTypes.Add(typeof (CheckBox), typeof (XmlCheckBox));
            registeredTypes.Add(typeof (DropDownList), typeof (XmlDropDownList));
            registeredTypes.Add(typeof (GroupBox), typeof (XmlGroupBox));
            registeredTypes.Add(typeof (Label), typeof (XmlLabel));
            registeredTypes.Add(typeof (OptionGroup), typeof (XmlOptionGroup));
            registeredTypes.Add(typeof (Panel), typeof (XmlPanel));
            registeredTypes.Add(typeof (TextBox), typeof (XmlTextBox));
            registeredTypes.Add(typeof (TrackBar), typeof (XmlTrackBar));
            registeredTypes.Add(typeof (Window), typeof (XmlWindow));
        }

        public static Type GetWrapperTypeForControl(Type controlClass)
        {
            return registeredTypes[controlClass];
        }


        public static void RegisterXmlWrapper(Type controlType, Type wrapperType)
        {
            if (!registeredTypes.ContainsKey(controlType))
                registeredTypes.Add(controlType, wrapperType);
        }

        public static void SerializeHud(Hud hud, string outputFilename)
        {
            XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
            xmlWriterSettings.Indent = true;

            XmlAttributes xmlAttributes = new XmlAttributes();
            xmlAttributes.XmlArray = new XmlArrayAttribute("Controls");
            foreach (Type type in registeredTypes.Values)
            {
                xmlAttributes.XmlArrayItems.Add(new XmlArrayItemAttribute(type));
            }
            XmlAttributeOverrides xmlAttributeOverrides = new XmlAttributeOverrides();
            xmlAttributeOverrides.Add(typeof (XmlContainerControl), "XmlControlList", xmlAttributes);

            XmlSerializer xmlSerializer = new XmlSerializer(typeof (XmlHud), xmlAttributeOverrides);
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("Style", "http://www.avengersutd.com");


            XmlHud xmlHud = new XmlHud();
            xmlHud.FromControl(hud);


            using (XmlWriter xmlWriter = XmlWriter.Create(outputFilename, xmlWriterSettings))
            {
                xmlWriter.WriteStartDocument();
                xmlWriter.WriteComment(
                    string.Format(
                        "This is an Odyssey User Interface Declaration file, generated on {0}.\nPlease do not modify it if you don't know what you are doing. Visit the Odyssey UI website at http://www.avengersutd.com/wiki/OdysseyUI for more information.\nThanks for using the Odyssey UI!",
                        DateTime.Now.ToString("f"), Assembly.GetExecutingAssembly().GetName().Version.ToString(3)));
                xmlSerializer.Serialize(xmlWriter, xmlHud, ns);
                xmlWriter.WriteEndDocument();
                xmlWriter.Flush();
            }
        }
    }
}