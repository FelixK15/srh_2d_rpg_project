using Animation_Importer.XML_Objects;
using Microsoft.Xna.Framework.Content.Pipeline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Xml;

namespace Animation_Importer
{
    [ContentImporter(".xml",DisplayName="XML K15 Animation Importer")]
    public class AnimationImporter : ContentImporter<XMLAnimation>
    {
        public override XMLAnimation Import(string filename, ContentImporterContext context)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(XMLAnimation));
            XmlReader     reader     = XmlReader.Create(filename);
            XMLAnimation  animation  = (XMLAnimation)serializer.Deserialize(reader);

            return animation;
        }
    }
}
