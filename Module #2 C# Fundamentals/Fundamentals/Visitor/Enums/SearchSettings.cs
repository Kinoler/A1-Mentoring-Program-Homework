using System;

namespace Visitor.Enums
{
    [Flags]
    public enum SearchSettings
    {
        None = 0x00,
        StopSearch = 0x01,
        ExcludeCurrentElement = 0x02
    }
}