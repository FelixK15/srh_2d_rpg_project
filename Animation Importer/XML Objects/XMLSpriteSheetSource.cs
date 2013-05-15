using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Animation_Importer.XML_Objects
{
    [XmlInclude(typeof(XMLSpriteSource))]
    [XmlRoot("SpriteSheetSource")]
    public class XMLSpriteSheetSource : XMLSpriteSource
    {
        [XmlAttribute("SingleSpriteWidth",DataType="int")]
        public int SingleSpriteWith { get; set; }

        [XmlAttribute("SingleSpriteHeight",DataType="int")]
        public int SingleSpriteHeight { get; set; }

        [XmlAttribute("SpriteAmount",DataType="int")]
        public int SpriteAmount { get; set; }

        public XMLSpriteSheetSource()
        {

        }
    }
}
