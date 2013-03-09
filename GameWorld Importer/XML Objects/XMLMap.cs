using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GameWorld_Importer.XML_Objects
{
    [XmlRoot("map")]
    [XmlInclude(typeof(XMLProperty))]
    [XmlInclude(typeof(XMLTileSet))]
    [XmlInclude(typeof(XMLLayer))]
    [XmlInclude(typeof(XMLObjectGroup))]
    public class XMLMap
    {
        [XmlAttribute("version",DataType="string")]
        public string Version { get; set; }

        [XmlAttribute("orientation",DataType="string")]
        public string Orientation { get; set; }

        [XmlAttribute("width",DataType="int")]
        public int Width { get; set; }

        [XmlAttribute("height",DataType="int")]
        public int Height { get; set; }

        [XmlAttribute("tilewidth",DataType="int")]
        public int TileWidth { get; set; }

        [XmlAttribute("tileheight",DataType="int")]
        public int TileHeight { get; set; }

        [XmlArray("properties")]
        [XmlArrayItem("property")]
        public List<XMLProperty> Properties { get; set; }

        [XmlElement("tileset")]
        public List<XMLTileSet> TileSets { get; set; }

        [XmlElement("layer")]
        public List<XMLLayer> Layers { get; set; }

        [XmlElement("objectgroup")]
        public List<XMLObjectGroup> ObjectGroups { get; set; }

        public XMLMap()
        {

        }
    }
}
