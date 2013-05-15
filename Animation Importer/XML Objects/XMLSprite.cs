using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Animation_Importer.XML_Objects
{
    [XmlRoot("Sprite")]
    public class XMLSprite
    {
        [XmlAttribute("X",DataType="int")]
        public int X { get; set; }

        [XmlAttribute("Y", DataType = "int")]
        public int Y { get; set; }

        [XmlAttribute("ID", DataType = "int")]
        public int ID { get; set; }

        public XMLSprite()
        {
            ID = -1;
        }
    }
}
