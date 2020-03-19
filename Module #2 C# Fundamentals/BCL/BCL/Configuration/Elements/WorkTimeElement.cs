using System;
using System.Configuration;

namespace BCL.Configuration.Elements
{
    public class WorkTimeElement : ConfigurationElement
    {
        [ConfigurationProperty("start")]
        public DateTime StartTime => (DateTime)this["start"];

        [ConfigurationProperty("duration")]
        public TimeSpan Duration => (TimeSpan)this["duration"];
    }

}
