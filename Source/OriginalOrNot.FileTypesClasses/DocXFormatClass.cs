

namespace OriginalOrNot.FileTypesClasses
{
    using Novacode;
    using System;
    using System.Collections.Concurrent;
    using System.Threading.Tasks;

    public class DocXFormatClass : ITextTypeFile
    {
        

        public Tuple<string[], int> LoadComparisonText(string path)
        {
            using (var reader = DocX.Load(path))
            {
                
                var allWords = reader.Text
                       .Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                return new Tuple<string[], int>(allWords, allWords.Length);
            }
        }

        public int LoadReferentText(ConcurrentDictionary<string, int> referentCollecton, string path, int expectedConcurencyLevel)
        {
            using (var doc = DocX.Load(path))
            {
                var allWords = doc.Text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                referentCollecton = new ConcurrentDictionary<string, int>(expectedConcurencyLevel, allWords.Length);
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
                return allWords.Length;
            }
        }

        public Tuple<ConcurrentDictionary<string, int>, int> LoadReferentText(string path, int expectedConcurencyLevel)
        {
            using (var doc = DocX.Load(path))
            {
                var allWords = doc.Text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
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
