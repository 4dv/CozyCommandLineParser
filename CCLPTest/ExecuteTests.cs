using System.Threading;
using CozyCommandLineParser;
using NUnit.Framework;

namespace CCLPTest
{
    public class ExecuteTests
    {
        private TestCommands TestCommandLine(string[] args, string expectedExecutedMethod, object[] expectedCommandArgs)
        {
            var commandLine = new CommandLine();
            commandLine.Execute(args);
            var instance = (TestCommands) commandLine.LastCommandInstance;
            instance.CheckLastExecutedCommand(expectedExecutedMethod, expectedCommandArgs);
            return instance;
        }

        [Test]
        public void TestCommandWithOptions()
        {
            TestCommands instance = TestCommandLine(
                new[] {"commandWithArgs", "--stringOption=abcde", "--intOption=42", "-b", "posArg"},
                nameof(TestCommands.SomeCommandWithArgs),
                new object[] {"posArg"});

            Assert.AreEqual("abcde", instance.StringOption);
            Assert.AreEqual(42, instance.IntOption);
            Assert.IsTrue(instance.BoolOption);
        }

        [Test]
        public void TestCommandWithArgs()
        {
            TestCommandLine(new[] {"commandWithArgs", "defg"}, nameof(TestCommands.SomeCommandWithArgs),
                new object[] {"defg", 42});

            TestCommandLine(new[] {"commandWithArgs"}, nameof(TestCommands.SomeCommandWithArgs),
                new object[] {"abc", 42});

            var ex = Assert.Throws<CommandLineException>(() => TestCommandLine(
                new[] {"commandWithArgs", "gaga", "35", "one more"},
                nameof(TestCommands.SomeCommandWithArgs),
                new object[] {"gaga", 35}));
            Assert.AreEqual("Get more arguments, than expected by function, excessive arguments: [one more]",
                ex.Message);

            TestCommandLine(new[] {"commandWithArgs", "gaga", "35"}, nameof(TestCommands.SomeCommandWithArgs),
                new object[] {"gaga", 35});
        }

        [Test]
        public void TestSimpleCommand()
        {
            TestCommandLine(new[] {"simpleCommand"}, nameof(TestCommands.SimpleCommand),
                new object[] { });
        }
    }
}