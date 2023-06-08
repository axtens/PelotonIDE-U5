using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace PelotonIDE.Presentation
{
    public partial class CustomTabItem : NavigationViewItem
    {
        public bool IsNewFile { get; set; }

        public StorageFile? SavedFilePath { get; set; }

        public CustomTabItem()
        {
            SavedFilePath = null;
        }
    }
}
