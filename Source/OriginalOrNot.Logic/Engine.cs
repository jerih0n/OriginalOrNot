namespace OriginalOrNot.Logic
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using OriginalOrNot.Shared;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Collections.Concurrent;
    using Novacode;
    using OriginalOrNot.Languages;
    public class Engine
    {
        private const int _standartFontSize = 12;
        private const int _approximateWordsPerPageWithDefaultFont = 300;
        private ConcurrentDictionary<string, int> _internalReferentTextCollection;       
        private const int _expectedConcurancyLevel = 8;
        private string[] _comparisonWordsCollection;
        private int _totalWordsCount = 0;
        public Engine()
        {
           
        }
        /// <summary>
        /// Load the text, and store all words. Return the total amount of words in the text
        /// </summary>
        public int LoadReferentText(string filePath,FileFormat format)
        {
            int totalWord = 0;
            switch(format)
            {
                case FileFormat.TextFile:
                    totalWord = this.LoadReferentTextFromTxtFile(filePath);
                    return totalWord;
                case FileFormat.DocXFormat:
                    totalWord =  this.LoadReferentTextFromDocXFile(filePath);
                    return totalWord;
                default:
                    return totalWord;
            }
        }
        public int LoadComparisonText(string filePath,FileFormat format)
        {
            int totalWordsToCompare = 0;
            switch(format)
            {
                case FileFormat.TextFile:
                    totalWordsToCompare = this.LoadComparisonTextFromTxtFile(filePath);
                    return totalWordsToCompare;
                case FileFormat.DocXFormat:
                    totalWordsToCompare = this.LoadComparisonTextFormDocXFile(filePath);
                    return totalWordsToCompare;
                default:
                    return totalWordsToCompare;

            }
        }
        /// <summary>
        /// Perform compare between two text and return the percentage of equal words.
        /// </summary>
        /// <returns></returns>
        public double CompareFiles(Language language)
        {
            var lanuageFactory = new LanguageFactory();
            var excludedWords = lanuageFactory.GetLanguage(language).GetExcludedWordsForComparison();
            if(this._comparisonWordsCollection == null || this._internalReferentTextCollection == null)
            {
                throw new InvalidOperationException("One or more file is not added, and comparison cannot happened");
            }
            int equalWords = this.CompareAndGetNumberOfEqualWord(excludedWords);
            double percents = (equalWords / (double)this._totalWordsCount) * 100d;
            return percents;
        }
        /// <summary>
        /// Calculate the app
        /// </summary>
        /// <param name="documentPagesCount"></param>
        /// <param name="fontsSize"></param>
        /// <returns></returns>
        
        private  int LoadReferentTextFromTxtFile(string filePath)
        {
            using(var reader = new StreamReader(filePath))
            {
                var allWords = reader.ReadToEnd()
                    .Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                this._internalReferentTextCollection = new ConcurrentDictionary<string, int>(_expectedConcurancyLevel, allWords.Length);
                Parallel.ForEach(allWords, word =>
                 {
                    
                    if (this._internalReferentTextCollection.ContainsKey(word))
                    {
                        this._internalReferentTextCollection[word]++;
                    }
                    else
                    {
                        this._internalReferentTextCollection.AddOrUpdate(word, 1, (key, value) => value++);
                    }
                     
                 });
                this._totalWordsCount = allWords.Length;
                return this._totalWordsCount;
            }
        }
        /// <summary>
        /// Load referent words from a DocX file
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private int LoadReferentTextFromDocXFile(string filePath)
        {
            using(var doc = DocX.Load(filePath))
            {
                var allWords = doc.Text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                this._internalReferentTextCollection = new ConcurrentDictionary<string, int>(_expectedConcurancyLevel, allWords.Length);
                Parallel.ForEach(allWords, word =>
                 {
                     if(this._internalReferentTextCollection.ContainsKey(word))
                     {
                         this._internalReferentTextCollection[word]++;
                     }
                     else
                     {
                         this._internalReferentTextCollection.AddOrUpdate(word, 1, (key, value) => value++);
                     }
                 });
                this._totalWordsCount = allWords.Length;
                return this._totalWordsCount;
            }
        }
        private int LoadComparisonTextFromTxtFile(string filePath)
        {
            using(var reader = new StreamReader(filePath))
            {
                this._comparisonWordsCollection = reader.ReadToEnd()
                     .Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                return this._comparisonWordsCollection.Length;
            }
        }
        private int LoadComparisonTextFormDocXFile(string filePath)
        {
            using(var reader = DocX.Load(filePath))
            {
                this._comparisonWordsCollection = reader.Text
                       .Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                return this._comparisonWordsCollection.Length;
            }
        }
        private int CompareAndGetNumberOfEqualWord(ConcurrentDictionary<string,int> excludedWords)
        {
            int equalWords = 0;
            Parallel.ForEach(this._comparisonWordsCollection, word =>
            {
                if (this._internalReferentTextCollection.ContainsKey(word)
                && ! excludedWords.ContainsKey(word))
                {
                    equalWords++;
                }
            });
            return equalWords;
        }
        

    }
}
