using System;

namespace ToStringEx.Memory
{
    /// <summary>
    /// Represents a default <see cref="Memory{T}"/> and <see cref="ReadOnlyMemory{T}"/> formatter provider.
    /// </summary>
    public class MemoryDefaultFormatterProvider : IFormatterProviderEx
    {
        private static readonly MemoryDefaultFormatterProvider instance = new MemoryDefaultFormatterProvider();

        /// <summary>
        /// Gets the singleton instance of the provider.
        /// </summary>
        public static IFormatterProviderEx Instance => instance;

        /// <inhertidoc/>
        public bool TryGetProvider(Type t, out IFormatterEx formatter)
        {
            if (t.FullName.StartsWith("System.Memory`1") || t.FullName.StartsWith("System.ReadOnlyMemory`1"))
            {
                Type[] types = t.GenericTypeArguments;
                formatter = (IFormatterEx)Activator.CreateInstance(Type.GetType("ToStringEx.Memory.MemoryFormatter`1").MakeGenericType(types));
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
