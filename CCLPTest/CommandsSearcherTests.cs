using System.Reflection;
using AnotherCCLPTest;
using CozyCommandLineParser;
using NUnit.Framework;

namespace CCLPTest
{
    [TestFixture]
    public class CommandsSearcherTests
    {
        [Test]
        public void SearchTest()
        {
            var defaultAssembly = Assembly.GetExecutingAssembly();

            var options = new ParserOptions();
            var types = CommandsSearcher.FindAllTypes(options, defaultAssembly);
            Assert.AreEqual(new[] {typeof(AnotherTestCommands), typeof(TestCommands)}, types);

            options = new ParserOptions() {SearchFilterByNamespaces = new[] {"AnotherCCLPTest"}};
            types = CommandsSearcher.FindAllTypes(options, defaultAssembly);
            Assert.AreEqual(new[] {typeof(AnotherTestCommands)}, types);

            options = new ParserOptions() {SearchInTypes = new[] {typeof(TestCommands)}};
            types = CommandsSearcher.FindAllTypes(options, defaultAssembly);
            Assert.AreEqual(new[] {typeof(TestCommands)}, types);
        }
    }
}