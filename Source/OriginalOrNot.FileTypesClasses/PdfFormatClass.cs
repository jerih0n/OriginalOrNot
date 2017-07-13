

namespace OriginalOrNot.FileTypesClasses
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using iTextSharp.text;
    using iTextSharp.text.pdf;
    using iTextSharp.text.pdf.parser;

    public class PdfFormatClass : ITextTypeFile
    {
        public Tuple<string[], int> LoadComparisonText(string path)
        {
            using(var reader = new PdfReader(path))
            {
                var builder = new StringBuilder();
                var numberOfPages = reader.NumberOfPages;
                Parallel.For(1, numberOfPages, currentPage =>
                  {
                      lock(builder)
                      {
                          ITextExtractionStrategy strategy = new SimpleTextExtractionStrategy();
                          builder.Append(PdfTextExtractor.GetTextFromPage(reader, currentPage));
                      }
                  });
                var allWords = builder.ToString().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                return new Tuple<string[], int>(allWords, allWords.Length);
            }            
        }

        public Tuple<ConcurrentDictionary<string, int>, int> LoadReferentText(string path, int expectedConcurencyLevel)
        {
            using (var reader = new PdfReader(path))
            {
                var builder = new StringBuilder();
                var numberOfPages = reader.NumberOfPages;
                Parallel.For(1, numberOfPages, currentPage =>
                {
                    lock (builder)
                    {
                        ITextExtractionStrategy strategy = new SimpleTextExtractionStrategy();
                        builder.Append(PdfTextExtractor.GetTextFromPage(reader, currentPage));
                    }
                });
                var allWords = builder.ToString().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                var concurentDictionary = new ConcurrentDictionary<string, int>(expectedConcurencyLevel,allWords.Length);
                Parallel.ForEach(allWords, word =>
                 {
                     if(concurentDictionary.ContainsKey(word))
                     {
                         concurentDictionary[word]++;
                     }
                     else
                     {
                         concurentDictionary.AddOrUpdate(word, 1, (key, value) => value++);
                     }
                 });
                return new Tuple<ConcurrentDictionary<string, int>, int>(concurentDictionary, allWords.Length);
            }
        }
    }
}
