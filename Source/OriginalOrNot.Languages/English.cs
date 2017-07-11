using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace OriginalOrNot.Languages
{
    public class English : ILanguage
    {
        public ConcurrentDictionary<string, int> GetExcludedWordsForComparison()
        {
            var concurentDictionary = new ConcurrentDictionary<string, int>()
            { };
            return concurentDictionary;
        }
    }
}
