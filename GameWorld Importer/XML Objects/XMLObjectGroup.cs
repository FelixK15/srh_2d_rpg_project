using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GameWorld_Importer.XML_Objects
{
    [XmlRoot("objectgroup")]
    [XmlInclude(typeof(XMLProperty))]
    [XmlInclude(typeof(XMLObject))]
    public class XMLObjectGroup
    {
        [XmlAttribute("name",DataType="string")]
        public string Name { get; set; }

        [XmlAttribute("width",DataType="int")]
        public int Width { get; set; }

        [XmlAttribute("height", DataType = "int")]
        public int Height { get; set; }

        [XmlElement("object")]
        public List<XMLObject> Objects { get; set; }
        
        [XmlArray("properties")]
        [XmlArrayItem("property")]
        public List<XMLProperty> Properties { get; set; }

        public XMLObjectGroup()
        {

        }
    }
}
