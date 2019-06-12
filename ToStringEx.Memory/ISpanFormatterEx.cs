using System;

namespace ToStringEx.Memory
{
    public interface ISpanFormatterEx<T>
    {
        string Format(ReadOnlySpan<T> span);
    }
}
