#region Using directives

using System.IO;
using System.Xml;
using System.Xml.Serialization;

#endregion

namespace AvengersUtd.BrickLab.Settings
{
    public static class XmlManager
    {
        #region Private fields

        #endregion

        #region Properties

        #endregion

        /// <summary>
        /// Deserializes an object of type <c>T</c> stored in a Xml file.
        /// </summary>
        /// <typeparam name="T">The type of the serialized object.</typeparam>
        /// <param name="filename">The filename of xml file.</param>
        /// <returns>An object of type <c>T</c>.</returns>
        /// <exception cref="FileNotFoundException">The specified filename cannot be found.</exception>
        public static T Deserialize<T>(string filename)
        {
            if (!File.Exists(filename))
                throw new FileNotFoundException("The specified filename cannot be found.", filename);

            T data;
            XmlSerializer xmlSerializer = new XmlSerializer(typeof (T));
            using (XmlTextReader xmlReader = new XmlTextReader(filename))
            {
                data = (T) xmlSerializer.Deserialize(xmlReader);
            }

            return data;
        }

        public static void Serialize<T>(T obj, string filename)
        {
            XmlWriterSettings xmlWriterSettings = new XmlWriterSettings {Indent = true, OmitXmlDeclaration= true};

            XmlSerializer xmlSerializer = new XmlSerializer(typeof (T));
            using (XmlWriter xmlWriter = XmlWriter.Create(filename, xmlWriterSettings))
            {
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                ns.Add("Settings", "http://www.avengersutd.com");

                xmlWriter.WriteStartDocument();
                xmlWriter.WriteComment(
                    string.Format(
                        "This file stores the settings for BrickLab.\n" +
                        "Please do not modify it if you do not know what you are doing. " +
                        "Visit the BrickLab website at http://www.avengersutd.com/blog/ for more information.\n"));
                xmlSerializer.Serialize(xmlWriter, obj, ns);
                xmlWriter.WriteEndDocument();
                xmlWriter.Flush();
            }
        }

        /// <summary>
        /// Deserializes a generic collection of type <c>T</c> stored in a Xml file.
        /// </summary>
        /// <typeparam name="T">The type of the serialized objects.</typeparam>
        /// <param name="filename">The filename of xml file.</param>
        /// <returns>An array of <c>T</c> objects.</returns>
        /// <exception cref="FileNotFoundException">The specified filename cannot be found.</exception>
        public static T[] DeserializeCollection<T>(string filename)
        {
            if (!File.Exists(filename))
                throw new FileNotFoundException("The specified filename cannot be found.", filename);

            T[] collection;
            XmlSerializer xmlSerializer = new XmlSerializer(typeof (T[]));

            using (XmlTextReader xmlReader = new XmlTextReader(filename))
            {
                collection = (T[]) xmlSerializer.Deserialize(xmlReader);
            }

            return collection;
        }
    }
}