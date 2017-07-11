

namespace OriginalOrNot.Languages
{
    using System.Collections.Generic;
    public interface ILanguage
    {
        ISet<string> GetExcludedWordsForComparison();
    }
}
