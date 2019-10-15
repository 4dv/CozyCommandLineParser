using CozyCommandLineParser.Utils;

namespace CozyCommandLineParser
{

    public class ParserOptions
    {
        public NameConventions DefaultNameConversion { get; set; } = NameConventions.CamelCase;

//        public bool IgnoreCase { get; set; } = true;
    }
}