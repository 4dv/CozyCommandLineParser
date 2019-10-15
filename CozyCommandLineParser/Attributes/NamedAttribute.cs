using System;
using System.Reflection;
using CozyCommandLineParser.Utils;

namespace CozyCommandLineParser.Attributes
{
    public class NamedAttribute : Attribute
    {
        public string Description { get; set; }

        protected string[] names;

        public NamedAttribute(string name = null, string description = null)
        {
            this.names = name?.Split('|');
            this.Description = description;
        }

        public virtual string[] GetNames(MemberInfo mi)
        {
            return names ?? new[]
                       {LetterCaseConverter.FromGenericCase(mi.Name, CommandLine.Options.DefaultNameConversion)};
        }
    }
}