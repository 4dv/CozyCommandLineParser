using System;
using System.Reflection;

namespace CozyCommandLineParser.Helpers
{
    public static class MemberInfoHelpers
    {
/*
        public static string[] GetNames(this MemberInfo mi)
        {
            var attr = mi.GetCustomAttribute<NamedAttribute>();
            Check.NotNull(attr, "attr");

            return attr.GetNames(mi);
        }
*/

        public static bool HasAttribute<T>(this MemberInfo mi) where T : Attribute
        {
            return mi.GetCustomAttribute<T>() != null;
        }
    }
}