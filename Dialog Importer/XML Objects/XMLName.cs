using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Dialog_Importer.XML_Objects
{
    [XmlRoot("Name")]

    public class XMLName
    {
         [XmlElement]
        public string name { get; set; }
        public XMLName()
        {

        }
    }
}
