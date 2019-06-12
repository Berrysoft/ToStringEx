using System;

namespace ToStringEx
{
    public class TupleDefaultFormatterProvider : IFormatterProviderEx
    {
        private static readonly TupleDefaultFormatterProvider instance = new TupleDefaultFormatterProvider();

        public static IFormatterProviderEx Instance => instance;

        public bool TryGetProvider(Type t, out IFormatterEx formatter)
        {
            if (t.FullName.StartsWith("System.Tuple`") || t.FullName.StartsWith("System.ValueTuple`"))
            {
                formatter = new TupleFormatter();
                return true;
            }
            else
            {
                formatter = null;
                return false;
            }
        }
    }
}
