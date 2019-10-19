using System;
using System.Reflection;

namespace CozyCommandLineParser.Helpers
{
    public static class MemberInfoHelpers
    {
        public static bool HasAttribute<T>(this MemberInfo mi) where T : Attribute
        {
            return mi.GetCustomAttribute<T>() != null;
        }
    }
}