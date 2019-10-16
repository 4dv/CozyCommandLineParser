using System;
using System.Runtime.CompilerServices;
using CozyCommandLineParser.Attributes;
using NUnit.Framework;

namespace CCLPTest
{
    public class TestCommands
    {
        private int lastCalledCount;

        [Option("--stringOption|-s", "some option with string value")]
        public string StringOption { get; set; }

        [Option(Description = "Int option will use default name --intOption")]
        public int IntOption { get; set; }

        [Option("-b")] public bool BoolOption { get; set; }

        private string lastExecutedCommand;
        private object[] lastFunctionArgs;

        [Command("commandWithArg", "Description of the command")]
        public int CommandToRun(string posArg)
        {
            CommandExecuted(new object[] {posArg});
            return 10;
        }

        [Command(Description = "Simple command to run")]
        public int SimpleCommand()
        {
            CommandExecuted();
            return 10;
        }

        public void CheckLastExecutedCommand(string expectedCommand, object[] expectedOptions = null)
        {
            Assert.AreEqual(expectedCommand, lastExecutedCommand);
            Assert.AreEqual(1, lastCalledCount);
            Assert.AreEqual(expectedOptions, lastFunctionArgs);
        }

        // if command arguments doesn't have any attributes, they work as positional arguments
        [Command("commandWithArgs", "This command has one string argument'")]
        public void SomeCommandWithArgs(string arg = "abc", int arg2 = 42)
        {
            CommandExecuted(new object[] {arg, arg2});
        }


        private void CommandExecuted(object[] args = null, [CallerMemberName] string callerName = "")
        {
            lastExecutedCommand = callerName;
            lastCalledCount++;
            lastFunctionArgs = args;
        }
    }
}