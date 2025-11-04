using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mush.AppLayer.Ports
{
    public interface ITextService
    {
        string CurrentLanguage { get; }
        event EventHandler? LanguageChanged;
        string T(string key);
        void SetLanguage(string lang);
    }
}
