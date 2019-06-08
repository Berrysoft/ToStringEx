using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ToStringEx
{
    /// <summary>
    /// Represents a base formatter which formats a set of elements.
    /// </summary>
    /// <typeparam name="T">The type of the elements.</typeparam>
    public abstract class EnumerableFormatterBase<T>
    {
        /// <summary>
        /// The formatter for each element.
        /// </summary>
        public IFormatterEx<T> Formatter { get; }

        /// <summary>
        /// Initializes an instance of <see cref="EnumerableFormatterBase{T}"/>.
        /// </summary>
        public EnumerableFormatterBase() : this(null) { }
        /// <summary>
        /// Initializes an instance of <see cref="EnumerableFormatterBase{T}"/> with a formatter.
        /// </summary>
        /// <param name="formatter">The formatter for each element.</param>
        public EnumerableFormatterBase(IFormatterEx<T> formatter)
            => Formatter = formatter;
    }

    /// <summary>
    /// Represents a formatter for <see cref="IEnumerable"/>.
    /// </summary>
    public class EnumerableFormatter : EnumerableFormatterBase<object>, IFormatterEx<IEnumerable>
    {
        /// <summary>
        /// Initializes an instance of <see cref="EnumerableFormatter"/>.
        /// </summary>
        public EnumerableFormatter() : base() { }
        /// <summary>
        /// Initializes an instance of <see cref="EnumerableFormatter"/> with a formatter.
        /// </summary>
        /// <param name="formatter">The formatter for each element.</param>
        public EnumerableFormatter(IFormatterEx<object> formatter) : base(formatter) { }

        /// <inhertidoc/>
        public Type TargetType => typeof(IEnumerable);

        /// <inhertidoc/>
        public string Format(IEnumerable value)
            => string.Format("{{{0}}}", string.Join(", ", value.Cast<object>().Select(obj => obj.ToStringEx(Formatter))));

        string IFormatterEx.Format(object value) => Format((IEnumerable)value);
    }

    public class EnumerableFormatter<T> : EnumerableFormatterBase<T>, IFormatterEx<IEnumerable<T>>
    {
        /// <summary>
        /// Initializes an instance of <see cref="EnumerableFormatter{T}"/>.
        /// </summary>
        public EnumerableFormatter() : base() { }
        /// <summary>
        /// Initializes an instance of <see cref="EnumerableFormatter{T}"/> with a formatter.
        /// </summary>
        /// <param name="formatter">The formatter for each element.</param>
        public EnumerableFormatter(IFormatterEx<T> formatter) : base(formatter) { }

        /// <inhertidoc/>
        public Type TargetType => typeof(IEnumerable<T>);

        /// <inhertidoc/>
        public string Format(IEnumerable<T> value)
            => string.Format("{{{0}}}", string.Join(", ", value.Select(obj => obj.ToStringEx(Formatter))));

        string IFormatterEx.Format(object value) => Format((IEnumerable<T>)value);
    }
}
