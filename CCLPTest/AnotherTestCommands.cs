using CozyCommandLineParser.Attributes;

namespace AnotherCCLPTest
{
    public class AnotherTestCommands
    {
        [Command(Description = "Another command")]
        public double AnotherCommand()
        {
            return 3.14;
        }
    }
}