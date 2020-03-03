using System;
using Visitor.Enums;

namespace Visitor.EventHandler
{
    public class ElementFoundEventArgs : EventArgs
    {
        public string Path { get; internal set; }

        public SearchSettings SearchSettings { get; set; }

        public ElementType ElementType { get; internal set; }
    }
}