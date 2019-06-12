using System;

namespace ToStringEx
{
    public interface IFormatterProviderEx
    {
        bool TryGetProvider(Type t, out IFormatterEx formatter);
    }
}
