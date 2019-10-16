using CozyCommandLineParser.Utils;
using NUnit.Framework;

namespace CCLPTest
{
    [TestFixture]
    public class LetterCaseConverterTests
    {
        [Test]
        public void ConverterTest()
        {
            var words = "SeveralWordsHere";

            var wordsCamel = "severalWordsHere";
            Assert.AreEqual(wordsCamel, LetterCaseConverter.FromGenericCase(words, NameConventions.CamelCase));
            Assert.AreEqual(words, LetterCaseConverter.ToGenericCase(wordsCamel, NameConventions.CamelCase));

            var wordsLower = "severalwordshere";
            Assert.AreEqual(wordsLower, LetterCaseConverter.FromGenericCase(words, NameConventions.LowerCase));
            Assert.AreEqual(wordsLower, LetterCaseConverter.ToGenericCase(wordsLower, NameConventions.LowerCase));

            Assert.AreEqual(words, LetterCaseConverter.FromGenericCase(words, NameConventions.PascalCase));
            Assert.AreEqual(words, LetterCaseConverter.ToGenericCase(words, NameConventions.PascalCase));

            var wordsKebab = "several-words-here";
            Assert.AreEqual(wordsKebab, LetterCaseConverter.FromGenericCase(words, NameConventions.KebabCase));
            Assert.AreEqual(words, LetterCaseConverter.ToGenericCase(wordsKebab, NameConventions.KebabCase));

            var wordsSnake = "several_words_here";
            Assert.AreEqual(wordsSnake, LetterCaseConverter.FromGenericCase(words, NameConventions.SnakeCase));
            Assert.AreEqual(words, LetterCaseConverter.ToGenericCase(wordsSnake, NameConventions.SnakeCase));
        }
    }
}