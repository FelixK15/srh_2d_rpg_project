using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Animation_Importer.XML_Objects
{
    [XmlInclude(typeof(XMLSpriteSource))]
    [XmlInclude(typeof(XMLSpriteSheetSource))]
    [XmlInclude(typeof(XMLFrame))]

    [XmlRoot("Animation")]
    public class XMLAnimation
    {
        [XmlAttribute("Loop",DataType="string")]
        public string Loop { get; set; }

        [XmlArray("SpriteSources")]
        [XmlArrayItem("SpriteSheetSource",typeof(XMLSpriteSheetSource)),
         XmlArrayItem("SingleSpriteSource",typeof(XMLSpriteSource))]
        public List<XMLSpriteSource> SpriteSources { get; set; }

        [XmlArray("Frames")]
        [XmlArrayItem("Frame")]
        public List<XMLFrame> Frames { get; set; }

        public XMLAnimation()
        {
            Loop = "false";
        }
    }
}
