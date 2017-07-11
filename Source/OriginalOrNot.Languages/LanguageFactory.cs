using System;
using System.Collections.Generic;
using System.Text;
using OriginalOrNot.Shared;
namespace OriginalOrNot.Languages
{
    public class LanguageFactory
    {
        private Dictionary<Language, ILanguage> _supportedLanguges = new Dictionary<Language, ILanguage>()
        {
            { Language.English, new English() }
        };
        public ILanguage GetLanguage(Language language)
        {
            return this._supportedLanguges[language];
        }
    }
}
