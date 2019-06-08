using System;
using System.Collections.Generic;
using System.Linq;

namespace ToStringEx
{
    /// <summary>
    /// Represents a formatter for tuple or tuple-like object.
    /// </summary>
    public class TupleFormatter : MultiFormatterBase, IFormatterEx<object>
    {
        /// <summary>
        /// Initializes an instance of <see cref="TupleFormatter"/>.
        /// </summary>
        public TupleFormatter() : base() { }
        /// <summary>
        /// Initializes an instance of <see cref="TupleFormatter"/> with a set of formatters.
        /// </summary>
        /// <param name="formatters">The set of formatters.</param>
        public TupleFormatter(IEnumerable<IFormatterEx> formatters) : base(formatters) { }
        /// <summary>
        /// Initializes an instance of <see cref="TupleFormatter"/> with a set of formatters.
        /// </summary>
        /// <param name="formatters">The set of formatters.</param>
        public TupleFormatter(params IFormatterEx[] formatters) : base(formatters) { }

        /// <inhertidoc/>
        public Type TargetType => typeof(object);

        /// <inhertidoc/>
        public string Format(object obj)
            => string.Format(
                    "({0})",
                    string.Join(", ", obj.EnumerateFields(Formatters).EnumerateTupleLike().Concat(obj.EnumerateProperties(Formatters).EnumerateTupleLike())));
    }
}
