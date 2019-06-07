using System;

namespace ToStringEx
{
    public class FuncFormatter<T> : IFormatterEx<T>
    {
        public Func<T, string> Func { get; }

        public FuncFormatter(Func<T, string> func) => Func = func;

        public string Format(T value)
        {
            if (Func == null)
                return value.ToStringEx();
            else
                return Func(value);
        }
    }
}
