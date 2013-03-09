using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GameWorld_Importer.XML_Objects
{
    [XmlRoot("tile")]
    [XmlInclude(typeof(XMLProperty))]
    public class XMLTile
    {
        [XmlAttribute("id",DataType="int")]
        public int ID { get; set; }

        [XmlArray("properties")]
        [XmlArrayItem("property")]
        public List<XMLProperty> Properties { get; set; }

        public XMLTile()
        {
            ID = -1;
        }
    }
}
