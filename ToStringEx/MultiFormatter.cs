using System;
using System.Collections.Generic;
using System.Linq;

namespace ToStringEx
{
    public abstract class MultiFormatterBase
    {
        public List<IFormatterEx> Formatters { get; }

        public MultiFormatterBase() => Formatters = new List<IFormatterEx>();
        public MultiFormatterBase(IEnumerable<IFormatterEx> formatters)
            => Formatters = new List<IFormatterEx>(formatters);
        public MultiFormatterBase(params IFormatterEx[] formatters)
            : this(formatters.AsEnumerable()) { }
    }

    public class MultiFormatter : MultiFormatterBase, IFormatterEx<object>
    {
        public MultiFormatter() : base() { }
        public MultiFormatter(IEnumerable<IFormatterEx> formatters) : base(formatters) { }
        public MultiFormatter(params IFormatterEx[] formatters) : base(formatters) { }

        public Type TargetType => typeof(object);

        public string Format(object obj)
            => obj.ToStringEx(Formatters);
    }
}
