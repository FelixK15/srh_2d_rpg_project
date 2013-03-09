using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GameWorld_Importer.XML_Objects
{
    [XmlRoot("image")]
    public class XMLImage
    {
        [XmlAttribute("source",DataType="string")]
        public string ImageSource { get; set; }

        [XmlAttribute("trans",DataType="string")]
        public string Transparent { get; set; }

        [XmlAttribute("width",DataType="int")]
        public int Width { get; set; }

        [XmlAttribute("height", DataType="int")]
        public int Height { get; set; }

        public XMLImage()
        {
            Transparent = "";
        }
    }
}
