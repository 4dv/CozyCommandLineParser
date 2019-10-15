using System;
using CozyCommandLineParser;
using CozyCommandLineParser.Attributes;
using NUnit.Framework;

namespace CCLPTest
{
    public class Commands
    {
        [Option("--stringOption|-s", "some option with string value")]
        public string StringOption { get; set; }

        [Option(Description = "Int option will use default name --intOption")]
        public int IntOption { get; set; }

        [Option("-b")] public bool BoolOption { get; set; }

        [Command("command", "Description of the command")]
        public int CommandToRun(string posArg)
        {
            OnCommandExectuted(this, new object[] {posArg});
            return 10;
        }

        [Command(Description = "Simple command to run")]
        public int SimpleCommand()
        {
            OnCommandExectuted(this, null);
            return 10;
        }

        public static Action<Commands, object[]> OnCommandExectuted;

        // if command arguments doesn't have any attributes, they work as positional arguments
        [Command("commandWithArgs", "This command has its own arguments and doesn't return anything'")]
        public void AnotherCommandToRun(int arg1, string arg2)
        {
            OnCommandExectuted(this, new object[] {arg1, arg2});
        }
    }

    [TestFixture]
    public class Tests
    {
        [Test]
        public void TestSimpleCommand()
        {
            CommandLine commandLine = new CommandLine();
            bool executed = false;
            Commands.OnCommandExectuted = (instance, moreArgs) => { executed = true; };

            commandLine.Execute(new[] {"simpleCommand"});
            Assert.IsTrue(executed);

        }

        [Test]
        public void Test1()
        {
            CommandLine commandLine = new CommandLine();
            Commands.OnCommandExectuted = (instance, moreArgs) =>
            {
                Assert.AreEqual("abcde", instance.StringOption);
                Assert.AreEqual(42, instance.IntOption);
                Assert.IsTrue(instance.BoolOption);
                Assert.AreEqual(1, moreArgs.Length);
            };

            commandLine.Execute(new[] {"command", "--stringOption='abcde'", "--intOption=42", "-b", "posArg"});
        }
    }
}