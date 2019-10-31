using System;

namespace CozyCommandLineParser.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter)]
    public class OptionAttribute : NamedAttribute
    {
        public OptionAttribute(string name = null, string description = null) : base(name, description)
        {
        }
    }
}