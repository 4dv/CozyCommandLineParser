using System.Reflection;

namespace CozyCommandLineParser.Attributes
{
    public class OptionAttribute : NamedAttribute
    {
        public OptionAttribute(string name = null, string description = null) : base(name, description)
        {
        }

        public override string[] GetNames(MemberInfo mi)
        {
            return names ?? new[] {CommandLine.OPTION_LONG_BEGINNING + mi.Name};
        }
    }
}