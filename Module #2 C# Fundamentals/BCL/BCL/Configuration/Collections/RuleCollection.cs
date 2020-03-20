using System.Configuration;
using BCL.Configuration.Elements;

namespace BCL.Configuration.Collections
{
    public class RuleCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement() => new RuleElement();

        protected override object GetElementKey(ConfigurationElement element) => $"{((RuleElement)element).Expression} - {((RuleElement)element).Target}";
    }

}
