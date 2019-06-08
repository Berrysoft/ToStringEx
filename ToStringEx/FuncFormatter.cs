using System;

namespace ToStringEx
{
    public class FuncFormatter<T> : IFormatterEx<T>
    {
        public Func<T, string> Func { get; }

        public Type TargetType => typeof(T);

        public FuncFormatter(Func<T, string> func) => Func = func;

        public string Format(T value)
        {
            if (Func == null)
                return value.ToStringEx();
            else
                return Func(value);
        }

        string IFormatterEx.Format(object value) => Format((T)value);
    }
}
