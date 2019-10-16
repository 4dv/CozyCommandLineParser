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
                // todo DimaCh don't process anything at the moment
                argsEnumerator.SaveCurrentToNextPass();
        }

        public object[] CreateParameters(MethodInfo methodInfo, MultiPassEnumerator<string> argsEnumerator)
        {
            argsEnumerator.Reset();
            object[] res = methodInfo.GetParameters()
                .Select(pi =>
                    argsEnumerator.MoveNext() ? ConvertToType(argsEnumerator.Current, pi.ParameterType) : Type.Missing)
                .ToArray();

            if (argsEnumerator.MoveNext() || !argsEnumerator.IsAllFinished)
            {
                argsEnumerator.SaveCurrentToNextPass();
                CommandLine.Error("Get more arguments, than expected by function, excessive arguments: [" +
                                  string.Join(",", argsEnumerator) + ']');
            }

            return res;
        }

        private object ConvertToType(string str, Type type)
        {
            return Convert.ChangeType(str, type, ParserOptions.Culture);
        }
    }
}