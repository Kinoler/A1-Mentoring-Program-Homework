using System.Configuration;
using BCL.Configuration.Collections;
using BCL.Configuration.Elements;

namespace BCL.Configuration.Sections
{
    public class SimpleConfigurationSection : ConfigurationSection
    {
        [ConfigurationProperty("appName")]
        public string ApplicationName => (string)base["appName"];

        [ConfigurationProperty("workTime")]
        public WorkTimeElement WorkTime => (WorkTimeElement)this["workTime"];

        [ConfigurationProperty("files")]
        public FileElementCollection Files => (FileElementCollection)this["files"];
    }

}
