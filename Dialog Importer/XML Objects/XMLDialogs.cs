using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Dialog_Importer.XML_Objects
{
    [XmlRoot("Dialogs")]
    [XmlInclude(typeof(XMLDialog))]
    public class XMLDialogs
    {  
        [XmlElement("Dialog")]
        public List<XMLDialog> DialogList { get; set; }

        public XMLDialogs()
        {

        }
    }
}

