using System;
using System.Reflection;
using CozyCommandLineParser.Utils;

namespace CozyCommandLineParser
{
    public class OptionsDictionary
    {
        public OptionsDictionary(Type type)
        {
//            type.GetProperties().Where(pi => ((MemberInfo) pi).GetCustomAttribute<OptionAttribute>() != null).ToDictionary(attr => );
        }

        public void FillProperties(object instance, MultiPassEnumerator<string> argsEnumerator)
        {
            foreach (string arg in argsEnumerator)
            {
                // todo DimaCh don't process anything at the moment
                argsEnumerator.SaveCurrentToNextPass();
            }
        }

        public object[] CreateParameters(MethodInfo methodInfo, MultiPassEnumerator<string> argsEnumerator)
        {
            foreach (var arg in argsEnumerator)
            {
            }
            return new object[]{};
        }
    }
}