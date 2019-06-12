using System;

namespace ToStringEx.Memory
{
    /// <summary>
    /// Exposes a set of members to format a <see cref="ReadOnlySpan{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of each element.</typeparam>
    public interface ISpanFormatterEx<T>
    {
        /// <summary>
        /// Formats the span.
        /// </summary>
        /// <param name="span">The span to be formatted.</param>
        /// <returns>A humanized string.</returns>
        string Format(ReadOnlySpan<T> span);
    }
}
