using CozyCommandLineParser.Attributes;

namespace AnotherCCLPTest
{
    public class AnotherTestCommands
    {
        [Option]
        public int AnotherInt { get; set; }


        [Command(Description = "Another command")]
        public double AnotherCommand()
        {
            return 3.14;
        }
    }

    public class AnotherTestCommandsDerived : AnotherTestCommands
    {
        [Option]
        public int IntInDerived { get; set; }

        [Command(Description = "Command in derivative")]
        public double DerivedCommand()
        {
            return 321;
        }
    }
}