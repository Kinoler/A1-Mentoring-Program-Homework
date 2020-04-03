using BCLLibrory;
using System;
using System.Configuration;

namespace BCL.Configuration.Elements
{
    public class RuleElement : ConfigurationElement
    {
        [ConfigurationProperty(nameof(Expression), IsRequired = true)]
        public string Expression => (string)this[nameof(Expression)];

        [ConfigurationProperty(nameof(Target), IsRequired = true)]
        public string Target => (string)this[nameof(Target)];

        [ConfigurationProperty(nameof(AddCreationTime), DefaultValue = false)]
        public bool AddCreationTime => (bool)this[nameof(AddCreationTime)];
        
        [ConfigurationProperty(nameof(AddSerialNumber), DefaultValue = false)]
        public bool AddSerialNumber => (bool)this[nameof(AddSerialNumber)];

        public OutputNameConfiguration NameConfiguration
        {
            get
            {
                OutputNameConfiguration outputName = OutputNameConfiguration.NoneModification;
                if (AddCreationTime)
                {
                    outputName |= OutputNameConfiguration.AddCreationTime;
                }

                if (AddSerialNumber)
                {
                    outputName |= OutputNameConfiguration.AddCreationTime;
                }

                return outputName;
            }
        }
    }

}
