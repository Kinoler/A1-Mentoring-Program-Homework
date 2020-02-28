using System;

namespace Visitor.Enums
{
    [Flags]
    public enum SearchSettings
    {
        StopSearch = 0x01,
        ExcludeCurrentElement = 0x02
    }

}