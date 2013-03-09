using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GameWorld_Importer.XML_Objects
{
    [XmlRoot("data")]
    public class XMLData
    {
        [XmlAttribute("encoding",DataType="string")]
        public string Encoding { get; set; }

        [XmlAttribute("compression",DataType="string")]
        public string Compression { get; set; }

        [XmlElement]
        public string EncodedData { get; set; }

        public XMLData()
        {

        }
    }
}
