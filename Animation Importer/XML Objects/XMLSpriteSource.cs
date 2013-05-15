using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Animation_Importer.XML_Objects
{
    [XmlRoot("SingleSpriteSource")]
    public class XMLSpriteSource
    {
        [XmlAttribute("Path",DataType="string")]
        public string   Path { get; set; }

        [XmlAttribute("ID",DataType="int")]
        public int      ID   { get; set; }

        public XMLSpriteSource()
        {
            
        }
    }
}
