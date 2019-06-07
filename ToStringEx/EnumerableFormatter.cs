using System.Collections.Generic;
using System.Linq;

namespace ToStringEx
{
    public class EnumerableFormatter<T> : IFormatterEx<IEnumerable<T>>
    {
        public IFormatterEx<T> Formatter { get; }

        public EnumerableFormatter() : this(null) { }
        public EnumerableFormatter(IFormatterEx<T> formatter)
            => Formatter = formatter;

        public string Format(IEnumerable<T> value)
            => string.Format("{{{0}}}", string.Join(", ", value.Select(obj => obj.ToStringEx(Formatter))));
    }
}
