using System;
using System.Linq;
using System.Reflection;
using CozyCommandLineParser.Helpers;
using CozyCommandLineParser.Utils;

namespace CozyCommandLineParser
{
    public class OptionsDictionary
    {
        public OptionsDictionary(Type type, NamesReader namesReader)
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
            argsEnumerator.Reset();
            var res = methodInfo.GetParameters()
                .Select(pi => argsEnumerator.MoveNext() ? argsEnumerator.Current : Type.Missing).ToArray();
            // todo check that we don't have excessive arguments
//            if (!argsEnumerator.MoveNext())
//                CommandLine.Error("Get more arguments, than expected by function, excessive arguments: " + string.Join(",", argsEnumerator));
            return res;
        }
    }
}