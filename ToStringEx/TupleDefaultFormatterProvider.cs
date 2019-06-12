using System;

namespace ToStringEx
{
    /// <summary>
    /// Represents a default tuple formatter provider.
    /// </summary>
    public class TupleDefaultFormatterProvider : IFormatterProviderEx
    {
        private static readonly TupleDefaultFormatterProvider instance = new TupleDefaultFormatterProvider();

        /// <summary>
        /// Gets the singleton instance of the provider.
        /// </summary>
        public static IFormatterProviderEx Instance => instance;

        /// <inhertidoc/>
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
