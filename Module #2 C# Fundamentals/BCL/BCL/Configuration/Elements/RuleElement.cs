using BCLLibrory;
using System;
using System.Configuration;

namespace BCL.Configuration.Elements
{
    public class RuleElement : ConfigurationElement
    {
        [ConfigurationProperty("Expression")]
        public string Expression => (string)this["Expression"];

        [ConfigurationProperty("Target")]
        public string Target => (string)this["Target"];

        [ConfigurationProperty("NameConfiguration")]
        public OutputNameConfiguration NameConfiguration => (OutputNameConfiguration)this["NameConfiguration"];
    }

}
