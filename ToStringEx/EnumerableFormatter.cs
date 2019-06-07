using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ToStringEx
{
    public abstract class EnumerableFormatterBase<T>
    {
        public IFormatterEx<T> Formatter { get; }

        public EnumerableFormatterBase() : this(null) { }
        public EnumerableFormatterBase(IFormatterEx<T> formatter)
            => Formatter = formatter;
    }

    public class EnumerableFormatter : EnumerableFormatterBase<object>, IFormatterEx<IEnumerable>
    {
        public EnumerableFormatter() : base() { }
        public EnumerableFormatter(IFormatterEx<object> formatter) : base(formatter) { }

        public string Format(IEnumerable value)
            => string.Format("{{{0}}}", string.Join(", ", value.Cast<object>().Select(obj => obj.ToStringEx(Formatter))));
    }

    public class EnumerableFormatter<T> : EnumerableFormatterBase<T>, IFormatterEx<IEnumerable<T>>
    {
        public EnumerableFormatter() : base() { }
        public EnumerableFormatter(IFormatterEx<T> formatter) : base(formatter) { }

        public string Format(IEnumerable<T> value)
            => string.Format("{{{0}}}", string.Join(", ", value.Select(obj => obj.ToStringEx(Formatter))));
    }
}
