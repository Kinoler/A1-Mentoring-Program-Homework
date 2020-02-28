using System;
using Visitor.Enums;

namespace Visitor.EventHandler
{
    public class ElementFound : EventArgs
    {
        public string Path { get; set; }

        public SearchSettings Flag { get; set; }
    }
}