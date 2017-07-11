

namespace OriginalOrNot.FileTypesClasses
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Collections.Concurrent;
    public interface ITextTypeFile
    {
        /// <summary>
        /// First item is the new collection of referent words, the second is the total number of words
        /// </summary>
        /// <param name="path"></param>
        /// <param name="expectedConcurencyLevel"></param>
        /// <returns></returns>
        Tuple<ConcurrentDictionary<string, int>,int> LoadReferentText(string path,int expectedConcurencyLevel);
        /// <summary>
        /// First item is the array of all words to compare. The second is the count
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        Tuple<string[],int> LoadComparisonText(string path);
    }
}
