using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Animation_Importer.XML_Objects
{
    [XmlInclude(typeof(XMLSprite))]

    [XmlRoot("Frame")]
    public class XMLFrame
    {
        [XmlAttribute("Duration",DataType="int")]
        public int Duration { get; set; }

        [XmlArray("Sprites")]
        [XmlArrayItem("Sprite")]
        public List<XMLSprite> Sprites { get; set; }

        public XMLFrame()
        {
            Sprites = new List<XMLSprite>();
        }
    }
}
