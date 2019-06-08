using System;
using System.Collections.Generic;
using System.Linq;

namespace ToStringEx
{
    public class TupleFormatter : MultiFormatterBase, IFormatterEx<object>
    {
        public TupleFormatter() : base() { }
        public TupleFormatter(IEnumerable<IFormatterEx> formatters) : base(formatters) { }
        public TupleFormatter(params IFormatterEx[] formatters) : base(formatters) { }

        public Type TargetType => typeof(object);

        public string Format(object obj)
            => string.Format(
                    "({0})",
                    string.Join(", ", obj.EnumerateFields(Formatters).EnumerateTupleLike().Concat(obj.EnumerateProperties(Formatters).EnumerateTupleLike())));
    }
}
