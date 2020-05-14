using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace BasicSerialization.Models
{
    [Serializable]
    [XmlType(TypeName = "catalog", IncludeInSchema = true)]
    public class Catalog
    {
        [XmlAttribute("date")]
        public DateTime Date { get; set; }

        [XmlElement("book")]
        public List<Book> Books { get; set; }
    }
}
