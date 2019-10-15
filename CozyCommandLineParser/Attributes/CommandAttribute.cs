namespace CozyCommandLineParser.Attributes
{
    public class CommandAttribute : NamedAttribute
    {
        public CommandAttribute(string name = null, string description = null) : base(name, description)
        {
        }

        public bool IsDefault { get; set; }
    }
}