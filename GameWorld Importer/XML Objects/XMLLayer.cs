using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GameWorld_Importer.XML_Objects
{
    [XmlRoot("layer")]
    [XmlInclude(typeof(XMLProperty))]
   // [XmlInclude(typeof(XMLData))]
    public class XMLLayer
    {
        [XmlAttribute("name",DataType="string")]
        public string Name { get; set; }

        [XmlAttribute("width",DataType="int")]
        public int Width { get; set; }

        [XmlAttribute("height",DataType="int")]
        public int Height { get; set; }

        [XmlElement("data")]
       // public XMLData Data { get; set; }
        public string Data { get; set; }

        [XmlArray("properties")]
        [XmlArrayItem("property")]
        public List<XMLProperty> Properties { get; set; }

        public XMLLayer()
        {

        }

    }
}
