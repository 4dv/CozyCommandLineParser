using CozyCommandLineParser;
using NUnit.Framework;

namespace CCLPTest
{
    [TestFixture]
    public class Tests
    {
        [Test]
        public void Test1()
        {
            var commandLine = new CommandLine();

            commandLine.Execute(new[] {"commandWithArg", "--stringOption='abcde'", "--intOption=42", "-b", "posArg"});
            var instance = ((TestCommands) commandLine.LastCommandInstance);

            Assert.AreEqual("abcde", instance.StringOption);
            Assert.AreEqual(42, instance.IntOption);
            Assert.IsTrue(instance.BoolOption);

            instance.CheckLastExecutedCommand(nameof(TestCommands.CommandToRun), new []{"posArg"});
        }

        [Test]
        public void TestSimpleCommand()
        {
            var commandLine = new CommandLine();
            commandLine.Execute(new[] {"simpleCommand"});
            ((TestCommands)commandLine.LastCommandInstance).CheckLastExecutedCommand(nameof(TestCommands.SimpleCommand));
        }
    }
}