using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using BasicSerialization.Models;

namespace BasicSerialization
{
    public class Program
    {
        public const string XmlFrom = "books.xml";
        public const string XmlTo = "booksSerialized.xml";

        public static void Main()
        {
            var serializer = new XmlSerializer(typeof(Catalog), "http://library.by/catalog");
            Catalog deserializedCatalog = null;

            using (var fileStream = File.OpenRead(XmlFrom))
            {
                deserializedCatalog = (Catalog)serializer.Deserialize(fileStream);
            }

            using (var fileStream = File.OpenWrite(XmlTo))
            {
                serializer.Serialize(fileStream, deserializedCatalog);
            }
        }
    }
}
