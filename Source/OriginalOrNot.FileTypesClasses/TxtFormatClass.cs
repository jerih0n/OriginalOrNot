

namespace OriginalOrNot.FileTypesClasses
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    public class TxtFormatClass : ITextTypeFile
    {

        public Tuple<string[], int> LoadComparisonText(string path)
        {
            using (var reader = new StreamReader(path))
            {
                var allWords = reader.ReadToEnd()
                     .Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                return new Tuple<string[], int>(allWords, allWords.Length);
            }
        }
        public Tuple<ConcurrentDictionary<string, int>, int> LoadReferentText(string path, int expectedConcurencyLevel)
        {
            using (var reader = new StreamReader(path))
            {
                var allWords = reader.ReadToEnd()
                    .Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                var referentCollecton = new ConcurrentDictionary<string, int>(expectedConcurencyLevel, allWords.Length);
                Parallel.ForEach(allWords, word =>
                {
                    if (referentCollecton.ContainsKey(word))
                    {
                        referentCollecton[word]++;
                    }
                    else
                    {
                        referentCollecton.AddOrUpdate(word, 1, (key, value) => value++);
                    }
                });
                return new Tuple<ConcurrentDictionary<string, int>, int>(referentCollecton, allWords.Length);
            }
        }

        
    }
}
