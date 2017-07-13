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
    using OriginalOrNot.FileTypesClasses;

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
            
            var fileType = FileFactory.GetFileClass(format); 
            var tupleResult = fileType.LoadReferentText(filePath, _expectedConcurancyLevel);
            this._internalReferentTextCollection = tupleResult.Item1;
            this._totalWordsCount = tupleResult.Item2;
            return tupleResult.Item2;
        }
        /// <summary>
        /// Load the comparison words. Return the count of all words
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public int LoadComparisonText(string filePath,FileFormat format)
        {
            var fileType = FileFactory.GetFileClass(format);
            var tupleResult = fileType.LoadComparisonText(filePath);
            this._comparisonWordsCollection = tupleResult.Item1;
            return tupleResult.Item2;
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
        /// Perform compare between two text and return the percentage of equal words. Creates a .docx file with all equal words
        /// in both texts and seve it in the given path
        /// </summary>
        /// <param name="language"></param>
        /// <param name="intersectionFilePath"></param>
        /// <returns></returns>
        public double CompareAndIntersectTheTwoTexts(Language language, string intersectionFilePath)
        {
            var lanuageFactory = new LanguageFactory();
            var excludedWords = lanuageFactory.GetLanguage(language).GetExcludedWordsForComparison();
            if (this._comparisonWordsCollection == null || this._internalReferentTextCollection == null)
            {
                throw new InvalidOperationException("One or more file is not added, and comparison cannot happened");
            }
            int equalWords = this.CompareAndIntersectTheTwoSetsInNewFile(excludedWords,intersectionFilePath);
            double percents = (equalWords / (double)this._totalWordsCount) * 100d;
            return percents;
        }
        public void UnloadReferentText()
        {
            this._internalReferentTextCollection = new ConcurrentDictionary<string, int>();
        }
        public void UnloadComparisonText()
        {
            this._comparisonWordsCollection = new string[1];
        }
        private int CompareAndGetNumberOfEqualWord(ConcurrentDictionary<string,int> excludedWords)
        {
            int equalWords = 0;
            Parallel.ForEach(this._comparisonWordsCollection, word =>
            {
                if (this._internalReferentTextCollection.ContainsKey(word)
                && ! excludedWords.ContainsKey(word))
                {
                    if(this._internalReferentTextCollection[word] > 0)
                    {
                        equalWords++;
                        this._internalReferentTextCollection[word]--;
                    }
                }
            });
            return equalWords;
        }
        private int CompareAndIntersectTheTwoSetsInNewFile(ConcurrentDictionary<string, int> excludedWords, string newFilePath)
        {
            int equalWords = 0;
            newFilePath += @"\EqualWords.docx";
            StringBuilder sb = new StringBuilder();
            const int  wordsCountPerParagraph = 15;
            int  wordsCountForThisParagraf = 0;
            using (var docX = DocX.Create(newFilePath))
            {
                Parallel.ForEach(this._comparisonWordsCollection, word =>
                {
                    if (this._internalReferentTextCollection.ContainsKey(word)
                    && !excludedWords.ContainsKey(word))
                    {
                        if (this._internalReferentTextCollection[word] > 0)
                        {
                            lock(docX)
                            {
                                var lockingObject = new Object();
                                equalWords++;
                                this._internalReferentTextCollection[word]--;
                                sb.Append(word);
                                sb.Append(" ");
                                wordsCountForThisParagraf++;
                                if (wordsCountForThisParagraf >= wordsCountPerParagraph)
                                {
                                    docX.InsertParagraph(sb.ToString());
                                    sb.Clear();
                                    wordsCountForThisParagraf = 0;
                                }
                            }
                        }
                    }
                });
                docX.Save();
                return equalWords;
            }
            
            
            
        }
        

    }
}
