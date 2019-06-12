using System;

namespace ToStringEx.Memory
{
    public class MemoryDefaultFormatterProvider : IFormatterProviderEx
    {
        private static readonly MemoryDefaultFormatterProvider instance = new MemoryDefaultFormatterProvider();

        public static IFormatterProviderEx Instance => instance;

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
