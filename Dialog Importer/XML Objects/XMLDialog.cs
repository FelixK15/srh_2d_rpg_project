using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Dialog_Importer.XML_Objects
{
    [XmlRoot("Dialog")]
    [XmlInclude(typeof(XMLNode))]
    public class XMLDialog
    {
        [XmlAttribute("ID", DataType = "int")]
        public int DialogID { get; set; }

        [XmlElement("Node")]
        public List<XMLNode> DialogNodes { get; set; }

        public XMLDialog()
        {

        }
    }
}
