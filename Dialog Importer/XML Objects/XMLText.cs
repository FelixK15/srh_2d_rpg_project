﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Dialog_Importer.XML_Objects
{
    [XmlRoot("Text")]
   
    public class XMLText
    {
        [XmlElement]
        public string text { get; set; }
        public XMLText()
        {

        }
    }
}
