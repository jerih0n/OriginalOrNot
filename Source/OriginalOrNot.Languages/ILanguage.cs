

namespace OriginalOrNot.Languages
{
    using System.Collections.Generic;
    using System.Collections.Concurrent;
    public interface ILanguage
    {
        ConcurrentDictionary<string,int> GetExcludedWordsForComparison();
    }
}
