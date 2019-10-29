using JetBrains.Annotations;

namespace CozyCommandLineParser.Checkers
{
    public static class Ensure
    {
        [ContractAnnotation("val:null=>halt")]
        public static T NotNull<T>(T val, string message = null) where T : class
        {
            if (message == null) message = "Type " + typeof(T);
            if (val == null) throw new CozyCommandLineParser.Checkers.CheckFailedException(message);
            return val;
        }
    }
}