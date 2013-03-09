using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GameWorld_Importer.XML_Objects
{
    [XmlRoot("tileset")]
    [XmlInclude(typeof(XMLProperty))]
    [XmlInclude(typeof(XMLImage))]
    [XmlInclude(typeof(XMLTile))]
    public class XMLTileSet
    {
        [XmlAttribute("firstgid",DataType="int")]
        public int FirstGID { get; set; }

        [XmlAttribute("name",DataType="string")]
        public string Name { get; set; }

        [XmlAttribute("tilewidth",DataType="int")]
        public int TileWidth { get; set; }

        [XmlAttribute("tileheight", DataType = "int")]
        public int TileHeight { get; set; }

        [XmlElement("image")]
        public XMLImage Image { get; set; }

        [XmlArray("properties")]
        [XmlArrayItem("property")]
        public List<XMLProperty> Properties { get; set; }

        [XmlElement("tile")]
        public List<XMLTile> Tiles { get; set; }

        public XMLTileSet()
        {

        }
    }
}
