using Domain.Model.Game;
using NUnit.Framework;
using Assert = UnityEngine.Assertions.Assert;

namespace Domain.Model.Tests
{
    public class WordTest
    {
        [TestCase("___S", false)]
        [TestCase("word", true)]
        public void WhenAskIfTheWordIsCompleted_ReturnTheCorrectValue(string word, bool expectedValue)
        {
            Assert.AreEqual(expectedValue, new Word(word).IsCompleted());
        }

    }
}
