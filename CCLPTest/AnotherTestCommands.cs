using CozyCommandLineParser.Attributes;

namespace AnotherCCLPTest
{
    public class AnotherTestCommands
    {
        [Option(Description = "Int argument")]
        public int AnotherInt { get; set; }


        [Command(Description = "Another command description")]
        public double AnotherCommand(int val = 0)
        {
            return val + 3.14;
        }
    }

    public class AnotherTestCommandsDerived : AnotherTestCommands
    {
        [Option]
        public int IntInDerived { get; set; }

        [Command(Description = "Command in derived class")]
        public double DerivedCommand()
        {
            return 321;
        }
    }
}