using System;
using System.Collections.Generic;
using System.Linq;

namespace ToStringEx
{
    public abstract class MultiFormatterBase
    {
        public Dictionary<Type, IFormatterEx> Formatters { get; }

        public MultiFormatterBase() => Formatters = new Dictionary<Type, IFormatterEx>();
        public MultiFormatterBase(IDictionary<Type, IFormatterEx> formatters)
            => Formatters = new Dictionary<Type, IFormatterEx>(formatters);
        public MultiFormatterBase(IEnumerable<IFormatterEx> formatters)
            => Formatters = formatters.ToDictionary(f => f.TargetType);
        public MultiFormatterBase(params IFormatterEx[] formatters)
            : this(formatters.AsEnumerable()) { }

        public void Add(IFormatterEx formatter)
        {
            if (formatter != null)
                Formatters.Add(formatter.TargetType, formatter);
        }

        public void Remove(Type t) => Formatters.Remove(t);
    }

    public class MultiFormatter : MultiFormatterBase, IFormatterEx<object>
    {
        public MultiFormatter() : base() { }
        public MultiFormatter(IDictionary<Type, IFormatterEx> formatters) : base(formatters) { }
        public MultiFormatter(IEnumerable<IFormatterEx> formatters) : base(formatters) { }
        public MultiFormatter(params IFormatterEx[] formatters) : base(formatters) { }

        public Type TargetType => typeof(object);

        public string Format(object obj)
            => obj.ToStringEx(Formatters);
    }
}
