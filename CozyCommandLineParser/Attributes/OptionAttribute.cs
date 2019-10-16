using System.Reflection;

namespace CozyCommandLineParser.Attributes
{
    public class OptionAttribute : NamedAttribute
    {
        public OptionAttribute(string name = null, string description = null) : base(name, description)
        {
        }
    }
}