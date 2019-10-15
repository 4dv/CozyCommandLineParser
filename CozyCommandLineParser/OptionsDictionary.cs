using System;
using System.Reflection;

namespace CozyCommandLineParser
{
    public class OptionsDictionary
    {
        public OptionsDictionary(Type type)
        {
//            type.GetProperties().Where(pi => ((MemberInfo) pi).GetCustomAttribute<OptionAttribute>() != null).ToDictionary(attr => );
        }

        public void FillProperties(object instance)
        {
            
        }

        public object[] CreateParameters(MethodInfo methodInfo)
        {
            return new object[]{};
        }
    }
}