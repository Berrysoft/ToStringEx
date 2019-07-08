using System;
using System.Collections;
using System.Collections.Generic;

namespace ToStringEx
{
    /// <summary>
    /// Represents a default formatter provider for <see cref="string"/>, <see cref="Array"/> and <see cref="IEnumerable"/>.
    /// </summary>
    public class EnumerableDefaultFormatterProvider : IFormatterProviderEx
    {
        private EnumerableDefaultFormatterProvider() { }

        private static readonly EnumerableDefaultFormatterProvider instance = new EnumerableDefaultFormatterProvider();

        /// <summary>
        /// Gets the singleton instance of the provider.
        /// </summary>
        public static IFormatterProviderEx Instance => instance;

        /// <inhertidoc/>
        public bool TryGetProvider(Type t, out IFormatterEx formatter)
        {
            if (t.IsArray)
            {
                if (t.GetArrayRank() == 1 && t.GetElementType() == typeof(char))
                {
                    formatter = new CharEnumerableFormatter();
                }
                else
                {
                    formatter = new ArrayFormatter();
                }
                return true;
            }
            else if (typeof(IEnumerable).IsAssignableFrom(t))
            {
                if (typeof(IEnumerable<char>).IsAssignableFrom(t))
                {
                    formatter = new CharEnumerableFormatter();
                }
                else
                {
                    formatter = new EnumerableFormatter();
                }
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
