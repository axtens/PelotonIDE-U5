using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PelotonIDE.Presentation
{
    public class TranslateToMainParams
    {
        public TranslateToMainParams()
        {
            selectedLangauge = 0;
        }
        public RichEditBox? translatedREB { get; set; }
        public int selectedLangauge { get; set; }
    }
}
