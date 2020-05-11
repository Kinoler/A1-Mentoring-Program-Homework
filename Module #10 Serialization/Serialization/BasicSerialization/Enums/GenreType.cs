using System;
using System.Xml.Serialization;

namespace BasicSerialization.Enums
{
    [Serializable]
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
