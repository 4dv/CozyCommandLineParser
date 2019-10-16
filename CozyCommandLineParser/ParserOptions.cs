using CozyCommandLineParser.Utils;

namespace CozyCommandLineParser
{

    public class ParserOptions
    {
        public NameConventions DefaultNameConvention { get; set; } = NameConventions.CamelCase;
        public string DefaultOptionLongPrefix { get; set; } = "--";
    }
}