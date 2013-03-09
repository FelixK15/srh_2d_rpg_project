using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GameWorld_Importer.XML_Objects
{
    [XmlRoot("property")]
    public class XMLProperty
    {
        [XmlAttribute("name",DataType="string")]
        public string Name { get; set; }

        [XmlAttribute("value", DataType = "string")]
        public string Value { get; set; }

        public XMLProperty()
        {

        }
    }
}
