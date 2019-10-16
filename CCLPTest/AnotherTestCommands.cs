using CozyCommandLineParser.Attributes;

namespace AnotherCCLPTest
{
    public class AnotherTestCommands
    {
        [Command(Description = "Another command")]
        public int AnotherCommand()
        {
            return 22;
        }
    }
}