using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Dialog_Importer.XML_Objects
{
    [XmlRoot("Node")]
    [XmlInclude(typeof(XMLText))]
    [XmlInclude(typeof(XMLName))]
    public class XMLNode
    {
        [XmlAttribute("ID", DataType = "int")]
        public int NodeID { get; set; }

        [XmlElement("Text")]
        public string text { get; set; }

        [XmlElement("Name")]
        public string name { get; set; }

        public XMLNode()
        {

        }
    }
}
