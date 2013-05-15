using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content.Pipeline;
using System.Xml.Serialization;
using System.Xml;

namespace Dialog_Importer.XML_Objects
{
    [ContentImporter(".xml", DefaultProcessor = "DialogProcessor", DisplayName = "XML Dialog Importer")]
    class DialogImporter : ContentImporter<XMLDialogs>
    {
        public override XMLDialogs Import(string filename, ContentImporterContext context)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(XMLDialogs));
            XmlReader reader = XmlReader.Create(filename);
            XMLDialogs newDialogs = (XMLDialogs)serializer.Deserialize(reader);

            return newDialogs;
        }
    }
}
