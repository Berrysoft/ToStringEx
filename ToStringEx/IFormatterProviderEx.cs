using System;

namespace ToStringEx
{
    /// <summary>
    /// Exposes a set of members for a formatter provider.
    /// </summary>
    public interface IFormatterProviderEx
    {
        /// <summary>
        /// Get a formatter from the type.
        /// </summary>
        /// <param name="t">The type of the object.</param>
        /// <param name="formatter">A formatter if succeeds; otherwise, <see langword="null"/>.</param>
        /// <returns><see langword="true"/> if succeeds; otherwise, <see langword="false"/>.</returns>
        bool TryGetProvider(Type t, out IFormatterEx formatter);
    }
}
