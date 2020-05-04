using System.Xml.Serialization;

namespace BasicSerialization.Enums
{
    public enum GenreType
    {
        [XmlEnum]
        Computer,
        [XmlEnum]
        Fantasy,
        [XmlEnum]
        Romance,
        [XmlEnum]
        Horror,
        [XmlEnum("Science Fiction")]
        ScienceFiction

    }
}
