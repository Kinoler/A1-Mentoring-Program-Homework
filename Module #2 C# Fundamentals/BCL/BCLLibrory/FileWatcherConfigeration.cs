using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCLLibrory
{
    public class FileWatcherConfigeration
    {
        private List<Rule> _setOfRules;
        private List<string> _watcherFolders;
        private string _defaultFolder;
        private CultureInfo _cultureInfo;

        public FileWatcherConfigeration(List<string> watcherFolders,
            string defaultFolder, 
            List<Rule> setofRules,
            CultureInfo cultureInfo)
        {
            _setOfRules = setofRules ?? throw new ArgumentNullException(nameof(setofRules));
            _watcherFolders = watcherFolders ?? throw new ArgumentNullException(nameof(watcherFolders));
            _defaultFolder = defaultFolder ?? throw new ArgumentNullException(nameof(defaultFolder));
            _cultureInfo = cultureInfo ?? throw new ArgumentNullException(nameof(cultureInfo));
        }
    }
}
