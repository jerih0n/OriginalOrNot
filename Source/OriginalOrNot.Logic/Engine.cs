namespace OriginalOrNot.Logic
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using OriginalOrNot.Shared;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;
    public class Engine
    {
        private const int _standartFontSize = 12;
        private const int _approximateWordsPerPageWithDefaultFont = 300;
        private Dictionary<string, int> _internalReferentTextCollection;
        private int _approximateWordsCountInTheReferentText;
        public Engine(int documentPagesCount, int fontSize)
        {
            //The goal is to avoid the amortized O(N) complexity
            this._approximateWordsCountInTheReferentText = this.CalculateTheApproximateAmountOfWords(documentPagesCount, fontSize);
            this._internalReferentTextCollection = new Dictionary<string, int>(this._approximateWordsCountInTheReferentText);
        }
        public int ApproximateReferentWordsCount { get { return this._approximateWordsCountInTheReferentText; } }
        /// <summary>
        /// Load the text, and store all words. Return the total amount of words in the text
        /// </summary>
        public int LoadReferentText(string filePath,FileFormat format)
        {
            int totalWord = 0;
            switch(format)
            {
                case FileFormat.TextFile: this.LoadTextFile(filePath);
                    return totalWord;
                case FileFormat.DocXFormat:
                    return totalWord;
            }
            return 0;
        }
        /// <summary>
        /// Calculate the app
        /// </summary>
        /// <param name="documentPagesCount"></param>
        /// <param name="fontsSize"></param>
        /// <returns></returns>
        private int CalculateTheApproximateAmountOfWords(int documentPagesCount, int fontsSize)
        {
            int approximateWordsPerPage = _approximateWordsPerPageWithDefaultFont;
            if(fontsSize == _standartFontSize)
            {
                return documentPagesCount * approximateWordsPerPage;
            }
            else if(fontsSize > _standartFontSize)
            {
                // lesser words
                double proportion = (double)((fontsSize - _standartFontSize) / (decimal)_standartFontSize) ;
                approximateWordsPerPage = _approximateWordsPerPageWithDefaultFont - (int)(approximateWordsPerPage * proportion);
                return approximateWordsPerPage * documentPagesCount;
            }
            else
            {
                // more words
                double proportion = (double)((_standartFontSize - fontsSize) / (decimal)_standartFontSize);
                approximateWordsPerPage = _approximateWordsPerPageWithDefaultFont + (int)(approximateWordsPerPage * proportion);
                return (approximateWordsPerPage * documentPagesCount);
            }
        }
        private  int LoadTextFile(string filePath)
        {
            using(var reader = new StreamReader(filePath))
            {
                var allWords = reader.ReadToEnd()
                    .Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                Parallel.ForEach(allWords, word =>
                 {
                     if(this._internalReferentTextCollection.ContainsKey(word))
                     {
                         this._internalReferentTextCollection[word]++;
                     }
                     else
                     {
                         this._internalReferentTextCollection.Add(word, 1);
                     }
                 });
                return allWords.Length;
            }
        }

    }
}
