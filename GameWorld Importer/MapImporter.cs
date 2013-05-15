using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content.Pipeline;
using System.Xml.Serialization;
using System.Xml;

namespace GameWorld_Importer.XML_Objects
{
    [ContentImporter(".tmx",DisplayName="TMX Map Importer")]
    class MapImporter : ContentImporter<XMLMap>
    {
        public override XMLMap Import(string filename, ContentImporterContext context)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(XMLMap));
            XmlReader reader = XmlReader.Create(filename);
            XMLMap newMap = (XMLMap)serializer.Deserialize(reader);

            return newMap;
        }
    }
}
