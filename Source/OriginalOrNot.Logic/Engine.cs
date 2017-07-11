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

    public class Engine
    {
        private const int _standartFontSize = 12;
        private const int _approximateWordsPerPageWithDefaultFont = 300;
        private ConcurrentDictionary<string, int> _internalReferentTextCollection;       
        private const int _expectedConcurancyLevel = 8;
        private string[] _comparisonWordsCollection;
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
                    totalWord = this.LoadTextFile(filePath);
                    return totalWord;
                case FileFormat.DocXFormat:
                    totalWord =  this.LoadDocXFile(filePath);
                    return totalWord;
                default:
                    return totalWord;
            }
        }
        /// <summary>
        /// Calculate the app
        /// </summary>
        /// <param name="documentPagesCount"></param>
        /// <param name="fontsSize"></param>
        /// <returns></returns>
        
        private  int LoadTextFile(string filePath)
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
                return allWords.Length;
            }
        }
        /// <summary>
        /// Load referent words from a DocX file
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private int LoadDocXFile(string filePath)
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
                return this._comparisonWordsCollection.Length;
            }
            
        }
        

    }
}
