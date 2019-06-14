using System;
using System.Collections.Generic;
using System.Linq;

namespace ToStringEx
{
    /// <summary>
    /// Represents the base class of a multi-formatter.
    /// </summary>
    public abstract class MultiFormatterBase
    {
        /// <summary>
        /// The set of formatters.
        /// </summary>
        public List<IFormatterEx> Formatters { get; }

        /// <summary>
        /// Initializes an instance of <see cref="MultiFormatterBase"/>.
        /// </summary>
        public MultiFormatterBase() => Formatters = new List<IFormatterEx>();
        /// <summary>
        /// Initializes an instance of <see cref="MultiFormatterBase"/> with a set of formatters.
        /// </summary>
        /// <param name="formatters">The set of formatters.</param>
        public MultiFormatterBase(IEnumerable<IFormatterEx> formatters)
            => Formatters = new List<IFormatterEx>(formatters);
    }

    /// <summary>
    /// Repesents a multi-formatter.
    /// </summary>
    public class MultiFormatter : MultiFormatterBase, IFormatterEx<object>
    {
        /// <summary>
        /// Initializes an instance of <see cref="MultiFormatter"/>.
        /// </summary>
        public MultiFormatter() : base() { }
        /// <summary>
        /// Initializes an instance of <see cref="MultiFormatter"/> with a set of formatters.
        /// </summary>
        /// <param name="formatters">The set of formatters.</param>
        public MultiFormatter(IEnumerable<IFormatterEx> formatters) : base(formatters) { }
        /// <summary>
        /// Initializes an instance of <see cref="MultiFormatter"/> with a set of formatters.
        /// </summary>
        /// <param name="formatters">The set of formatters.</param>
        public MultiFormatter(params IFormatterEx[] formatters) : base(formatters) { }

        /// <inhertidoc/>
        public Type TargetType => typeof(object);

        /// <inhertidoc/>
        public string Format(object obj) => obj.ToStringEx(Formatters);
    }
}
