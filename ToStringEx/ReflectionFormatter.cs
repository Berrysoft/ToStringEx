using System;
using System.Collections.Generic;
using System.Linq;

namespace ToStringEx
{
    public class ReflectionFormatter : MultiFormatterBase, IFormatterEx<object>
    {
        public ReflectionFormatter() : base() { }
        public ReflectionFormatter(IEnumerable<IFormatterEx> formatters) : base(formatters) { }
        public ReflectionFormatter(params IFormatterEx[] formatters) : base(formatters) { }

        public Type TargetType => typeof(object);

        public string Format(object obj)
            => string.Format(
                    "{{ {0} }}",
                    string.Join(", ", obj.EnumerateFields(Formatters).EnumeratePropLike().Concat(obj.EnumerateProperties(Formatters).EnumeratePropLike())));
    }
}
