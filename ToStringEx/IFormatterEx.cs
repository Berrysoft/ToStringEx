using System;

namespace ToStringEx
{
    public interface IFormatterEx
    {
        Type TargetType { get; }
        string Format(object value);
    }

    public interface IFormatterEx<in T> : IFormatterEx
    {
        string Format(T value);
    }
}
