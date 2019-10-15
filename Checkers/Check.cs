using JetBrains.Annotations;

namespace Checkers
{
    public static class Check
    {
        [ContractAnnotation("val:null=>halt")]
        public static T NotNull<T>(T val, string message = null) where T : class
        {
            if (message == null) message = "Type " + typeof(T);
            if (val == null) throw new CheckFailedException(message);
            return val;
        }
    }
}