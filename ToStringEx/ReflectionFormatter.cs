using System;
using System.Collections.Generic;
using System.Linq;

namespace ToStringEx
{
    /// <summary>
    /// Represents a formatter which formats an object with reflection.
    /// </summary>
    public class ReflectionFormatter : MultiFormatterBase, IFormatterEx<object>
    {
        /// <summary>
        /// Initializes an instance of <see cref="ReflectionFormatter"/>.
        /// </summary>
        public ReflectionFormatter() : base() { }
        /// <summary>
        /// Initializes an instance of <see cref="ReflectionFormatter"/> with a set of formatters.
        /// </summary>
        /// <param name="formatters">The set of formatters.</param>
        public ReflectionFormatter(IEnumerable<IFormatterEx> formatters) : base(formatters) { }
        /// <summary>
        /// Initializes an instance of <see cref="ReflectionFormatter"/> with a set of formatters.
        /// </summary>
        /// <param name="formatters">The set of formatters.</param>
        public ReflectionFormatter(params IFormatterEx[] formatters) : base(formatters) { }

        /// <inhertidoc/>
        public Type TargetType => typeof(object);

        /// <inhertidoc/>
        public string Format(object obj)
            => string.Format(
                    "{{ {0} }}",
                    string.Join(", ", obj.EnumerateFields(Formatters).EnumeratePropLike().Concat(obj.EnumerateProperties(Formatters).EnumeratePropLike())));
    }
}
