using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
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
        public Currency Currency { get; set; }
    }
}
