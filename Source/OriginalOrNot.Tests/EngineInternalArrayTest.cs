

namespace OriginalOrNot.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using OriginalOrNot.Logic;
    /// <summary>
    /// This class contains tests regarding, the proper creation of the internal dictionary of the Engine.
    /// </summary>
    [TestClass]
    
    public class EngineInternalArrayTest
    {
        [TestMethod]
        public void TestWordsCountWithDefaultParametersShouldReturn300()
        {
            var pageCount = 1;
            var fontSize = 12;
            var expectedAnswer = 300;
            var engine = new Engine(pageCount, fontSize);
            Assert.AreEqual(expectedAnswer, engine.ApproximateReferentWordsCount);
        }
        [TestMethod]
        public void TestWordsCountWithDefaultFontSizeAnd10PagesShouldReturn3000()
        {
            var pageCount = 10;
            var fontSize = 12;
            var expectedAnswer = 3000;
            var engine = new Engine(pageCount, fontSize);
            Assert.AreEqual(expectedAnswer, engine.ApproximateReferentWordsCount);
        }
        [TestMethod]
        public void TestWordsWith10FontSizeAnd10PagesShouldRetur3480()
        {
            var pageCount = 10;
            var fontSize = 10;
            var expectedAnswer = 3500;
            var engine = new Engine(pageCount, fontSize);
            Assert.AreEqual(expectedAnswer, engine.ApproximateReferentWordsCount);
        }
        [TestMethod]
        public void TestWordsWith14FontSizeAnd10PagesShouldReturn2520()
        {
            var pageCount = 10;
            var fontSize = 14;
            var expectedAnswer = 2500;
            var engine = new Engine(pageCount, fontSize);
            Assert.AreEqual(expectedAnswer, engine.ApproximateReferentWordsCount);
        }

    }
}
