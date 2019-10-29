using System;
using System.Collections.Generic;
using System.Reflection;
using AnotherCCLPTest;
using CozyCommandLineParser;
using NUnit.Framework;

namespace CCLPTest
{
    public class CommandsSearcherTests
    {
        [Test]
        public void SearchTest()
        {
            Assembly defaultAssembly = Assembly.GetExecutingAssembly();

            var options = new ParserOptions();
            List<Type> types = CommandsSearcher.FindAllTypes(options, defaultAssembly);
            Assert.AreEqual(
                new[] {typeof(AnotherTestCommands), typeof(AnotherTestCommandsDerived), typeof(TestCommands)},
                types);

            options = new ParserOptions {SearchFilterByNamespaces = new[] {"AnotherCCLPTest"}};
            types = CommandsSearcher.FindAllTypes(options, defaultAssembly);
            Assert.AreEqual(new[] {typeof(AnotherTestCommands), typeof(AnotherTestCommandsDerived)}, types);

            options = new ParserOptions {SearchInTypes = new[] {typeof(TestCommands)}};
            types = CommandsSearcher.FindAllTypes(options, defaultAssembly);
            Assert.AreEqual(new[] {typeof(TestCommands)}, types);
        }
    }
}