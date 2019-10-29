using AnotherCCLPTest;
using CozyCommandLineParser;
using CozyCommandLineParser.Checkers;
using NUnit.Framework;

namespace CCLPTest
{
    public class ExecuteTests
    {
        private TestCommands TestCommandLine(string[] args, string expectedExecutedMethod, object[] expectedCommandArgs,
            object result = null)
        {
            var commandLine = new CommandLine();
            Assert.AreEqual(result, commandLine.Execute(args));
            var instance = (TestCommands) commandLine.LastCommandInstance;
            instance.CheckLastExecutedCommand(expectedExecutedMethod, expectedCommandArgs);
            return instance;
        }

        [Test]
        public void TestCommandWithOptions()
        {
            TestCommands instance = TestCommandLine(
                new[] {"commandWithArgs", "--stringOption=abcde", "-b", "--intOption=42", "posArg"},
                nameof(TestCommands.SomeCommandWithArgs),
                new object[] {"posArg", 42});

            Assert.AreEqual("abcde", instance.StringOption);
            Assert.AreEqual(42, instance.IntOption);
            Assert.IsTrue(instance.BoolOption);
            
            var ex = Assert.Throws<CommandLineException>(() => TestCommandLine(
                new[] {"commandWithArgs", "--intOption=23", "--intOption=231"},
                nameof(TestCommands.SomeCommandWithArgs),
                new object[] {"posArg", 42}));

            Assert.AreEqual("Property --intOption was already set with name --intOption",
                ex.Message);

            TestCommandLine(
                new[] {"commandWithArgs", "--intOption=23", "--", "--intOption=231"},
                nameof(TestCommands.SomeCommandWithArgs),
                new object[] {"--intOption=231", 42});
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
                new object[] { }, 10);
        }

        [Test]
        public void TestCommandWithParams()
        {
            TestCommandLine(new[] {"commandWithParams", "22", "a", "b", "c"}, nameof(TestCommands.SomeCommandWithParams),
                new object[] { 22, new[]{"a", "b", "c"}});
        }

        [Test]
        public void TestCommandWithIntParams()
        {
            TestCommandLine(new[] {"commandWithIntParams", "22", "3", "2"}, nameof(TestCommands.CommandWithIntParams),
                new object[] {22, new[] {3, 2}});
        }

        [Test]
        public void TestDerivedCommand()
        {
            var commandLine = new CommandLine();

            commandLine.Execute(new[] {"derivedCommand", "--intInDerived=22", "--anotherInt=12"});
            var instance = Ensure.NotNull(commandLine.LastCommandInstance as AnotherTestCommandsDerived);

            Assert.AreEqual(22, instance.IntInDerived);
            Assert.AreEqual(12, instance.AnotherInt);
        }

        [Test]
        public void TestAnotherSimpleCommand()
        {
            var commandLine = new CommandLine();
            Assert.AreEqual(3.14, commandLine.Execute(new string[]{"anotherCommand"}));
        }
    }
}