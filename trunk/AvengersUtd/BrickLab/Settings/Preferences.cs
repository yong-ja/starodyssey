using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using AvengersUtd.BrickLab.Data;
using AvengersUtd.BrickLab.Model;

namespace AvengersUtd.BrickLab.Settings
{
    [Serializable]
    public class Preferences
    {
        public Preferences() 
        {
        }

        [XmlElement]
        public string UserId { get; set; }

        [XmlElement]
        public string Password { get; set; }

        [XmlElement]
        public string ProxyAddress { get; set; }

        [XmlElement]
        public int ProxyPort { get; set; }

        [XmlElement]
        public Currency Currency { get; set; }

        public bool HasProxy
        {
            get { return !string.IsNullOrEmpty(ProxyAddress) && ProxyPort != 0; }
        }

        public bool HasAccountInfo
        {
            get { return !(string.IsNullOrEmpty(UserId) && string.IsNullOrEmpty(Password)); }
        }
    }
}
