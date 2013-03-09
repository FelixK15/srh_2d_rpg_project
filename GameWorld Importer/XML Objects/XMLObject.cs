using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GameWorld_Importer.XML_Objects
{
    [XmlRoot("object")]
    [XmlInclude(typeof(XMLProperty))]
    public class XMLObject
    {
        [XmlAttribute("x",DataType="int")]
        public int X { get; set; }

        [XmlAttribute("y", DataType = "int")]
        public int Y { get; set; }

        [XmlAttribute("width", DataType = "int")]
        public int Width { get; set; }

        [XmlAttribute("height", DataType = "int")]
        public int Height { get; set; }

        [XmlAttribute("gid", DataType = "int")]
        public int GID { get; set; }

        [XmlArray("properties")]
        [XmlArrayItem("property")]
        public List<XMLProperty> Properties { get; set; }

        public XMLObject()
        {
            X = Y = Width = Height = 0;
            GID = -1;
            Properties = new List<XMLProperty>();
        }
    }
}
